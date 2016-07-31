using System.Collections.Generic;

namespace UserStorage.Interfacies.Creators
{
    public interface IDependencyCreator
    {
        T CreateInstance<T>();
        T CreateInstance<T>(params object[] parameters);
    }
}
