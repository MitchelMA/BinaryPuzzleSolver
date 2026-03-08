using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Solver.Collections;

public readonly ref struct ScatteredSpan<T>
{
    private readonly ref T _refArray;
    private readonly int _refLength;
    private readonly int[] _indices;

    public int Length => _indices.Length;

    public ScatteredSpan(T[] array, int[] indices)
    {
        _refArray = ref MemoryMarshal.GetArrayDataReference(array);
        _refLength = array.Length;
        _indices = indices;
    }

    public ScatteredSpan(T[] array, Range range)
    {
        _refArray = ref MemoryMarshal.GetArrayDataReference(array);
        _refLength = array.Length;
        
        var bound = range.GetOffsetAndLength(array.Length);
        _indices = Enumerable.Range(bound.Offset, bound.Length).ToArray();
    }

    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((uint)index >= (uint)Length || (uint)_indices[index] >= (uint)_refLength)
                throw new IndexOutOfRangeException(nameof(index));
            
            return ref Unsafe.Add(ref _refArray, (nint)(uint)_indices[index]);
        }
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