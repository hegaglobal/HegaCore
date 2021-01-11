using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public class AnimatorStateBehaviour : StateMachineBehaviour
    {
        private readonly Dictionary<AnimatorStateEventBase, bool> events = new Dictionary<AnimatorStateEventBase, bool>();

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
            var list = Pool.Provider.List<AnimatorStateEventBase>();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                this.events[item] = false;
                item.Enter(stateInfo.normalizedTime);
            }

            Pool.Provider.Return(list);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = Pool.Provider.List<AnimatorStateEventBase>();
            list.AddRange(this.events.Keys);

            foreach (var item in list)
            {
                item.Exit(stateInfo.normalizedTime);
            }

            Pool.Provider.Return(list);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var list = Pool.Provider.List<AnimatorStateEventBase>();
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

            Pool.Provider.Return(list);
        }
    }
}