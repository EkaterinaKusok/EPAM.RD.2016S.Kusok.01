namespace UserStorage.StateSaver
{
    public interface IStateSaver
    {
        UserState LoadState();
        void SaveState(UserState state);
    }
}
