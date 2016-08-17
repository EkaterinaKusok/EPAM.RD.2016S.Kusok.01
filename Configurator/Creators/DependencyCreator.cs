using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Interfacies.Creators;
using UserStorage.Interfacies.Generators;
using UserStorage.Interfacies.StateSavers;
using UserStorage.Interfacies.Storages;
using UserStorage.Interfacies.Validators;

namespace Configurator.Creators
{
    /// <summary>
    /// Represents common functionality for creating dependencies.
    /// </summary>
    /// <seealso cref="UserStorage.Interfacies.Creators.IDependencyCreator" />
    [Serializable]
    public class DependencyCreator : IDependencyCreator
    {
        private readonly Dictionary<Type, InstanceInfo> typesSingle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyCreator"/> class.
        /// </summary>
        /// <param name="typesSingle">The single types.</param>
        public DependencyCreator(Dictionary<Type, InstanceInfo> typesSingle)
        {
            this.typesSingle = typesSingle;
        }

        /// <summary>
        /// Creates the instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// The instance of type T.
        /// </returns>
        /// <exception cref="ArgumentNullException">Creator hasn't got types.</exception>
        public T CreateInstance<T>()
        {
            if (typesSingle == null)
            {
                throw new ArgumentNullException("Creator hasn't got types.");
            }

            var info = typesSingle[typeof(T)];
            return Create<T>(info);
        }

        // it's temporary fix
        /// <summary>
        /// Creates the instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters for creating.</param>
        /// <returns>
        /// The instance of type T.
        /// </returns>
        /// <exception cref="ArgumentNullException">Creator hasn't got types.</exception>
        /// <exception cref="NullReferenceException">Type '{type.Name}'</exception>
        /// <exception cref="ArgumentException"></exception>
        public T CreateInstance<T>(params object[] parameters)
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

            if (parameters.Length == 3)
            {
                return (T)Activator.CreateInstance(type, (IGenerator<int>)parameters[0], (IUserValidator)parameters[1], (IStateSaver)parameters[2]);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Creates the specified instance information.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instanceInfo">The instance information.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">{nameof(instanceInfo)}</exception>
        /// <exception cref="NullReferenceException">Type '{instanceInfo.TypeName}'</exception>
        /// <exception cref="ArgumentException">'{instanceInfo.TypeName}' doesn't implement interface '{typeof(T).Name}'.</exception>
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
