namespace HegaCore
{
    public interface IContains<in T>
    {
        bool Contains(T value);
    }
}