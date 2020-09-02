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
            var list = ListPool<BaseAnimatorStateEvent>.Get();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                if (item == null || !this.events.ContainsKey(item))
                    continue;

                this.events[item] = false;
                item.Enter(stateInfo);
            }

            ListPool<BaseAnimatorStateEvent>.Return(list);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = ListPool<BaseAnimatorStateEvent>.Get();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                if (item == null || !this.events.ContainsKey(item))
                    continue;

                item.Exit(stateInfo);
            }

            ListPool<BaseAnimatorStateEvent>.Return(list);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = ListPool<BaseAnimatorStateEvent>.Get();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                if (item == null || !this.events.TryGetValue(item, out var invoked))
                    continue;

                item.Update(stateInfo);

                if (invoked || stateInfo.normalizedTime < item.InvokeTime)
                    continue;

                this.events[item] = true;
                item.Invoke(stateInfo);
            }

            ListPool<BaseAnimatorStateEvent>.Return(list);
        }
    }
}