using UnityEngine;
using DG.Tweening;

namespace HegaCore
{
    public class DOTweenMoveBehaviour : DOTweenAnimationBehaviour
    {
        public void Move(in Vector3 position)
            => Setup(this.transform.DOMove(position, this.Duration));
    }
}