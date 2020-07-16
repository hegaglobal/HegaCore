using System;
using UnityEngine;
using UnuGames;

namespace HegaCore
{
    public partial class UIModalDialog : UIManDialog
    {
        public static void ShowConfirm(string title = "", string content = "", Action onConfirm = null)
        {
            UIMan.Instance.ShowDialog<UIModalDialog>(false, title ?? string.Empty, content ?? string.Empty,
                                                     onConfirm);
        }

        public static void ShowProceed(string title = "", string content = "", Action onProceed = null, Action onCancel = null)
        {
            UIMan.Instance.ShowDialog<UIModalDialog>(true, title ?? string.Empty, content ?? string.Empty,
                                                     onProceed, onCancel);
        }

        public static void Hide()
        {
            UIMan.Instance.DestroyUI<UIModalDialog>();
        }

        [SerializeField]
        private Panel rootPanel = null;

        private Action onProceed;
        private Action onConfirmCancel;

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
                    .GetThenMoveNext(ref index, out this.onConfirmCancel);
            }
            else
            {
                args.GetThenMoveNext(ref index, out this.onConfirmCancel);
            }

            this.rootPanel.Show(true);
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();

            this.rootPanel.Hide(true);
        }

        public void UI_Button_Proceed()
        {
            this.onProceed?.Invoke();

            Hide();
        }

        public void UI_Button_ConfirmCancel()
        {
            this.onConfirmCancel?.Invoke();

            Hide();
        }
    }
}