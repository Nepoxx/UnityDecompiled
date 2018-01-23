// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.PhysicsRaycaster
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Raycaster for casting against 3D Physics components.</para>
  /// </summary>
  [AddComponentMenu("Event/Physics Raycaster")]
  [RequireComponent(typeof (Camera))]
  public class PhysicsRaycaster : BaseRaycaster
  {
    [SerializeField]
    protected LayerMask m_EventMask = (LayerMask) -1;
    [SerializeField]
    protected int m_MaxRayIntersections = 0;
    protected int m_LastMaxRayIntersections = 0;
    protected const int kNoEventMaskSet = -1;
    protected Camera m_EventCamera;
    private RaycastHit[] m_Hits;

    protected PhysicsRaycaster()
    {
    }

    /// <summary>
    ///   <para>Get the camera that is used for this module.</para>
    /// </summary>
    public override Camera eventCamera
    {
      get
      {
        if ((UnityEngine.Object) this.m_EventCamera == (UnityEngine.Object) null)
          this.m_EventCamera = this.GetComponent<Camera>();
        return this.m_EventCamera ?? Camera.main;
      }
    }

    /// <summary>
    ///   <para>Get the depth of the configured camera.</para>
    /// </summary>
    public virtual int depth
    {
      get
      {
        return !((UnityEngine.Object) this.eventCamera != (UnityEngine.Object) null) ? 16777215 : (int) this.eventCamera.depth;
      }
    }

    /// <summary>
    ///   <para>Logical and of Camera mask and eventMask.</para>
    /// </summary>
    public int finalEventMask
    {
      get
      {
        return !((UnityEngine.Object) this.eventCamera != (UnityEngine.Object) null) ? -1 : this.eventCamera.cullingMask & (int) this.m_EventMask;
      }
    }

    /// <summary>
    ///   <para>Mask of allowed raycast events.</para>
    /// </summary>
    public LayerMask eventMask
    {
      get
      {
        return this.m_EventMask;
      }
      set
      {
        this.m_EventMask = value;
      }
    }

    /// <summary>
    ///   <para>Max number of ray intersection allowed to be found.</para>
    /// </summary>
    public int maxRayIntersections
    {
      get
      {
        return this.m_MaxRayIntersections;
      }
      set
      {
        this.m_MaxRayIntersections = value;
      }
    }

    protected void ComputeRayAndDistance(PointerEventData eventData, out Ray ray, out float distanceToClipPlane)
    {
      ray = this.eventCamera.ScreenPointToRay((Vector3) eventData.position);
      float z = ray.direction.z;
      distanceToClipPlane = !Mathf.Approximately(0.0f, z) ? Mathf.Abs((this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane) / z) : float.PositiveInfinity;
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      if ((UnityEngine.Object) this.eventCamera == (UnityEngine.Object) null || !this.eventCamera.pixelRect.Contains(eventData.position))
        return;
      Ray ray;
      float distanceToClipPlane;
      this.ComputeRayAndDistance(eventData, out ray, out distanceToClipPlane);
      int num;
      if (this.m_MaxRayIntersections == 0)
      {
        if (ReflectionMethodsCache.Singleton.raycast3DAll == null)
          return;
        this.m_Hits = ReflectionMethodsCache.Singleton.raycast3DAll(ray, distanceToClipPlane, this.finalEventMask);
        num = this.m_Hits.Length;
      }
      else
      {
        if (ReflectionMethodsCache.Singleton.getRaycastNonAlloc == null)
          return;
        if (this.m_LastMaxRayIntersections != this.m_MaxRayIntersections)
        {
          this.m_Hits = new RaycastHit[this.m_MaxRayIntersections];
          this.m_LastMaxRayIntersections = this.m_MaxRayIntersections;
        }
        num = ReflectionMethodsCache.Singleton.getRaycastNonAlloc(ray, this.m_Hits, distanceToClipPlane, this.finalEventMask);
      }
      if (num > 1)
        Array.Sort<RaycastHit>(this.m_Hits, (Comparison<RaycastHit>) ((r1, r2) => r1.distance.CompareTo(r2.distance)));
      if (num == 0)
        return;
      int index1 = 0;
      for (int index2 = num; index1 < index2; ++index1)
      {
        RaycastResult raycastResult = new RaycastResult() { gameObject = this.m_Hits[index1].collider.gameObject, module = (BaseRaycaster) this, distance = this.m_Hits[index1].distance, worldPosition = this.m_Hits[index1].point, worldNormal = this.m_Hits[index1].normal, screenPosition = eventData.position, index = (float) resultAppendList.Count, sortingLayer = 0, sortingOrder = 0 };
        resultAppendList.Add(raycastResult);
      }
    }
  }
}
