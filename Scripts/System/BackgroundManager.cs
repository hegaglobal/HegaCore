using UnityEngine;

namespace HegaCore
{
    using UI;

    public sealed class BackgroundManager : SingletonBehaviour<BackgroundManager>
    {
        [SerializeField]
        private Canvas canvas = null;

        [SerializeField]
        private bool findCameraOnAwake = true;

        [SerializeField]
        private ImageSwitcherModule background = null;

        [SerializeField]
        private ImageSwitcherModule foreground = null;

        [SerializeField] 
        private GameObject BlurObject;
        
        private void Awake()
        {
            if (this.findCameraOnAwake)
                this.canvas.worldCamera = Camera.main;
        }

        public void SetCamera(Camera camera)
            => this.canvas.worldCamera = camera;

        public void Initialize(string background, string foreground,
                               Color? backgroundColor = null, Color? foregroundColor = null,
                               float? duration = null)
        {
            SetBackground(background, backgroundColor, duration);
            SetForeground(foreground, foregroundColor, duration);
        }

        public void Deinitialize()
        {
            SetBackground(string.Empty);
            SetForeground(string.Empty);
        }

        public void SetBackground(string name, Color? color = null, float? duration = null)
            => this.background.Switch(name, color, duration);

        public void SetForeground(string name, Color? color = null, float? duration = null)
            => this.foreground.Switch(name, color, duration);

        public void SetBlur(bool blur)
        {
            BlurObject.SetActive(blur);
        }
    }
}
