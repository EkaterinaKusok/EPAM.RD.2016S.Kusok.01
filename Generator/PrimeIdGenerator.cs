using UserStorage.Interfacies.Generators;

namespace Generator
{
    public class PrimeIdGenerator : IGenerator<int>
    {
        private readonly PrimeSequence sequence = new PrimeSequence();

        public int GenerateNewId()
        {
            int result = sequence.Current;
            sequence.MoveNext();
            return result;
        }

        public int GenerateNewId(int currentId)
        {
            sequence.Current = currentId;
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
}
