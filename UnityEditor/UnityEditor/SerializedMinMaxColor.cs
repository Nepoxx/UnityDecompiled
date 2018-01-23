// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedMinMaxColor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class SerializedMinMaxColor
  {
    public SerializedProperty maxColor;
    public SerializedProperty minColor;
    public SerializedProperty minMax;

    public SerializedMinMaxColor(SerializedModule m)
    {
      this.Init(m, "curve");
    }

    public SerializedMinMaxColor(SerializedModule m, string name)
    {
      this.Init(m, name);
    }

    private void Init(SerializedModule m, string name)
    {
      this.maxColor = m.GetProperty(name, "maxColor");
      this.minColor = m.GetProperty(name, "minColor");
      this.minMax = m.GetProperty(name, "minMax");
    }
  }
}
