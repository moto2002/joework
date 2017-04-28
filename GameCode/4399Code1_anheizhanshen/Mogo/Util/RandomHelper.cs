namespace Mogo.Util
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RandomHelper
    {
        private static Random globalRandomGenerator = Utils.CreateRandom();

        public static void GenerateNewRandomGenerator()
        {
            globalRandomGenerator = Utils.CreateRandom();
        }

        public static bool GetRandomBoolean()
        {
            if (GetRandomInt(2) == 0)
            {
                return false;
            }
            return true;
        }

        public static byte GetRandomByte()
        {
            byte[] buffer = new byte[1];
            globalRandomGenerator.NextBytes(buffer);
            return buffer[0];
        }

        public static void GetRandomBytes(byte[] bytes)
        {
            globalRandomGenerator.NextBytes(bytes);
        }

        public static Color GetRandomColor()
        {
            byte[] buffer = new byte[4];
            globalRandomGenerator.NextBytes(buffer);
            return new Color((float) buffer[0], (float) buffer[1], (float) buffer[2], (float) buffer[3]);
        }

        public static Color GetRandomColor(int alpha)
        {
            byte[] buffer = new byte[3];
            globalRandomGenerator.NextBytes(buffer);
            if (alpha < 0)
            {
                alpha = 0;
            }
            if (alpha > 0xff)
            {
                alpha = 0xff;
            }
            return new Color((float) buffer[0], (float) buffer[1], (float) buffer[2], (float) alpha);
        }

        public static float GetRandomFloat()
        {
            return (float) globalRandomGenerator.NextDouble();
        }

        public static float GetRandomFloat(float max)
        {
            return (((float) globalRandomGenerator.NextDouble()) * max);
        }

        public static float GetRandomFloat(float min, float max)
        {
            if (min < max)
            {
                return ((((float) globalRandomGenerator.NextDouble()) * (max - min)) + min);
            }
            return max;
        }

        public static int GetRandomInt(bool positive = true)
        {
            if (positive)
            {
                return globalRandomGenerator.Next();
            }
            int num = globalRandomGenerator.Next() % 2;
            if (num == 0)
            {
                return globalRandomGenerator.Next();
            }
            return -globalRandomGenerator.Next();
        }

        public static int GetRandomInt(int max)
        {
            if (max > 0)
            {
                return (globalRandomGenerator.Next() % max);
            }
            if (max < 0)
            {
                return (-globalRandomGenerator.Next() % max);
            }
            return 0;
        }

        public static int GetRandomInt(int min, int max)
        {
            if (min < max)
            {
                return globalRandomGenerator.Next(min, max);
            }
            return max;
        }

        public static Vector3 GetRandomNormalVector3()
        {
            Vector3 vector2 = new Vector3((float) GetRandomInt(false), (float) GetRandomInt(false), (float) GetRandomInt(false));
            return vector2.get_normalized();
        }

        public static Vector2 GetRandomVector2()
        {
            return new Vector2(GetRandomInt(false) * GetRandomFloat(), GetRandomInt(false) * GetRandomFloat());
        }

        public static Vector2 GetRandomVector2(float xMax, float yMax = 0f)
        {
            return new Vector3(GetRandomFloat(xMax), GetRandomFloat(yMax));
        }

        public static Vector2 GetRandomVector2(float xMin, float xMax, float yMin, float yMax)
        {
            return new Vector3(GetRandomFloat(xMin, xMax), GetRandomFloat(yMin, yMax));
        }

        public static Vector3 GetRandomVector3()
        {
            return new Vector3(GetRandomInt(false) * GetRandomFloat(), GetRandomInt(false) * GetRandomFloat(), GetRandomInt(false) * GetRandomFloat());
        }

        public static Vector3 GetRandomVector3(float xMax, float yMax = 0f, float zMax = 0f)
        {
            return new Vector3(GetRandomFloat(xMax), GetRandomFloat(yMax), GetRandomFloat(zMax));
        }

        public static Vector3 GetRandomVector3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            return new Vector3(GetRandomFloat(xMin, xMax), GetRandomFloat(yMin, yMax), GetRandomFloat(zMin, zMax));
        }

        public static Vector3 GetRandomVector3InRangeCircle(float range, float y = 0f)
        {
            float randomFloat = GetRandomFloat(0f, range);
            float num2 = GetRandomFloat(0f, 360f);
            return new Vector3((float) (randomFloat * Math.Sin(num2 * 0.017453292519943295)), y, (float) (randomFloat * Math.Cos(num2 * 0.017453292519943295)));
        }
    }
}

