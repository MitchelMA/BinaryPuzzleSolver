namespace Solver.Collections;

public readonly struct ScatteredMemory<T>
{
    private readonly object _refArray;
    private readonly int[] _indices;

    public int Length => _indices.Length;

    public ScatteredSpan<T> Span =>
        new ScatteredSpan<T>((T[])_refArray, _indices);

    public ScatteredMemory(T[] array, int[] indices)
    {
        BoundsCheck(array, indices);
        
        _refArray = array;
        _indices = indices;
    }

    public ScatteredMemory(T[] array, Range range)
    {
        _refArray = array;

        var (offset, length) = range.GetOffsetAndLength(array.Length);
        _indices = Enumerable.Range(offset, length).ToArray();
    }

    private ScatteredMemory(T pinnedReference, int[] indices)
    {
        _refArray = pinnedReference!;
        _indices = indices;
    }

    private static void BoundsCheck(Array array, int[] indices)
    {
        if (indices.Length > array.Length && !indices.Any(i => i >= array.Length))
            throw new ArgumentException($"{nameof(indices)} may not be larger than {nameof(array)}");
    }
    
    public ScatteredMemory<T> Slice(int start)
    {
        return Slice(start..);
    }
    
    public ScatteredMemory<T> Slice(int start, int end)
    {
        return Slice(start..end);
    }

    private ScatteredMemory<T> Slice(Range range)
    {
        return new ScatteredMemory<T>((T)_refArray, _indices[range]);
    }
}