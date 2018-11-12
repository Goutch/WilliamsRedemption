namespace Game.Puzzle
{
    public interface ITriggerable
    {
        void Open();
        void Close();
        void Unlock();
        void Lock();
        bool IsLocked();
        bool IsOpened();
    }
}


