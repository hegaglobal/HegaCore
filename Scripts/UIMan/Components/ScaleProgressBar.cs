using UnityEngine;
using UnuGames;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace HegaCore
{
    public class ScaleProgressBar : MonoBehaviour, IProgressBar
    {
        [SerializeField]
        private Transform bar = null;

        [SerializeField]
        private Vector2 barSize = Vector2.zero;

        [SerializeField, Range(0f, 1f)]
        private float value = 0;

#if ODIN_INSPECTOR
        [FoldoutGroup("Horizontal"), LabelText("Enable")]
#else
        [Space]
        [Header("Horizonal")]
#endif
        [SerializeField]
        private bool horizontal = true;

#if ODIN_INSPECTOR
        [FoldoutGroup("Horizontal"), LabelText("Thumb")]
#endif
        [SerializeField]
        private SpriteRenderer horizontalThumb = null;

#if ODIN_INSPECTOR
        [FoldoutGroup("Horizontal"), LabelText("Thumb Origin")]
#endif
        [SerializeField]
        private HorizontalOrigins horizontalThumbOrigin = HorizontalOrigins.Left;

#if ODIN_INSPECTOR
        [FoldoutGroup("Vertical"), LabelText("Enable")]
#else
        [Space]
        [Header("Vertical")]
#endif
        [SerializeField]
        private bool vertical = true;

#if ODIN_INSPECTOR
        [FoldoutGroup("Vertical"), LabelText("Thumb")]
#endif
        [SerializeField]
        private SpriteRenderer verticalThumb = null;

#if ODIN_INSPECTOR
        [FoldoutGroup("Vertical"), LabelText("Thumb Origin")]
#endif
        [SerializeField]
        private VerticalOrigins verticalThumbOrigin = VerticalOrigins.Top;

        public float Value
        {
            get { return this.value; }
            set { this.value = value; UpdateVisual(); }
        }

        public bool Horizontal
        {
            get { return this.horizontal; }
            set { this.horizontal = value; }
        }

        public HorizontalOrigins HorizontalThumbOrigin
        {
            get { return this.horizontalThumbOrigin; }
            set { this.horizontalThumbOrigin = value; }
        }

        public bool Vertical
        {
            get { return this.vertical; }
            set { this.vertical = value; }
        }

        public VerticalOrigins VerticalThumbOrigin
        {
            get { return this.verticalThumbOrigin; }
            set { this.verticalThumbOrigin = value; }
        }

        private void UpdateVisual()
        {
            UpdateBar();
            UpdateHorizontalThumb();
            UpdateVerticalThumb();
        }

        private void UpdateBar()
        {
            if (!this.bar)
                return;

            var size = this.bar.localScale;

            if (this.horizontal)
            {
                size.x = this.value;
            }

            if (this.vertical)
            {
                size.y = this.value;
            }

            this.bar.localScale = size;
        }

        private void UpdateHorizontalThumb()
        {
            if (!this.horizontal || !this.horizontalThumb)
                return;

            var barSize = this.barSize.x / 2f;
            var thumbSize = GetSize(this.horizontalThumb).x / 2f;
            var newPos = this.horizontalThumb.transform.localPosition;
            var newValue = Mathf.Lerp(-barSize, barSize, this.value) - Mathf.Lerp(-thumbSize, thumbSize, this.value);
            newPos.x = this.horizontalThumbOrigin == HorizontalOrigins.Left ? newValue : -newValue;

            this.horizontalThumb.transform.localPosition = newPos;
        }

        private void UpdateVerticalThumb()
        {
            if (!this.vertical || !this.verticalThumb)
                return;

            var barSize = this.barSize.y / 2f;
            var thumbSize = GetSize(this.verticalThumb).y / 2f;
            var newPos = this.verticalThumb.transform.localPosition;
            var newValue = Mathf.Lerp(-barSize, barSize, this.value) - Mathf.Lerp(-thumbSize, thumbSize, this.value);
            newPos.y = this.verticalThumbOrigin == VerticalOrigins.Bottom ? newValue : -newValue;

            this.verticalThumb.transform.localPosition = newPos;
        }

        private Vector2 GetSize(SpriteRenderer renderer)
        {
            switch (renderer.drawMode)
            {
                case SpriteDrawMode.Sliced:
                case SpriteDrawMode.Tiled:
                    return renderer.size;

                default:
                    return renderer.transform.localScale;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdateVisual();
        }

#endif

        public enum HorizontalOrigins
        {
            Left, Right
        }

        public enum VerticalOrigins
        {
            Top, Bottom
        }
    }
}