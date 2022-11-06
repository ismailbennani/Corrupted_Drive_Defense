using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static int ToSignedPercent(float value) {
            return Mathf.RoundToInt((value - 1) * 100);
        } 
    }
}
