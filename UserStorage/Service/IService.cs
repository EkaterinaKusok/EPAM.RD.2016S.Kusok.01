using System;
using System.Collections.Generic;
using UserStorage.UserEntities;
using UserStorage.UserStorage;

namespace UserStorage.Service
{
    public interface IService : IUserStorage
    {
        ServiceMode Mode { get; }
        string Name { get; }
        void ListenForUpdates();
    }
}
