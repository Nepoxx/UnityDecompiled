// Decompiled with JetBrains decompiler
// Type: UnityEditor.CameraState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CameraState
  {
    private static readonly Quaternion kDefaultRotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1f));
    private static readonly Vector3 kDefaultPivot = Vector3.zero;
    [SerializeField]
    private AnimVector3 m_Pivot = new AnimVector3(CameraState.kDefaultPivot);
    [SerializeField]
    private AnimQuaternion m_Rotation = new AnimQuaternion(CameraState.kDefaultRotation);
    [SerializeField]
    private AnimFloat m_ViewSize = new AnimFloat(10f);
    private const float kDefaultViewSize = 10f;
    private const float kDefaultFoV = 90f;

    public float GetCameraDistance()
    {
      return this.m_ViewSize.value / Mathf.Tan((float) (90.0 * 0.5 * (Math.PI / 180.0)));
    }

    public void FixNegativeSize()
    {
      float num = 90f;
      if ((double) this.m_ViewSize.value >= 0.0)
        return;
      Vector3 vector3 = this.m_Pivot.value + this.m_Rotation.value * new Vector3(0.0f, 0.0f, -(this.m_ViewSize.value / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)))));
      this.m_ViewSize.value = -this.m_ViewSize.value;
      float z = this.m_ViewSize.value / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
      this.m_Pivot.value = vector3 + this.m_Rotation.value * new Vector3(0.0f, 0.0f, z);
    }

    public void UpdateCamera(Camera camera)
    {
      camera.transform.rotation = this.m_Rotation.value;
      camera.transform.position = this.m_Pivot.value + camera.transform.rotation * new Vector3(0.0f, 0.0f, -this.GetCameraDistance());
      float num = Mathf.Max(1000f, 2000f * this.m_ViewSize.value);
      camera.nearClipPlane = num * 5E-06f;
      camera.farClipPlane = num;
    }

    public CameraState Clone()
    {
      CameraState cameraState = new CameraState();
      cameraState.pivot.value = this.pivot.value;
      cameraState.rotation.value = this.rotation.value;
      cameraState.viewSize.value = this.viewSize.value;
      return cameraState;
    }

    public void Copy(CameraState cameraStateIn)
    {
      this.pivot.value = cameraStateIn.pivot.value;
      this.rotation.value = cameraStateIn.rotation.value;
      this.viewSize.value = cameraStateIn.viewSize.value;
    }

    public AnimVector3 pivot
    {
      get
      {
        return this.m_Pivot;
      }
      set
      {
        this.m_Pivot = value;
      }
    }

    public AnimQuaternion rotation
    {
      get
      {
        return this.m_Rotation;
      }
      set
      {
        this.m_Rotation = value;
      }
    }

    public AnimFloat viewSize
    {
      get
      {
        return this.m_ViewSize;
      }
      set
      {
        this.m_ViewSize = value;
      }
    }
  }
}
