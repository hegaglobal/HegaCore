namespace HegaCore
{
    public interface IData
    {
        int Id { get; }

        string ToString(string format);
    }
}