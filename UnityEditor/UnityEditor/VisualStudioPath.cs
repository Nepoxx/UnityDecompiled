// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioPath
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class VisualStudioPath
  {
    public VisualStudioPath(string path, string edition = "")
    {
      this.Path = path;
      this.Edition = edition;
    }

    public string Path { get; set; }

    public string Edition { get; set; }
  }
}
