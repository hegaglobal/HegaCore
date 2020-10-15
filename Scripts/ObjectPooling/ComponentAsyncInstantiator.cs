using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public class ComponentAsyncInstantiator<T> : AsyncInstantiator<T> where T : Component
    {
        private bool useReference;
        private AssetReferenceGameObject reference;
        private string key;
        private Transform root;

        public void Initialize(Transform root, bool useReference, AssetReferenceGameObject reference, string key)
        {
            this.root = root;
            this.useReference = useReference;
            this.reference = reference;
            this.key = key;
        }

        public sealed override async UniTask<T> InstantiateAsync()
        {
            if (this.useReference && this.reference == null)
            {
                UnuLogger.LogError($"Instantiator uses asset reference but it is null");
                return null;
            }

            if (!this.useReference && string.IsNullOrEmpty(this.key))
            {
                UnuLogger.LogError($"Instantiator uses key but it is null or empty");
                return null;
            }

            OperationResult<GameObject> result;

            try
            {
                if (this.useReference)
                    result = await AddressablesManager.InstantiateAsync(this.reference, this.root);
                else
                    result = await AddressablesManager.InstantiateAsync(this.key, this.root);
            }
            catch (Exception ex)
            {
                UnuLogger.LogException(ex);
                return null;
            }

            var go = result.Value;
            go.transform.localScale = Vector3.one;

            if (go.TryGetComponent<T>(out var component))
                return component;

            AddressablesManager.ReleaseInstance(this.reference, go);
            return null;
        }
    }
}