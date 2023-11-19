namespace DataCompression.Algorithm.MoveToFront;

public static class MoveToFront
{
    public static byte[] UseMoveToFrontTransform(this byte[] input)
    {
        var sequence = new byte[256];
        
        for (var i = 0; i < 256; i++)
        {
            sequence[i] = (byte)i;
        }
    
        var result = new byte[input.Length];
    
        for(var j = 0; j < input.Length; j++)
        {
            var b = input[j];
            for (var i = 0; i < 256; i++)
            {
                if (sequence[i] != b) continue;
                result[j] = (byte)i;
                var temp = sequence[i];
                for (var k = i; k > 0; k--)
                {
                    sequence[k] = sequence[k-1];
                }
                sequence[0] = temp;
                break;
            }
        }
        return result;
    }
    
    public static byte[] UseMoveToFrontReverseTransform(this byte[] input)
    {
        var sequence = new byte[256];
        for (var i = 0; i < 256; i++)
        {
            sequence[i] = (byte)i;
        }
    
        var result = new byte[input.Length];
    
        for(var j = 0; j < input.Length; j++)
        {
            var index = input[j];
            var value = sequence[index];
            result[j] = value;
            for (int i = index; i > 0; i--)
            {
                sequence[i] = sequence[i-1];
            }
            sequence[0] = value;
        }
        return result;
    }
}