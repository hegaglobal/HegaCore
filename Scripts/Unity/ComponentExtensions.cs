namespace UnityEngine
{
    public static class ComponentExtensions
    {
        public static T AddComponent<T>(this Component self) where T : Component
            => self.gameObject.AddComponent<T>();

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            if (!self.TryGetComponent<T>(out var component))
                component = self.AddComponent<T>();

            return component;
        }

        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            if (!self.TryGetComponent<T>(out var component))
                component = self.gameObject.AddComponent<T>();

            return component;
        }

        public static T GetOrAddComponentInChildren<T>(this GameObject self) where T : Component
        {
            var component = self.GetComponentInChildren<T>();

            if (!component)
                component = self.AddComponent<T>();

            return component;
        }

        public static T GetOrAddComponentInChildren<T>(this Component self) where T : Component
        {
            var component = self.GetComponentInChildren<T>();

            if (!component)
                component = self.gameObject.AddComponent<T>();

            return component;
        }

        public static bool Validate(this Component self)
            => self && self.gameObject;
    }
}