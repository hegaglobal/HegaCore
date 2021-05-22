namespace HegaCore
{
    public static class ObjectExtensions
    {
        public static bool Is<T>(this object self)
            => self != null && self is T;

        public static bool Is<T>(this UnityEngine.Object self)
            => self && self is T;

        public static bool TryAs<T>(this object self, out T result)
        {
            if (self != null && self is T m_result)
            {
                result = m_result;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryAs<T, TAs>(this T self, out TAs result)
        {
            if (self != null && self is TAs m_result)
            {
                result = m_result;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryAs<T>(this UnityEngine.Object self, out T result)
        {
            if (self && self is T m_result)
            {
                result = m_result;
                return true;
            }

            result = default;
            return false;
        }
    }
}