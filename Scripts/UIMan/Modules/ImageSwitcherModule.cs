using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public sealed class ImageSwitcherModule : UIManModule<ImageSwitcherViewModel>
    {
        [SerializeField]
        private Panel image1 = null;

        [SerializeField]
        private Panel image2 = null;

        private Panel showPanel;
        private Panel hidePanel;

        private void Awake()
        {
            this.showPanel = this.image1;
            this.hidePanel = this.image2;
        }

        public void Initialize(string name, Color? color = null, float? duration = null)
        {
            SwitchPanel();
            Set(name ?? string.Empty, color ?? Color.white);

            if (duration.HasValue)
            {
                this.showPanel.Show(duration.Value);
                this.hidePanel.Hide(duration.Value);
            }
            else
            {
                this.showPanel.Show();
                this.hidePanel.Hide();
            }
        }

        private void SwitchPanel()
        {
            if (this.showPanel.IsHidden)
                return;

            if (this.showPanel == this.image1)
            {
                this.showPanel = this.image2;
                this.hidePanel = this.image1;
            }
            else
            {
                this.showPanel = this.image1;
                this.hidePanel = this.image2;
            }
        }

        private void Set(string name, Color color)
        {
            if (this.showPanel == this.image1)
            {
                this.DataInstance.ImageName1 = name;
                this.DataInstance.ImageColor1 = color;
            }
            else
            {
                this.DataInstance.ImageName2 = name;
                this.DataInstance.ImageColor2 = color;
            }
        }
    }
}