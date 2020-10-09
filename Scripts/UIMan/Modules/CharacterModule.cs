using System;
using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public class CharacterModule : UIManModule<CharacterViewModel>
    {
        public event Action<string> OnSelect;

        [SerializeField]
        private string character = string.Empty;

        [SerializeField]
        private string image = string.Empty;

        public string Character => this.character;

        public void Initialize(bool unlocked, string image = "")
        {
            this.DataInstance.Unlocked = unlocked;
            this.DataInstance.Selected = false;
            this.DataInstance.GrayscaleAmount = unlocked ? 0f : 1f;
            this.DataInstance.Image = string.IsNullOrEmpty(image) ? this.image : image;
        }

        public void Deinitialize()
        {
            this.OnSelect = null;
            this.DataInstance.Selected = false;
            this.DataInstance.Unlocked = false;
        }

        public void SetSelected(bool value)
            => this.DataInstance.Selected = value;

        public void UI_Button_Select()
            => this.OnSelect?.Invoke(this.character);
    }
}