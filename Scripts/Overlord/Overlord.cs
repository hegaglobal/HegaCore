using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public abstract class Overlord<T> : SingletonBehaviour<T> where T : Overlord<T>
    {
        [SerializeField]
        private float tempo = 60f;

        private bool alive;

        private readonly List<OverlordTask> tasks = new List<OverlordTask>();

        private float runTempo;
        private bool canRunTasks;
        private float elapsed;
        private int current;

        private void Awake()
        {
            this.alive = false;
            this.canRunTasks = false;
        }

        public void Initialize(bool alive, float? tempo = null)
        {
            this.alive = alive;
            this.canRunTasks = false;
            this.runTempo = tempo ?? this.tempo;
            this.tasks.Clear();

            if (!this.alive)
                return;

            OnInitialize();
            RunTasks();
        }

        protected abstract void OnInitialize();

        private void RunTasks()
        {
            UnuLogger.Log($"Run {GetType().Name} tasks...");

            this.current = 0;
            this.elapsed = this.runTempo;
            this.canRunTasks = true;
        }

        private void Update()
        {
            if (!this.canRunTasks)
                return;

            this.elapsed += GameTime.Provider.DeltaTime;

            if (this.elapsed < this.runTempo)
                return;

            this.elapsed = 0f;

            if (this.current >= this.tasks.Count)
                return;

            this.tasks[this.current].Run();
            this.current += 1;
        }

        protected void Register(params OverlordTask[] tasks)
        {
            foreach (var daemon in tasks)
            {
                if (daemon != null)
                    this.tasks.Add(daemon);
            }
        }
    }
}