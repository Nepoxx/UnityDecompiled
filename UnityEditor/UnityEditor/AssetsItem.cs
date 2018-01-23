// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Obsolete("AssetsItem class is not used anymore (Asset Server has been removed)")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class AssetsItem
  {
    public string guid;
    public string pathName;
    public string message;
    public string exportedAssetPath;
    public string guidFolder;
    public int enabled;
    public int assetIsDir;
    public int changeFlags;
    public string previewPath;
    public int exists;
  }
}
