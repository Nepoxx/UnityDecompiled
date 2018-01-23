// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorUsageDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (ColorUsageAttribute))]
  internal sealed class ColorUsageDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      ColorUsageAttribute attribute = (ColorUsageAttribute) this.attribute;
      ColorPickerHDRConfig hdrConfig = ColorPickerHDRConfig.Temp(attribute.minBrightness, attribute.maxBrightness, attribute.minExposureValue, attribute.maxExposureValue);
      EditorGUI.BeginChangeCheck();
      Color color = EditorGUI.ColorField(position, label, property.colorValue, true, attribute.showAlpha, attribute.hdr, hdrConfig);
      if (!EditorGUI.EndChangeCheck())
        return;
      property.colorValue = color;
    }
  }
}
