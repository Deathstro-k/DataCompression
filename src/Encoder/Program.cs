using DataCompression.Algorithm.BurrowsWheelerTransform;
using DataCompression.Algorithm.HuffmanCode;
using DataCompression.Algorithm.MoveToFront;
using DataCompression.Algorithm.RunLenghtEncoding;

namespace DataCompression.Encoder;

internal static class Program 
{
    public static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            var inputFileName = args[0];
            var outputFileName = args[1];
            if (File.Exists(inputFileName))
            {
                var encodedBytes = EncodeBytes(inputFileName);
                File.WriteAllBytes(outputFileName, encodedBytes);
            }
            else
            {
                Console.WriteLine("Входной файл не существует.");
            }
        }
        else
        {
            Console.WriteLine("Неверный синтаксис. Используйте: coder (infile) (zipfile)");
        }

        Console.WriteLine("Работа программы завершена");
        
        Console.ReadLine();
    }
    
    private static byte[] EncodeBytes(string fileName)
    {
        var inputFileBytes = File.ReadAllBytes(fileName);
       
        byte callsRle = 0;
        byte callsHuffman = 1;
        
        var encodedBody = inputFileBytes
            .UseRleIfImprovement(ref callsRle)
            .UseBurrowsWheelerTransform()
            .UseMoveToFrontTransform()
            .EncodeHuffman()
            .RepeatHuffmanIfImprovement(ref callsHuffman);
        
        var tail = new[] { callsRle, callsHuffman };
        var encoded = encodedBody.Concat(tail).ToArray();
        Console.WriteLine($"Размер исходного файла: {inputFileBytes.Length}");
        Console.WriteLine($"Размер закодированного файла: {encoded.Length}");
        return encoded;
    }
}