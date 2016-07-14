using System;
using System.Collections;
using System.Collections.Generic;
using UserStorage.Interfacies;

namespace UserStorage
{
    public class CustomIdGenerator : IGenerator<int>
    {
        private readonly Sequence sequence = new Sequence();

        public int GenerateNewId()
        {
            int result = sequence.Current;
            sequence.MoveNext();
            return result;
        }

        public int GenerateNewId(int prev)
        {
            sequence.Current = prev;
            sequence.MoveNext();
            int result = sequence.Current;
            sequence.MoveNext();
            return result;
        }

        public bool SetCurrentId(int currentId)
        {
            sequence.Current = currentId;
            return true;
        }

        public int GetCurrentId()
        {
            return sequence.Current;
        }
    }

    public class Sequence : IEnumerator<int>
    {
        public int Current { get; set; }

        public Sequence()
        {
            Current = 1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            do
            {
                try
                {
                    checked
                    {
                        Current++;
                    }
                }
                catch (OverflowException ex)
                {
                    //this.Reset();
                    throw new OverflowException("", ex);
                }
            } while (!IsSimple(Current));
            return true;
        }

        public void Reset()
        {
            Current = 1;
        }

        object IEnumerator.Current => Current;

        private bool IsSimple(int value)
        {
            if (value < 1)
            {
                throw new ArgumentException();
            }
            for (int i = 2; i <= Math.Sqrt(value); i++)
            {
                if (value%i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
