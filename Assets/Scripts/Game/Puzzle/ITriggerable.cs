namespace Game.Puzzle
{
    //BEN_REVIEW : Chouette! Du polymorphisme.
    //             Ça fait ça de moins à vérifier dans ma liste...
    //BEN_CORRECTION : D'un autre coté, vous semblez avoir des méthodes qui sont liés à des "Unlockables" (des portes plus précisément).
    //                 Pas certain que c'est ce que vous vouliez.
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


