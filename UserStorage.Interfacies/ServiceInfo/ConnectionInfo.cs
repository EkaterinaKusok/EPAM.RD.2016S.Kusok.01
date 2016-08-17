using System;
using System.Net;

namespace UserStorage.Interfacies.ServiceInfo
{
    /// <summary>
    /// Connection information for communication.
    /// </summary>
    [Serializable]
    public class ConnectionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionInfo"/> class.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="port">The port.</param>
        /// <exception cref="ArgumentException">{nameof(ipAddress)}</exception>
        public ConnectionInfo(string ipAddress, int port)
        {
            IPAddress address;
            if (!IPAddress.TryParse(ipAddress, out address))
            {
                throw new ArgumentException($"{nameof(ipAddress)} is invalid.");
            }

            IPAddress = address;
            Port = port;
        }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public int Port { get; }
    }
}
