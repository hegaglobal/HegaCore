namespace HegaCore
{
    public interface IEnergyView : IComponentView
    {
        int Value { get; }

        int ActualValue { get; }

        int MaxValue { get; }

        void Initialize(int maxValue, int value);

        void ChangeValue(int amount);

        void SetValue(int value);
    }
}