namespace HegaCore
{
    public interface IEnergyView : IView
    {
        int Value { get; }

        int ActualValue { get; }

        int MaxValue { get; }

        void Initialize(int maxValue, int value);

        void ChangeValue(int amount);

        void SetValue(int value);
    }
}