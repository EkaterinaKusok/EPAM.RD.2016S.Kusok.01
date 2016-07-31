using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Interfacies.Creators;
using UserStorage.Interfacies.Generators;
using UserStorage.Interfacies.StateSavers;
using UserStorage.Interfacies.Validators;

namespace Configurator.Creators
{
    [Serializable]
    public class DependencyCreator : IDependencyCreator
    {
        private readonly Dictionary<Type, InstanceInfo> typesSingle;

        public DependencyCreator(Dictionary<Type, InstanceInfo> typesSingle)
        {
            this.typesSingle = typesSingle;
        }

        public T CreateInstance<T>()
        {
            if (typesSingle == null)
            {
                throw new ArgumentNullException("Creator hasn't got types.");
            }

            var info = typesSingle[typeof(T)];
            return Create<T>(info);
        }

        public T CreateInstance<T>(params object[] parameters)
        {
            throw new NotImplementedException();
        }

        //it's temporary fix
        public IUserStorage CreateInstance<IUserStorage>(IGenerator<int> generator, IUserValidator validator, IStateSaver saver)
        {
            if (typesSingle == null)
            {
                throw new ArgumentNullException("Creator hasn't got types.");
            }

            Type type = Type.GetType(typesSingle[typeof(IUserStorage)].TypeName);
            if (type == null)
            {
                throw new NullReferenceException($"Type '{type.Name}' not found.");
            }
            return (IUserStorage)Activator.CreateInstance(type, generator, validator, saver);
        }

        private T Create<T>(InstanceInfo instanceInfo)
        {
            if (instanceInfo == null)
            {
                throw new ArgumentNullException($"{nameof(instanceInfo)} must be not null.");
            }

            Type type = Type.GetType(instanceInfo.TypeName);

            if (type == null)
            {
                throw new NullReferenceException($"Type '{instanceInfo.TypeName}' not found.");
            }

            if (type.GetInterface(typeof(T).Name) == null)
            {
                throw new ArgumentException($"'{instanceInfo.TypeName}' doesn't implement interface '{typeof(T).Name}'.");
            }

            return (T)Activator.CreateInstance(type, instanceInfo.Parameters);
        }
    }
}
