namespace HegaCore
{
    public interface ISetableIn<T>
    {
        void Set(in T value);
    }
}