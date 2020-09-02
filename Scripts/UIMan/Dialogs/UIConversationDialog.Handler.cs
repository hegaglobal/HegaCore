using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnuGames;
using UnuGames.MVVM;
using RedBlueGames.Tools.TextTyper;
using VisualNovelData.Data;
using VisualNovelData.Commands;

namespace HegaCore.UI
{
    using Database;

    public partial class UIConversationDialog : UIManDialog, IPointerClickHandler
    {
        public static void Show(string id, Action onHideCompleted)
        {
            UIMan.Instance.ShowDialog<UIConversationDialog>(id, onHideCompleted);
        }

        public static void Hide()
        {
            UIMan.Instance.HideDialog<UIConversationDialog>(true);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
        }
    }
}