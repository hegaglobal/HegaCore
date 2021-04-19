namespace HegaCore
{
    public interface IGetable<in TKey, out TValue>
    {
        TValue Get(TKey key);
    }
}