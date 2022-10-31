using UnityEngine;

namespace Utils
{
    public static class Mouse
    {
        private static Camera Camera;

        public static Vector2 WorldSpacePosition()
        {
            GetCamera();

            if (Camera)
            {
                Vector3 position = Camera.ScreenToWorldPoint(Input.mousePosition);
                return new Vector2(position.x, position.y);
            }

            return Vector2.zero;
        }

        public static Vector2Int? GridPosition()
        {
            Vector2 worldPosition = WorldSpacePosition();
            MapManager mapManager = GameManager.Instance.mapManager;

            return mapManager.WorldPositionToGridCell(worldPosition);
        }

        private static void GetCamera()
        {
            if (!Camera)
            {
                Camera = Camera.main;
            }
        }
    }
}
