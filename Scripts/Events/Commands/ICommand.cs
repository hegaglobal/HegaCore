namespace HegaCore.Events.Commands
{
    public interface ICommand : VisualNovelData.Commands.ICommand
    {
        string Key { get; }
    }
}