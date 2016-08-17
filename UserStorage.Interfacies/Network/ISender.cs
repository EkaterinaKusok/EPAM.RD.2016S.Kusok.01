using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.Network
{
    /// <summary>
    /// Provides functionality for working as sender.
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The service message.</param>
        void SendMessage(ServiceMessage message);
    }
}
