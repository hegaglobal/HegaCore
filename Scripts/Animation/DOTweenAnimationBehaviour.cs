using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public abstract class DOTweenAnimationBehaviour : MonoBehaviour
    {
        [field: SerializeField, LabelText(nameof(Duration))]
        public float Duration { get; set; } = 1f;

        [field: SerializeField, LabelText(nameof(Ease))]
        public Ease Ease { get; set; } = Ease.Linear;

        private readonly Dictionary<BaseAnimatorStateEvent, bool> events = new Dictionary<BaseAnimatorStateEvent, bool>();
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

            if (this.tween == null)
                return;

            this.tween.SetEase(this.Ease)
                      .OnPlay(OnPlay)
                      .OnUpdate(OnUpdate)
                      .OnComplete(OnComplete);
        }

        private void OnPlay()
        {
            var list = ListPool<BaseAnimatorStateEvent>.Get();
            list.AddRange(this.events.Keys);

            var normalizedTime = this.tween.ElapsedPercentage(false);

            foreach (var item in list)
            {
                this.events[item] = false;
                item.Enter(normalizedTime);
            }

            ListPool<BaseAnimatorStateEvent>.Return(list);
        }

        private void OnUpdate()
        {
            var list = ListPool<BaseAnimatorStateEvent>.Get();
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

            ListPool<BaseAnimatorStateEvent>.Return(list);
        }

        private void OnComplete()
        {
            var list = ListPool<BaseAnimatorStateEvent>.Get();
            list.AddRange(this.events.Keys);

            var normalizedTime = this.tween.ElapsedPercentage(false);

            foreach (var item in list)
            {
                item.Exit(normalizedTime);
            }

            ListPool<BaseAnimatorStateEvent>.Return(list);
        }

        public void Register(BaseAnimatorStateEvent @event)
        {
            if (@event == null || this.events.ContainsKey(@event))
                return;

            this.events[@event] = false;
        }

        public void Register(params BaseAnimatorStateEvent[] events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Register(@event);
            }
        }

        public void Register(IEnumerable<BaseAnimatorStateEvent> events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Register(@event);
            }
        }

        public void Remove(BaseAnimatorStateEvent @event)
        {
            if (@event == null || !this.events.ContainsKey(@event))
                return;

            this.events.Remove(@event);
        }

        public void Remove(params BaseAnimatorStateEvent[] events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Remove(@event);
            }
        }

        public void Remove(IEnumerable<BaseAnimatorStateEvent> events)
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
    }
}