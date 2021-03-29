using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public interface ITextModuleSpawner
    {
        UniTask<TextModule> GetTextAsync(string key);

        void Return(TextModule module);
    }
}