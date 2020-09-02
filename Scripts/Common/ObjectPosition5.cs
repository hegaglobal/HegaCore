using UnityEngine;

namespace HegaCore
{
    public class ObjectPosition5 : MonoBehaviour, IPosition5
    {
        [SerializeField]
        private Transform above = null;

        [SerializeField]
        private Transform below = null;

        [SerializeField]
        private Transform ahead = null;

        [SerializeField]
        private Transform behind = null;

        [SerializeField]
        private Transform center = null;

        public Vector3 Above => this.above ? this.above.position : this.transform.position;

        public Vector3 Below => this.below ? this.below.position : this.transform.position;

        public Vector3 Ahead => this.ahead ? this.ahead.position : this.transform.position;

        public Vector3 Behind => this.behind ? this.behind.position : this.transform.position;

        public Vector3 Center => this.center ? this.center.position : this.transform.position;

        public static implicit operator Position5(ObjectPosition5 value)
            => value ? new Position5(value.Above, value.Below, value.Ahead, value.Behind, value.Center) : Position5.Zero;
    }
}