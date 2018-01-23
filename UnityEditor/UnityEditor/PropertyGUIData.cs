// Decompiled with JetBrains decompiler
// Type: UnityEditor.PropertyGUIData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal struct PropertyGUIData
  {
    public SerializedProperty property;
    public Rect totalPosition;
    public bool wasBoldDefaultFont;
    public bool wasEnabled;
    public Color color;

    public PropertyGUIData(SerializedProperty property, Rect totalPosition, bool wasBoldDefaultFont, bool wasEnabled, Color color)
    {
      this.property = property;
      this.totalPosition = totalPosition;
      this.wasBoldDefaultFont = wasBoldDefaultFont;
      this.wasEnabled = wasEnabled;
      this.color = color;
    }
  }
}
