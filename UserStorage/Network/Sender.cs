using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using UserStorage.Interfacies.Network;
using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Network
{
    /// <summary>
    ///Imploments functionality for working as sender.
    /// </summary>
    public class Sender : ISender
    {
        private readonly IEnumerable<ConnectionInfo> slavesInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sender"/> class.
        /// </summary>
        /// <param name="slavesInfo">The slaves connection information.</param>
        /// <exception cref="ArgumentNullException">{nameof(slavesInfo)}</exception>
        public Sender(IEnumerable<ConnectionInfo> slavesInfo)
        {
            if (slavesInfo == null)
            {
                throw new ArgumentNullException($"{nameof(slavesInfo)} must be not null.");
            }

            this.slavesInfo = slavesInfo;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The service message.</param>
        public async void SendMessage(ServiceMessage message)
        {
            var serializer = new JavaScriptSerializer();
            string serializedMessage = serializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(serializedMessage);

            foreach (var slave in slavesInfo)
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(slave.IPAddress, slave.Port);
                    using (NetworkStream stream = client.GetStream())
                    {
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
        }
    }
}
