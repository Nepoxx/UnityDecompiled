// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ContentZoomer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class ContentZoomer : Manipulator
  {
    public static readonly Vector3 DefaultMinScale = new Vector3(0.1f, 0.1f, 1f);
    public static readonly Vector3 DefaultMaxScale = new Vector3(3f, 3f, 1f);
    private IVisualElementScheduledItem m_OnTimerTicker;

    public ContentZoomer()
    {
      this.zoomStep = 0.01f;
      this.minScale = ContentZoomer.DefaultMinScale;
      this.maxScale = ContentZoomer.DefaultMaxScale;
      this.keepPixelCacheOnZoom = false;
    }

    public ContentZoomer(Vector3 minScale, Vector3 maxScale)
    {
      this.zoomStep = 0.01f;
      this.minScale = minScale;
      this.maxScale = maxScale;
      this.keepPixelCacheOnZoom = false;
    }

    public float zoomStep { get; set; }

    public Vector3 minScale { get; set; }

    public Vector3 maxScale { get; set; }

    public bool keepPixelCacheOnZoom { get; set; }

    private bool delayRepaintScheduled { get; set; }

    protected override void RegisterCallbacksOnTarget()
    {
      if (!(this.target is UnityEditor.Experimental.UIElements.GraphView.GraphView))
        throw new InvalidOperationException("Manipulator can only be added to a GraphView");
      this.target.RegisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnWheel), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnWheel), Capture.NoCapture);
    }

    private void OnTimer(TimerState timerState)
    {
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      if (target.elementPanel != null)
        target.elementPanel.keepPixelCacheOnWorldBoundChange = false;
      this.delayRepaintScheduled = false;
    }

    private void OnWheel(WheelEvent evt)
    {
      UnityEditor.Experimental.UIElements.GraphView.GraphView target = this.target as UnityEditor.Experimental.UIElements.GraphView.GraphView;
      if (target == null)
        return;
      Vector3 position = target.viewTransform.position;
      Vector3 scale = target.viewTransform.scale;
      Vector2 vector2 = this.target.ChangeCoordinatesTo(target.contentViewContainer, evt.localMousePosition);
      float x = vector2.x + target.contentViewContainer.layout.x;
      float y = vector2.y + target.contentViewContainer.layout.y;
      Vector3 vector3_1 = position + Vector3.Scale(new Vector3(x, y, 0.0f), scale);
      Vector3 b = Vector3.one - Vector3.one * evt.delta.y * this.zoomStep;
      b.z = 1f;
      Vector3 vector3_2 = Vector3.Scale(scale, b);
      vector3_2.x = Mathf.Max(Mathf.Min(this.maxScale.x, vector3_2.x), this.minScale.x);
      vector3_2.y = Mathf.Max(Mathf.Min(this.maxScale.y, vector3_2.y), this.minScale.y);
      vector3_2.z = Mathf.Max(Mathf.Min(this.maxScale.z, vector3_2.z), this.minScale.z);
      Vector3 newPosition = vector3_1 - Vector3.Scale(new Vector3(x, y, 0.0f), vector3_2);
      if (target.elementPanel != null && this.keepPixelCacheOnZoom)
      {
        target.elementPanel.keepPixelCacheOnWorldBoundChange = true;
        if (this.m_OnTimerTicker == null)
          this.m_OnTimerTicker = target.schedule.Execute(new System.Action<TimerState>(this.OnTimer));
        this.m_OnTimerTicker.ExecuteLater(500L);
        this.delayRepaintScheduled = true;
      }
      target.UpdateViewTransform(newPosition, vector3_2);
      evt.StopPropagation();
    }
  }
}
