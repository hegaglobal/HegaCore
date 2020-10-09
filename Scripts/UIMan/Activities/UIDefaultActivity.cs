using System;
using UnuGames;

namespace HegaCore.UI
{
    public static class UIDefaultActivity
    {
        public static float ShowDuration { get; set; } = 0.25f;

        public static float HideDuration { get; set; } = 0.25f;

        private readonly static UIActivity.Settings _settings;

        private static UIActivity _activity;
        private static Action _onCompleted;

        static UIDefaultActivity()
        {
            _settings = UIActivity.Settings.Default.With(false, false, false, false, true, 0f);
        }

        public static void Preload()
            => UIMan.Instance.Preload<UIActivity>(true);

        public static void Show(Action onCompleted = null)
            => Show(true, onCompleted);

        public static void Show(bool autoHide, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            if (autoHide)
                UIMan.Instance.ShowActivity<UIActivity>(ShowDuration, HideDuration, _settings, OnCompleted);
            else
                UIMan.Instance.ShowActivity<UIActivity>(ShowDuration, _settings, OnCompleted);
        }

        private static void OnCompleted(UIActivity sender, params object[] args)
        {
            _onCompleted?.Invoke();
        }

        public static void Show(UIActivityAction onCompleted, params object[] args)
            => Show(true, onCompleted, args);

        public static void Show(bool autoHide, UIActivityAction onCompleted, params object[] args)
        {
            TryInit();

            if (autoHide)
                UIMan.Instance.ShowActivity<UIActivity>(ShowDuration, HideDuration, _settings, onCompleted, args);
            else
                UIMan.Instance.ShowActivity<UIActivity>(ShowDuration, _settings, onCompleted, args);
        }

        public static void Hide()
        {
            TryInit();

            if (_activity)
                _activity.Hide(HideDuration);
        }

        private static void TryInit()
        {
            if (!_activity)
                _activity = UIMan.Instance.GetActivity<UIActivity>();

            _onCompleted = null;
        }
    }
}