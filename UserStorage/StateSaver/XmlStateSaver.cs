using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.StateSavers;

namespace UserStorage.StateSaver
{
    public class XmlStateSaver : IStateSaver
    {
        private readonly string fileName;

        public XmlStateSaver()
        {
            this.fileName = ConfigurationManager.AppSettings["XmlUserStoragePath"];
        }

        public XmlStateSaver(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = ConfigurationManager.AppSettings["XmlUserStoragePath"];
            }

            this.fileName = fileName;
        }

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
