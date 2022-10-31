using System;

namespace Utils.Interfaces
{
    public interface INeedsGameManager
    {
        GameManager GameManager { get; set; }
    }

    public static class NeedsGameManagerExtensions
    {
        public static bool TryGetGameManager(this INeedsGameManager @this)
        {
            if (!@this.GameManager)
            {
                @this.GameManager = GameManager.Instance;
            }

            return @this.GameManager;
        }
        
        public static void RequireGameManager(this INeedsGameManager @this)
        {
            if (!TryGetGameManager(@this))
            {
                throw new InvalidOperationException("could not get game manager");
            }
        }
    }
}
