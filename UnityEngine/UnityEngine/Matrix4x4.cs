// Decompiled with JetBrains decompiler
// Type: UnityEngine.Matrix4x4
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A standard 4x4 transformation matrix.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct Matrix4x4
  {
    private static readonly Matrix4x4 zeroMatrix = new Matrix4x4(new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
    private static readonly Matrix4x4 identityMatrix = new Matrix4x4(new Vector4(1f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 1f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1f));
    public float m00;
    public float m10;
    public float m20;
    public float m30;
    public float m01;
    public float m11;
    public float m21;
    public float m31;
    public float m02;
    public float m12;
    public float m22;
    public float m32;
    public float m03;
    public float m13;
    public float m23;
    public float m33;

    public Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
      this.m00 = column0.x;
      this.m01 = column1.x;
      this.m02 = column2.x;
      this.m03 = column3.x;
      this.m10 = column0.y;
      this.m11 = column1.y;
      this.m12 = column2.y;
      this.m13 = column3.y;
      this.m20 = column0.z;
      this.m21 = column1.z;
      this.m22 = column2.z;
      this.m23 = column3.z;
      this.m30 = column0.w;
      this.m31 = column1.w;
      this.m32 = column2.w;
      this.m33 = column3.w;
    }

    [ThreadAndSerializationSafe]
    public static Matrix4x4 Inverse(Matrix4x4 m)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Inverse(ref m, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Inverse(ref Matrix4x4 m, out Matrix4x4 value);

    public static Matrix4x4 Transpose(Matrix4x4 m)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Transpose(ref m, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Transpose(ref Matrix4x4 m, out Matrix4x4 value);

    internal static bool Invert(Matrix4x4 inMatrix, out Matrix4x4 dest)
    {
      return Matrix4x4.INTERNAL_CALL_Invert(ref inMatrix, out dest);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Invert(ref Matrix4x4 inMatrix, out Matrix4x4 dest);

    /// <summary>
    ///   <para>The inverse of this matrix (Read Only).</para>
    /// </summary>
    public Matrix4x4 inverse
    {
      get
      {
        return Matrix4x4.Inverse(this);
      }
    }

    /// <summary>
    ///   <para>Returns the transpose of this matrix (Read Only).</para>
    /// </summary>
    public Matrix4x4 transpose
    {
      get
      {
        return Matrix4x4.Transpose(this);
      }
    }

    /// <summary>
    ///   <para>Attempts to get a rotation quaternion from this matrix.</para>
    /// </summary>
    public Quaternion rotation
    {
      get
      {
        Quaternion quaternion;
        this.INTERNAL_get_rotation(out quaternion);
        return quaternion;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_rotation(out Quaternion value);

    /// <summary>
    ///   <para>Attempts to get a scale value from the matrix.</para>
    /// </summary>
    public Vector3 lossyScale
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_lossyScale(out vector3);
        return vector3;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_lossyScale(out Vector3 value);

    /// <summary>
    ///   <para>Checks if this matrix is a valid transform matrix.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ValidTRS();

    /// <summary>
    ///   <para>Is this the identity matrix?</para>
    /// </summary>
    public extern bool isIdentity { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static float Determinant(Matrix4x4 m)
    {
      return Matrix4x4.INTERNAL_CALL_Determinant(ref m);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float INTERNAL_CALL_Determinant(ref Matrix4x4 m);

    /// <summary>
    ///   <para>The determinant of the matrix.</para>
    /// </summary>
    public float determinant
    {
      get
      {
        return Matrix4x4.Determinant(this);
      }
    }

    /// <summary>
    ///   <para>Sets this matrix to a translation, rotation and scaling matrix.</para>
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="q"></param>
    /// <param name="s"></param>
    public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
    {
      this = Matrix4x4.TRS(pos, q, s);
    }

    /// <summary>
    ///   <para>Creates a translation, rotation and scaling matrix.</para>
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="q"></param>
    /// <param name="s"></param>
    public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_TRS(ref pos, ref q, ref s, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TRS(ref Vector3 pos, ref Quaternion q, ref Vector3 s, out Matrix4x4 value);

    /// <summary>
    ///   <para>Creates an orthogonal projection matrix.</para>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="top"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Ortho(left, right, bottom, top, zNear, zFar, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Ortho(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4x4 value);

    /// <summary>
    ///   <para>Creates a perspective projection matrix.</para>
    /// </summary>
    /// <param name="fov"></param>
    /// <param name="aspect"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    public static Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Perspective(fov, aspect, zNear, zFar, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Perspective(float fov, float aspect, float zNear, float zFar, out Matrix4x4 value);

    /// <summary>
    ///   <para>Given a source point, a target point, and an up vector, computes a transformation matrix that corresponds to a camera viewing the target from the source, such that the right-hand vector is perpendicular to the up vector.</para>
    /// </summary>
    /// <param name="from">The source point.</param>
    /// <param name="to">The target point.</param>
    /// <param name="up">The vector describing the up direction (typically Vector3.up).</param>
    /// <returns>
    ///   <para>The resulting transformation matrix.</para>
    /// </returns>
    public static Matrix4x4 LookAt(Vector3 from, Vector3 to, Vector3 up)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_LookAt(ref from, ref to, ref up, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_LookAt(ref Vector3 from, ref Vector3 to, ref Vector3 up, out Matrix4x4 value);

    /// <summary>
    ///   <para>This property takes a projection matrix and returns the six plane coordinates that define a projection frustum.</para>
    /// </summary>
    public FrustumPlanes decomposeProjection
    {
      get
      {
        FrustumPlanes frustumPlanes;
        this.INTERNAL_get_decomposeProjection(out frustumPlanes);
        return frustumPlanes;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_decomposeProjection(out FrustumPlanes value);

    /// <summary>
    ///   <para>This function returns a projection matrix with viewing frustum that has a near plane defined by the coordinates that were passed in.</para>
    /// </summary>
    /// <param name="left">The X coordinate of the left side of the near projection plane in view space.</param>
    /// <param name="right">The X coordinate of the right side of the near projection plane in view space.</param>
    /// <param name="bottom">The Y coordinate of the bottom side of the near projection plane in view space.</param>
    /// <param name="top">The Y coordinate of the top side of the near projection plane in view space.</param>
    /// <param name="zNear">Z distance to the near plane from the origin in view space.</param>
    /// <param name="zFar">Z distance to the far plane from the origin in view space.</param>
    /// <param name="frustumPlanes">Frustum planes struct that contains the view space coordinates of that define a viewing frustum.</param>
    /// <returns>
    ///   <para>A projection matrix with a viewing frustum defined by the plane coordinates passed in.</para>
    /// </returns>
    public static Matrix4x4 Frustum(float left, float right, float bottom, float top, float zNear, float zFar)
    {
      return Matrix4x4.FrustumInternal(left, right, bottom, top, zNear, zFar);
    }

    /// <summary>
    ///   <para>This function returns a projection matrix with viewing frustum that has a near plane defined by the coordinates that were passed in.</para>
    /// </summary>
    /// <param name="left">The X coordinate of the left side of the near projection plane in view space.</param>
    /// <param name="right">The X coordinate of the right side of the near projection plane in view space.</param>
    /// <param name="bottom">The Y coordinate of the bottom side of the near projection plane in view space.</param>
    /// <param name="top">The Y coordinate of the top side of the near projection plane in view space.</param>
    /// <param name="zNear">Z distance to the near plane from the origin in view space.</param>
    /// <param name="zFar">Z distance to the far plane from the origin in view space.</param>
    /// <param name="frustumPlanes">Frustum planes struct that contains the view space coordinates of that define a viewing frustum.</param>
    /// <returns>
    ///   <para>A projection matrix with a viewing frustum defined by the plane coordinates passed in.</para>
    /// </returns>
    public static Matrix4x4 Frustum(FrustumPlanes frustumPlanes)
    {
      return Matrix4x4.FrustumInternal(frustumPlanes.left, frustumPlanes.right, frustumPlanes.bottom, frustumPlanes.top, frustumPlanes.zNear, frustumPlanes.zFar);
    }

    private static Matrix4x4 FrustumInternal(float left, float right, float bottom, float top, float zNear, float zFar)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_FrustumInternal(left, right, bottom, top, zNear, zFar, out matrix4x4);
      return matrix4x4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_FrustumInternal(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4x4 value);

    public float this[int row, int column]
    {
      get
      {
        return this[row + column * 4];
      }
      set
      {
        this[row + column * 4] = value;
      }
    }

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.m00;
          case 1:
            return this.m10;
          case 2:
            return this.m20;
          case 3:
            return this.m30;
          case 4:
            return this.m01;
          case 5:
            return this.m11;
          case 6:
            return this.m21;
          case 7:
            return this.m31;
          case 8:
            return this.m02;
          case 9:
            return this.m12;
          case 10:
            return this.m22;
          case 11:
            return this.m32;
          case 12:
            return this.m03;
          case 13:
            return this.m13;
          case 14:
            return this.m23;
          case 15:
            return this.m33;
          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.m00 = value;
            break;
          case 1:
            this.m10 = value;
            break;
          case 2:
            this.m20 = value;
            break;
          case 3:
            this.m30 = value;
            break;
          case 4:
            this.m01 = value;
            break;
          case 5:
            this.m11 = value;
            break;
          case 6:
            this.m21 = value;
            break;
          case 7:
            this.m31 = value;
            break;
          case 8:
            this.m02 = value;
            break;
          case 9:
            this.m12 = value;
            break;
          case 10:
            this.m22 = value;
            break;
          case 11:
            this.m32 = value;
            break;
          case 12:
            this.m03 = value;
            break;
          case 13:
            this.m13 = value;
            break;
          case 14:
            this.m23 = value;
            break;
          case 15:
            this.m33 = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
    }

    public override int GetHashCode()
    {
      return this.GetColumn(0).GetHashCode() ^ this.GetColumn(1).GetHashCode() << 2 ^ this.GetColumn(2).GetHashCode() >> 2 ^ this.GetColumn(3).GetHashCode() >> 1;
    }

    public override bool Equals(object other)
    {
      if (!(other is Matrix4x4))
        return false;
      Matrix4x4 matrix4x4 = (Matrix4x4) other;
      return this.GetColumn(0).Equals((object) matrix4x4.GetColumn(0)) && this.GetColumn(1).Equals((object) matrix4x4.GetColumn(1)) && this.GetColumn(2).Equals((object) matrix4x4.GetColumn(2)) && this.GetColumn(3).Equals((object) matrix4x4.GetColumn(3));
    }

    public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
    {
      Matrix4x4 matrix4x4;
      matrix4x4.m00 = (float) ((double) lhs.m00 * (double) rhs.m00 + (double) lhs.m01 * (double) rhs.m10 + (double) lhs.m02 * (double) rhs.m20 + (double) lhs.m03 * (double) rhs.m30);
      matrix4x4.m01 = (float) ((double) lhs.m00 * (double) rhs.m01 + (double) lhs.m01 * (double) rhs.m11 + (double) lhs.m02 * (double) rhs.m21 + (double) lhs.m03 * (double) rhs.m31);
      matrix4x4.m02 = (float) ((double) lhs.m00 * (double) rhs.m02 + (double) lhs.m01 * (double) rhs.m12 + (double) lhs.m02 * (double) rhs.m22 + (double) lhs.m03 * (double) rhs.m32);
      matrix4x4.m03 = (float) ((double) lhs.m00 * (double) rhs.m03 + (double) lhs.m01 * (double) rhs.m13 + (double) lhs.m02 * (double) rhs.m23 + (double) lhs.m03 * (double) rhs.m33);
      matrix4x4.m10 = (float) ((double) lhs.m10 * (double) rhs.m00 + (double) lhs.m11 * (double) rhs.m10 + (double) lhs.m12 * (double) rhs.m20 + (double) lhs.m13 * (double) rhs.m30);
      matrix4x4.m11 = (float) ((double) lhs.m10 * (double) rhs.m01 + (double) lhs.m11 * (double) rhs.m11 + (double) lhs.m12 * (double) rhs.m21 + (double) lhs.m13 * (double) rhs.m31);
      matrix4x4.m12 = (float) ((double) lhs.m10 * (double) rhs.m02 + (double) lhs.m11 * (double) rhs.m12 + (double) lhs.m12 * (double) rhs.m22 + (double) lhs.m13 * (double) rhs.m32);
      matrix4x4.m13 = (float) ((double) lhs.m10 * (double) rhs.m03 + (double) lhs.m11 * (double) rhs.m13 + (double) lhs.m12 * (double) rhs.m23 + (double) lhs.m13 * (double) rhs.m33);
      matrix4x4.m20 = (float) ((double) lhs.m20 * (double) rhs.m00 + (double) lhs.m21 * (double) rhs.m10 + (double) lhs.m22 * (double) rhs.m20 + (double) lhs.m23 * (double) rhs.m30);
      matrix4x4.m21 = (float) ((double) lhs.m20 * (double) rhs.m01 + (double) lhs.m21 * (double) rhs.m11 + (double) lhs.m22 * (double) rhs.m21 + (double) lhs.m23 * (double) rhs.m31);
      matrix4x4.m22 = (float) ((double) lhs.m20 * (double) rhs.m02 + (double) lhs.m21 * (double) rhs.m12 + (double) lhs.m22 * (double) rhs.m22 + (double) lhs.m23 * (double) rhs.m32);
      matrix4x4.m23 = (float) ((double) lhs.m20 * (double) rhs.m03 + (double) lhs.m21 * (double) rhs.m13 + (double) lhs.m22 * (double) rhs.m23 + (double) lhs.m23 * (double) rhs.m33);
      matrix4x4.m30 = (float) ((double) lhs.m30 * (double) rhs.m00 + (double) lhs.m31 * (double) rhs.m10 + (double) lhs.m32 * (double) rhs.m20 + (double) lhs.m33 * (double) rhs.m30);
      matrix4x4.m31 = (float) ((double) lhs.m30 * (double) rhs.m01 + (double) lhs.m31 * (double) rhs.m11 + (double) lhs.m32 * (double) rhs.m21 + (double) lhs.m33 * (double) rhs.m31);
      matrix4x4.m32 = (float) ((double) lhs.m30 * (double) rhs.m02 + (double) lhs.m31 * (double) rhs.m12 + (double) lhs.m32 * (double) rhs.m22 + (double) lhs.m33 * (double) rhs.m32);
      matrix4x4.m33 = (float) ((double) lhs.m30 * (double) rhs.m03 + (double) lhs.m31 * (double) rhs.m13 + (double) lhs.m32 * (double) rhs.m23 + (double) lhs.m33 * (double) rhs.m33);
      return matrix4x4;
    }

    public static Vector4 operator *(Matrix4x4 lhs, Vector4 vector)
    {
      Vector4 vector4;
      vector4.x = (float) ((double) lhs.m00 * (double) vector.x + (double) lhs.m01 * (double) vector.y + (double) lhs.m02 * (double) vector.z + (double) lhs.m03 * (double) vector.w);
      vector4.y = (float) ((double) lhs.m10 * (double) vector.x + (double) lhs.m11 * (double) vector.y + (double) lhs.m12 * (double) vector.z + (double) lhs.m13 * (double) vector.w);
      vector4.z = (float) ((double) lhs.m20 * (double) vector.x + (double) lhs.m21 * (double) vector.y + (double) lhs.m22 * (double) vector.z + (double) lhs.m23 * (double) vector.w);
      vector4.w = (float) ((double) lhs.m30 * (double) vector.x + (double) lhs.m31 * (double) vector.y + (double) lhs.m32 * (double) vector.z + (double) lhs.m33 * (double) vector.w);
      return vector4;
    }

    public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
    {
      return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
    }

    public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
    {
      return !(lhs == rhs);
    }

    /// <summary>
    ///   <para>Get a column of the matrix.</para>
    /// </summary>
    /// <param name="index"></param>
    public Vector4 GetColumn(int index)
    {
      switch (index)
      {
        case 0:
          return new Vector4(this.m00, this.m10, this.m20, this.m30);
        case 1:
          return new Vector4(this.m01, this.m11, this.m21, this.m31);
        case 2:
          return new Vector4(this.m02, this.m12, this.m22, this.m32);
        case 3:
          return new Vector4(this.m03, this.m13, this.m23, this.m33);
        default:
          throw new IndexOutOfRangeException("Invalid column index!");
      }
    }

    /// <summary>
    ///   <para>Returns a row of the matrix.</para>
    /// </summary>
    /// <param name="index"></param>
    public Vector4 GetRow(int index)
    {
      switch (index)
      {
        case 0:
          return new Vector4(this.m00, this.m01, this.m02, this.m03);
        case 1:
          return new Vector4(this.m10, this.m11, this.m12, this.m13);
        case 2:
          return new Vector4(this.m20, this.m21, this.m22, this.m23);
        case 3:
          return new Vector4(this.m30, this.m31, this.m32, this.m33);
        default:
          throw new IndexOutOfRangeException("Invalid row index!");
      }
    }

    /// <summary>
    ///   <para>Sets a column of the matrix.</para>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="column"></param>
    public void SetColumn(int index, Vector4 column)
    {
      this[0, index] = column.x;
      this[1, index] = column.y;
      this[2, index] = column.z;
      this[3, index] = column.w;
    }

    /// <summary>
    ///   <para>Sets a row of the matrix.</para>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="row"></param>
    public void SetRow(int index, Vector4 row)
    {
      this[index, 0] = row.x;
      this[index, 1] = row.y;
      this[index, 2] = row.z;
      this[index, 3] = row.w;
    }

    /// <summary>
    ///   <para>Transforms a position by this matrix (generic).</para>
    /// </summary>
    /// <param name="point"></param>
    public Vector3 MultiplyPoint(Vector3 point)
    {
      Vector3 vector3;
      vector3.x = (float) ((double) this.m00 * (double) point.x + (double) this.m01 * (double) point.y + (double) this.m02 * (double) point.z) + this.m03;
      vector3.y = (float) ((double) this.m10 * (double) point.x + (double) this.m11 * (double) point.y + (double) this.m12 * (double) point.z) + this.m13;
      vector3.z = (float) ((double) this.m20 * (double) point.x + (double) this.m21 * (double) point.y + (double) this.m22 * (double) point.z) + this.m23;
      float num = 1f / ((float) ((double) this.m30 * (double) point.x + (double) this.m31 * (double) point.y + (double) this.m32 * (double) point.z) + this.m33);
      vector3.x *= num;
      vector3.y *= num;
      vector3.z *= num;
      return vector3;
    }

    /// <summary>
    ///   <para>Transforms a position by this matrix (fast).</para>
    /// </summary>
    /// <param name="point"></param>
    public Vector3 MultiplyPoint3x4(Vector3 point)
    {
      Vector3 vector3;
      vector3.x = (float) ((double) this.m00 * (double) point.x + (double) this.m01 * (double) point.y + (double) this.m02 * (double) point.z) + this.m03;
      vector3.y = (float) ((double) this.m10 * (double) point.x + (double) this.m11 * (double) point.y + (double) this.m12 * (double) point.z) + this.m13;
      vector3.z = (float) ((double) this.m20 * (double) point.x + (double) this.m21 * (double) point.y + (double) this.m22 * (double) point.z) + this.m23;
      return vector3;
    }

    /// <summary>
    ///   <para>Transforms a direction by this matrix.</para>
    /// </summary>
    /// <param name="vector"></param>
    public Vector3 MultiplyVector(Vector3 vector)
    {
      Vector3 vector3;
      vector3.x = (float) ((double) this.m00 * (double) vector.x + (double) this.m01 * (double) vector.y + (double) this.m02 * (double) vector.z);
      vector3.y = (float) ((double) this.m10 * (double) vector.x + (double) this.m11 * (double) vector.y + (double) this.m12 * (double) vector.z);
      vector3.z = (float) ((double) this.m20 * (double) vector.x + (double) this.m21 * (double) vector.y + (double) this.m22 * (double) vector.z);
      return vector3;
    }

    /// <summary>
    ///   <para>Returns a plane that is transformed in space.</para>
    /// </summary>
    /// <param name="plane"></param>
    public Plane TransformPlane(Plane plane)
    {
      Matrix4x4 inverse = this.inverse;
      float x = plane.normal.x;
      float y = plane.normal.y;
      float z = plane.normal.z;
      float distance = plane.distance;
      return new Plane(new Vector3((float) ((double) inverse.m00 * (double) x + (double) inverse.m10 * (double) y + (double) inverse.m20 * (double) z + (double) inverse.m30 * (double) distance), (float) ((double) inverse.m01 * (double) x + (double) inverse.m11 * (double) y + (double) inverse.m21 * (double) z + (double) inverse.m31 * (double) distance), (float) ((double) inverse.m02 * (double) x + (double) inverse.m12 * (double) y + (double) inverse.m22 * (double) z + (double) inverse.m32 * (double) distance)), (float) ((double) inverse.m03 * (double) x + (double) inverse.m13 * (double) y + (double) inverse.m23 * (double) z + (double) inverse.m33 * (double) distance));
    }

    /// <summary>
    ///   <para>Creates a scaling matrix.</para>
    /// </summary>
    /// <param name="vector"></param>
    public static Matrix4x4 Scale(Vector3 vector)
    {
      Matrix4x4 matrix4x4;
      matrix4x4.m00 = vector.x;
      matrix4x4.m01 = 0.0f;
      matrix4x4.m02 = 0.0f;
      matrix4x4.m03 = 0.0f;
      matrix4x4.m10 = 0.0f;
      matrix4x4.m11 = vector.y;
      matrix4x4.m12 = 0.0f;
      matrix4x4.m13 = 0.0f;
      matrix4x4.m20 = 0.0f;
      matrix4x4.m21 = 0.0f;
      matrix4x4.m22 = vector.z;
      matrix4x4.m23 = 0.0f;
      matrix4x4.m30 = 0.0f;
      matrix4x4.m31 = 0.0f;
      matrix4x4.m32 = 0.0f;
      matrix4x4.m33 = 1f;
      return matrix4x4;
    }

    /// <summary>
    ///   <para>Creates a translation matrix.</para>
    /// </summary>
    /// <param name="vector"></param>
    public static Matrix4x4 Translate(Vector3 vector)
    {
      Matrix4x4 matrix4x4;
      matrix4x4.m00 = 1f;
      matrix4x4.m01 = 0.0f;
      matrix4x4.m02 = 0.0f;
      matrix4x4.m03 = vector.x;
      matrix4x4.m10 = 0.0f;
      matrix4x4.m11 = 1f;
      matrix4x4.m12 = 0.0f;
      matrix4x4.m13 = vector.y;
      matrix4x4.m20 = 0.0f;
      matrix4x4.m21 = 0.0f;
      matrix4x4.m22 = 1f;
      matrix4x4.m23 = vector.z;
      matrix4x4.m30 = 0.0f;
      matrix4x4.m31 = 0.0f;
      matrix4x4.m32 = 0.0f;
      matrix4x4.m33 = 1f;
      return matrix4x4;
    }

    /// <summary>
    ///   <para>Creates a rotation matrix.</para>
    /// </summary>
    /// <param name="q"></param>
    public static Matrix4x4 Rotate(Quaternion q)
    {
      float num1 = q.x * 2f;
      float num2 = q.y * 2f;
      float num3 = q.z * 2f;
      float num4 = q.x * num1;
      float num5 = q.y * num2;
      float num6 = q.z * num3;
      float num7 = q.x * num2;
      float num8 = q.x * num3;
      float num9 = q.y * num3;
      float num10 = q.w * num1;
      float num11 = q.w * num2;
      float num12 = q.w * num3;
      Matrix4x4 matrix4x4;
      matrix4x4.m00 = (float) (1.0 - ((double) num5 + (double) num6));
      matrix4x4.m10 = num7 + num12;
      matrix4x4.m20 = num8 - num11;
      matrix4x4.m30 = 0.0f;
      matrix4x4.m01 = num7 - num12;
      matrix4x4.m11 = (float) (1.0 - ((double) num4 + (double) num6));
      matrix4x4.m21 = num9 + num10;
      matrix4x4.m31 = 0.0f;
      matrix4x4.m02 = num8 + num11;
      matrix4x4.m12 = num9 - num10;
      matrix4x4.m22 = (float) (1.0 - ((double) num4 + (double) num5));
      matrix4x4.m32 = 0.0f;
      matrix4x4.m03 = 0.0f;
      matrix4x4.m13 = 0.0f;
      matrix4x4.m23 = 0.0f;
      matrix4x4.m33 = 1f;
      return matrix4x4;
    }

    /// <summary>
    ///   <para>Returns a matrix with all elements set to zero (Read Only).</para>
    /// </summary>
    public static Matrix4x4 zero
    {
      get
      {
        return Matrix4x4.zeroMatrix;
      }
    }

    /// <summary>
    ///   <para>Returns the identity matrix (Read Only).</para>
    /// </summary>
    public static Matrix4x4 identity
    {
      get
      {
        return Matrix4x4.identityMatrix;
      }
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string for this matrix.</para>
    /// </summary>
    /// <param name="format"></param>
    public override string ToString()
    {
      return UnityString.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", (object) this.m00, (object) this.m01, (object) this.m02, (object) this.m03, (object) this.m10, (object) this.m11, (object) this.m12, (object) this.m13, (object) this.m20, (object) this.m21, (object) this.m22, (object) this.m23, (object) this.m30, (object) this.m31, (object) this.m32, (object) this.m33);
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string for this matrix.</para>
    /// </summary>
    /// <param name="format"></param>
    public string ToString(string format)
    {
      return UnityString.Format("{0}\t{1}\t{2}\t{3}\n{4}\t{5}\t{6}\t{7}\n{8}\t{9}\t{10}\t{11}\n{12}\t{13}\t{14}\t{15}\n", (object) this.m00.ToString(format), (object) this.m01.ToString(format), (object) this.m02.ToString(format), (object) this.m03.ToString(format), (object) this.m10.ToString(format), (object) this.m11.ToString(format), (object) this.m12.ToString(format), (object) this.m13.ToString(format), (object) this.m20.ToString(format), (object) this.m21.ToString(format), (object) this.m22.ToString(format), (object) this.m23.ToString(format), (object) this.m30.ToString(format), (object) this.m31.ToString(format), (object) this.m32.ToString(format), (object) this.m33.ToString(format));
    }
  }
}
