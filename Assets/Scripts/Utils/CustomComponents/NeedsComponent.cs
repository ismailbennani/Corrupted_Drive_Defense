using System;
using UnityEngine;

namespace Utils.CustomComponents
{
    public interface INeedsComponent<T> where T: Component
    {
        T Component { get; set; }
        T GetInstance();
    }

    public static class NeedsComponentExtensions
    {
        public static bool TryGetComponent<T>(this INeedsComponent<T> @this) where T: Component
        {
            if (!@this.Component)
            {
                @this.Component = @this.GetInstance();
            }

            return @this.Component;
        }

        public static void RequireComponent<T>(this INeedsComponent<T> @this) where T: Component
        {
            if (!TryGetComponent(@this))
            {
                throw new InvalidOperationException($"could not get component {typeof(T).Name}");
            }
        }
    }
}
