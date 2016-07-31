using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.Network
{
    public interface ISender
    {
        void SendMessage(ServiceMessage message);
    }
}
