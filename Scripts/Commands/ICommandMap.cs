namespace HegaCore
{
    public interface IReadOnlyCommandMap
    {
        bool Contains(string key);

        bool TryGetCommand(string commandKey, out ICommand command);
    }

    public interface ICommandMap : IReadOnlyCommandMap
    {
        void Register(string key, ICommand command);

        void Register<T>(string key) where T : ICommand, new();

        void Remove(string key);

        void Remove(params string[] keys);
    }
}