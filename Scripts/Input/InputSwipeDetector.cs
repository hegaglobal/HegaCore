using UnityEngine;

namespace HegaCore
{
    public class InputSwipeDetector : MonoBehaviour, IInput<SwipeDirection>
    {
        [SerializeField]
        private DirectionMode mode = DirectionMode.FourDirection;

        [SerializeField]
        private float sensetivity = 10;

        private const SwipeDirection None = (SwipeDirection)(-1);

        private bool waitForSwipe = false;
        private float minMoveDistance = 0.1f;
        private Vector3 startPoint;
        private Vector3 currentPoint;
        private Vector3 offset;
        private SwipeDirection detectedInput = None;

        private void Start()
        {
            UpdateSensetivity();
        }

        private void UpdateSensetivity()
        {
            var screenShortSide = Screen.width < Screen.height ? Screen.width : Screen.height;
            this.minMoveDistance = screenShortSide / this.sensetivity;
        }

        private void Activate()
        {
            SampleSwipeStart();
            this.detectedInput = None;
            this.waitForSwipe = true;
        }

        private void Deactivate()
        {
            SampleSwipeStart();
            this.detectedInput = None;
            this.waitForSwipe = false;
        }

        private void CheckSwipe()
        {
            this.offset = this.startPoint - this.currentPoint;

            if (this.offset.magnitude < this.minMoveDistance)
                return;

            switch (this.mode)
            {
                case DirectionMode.FourDirection:
                    this.detectedInput = DetectFourDirection();
                    break;

                case DirectionMode.EightDirection:
                    this.detectedInput = DetectEightDirection();
                    break;

                case DirectionMode.FourDirectionIsometric:
                    this.detectedInput = DetectFourDirectionIsometric();
                    break;

                default:
                    this.detectedInput = None;
                    break;
            }

            this.waitForSwipe = false;
            SampleSwipeStart();
        }

        private void SampleSwipeStart()
        {
            this.startPoint = this.currentPoint;
            this.offset = Vector3.zero;
        }

        public bool Get(SwipeDirection input)
        {
            this.currentPoint = Input.mousePosition;

            if (!this.waitForSwipe && Input.GetMouseButtonDown(0))
            {
                Activate();
                return false;
            }

            if (this.waitForSwipe && Input.GetMouseButton(0))
            {
                CheckSwipe();
            }

            if (Input.GetMouseButtonUp(0))
            {
                Deactivate();
            }

            return this.detectedInput != None && this.detectedInput == input;
        }

        public void ResetInput()
            => this.detectedInput = None;

        private SwipeDirection DetectFourDirection()
        {
            var angles = Quaternion.FromToRotation(this.offset, Vector3.right).eulerAngles;
            var angle = angles.y + angles.z;

            if ((angle >= 0f && angle < 45f) ||
                (angle >= 315f && angle < 360f))
                return SwipeDirection.Left;

            if (angle >= 45f && angle < 135f)
                return SwipeDirection.Up;

            if (angle >= 135f && angle < 225f)
                return SwipeDirection.Right;

            if (angle >= 225f && angle < 315f)
                return SwipeDirection.Down;

            return None;
        }

        private SwipeDirection DetectEightDirection()
        {
            var angles = Quaternion.FromToRotation(this.offset, Vector3.right).eulerAngles;
            var angle = angles.y + angles.z;

            if ((angle >= 0f && angle < 15f) ||
                (angle >= 345f && angle < 360f))
                return SwipeDirection.Left;

            if (angle >= 15f && angle < 75f)
                return SwipeDirection.UpLeft;

            if (angle >= 75f && angle < 105f)
                return SwipeDirection.Up;

            if (angle >= 105f && angle < 165f)
                return SwipeDirection.UpRight;

            if (angle >= 165f && angle < 195f)
                return SwipeDirection.Right;

            if (angle >= 195f && angle < 255f)
                return SwipeDirection.DownRight;

            if (angle >= 255f && angle < 285f)
                return SwipeDirection.Down;

            if (angle >= 285f && angle < 345f)
                return SwipeDirection.DownLeft;

            return None;
        }

        private SwipeDirection DetectFourDirectionIsometric()
        {
            var angles = Quaternion.FromToRotation(this.offset, Vector3.right).eulerAngles;
            var angle = angles.y + angles.z;

            if (angle >= 15f && angle < 75f)
                return SwipeDirection.UpLeft;

            if (angle >= 105f && angle < 165f)
                return SwipeDirection.UpRight;

            if (angle >= 195f && angle < 255f)
                return SwipeDirection.DownRight;

            if (angle >= 285f && angle < 345f)
                return SwipeDirection.DownLeft;

            return None;
        }

        public enum DirectionMode
        {
            FourDirection,
            EightDirection,
            FourDirectionIsometric
        }
    }
}
