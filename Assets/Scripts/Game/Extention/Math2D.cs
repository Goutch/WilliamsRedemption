using UnityEngine;

namespace Math
{
    public static class Math2D
    {
        public static float VectorToRadian(Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        public static float VectorToDegree(Vector2 vector)
        {
            return (180 / Mathf.PI) * VectorToRadian(vector);
        }

        public static Vector2 RadianToVector(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector(float degree)
        {
            float radian = degree * Mathf.Deg2Rad;
            return RadianToVector(radian);
        }
    }
}