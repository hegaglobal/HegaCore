using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
    public static partial class AsyncOnDisableExtensions
    {
        public static AsyncOnDisableTrigger GetAsyncOnDisableTrigger(this GameObject self)
            => self.GetOrAddComponent<AsyncOnDisableTrigger>();

        public static AsyncOnDisableTrigger GetAsyncOnDisableTrigger(this Component self)
            => GetAsyncOnDisableTrigger(self.gameObject);
    }

    [DisallowMultipleComponent]
    public sealed class AsyncOnDisableTrigger : MonoBehaviour
    {
        private bool awakeCalled = false;
        private bool called = false;
        private CancellationTokenSource cts;

        public CancellationToken CancellationToken
        {
            get
            {
                if (this.cts == null)
                    this.cts = new CancellationTokenSource();

                if (!this.awakeCalled)
                    PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, new AwakeMonitor(this));

                return this.cts.Token;
            }
        }

        private void Awake()
        {
            this.awakeCalled = true;
        }

        private void OnEnable()
        {
            this.called = false;
        }

        private void OnDisable()
        {
            this.called = true;
            this.cts?.Cancel();
            this.cts?.Dispose();
            this.cts = null;
        }

        public UniTask OnDisableAsync()
        {
            if (this.called)
            {
                this.called = false;
                return UniTask.CompletedTask;
            }

            var tcs = new UniTaskCompletionSource();

            // OnDisable = Called Cancel.
            this.CancellationToken.RegisterWithoutCaptureExecutionContext(state => {
                var tcs2 = (UniTaskCompletionSource)state;
                tcs2.TrySetResult();
            }, tcs);

            return tcs.Task;
        }

        private class AwakeMonitor : IPlayerLoopItem
        {
            private readonly AsyncOnDisableTrigger trigger;

            public AwakeMonitor(AsyncOnDisableTrigger trigger)
            {
                this.trigger = trigger;
            }

            public bool MoveNext()
            {
                if (this.trigger.called) return false;
                if (this.trigger == null)
                {
                    this.trigger.OnDisable();
                    return false;
                }
                return true;
            }
        }
    }

    public static partial class OnDisableAsyncTriggerExtensions
    {
        /// <summary>This function is called when the MonoBehaviour will be disabed.</summary>
        public static UniTask OnDisableAsync(this GameObject gameObject)
            => gameObject.GetAsyncOnDisableTrigger().OnDisableAsync();

        /// <summary>This function is called when the MonoBehaviour will be disabed.</summary>
        public static UniTask OnDisableAsync(this Component component)
            => component.GetAsyncOnDisableTrigger().OnDisableAsync();
    }
}

namespace Cysharp.Threading.Tasks
{
    using Triggers;

    public static class OnDisableCancellationExtensions
    {
        /// <summary>This CancellationToken is canceled when the MonoBehaviour will be disabled.</summary>
        public static CancellationToken GetCancellationTokenOnDisable(this GameObject gameObject)
            => gameObject.GetAsyncOnDisableTrigger().CancellationToken;

        /// <summary>This CancellationToken is canceled when the MonoBehaviour will be disabled.</summary>
        public static CancellationToken GetCancellationTokenOnDisable(this Component component)
            => component.GetAsyncOnDisableTrigger().CancellationToken;
    }
}