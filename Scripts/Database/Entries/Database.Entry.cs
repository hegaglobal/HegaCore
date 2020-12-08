using System.Table;

namespace HegaCore.Database
{
    public abstract class Entry : IEntry
    {
        public int Id { get; protected set; }

        void IEntry.SetId(int value)
            => this.Id = value;
    }
}