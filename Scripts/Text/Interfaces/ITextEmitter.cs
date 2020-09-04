using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public interface ITextEmitter
    {
        TextEmission GetEmission(string key = null);
    }
}