using System.Threading;

namespace HegaCore
{
    public static class CancellationTokenSourceExtensions
    {
        public static CancellationTokenSource Renew(this CancellationTokenSource self)
        {
            self?.Dispose();
            return new CancellationTokenSource();
        }
    }
}