using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public sealed class CameraManager : SingletonBehaviour<CameraManager>
    {
        [SerializeField, ReadOnly, PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "Type", ValueLabel = "Camera")]
        private CameraMap cameraMap = new CameraMap();

        private Camera defaultCamera;

        private void Awake()
        {
            this.defaultCamera = Camera.main;
        }

        public Camera Get(CameraType type)
        {
            if (this.cameraMap.TryGetValue(type, out var camera))
                return camera;

            return this.defaultCamera;
        }

        public bool TryGet(CameraType type, out Camera camera)
            => this.cameraMap.TryGetValue(type, out camera);

        public void Add(CameraType type, Camera camera)
        {
            if (!camera)
            {
                UnuLogger.LogError("Cannot add null camera");
                return;
            }

            if (this.cameraMap.ContainsKey(type))
                UnuLogger.LogWarning($"Camera type={type} will be overwritten");

            this.cameraMap[type] = camera;
        }

        [Serializable]
        private sealed class CameraMap : SerializableDictionary<CameraType, Camera> { }
    }
}
