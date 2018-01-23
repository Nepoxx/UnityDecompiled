// Decompiled with JetBrains decompiler
// Type: UnityEditor.CacheServerPreferences
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class CacheServerPreferences
  {
    private static bool s_HasPendingChanges = false;
    private static long s_LocalCacheServerUsedSize = -1;
    private static CacheServerPreferences.Constants s_Constants = (CacheServerPreferences.Constants) null;
    private const string kIPAddressKey = "CacheServerIPAddress";
    private const string kIpAddressKeyArgs = "-CacheServerIPAddress";
    private const string kModeKey = "CacheServerMode";
    private const string kDeprecatedEnabledKey = "CacheServerEnabled";
    private static bool s_PrefsLoaded;
    private static CacheServerPreferences.ConnectionState s_ConnectionState;
    private static CacheServerPreferences.CacheServerMode s_CacheServerMode;
    private static string s_CacheServerIPAddress;
    private static int s_LocalCacheServerSize;
    private static bool s_EnableCustomPath;
    private static string s_CachePath;

    public static void ReadPreferences()
    {
      CacheServerPreferences.s_CacheServerIPAddress = EditorPrefs.GetString("CacheServerIPAddress", CacheServerPreferences.s_CacheServerIPAddress);
      CacheServerPreferences.s_CacheServerMode = (CacheServerPreferences.CacheServerMode) EditorPrefs.GetInt("CacheServerMode", !EditorPrefs.GetBool("CacheServerEnabled") ? 2 : 1);
      CacheServerPreferences.s_LocalCacheServerSize = EditorPrefs.GetInt("LocalCacheServerSize", 10);
      CacheServerPreferences.s_CachePath = EditorPrefs.GetString("LocalCacheServerPath");
      CacheServerPreferences.s_EnableCustomPath = EditorPrefs.GetBool("LocalCacheServerCustomPath");
    }

    public static void WritePreferences()
    {
      if (CacheServerPreferences.GetCommandLineRemoteAddressOverride() != null)
        return;
      CacheServerPreferences.CacheServerMode cacheServerMode = (CacheServerPreferences.CacheServerMode) EditorPrefs.GetInt("CacheServerMode");
      string str = EditorPrefs.GetString("LocalCacheServerPath");
      bool flag1 = EditorPrefs.GetBool("LocalCacheServerCustomPath");
      bool flag2 = false;
      if (cacheServerMode != CacheServerPreferences.s_CacheServerMode && cacheServerMode == CacheServerPreferences.CacheServerMode.Local)
        flag2 = true;
      if (CacheServerPreferences.s_EnableCustomPath && str != CacheServerPreferences.s_CachePath)
        flag2 = true;
      if (CacheServerPreferences.s_EnableCustomPath != flag1 && CacheServerPreferences.s_CachePath != LocalCacheServer.GetCacheLocation() && CacheServerPreferences.s_CachePath != "")
        flag2 = true;
      if (flag2)
      {
        CacheServerPreferences.s_LocalCacheServerUsedSize = -1L;
        if (EditorUtility.DisplayDialog("Delete old Cache", (CacheServerPreferences.s_CacheServerMode != CacheServerPreferences.CacheServerMode.Local ? "You have disabled the local cache." : "You have changed the location of the local cache storage.") + " Do you want to delete the old locally cached data at " + LocalCacheServer.GetCacheLocation() + "?", "Delete", "Don't Delete"))
        {
          LocalCacheServer.Clear();
          CacheServerPreferences.s_LocalCacheServerUsedSize = -1L;
        }
      }
      EditorPrefs.SetString("CacheServerIPAddress", CacheServerPreferences.s_CacheServerIPAddress);
      EditorPrefs.SetInt("CacheServerMode", (int) CacheServerPreferences.s_CacheServerMode);
      EditorPrefs.SetInt("LocalCacheServerSize", CacheServerPreferences.s_LocalCacheServerSize);
      EditorPrefs.SetString("LocalCacheServerPath", CacheServerPreferences.s_CachePath);
      EditorPrefs.SetBool("LocalCacheServerCustomPath", CacheServerPreferences.s_EnableCustomPath);
      LocalCacheServer.Setup();
      if (!flag2)
        return;
      GUIUtility.ExitGUI();
    }

    private static string GetCommandLineRemoteAddressOverride()
    {
      string str = (string) null;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      int num = Array.IndexOf<string>(commandLineArgs, "-CacheServerIPAddress");
      if (num >= 0 && commandLineArgs.Length > num + 1)
        str = commandLineArgs[num + 1];
      return str;
    }

    [PreferenceItem("Cache Server")]
    private static void OnGUI()
    {
      EventType type = Event.current.type;
      if (CacheServerPreferences.s_Constants == null)
        CacheServerPreferences.s_Constants = new CacheServerPreferences.Constants();
      if (!InternalEditorUtility.HasTeamLicense())
        GUILayout.Label(EditorGUIUtility.TempContent("You need to have a Pro or Team license to use the cache server.", (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning)), EditorStyles.helpBox, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(!InternalEditorUtility.HasTeamLicense()))
      {
        if (!CacheServerPreferences.s_PrefsLoaded)
        {
          CacheServerPreferences.ReadPreferences();
          if (CacheServerPreferences.s_CacheServerMode != CacheServerPreferences.CacheServerMode.Disabled && CacheServerPreferences.s_ConnectionState == CacheServerPreferences.ConnectionState.Unknown)
            CacheServerPreferences.s_ConnectionState = !InternalEditorUtility.CanConnectToCacheServer() ? CacheServerPreferences.ConnectionState.Failure : CacheServerPreferences.ConnectionState.Success;
          CacheServerPreferences.s_PrefsLoaded = true;
        }
        EditorGUI.BeginChangeCheck();
        string remoteAddressOverride = CacheServerPreferences.GetCommandLineRemoteAddressOverride();
        if (remoteAddressOverride != null)
          EditorGUILayout.HelpBox("Cache Server preferences cannot be modified because a remote address was specified via command line argument. To modify Cache Server preferences, restart Unity without the -CacheServerIPAddress command line argument.", MessageType.Info, true);
        using (new EditorGUI.DisabledScope(remoteAddressOverride != null))
          CacheServerPreferences.s_CacheServerMode = (CacheServerPreferences.CacheServerMode) EditorGUILayout.EnumPopup("Cache Server Mode", (Enum) (CacheServerPreferences.CacheServerMode) (remoteAddressOverride == null ? (int) CacheServerPreferences.s_CacheServerMode : 1), new GUILayoutOption[0]);
        switch (CacheServerPreferences.s_CacheServerMode)
        {
          case CacheServerPreferences.CacheServerMode.Local:
            CacheServerPreferences.s_LocalCacheServerSize = EditorGUILayout.IntSlider(CacheServerPreferences.Styles.maxCacheSize, CacheServerPreferences.s_LocalCacheServerSize, 1, 200, new GUILayoutOption[0]);
            CacheServerPreferences.s_EnableCustomPath = EditorGUILayout.Toggle(CacheServerPreferences.Styles.customCacheLocation, CacheServerPreferences.s_EnableCustomPath, new GUILayoutOption[0]);
            if (CacheServerPreferences.s_EnableCustomPath)
            {
              GUIStyle miniButton = EditorStyles.miniButton;
              GUILayout.BeginHorizontal();
              EditorGUILayout.PrefixLabel(CacheServerPreferences.Styles.cacheFolderLocation, miniButton);
              if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(GUIContent.none, miniButton), !string.IsNullOrEmpty(CacheServerPreferences.s_CachePath) ? new GUIContent(CacheServerPreferences.s_CachePath) : CacheServerPreferences.Styles.browse, FocusType.Passive, miniButton))
              {
                string cachePath = CacheServerPreferences.s_CachePath;
                string path = EditorUtility.OpenFolderPanel(CacheServerPreferences.Styles.browseCacheLocation.text, cachePath, "");
                if (!string.IsNullOrEmpty(path))
                {
                  if (LocalCacheServer.CheckValidCacheLocation(path))
                  {
                    CacheServerPreferences.s_CachePath = path;
                    CacheServerPreferences.WritePreferences();
                  }
                  else
                    EditorUtility.DisplayDialog("Invalid Cache Location", string.Format("The directory {0} contains some files which don't look like Unity Cache server files. Please delete the directory contents or choose another directory.", (object) path), "OK");
                  GUIUtility.ExitGUI();
                }
              }
              GUILayout.EndHorizontal();
            }
            else
              CacheServerPreferences.s_CachePath = "";
            if (LocalCacheServer.CheckCacheLocationExists())
            {
              GUIContent label = EditorGUIUtility.TextContent("Cache size is unknown");
              if (CacheServerPreferences.s_LocalCacheServerUsedSize != -1L)
                label = EditorGUIUtility.TextContent("Cache size is " + EditorUtility.FormatBytes(CacheServerPreferences.s_LocalCacheServerUsedSize));
              GUILayout.BeginHorizontal();
              GUIStyle miniButton = EditorStyles.miniButton;
              EditorGUILayout.PrefixLabel(label, miniButton);
              if (EditorGUI.Button(GUILayoutUtility.GetRect(GUIContent.none, miniButton), CacheServerPreferences.Styles.enumerateCache, miniButton))
                CacheServerPreferences.s_LocalCacheServerUsedSize = !LocalCacheServer.CheckCacheLocationExists() ? 0L : FileUtil.GetDirectorySize(LocalCacheServer.GetCacheLocation());
              GUILayout.EndHorizontal();
              GUILayout.BeginHorizontal();
              EditorGUILayout.PrefixLabel(EditorGUIUtility.blankContent, miniButton);
              if (EditorGUI.Button(GUILayoutUtility.GetRect(GUIContent.none, miniButton), CacheServerPreferences.Styles.cleanCache, miniButton))
              {
                LocalCacheServer.Clear();
                CacheServerPreferences.s_LocalCacheServerUsedSize = 0L;
              }
              GUILayout.EndHorizontal();
            }
            else
            {
              EditorGUILayout.HelpBox("Local cache directory does not exist - please check that you can access the cache folder and are able to write to it", MessageType.Warning, false);
              CacheServerPreferences.s_LocalCacheServerUsedSize = -1L;
            }
            GUILayout.Label(CacheServerPreferences.Styles.cacheFolderLocation.text + ":");
            GUILayout.Label(LocalCacheServer.GetCacheLocation(), CacheServerPreferences.s_Constants.cacheFolderLocation, new GUILayoutOption[0]);
            break;
          case CacheServerPreferences.CacheServerMode.Remote:
            using (new EditorGUI.DisabledScope(remoteAddressOverride != null))
            {
              CacheServerPreferences.s_CacheServerIPAddress = EditorGUILayout.DelayedTextField("IP Address", remoteAddressOverride == null ? CacheServerPreferences.s_CacheServerIPAddress : remoteAddressOverride, new GUILayoutOption[0]);
              if (GUI.changed)
                CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Unknown;
            }
            GUILayout.Space(5f);
            if (GUILayout.Button("Check Connection", new GUILayoutOption[1]{ GUILayout.Width(150f) }))
              CacheServerPreferences.s_ConnectionState = !InternalEditorUtility.CanConnectToCacheServer() ? CacheServerPreferences.ConnectionState.Failure : CacheServerPreferences.ConnectionState.Success;
            GUILayout.Space(-25f);
            switch (CacheServerPreferences.s_ConnectionState)
            {
              case CacheServerPreferences.ConnectionState.Unknown:
                GUILayout.Space(44f);
                break;
              case CacheServerPreferences.ConnectionState.Success:
                EditorGUILayout.HelpBox("Connection successful.", MessageType.Info, false);
                break;
              case CacheServerPreferences.ConnectionState.Failure:
                EditorGUILayout.HelpBox("Connection failed.", MessageType.Warning, false);
                break;
            }
        }
        if (EditorGUI.EndChangeCheck())
          CacheServerPreferences.s_HasPendingChanges = true;
        if (!CacheServerPreferences.s_HasPendingChanges || GUIUtility.hotControl != 0)
          return;
        CacheServerPreferences.s_HasPendingChanges = false;
        CacheServerPreferences.WritePreferences();
        CacheServerPreferences.ReadPreferences();
      }
    }

    internal class Styles
    {
      public static readonly GUIContent browse = EditorGUIUtility.TextContent("Browse...");
      public static readonly GUIContent maxCacheSize = EditorGUIUtility.TextContent("Maximum Cache Size (GB)|The size of the local asset cache server folder will be kept below this maximum value.");
      public static readonly GUIContent customCacheLocation = EditorGUIUtility.TextContent("Custom cache location|Specify the local asset cache server folder location.");
      public static readonly GUIContent cacheFolderLocation = EditorGUIUtility.TextContent("Cache Folder Location|The local asset cache server folder is shared between all projects.");
      public static readonly GUIContent cleanCache = EditorGUIUtility.TextContent("Clean Cache");
      public static readonly GUIContent enumerateCache = EditorGUIUtility.TextContent("Check Cache Size|Check the size of the local asset cache server - can take a while");
      public static readonly GUIContent browseCacheLocation = EditorGUIUtility.TextContent("Browse for local asset cache server location");
    }

    internal class Constants
    {
      public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);

      public Constants()
      {
        this.cacheFolderLocation.wordWrap = true;
      }
    }

    private enum ConnectionState
    {
      Unknown,
      Success,
      Failure,
    }

    public enum CacheServerMode
    {
      Local,
      Remote,
      Disabled,
    }
  }
}
