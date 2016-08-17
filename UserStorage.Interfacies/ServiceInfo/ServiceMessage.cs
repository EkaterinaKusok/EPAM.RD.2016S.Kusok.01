using System;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.ServiceInfo
{
    /// <summary>
    /// Contains information about change in user storage.
    /// </summary>
    [Serializable]
    public class ServiceMessage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMessage"/> class.
        /// </summary>
        public ServiceMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMessage"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="operation">The service operation.</param>
        public ServiceMessage(User user, ServiceOperation operation)
        {
            User = user;
            Operation = operation;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the User instance.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user was changed.
        /// </summary>
        public ServiceOperation Operation { get; set; }
        #endregion
    }
}
