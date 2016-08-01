using System;
using System.Collections.Generic;
using System.ServiceModel;
using UserStorage.Interfacies.UserEntities;

namespace WcfServiceLibrary
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        int Add(User user);

        [OperationContract]
        void Delete(int id);

        [OperationContract]
        IList<User> SearchForUser(params Func<User, bool>[] predicates);

        [OperationContract]
        void Save();

        [OperationContract]
        void Load();
    }
}
