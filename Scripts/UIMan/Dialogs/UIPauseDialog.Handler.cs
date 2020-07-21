using System;
using UnuGames;

namespace HegaCore
{
    public partial class UIPauseDialog : UIManDialog
    {
        public static void Show(Action<bool> onHideCompleted)
        {
            UIMan.Instance.ShowDialog<UIPauseDialog>(onHideCompleted);
        }

        public static void Hide()
        {
            UIMan.Instance.HideDialog<UIPauseDialog>(inactiveWhenHidden: true);
        }

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
            HideMe();
        }

        public void UI_Button_Quit()
        {
            this.resume = false;
            HideMe();
        }

        public void UI_Button_Replay()
        {
            this.resume = false;
            HideMe();
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();
            this.onHideCompleted?.Invoke(this.resume);
        }
    }
}