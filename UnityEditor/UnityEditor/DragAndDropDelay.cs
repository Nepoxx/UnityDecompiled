// Decompiled with JetBrains decompiler
// Type: UnityEditor.DragAndDropDelay
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DragAndDropDelay
  {
    public Vector2 mouseDownPosition;

    public bool CanStartDrag()
    {
      return (double) Vector2.Distance(this.mouseDownPosition, Event.current.mousePosition) > 6.0;
    }
  }
}
