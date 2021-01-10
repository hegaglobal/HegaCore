namespace HegaCore
{
    public interface ISetable<in T>
    {
        void Set(T value);
    }
}