using UnityEngine;

public static class Matrix4x4Extensions
{
    public const float DefaultEpsilon = 1e-4f;

    public static bool Approximately(this Matrix4x4 a, Matrix4x4 b, float epsilon = DefaultEpsilon)
    {
        for (var i = 0; i < 16; i++)
        {
            if (Mathf.Abs(a[i] - b[i]) > epsilon)
                return false;
        }

        return true;
    }

    public static Matrix4x4 SnapToEpsilon(this Matrix4x4 matrix, float epsilon = DefaultEpsilon)
    {
        var invEpsilon = 1f / epsilon;
        var result = matrix;

        for (var i = 0; i < 16; i++)
            result[i] = Mathf.Round(matrix[i] * invEpsilon) * epsilon;

        return result;
    }
}
