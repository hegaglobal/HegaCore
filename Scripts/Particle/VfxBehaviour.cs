using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public class VfxBehaviour : MonoBehaviour, IAlive, ISetActive, IPlayState, IOnUpdate
    {
        [SerializeField, Required]
        private Component target = null;

        [SerializeField]
        private bool enableEvents = false;

        [SerializeField, ShowIf(nameof(enableEvents))]
        private UnityEvent onPlay = new UnityEvent();

        [SerializeField, ShowIf(nameof(enableEvents))]
        private UnityEvent onPause = new UnityEvent();

        [SerializeField, ShowIf(nameof(enableEvents))]
        private UnityEvent onStop = new UnityEvent();

        public Component Target => this.target;

        public bool Alive
        {
            get
            {
                if (this.targetAlive != null)
                    return this.targetAlive.Alive;

                if (this.target)
                    return this.targetGameObject.activeInHierarchy;

                return this.m_gameObject.activeInHierarchy;
            }
        }

        public PlayState PlayState { get; private set; }

        public UnityEvent OnPlay => this.onPlay;

        public UnityEvent OnPause => this.onPause;

        public UnityEvent OnStop => this.onStop;

        private GameObject m_gameObject;
        private Transform m_transform;

        private GameObject targetGameObject;
        private IAlive targetAlive;
        private ISetActive targetSetActive;
        private IPlayState targetPlayState;
        private IOnUpdate targetOnUpdate;

        private void Awake()
        {
            this.m_gameObject = this.gameObject;
            this.m_transform = this.transform;

            if (this.target)
                this.targetGameObject = this.target.gameObject;
            else
                this.targetGameObject = this.m_gameObject;

            this.targetAlive = this.target as IAlive;
            this.targetSetActive = this.target as ISetActive;
            this.targetPlayState = this.target as IPlayState;
            this.targetOnUpdate = this.target as IOnUpdate;
        }

        public virtual void Set(in Vector3 position)
        {
            this.m_transform.position = position;
        }

        public void Play()
        {
            if (!this.Alive)
                return;

            this.targetPlayState?.SetPlayState(PlayState.Running);

            if (this.enableEvents)
                this.onPlay.Invoke();
        }

        public void Pause()
        {
            if (!this.Alive)
                return;

            this.targetPlayState?.SetPlayState(PlayState.Paused);

            if (this.enableEvents)
                this.onPause.Invoke();
        }

        public void Stop()
        {
            if (!this.Alive)
                return;

            this.targetPlayState?.SetPlayState(PlayState.Stopped);

            if (this.enableEvents)
                this.onStop.Invoke();
        }

        public void SetActive(bool value)
        {
            this.targetSetActive?.SetActive(value);
            this.m_gameObject.SetActive(value);
        }

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

        public void OnUpdate(float deltaTime)
        {
            if (this.Alive)
                this.targetOnUpdate?.OnUpdate(deltaTime);
        }
    }
}