using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public class AsyncComponentInstantiator<T> : AsyncInstantiator<T> where T : Component
    {
        private readonly AssetReferenceGameObject prefabReference;
        private Transform root;

        public AsyncComponentInstantiator(AssetReferenceGameObject prefabReference)
        {
            this.prefabReference = prefabReference;
        }

        public void Initialize(Transform root)
        {
            this.root = root;
        }

        public sealed override async UniTask<T> InstantiateAsync()
        {
            var result = await AddressablesManager.InstantiateAsync(this.prefabReference, this.root);
            var go = result.Value;
            go.transform.localScale = Vector3.one;
            var component = go.GetComponent<T>();

            if (!component)
            {
                AddressablesManager.ReleaseInstance(this.prefabReference, go);
            }

            return component;
        }
    }
}