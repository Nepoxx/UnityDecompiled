// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.DragAndDropDelay
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class DragAndDropDelay
  {
    private const float k_StartDragTreshold = 4f;

    private Vector2 mouseDownPosition { get; set; }

    public void Init(Vector2 mousePosition)
    {
      this.mouseDownPosition = mousePosition;
    }

    public bool CanStartDrag(Vector2 mousePosition)
    {
      return (double) Vector2.Distance(this.mouseDownPosition, mousePosition) > 4.0;
    }
  }
}
