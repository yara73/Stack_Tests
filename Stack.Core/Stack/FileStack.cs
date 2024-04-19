using Stack.Core.CompressedArray;

namespace Stack.Core.Stack;

public class FileStack<T> : IStack<T> where T : unmanaged
{
    private readonly ICompressedArray<T> _stackData;
    private int _stackPointer = -1;

    public FileStack(string path)
    {
        var indexDirectoryPath = Path.Combine(Path.GetDirectoryName(path) ?? throw new InvalidOperationException(), "IndexFiles");
        Directory.CreateDirectory(indexDirectoryPath);
        _stackData = new CompressedArray<T>(path, indexDirectoryPath);
    }

    public void Push(T item)
    {
        _stackPointer++;
        _stackData[_stackPointer] = item;
    }

    public T Pop()
    {
        if (_stackPointer < 0)
            throw new InvalidOperationException("Pop from an empty stack.");

        var item = _stackData[_stackPointer];
        _stackPointer--;
        return item;
    }

    public void Print()
    {
        Console.WriteLine("Current Stack Contents:");
        for (int i = _stackPointer; i >= 0; i--)
        {
            Console.WriteLine(_stackData[i].ToString());
        }
    }
    
    public bool IsEmpty()
    {
        return _stackPointer == -1;
    }

    public void Dispose()
    {
        _stackData.Dispose();
        GC.SuppressFinalize(this);
    }
}
