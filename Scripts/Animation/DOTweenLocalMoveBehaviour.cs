using UnityEngine;
using DG.Tweening;

namespace HegaCore
{
    public class DOTweenLocalMoveBehaviour : DOTweenAnimationBehaviour
    {
        public void Move(Vector3 position)
            => Setup(this.transform.DOLocalMove(position, this.Duration));
    }
}