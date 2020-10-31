﻿using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnuGames;
using Cysharp.Threading.Tasks;

namespace HegaCore.UI
{
    public static class UIDefaultActivity
    {
        private readonly static UIActivity.Settings _settings;

        private static UIActivity _activity;
        private static Action _onCompleted;

        static UIDefaultActivity()
        {
            _settings = UIActivity.Settings.Default.With(false, false, true, false, true, 0f, 0.5f);
        }

        public static void Preload()
            => UIMan.Instance.Preload<UIActivity>(true);

        public static void Show(bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(autoHide, _settings, OnCompleted);
        }

        public static void Show(float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show(float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show(IEnumerator task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show(IEnumerator task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show(IEnumerator task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show(AsyncOperation task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show(AsyncOperation task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show(AsyncOperation task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show(UnityWebRequest task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show(UnityWebRequest task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show(UnityWebRequest task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show(Func<Task> task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show(Func<Task> task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show(Func<Task> task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show<T>(Func<Task<T>> task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<Task<T>> task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<Task<T>> task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show<T>(Func<Task<T>> task, Action<T> onTaskCompleted, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, onTaskCompleted, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<Task<T>> task, Action<T> onTaskCompleted, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, onTaskCompleted, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<Task<T>> task, Action<T> onTaskCompleted, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, onTaskCompleted, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show(Func<UniTask> task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show(Func<UniTask> task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show(Func<UniTask> task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show<T>(Func<UniTask<T>> task, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<UniTask<T>> task, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<UniTask<T>> task, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, showDuration, hideDuration, _settings, OnCompleted);
        }

        public static void Show<T>(Func<UniTask<T>> task, Action<T> onTaskCompleted, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, onTaskCompleted, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<UniTask<T>> task, Action<T> onTaskCompleted, float showDuration, bool autoHide = false, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, onTaskCompleted, showDuration, autoHide, _settings, OnCompleted);
        }

        public static void Show<T>(Func<UniTask<T>> task, Action<T> onTaskCompleted, float showDuration, float hideDuration, Action onCompleted = null)
        {
            TryInit();
            _onCompleted = onCompleted;

            UIMan.Instance.ShowActivity<UIActivity, T>(task, onTaskCompleted, showDuration, hideDuration, _settings, OnCompleted);
        }

        private static void OnCompleted(UIActivity sender, params object[] args)
            => _onCompleted?.Invoke();

        public static void Hide()
        {
            TryInit();

            if (_activity)
                _activity.Hide();
        }

        private static void TryInit()
        {
            if (!_activity)
                _activity = UIMan.Instance.GetActivity<UIActivity>();

            _onCompleted = null;
        }
    }
}