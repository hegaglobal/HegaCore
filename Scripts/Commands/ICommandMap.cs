namespace HegaCore
{
    public interface IReadOnlyCommandMap
    {
        bool Contains(string id);

        bool TryGetCommand(string id, out ICommand command);
    }

    public interface ICommandMap : IReadOnlyCommandMap
    {
        void Register(string id, ICommand command);

        void Register<T>(string id) where T : ICommand, new();

        void Remove(string id);

        void Remove(params string[] ids);
    }
}