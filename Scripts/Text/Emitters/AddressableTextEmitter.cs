﻿using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class AddressableTextEmitter : SimpleAddressableComponentSpawner<TextModule>, ITextEmitter, ITextModuleSpawner
    {
        private readonly TextEmission emission = new TextEmission();

        protected override void OnInitialize()
        {
            this.emission.Initialize(this, string.Empty);
        }

        public TextEmission GetEmission(string key = null)
            => this.emission;

        UniTask<TextModule> ITextModuleSpawner.GetTextAsync(string key)
            => GetAsync();
    }
}
