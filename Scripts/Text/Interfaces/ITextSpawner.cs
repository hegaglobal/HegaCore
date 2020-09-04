using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public interface ITextSpawner
    {
        UniTask<TextModule> GetTextAsync(string key);

        void Return(TextModule module);
    }
}