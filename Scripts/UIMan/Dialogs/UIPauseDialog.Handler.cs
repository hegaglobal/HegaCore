using System;
using UnityEngine;
using UnuGames;
using Sirenix.OdinInspector;

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

        [ShowIf("@UnityEngine.Application.isPlaying")]
        private IInitializable<UIPauseDialog>[] initializables;

        [ShowIf("@UnityEngine.Application.isPlaying")]
        private IDeinitializable<UIPauseDialog>[] deinitializables;

        private PauseReturnType function;
        private Action<PauseReturnType> onHideCompleted;

        private void Awake()
        {
            this.initializables = GetComponentsInChildren<IInitializable<UIPauseDialog>>().OrEmpty();
            this.deinitializables = GetComponentsInChildren<IDeinitializable<UIPauseDialog>>().OrEmpty();
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            this.function = PauseReturnType.Resume;
            var index = 0;

            args.GetThenMoveNext(ref index, out this.onHideCompleted);

            if (this.initializables != null && this.initializables.Length > 0)
            {
                for (var i = 0; i < this.initializables.Length; i++)
                {
                    this.initializables[i]?.Initialize(this);
                }
            }
        }

        public override void OnHide()
        {
            base.OnHide();

            if (this.deinitializables != null && this.deinitializables.Length > 0)
            {
                for (var i = 0; i < this.deinitializables.Length; i++)
                {
                    this.deinitializables[i]?.Deinitialize(this);
                }
            }
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();
            this.onHideCompleted?.Invoke(this.function);
        }

        public void Hide(PauseReturnType function)
        {
            this.function = function;
            Hide(this.deactiveOnHide);
        }

        public void UI_Button_Settings()
        {
            UISettingsDialog.Show();
        }

        public void UI_Button_Resume()
            => Hide(PauseReturnType.Resume);

        public void UI_Button_Quit()
            => Hide(PauseReturnType.Quit);

        public void UI_Button_Replay()
            => Hide(PauseReturnType.Replay);
    }
}