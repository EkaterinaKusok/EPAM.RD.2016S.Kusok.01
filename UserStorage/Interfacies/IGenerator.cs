namespace UserStorage.Interfacies
{
    public interface IGenerator<T>
    {
        T GenerateNewId();
        T GenerateNewId(T prev);
        bool SetCurrentId(T currentId);
        T GetCurrentId();
    }
}
