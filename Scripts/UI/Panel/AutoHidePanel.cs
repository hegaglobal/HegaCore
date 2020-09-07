using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Panel))]
    public sealed class AutoHidePanel : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private Panel panel = null;

        [SerializeField]
        private float showingTime = 0f;

        private float autoHideAfter;

        private void OnValidate()
        {
            this.panel = GetComponent<Panel>();
        }

        private void Awake()
        {
            this.panel.OnShowComplete.AddListener(OnPanelShowComplete);
        }

        public void Show()
            => Show(this.showingTime);

        public void Show(float autoHideAfter)
        {
            this.autoHideAfter = autoHideAfter;
            this.panel.Show();
        }

        private void OnPanelShowComplete()
            => AutoHide().Forget();

        private async UniTaskVoid AutoHide()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(this.autoHideAfter));

            this.panel.Hide();
        }
    }
}