using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public static class AnimatorExtensions
    {
        public static T[] RegisterStateEvent<T>(this Animator self, AnimatorStateEventBase @event)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.Register(@event);
            }

            return behaviours;
        }

        public static T[] RegisterStateEvent<T>(this Animator self, params AnimatorStateEventBase[] events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.Register(events);
            }

            return behaviours;
        }

        public static T[] RegisterStateEvent<T>(this Animator self, IEnumerable<AnimatorStateEventBase> events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.Register(events);
            }

            return behaviours;
        }

        public static T[] RemoveStateEvent<T>(this Animator self, AnimatorStateEventBase @event)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.Remove(@event);
            }

            return behaviours;
        }

        public static T[] RemoveStateEvent<T>(this Animator self, params AnimatorStateEventBase[] events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.Remove(events);
            }

            return behaviours;
        }

        public static T[] RemoveStateEvent<T>(this Animator self, IEnumerable<AnimatorStateEventBase> events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.Remove(events);
            }

            return behaviours;
        }

        public static T[] RemoveAllStateEvents<T>(this Animator self)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviours = self.GetBehaviours<T>();

            foreach (var behaviour in behaviours)
            {
                behaviour.RemoveAllEvents();
            }

            return behaviours;
        }
    }
}
