namespace HegaCore
{
    public interface IDeinitializable
    {
        void Deinitialize();
    }

    public interface IDeinitializable<T>
    {
        void Deinitialize(T arg);
    }
}
