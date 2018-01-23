// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialToggleOffDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class MaterialToggleOffDrawer : MaterialToggleDrawer
  {
    public MaterialToggleOffDrawer()
    {
    }

    public MaterialToggleOffDrawer(string keyword)
      : base(keyword)
    {
    }

    protected override void SetKeyword(MaterialProperty prop, bool on)
    {
      this.SetKeywordInternal(prop, !on, "_OFF");
    }
  }
}
