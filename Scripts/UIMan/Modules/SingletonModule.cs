using JetBrains.Annotations;
using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    [DisallowMultipleComponent]
    public abstract class SingletonModule<TModule, TModel> : UIManModule<TModel>
        where TModule : SingletonModule<TModule, TModel>
        where TModel : new()
    {
        [CanBeNull]
        private static TModule _instance;

        [NotNull]
        private static readonly object Lock = new object();

        [SerializeField]
        private bool persistent = true;

        [NotNull]
        public static TModule Instance
        {
            get
            {
                if (SingletonBehaviour.Quitting)
                {
                    UnuLogger.LogWarning($"[{nameof(SingletonBehaviour)}<{typeof(TModule)}>] Instance will not be returned because the application is quitting.");

                    return null;
                }

                lock (Lock)
                {
                    if (_instance != null)
                        return _instance;

                    var instances = FindObjectsOfType<TModule>();
                    var count = instances.Length;

                    if (count > 0)
                    {
                        if (count == 1)
                            return _instance = instances[0];

                        UnuLogger.LogWarning($"[{nameof(SingletonBehaviour)}<{typeof(TModule)}>] There should never be more than one {nameof(SingletonBehaviour)} of type {typeof(TModule)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");

                        for (var i = 1; i < instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }

                        return _instance = instances[0];
                    }

                    UnuLogger.Log($"[{nameof(SingletonBehaviour)}<{typeof(TModule)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");

                    return _instance = new GameObject($"({nameof(SingletonBehaviour)}){typeof(TModule)}")
                               .AddComponent<TModule>();
                }
            }
        }

        private void Awake()
        {
            if (this.persistent)
                DontDestroyOnLoad(this.gameObject);

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}