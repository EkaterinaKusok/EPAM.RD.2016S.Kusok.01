using System;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using UserStorage.Interfacies.Network;
using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Network
{
    /// <summary>
    /// Implements functionality for working as receiver.
    /// </summary>
    public class Receiver : IReceiver
    {
        private readonly TcpListener server;

        /// <summary>
        /// Initializes a new instance of the <see cref="Receiver"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        public Receiver(ConnectionInfo info)
        {
            server = new TcpListener(info.IPAddress, info.Port);
            server.Start();
        }

        /// <summary>
        /// Occurs when [updating].
        /// </summary>
        public event EventHandler<UserEventArgs> Updating = delegate { };

        /// <summary>
        /// Starts the receiving messages.
        /// </summary>
        /// <exception cref="InvalidOperationException">Unable to deserialize request.</exception>
        public async void StartReceivingMessages()
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                while (true)
                {
                    using (TcpClient client = await server.AcceptTcpClientAsync())
                    using (NetworkStream stream = client.GetStream())
                    {
                        string serializedMessage = string.Empty;
                        byte[] data = new byte[1024];

                        int i = 0;
                        do
                        {
                            i = await stream.ReadAsync(data, 0, data.Length);
                            serializedMessage += Encoding.UTF8.GetString(data, 0, i);
                        }
                        while (i >= 1024);

                        try
                        {
                            ServiceMessage message = serializer.Deserialize<ServiceMessage>(serializedMessage);
                            OnUpdate(this, new UserEventArgs(message.User, message.Operation));
                        }
                        catch
                        {
                            throw new InvalidOperationException("Unable to deserialize request.");
                        }
                    }
                }
            }
            finally
            {
                server.Stop();
            }
        }

        /// <summary>
        /// Stops the receiver.
        /// </summary>
        public void StopReceiver()
        {
            server.Stop();
        }

        protected virtual void OnUpdate(object sender, UserEventArgs e)
        {
            Updating(sender, e);
        }
    }
}
