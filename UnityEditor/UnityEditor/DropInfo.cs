// Decompiled with JetBrains decompiler
// Type: UnityEditor.DropInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DropInfo
  {
    public object userData = (object) null;
    public DropInfo.Type type = DropInfo.Type.Window;
    public IDropArea dropArea;
    public Rect rect;

    public DropInfo(IDropArea source)
    {
      this.dropArea = source;
    }

    internal enum Type
    {
      Tab,
      Pane,
      Window,
    }
  }
}
