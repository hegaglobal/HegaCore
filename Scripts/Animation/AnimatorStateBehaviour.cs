using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class AnimatorStateBehaviour : StateMachineBehaviour
    {
        private readonly Dictionary<BaseAnimatorStateEvent, bool> events = new Dictionary<BaseAnimatorStateEvent, bool>();

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

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = PoolProvider.List<BaseAnimatorStateEvent>();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                this.events[item] = false;
                item.Enter(stateInfo.normalizedTime);
            }

            PoolProvider.Return(list);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = PoolProvider.List<BaseAnimatorStateEvent>();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                item.Exit(stateInfo.normalizedTime);
            }

            PoolProvider.Return(list);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = PoolProvider.List<BaseAnimatorStateEvent>();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                var invoked = this.events[item];

                item.Update(stateInfo.normalizedTime);

                if (invoked || stateInfo.normalizedTime < item.InvokeTime)
                    continue;

                this.events[item] = true;
                item.Invoke(stateInfo.normalizedTime);
            }

            PoolProvider.Return(list);
        }
    }
}