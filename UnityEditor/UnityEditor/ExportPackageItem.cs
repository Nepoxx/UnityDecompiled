// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExportPackageItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class ExportPackageItem
  {
    public string assetPath;
    public string guid;
    public bool isFolder;
    public int enabledStatus;
  }
}
