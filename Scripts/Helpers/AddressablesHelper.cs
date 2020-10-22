using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnuGames;
using Live2D.Cubism.Loader;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    using UnityObject = UnityEngine.Object;

    public static class AddressablesHelper
    {
        private static bool _registered;

        public static void RegisterLoaders()
        {
            if (_registered)
                return;

            Live2D.Cubism.Loader.CubismLoader.Initialize(new CubismLoader());
            UnuGames.UIManLoader.Initialize(new UIManLoader());
            UnuGames.SpriteAtlasManager.Silent = true;
            UnityEngine.U2D.SpriteAtlasManager.atlasRequested += LoadSpriteAtlas;

            _registered = true;
        }

        public static void Initialize(Action onComplete)
        {
            AddressablesManager.Initialize(onComplete);
        }

        public static async UniTask InitializeAsync()
        {
            await AddressablesManager.InitializeAsync();
        }

        private readonly struct UIManLoader : IUIManLoader
        {
            void IUIManLoader.LoadGameObject(string key, Action<string, UnityObject> onLoaded)
            {
                AddressablesManager.LoadAsset<GameObject>(key, onLoaded);
            }

            void IUIManLoader.LoadSprite(string key, Action<string, UnityObject> onLoaded)
            {
                AddressablesManager.LoadAsset<Sprite>(key, onLoaded);
            }

            void IUIManLoader.LoadTexture2D(string key, Action<string, UnityObject> onLoaded)
            {
                AddressablesManager.LoadAsset<Texture2D>(key, onLoaded);
            }

            void IUIManLoader.LoadSpriteAtlas(string key, Action<string, UnityObject> onLoaded)
            {
                AddressablesManager.LoadAsset<SpriteAtlas>(key, onLoaded);
            }

            void IUIManLoader.LoadObject(string key, Action<string, UnityObject> onLoaded)
            {
                AddressablesManager.LoadAsset(key, onLoaded);
            }
        }

        private readonly struct CubismLoader : ICubsimLoader
        {
            void ICubsimLoader.Load(string key, Action<string, Texture2D> onLoaded)
            {
                AddressablesManager.LoadAsset(key, onLoaded);
            }
        }

        private static void LoadSpriteAtlas(string key, Action<SpriteAtlas> onSucceeded)
        {
            AddressablesManager.LoadAsset<SpriteAtlas>(key, (_, atlas) => onSucceeded?.Invoke(atlas));
        }
    }
}
