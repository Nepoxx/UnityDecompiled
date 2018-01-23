// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class AssetStoreUtils
  {
    private const string kAssetStoreUrl = "https://shawarma.unity3d.com";

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK, [DefaultValue("null")] AssetStoreUtils.DownloadDoneCallback doneCallback);

    [ExcludeFromDocs]
    public static void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK)
    {
      AssetStoreUtils.DownloadDoneCallback doneCallback = (AssetStoreUtils.DownloadDoneCallback) null;
      AssetStoreUtils.Download(id, url, destination, key, jsonData, resumeOK, doneCallback);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CheckDownload(string id, string url, string[] destination, string key);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RegisterDownloadDelegate(ScriptableObject d);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UnRegisterDownloadDelegate(ScriptableObject d);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLoaderPath();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UpdatePreloading();

    public static string GetOfflinePath()
    {
      return Uri.EscapeUriString(EditorApplication.applicationContentsPath + "/Resources/offline.html");
    }

    public static string GetAssetStoreUrl()
    {
      return "https://shawarma.unity3d.com";
    }

    public static string GetAssetStoreSearchUrl()
    {
      return AssetStoreUtils.GetAssetStoreUrl().Replace("https", "http");
    }

    public delegate void DownloadDoneCallback(string package_id, string message, int bytes, int total);
  }
}
