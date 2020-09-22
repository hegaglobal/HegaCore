using UnityEngine;
using DG.Tweening;

namespace HegaCore
{
    public class DOTweenMoveBehaviour : DOTweenAnimationBehaviour
    {
        public void Move(Vector3 position)
            => Setup(this.transform.DOMove(position, this.Duration));
    }
}