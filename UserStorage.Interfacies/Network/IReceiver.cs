using System;
using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.Network
{
    public interface IReceiver
    {
        event EventHandler<UserEventArgs> Updating;

        void StartReceivingMessages();

        void StopReceiver();
    }
}
