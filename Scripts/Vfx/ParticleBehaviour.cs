using UnityEngine;

namespace HegaCore
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleBehaviour : MonoBehaviour, IAlive, ISetActive, IPlayState
    {
        public ParticleSystem Particle { get; private set; }

        public bool Alive => this.Particle && this.Particle.IsAlive(true);

        public PlayState PlayState { get; private set; }

        private GameObject m_gameObject;
        private Transform m_transform;

        protected virtual void Awake()
        {
            this.m_gameObject = this.gameObject;
            this.m_transform = this.transform;
            this.Particle = GetComponent<ParticleSystem>();
        }

        public virtual void Set(in Vector3 position)
        {
            this.m_transform.position = position;
        }

        public void Play()
            => this.Particle.Play(true);

        public void Pause()
            => this.Particle.Pause(true);

        public void Stop()
            => this.Particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        public void SetActive(bool value)
            => this.m_gameObject.SetActive(value);

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