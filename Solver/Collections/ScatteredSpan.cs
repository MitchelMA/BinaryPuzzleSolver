using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Solver.Collections;

public readonly ref struct ScatteredSpan<T>
{
    private readonly ref T _refArray;
    private readonly int[] _indices;

    public int Length => _indices.Length;

    public ScatteredSpan(T[] array, int[] indices)
    {
        BoundsCheck(array, indices);
        
        _refArray = ref MemoryMarshal.GetArrayDataReference(array);
        _indices = indices;
    }

    public ScatteredSpan(T[] array, Range range)
    {
        _refArray = ref MemoryMarshal.GetArrayDataReference(array);
        
        var (offset, length) = range.GetOffsetAndLength(array.Length);
        _indices = Enumerable.Range(offset, length).ToArray();
    }

    private ScatteredSpan(T pinnedStart, int[] indices)
    {
        _refArray = pinnedStart;
        _indices = indices;
    }
    
    private static void BoundsCheck(Array array, int[] indices)
    {
        if (indices.Length > array.Length && !indices.Any(i => i >= array.Length))
            throw new ArgumentException($"{nameof(indices)} may not be larger than {nameof(array)}");
    }

    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((uint)index >= (uint)Length)
                throw new IndexOutOfRangeException(nameof(index));
            
            return ref Unsafe.Add(ref _refArray, (nint)(uint)_indices[index]);
        }
    }

    public ScatteredSpan<T> Slice(int start)
    {
        return Slice(start..);
    }

    public ScatteredSpan<T> Slice(int start, int end)
    {
        return Slice(start..end);
    }

    private ScatteredSpan<T> Slice(Range range)
    {
        return new ScatteredSpan<T>(_refArray, _indices[range]);
    }

    public Enumerator GetEnumerator() => new (this);

    public ref struct Enumerator
    {
        private readonly ScatteredSpan<T> _arr;

        private int _index;
        
        internal Enumerator(ScatteredSpan<T> span)
        {
            _arr = span;
            _index = -1;
        }

        public bool MoveNext()
        {
            var num = _index + 1;
            if (num >= _arr.Length)
                return false;

            _index = num;
            return true;
        }

        public ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _arr[_index];
        }
    }
}