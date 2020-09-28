namespace HegaCore
{
    public readonly struct CellModes
    {
        public readonly CellMode Occupied;
        public readonly CellMode Diagonal;

        public CellModes(CellMode occupied, CellMode diagonal)
        {
            this.Occupied = occupied;
            this.Diagonal = diagonal;
        }

        public CellModes With(in CellMode? Occupied = null, in CellMode? Diagonal = null)
            => new CellModes(
                Occupied ?? this.Occupied,
                Diagonal ?? this.Diagonal
            );

        public static CellModes Include { get; } = new CellModes(CellMode.Include, CellMode.Include);

        public static CellModes Exclude { get; } = new CellModes(CellMode.Exclude, CellMode.Exclude);

        public static CellModes Only { get; } = new CellModes(CellMode.Only, CellMode.Only);
    }
}