namespace HegaCore
{
    public interface IStopById<T1, T2>
        where T1 : unmanaged
        where T2 : unmanaged
    {
        void Stop(in T1 id1, in T2 id2);
    }
}