namespace Stack.Core.Stack;

public class MemoryStack<T> : IStack<T> where T : struct
{
    private T[] _items;
    private int _count;
    private readonly float _resizeFactor;

    public MemoryStack(int initialCapacity = 256, float resizeFactor = 1.5f)
    {
        _resizeFactor = resizeFactor;
        _items = new T[initialCapacity];
    }

    public void Push(T item)
    {
        if (_count == _items.Length)
        {
            Resize();
        }
        _items[_count++] = item;
    }

    private void Resize()
    {
        var newSize = (int)(_items.Length * _resizeFactor);
        var newItems = new T[newSize];
        Array.Copy(_items, newItems, _count);
        _items = newItems;
    }

    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Stack is empty");
        var item = _items[--_count];
        return item;
    }
    
    public void Print()
    {
        Console.WriteLine("Current Stack Contents:");
        for (var i = _count - 1; i >= 0; i--)
        {
            Console.WriteLine(_items[i].ToString());
        }
    }

    public bool IsEmpty() => _count == 0;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
