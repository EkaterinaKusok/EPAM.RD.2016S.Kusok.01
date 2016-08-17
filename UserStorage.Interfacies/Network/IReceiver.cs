using System;
using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.Network
{
    /// <summary>
    /// Provides functionality for working as receiver.
    /// </summary>
    public interface IReceiver
    {
        /// <summary>
        /// Occurs when [updating].
        /// </summary>
        event EventHandler<UserEventArgs> Updating;

        /// <summary>
        /// Starts the receiving messages.
        /// </summary>
        void StartReceivingMessages();

        /// <summary>
        /// Stops the receiver.
        /// </summary>
        void StopReceiver();
    }
}
