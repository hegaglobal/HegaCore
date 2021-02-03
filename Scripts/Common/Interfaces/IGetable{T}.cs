namespace HegaCore
{
    public interface IGetable<out T>
    {
        T Get();
    }
}