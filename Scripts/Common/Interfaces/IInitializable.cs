namespace HegaCore
{
    public interface IInitializable
    {
        void Initialize(params object[] args);
    }

    public interface IInitializable<T>
    {
        void Initialize(T arg, params object[] args);
    }
}
