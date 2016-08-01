using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.StateSavers
{
    public interface IStateSaver
    {
        StorageState LoadState();

        void SaveState(StorageState state);
    }
}
