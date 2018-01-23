// Decompiled with JetBrains decompiler
// Type: UnityEditor.SliderLabels
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal struct SliderLabels
  {
    public GUIContent leftLabel;
    public GUIContent rightLabel;

    public void SetLabels(GUIContent leftLabel, GUIContent rightLabel)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.leftLabel = leftLabel;
      this.rightLabel = rightLabel;
    }

    public bool HasLabels()
    {
      if (Event.current.type == EventType.Repaint)
        return this.leftLabel != null && this.rightLabel != null;
      return false;
    }
  }
}
