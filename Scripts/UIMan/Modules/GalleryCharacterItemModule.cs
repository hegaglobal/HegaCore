using System;
using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public class GalleryCharacterItemModule : UIManModule<GalleryCharacterItemViewModel>
    {
        [SerializeField]
        private int id = 0;

        public int Id => this.id;

        private Action<int> onSelect;

        public void Initialize(bool unlocked, Action<int> onSelect)
        {
            this.DataInstance.Character = string.Empty;
            this.DataInstance.Unlocked = unlocked;
            this.onSelect = onSelect;
        }

        public void Initialize(string character, bool unlocked, Action<int> onSelect)
        {
            this.DataInstance.Character = character ?? string.Empty;
            this.DataInstance.Unlocked = unlocked;
            this.onSelect = onSelect;
        }

        public void Deinitialize()
        {
            this.onSelect = null;
            this.DataInstance.Unlocked = true;
            this.DataInstance.Selected = false;
            this.DataInstance.Character = string.Empty;
        }

        public void SetSelected(bool value)
            => this.DataInstance.Selected = value;

        public void UI_Button_Select()
            => this.onSelect?.Invoke(this.id);
    }
}