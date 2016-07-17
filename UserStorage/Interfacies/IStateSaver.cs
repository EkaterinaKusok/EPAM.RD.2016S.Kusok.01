using System;
using System.Collections.Generic;
using UserStorage.Entities;

namespace UserStorage.Interfacies
{
    public interface IStateSaver
    {
        State LoadState(string path);
        void SaveState(string path, State state);
    }
}
