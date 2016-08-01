using System;
using System.Collections.Generic;
using System.ServiceModel;
using UserStorage.Interfacies.Services;
using UserStorage.Interfacies.UserEntities;

namespace WcfServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfService : MarshalByRefObject, IWcfService
    {
        private readonly IService service;

        public WcfService(IService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException($"{nameof(service)} must be not null.");
            }

            this.service = service;
        }

        public int Add(User user)
        {
            return this.service.Add(user);
        }

        public void Delete(int id)
        {
            this.service.Delete(id);
        }

        public IList<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            return this.service.SearchForUser(predicates);
        }

        public void Save()
        {
            this.service.Save();
        }

        public void Load()
        {
            this.service.Load();
        }
    }
}
