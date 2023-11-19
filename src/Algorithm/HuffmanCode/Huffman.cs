using DataCompression.Data;

namespace DataCompression.Algorithm.HuffmanCode;

public static class Huffman
{
    public static byte[] RepeatHuffmanIfImprovement(this byte[] encoded, ref byte callsHuffman)
    {
       var reEncoded = encoded.EncodeHuffman();
        
        if (reEncoded.Length < encoded.Length)
        {
            callsHuffman++;
            return RepeatHuffmanIfImprovement(reEncoded, ref callsHuffman);
        } 
        return encoded;
    }
    
    public static byte[] DecryptRepeatHuffman(this byte[] encoded, ref byte callsHuffman, ref byte callsRle)
    {
        var decrypt = encoded[..^2];
        callsHuffman = encoded[^1];
        callsRle = encoded[^2];
        
        for (var i = 0; i < callsHuffman; i++)
        {
            decrypt = decrypt.DecryptHuffman();
        }

        return decrypt;
    }
    
    public static byte[] EncodeHuffman(this byte[] originalArray)
    {
        var frequencies = CalculateFrequencies(originalArray);

        var tree = CreateTree(frequencies);

        var codes = CreateCode(tree);

        var encodedBits = CorrelateCodes(originalArray, codes);

        var header = CreateHeader(originalArray.Length, frequencies);

        return header.Concat(encodedBits).ToArray();
    }

    public static byte[] DecryptHuffman(this byte[] encodedArray)
    {
        ReadHeader(encodedArray, out var lenght, out var startIndex, out var frequencies);
        
        var tree = CreateTree(frequencies);

        var decodedArray = TreeTraversal(encodedArray, startIndex, lenght, tree);
        return decodedArray;
    }

    private static byte[] TreeTraversal(byte[] encodedArray, int startIndex, int lenght, Node tree)
    {
        var currentNode = tree;
        var size = 0;
        var data = new List<byte>();

        for (var i = startIndex; i < encodedArray.Length; i++)
        {
            for (var bit = 1; bit <= 128; bit <<= 1)
            {
                var zero = (encodedArray[i] & bit) == 0;

                currentNode = zero ? currentNode.Left : currentNode.Right;

                if (currentNode.Left is not null) continue;

                if (size++ < lenght)
                {
                    data.Add(currentNode.Code);
                }
                currentNode = tree;
            }
        }
        return data.ToArray();
    }

    private static void ReadHeader(byte[] encodedArray, out int lenght, out int startIndex, out int[] frequencies)
    {
        lenght = encodedArray[..4].ToInt(0);

        frequencies = new int[256];

        for (var i = 0; i < 256; i++)
        {
            frequencies[i] = encodedArray[4 + i];
        }

        startIndex = 4 + 256;
    }

    private static byte[] CreateHeader(int originalArrayLength, int[] frequencies)
    {
        var header = new List<byte>();
        header.AddRange(originalArrayLength.ToByteArray());

        header.AddRange(frequencies.Select(frequency => (byte)frequency));

        return header.ToArray();
    }

    private static Node CreateTree(int[] frequencies)
    {
        var priorityQueue = new PriorityQueue<Node>();

        for (var i = 0; i < 256; i++)
        {
            if (frequencies[i] > 0)
            {
                priorityQueue.Enqueue(frequencies[i], new Node((byte)i, frequencies[i]));
            }
        }

        while (priorityQueue.Size > 1)
        {
            var left = priorityQueue.Dequeue();
            var right = priorityQueue.Dequeue();

            var frequency = left.Frequency + right.Frequency;

            var next = new Node(frequency, left, right);
            priorityQueue.Enqueue(frequency, next);
        }

        return priorityQueue.Dequeue();
    }

    private static string[] CreateCode(Node haffmanTree)
    {
        var codes = new string[256];

        ExtendToChild(haffmanTree, code: string.Empty);

        return codes;

        void ExtendToChild(Node node, string code)
        {
            if (node.Left == null)
            {
                codes[node.Code] = code;
            }
            else
            {
                ExtendToChild(node.Left, $"{code}0");
                ExtendToChild(node.Right, $"{code}1");
            }
        }
    }

    private static byte[] CorrelateCodes(byte[] array, string[] codes)
    {
        var encodedBytes = new List<byte>();
        byte sum = 0;
        byte b = 1;

        foreach (var symbol in array)
        {
            if (codes[symbol] != null)
            {
                foreach (var code in codes[symbol])
                {
                    if (code == '1') sum |= b;

                    if (b < 128) b <<= 1;
                    else
                    {
                        encodedBytes.Add(sum);
                        sum = 0;
                        b = 1;
                    }
                }
            }
        }

        if (b > 1) encodedBytes.Add(sum);
        
        return encodedBytes.ToArray();
    }

    private static int[] CalculateFrequencies(byte[] array)
    {
        var frequencies = new int[256];
        foreach (var symbol in array)
        {
            frequencies[symbol]++;
        }

        Normalize();

        return frequencies;

        void Normalize()
        {
            var max = frequencies.Max();
            
            if(max <= 255) return;
            
            for (var i = 0; i < 256; i++)
            {
                if (frequencies[i] > 0)
                {
                    frequencies[i] = 1 + frequencies[i] * 255 / (max + 1);
                }
            }
        }
    }
}