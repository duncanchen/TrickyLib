using ExpassistLib.Error;
using ExpassistLib.Sort;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TrickyLib.Sort
{
    public class Heap<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        public Comparison<T> ItemComparison { get; set; }
        public Directions Direction { get; set; }

        // Fields
        private T[] _array;
        private const int _defaultCapacity = 4;
        private static T[] _emptyArray;
        private int _size;
        [NonSerialized]
        private object _syncRoot;
        private int _version;

        // Methods
        static Heap()
        {
            Heap<T>._emptyArray = new T[0];
        }

        public Heap(Directions direction, Comparison<T> comparison)
        {
            this._array = Heap<T>._emptyArray;
            this._size = 0;
            this._version = 0;
            this.Direction = direction;
            this.ItemComparison = comparison;
        }
        public Heap(Directions direction)
            : this(direction, Comparer<T>.Default.Compare)
        { }

        public Heap(IEnumerable<T> collection, Directions direction, Comparison<T> comparison)
        {
            if (collection == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            }
            ICollection<T> is2 = collection as ICollection<T>;
            if (is2 != null)
            {
                int count = is2.Count;
                this._array = new T[count];
                is2.CopyTo(this._array, 0);
                this._size = count;
            }
            else
            {
                this._size = 0;
                this._array = new T[4];
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        this.Push(enumerator.Current);
                    }
                }
            }
            this.Direction = direction;
            this.ItemComparison = comparison;
        }
        public Heap(IEnumerable<T> collection, Directions direction)
            : this(collection, direction, Comparer<T>.Default.Compare)
        { }

        public Heap(int capacity, Directions direction, Comparison<T> comparison)
        {
            if (capacity < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
            }
            this._array = new T[capacity];
            this._size = 0;
            this._version = 0;
            this.Direction = direction;
            this.ItemComparison = comparison;
        }
        public Heap(int capacity, Directions direction)
            : this(capacity, direction, Comparer<T>.Default.Compare)
        { }

        public void Clear()
        {
            Array.Clear(this._array, 0, this._size);
            this._size = 0;
            this._version++;
        }

        public bool Contains(T item)
        {
            int index = this._size;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            while (index-- > 0)
            {
                if (item == null)
                {
                    if (this._array[index] == null)
                    {
                        return true;
                    }
                }
                else if ((this._array[index] != null) && comparer.Equals(this._array[index], item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }
            if ((arrayIndex < 0) || (arrayIndex > array.Length))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
            if ((array.Length - arrayIndex) < this._size)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
            }
            Array.Copy(this._array, 0, array, arrayIndex, this._size);
            Array.Reverse(array, arrayIndex, this._size);
        }

        public Enumerator<T> GetEnumerator()
        {
            return new Enumerator<T>((Heap<T>)this);
        }

        public T Peek()
        {
            if (this._size == 0)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyHeap);
            }
            return this._array[0];
        }

        public T Pop()
        {
            if (this._size == 0)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyHeap);
            }
            this._version++;
            this._size--;
            T local = this._array[0];
            this._array[0] = this._array[this._size];
            this._array[this._size] = default(T);
            SiftDown(0);
            return local;
        }

        public void Push(T item)
        {
            if (this._size == this._array.Length)
            {
                T[] destinationArray = new T[(this._array.Length == 0) ? 4 : (2 * this._array.Length)];
                Array.Copy(this._array, 0, destinationArray, 0, this._size);
                this._array = destinationArray;
            }
            this._array[this._size++] = item;
            this._version++;
            SiftUp(this._size - 1);
        }

        void SiftDown(int pos)
        {
            int targetChild = (pos + 1) << 1;
            if (targetChild == this._size)
                targetChild--;

            while (targetChild < this._size)
            {
                if (targetChild % 2 == 0 && this.ItemComparison(this._array[targetChild - 1], this._array[targetChild]) * (int)this.Direction > 0)
                    targetChild--;

                if (this.ItemComparison(this._array[targetChild], this._array[pos]) * (int)this.Direction > 0)
                {
                    T temp = this._array[targetChild];
                    this._array[targetChild] = this._array[pos];
                    this._array[pos] = temp;
                    pos = targetChild;
                    targetChild = (pos + 1) << 1;
                    if (targetChild == this._size)
                        targetChild--;
                }
                else
                    break;
            }
        }

        void SiftUp(int pos)
        {
            while (pos > 0)
            {
                int parent = pos >> 1;
                if (this.ItemComparison(this._array[pos], this._array[parent]) * (int)this.Direction > 0)
                {
                    T temp = this._array[parent];
                    this._array[parent] = this._array[pos];
                    this._array[pos] = temp;
                    pos = parent;
                }
                else
                    break;
            }
        }

        public bool Exists(Predicate<T> match)
        {
            if (this._array == null || this._array.Length == 0)
                return false;

            foreach (var obj in this._array)
                if (match(obj))
                    return true;

            return false;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator<T>((Heap<T>)this);
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }
            if (array.Rank != 1)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }
            if (array.GetLowerBound(0) != 0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            }
            if ((arrayIndex < 0) || (arrayIndex > array.Length))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
            if ((array.Length - arrayIndex) < this._size)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
            }
            try
            {
                Array.Copy(this._array, 0, array, arrayIndex, this._size);
                Array.Reverse(array, arrayIndex, this._size);
            }
            catch (ArrayTypeMismatchException)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator<T>((Heap<T>)this);
        }

        public T[] ToArray()
        {
            T[] localArray = new T[this._size];
            for (int i = 0; i < this._size; i++)
            {
                localArray[i] = this._array[(this._size - i) - 1];
            }
            return localArray;
        }

        public void TrimExcess()
        {
            int num = (int)(this._array.Length * 0.9);
            if (this._size < num)
            {
                T[] destinationArray = new T[this._size];
                Array.Copy(this._array, 0, destinationArray, 0, this._size);
                this._array = destinationArray;
                this._version++;
            }
        }

        // Properties
        public int Count
        {
            get
            {
                return this._size;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        // Nested Types
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Enumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
        {
            private Heap<T> _Heap;
            private int _index;
            private int _version;
            private T currentElement;
            internal Enumerator(Heap<T> Heap)
            {
                this._Heap = Heap;
                this._version = this._Heap._version;
                this._index = -2;
                this.currentElement = default(T);
            }

            public void Dispose()
            {
                this._index = -1;
            }

            public bool MoveNext()
            {
                bool flag;
                if (this._version != this._Heap._version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                if (this._index == -2)
                {
                    this._index = this._Heap._size - 1;
                    flag = this._index >= 0;
                    if (flag)
                    {
                        this.currentElement = this._Heap._array[this._index];
                    }
                    return flag;
                }
                if (this._index == -1)
                {
                    return false;
                }
                flag = --this._index >= 0;
                if (flag)
                {
                    this.currentElement = this._Heap._array[this._index];
                    return flag;
                }
                this.currentElement = default(T);
                return flag;
            }

            public T Current
            {
                get
                {
                    if (this._index == -2)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }
                    if (this._index == -1)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }
                    return this.currentElement;
                }
            }

            object IEnumerator.Current
            {

                get
                {
                    if (this._index == -2)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }
                    if (this._index == -1)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }
                    return this.currentElement;
                }
            }

            void IEnumerator.Reset()
            {
                if (this._version != this._Heap._version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                this._index = -2;
                this.currentElement = default(T);
            }
        }
    }
}
