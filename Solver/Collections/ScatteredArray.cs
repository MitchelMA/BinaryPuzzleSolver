using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Solver.Collections;

public readonly ref struct ScatteredArray<T>
{
    private readonly ref T _refArray;
    private readonly int[] _indices;

    public int Length => _indices.Length;

    public ScatteredArray(T[] array, int[] indices)
    {
        _refArray = ref MemoryMarshal.GetArrayDataReference(array);
        _indices = indices;
    }

    public ScatteredArray(T[] array, Range range)
    {
        _refArray = ref MemoryMarshal.GetArrayDataReference(array);
        var bound = range.GetOffsetAndLength(array.Length);
        _indices = Enumerable.Range(bound.Offset, bound.Length).ToArray();
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

    public Enumerator GetEnumerator() => new (this);

    public ref struct Enumerator
    {
        private readonly ScatteredArray<T> _arr;

        private int _index;
        
        internal Enumerator(ScatteredArray<T> array)
        {
            _arr = array;
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