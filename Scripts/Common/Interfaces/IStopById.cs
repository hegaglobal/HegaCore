namespace HegaCore
{
    public interface IStopById<T> where T : unmanaged
    {
        void Stop(in T id);
    }
}