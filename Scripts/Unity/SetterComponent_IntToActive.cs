using UnityEngine;

namespace HegaCore
{
    public sealed class SetterComponent_IntToActive : SetterComponentInt
    {
        [SerializeField]
        private int value = default;

        [SerializeField]
        private GameObject[] activeOnTrue;

        [SerializeField]
        private GameObject[] inactiveOnTrue;

        public override void Set(int value)
        {
            var val = this.value == value;

            if (this.activeOnTrue != null && this.activeOnTrue.Length > 0)
            {
                for (var i = 0; i < this.activeOnTrue.Length; i++)
                {
                    if (this.activeOnTrue[i])
                        this.activeOnTrue[i].SetActive(val);
                }
            }

            if (this.inactiveOnTrue != null && this.inactiveOnTrue.Length > 0)
            {
                for (var i = 0; i < this.inactiveOnTrue.Length; i++)
                {
                    if (this.inactiveOnTrue[i])
                        this.inactiveOnTrue[i].SetActive(!val);
                }
            }
        }
    }
}