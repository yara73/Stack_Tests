using System.Collections;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization;

namespace Stack.Core.CompressedArray;

public class CompressedArray<T> : ICompressedArray<T>
{
    private readonly MemoryMappedFile _memoryMappedFile;
    private readonly string _indexDirectoryPath;
    private readonly DataContractSerializer _serializer = new(typeof(T));
    private readonly ReaderWriterLockSlim _lockSlim = new();
    private long _currentEndPosition;
    private const int IndexBlockSize = 100;

    public CompressedArray(string filePath, string indexDirectoryPath, long initialSize = 1024 * 1024 * 10)
    {
        _indexDirectoryPath = indexDirectoryPath;

        var fileStream = new FileStream(
            filePath,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);

        _memoryMappedFile = MemoryMappedFile.CreateFromFile(
            fileStream,
            null,
            initialSize,
            MemoryMappedFileAccess.ReadWrite,
            HandleInheritability.None,
            leaveOpen: false);
    }

    public T this[long index]
    {
        get => Read(index);
        set => Write(index, value);
    }

    private T Read(long index)
    {
        _lockSlim.EnterReadLock();
        try
        {
            var fileInfo = FindIndexFile(index);
            if (!fileInfo.HasValue)
                throw new IndexOutOfRangeException($"DataBlock[{index}] cannot be found.");

            using var accessor = _memoryMappedFile.CreateViewAccessor(fileInfo.Value.position, fileInfo.Value.length);
            var data = new byte[fileInfo.Value.length];
            accessor.ReadArray(0, data, 0, data.Length);
            using var ms = new MemoryStream(data);
            using var decompressionStream = new GZipStream(ms, CompressionMode.Decompress);
            return (T) _serializer.ReadObject(decompressionStream)!;
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    private void Write(long index, T value)
    {
        _lockSlim.EnterWriteLock();
        try
        {
            using var ms = new MemoryStream();
            using var compressionStream = new GZipStream(ms, CompressionMode.Compress);
            _serializer.WriteObject(compressionStream, value);
            compressionStream.Close();
            var data = ms.ToArray();
            long dataLength = data.Length;

            using (var accessor = _memoryMappedFile.CreateViewAccessor(_currentEndPosition, dataLength))
            {
                accessor.WriteArray(0, data, 0, data.Length);
            }

            UpdateIndexMap(index, _currentEndPosition, dataLength);
            _currentEndPosition += dataLength;
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    private void UpdateIndexMap(long index, long position, long length)
    {
        var indexBlock = LoadIndexBlock(index);
        indexBlock[index] = (position, length);
        SaveIndexBlock(index, indexBlock);
    }

    private Dictionary<long, (long position, long length)> LoadIndexBlock(long index)
    {
        var blockId = index / IndexBlockSize;
        var blockFilePath = Path.Combine(_indexDirectoryPath, $"indexBlock_{blockId}.bin");

        if (!File.Exists(blockFilePath)) return new Dictionary<long, (long position, long length)>();
        using var fs = new FileStream(blockFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var serializer = new DataContractSerializer(typeof(Dictionary<long, (long position, long length)>));
        return (Dictionary<long, (long position, long length)>) serializer.ReadObject(fs)!;
    }

    private void SaveIndexBlock(long index, Dictionary<long, (long position, long length)> block)
    {
        var blockId = index / IndexBlockSize;
        var blockFilePath = Path.Combine(_indexDirectoryPath, $"indexBlock_{blockId}.bin");

        using var fs = new FileStream(blockFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        var serializer = new DataContractSerializer(typeof(Dictionary<long, (long position, long length)>));
        serializer.WriteObject(fs, block);
    }

    private (long position, long length)? FindIndexFile(long index)
    {
        var indexBlock = LoadIndexBlock(index);
        if (indexBlock.TryGetValue(index, out var info))
        {
            return info;
        }

        return null;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (long i = 0; i < _currentEndPosition; i++)
        {
            yield return Read(i);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _memoryMappedFile?.Dispose();
        _lockSlim?.Dispose();
    }

    ~CompressedArray()
    {
        Dispose(disposing: false);
    }
}