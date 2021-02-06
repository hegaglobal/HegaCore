using UnityEngine;
using DG.Tweening;

namespace HegaCore
{
    public class DOTweenLocalMoveBehaviour : DOTweenAnimationBehaviour
    {
        public void Move(in Vector3 position)
            => Setup(this.transform.DOLocalMove(position, this.Duration));
    }
}