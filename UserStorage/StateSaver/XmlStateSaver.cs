using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using UserStorage.Interfacies.StateSavers;
using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.StateSaver
{
    public class XmlStateSaver : IStateSaver
    {
        private readonly string _fileName;

        public XmlStateSaver(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = ConfigurationManager.AppSettings["XmlUserStoragePath"];
            }
            _fileName = fileName;
        }

        public StorageState LoadState()
        {
            XmlSerializer formatter = new XmlSerializer(typeof (StorageState));
            StorageState state = new StorageState();
            using (Stream s = new FileStream(_fileName, FileMode.Open))
            {
                state = (StorageState) formatter.Deserialize(s);
            }
            return state;
        }

        public void SaveState(StorageState state)
        {
            XmlSerializer formatter = new XmlSerializer(typeof (StorageState));
            using (FileStream s = File.Create(_fileName))
            {
                formatter.Serialize(s, state);
            }
        }
    }
}
