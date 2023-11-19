namespace DataCompression.Algorithm.BurrowsWheelerTransform;

public static class BurrowsWheeler
{
    public static byte[] UseBurrowsWheelerTransform(this byte[] originalArray)
    {
        var inputLength = originalArray.Length;
        var outputArray = new byte[inputLength + 4];
            
        var modifiedArray = AddOffset(originalArray);

        var suffixArray = SuffixArray.New(modifiedArray);

        var end = FindEndIndex(suffixArray);
        int outputIndex = 0;
        for (int i = 0; i < suffixArray.Length; i++)
        {
            if (suffixArray[i] == 0)
            {
                end = i;
                continue;
            }

            outputArray[outputIndex] = (byte)(modifiedArray[suffixArray[i] - 1] - 1);
            outputIndex++;
        }

        byte[] endBytes = BitConverter.GetBytes(end);
        Array.Copy(endBytes, 0, outputArray, inputLength, 4);

        return outputArray;
    }
        
    public static byte[] UseBurrowsWheelerReverseTransform(this byte[] transformedArray)
    {
        int length = transformedArray.Length - 4;
        int index = BitConverter.ToInt32(transformedArray, length);

        int[] frequencies = new int[256];
        int[] firstOccurrence = new int[length + 1];
        int[] cumulativeFrequencies = new int[256];
        CalculateFrequenciesAndFirstOccurrence(transformedArray, length, frequencies, firstOccurrence);
        CalculateCumulativeFrequencies(frequencies, cumulativeFrequencies);

        var outputArray = new byte[length];
        var nextIndex = 0;
        for (var i = length - 1; i >= 0; i--)
        {
            outputArray[i] = transformedArray[nextIndex];
            var a = firstOccurrence[nextIndex];
            var b = cumulativeFrequencies[transformedArray[nextIndex]];
            nextIndex = a + b;
            if (nextIndex >= index)
            {
                nextIndex--;
            }
        }

        return outputArray;
    }
        
    private static int[] AddOffset(IReadOnlyList<byte> inputArray)
    {
        var modifiedArray = new int[inputArray.Count + 1];
        for (var i = 0; i < inputArray.Count; i++)
        {
            modifiedArray[i] = inputArray[i] + 1;
        }

        modifiedArray[inputArray.Count] = 0;
        return modifiedArray;
    }

    private static int FindEndIndex(int[] suffixArray)
    {
        for (int i = 0; i < suffixArray.Length; i++)
        {
            if (suffixArray[i] == 0)
            {
                return i;
            }
        }
        return 0;
    }

    private static void CalculateFrequenciesAndFirstOccurrence(byte[] inputArray, int length, int[] frequencies, int[] firstOccurrence)
    {
        for (int i = 0; i < length; i++)
        {
            firstOccurrence[i] = frequencies[inputArray[i]];
            frequencies[inputArray[i]]++;
        }
    }

    private static void CalculateCumulativeFrequencies(int[] frequencies, int[] cumulativeFrequencies)
    {
        cumulativeFrequencies[0] = 1;
        for (var i = 1; i < 256; i++)
        {
            cumulativeFrequencies[i] = cumulativeFrequencies[i - 1] + frequencies[i - 1];
        }
    }
}