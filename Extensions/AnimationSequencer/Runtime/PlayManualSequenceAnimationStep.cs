using System;
using UnityEngine;
using BrunoMikoski.AnimationSequencer;

namespace HegaCore
{
    [Serializable]
    public sealed class PlayManualSequenceAnimationStep : AnimationStepBase
    {
        public override string DisplayName => "Play Sequence Manual";

        [SerializeField]
        private ManualAnimationSequencerController sequencer;

        public override float Duration
        {
            get
            {
                if (this.sequencer == null)
                    return 0;
                return this.sequencer.Duration;
            }
        }

        public ManualAnimationSequencerController Target => this.sequencer;

        public override bool CanBePlayed()
        {
            return this.sequencer != null;
        }

        public override void PrepareForPlay()
        {
            base.PrepareForPlay();
            this.sequencer.PrepareForPlay();
        }


        public override void Play()
        {
            base.Play();
            this.sequencer.Play();
        }

        public override string GetDisplayNameForEditor(int index)
        {
            var display = "NULL";
            if (this.sequencer != null)
                display = this.sequencer.name;
            return $"{index}. Play {display} Sequence";
        }

        public void SetTarget(ManualAnimationSequencerController newTarget)
        {
            this.sequencer = newTarget;
        }

        public override void Complete()
        {
            this.sequencer.Complete();
        }
    }
}
