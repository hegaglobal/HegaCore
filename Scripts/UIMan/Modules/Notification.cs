using UnityEngine;

namespace HegaCore.UI
{
    [RequireComponent(typeof(AutoHidePanel))]
    public sealed class Notification : SingletonModule<Notification, NotificationViewModel>
    {
        private AutoHidePanel autoHide;

        private void Awake()
        {
            this.autoHide = GetComponent<AutoHidePanel>();
        }

        public void Show(string content, float? duration = null)
            => Show(string.Empty, content, duration);

        public void Show(string title, string content, float? duration = null)
        {
            this.DataInstance.Title = title ?? string.Empty;
            this.DataInstance.Content = content;
            this.DataInstance.HasTitle = !string.IsNullOrEmpty(title);

            if (duration.HasValue)
                this.autoHide.Show(duration.Value);
            else
                this.autoHide.Show();
        }

        public void Hide()
            => this.autoHide.Hide();
    }
}
