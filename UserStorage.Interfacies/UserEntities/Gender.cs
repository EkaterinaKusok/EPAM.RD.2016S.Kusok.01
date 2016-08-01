using System.Runtime.Serialization;

namespace UserStorage.Interfacies.UserEntities
{
    [DataContract]
    public enum Gender
    {
        [EnumMember] Male = 1,
        [EnumMember] Female
    }
}
