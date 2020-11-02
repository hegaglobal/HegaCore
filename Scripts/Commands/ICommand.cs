namespace HegaCore
{
    public interface ICommand
    {
        void PreExecute();

        void Execute();

        void PostExecute();
    }
}
