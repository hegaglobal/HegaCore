namespace HegaCore
{
    public interface ISetableReadOnlyStruct<T> where T : struct
    {
        void Set(in T value);
    }
}