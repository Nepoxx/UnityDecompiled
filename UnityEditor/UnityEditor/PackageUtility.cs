// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class PackageUtility
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ExportPackageItem[] BuildExportPackageItemsList(string[] guids, bool dependencies);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ExportPackage(string[] guids, string fileName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ImportPackageItem[] ExtractAndPrepareAssetList(string packagePath, out string packageIconPath, out bool canPerformReInstall);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ImportPackageAssets(string packageName, ImportPackageItem[] items, bool performReInstall);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ImportPackageAssetsImmediately(string packageName, ImportPackageItem[] items, bool performReInstall);
  }
}
