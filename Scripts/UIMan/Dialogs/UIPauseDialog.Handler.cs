using System;
using UnityEngine;
using UnuGames;

namespace HegaCore
{
    public partial class UIPauseDialog : UIManDialog
    {
        public static void Show(Action<bool> onHideCompleted)
        {
            UIMan.Instance.ShowDialog<UIPauseDialog>(onHideCompleted);
        }

        public static void Hide(bool deactive = true)
        {
            UIMan.Instance.HideDialog<UIPauseDialog>(deactive);
        }

        [SerializeField]
        private bool deactiveOnHide = false;

        private bool resume;
        private Action<bool> onHideCompleted;

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            this.resume = true;
            var index = 0;

            args.GetThenMoveNext(ref index, out this.onHideCompleted);
        }

        public void UI_Button_Settings()
        {
            UISettingsDialog.Show();
        }

        public void UI_Button_Resume()
        {
            this.resume = true;
            Hide(this.deactiveOnHide);
        }

        public void UI_Button_Quit()
        {
            this.resume = false;
            Hide(this.deactiveOnHide);
        }

        public void UI_Button_Replay()
        {
            this.resume = false;
            Hide(this.deactiveOnHide);
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();
            this.onHideCompleted?.Invoke(this.resume);
        }
    }
}