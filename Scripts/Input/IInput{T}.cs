namespace HegaCore
{
    public interface IInput<T>
    {
        bool AutoReset { get; }

        bool Consumable { get; }

        bool Get(T input);

        void Consume(T input);

        void ResetInput();
    }
}
