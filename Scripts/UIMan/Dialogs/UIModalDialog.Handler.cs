using System;
using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public partial class UIModalDialog : UIManDialog
    {
        public static void ShowConfirm(string title = "", 
            string content = "", 
            Action onConfirm = null, 
            string okBtn = "",
            KeyCode yesKey = KeyCode.None)
        {
            UIMan.Instance.ShowDialog<UIModalDialog>(false, 
                title ?? string.Empty, 
                content ?? string.Empty,
                onConfirm, 
                okBtn ?? string.Empty,
                yesKey);
        }

        public static void ShowProceed(string title = "", 
            string content = "", 
            Action onProceed = null, 
            Action onCancel = null, 
            string okBtn = "", 
            string cancelBtn ="" , 
            KeyCode yesKey = KeyCode.None,
            KeyCode cancelKey = KeyCode.None)
        {
            UIMan.Instance.ShowDialog<UIModalDialog>(true, 
                title ?? string.Empty, 
                content ?? string.Empty,
                onProceed, 
                onCancel, 
                okBtn ?? string.Empty, 
                cancelBtn ?? string.Empty,
                yesKey, cancelKey);
        }

        public static void Hide(bool deactive = true)
        {
            UIMan.Instance.HideDialog<UIModalDialog>(deactive);
        }

        [SerializeField]
        private Panel rootPanel = null;

        private Action onProceed;
        private Action onConfirmCancel;

        private KeyCode yesKeyCode = KeyCode.None;
        private KeyCode cancelKeyCode = KeyCode.None;
        
        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            var index = 0;

            args.GetThenMoveNext(ref index, out bool isProceed)
                .GetThenMoveNext(ref index, out string title)
                .GetThenMoveNext(ref index, out string content);

            this.IsProceed = isProceed;
            this.Title = title;
            this.Content = content;

            this.HasTitle = !string.IsNullOrEmpty(this.Title);

            if (this.IsProceed)
            {
                args.GetThenMoveNext(ref index, out this.onProceed)
                    .GetThenMoveNext(ref index, out this.onConfirmCancel)
                    .GetThenMoveNext(ref index, out string customOkBtn)
                    .GetThenMoveNext(ref index, out string customCancelBtn)
                    .GetThenMoveNext(ref index, out this.yesKeyCode)
                    .GetThenMoveNext(ref index, out this.cancelKeyCode);

                string yesStr = yesKeyCode == KeyCode.None ? string.Empty : $"( {yesKeyCode})";
                string cancelStr = cancelKeyCode == KeyCode.None ? string.Empty : $"( {cancelKeyCode})";
                
                OkBtn = (string.IsNullOrEmpty(customOkBtn) ? L10n.Localize("btn-ok") : customOkBtn) + yesStr;
                CancelBtn = (string.IsNullOrEmpty(customCancelBtn) ? L10n.Localize("btn-cancel") : customCancelBtn) + cancelStr;
            }
            else
            {
                args.GetThenMoveNext(ref index, out this.onProceed)
                    .GetThenMoveNext(ref index, out string customOkBtn)
                    .GetThenMoveNext(ref index, out this.yesKeyCode);
                
                string yesStr = yesKeyCode == KeyCode.None ? string.Empty : $"( {yesKeyCode})";
                
                OkBtn = (string.IsNullOrEmpty(customOkBtn) ? L10n.Localize("btn-ok") : customOkBtn) + yesStr;
            }

            this.rootPanel.Show(true);
        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();

            this.rootPanel.Hide(true);
        }

        /// <summary>
        /// UI BUTTON, PLEASE DO NOT RENAME
        /// </summary>
        public void UI_Button_Proceed()
        {
            this.onProceed?.Invoke();

            Hide();
        }

        /// <summary>
        /// UI BUTTON, PLEASE DO NOT RENAME
        /// </summary>
        public void UI_Button_ConfirmCancel()
        {
            this.onConfirmCancel?.Invoke();

            Hide();
        }

        void Update()
        {
            if (State == UIState.Show)
            {
                if (yesKeyCode != KeyCode.None)
                {
                    if (Input.GetKeyDown(yesKeyCode))
                    {
                        UI_Button_Proceed();
                    }
                }
                
                if (cancelKeyCode != KeyCode.None)
                {
                    if (Input.GetKeyDown(cancelKeyCode))
                    {
                        UI_Button_ConfirmCancel();
                    }
                }
            }
        }
    }
}