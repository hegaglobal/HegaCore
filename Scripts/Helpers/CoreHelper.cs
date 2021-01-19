namespace HegaCore
{
    public static class CoreHelper
    {
        public static void Initialize()
        {
            System.Collections.Pooling.Pool.Set<PoolProvider>();
        }
    }
}