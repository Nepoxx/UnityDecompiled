// Decompiled with JetBrains decompiler
// Type: UnityEditor.ImportPackageItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class ImportPackageItem
  {
    public string exportedAssetPath;
    public string destinationAssetPath;
    public string sourceFolder;
    public string previewPath;
    public string guid;
    public int enabledStatus;
    public bool isFolder;
    public bool exists;
    public bool assetChanged;
    public bool pathConflict;
    public bool projectAsset;
  }
}
