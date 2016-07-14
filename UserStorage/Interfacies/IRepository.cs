using System;
using System.Collections.Generic;
using UserStorage.Entities;

namespace UserStorage.Interfacies
{
    public interface IRepository<T>
    {
        IEnumerable<T> Load(string path);
        bool Save(string path, IEnumerable<T> items);
    }
}
