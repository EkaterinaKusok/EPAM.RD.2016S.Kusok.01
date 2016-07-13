using UserStorage.Entities;

namespace UserStorage.Interfacies
{
    public interface IValidator
    {
        bool Validate(User user);
    }
}