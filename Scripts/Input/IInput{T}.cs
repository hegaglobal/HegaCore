namespace HegaCore
{
    public interface IInput<T>
    {
        bool AutoReset { get; }

        bool Get(T input);

        void ResetInput();
    }
}
