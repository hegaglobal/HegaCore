namespace HegaCore.Events.Commands
{
    public interface IEventCommand : VisualNovelData.Commands.ICommand
    {
        string Key { get; }
    }
}