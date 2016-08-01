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

        public ServiceMode Mode => ServiceMode.Slave;

        public int Add(User user)
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot write to storage.");
        }

        public void Delete(int personalId)
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\tremoving attempt; access denied.");
            throw new NotSupportedException("Slave cannot delete from storage.");
        }

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

        public void ListenForUpdates()
        {
            this.receiver.StartReceivingMessages();
        }
        
        public void Save()
        {
            this.receiver.StopReceiver();
            this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\taddition attempt; access denied.");
            throw new NotSupportedException("Slave cannot save state.");
        }

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
