namespace Stack.Core.CompressedArray;

public interface ICompressedArray<T> : IDisposable, IEnumerable<T>
{
    T this[long index] { get; set; }
}