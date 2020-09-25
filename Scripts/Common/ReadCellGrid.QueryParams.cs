namespace HegaCore
{
    public partial class ReadCellGrid
    {
        public readonly struct QueryParams
        {
            public readonly bool IncludeOccupied;
            public readonly bool IncludeDiagonal;

            public QueryParams(bool includeOccupied, bool includeDiagonal)
            {
                this.IncludeOccupied = includeOccupied;
                this.IncludeDiagonal = includeDiagonal;
            }

            public QueryParams With(bool? IncludeOccupied = null, bool? IncludeDiagonal = null)
                => new QueryParams(
                    IncludeOccupied ?? this.IncludeOccupied,
                    IncludeDiagonal ?? this.IncludeDiagonal
                );
        }
    }
}