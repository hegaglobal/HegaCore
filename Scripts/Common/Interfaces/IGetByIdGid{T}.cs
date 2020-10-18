namespace HegaCore
{
    public interface IGetByIdGid<T>
    {
        T Get(in IdGid id);

        bool TryGet(in IdGid id, out T value);
    }
}