namespace HegaCore.Commands
{
    public interface ICommand : VisualNovelData.Commands.ICommand
    {
        string Key { get; }
    }
}