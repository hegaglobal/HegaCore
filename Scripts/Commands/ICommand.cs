namespace HegaCore
{
    public interface ICommand
    {
        void Execute();

        void Deactivate();
    }
}
