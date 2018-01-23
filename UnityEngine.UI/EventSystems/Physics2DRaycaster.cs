// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.Physics2DRaycaster
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Raycaster for casting against 2D Physics components.</para>
  /// </summary>
  [AddComponentMenu("Event/Physics 2D Raycaster")]
  [RequireComponent(typeof (Camera))]
  public class Physics2DRaycaster : PhysicsRaycaster
  {
    private RaycastHit2D[] m_Hits;

    protected Physics2DRaycaster()
    {
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      if ((Object) this.eventCamera == (Object) null)
        return;
      Ray ray;
      float distanceToClipPlane;
      this.ComputeRayAndDistance(eventData, out ray, out distanceToClipPlane);
      int num;
      if (this.maxRayIntersections == 0)
      {
        if (ReflectionMethodsCache.Singleton.getRayIntersectionAll == null)
          return;
        this.m_Hits = ReflectionMethodsCache.Singleton.getRayIntersectionAll(ray, distanceToClipPlane, this.finalEventMask);
        num = this.m_Hits.Length;
      }
      else
      {
        if (ReflectionMethodsCache.Singleton.getRayIntersectionAllNonAlloc == null)
          return;
        if (this.m_LastMaxRayIntersections != this.m_MaxRayIntersections)
        {
          this.m_Hits = new RaycastHit2D[this.maxRayIntersections];
          this.m_LastMaxRayIntersections = this.m_MaxRayIntersections;
        }
        num = ReflectionMethodsCache.Singleton.getRayIntersectionAllNonAlloc(ray, this.m_Hits, distanceToClipPlane, this.finalEventMask);
      }
      if (num == 0)
        return;
      int index1 = 0;
      for (int index2 = num; index1 < index2; ++index1)
      {
        SpriteRenderer component = this.m_Hits[index1].collider.gameObject.GetComponent<SpriteRenderer>();
        RaycastResult raycastResult = new RaycastResult() { gameObject = this.m_Hits[index1].collider.gameObject, module = (BaseRaycaster) this, distance = Vector3.Distance(this.eventCamera.transform.position, (Vector3) this.m_Hits[index1].point), worldPosition = (Vector3) this.m_Hits[index1].point, worldNormal = (Vector3) this.m_Hits[index1].normal, screenPosition = eventData.position, index = (float) resultAppendList.Count, sortingLayer = !((Object) component != (Object) null) ? 0 : component.sortingLayerID, sortingOrder = !((Object) component != (Object) null) ? 0 : component.sortingOrder };
        resultAppendList.Add(raycastResult);
      }
    }
  }
}
