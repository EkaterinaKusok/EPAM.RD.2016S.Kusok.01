using UserStorage.Interfacies.Generators;

namespace Generator
{
    /// <summary>
    /// Provides methods for generating prime numbers.
    /// </summary>
    public class PrimeIdGenerator : IGenerator<int>
    {
        private readonly PrimeSequence sequence = new PrimeSequence();

        #region IGenerator methods

        /// <summary>
        /// Generates the new identifier of type T.
        /// </summary>
        /// <returns>
        /// The new identifier of type T.
        /// </returns>
        public int GenerateNewId()
        {
            int result = sequence.Current;
            sequence.MoveNext();
            return result;
        }

        /// <summary>
        /// Generates the new identifier of type T.
        /// </summary>
        /// <param name="currentId">The current identifier.</param>
        /// <returns>
        /// The new identifier of type T.
        /// </returns>
        public int GenerateNewId(int currentId)
        {
            sequence.Current = currentId;
            int result = sequence.Current;
            sequence.MoveNext();
            return result;
        }

        /// <summary>
        /// Sets the current value of identifier.
        /// </summary>
        /// <param name="currentId">The current identifier.</param>
        /// <returns>
        /// True if function work correctly; otherwise - false.
        /// </returns>
        public bool SetCurrentId(int currentId)
        {
            sequence.Current = currentId;
            return true;
        }

        /// <summary>
        /// Gets the current identifier.
        /// </summary>
        /// <returns>
        /// The current identifier of type T.
        /// </returns>
        public int GetCurrentId()
        {
            return sequence.Current;
        }
        #endregion
    }
}
