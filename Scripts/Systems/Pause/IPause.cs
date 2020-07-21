namespace HegaCore
{
    public interface IPause<T>
    {
        void Enter(T current, T previous);

        void Update();

        void Exit(T current);
    }
}