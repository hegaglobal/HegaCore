#if ANIMATION_SEQUENCER

using System;
using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using UnityEngine;

namespace HegaCore
{
    [DisallowMultipleComponent]
    public class ManualAnimationSequencerController : MonoBehaviour, IOnUpdate
    {
        public enum InitializeMode
        {
            None,
            PrepareToPlayOnAwake,
            PlayOnAwake
        }

        [SerializeReference]
        private AnimationStepBase[] animationSteps = new AnimationStepBase[0];

        public AnimationStepBase[] AnimationSteps => this.animationSteps;

        [SerializeField]
        private float duration;

        public float Duration => this.duration;

        [SerializeField]
        private InitializeMode initializeMode = InitializeMode.PlayOnAwake;

        private bool isPlaying;

        public bool IsPlaying => this.isPlaying;

        private readonly List<AnimationStepBase> stepsToBePlayed = new List<AnimationStepBase>();
        private readonly List<AnimationStepBase> stepsQueue = new List<AnimationStepBase>();
        private bool preparedToPlay;

        public event Action OnSequenceFinishedPlayingEvent;

        public event Action<int> OnAnimationStepBeginEvent;
        public event Action<int> OnAnimationStepFinishedEvent;

        private void Awake()
        {
            if (this.initializeMode == InitializeMode.PlayOnAwake || this.initializeMode == InitializeMode.PrepareToPlayOnAwake)
            {
                PrepareForPlay();
                if (this.initializeMode == InitializeMode.PlayOnAwake)
                    Play();
            }
        }

        public void Play()
        {
            PrepareForPlay();
            this.isPlaying = true;
        }

        public void Rewind()
        {
            for (var i = 0; i < this.animationSteps.Length; ++i)
            {
                this.animationSteps[i].Rewind();
            }
        }

        public void Complete()
        {
            if (!this.isPlaying)
                return;

            for (var i = 0; i < this.animationSteps.Length; i++)
            {
                AnimationStepBase animationStepBase = this.animationSteps[i];
                if (animationStepBase.IsComplete)
                    continue;

                animationStepBase.Complete();
            }
            AnimationFinished();
        }

        public void Stop()
        {
            if (!this.isPlaying)
                return;

            this.isPlaying = false;
            this.preparedToPlay = false;
            for (var i = 0; i < this.animationSteps.Length; i++)
                this.animationSteps[i].Stop();
        }

        public IEnumerator PlayEnumerator()
        {
            Play();
            while (this.isPlaying)
                yield return null;
        }

        public void PrepareForPlay(bool force = false)
        {
            if (!force && this.preparedToPlay)
                return;

            for (var i = 0; i < this.animationSteps.Length; i++)
                this.animationSteps[i].PrepareForPlay();

            this.stepsQueue.AddRange(this.animationSteps);
            this.preparedToPlay = true;
        }

        private void AnimationFinished()
        {
            Stop();
            OnSequenceFinishedPlayingEvent?.Invoke();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!this.isPlaying)
                return;

            for (var i = this.stepsToBePlayed.Count - 1; i >= 0; i--)
            {
                AnimationStepBase animationStepBase = this.stepsToBePlayed[i];
                animationStepBase.UpdateStep(deltaTime);

                if (animationStepBase.IsWaitingOnDelay)
                    continue;

                if (!animationStepBase.IsPlaying)
                {
                    animationStepBase.Play();
                    DispatchOnStepBeginToPlay(animationStepBase);
                }
                else
                {
                    if (!animationStepBase.IsComplete)
                        continue;

                    animationStepBase.StepFinished();
                    this.stepsToBePlayed.Remove(animationStepBase);
                    DispatchOnStepFinished(animationStepBase);
                }
            }

            if (this.stepsToBePlayed.Count == 0)
            {
                if (this.stepsQueue.Count == 0)
                {
                    AnimationFinished();
                }
                else
                {
                    UpdateNextSteps();
                }
            }
        }

        public void UpdateStep(float deltaTime)
        {
            if (!this.isPlaying)
                return;

            for (var i = this.stepsToBePlayed.Count - 1; i >= 0; i--)
            {
                AnimationStepBase animationStepBase = this.stepsToBePlayed[i];
                animationStepBase.UpdateStep(deltaTime);

                if (animationStepBase.IsWaitingOnDelay)
                    continue;

                if (!animationStepBase.IsPlaying)
                {
                    animationStepBase.Play();
                    DispatchOnStepBeginToPlay(animationStepBase);
                }
                else
                {
                    if (!animationStepBase.IsComplete)
                        continue;

                    animationStepBase.StepFinished();
                    this.stepsToBePlayed.Remove(animationStepBase);
                    DispatchOnStepFinished(animationStepBase);
                }
            }

            if (this.stepsToBePlayed.Count == 0)
            {
                if (this.stepsQueue.Count == 0)
                {
                    AnimationFinished();
                }
                else
                {
                    UpdateNextSteps();
                }
            }
        }

        private void DispatchOnStepBeginToPlay(AnimationStepBase animationStepBase)
        {
            var index = Array.IndexOf(this.animationSteps, animationStepBase);
            if (index == -1)
                return;

            OnAnimationStepBeginEvent?.Invoke(index);
        }

        private void DispatchOnStepFinished(AnimationStepBase animationStepBase)
        {
            var index = Array.IndexOf(this.animationSteps, animationStepBase);
            if (index == -1)
                return;

            OnAnimationStepFinishedEvent?.Invoke(index);
        }

        private void UpdateNextSteps()
        {
            for (var i = 0; i < this.stepsQueue.Count; i++)
            {
                AnimationStepBase possibleStepToPlay = this.stepsQueue[i];
                if (possibleStepToPlay.FlowType == FlowType.Append && this.stepsToBePlayed.Count == 0
                    || possibleStepToPlay.FlowType == FlowType.Join && this.stepsToBePlayed.Count > 0)
                {
                    this.stepsToBePlayed.Add(possibleStepToPlay);
                }
                else
                    break;
            }

            for (var i = 0; i < this.stepsToBePlayed.Count; i++)
            {
                AnimationStepBase animationStepBase = this.stepsToBePlayed[i];
                animationStepBase.WillBePlayed();
                this.stepsQueue.Remove(animationStepBase);
            }
        }

        public List<T> GetStepsOfType<T>() where T : AnimationStepBase
        {
            var results = new List<T>();
            for (var i = 0; i < this.animationSteps.Length; i++)
            {
                if (this.animationSteps[i] is T castedStep)
                    results.Add(castedStep);
            }

            return results;
        }

        public List<DOTweenActionBase> GetDOTweenActionsThatUseComponent<T>() where T : Component
        {
            List<DOTweenAnimationStep> dotweenSteps = GetStepsOfType<DOTweenAnimationStep>();
            var results = new List<DOTweenActionBase>();
            for (var i = 0; i < dotweenSteps.Count; i++)
            {
                DOTweenAnimationStep doTweenAnimationStep = dotweenSteps[i];
                for (var j = 0; j < doTweenAnimationStep.Actions.Length; j++)
                {
                    DOTweenActionBase actionBase = doTweenAnimationStep.Actions[j];
                    if (actionBase.TargetComponentType == typeof(T))
                        results.Add(actionBase);
                }
            }

            return results;
        }
    }
}

#endif