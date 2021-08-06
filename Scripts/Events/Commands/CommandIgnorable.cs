namespace HegaCore.Events.Commands
{
    public readonly struct CommandIgnorable
    {
        public readonly string Key;
        public readonly bool Ignorable;

        public CommandIgnorable(IEventCommand command, bool ignorable)
        {
            this.Key = command.Key ?? string.Empty;
            this.Ignorable = ignorable;
        }

        public CommandIgnorable(string key, bool ignorable)
        {
            this.Key = key ?? string.Empty;
            this.Ignorable = ignorable;
        }
    }
}
