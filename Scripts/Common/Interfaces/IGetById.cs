namespace HegaCore
{
    public interface IGetById<TId, TValue> where TId : unmanaged
    {
        TValue Get(in TId id);

        bool TryGet(in TId id, out TValue value);
    }
}