// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialHeaderDecorator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Globalization;
using UnityEngine;

namespace UnityEditor
{
  internal class MaterialHeaderDecorator : MaterialPropertyDrawer
  {
    private readonly string header;

    public MaterialHeaderDecorator(string header)
    {
      this.header = header;
    }

    public MaterialHeaderDecorator(float headerAsNumber)
    {
      this.header = headerAsNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      return 24f;
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
      position.y += 8f;
      position = EditorGUI.IndentedRect(position);
      GUI.Label(position, this.header, EditorStyles.boldLabel);
    }
  }
}
