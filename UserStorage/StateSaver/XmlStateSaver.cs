using System.Configuration;
using System.IO;
using System.Xml.Serialization;

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

        public UserState LoadState()
        {
            XmlSerializer formatter = new XmlSerializer(typeof (UserState));
            UserState state = new UserState();
            using (Stream s = new FileStream(_fileName, FileMode.Open))
            {
                state = (UserState) formatter.Deserialize(s);
            }
            return state;
        }

        public void SaveState( UserState state)
        {
            XmlSerializer formatter = new XmlSerializer(typeof (UserState));
            using (FileStream s = File.Create(_fileName))
            {
                formatter.Serialize(s, state);
            }
        }
    }
}
