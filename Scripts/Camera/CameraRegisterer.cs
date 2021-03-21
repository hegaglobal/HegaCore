using UnityEngine;

namespace HegaCore
{
    [RequireComponent(typeof(Camera))]
    public class CameraRegisterer : MonoBehaviour
    {
        [SerializeField]
        private CameraType[] types = new CameraType[0];

        private void Awake()
        {
            foreach (var type in this.types)
            {
                CameraManager.Instance.Add(type, GetComponent<Camera>());
            }
        }
    }
}
