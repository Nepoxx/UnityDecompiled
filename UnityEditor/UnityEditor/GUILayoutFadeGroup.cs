// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUILayoutFadeGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal sealed class GUILayoutFadeGroup : GUILayoutGroup
  {
    public float fadeValue;
    public bool wasGUIEnabled;
    public Color guiColor;

    public override void CalcHeight()
    {
      base.CalcHeight();
      this.minHeight *= this.fadeValue;
      this.maxHeight *= this.fadeValue;
    }
  }
}
