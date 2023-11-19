namespace DataCompression.Data;

/// <summary>
/// Расширения для преобразования типов
/// </summary>
public static class DataExtensions
{
    public static byte[] ToByteArray(this int value)
    {
        return BitConverter.GetBytes(value);
    }
    
    public static int ToInt(this byte[] input, int startIndex)
    {
        return BitConverter.ToInt32(input, startIndex);
    }
}