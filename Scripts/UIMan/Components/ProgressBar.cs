using UnityEngine;
using UnuGames;

namespace HegaCore
{
    public sealed class ProgressBar : MonoBehaviour
    {
        public float Value
        {
            get => GetBar().Value;
            set => GetBar().Value = value;
        }

        private IProgressBar bar;

        private void Awake()
        {
            this.bar = GetComponent<IProgressBar>();
        }

        public IProgressBar GetBar()
            => this.bar ?? DefaultBar.Default;

        private struct DefaultBar : IProgressBar
        {
            public float Value { get; set; }

            public static DefaultBar Default { get; } = new DefaultBar();
        }
    }
}