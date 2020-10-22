namespace HegaCore
{
    public interface IContainsId<T> where T : unmanaged
    {
        bool Contains(in T id);
    }
}