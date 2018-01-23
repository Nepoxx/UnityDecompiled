// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.CameraProperties
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
  [UsedByNativeCode]
  public struct CameraProperties
  {
    private const int kNumLayers = 32;
    private Rect screenRect;
    private Vector3 viewDir;
    private float projectionNear;
    private float projectionFar;
    private float cameraNear;
    private float cameraFar;
    private float cameraAspect;
    private Matrix4x4 cameraToWorld;
    private Matrix4x4 actualWorldToClip;
    private Matrix4x4 cameraClipToWorld;
    private Matrix4x4 cameraWorldToClip;
    private Matrix4x4 implicitProjection;
    private Matrix4x4 stereoWorldToClipLeft;
    private Matrix4x4 stereoWorldToClipRight;
    private Matrix4x4 worldToCamera;
    private Vector3 up;
    private Vector3 right;
    private Vector3 transformDirection;
    private Vector3 cameraEuler;
    private Vector3 velocity;
    private float farPlaneWorldSpaceLength;
    private uint rendererCount;
    private CameraProperties.\u003C_shadowCullPlanes\u003E__FixedBuffer1 _shadowCullPlanes;
    private CameraProperties.\u003C_cameraCullPlanes\u003E__FixedBuffer2 _cameraCullPlanes;
    private float baseFarDistance;
    private Vector3 shadowCullCenter;
    private CameraProperties.\u003ClayerCullDistances\u003E__FixedBuffer3 layerCullDistances;
    private int layerCullSpherical;
    private CoreCameraValues coreCameraValues;
    private uint cameraType;

    public unsafe Plane GetShadowCullingPlane(int index)
    {
      if (index < 0 || index >= 6)
        throw new IndexOutOfRangeException("Invalid plane index");
      // ISSUE: reference to a compiler-generated field
      fixed (float* numPtr = &this._shadowCullPlanes.FixedElementField)
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        return new Plane(new Vector3(^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4) * 4), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 1) * 4), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 2) * 4)), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 3) * 4));
      }
    }

    public unsafe void SetShadowCullingPlane(int index, Plane plane)
    {
      if (index < 0 || index >= 6)
        throw new IndexOutOfRangeException("Invalid plane index");
      // ISSUE: reference to a compiler-generated field
      fixed (float* numPtr = &this._shadowCullPlanes.FixedElementField)
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4) * 4) = plane.normal.x;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 1) * 4) = plane.normal.y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 2) * 4) = plane.normal.z;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 3) * 4) = plane.distance;
      }
    }

    public unsafe Plane GetCameraCullingPlane(int index)
    {
      if (index < 0 || index >= 6)
        throw new IndexOutOfRangeException("Invalid plane index");
      // ISSUE: reference to a compiler-generated field
      fixed (float* numPtr = &this._cameraCullPlanes.FixedElementField)
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        return new Plane(new Vector3(^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4) * 4), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 1) * 4), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 2) * 4)), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 3) * 4));
      }
    }

    public unsafe void SetCameraCullingPlane(int index, Plane plane)
    {
      if (index < 0 || index >= 6)
        throw new IndexOutOfRangeException("Invalid plane index");
      // ISSUE: reference to a compiler-generated field
      fixed (float* numPtr = &this._cameraCullPlanes.FixedElementField)
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4) * 4) = plane.normal.x;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 1) * 4) = plane.normal.y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 2) * 4) = plane.normal.z;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 3) * 4) = plane.distance;
      }
    }
  }
}
