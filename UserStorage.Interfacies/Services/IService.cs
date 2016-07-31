using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.Storages;

namespace UserStorage.Interfacies.Services
{
    public interface IService : IUserStorage
    {
        ServiceMode Mode { get; }
    }
}
