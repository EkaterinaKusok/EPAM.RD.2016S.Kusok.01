using System;

namespace UserStorage.Service
{
    [Serializable]
    public class ServiceSetup 
    {
        public string Name { get; set; }
        public ServiceMode Mode { get; set; }
        public string IdGenerator { get; set; }
        public string Validator { get; set; }
        public string Saver { get; set; }
        public string Logger { get; set; }
        public string Communicator { get; set; }
    }
}
