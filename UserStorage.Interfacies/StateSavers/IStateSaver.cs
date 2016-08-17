using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.StateSavers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStateSaver
    {
        StorageState LoadState();

        void SaveState(StorageState state);
    }
}
