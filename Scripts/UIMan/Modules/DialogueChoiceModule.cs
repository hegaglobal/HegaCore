using System;
using UnuGames;

namespace HegaCore.UI
{
    public sealed class DialogueChoiceModule : UIManModule<DialogueChoiceViewModel>
    {
        /// <summary>
        /// Call from UI: Button
        /// </summary>
        public void Button_OnClick()
            => this.DataInstance.OnSelect?.Invoke(this.DataInstance.Id);
    }

    public partial class DialogueChoiceViewModel
    {
        public Action<int> OnSelect { get; set; }
    }
}