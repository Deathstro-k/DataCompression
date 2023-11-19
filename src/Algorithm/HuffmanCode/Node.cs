namespace DataCompression.Algorithm.HuffmanCode;

/// <summary>
/// Узёл дерева Хаффмана
/// </summary>
internal sealed class Node(int frequency, Node left, Node right)
{
    /// <summary>
    /// Код 
    /// </summary>
    public byte Code { get; set; }

    /// <summary>
    /// Частота
    /// </summary>
    public int Frequency => frequency;

    /// <summary>
    /// Левый потомок(0)
    /// </summary>
    public Node Left => left;

    /// <summary>
    /// Правый потомок(1)
    /// </summary>
    public Node Right => right;

    public Node(byte code, int frequency) : this(frequency, left: null!, right: null!)
    {
        Code = code;
    }
}