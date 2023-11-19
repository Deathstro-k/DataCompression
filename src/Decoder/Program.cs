using DataCompression.Algorithm.BurrowsWheelerTransform;
using DataCompression.Algorithm.HuffmanCode;
using DataCompression.Algorithm.MoveToFront;
using DataCompression.Algorithm.RunLenghtEncoding;

namespace DataCompression.Decoder;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            var zipFile = args[0];
            var decFile = args[1];
            if (File.Exists(zipFile))
            {
                var encodedBytes = DecryptBytes(zipFile);
                File.WriteAllBytes(decFile, encodedBytes);
            }
            else
            {
                Console.WriteLine("Входной файл не существует.");
            }
        }
        else
        {
            Console.WriteLine("Неверный синтаксис. Используйте: decoder (zipfile) (decfile)");
        }
        Console.WriteLine("Работа программы завершена");
        Console.ReadLine();
    }
    
    private static byte[] DecryptBytes(string fileName)
    {
        var encodedBytes = File.ReadAllBytes(fileName);
       
        byte callsRle = 0;
        byte callsHuffman = 0;
        
        var decrypt = encodedBytes
            .DecryptRepeatHuffman(ref callsHuffman, ref callsRle)
            .UseMoveToFrontReverseTransform()
            .UseBurrowsWheelerReverseTransform()
            .DecryptRleIfImprovement(callsRle);
        
        return decrypt;
    }
}