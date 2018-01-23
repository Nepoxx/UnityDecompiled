// Decompiled with JetBrains decompiler
// Type: UnityEngine.GeometryUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class GeometryUtility
  {
    public static Plane[] CalculateFrustumPlanes(Camera camera)
    {
      Plane[] planes = new Plane[6];
      GeometryUtility.CalculateFrustumPlanes(camera, planes);
      return planes;
    }

    public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)
    {
      Plane[] planes = new Plane[6];
      GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix, planes);
      return planes;
    }

    public static void CalculateFrustumPlanes(Camera camera, Plane[] planes)
    {
      GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix, planes);
    }

    public static void CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix, Plane[] planes)
    {
      if (planes == null)
        throw new ArgumentNullException(nameof (planes));
      if (planes.Length != 6)
        throw new ArgumentException("Planes array must be of length 6.", nameof (planes));
      GeometryUtility.Internal_ExtractPlanes(planes, worldToProjectionMatrix);
    }

    public static Bounds CalculateBounds(Vector3[] positions, Matrix4x4 transform)
    {
      if (positions == null)
        throw new ArgumentNullException(nameof (positions));
      if (positions.Length == 0)
        throw new ArgumentException("Zero-sized array is not allowed.", nameof (positions));
      return GeometryUtility.Internal_CalculateBounds(positions, transform);
    }

    public static bool TryCreatePlaneFromPolygon(Vector3[] vertices, out Plane plane)
    {
      if (vertices == null || vertices.Length < 3)
      {
        plane = new Plane(Vector3.up, 0.0f);
        return false;
      }
      if (vertices.Length == 3)
      {
        Vector3 vertex1 = vertices[0];
        Vector3 vertex2 = vertices[1];
        Vector3 vertex3 = vertices[2];
        plane = new Plane(vertex1, vertex2, vertex3);
        return (double) plane.normal.sqrMagnitude > 0.0;
      }
      Vector3 zero = Vector3.zero;
      int index1 = vertices.Length - 1;
      Vector3 vector3 = vertices[index1];
      for (int index2 = 0; index2 < vertices.Length; ++index2)
      {
        Vector3 vertex = vertices[index2];
        zero.x += (float) (((double) vector3.y - (double) vertex.y) * ((double) vector3.z + (double) vertex.z));
        zero.y += (float) (((double) vector3.z - (double) vertex.z) * ((double) vector3.x + (double) vertex.x));
        zero.z += (float) (((double) vector3.x - (double) vertex.x) * ((double) vector3.y + (double) vertex.y));
        vector3 = vertex;
      }
      zero.Normalize();
      float num = 0.0f;
      for (int index2 = 0; index2 < vertices.Length; ++index2)
      {
        Vector3 vertex = vertices[index2];
        num -= Vector3.Dot(zero, vertex);
      }
      float d = num / (float) vertices.Length;
      plane = new Plane(zero, d);
      return (double) plane.normal.sqrMagnitude > 0.0;
    }

    private static void Internal_ExtractPlanes(Plane[] planes, Matrix4x4 worldToProjectionMatrix)
    {
      GeometryUtility.Internal_ExtractPlanes_Injected(planes, ref worldToProjectionMatrix);
    }

    public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)
    {
      return GeometryUtility.TestPlanesAABB_Injected(planes, ref bounds);
    }

    private static Bounds Internal_CalculateBounds(Vector3[] positions, Matrix4x4 transform)
    {
      Bounds ret;
      GeometryUtility.Internal_CalculateBounds_Injected(positions, ref transform, out ret);
      return ret;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_ExtractPlanes_Injected(Plane[] planes, ref Matrix4x4 worldToProjectionMatrix);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TestPlanesAABB_Injected(Plane[] planes, ref Bounds bounds);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CalculateBounds_Injected(Vector3[] positions, ref Matrix4x4 transform, out Bounds ret);
  }
}
