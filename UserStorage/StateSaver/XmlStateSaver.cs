using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.StateSavers;

namespace UserStorage.StateSaver
{
    /// <summary>
    /// Implements functionality for saving state.
    /// </summary>
    /// <seealso cref="UserStorage.Interfacies.StateSavers.IStateSaver" />
    public class XmlStateSaver : IStateSaver
    {
        private readonly string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStateSaver"/> class.
        /// </summary>
        public XmlStateSaver()
        {
            this.fileName = ConfigurationManager.AppSettings["XmlUserStoragePath"];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStateSaver"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public XmlStateSaver(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = ConfigurationManager.AppSettings["XmlUserStoragePath"];
            }

            this.fileName = fileName;
        }

        /// <summary>
        /// Loads the state.
        /// </summary>
        /// <returns></returns>
        public StorageState LoadState()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(StorageState));
            StorageState state = new StorageState();
            using (Stream s = new FileStream(this.fileName, FileMode.Open))
            {
                state = (StorageState)formatter.Deserialize(s);
            }

            return state;
        }

        /// <summary>
        /// Saves the state.
        /// </summary>
        /// <param name="state">The storage state.</param>
        public void SaveState(StorageState state)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(StorageState));
            using (FileStream s = File.Create(this.fileName))
            {
                formatter.Serialize(s, state);
            }
        }
    }
}
