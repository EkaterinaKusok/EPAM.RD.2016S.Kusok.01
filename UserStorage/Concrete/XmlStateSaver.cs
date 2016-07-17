using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UserStorage.Entities;
using UserStorage.Interfacies;

namespace UserStorage.Concrete
{
    public class XmlStateSaver : IStateSaver
    {
        public State LoadState(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof (State));
            State state = new State();
            using (Stream s = new FileStream(path, FileMode.Open))
            {
                state = (State) formatter.Deserialize(s);
            }
            return state;
        }

        public void SaveState( string path, State state)
        {
            XmlSerializer formatter = new XmlSerializer(typeof (State));
            using (FileStream s = File.Create(path))
            {
                formatter.Serialize(s, state);
            }
        }
    }
}
