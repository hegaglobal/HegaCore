namespace HegaCore
{
    public static class CoreHelper
    {
        public static void Initialize()
        {
            System.Collections.Generic.Pool.Set<PoolProvider>();
        }
    }
}