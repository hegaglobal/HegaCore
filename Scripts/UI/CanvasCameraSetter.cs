using UnityEngine;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        [SerializeField]
        private CameraType type;

        private Canvas canvas;

        private void Awake()
        {
            this.canvas = GetComponent<Canvas>();

            if (CameraManager.Instance.TryGet(this.type, out var camera))
                this.canvas.worldCamera = camera;
        }
    }
}
