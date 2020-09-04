namespace HegaCore
{
    public interface ITextEmitter
    {
        TextEmission GetEmission(string key = null);
    }
}