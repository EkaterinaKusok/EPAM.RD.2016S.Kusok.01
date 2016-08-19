using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UserStorage.Interfacies.Creators;
using UserStorage.Interfacies.Generators;
using UserStorage.Interfacies.Logger;
using UserStorage.Interfacies.Network;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.Services;
using UserStorage.Interfacies.StateSavers;
using UserStorage.Interfacies.Storages;
using UserStorage.Interfacies.UserEntities;
using UserStorage.Interfacies.Validators;

namespace UserStorage.Service
{
    /// <summary>
    /// Implements functionality for working with users as slave process.
    /// </summary>
    /// <seealso cref="System.MarshalByRefObject" />
    /// <seealso cref="UserStorage.Interfacies.Services.IService" />
    /// <seealso cref="UserStorage.Interfacies.Services.IListener" />
    [Serializable]
    public class SlaveService : MarshalByRefObject, IService, IListener
    {
        private readonly IUserStorage userStorage;
        private readonly ILogger logger;
        private readonly IReceiver receiver;
        private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Initializes a new instance of the <see cref="SlaveService"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <exception cref="ArgumentNullException">{nameof(creator)}</exception>
        /// <exception cref="InvalidOperationException">
        /// Unable to create {nameof(this.userStorage)}.
        /// or
        /// Unable to create {nameof(this.logger)}.
        /// or
        /// Unable to create {nameof(this.receiver)}.
        /// </exception>
        public SlaveService(IDependencyCreator creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException($"{nameof(creator)} must be not null.");
            }

            // var validator = creator.CreateInstance<IUserValidator>();
            var saver = creator.CreateInstance<IStateSaver>();
            var generator = creator.CreateInstance<IGenerator<int>>();
            this.userStorage = creator.CreateInstance<IUserStorage>(generator, null, saver);
            if (this.userStorage == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(this.userStorage)}.");
            }

            this.logger = creator.CreateInstance<ILogger>();
            if (this.logger == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(this.logger)}.");
            }

            this.receiver = creator.CreateInstance<IReceiver>();
            if (this.receiver == null)
            {
                this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\t{nameof(this.receiver)} is null.");
                throw new InvalidOperationException($"Unable to create {nameof(this.receiver)}.");
            }

            this.receiver.Updating += this.UpdateOnModifying;
            this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tslave service created.");
        }

        /// <summary>
        /// Gets the service mode.
        /// </summary>
        public ServiceMode Mode => ServiceMode.Slave;

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">User instance.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Slave cannot write to storage.</exception>
        public int Add(User user)
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot write to storage.");
        }

        /// <summary>
        /// Deletes the specified personal identifier.
        /// </summary>
        /// <param name="personalId">The personal identifier.</param>
        /// <exception cref="NotSupportedException">Slave cannot delete from storage.</exception>
        public void Delete(int personalId)
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\tremoving attempt; access denied.");
            throw new NotSupportedException("Slave cannot delete from storage.");
        }

        /// <summary>
        /// Performs a search for user using specified predicates.
        /// </summary>
        /// <param name="predicates">Criterias for search.</param>
        /// <returns></returns>
        public IList<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            this.readerWriterLock.EnterReadLock();
            try
            {
                var foundUsers = this.userStorage.SearchForUser(predicates);
                this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tusers search ({foundUsers.Count()} found).");
                return foundUsers;
            }
            finally
            {
                this.readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Listens for updates.
        /// </summary>
        public void ListenForUpdates()
        {
            this.receiver.StartReceivingMessages();
        }

        /// <summary>
        /// Saves storage state.
        /// </summary>
        /// <exception cref="NotSupportedException">Slave cannot save state.</exception>
        public void Save()
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot save state.");
        }

        /// <summary>
        /// Loads storage state.
        /// </summary>
        /// <exception cref="NotSupportedException">Slave cannot load state.</exception>
        public void Load()
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot load state.");
        }

        private void UpdateOnModifying(object sender, UserEventArgs eventArgs)
        {
            this.readerWriterLock.EnterWriteLock();
            try
            {
                switch (eventArgs.Operation)
                {
                    case ServiceOperation.Add:
                        this.userStorage.Add(eventArgs.User);
                        this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\treceived user added.");
                        break;
                    case ServiceOperation.Remove:
                        this.userStorage.Delete(eventArgs.User.Id);
                        this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\treceived user removed.");
                        break;
                }
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }
    }
}
