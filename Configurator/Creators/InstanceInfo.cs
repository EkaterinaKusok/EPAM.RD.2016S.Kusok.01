using System;

namespace Configurator.Creators
{
    /// <summary>
    /// Represents a instanse info.
    /// </summary>
    [Serializable]
    public class InstanceInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceInfo"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="parameters">The parameters.</param>
        public InstanceInfo(string typeName, params object[] parameters)
        {
            TypeName = typeName;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public object[] Parameters { get; }
    }
}
