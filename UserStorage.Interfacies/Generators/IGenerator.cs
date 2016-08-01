namespace UserStorage.Interfacies.Generators
{
    public interface IGenerator<T>
    {
        T GenerateNewId();

        T GenerateNewId(T currentId);

        bool SetCurrentId(T currentId);

        T GetCurrentId();
    }
}
