// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageManager.NativeClient
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
  internal sealed class NativeClient
  {
    public static Dictionary<string, OutdatedPackage> GetOutdatedOperationData(long operationId)
    {
      string[] names;
      OutdatedPackage[] outdatedOperationData = NativeClient.GetOutdatedOperationData(operationId, out names);
      Dictionary<string, OutdatedPackage> dictionary = new Dictionary<string, OutdatedPackage>(names.Length);
      for (int index = 0; index < names.Length; ++index)
        dictionary[names[index]] = outdatedOperationData[index];
      return dictionary;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode List(out long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode Add(out long operationId, string packageId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode Remove(out long operationId, string packageId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode Search(out long operationId, string packageName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode Outdated(out long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode ResetToEditorDefaults(out long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NativeClient.StatusCode GetOperationStatus(long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Error GetOperationError(long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern OperationStatus GetListOperationData(long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UpmPackageInfo GetAddOperationData(long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetRemoveOperationData(long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UpmPackageInfo[] GetSearchOperationData(long operationId);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern OutdatedPackage[] GetOutdatedOperationData(long operationId, out string[] names);

    public enum StatusCode : uint
    {
      InQueue,
      InProgress,
      Done,
      Error,
      NotFound,
    }
  }
}
