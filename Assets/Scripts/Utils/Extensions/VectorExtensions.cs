using UnityEngine;

namespace Utils.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 WithoutDepth(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 WithDepth(this Vector2 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector2Int WithoutDepth(this Vector3Int v)
        {
            return new Vector2Int(v.x, v.y);
        }

        public static Vector3Int WithDepth(this Vector2Int v, int z)
        {
            return new Vector3Int(v.x, v.y, z);
        }
    }
}
