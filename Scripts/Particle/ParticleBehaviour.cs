using UnityEngine;

namespace HegaCore
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleBehaviour : MonoBehaviour, IAlive, IPlayState
    {
        public ParticleSystem Particle { get; private set; }

        public bool Alive => this.Particle && this.Particle.IsAlive(true);

        public PlayState PlayState { get; private set; }

        protected virtual void Awake()
        {
            this.Particle = GetComponent<ParticleSystem>();
        }

        public virtual void Initialize(in Vector3 position)
        {
            this.transform.position = position;
        }

        public void Play()
            => this.Particle.Play(true);

        public void Pause()
            => this.Particle.Pause(true);

        public void Stop()
            => this.Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

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