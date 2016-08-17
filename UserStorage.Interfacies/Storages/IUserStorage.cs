using System;
using System.Collections.Generic;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.Storages
{
    /// <summary>
    /// Represents common functionality for accessing user storage.
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">User instance.</param>
        /// <returns></returns>
        int Add(User user);

        /// <summary>
        /// Performs a search for user using specified predicates.
        /// </summary>
        /// <param name="predicates">Criterias for search.</param>
        /// <returns></returns>
        IList<User> SearchForUser(params Func<User, bool>[] predicates);

        /// <summary>
        /// Deletes user from storage.
        /// </summary>
        /// <param name="id">User identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Saves storage state.
        /// </summary>
        void Save();

        /// <summary>
        /// Loads storage state.
        /// </summary>
        void Load();
    }
}
