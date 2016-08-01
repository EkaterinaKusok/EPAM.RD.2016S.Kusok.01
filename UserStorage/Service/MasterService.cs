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
    public class MasterService : MarshalByRefObject, IService
    {
        private readonly IUserStorage userStorage;

        private readonly ISender sender;

        private readonly ILogger logger;

        private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        public MasterService(IDependencyCreator creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException($"{nameof(creator)} must be not null.");
            }

            var validator = creator.CreateInstance<IUserValidator>();
            var saver = creator.CreateInstance<IStateSaver>();
            var generator = creator.CreateInstance<IGenerator<int>>();
            this.userStorage = creator.CreateInstance<IUserStorage>(generator, validator, saver);

            if (this.userStorage == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(this.userStorage)}.");
            }

            this.logger = creator.CreateInstance<ILogger>();
            if (this.logger == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(this.logger)}.");
            }

            this.sender = creator.CreateInstance<ISender>();
            if (this.sender == null)
            {
                this.logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\t{nameof(this.sender)} is null.");
                throw new InvalidOperationException($"Unable to create {nameof(this.sender)}.");
            }

            this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tmaster service created.");
        }

        public ServiceMode Mode => ServiceMode.Master;

        public int Add(User user)
        {
            this.readerWriterLock.EnterWriteLock();
            try
            {
                int id = this.userStorage.Add(user);
                user.Id = id;
                this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tuser added (id: {id}).");
                this.sender.SendMessage(new ServiceMessage(user, ServiceOperation.Add));
                return id;
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }

        public void Delete(int id)
        {
            this.readerWriterLock.EnterWriteLock();
            try
            {
                var users = this.userStorage.SearchForUser(u => u.Id == id);
                this.userStorage.Delete(id);
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tuser removed (id: {user.Id}).");
                        this.sender.SendMessage(new ServiceMessage(user, ServiceOperation.Remove));
                    }
                }
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
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

        public void Load()
        {
            this.readerWriterLock.EnterWriteLock();
            try
            {
                this.userStorage.Load();
                var users = this.userStorage.SearchForUser(u => true).ToList();
                foreach (var user in users)
                {
                    this.sender.SendMessage(new ServiceMessage(user, ServiceOperation.Add));
                }

                this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tservice state loaded.");
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }

        public void Save()
        {
            this.readerWriterLock.EnterWriteLock();
            try
            {
                this.userStorage.Save();
                this.logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tservice state saved.");
            }
            finally
            {
                this.readerWriterLock.ExitWriteLock();
            }
        }
    }
}