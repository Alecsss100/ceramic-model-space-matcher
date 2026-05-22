using System;
using UnityEngine;

[Serializable]
public class Matrix4x4JsonDto
{
    public float m00, m10, m20, m30;
    public float m01, m11, m21, m31;
    public float m02, m12, m22, m32;
    public float m03, m13, m23, m33;

    public static Matrix4x4JsonDto FromMatrix4x4(Matrix4x4 matrix) => new()
    {
        m00 = matrix.m00,
        m10 = matrix.m10,
        m20 = matrix.m20,
        m30 = matrix.m30,
        m01 = matrix.m01,
        m11 = matrix.m11,
        m21 = matrix.m21,
        m31 = matrix.m31,
        m02 = matrix.m02,
        m12 = matrix.m12,
        m22 = matrix.m22,
        m32 = matrix.m32,
        m03 = matrix.m03,
        m13 = matrix.m13,
        m23 = matrix.m23,
        m33 = matrix.m33,
    };

    public Matrix4x4 ToMatrix4x4() => new Matrix4x4(
        new Vector4(m00, m10, m20, m30),
        new Vector4(m01, m11, m21, m31),
        new Vector4(m02, m12, m22, m32),
        new Vector4(m03, m13, m23, m33));
}
