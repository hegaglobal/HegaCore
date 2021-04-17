using System.Collections.ArrayBased;
using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class AnimatorStateBehaviour : StateMachineBehaviour
    {
        private readonly ArrayDictionary<AnimatorStateEventBase, bool> events = new ArrayDictionary<AnimatorStateEventBase, bool>();

        public void Register(AnimatorStateEventBase @event)
        {
            if (@event == null || this.events.ContainsKey(@event))
                return;

            this.events[@event] = false;
        }

        public void Register(params AnimatorStateEventBase[] events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Register(@event);
            }
        }

        public void Register(IEnumerable<AnimatorStateEventBase> events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Register(@event);
            }
        }

        public void Remove(AnimatorStateEventBase @event)
        {
            if (@event == null || !this.events.ContainsKey(@event))
                return;

            this.events.Remove(@event);
        }

        public void Remove(params AnimatorStateEventBase[] events)
        {
            if (events == null)
                return;

            foreach (var @event in events)
            {
                Remove(@event);
            }
        }

        public void Remove(IEnumerable<AnimatorStateEventBase> events)
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
            this.events.GetUnsafeKeys(out var list, out var count);

            for (var i = 0u; i < count; i++)
            {
                var item = list[i].Key;
                this.events[item] = false;
                item.Enter(stateInfo.normalizedTime);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.events.GetUnsafeKeys(out var list, out var count);

            for (var i = 0u; i < count; i++)
            {
                var item = list[i].Key;
                item.Exit(stateInfo.normalizedTime);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.events.GetUnsafeKeys(out var list, out var count);

            for (var i = 0u; i < count; i++)
            {
                var item = list[i].Key;
                var invoked = this.events[item];

                item.Update(stateInfo.normalizedTime);

                if (invoked || stateInfo.normalizedTime < item.InvokeTime)
                    continue;

                this.events[item] = true;
                item.Invoke(stateInfo.normalizedTime);
            }
        }
    }
}