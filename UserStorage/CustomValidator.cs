using UserStorage.Entities;
using UserStorage.Interfacies;

namespace UserStorage
{
    public class CustomValidator : IValidator
    {
        public bool Validate(User user)
        {
            return true;
        }
    }
}
