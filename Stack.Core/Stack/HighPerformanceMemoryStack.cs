using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Stack.Core.Stack;

public unsafe class HighPerformanceMemoryStack<T> : IStack<T> where T : unmanaged
{
    private T* _items;
    private int _capacity;
    private int _count;

    public HighPerformanceMemoryStack(int initialCapacity = 256)
    {
        _capacity = initialCapacity;
        _items = (T*)NativeMemory.Alloc((nuint)(_capacity * sizeof(T)));
        _count = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public void Push(T item)
    {
        if (_count >= _capacity)
        {
            Resize();
        }
        _items[_count++] = item;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private void Resize()
    {
        int newCapacity = _capacity * 2; 
        T* newItems = (T*)NativeMemory.Alloc((nuint)(newCapacity * sizeof(T)));
        Buffer.MemoryCopy(_items, newItems, newCapacity * sizeof(T), _capacity * sizeof(T));
        NativeMemory.Free(_items);
        _items = newItems;
        _capacity = newCapacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public T Pop()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        return _items[--_count];
    }

    public void Print()
    {
        Console.WriteLine("Current Stack Contents:");
        for (int i = _count - 1; i >= 0; i--)
        {
            Console.WriteLine(_items[i].ToString());
        }
    }

    public bool IsEmpty() => _count == 0;

    public void Dispose()
    {
        if (_items == null) return;
        NativeMemory.Free(_items);
        _items = null;
    }
}