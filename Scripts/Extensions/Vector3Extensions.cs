using UnityEngine;

namespace HegaCore
{
    public static class Vector3Extensions
    {
        public static Vector3 ZSortingOrder(in this Vector3 self, int order)
        {
            var z = Mathf.Abs(self.y) * order;
            return self.With(z: z);
        }
    }
}