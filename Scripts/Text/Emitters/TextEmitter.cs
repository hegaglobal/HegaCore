using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class TextEmitter : SimpleComponentSpawner<TextModule>, ITextEmitter, ITextSpawner
    {
        private readonly TextEmission emission = new TextEmission();

        protected override void OnInitialize()
        {
            this.emission.Initialize(this, string.Empty);
        }

        public TextEmission GetEmission(string key = null)
            => this.emission;

        UniTask<TextModule> ITextSpawner.GetTextAsync(string key)
        {
            var text = Get();
            return UniTask.FromResult(text);
        }
    }
}
