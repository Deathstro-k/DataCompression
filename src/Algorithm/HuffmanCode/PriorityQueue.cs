namespace DataCompression.Algorithm.HuffmanCode;

internal class PriorityQueue<T>
{
    private readonly SortedDictionary<int, Queue<T>> _storage = new();

    /// <summary>
    /// Размер очереди
    /// </summary>
    public int Size { get; private set; } = 0;

    /// <summary>
    /// Поставить в очередь
    /// </summary>
    /// <param name="priority">Приоритет</param>
    /// <param name="item">Элемент</param>
    public void Enqueue(int priority, T item)
    {
        if (!_storage.ContainsKey(priority))
        {
            _storage.Add(priority, new Queue<T>());
        }

        _storage[priority].Enqueue(item);
        Size++;
    }
    
    /// <summary>
    /// Вывести из очереди
    /// </summary>
    public T Dequeue()
    {
        if (Size == 0) throw new Exception("Очередь пустая");
        Size--;
        
        foreach (var element in _storage.Values.Where(element => element.Count > 0))
        {
            return element.Dequeue();
        }

        throw new Exception();
    }
}