// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.TooltipEvent
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
  internal class TooltipEvent : EventBase<TooltipEvent>, IPropagatableEvent
  {
    public string tooltip { get; set; }

    public Rect rect { get; set; }
  }
}
