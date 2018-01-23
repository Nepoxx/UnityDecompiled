// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ShortcutHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class ShortcutHandler : Manipulator
  {
    private readonly Dictionary<Event, ShortcutDelegate> m_Dictionary;

    public ShortcutHandler(Dictionary<Event, ShortcutDelegate> dictionary)
    {
      this.m_Dictionary = dictionary;
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
    }

    private void OnKeyDown(KeyDownEvent evt)
    {
      if (!this.m_Dictionary.ContainsKey(evt.imguiEvent) || this.m_Dictionary[evt.imguiEvent]() != EventPropagation.Stop)
        return;
      evt.StopPropagation();
    }
  }
}
