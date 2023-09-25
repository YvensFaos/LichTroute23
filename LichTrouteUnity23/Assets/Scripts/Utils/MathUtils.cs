using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Utils
{
    public class MathUtils
    {
        public static Vector3 RandomPointInCircumference(float radius)
        {
            var angle = Mathf.Deg2Rad * Random.Range(-180.0f, 180.0f);
            var randomPoint = Vector3.zero;
            randomPoint.x = Mathf.Cos(angle) * radius;
            randomPoint.y = Mathf.Sin(angle) * radius;
            return randomPoint;
        }
    
        public static Quaternion CalculateRotation(Vector3 direction, float bias = 180.0f)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + bias;
            var rotationForward = Quaternion.AngleAxis(angle, Vector3.forward);
            return rotationForward;
        }

        public static Quaternion RotationTowards(Vector3 position, Vector3 destination, out float distance, float bias = 180.0f)
        {
            var direction = destination - position;
            distance = direction.magnitude;
            return CalculateRotation(direction.normalized);
        }
    
        public static Vector3 RandomPointInBounds(Bounds bounds) {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                0.0f //do not randomize the Z position
            );
        }

        public static bool RandomChange(float min, float max, float chance)
        {
            return Random.Range(min, max) <= chance;
        }
    }
}
