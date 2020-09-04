namespace UnityEngine
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject self)
               where T : Component
        {
            if (!self.TryGetComponent<T>(out var component))
                component = self.AddComponent<T>();

            return component;
        }

        public static T GetOrAddComponent<T>(this Component self)
               where T : Component
        {
            if (!self.TryGetComponent<T>(out var component))
                component = self.gameObject.AddComponent<T>();

            return component;
        }

        public static bool Validate(this Component self)
            => self && self.gameObject;
    }
}