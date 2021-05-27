using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public class VfxBehaviour : MonoBehaviour, IAlive, ISetActive, IPlayState, IOnUpdate
    {
        [SerializeField, Required]
        private Component target = null;

        [VerticalGroup("Play", PaddingTop = 10)]
        [BoxGroup("Play/On Play")]
        [SerializeField, LabelText("Enable")]
        private bool enableOnPlay = false;

        [BoxGroup("Play/On Play")]
        [SerializeField, ShowIf(nameof(enableOnPlay)), HideLabel]
        private UnityEvent onPlay = new UnityEvent();

        [VerticalGroup("Pause", PaddingTop = 10)]
        [BoxGroup("Pause/On Pause")]
        [SerializeField, LabelText("Enable")]
        private bool enableOnPause = false;

        [BoxGroup("Pause/On Pause")]
        [SerializeField, ShowIf(nameof(enableOnPause)), HideLabel]
        private UnityEvent onPause = new UnityEvent();

        [VerticalGroup("Stop", PaddingTop = 10)]
        [BoxGroup("Stop/On Stop")]
        [SerializeField, LabelText("Enable")]
        private bool enableOnStop = false;

        [BoxGroup("Stop/On Stop")]
        [SerializeField, ShowIf(nameof(enableOnStop)), HideLabel]
        private UnityEvent onStop = new UnityEvent();

        [VerticalGroup("Update", PaddingTop = 10)]
        [BoxGroup("Update/On Update Step")]
        [SerializeField, LabelText("Enable")]
        private bool enableOnUpdateStep = false;

        [BoxGroup("Update/On Update Step")]
        [SerializeField, ShowIf(nameof(enableOnUpdateStep)), HideLabel]
        private UnityEventFloat onUpdateStep = new UnityEventFloat();

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

        public UnityEvent<float> OnUpdateStep => this.onUpdateStep;

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

            if (this.enableOnPlay)
                this.onPlay.Invoke();
        }

        public void Pause()
        {
            if (!this.Alive)
                return;

            this.targetPlayState?.SetPlayState(PlayState.Paused);

            if (this.enableOnPause)
                this.onPause.Invoke();
        }

        public void Stop()
        {
            if (!this.Alive)
                return;

            this.targetPlayState?.SetPlayState(PlayState.Stopped);

            if (this.enableOnStop)
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
            if (!this.Alive)
                return;

            this.targetOnUpdate?.OnUpdate(deltaTime);

            if (this.enableOnUpdateStep)
                this.onUpdateStep.Invoke(deltaTime);
        }

        [Serializable]
        private class UnityEventFloat : UnityEvent<float> { }
    }
}