using System;
using UnityEngine;
using UnuGames;

namespace HegaCore.UI
{
    public partial class UIPauseDialog : UIManDialog
    {
        public static void Show(Action<PauseReturnType> onHideCompleted)
        {
            UIMan.Instance.ShowDialog<UIPauseDialog>(onHideCompleted);
        }

        public static void Hide(bool deactive = true)
        {
            UIMan.Instance.HideDialog<UIPauseDialog>(deactive);
        }

        [SerializeField]
        private bool deactiveOnHide = false;

        private PauseReturnType function;
        private Action<PauseReturnType> onHideCompleted;

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            this.function = PauseReturnType.Resume;
            var index = 0;

            args.GetThenMoveNext(ref index, out this.onHideCompleted);
        }

        public void UI_Button_Settings()
        {
            UISettingsDialog.Show();
        }

        public void UI_Button_Resume()
        {
            this.function = PauseReturnType.Resume;
            Hide(this.deactiveOnHide);
        }

        public void UI_Button_Quit()
        {
            this.function = PauseReturnType.Quit;
            Hide(this.deactiveOnHide);
        }

        public void UI_Button_Replay()
        {
            this.function = PauseReturnType.Replay;
            Hide(this.deactiveOnHide);
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();
            this.onHideCompleted?.Invoke(this.function);
        }
    }
}