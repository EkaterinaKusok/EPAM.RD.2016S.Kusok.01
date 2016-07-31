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
    [Serializable]
    public class SlaveService : MarshalByRefObject, IService, IListener
    {
        private readonly IUserStorage userStorage;
        private readonly ILogger logger;
        private readonly IReceiver receiver;
        private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();
        public ServiceMode Mode => ServiceMode.Slave;

        public SlaveService(IDependencyCreator creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException($"{nameof(creator)} must be not null.");
            }

            var validator = creator.CreateInstance<IUserValidator>();
            var saver = creator.CreateInstance<IStateSaver>();
            var generator = creator.CreateInstance<IGenerator<int> >();
            userStorage = creator.CreateInstance<IUserStorage>(generator, validator, saver);
            if (userStorage == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(userStorage)}.");
            }

            logger = creator.CreateInstance<ILogger>();
            if (logger == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(logger)}.");
            }
            
            receiver = creator.CreateInstance<IReceiver>();
            if (receiver == null)
            {
                logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\t{nameof(receiver)} is null.");
                throw new InvalidOperationException($"Unable to create {nameof(receiver)}.");
            }

            receiver.Updating += UpdateOnModifying;
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tslave service created.");
        }
        

        public int Add(User user)
        {
            receiver.StopReceiver();
            logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot write to storage.");
        }

        public void Delete(int personalId)
        {
            receiver.StopReceiver();
            logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\tremoving attempt; access denied.");
            throw new NotSupportedException("Slave cannot delete from storage.");
        }

        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            readerWriterLock.EnterReadLock();
            try
            {
                var foundUsers = this.userStorage.SearchForUser(predicates);
                logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tusers search ({foundUsers.Count()} found).");
                return foundUsers;
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        public void ListenForUpdates()
        {
            receiver.StartReceivingMessages();
        }

        private void UpdateOnModifying(object sender, UserEventArgs eventArgs)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                switch (eventArgs.Operation)
                {
                    case ServiceOperation.Add:
                        userStorage.Add(eventArgs.User);
                        logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\treceived user added.");
                        break;
                    case ServiceOperation.Remove:
                        userStorage.Delete(eventArgs.User.Id);
                        logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\treceived user removed.");
                        break;
                }
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Save()
        {
            receiver.StopReceiver();
            logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot save state.");
        }

        public void Load()
        {
            receiver.StopReceiver();
            logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot load state.");
        }
    }
}
