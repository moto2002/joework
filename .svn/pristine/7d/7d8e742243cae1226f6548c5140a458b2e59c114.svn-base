using UnityEngine;

public class MyTool
{
    private static float triangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }
    public static bool isINTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float t = triangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = triangleArea(v0x, v0y, v1x, v1y, x, y) + triangleArea(v0x, v0y, x, y, v2x, v2y) + triangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private static float Multiply(float p1x, float p1y, float p2x, float p2y, float p0x, float p0y)
    {
        return ((p1x - p0x) * (p2y - p0y) - (p2x - p0x) * (p1y - p0y));
    }

    public static bool isINRect(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float v3x = v3.x;
        float v3y = v3.z;

        if (Multiply(x, y, v0x, v0y, v1x, v1y) * Multiply(x, y, v3x, v3y, v2x, v2y) <= 0 && Multiply(x, y, v3x, v3y, v0x, v0y) * Multiply(x, y, v2x, v2y, v1x, v1y) <= 0)
            return true;
        else
            return false;

    }
}
