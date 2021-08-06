using System.Collections.Generic;
using System.Collections.Pooling;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract class DOTweenAnimationBehaviour : MonoBehaviour, IPlayState
    {
        [field: SerializeField, LabelText(nameof(Duration))]
        public float Duration { get; set; } = 1f;

        [field: SerializeField, LabelText(nameof(Ease))]
        public Ease Ease { get; set; } = Ease.Linear;

        public PlayState PlayState { get; private set; }

        private readonly Dictionary<AnimatorStateEventBase, bool> events = new Dictionary<AnimatorStateEventBase, bool>();
        private Tweener tween;

        public void Play()
            => this.tween?.Play();

        public void Pause()
            => this.tween?.Pause();

        public void Stop()
            => this.tween?.Kill();

        protected void Setup(Tweener value)
        {
            this.tween?.Kill();
            this.tween = value;

            this.tween?.SetEase(this.Ease)
                       .OnPlay(OnPlay)
                       .OnUpdate(OnUpdate)
                       .OnComplete(OnComplete);
        }

        private void OnPlay()
        {
            var list = Pool.Provider.List<AnimatorStateEventBase>();
            list.AddRange(this.events.Keys);

            var normalizedTime = this.tween.ElapsedPercentage(false);

            foreach (var item in list)
            {
                this.events[item] = false;
                item.Enter(normalizedTime);
            }

            Pool.Provider.Return(list);
        }

        private void OnUpdate()
        {
            var list = Pool.Provider.List<AnimatorStateEventBase>();
            list.AddRange(this.events.Keys);

            var normalizedTime = this.tween.ElapsedPercentage(false);

            foreach (var item in list)
            {
                if (!this.events.TryGetValue(item, out var invoked))
                    continue;

                item.Update(normalizedTime);

                if (invoked || normalizedTime < item.InvokeTime)
                    continue;

                this.events[item] = true;
                item.Invoke(normalizedTime);
            }

            Pool.Provider.Return(list);
        }

        private void OnComplete()
        {
            var list = Pool.Provider.List<AnimatorStateEventBase>();
            list.AddRange(this.events.Keys);

            var normalizedTime = this.tween.ElapsedPercentage(false);

            foreach (var item in list)
            {
                item.Exit(normalizedTime);
            }

            Pool.Provider.Return(list);
        }

        public void Register(AnimatorStateEventBase @event)
        {
            if (@event == null || this.events.ContainsKey(@event))
                return;

            this.events[@event] = false;
        }

        public void Register(params AnimatorStateEventBase[] events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Register(@event);
            }
        }

        public void Register(IEnumerable<AnimatorStateEventBase> events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Register(@event);
            }
        }

        public void Remove(AnimatorStateEventBase @event)
        {
            if (@event == null || !this.events.ContainsKey(@event))
                return;

            this.events.Remove(@event);
        }

        public void Remove(params AnimatorStateEventBase[] events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Remove(@event);
            }
        }

        public void Remove(IEnumerable<AnimatorStateEventBase> events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Remove(@event);
            }
        }

        public void RemoveAllEvents()
            => this.events.Clear();

        public void SetPlayState(PlayState value)
        {
            this.PlayState = value;

            switch (value)
            {
                case PlayState.Paused: Pause(); break;
                case PlayState.Running: Play(); break;
                case PlayState.Stopped: Stop(); break;
            }
        }
    }
}