// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.GraphicRaycaster
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A BaseRaycaster to raycast against Graphic elements.</para>
  /// </summary>
  [AddComponentMenu("Event/Graphic Raycaster")]
  [RequireComponent(typeof (Canvas))]
  public class GraphicRaycaster : BaseRaycaster
  {
    [NonSerialized]
    private static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();
    [FormerlySerializedAs("ignoreReversedGraphics")]
    [SerializeField]
    private bool m_IgnoreReversedGraphics = true;
    [FormerlySerializedAs("blockingObjects")]
    [SerializeField]
    private GraphicRaycaster.BlockingObjects m_BlockingObjects = GraphicRaycaster.BlockingObjects.None;
    [SerializeField]
    protected LayerMask m_BlockingMask = (LayerMask) -1;
    [NonSerialized]
    private List<Graphic> m_RaycastResults = new List<Graphic>();
    protected const int kNoEventMaskSet = -1;
    private Canvas m_Canvas;

    protected GraphicRaycaster()
    {
    }

    public override int sortOrderPriority
    {
      get
      {
        if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
          return this.canvas.sortingOrder;
        return base.sortOrderPriority;
      }
    }

    public override int renderOrderPriority
    {
      get
      {
        if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
          return this.canvas.rootCanvas.renderOrder;
        return base.renderOrderPriority;
      }
    }

    /// <summary>
    ///   <para>Should graphics facing away from the raycaster be considered?</para>
    /// </summary>
    public bool ignoreReversedGraphics
    {
      get
      {
        return this.m_IgnoreReversedGraphics;
      }
      set
      {
        this.m_IgnoreReversedGraphics = value;
      }
    }

    /// <summary>
    ///   <para>Type of objects that will block graphic raycasts.</para>
    /// </summary>
    public GraphicRaycaster.BlockingObjects blockingObjects
    {
      get
      {
        return this.m_BlockingObjects;
      }
      set
      {
        this.m_BlockingObjects = value;
      }
    }

    private Canvas canvas
    {
      get
      {
        if ((UnityEngine.Object) this.m_Canvas != (UnityEngine.Object) null)
          return this.m_Canvas;
        this.m_Canvas = this.GetComponent<Canvas>();
        return this.m_Canvas;
      }
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      if ((UnityEngine.Object) this.canvas == (UnityEngine.Object) null)
        return;
      IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(this.canvas);
      if (graphicsForCanvas == null || graphicsForCanvas.Count == 0)
        return;
      Camera eventCamera = this.eventCamera;
      int index1 = this.canvas.renderMode == RenderMode.ScreenSpaceOverlay || (UnityEngine.Object) eventCamera == (UnityEngine.Object) null ? this.canvas.targetDisplay : eventCamera.targetDisplay;
      Vector3 position = Display.RelativeMouseAt((Vector3) eventData.position);
      if (position != Vector3.zero)
      {
        if ((int) position.z != index1)
          return;
      }
      else
        position = (Vector3) eventData.position;
      Vector2 vector2;
      if ((UnityEngine.Object) eventCamera == (UnityEngine.Object) null)
      {
        float num1 = (float) Screen.width;
        float num2 = (float) Screen.height;
        if (index1 > 0 && index1 < Display.displays.Length)
        {
          num1 = (float) Display.displays[index1].systemWidth;
          num2 = (float) Display.displays[index1].systemHeight;
        }
        vector2 = new Vector2(position.x / num1, position.y / num2);
      }
      else
        vector2 = (Vector2) eventCamera.ScreenToViewportPoint(position);
      if ((double) vector2.x < 0.0 || (double) vector2.x > 1.0 || ((double) vector2.y < 0.0 || (double) vector2.y > 1.0))
        return;
      float num3 = float.MaxValue;
      Ray r = new Ray();
      if ((UnityEngine.Object) eventCamera != (UnityEngine.Object) null)
        r = eventCamera.ScreenPointToRay(position);
      if (this.canvas.renderMode != RenderMode.ScreenSpaceOverlay && this.blockingObjects != GraphicRaycaster.BlockingObjects.None)
      {
        float f = 100f;
        if ((UnityEngine.Object) eventCamera != (UnityEngine.Object) null)
        {
          float z = r.direction.z;
          f = !Mathf.Approximately(0.0f, z) ? Mathf.Abs((eventCamera.farClipPlane - eventCamera.nearClipPlane) / z) : float.PositiveInfinity;
        }
        if ((this.blockingObjects == GraphicRaycaster.BlockingObjects.ThreeD || this.blockingObjects == GraphicRaycaster.BlockingObjects.All) && ReflectionMethodsCache.Singleton.raycast3D != null)
        {
          RaycastHit[] raycastHitArray = ReflectionMethodsCache.Singleton.raycast3DAll(r, f, (int) this.m_BlockingMask);
          if (raycastHitArray.Length > 0)
            num3 = raycastHitArray[0].distance;
        }
        if ((this.blockingObjects == GraphicRaycaster.BlockingObjects.TwoD || this.blockingObjects == GraphicRaycaster.BlockingObjects.All) && ReflectionMethodsCache.Singleton.raycast2D != null)
        {
          RaycastHit2D[] raycastHit2DArray = ReflectionMethodsCache.Singleton.getRayIntersectionAll(r, f, (int) this.m_BlockingMask);
          if (raycastHit2DArray.Length > 0)
            num3 = raycastHit2DArray[0].distance;
        }
      }
      this.m_RaycastResults.Clear();
      GraphicRaycaster.Raycast(this.canvas, eventCamera, (Vector2) position, graphicsForCanvas, this.m_RaycastResults);
      int count = this.m_RaycastResults.Count;
      for (int index2 = 0; index2 < count; ++index2)
      {
        GameObject gameObject = this.m_RaycastResults[index2].gameObject;
        bool flag = true;
        if (this.ignoreReversedGraphics)
          flag = !((UnityEngine.Object) eventCamera == (UnityEngine.Object) null) ? (double) Vector3.Dot(eventCamera.transform.rotation * Vector3.forward, gameObject.transform.rotation * Vector3.forward) > 0.0 : (double) Vector3.Dot(Vector3.forward, gameObject.transform.rotation * Vector3.forward) > 0.0;
        if (flag)
        {
          float num1;
          if ((UnityEngine.Object) eventCamera == (UnityEngine.Object) null || this.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
          {
            num1 = 0.0f;
          }
          else
          {
            Transform transform = gameObject.transform;
            Vector3 forward = transform.forward;
            num1 = Vector3.Dot(forward, transform.position - eventCamera.transform.position) / Vector3.Dot(forward, r.direction);
            if ((double) num1 < 0.0)
              continue;
          }
          if ((double) num1 < (double) num3)
          {
            RaycastResult raycastResult = new RaycastResult() { gameObject = gameObject, module = (BaseRaycaster) this, distance = num1, screenPosition = (Vector2) position, index = (float) resultAppendList.Count, depth = this.m_RaycastResults[index2].depth, sortingLayer = this.canvas.sortingLayerID, sortingOrder = this.canvas.sortingOrder };
            resultAppendList.Add(raycastResult);
          }
        }
      }
    }

    /// <summary>
    ///   <para>See: BaseRaycaster.</para>
    /// </summary>
    public override Camera eventCamera
    {
      get
      {
        if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay || this.canvas.renderMode == RenderMode.ScreenSpaceCamera && (UnityEngine.Object) this.canvas.worldCamera == (UnityEngine.Object) null)
          return (Camera) null;
        return !((UnityEngine.Object) this.canvas.worldCamera != (UnityEngine.Object) null) ? Camera.main : this.canvas.worldCamera;
      }
    }

    private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, List<Graphic> results)
    {
      int count1 = foundGraphics.Count;
      for (int index = 0; index < count1; ++index)
      {
        Graphic foundGraphic = foundGraphics[index];
        if (foundGraphic.depth != -1 && foundGraphic.raycastTarget && (!foundGraphic.canvasRenderer.cull && RectTransformUtility.RectangleContainsScreenPoint(foundGraphic.rectTransform, pointerPosition, eventCamera)) && (!((UnityEngine.Object) eventCamera != (UnityEngine.Object) null) || (double) eventCamera.WorldToScreenPoint(foundGraphic.rectTransform.position).z <= (double) eventCamera.farClipPlane) && foundGraphic.Raycast(pointerPosition, eventCamera))
          GraphicRaycaster.s_SortedGraphics.Add(foundGraphic);
      }
      GraphicRaycaster.s_SortedGraphics.Sort((Comparison<Graphic>) ((g1, g2) => g2.depth.CompareTo(g1.depth)));
      int count2 = GraphicRaycaster.s_SortedGraphics.Count;
      for (int index = 0; index < count2; ++index)
        results.Add(GraphicRaycaster.s_SortedGraphics[index]);
      GraphicRaycaster.s_SortedGraphics.Clear();
    }

    /// <summary>
    ///   <para>List of Raycasters to check for canvas blocking elements.</para>
    /// </summary>
    public enum BlockingObjects
    {
      None,
      TwoD,
      ThreeD,
      All,
    }
  }
}
