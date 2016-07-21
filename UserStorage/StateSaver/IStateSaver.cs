
namespace UserStorage.StateSaver
{
    public interface IStateSaver
    {
        State LoadState(string path);
        void SaveState(string path, State state);
    }
}
