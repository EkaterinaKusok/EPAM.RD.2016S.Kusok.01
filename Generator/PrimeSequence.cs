using System;
using System.Collections;
using System.Collections.Generic;

namespace Generator
{
    public class PrimeSequence : IEnumerator<int>
    {
        private int current;
        public int Current
        {
            get { return current; }
            set
            {
                current = value;
                if (!this.IsPrime(current))
                    this.MoveNext();
            }
        }

        public PrimeSequence()
        {
            current = 1;
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
                        current++;
                    }
                }
                catch (OverflowException ex)
                {
                    //this.Reset();
                    throw new OverflowException("", ex);
                }
            } while (!IsPrime(Current));
            return true;
        }

        public void Reset()
        {
            current = 1;
        }

        object IEnumerator.Current => Current;

        private bool IsPrime(int value)
        {
            if (value < 1)
            {
                throw new ArgumentException();
            }
            for (int i = 2; i <= Math.Sqrt(value); i++)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
