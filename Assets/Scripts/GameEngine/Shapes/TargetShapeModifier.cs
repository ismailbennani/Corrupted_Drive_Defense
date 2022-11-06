using System;
using System.Text;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class TargetShapeModifier
    {
        public bool changeShape;
        public ShapeType newShape;
        public Vector2Int additionalRadius;

        public static TargetShapeModifier CombineInPlace(TargetShapeModifier @this, TargetShapeModifier other)
        {
            @this.changeShape = @this.changeShape || other.changeShape;

            if (@this.changeShape && other.changeShape && @this.newShape != other.newShape)
            {
                Debug.LogWarning($"Conflict between shape modifiers: new shape {@this.newShape} != {other.newShape}");
            }
            else if (other.changeShape)
            {
                @this.newShape = other.newShape;
            }

            @this.additionalRadius += other.additionalRadius;

            return @this;
        }

        public string GetTechnicalDescription()
        {
            if (!changeShape && additionalRadius == Vector2Int.zero)
            {
                return null;
            }

            StringBuilder builder = new();

            if (changeShape)
            {
                builder.AppendLine(newShape.ToString());
            }

            if (additionalRadius != Vector2Int.zero)
            {
                builder.AppendFormat($"Radius: {additionalRadius.x:+#;-#;0}, {additionalRadius.y:+#;-#;0}");
            }

            return builder.ToString();
        }
    }
}
