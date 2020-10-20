namespace HegaCore
{
    public interface IGetByLuid<T>
    {
        T Get(in Luid id);

        bool TryGet(in Luid id, out T value);
    }
}