namespace Stack.Core.Stack;

public interface IStack<T> : IDisposable
{
    void Push(T item);
    T Pop();
    void Print();
    bool IsEmpty();
}