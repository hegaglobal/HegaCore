namespace HegaCore
{
    public interface IInput<T>
    {
        bool Get(T input);

        void ResetInput();
    }
}
