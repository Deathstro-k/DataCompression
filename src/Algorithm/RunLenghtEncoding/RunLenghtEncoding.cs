namespace DataCompression.Algorithm.RunLenghtEncoding;

/// <summary>
/// RLE
/// </summary>
public static class RunLenghtEncoding
{
    public static byte[] UseRleIfImprovement(this byte[] encoded, ref byte callsRle)
    {
        var reEncoded = encoded[..^2].EncodeRunLenght();

        if (reEncoded.Length < encoded.Length)
        {
            callsRle++;
            return reEncoded;
        }
        return encoded;
    }
    
    public static byte[] DecryptRleIfImprovement(this byte[] encoded, byte callsRle)
    {
        var decrypt = encoded;
        
        if (callsRle > 0)
        {
             decrypt = decrypt.DecryptRunLenght();
        }
        
        return decrypt;
    }
    
    public static byte[] EncodeRunLenght(this byte[] originalArray)
    {
        var output = new List<byte>();

        byte count = 1;
        var i = 0;
        for (; i < originalArray.Length; i++)
        {
            while (i < originalArray.Length - 1 && originalArray[i] == originalArray[i + 1] && count < 255)
            {
                count++;
                i++;
            }
            output.Add(count);
            output.Add(originalArray[i]);
            count = 1;
        }

        return output.ToArray();
    }

    public static byte[] DecryptRunLenght(this byte[] encodedArray)
    {
        var output = new List<byte>();

        for (var i = 0; i < encodedArray.Length; i += 2)
        {
            var count = encodedArray[i];
            var value = encodedArray[i + 1];
            for (var j = 0; j < count; j++)
            {
                output.Add(value);
            }
        }

        return output.ToArray();
    }
}