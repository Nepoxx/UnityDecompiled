// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialSpaceDecorator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MaterialSpaceDecorator : MaterialPropertyDrawer
  {
    private readonly float height;

    public MaterialSpaceDecorator()
    {
      this.height = 6f;
    }

    public MaterialSpaceDecorator(float height)
    {
      this.height = height;
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      return this.height;
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
    }
  }
}
