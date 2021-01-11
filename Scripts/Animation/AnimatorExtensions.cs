using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public static class AnimatorExtensions
    {
        public static T RegisterStateEvent<T>(this Animator self, AnimatorStateEventBase @event)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.Register(@event);

            return behaviour;
        }

        public static T RegisterStateEvent<T>(this Animator self, params AnimatorStateEventBase[] events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.Register(events);

            return behaviour;
        }

        public static T RegisterStateEvent<T>(this Animator self, IEnumerable<AnimatorStateEventBase> events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.Register(events);

            return behaviour;
        }

        public static T RemoveStateEvent<T>(this Animator self, AnimatorStateEventBase @event)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.Remove(@event);

            return behaviour;
        }

        public static T RemoveStateEvent<T>(this Animator self, params AnimatorStateEventBase[] events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.Remove(events);

            return behaviour;
        }

        public static T RemoveStateEvent<T>(this Animator self, IEnumerable<AnimatorStateEventBase> events)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.Remove(events);

            return behaviour;
        }

        public static T RemoveAllStateEvents<T>(this Animator self)
            where T : AnimatorStateBehaviour
        {
            if (!self)
                return null;

            var behaviour = self.GetBehaviour<T>();

            if (behaviour)
                behaviour.RemoveAllEvents();

            return behaviour;
        }
    }
}
