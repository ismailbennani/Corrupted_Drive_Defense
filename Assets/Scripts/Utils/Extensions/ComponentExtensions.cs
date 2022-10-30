using UnityEngine;

namespace Utils.Extensions
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInSelfOrChildren<T>(this Component c, out T component) where T: Component
        {
            if (c.TryGetComponent(out component))
            {
                return true;
            }

            if (c.TryGetComponentInChildren(out component))
            {
                return true;
            }

            component = null;
            return false;
        }
        
        public static bool TryGetComponentInChildren<T>(this Component c, out T component) where T: Component
        {
            try
            {
                component = c.GetComponentInChildren<T>();
                return true;
            }
            catch
            {
                component = null;
                return false;
            }
        }
    }
}
