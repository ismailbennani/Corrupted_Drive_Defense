using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utils.CustomComponents
{
    public interface INeedsComponent<T> where T: Component
    {
        T Component { get; set; }
        T GetInstance();
    }

    public static class NeedsComponentExtensions
    {
        public static bool TryGetNeededComponent<T>(this INeedsComponent<T> @this) where T: Component
        {
            if (!@this.Component)
            {
                @this.Component = @this.GetInstance();
            }

            return @this.Component;
        }

        public static void RequireComponent<T>(this INeedsComponent<T> @this) where T: Component
        {
            Assert.IsTrue(TryGetNeededComponent(@this));
        }
    }
}
