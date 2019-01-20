namespace ReversedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ReversedList<T> : IEnumerable<T>
    {
        private const int DefaultListLength = 2;

        private T[] reversedArr;     

        public ReversedList()
        {
            this.reversedArr = new T[DefaultListLength];
        }

        public int Count { get; private set; }

        public T this[int index]
        {
            get
            {
                
                return this.reversedArr[this.reversedArr.Count() - index];
            }
            set
            {
                this.reversedArr[index] = value;
            }
        }

        public void Add(T item)
        {
            if(this.Count == this.reversedArr.Length)
            {
                ResizeArr();
            }
            this.reversedArr[Count++] = item;
        }

        public T RemoveAt(int index)
        {
            if(index < 0 || index >= this.reversedArr.Length)
            {
                throw new IndexOutOfRangeException();
            }

            T element = this.reversedArr[index];

            for (int i = this.reversedArr.Length - index - 1; i < this.Count; i++)
            {
                this.reversedArr[i] = this.reversedArr[i + 1];
            }

            return element;
        }

        private void ResizeArr()
        {
            T[] newArr = new T[reversedArr.Length * 2];
            Array.Copy(reversedArr, newArr, reversedArr.Length);
            reversedArr = newArr;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                yield return this.reversedArr[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
