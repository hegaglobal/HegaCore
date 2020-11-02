namespace HegaCore
{
    public interface ICommand
    {
        bool Validate();

        void PreExecute();

        void Execute();

        void PostExecute();
    }
}
