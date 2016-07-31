using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
        public ServiceMode Mode => ServiceMode.Master;
        private readonly IEnumerable<ConnectionInfo> slavesInfo;
        private readonly ISender sender;


        //private readonly IGenerator<int> idGenerator;
        //private readonly IStateSaver loader;
        private readonly ILogger logger;
        //private readonly IUserValidator validator;
        private readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

       public MasterService(IDependencyCreator creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException($"{nameof(creator)} must be not null.");
            }

           var validator = creator.CreateInstance<IUserValidator>();
           var saver = creator.CreateInstance<IStateSaver>();
           var generator = creator.CreateInstance<IGenerator<int> >();
            userStorage = creator.CreateInstance<IUserStorage>(generator,validator,saver);

            if (userStorage == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(userStorage)}.");
            }

            logger = creator.CreateInstance<ILogger>();
            if (logger == null)
            {
                throw new InvalidOperationException($"Unable to create {nameof(logger)}.");
            }

            sender = creator.CreateInstance<ISender>();
            if (sender == null)
            {
                logger.Log(TraceEventType.Error, $"{AppDomain.CurrentDomain.FriendlyName}:\t{nameof(sender)} is null.");
                throw new InvalidOperationException($"Unable to create {nameof(sender)}.");
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tmaster service created.");
        }

        public IList<User> Users { get; private set; }

        public int Add(User user)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                int id = this.userStorage.Add(user);
                user.Id = id;
                logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tuser added (id: {id}).");
                sender.SendMessage(new ServiceMessage(user, ServiceOperation.Add));
                return id;
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Delete(int id)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                var users = userStorage.SearchForUser(u => u.Id == id);
                this.userStorage.Delete(id);
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tuser removed (id: {user.Id}).");
                        sender.SendMessage(new ServiceMessage(user, ServiceOperation.Remove));
                    }
                }
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
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
        
        public void Load()
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                this.userStorage.Load();
                var users = userStorage.SearchForUser(u => true).ToList();
                foreach (var user in users)
                {
                    sender.SendMessage(new ServiceMessage(user, ServiceOperation.Add));
                }
                logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tservice state loaded.");
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Save()
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                this.userStorage.Save();
                logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName}:\tservice state saved.");
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        
    }

    //[Serializable]
    //public class MasterService : MarshalByRefObject,IService 
    //{
    //    private readonly IUserStorage userStorage;
    //    public string Name { get; }
    //    public ServiceMode Mode => ServiceMode.Master;
    //    private readonly IEnumerable<ConnectionInfo> slavesInfo;

    //    public MasterService(string name, IUserStorage userStorage)
    //    {
    //        if (userStorage == null)
    //        {
    //            throw new ArgumentNullException(nameof(userStorage));
    //        }
    //        this.userStorage = userStorage;
    //        this.Name = name;
    //    }

    //    public MasterService(IGenerator<int> idGenerator, IStateSaver saver, IUserValidator validator, IEnumerable<ConnectionInfo> slavesInfo)
    //    {
    //        userStorage = new MemoryUserStorage(idGenerator, validator, saver);
    //        this.slavesInfo = slavesInfo;
    //    }

    //    public int Add(User user)
    //    {
    //        int id = this.userStorage.Add(user);
    //        user.Id = id;
    //        SendMessageToSlaves(new ServiceMessage(user, ServiceOperation.Add));
    //        return id;
    //    }

    //    public void Delete(int id)
    //    {
    //        var users = userStorage.SearchForUser(u => u.Id == id);
    //        this.userStorage.Delete(id);

    //        foreach (var user in users)
    //        {
    //            SendMessageToSlaves(new ServiceMessage(user, ServiceOperation.Remove));
    //        }
    //    }

    //    public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
    //    {
    //        return this.userStorage.SearchForUser(predicates);
    //    }

    //    public void Save()
    //    {
    //        this.userStorage.Save();
    //    }

    //    public void Load()
    //    {
    //        this.userStorage.Load();
    //    }

    //    public void ListenForUpdates()
    //    {
    //        throw new NotSupportedException();
    //    }

    //    private void SendMessageToSlaves<T>(T message)
    //    {
    //        var serializer = new JavaScriptSerializer();
    //        string serializedMessage = serializer.Serialize(message);
    //        byte[] data = Encoding.UTF8.GetBytes(serializedMessage);

    //        foreach (var slave in slavesInfo)
    //        {
    //            using (TcpClient client = new TcpClient())
    //            {
    //                client.Connect(slave.IPAddress, slave.Port);
    //                using (NetworkStream stream = client.GetStream())
    //                {
    //                    stream.Write(data, 0, data.Length);
    //                }
    //            }
    //        }
    //    }
    //}

}
