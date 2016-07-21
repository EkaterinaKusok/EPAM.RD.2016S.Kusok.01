using System.Collections.Generic;

namespace UserStorage.Generator
{
    public interface IGenerator<T>
    {
        T GenerateNewId();
        T GenerateNewId(T prev);
        bool SetCurrentId(T currentId);
        T GetCurrentId();
    }
}
