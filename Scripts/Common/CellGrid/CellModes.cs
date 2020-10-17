namespace HegaCore
{
    public readonly struct CellModes
    {
        public readonly CellMode Marked;
        public readonly CellMode Diagonal;

        public CellModes(CellMode marked, CellMode diagonal)
        {
            this.Marked = marked;
            this.Diagonal = diagonal;
        }

        public CellModes With(in CellMode? Marked = null, in CellMode? Diagonal = null)
            => new CellModes(
                Marked ?? this.Marked,
                Diagonal ?? this.Diagonal
            );

        public static CellModes Include { get; } = new CellModes(CellMode.Include, CellMode.Include);

        public static CellModes Exclude { get; } = new CellModes(CellMode.Exclude, CellMode.Exclude);

        public static CellModes Only { get; } = new CellModes(CellMode.Only, CellMode.Only);
    }
}