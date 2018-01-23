// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreferencesWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Collaboration;
using UnityEditor.Connect;
using UnityEditor.Modules;
using UnityEditor.VisualStudioIntegration;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class PreferencesWindow : EditorWindow
  {
    private static PreferencesWindow.Constants constants = (PreferencesWindow.Constants) null;
    private static Vector2 s_ScrollPosition = Vector2.zero;
    private static int kMinSpriteCacheSizeInGigabytes = 1;
    private static int kMaxSpriteCacheSizeInGigabytes = 200;
    private static Regex s_VersionPattern = new Regex("(?<shortVersion>\\d+\\.\\d+\\.\\d+(?<suffix>((?<alphabeta>[abx])|[fp])[^\\s]*))( \\((?<revision>[a-fA-F\\d]+)\\))?", RegexOptions.Compiled);
    private static int s_KeysControlHash = "KeysControlHash".GetHashCode();
    private PreferencesWindow.RefString m_ScriptEditorPath = new PreferencesWindow.RefString("");
    private string m_ScriptEditorArgs = "";
    private PreferencesWindow.RefString m_ImageAppPath = new PreferencesWindow.RefString("");
    private SystemLanguage m_SelectedLanguage = SystemLanguage.English;
    private SystemLanguage[] m_stableLanguages = new SystemLanguage[1]{ SystemLanguage.English };
    private bool m_AllowAlphaNumericHierarchy = false;
    private string m_noDiffToolsMessage = string.Empty;
    private PrefKey m_SelectedKey = (PrefKey) null;
    private SortedDictionary<string, List<KeyValuePair<string, PrefColor>>> s_CachedColors = (SortedDictionary<string, List<KeyValuePair<string, PrefColor>>>) null;
    private bool m_ValidKeyChange = true;
    private string m_InvalidKeyMessage = string.Empty;
    private List<PreferencesWindow.Section> m_Sections;
    private int m_SelectedSectionIndex;
    private List<IPreferenceWindowExtension> prefWinExtensions;
    private bool m_AutoRefresh;
    private bool m_ReopenLastUsedProjectOnStartup;
    private bool m_CompressAssetsOnImport;
    private bool m_UseOSColorPicker;
    private bool m_EnableEditorAnalytics;
    private bool m_ShowAssetStoreSearchHits;
    private bool m_VerifySavingAssets;
    private bool m_AllowAttachedDebuggingOfEditor;
    private bool m_AllowAttachedDebuggingOfEditorStateChangedThisSession;
    private string m_GpuDevice;
    private string[] m_CachedGpuDevices;
    private PreferencesWindow.GICacheSettings m_GICacheSettings;
    private bool m_ExternalEditorSupportsUnityProj;
    private int m_DiffToolIndex;
    private const int k_LangListMenuOffset = 2;
    private string[] m_EditorLanguageNames;
    private bool m_EnableEditorLocalization;
    private string[] m_ScriptApps;
    private string[] m_ScriptAppsEditions;
    private string[] m_ImageApps;
    private string[] m_DiffTools;
    private bool m_RefreshCustomPreferences;
    private string[] m_ScriptAppDisplayNames;
    private string[] m_ImageAppDisplayNames;
    private Vector2 m_KeyScrollPos;
    private Vector2 m_SectionScrollPos;
    private const string kRecentScriptAppsKey = "RecentlyUsedScriptApp";
    private const string kRecentImageAppsKey = "RecentlyUsedImageApp";
    private const string m_ExpressNotSupportedMessage = "Unfortunately Visual Studio Express does not allow itself to be controlled by external applications. You can still use it by manually opening the Visual Studio project file, but Unity cannot automatically open files for you when you doubleclick them. \n(This does work with Visual Studio Pro)";
    private const int kRecentAppsCount = 10;
    private int m_SpriteAtlasCacheSize;

    private int selectedSectionIndex
    {
      get
      {
        return this.m_SelectedSectionIndex;
      }
      set
      {
        if (this.m_SelectedSectionIndex != value)
          this.m_ValidKeyChange = true;
        this.m_SelectedSectionIndex = value;
        if (this.m_SelectedSectionIndex >= this.m_Sections.Count)
        {
          this.m_SelectedSectionIndex = 0;
        }
        else
        {
          if (this.m_SelectedSectionIndex >= 0)
            return;
          this.m_SelectedSectionIndex = this.m_Sections.Count - 1;
        }
      }
    }

    private PreferencesWindow.Section selectedSection
    {
      get
      {
        return this.m_Sections[this.m_SelectedSectionIndex];
      }
    }

    private static void ShowPreferencesWindow()
    {
      EditorWindow window = (EditorWindow) EditorWindow.GetWindow<PreferencesWindow>(true, "Unity Preferences");
      window.minSize = new Vector2(500f, 400f);
      window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
      window.position = new Rect(new Vector2(100f, 100f), window.minSize);
      window.m_Parent.window.m_DontSaveToLayout = true;
    }

    private void OnEnable()
    {
      this.prefWinExtensions = ModuleManager.GetPreferenceWindowExtensions();
      this.ReadPreferences();
      this.m_Sections = new List<PreferencesWindow.Section>();
      this.m_Sections.Add(new PreferencesWindow.Section("General", new PreferencesWindow.OnGUIDelegate(this.ShowGeneral)));
      this.m_Sections.Add(new PreferencesWindow.Section("External Tools", new PreferencesWindow.OnGUIDelegate(this.ShowExternalApplications)));
      this.m_Sections.Add(new PreferencesWindow.Section("Colors", new PreferencesWindow.OnGUIDelegate(this.ShowColors)));
      this.m_Sections.Add(new PreferencesWindow.Section("Keys", new PreferencesWindow.OnGUIDelegate(this.ShowKeys)));
      this.m_Sections.Add(new PreferencesWindow.Section("GI Cache", new PreferencesWindow.OnGUIDelegate(this.ShowGICache)));
      this.m_Sections.Add(new PreferencesWindow.Section("2D", new PreferencesWindow.OnGUIDelegate(this.Show2D)));
      if (Unsupported.IsDeveloperBuild() || UnityConnect.preferencesEnabled)
        this.m_Sections.Add(new PreferencesWindow.Section("Unity Services", new PreferencesWindow.OnGUIDelegate(this.ShowUnityConnectPrefs)));
      this.m_RefreshCustomPreferences = true;
    }

    private void AddCustomSections()
    {
      foreach (AttributeHelper.MethodWithAttribute methodsWithAttribute in AttributeHelper.GetMethodsWithAttribute<PreferenceItem>(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MethodsWithAttributes)
      {
        PreferencesWindow.OnGUIDelegate guiFunc = Delegate.CreateDelegate(typeof (PreferencesWindow.OnGUIDelegate), methodsWithAttribute.info) as PreferencesWindow.OnGUIDelegate;
        if (guiFunc != null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PreferencesWindow.\u003CAddCustomSections\u003Ec__AnonStorey0 sectionsCAnonStorey0 = new PreferencesWindow.\u003CAddCustomSections\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          sectionsCAnonStorey0.attributeName = (methodsWithAttribute.attribute as PreferenceItem).name;
          // ISSUE: reference to a compiler-generated method
          int index = this.m_Sections.FindIndex(new Predicate<PreferencesWindow.Section>(sectionsCAnonStorey0.\u003C\u003Em__0));
          if (index >= 0)
          {
            this.m_Sections[index].guiFunc += guiFunc;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Sections.Add(new PreferencesWindow.Section(sectionsCAnonStorey0.attributeName, guiFunc));
          }
        }
      }
    }

    private void OnGUI()
    {
      if (this.m_RefreshCustomPreferences)
      {
        this.AddCustomSections();
        this.m_RefreshCustomPreferences = false;
      }
      EditorGUIUtility.labelWidth = 200f;
      if (PreferencesWindow.constants == null)
        PreferencesWindow.constants = new PreferencesWindow.Constants();
      this.HandleKeys();
      GUILayout.BeginHorizontal();
      this.m_SectionScrollPos = GUILayout.BeginScrollView(this.m_SectionScrollPos, PreferencesWindow.constants.sectionScrollView, GUILayout.Width(120f));
      GUILayout.Space(40f);
      for (int index = 0; index < this.m_Sections.Count; ++index)
      {
        PreferencesWindow.Section section = this.m_Sections[index];
        Rect rect = GUILayoutUtility.GetRect(section.content, PreferencesWindow.constants.sectionElement, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
        if (section == this.selectedSection && Event.current.type == EventType.Repaint)
          PreferencesWindow.constants.selected.Draw(rect, false, false, false, false);
        EditorGUI.BeginChangeCheck();
        if (GUI.Toggle(rect, this.selectedSectionIndex == index, section.content, PreferencesWindow.constants.sectionElement))
          this.selectedSectionIndex = index;
        if (EditorGUI.EndChangeCheck())
          GUIUtility.keyboardControl = 0;
      }
      GUILayout.EndScrollView();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      GUILayout.Label(this.selectedSection.content, PreferencesWindow.constants.sectionHeader, new GUILayoutOption[0]);
      GUILayout.Space(10f);
      PreferencesWindow.s_ScrollPosition = EditorGUILayout.BeginScrollView(PreferencesWindow.s_ScrollPosition);
      this.selectedSection.guiFunc();
      EditorGUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private void HandleKeys()
    {
      if (Event.current.type != EventType.KeyDown || GUIUtility.keyboardControl != 0)
        return;
      switch (Event.current.keyCode)
      {
        case KeyCode.UpArrow:
          --this.selectedSectionIndex;
          Event.current.Use();
          break;
        case KeyCode.DownArrow:
          ++this.selectedSectionIndex;
          Event.current.Use();
          break;
      }
    }

    private string GetMonoDevelopInstallerURL()
    {
      string fullUnityVersion = InternalEditorUtility.GetFullUnityVersion();
      string str1 = "";
      Match match = PreferencesWindow.s_VersionPattern.Match(fullUnityVersion);
      if (!match.Success || !match.Groups["suffix"].Success)
        Debug.LogWarningFormat("Error parsing version '{0}'", (object) fullUnityVersion);
      if (match.Groups["revision"].Success)
        str1 = match.Groups["revision"].Value;
      string str2 = "download";
      string str3 = "download_unity";
      string str4 = "Unknown";
      if (match.Groups["alphabeta"].Success)
      {
        str2 = "beta";
        str3 = "download";
      }
      string str5 = "Unsupported";
      switch (Application.platform)
      {
        case RuntimePlatform.OSXEditor:
          str4 = "MacMonoDevelopInstaller";
          str5 = "UnityMonoDevelop.pkg";
          break;
        case RuntimePlatform.WindowsEditor:
          str4 = "WindowsMonoDevelopInstaller";
          str5 = "UnityMonoDevelopSetup.exe";
          break;
      }
      return string.Format("http://{0}.unity3d.com/{1}/{2}/{3}/{4}", (object) str2, (object) str3, (object) str1, (object) str4, (object) str5);
    }

    private void ShowExternalApplications()
    {
      this.FilePopup("External Script Editor", (string) this.m_ScriptEditorPath, ref this.m_ScriptAppDisplayNames, ref this.m_ScriptApps, this.m_ScriptEditorPath, "internal", new Action(this.OnScriptEditorChanged));
      if (!this.IsSelectedScriptEditorSpecial())
      {
        string scriptEditorArgs = this.m_ScriptEditorArgs;
        this.m_ScriptEditorArgs = EditorGUILayout.TextField("External Script Editor Args", this.m_ScriptEditorArgs, new GUILayoutOption[0]);
        if (scriptEditorArgs != this.m_ScriptEditorArgs)
          this.OnScriptEditorArgsChanged();
      }
      else
      {
        string internalEditorPath = EditorUtility.GetInternalEditorPath();
        if (internalEditorPath != null && !Directory.Exists(internalEditorPath) && !File.Exists(internalEditorPath))
        {
          if (GUILayout.Button("Download MonoDevelop Installer", new GUILayoutOption[1]{ GUILayout.Width(220f) }))
            Help.BrowseURL(this.GetMonoDevelopInstallerURL());
        }
      }
      this.DoUnityProjCheckbox();
      bool debuggingOfEditor = this.m_AllowAttachedDebuggingOfEditor;
      this.m_AllowAttachedDebuggingOfEditor = EditorGUILayout.Toggle("Editor Attaching", this.m_AllowAttachedDebuggingOfEditor, new GUILayoutOption[0]);
      if (debuggingOfEditor != this.m_AllowAttachedDebuggingOfEditor)
        this.m_AllowAttachedDebuggingOfEditorStateChangedThisSession = true;
      if (this.m_AllowAttachedDebuggingOfEditorStateChangedThisSession)
        GUILayout.Label("Changing this setting requires a restart to take effect.", EditorStyles.helpBox, new GUILayoutOption[0]);
      if (this.GetSelectedScriptEditor() == ScriptEditorUtility.ScriptEditor.VisualStudioExpress)
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("", PreferencesWindow.constants.warningIcon, new GUILayoutOption[0]);
        GUILayout.Label("Unfortunately Visual Studio Express does not allow itself to be controlled by external applications. You can still use it by manually opening the Visual Studio project file, but Unity cannot automatically open files for you when you doubleclick them. \n(This does work with Visual Studio Pro)", PreferencesWindow.constants.errorLabel, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
      }
      GUILayout.Space(10f);
      this.FilePopup("Image application", (string) this.m_ImageAppPath, ref this.m_ImageAppDisplayNames, ref this.m_ImageApps, this.m_ImageAppPath, "internal", (Action) null);
      GUILayout.Space(10f);
      using (new EditorGUI.DisabledScope(!InternalEditorUtility.HasTeamLicense()))
        this.m_DiffToolIndex = EditorGUILayout.Popup("Revision Control Diff/Merge", this.m_DiffToolIndex, this.m_DiffTools, new GUILayoutOption[0]);
      if (this.m_noDiffToolsMessage != string.Empty)
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("", PreferencesWindow.constants.warningIcon, new GUILayoutOption[0]);
        GUILayout.Label(this.m_noDiffToolsMessage, PreferencesWindow.constants.errorLabel, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
      }
      GUILayout.Space(10f);
      foreach (IPreferenceWindowExtension prefWinExtension in this.prefWinExtensions)
      {
        if (prefWinExtension.HasExternalApplications())
        {
          GUILayout.Space(10f);
          prefWinExtension.ShowExternalApplications();
        }
      }
      this.ApplyChangesToPrefs(false);
    }

    private void DoUnityProjCheckbox()
    {
      bool flag1 = false;
      bool flag2 = false;
      switch (this.GetSelectedScriptEditor())
      {
        case ScriptEditorUtility.ScriptEditor.Internal:
          flag2 = true;
          break;
        case ScriptEditorUtility.ScriptEditor.MonoDevelop:
          flag1 = true;
          flag2 = this.m_ExternalEditorSupportsUnityProj;
          break;
      }
      using (new EditorGUI.DisabledScope(!flag1))
        flag2 = EditorGUILayout.Toggle("Add .unityproj's to .sln", flag2, new GUILayoutOption[0]);
      if (!flag1)
        return;
      this.m_ExternalEditorSupportsUnityProj = flag2;
    }

    private bool IsSelectedScriptEditorSpecial()
    {
      return ScriptEditorUtility.IsScriptEditorSpecial(this.m_ScriptEditorPath.str);
    }

    private ScriptEditorUtility.ScriptEditor GetSelectedScriptEditor()
    {
      return ScriptEditorUtility.GetScriptEditorFromPath(this.m_ScriptEditorPath.str);
    }

    private void OnScriptEditorChanged()
    {
      ScriptEditorUtility.SetExternalScriptEditor((string) this.m_ScriptEditorPath);
      this.m_ScriptEditorArgs = ScriptEditorUtility.GetExternalScriptEditorArgs();
      UnityVSSupport.ScriptEditorChanged(this.m_ScriptEditorPath.str);
    }

    private void OnScriptEditorArgsChanged()
    {
      ScriptEditorUtility.SetExternalScriptEditorArgs(this.m_ScriptEditorArgs);
    }

    private void ShowUnityConnectPrefs()
    {
      UnityConnectPrefs.ShowPanelPrefUI();
      this.ApplyChangesToPrefs(false);
    }

    private void ShowGeneral()
    {
      bool disabled = Collab.instance.IsCollabEnabledForCurrentProject();
      using (new EditorGUI.DisabledScope(disabled))
      {
        if (disabled)
        {
          EditorGUILayout.Toggle("Auto Refresh", true, new GUILayoutOption[0]);
          EditorGUILayout.HelpBox("Auto Refresh must be set when using Collaboration feature.", MessageType.Warning);
        }
        else
          this.m_AutoRefresh = EditorGUILayout.Toggle("Auto Refresh", this.m_AutoRefresh, new GUILayoutOption[0]);
      }
      this.m_ReopenLastUsedProjectOnStartup = EditorGUILayout.Toggle("Load Previous Project on Startup", this.m_ReopenLastUsedProjectOnStartup, new GUILayoutOption[0]);
      bool compressAssetsOnImport = this.m_CompressAssetsOnImport;
      this.m_CompressAssetsOnImport = EditorGUILayout.Toggle("Compress Assets on Import", compressAssetsOnImport, new GUILayoutOption[0]);
      if (GUI.changed && this.m_CompressAssetsOnImport != compressAssetsOnImport)
        Unsupported.SetApplicationSettingCompressAssetsOnImport(this.m_CompressAssetsOnImport);
      if (Application.platform == RuntimePlatform.OSXEditor)
        this.m_UseOSColorPicker = EditorGUILayout.Toggle("OS X Color Picker", this.m_UseOSColorPicker, new GUILayoutOption[0]);
      bool flag1 = Application.HasProLicense();
      using (new EditorGUI.DisabledScope(!flag1))
      {
        this.m_EnableEditorAnalytics = !EditorGUILayout.Toggle("Disable Editor Analytics (Pro Only)", !this.m_EnableEditorAnalytics, new GUILayoutOption[0]);
        if (!flag1)
        {
          if (!this.m_EnableEditorAnalytics)
            this.m_EnableEditorAnalytics = true;
        }
      }
      bool flag2 = false;
      EditorGUI.BeginChangeCheck();
      this.m_ShowAssetStoreSearchHits = EditorGUILayout.Toggle("Show Asset Store search hits", this.m_ShowAssetStoreSearchHits, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        flag2 = true;
      this.m_VerifySavingAssets = EditorGUILayout.Toggle("Verify Saving Assets", this.m_VerifySavingAssets, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(!flag1))
      {
        if ((EditorGUIUtility.isProSkin ? 1 : 0) != EditorGUILayout.Popup("Editor Skin", EditorGUIUtility.isProSkin ? 1 : 0, new string[2]{ "Personal", "Professional" }, new GUILayoutOption[0]))
          InternalEditorUtility.SwitchSkinAndRepaintAllViews();
      }
      bool numericHierarchy = this.m_AllowAlphaNumericHierarchy;
      this.m_AllowAlphaNumericHierarchy = EditorGUILayout.Toggle("Enable Alpha Numeric Sorting", this.m_AllowAlphaNumericHierarchy, new GUILayoutOption[0]);
      if (InternalEditorUtility.IsGpuDeviceSelectionSupported())
      {
        if (this.m_CachedGpuDevices == null)
        {
          string[] gpuDevices = InternalEditorUtility.GetGpuDevices();
          this.m_CachedGpuDevices = new string[gpuDevices.Length + 1];
          this.m_CachedGpuDevices[0] = "Automatic";
          Array.Copy((Array) gpuDevices, 0, (Array) this.m_CachedGpuDevices, 1, gpuDevices.Length);
        }
        int selectedIndex = Array.FindIndex<string>(this.m_CachedGpuDevices, (Predicate<string>) (gpuDevice => this.m_GpuDevice == gpuDevice));
        if (selectedIndex == -1)
          selectedIndex = 0;
        int index = EditorGUILayout.Popup("Device To Use", selectedIndex, this.m_CachedGpuDevices, new GUILayoutOption[0]);
        if (selectedIndex != index)
        {
          this.m_GpuDevice = this.m_CachedGpuDevices[index];
          InternalEditorUtility.SetGpuDeviceAndRecreateGraphics(index - 1, this.m_GpuDevice);
        }
      }
      this.ApplyChangesToPrefs(false);
      if (numericHierarchy != this.m_AllowAlphaNumericHierarchy)
        EditorApplication.DirtyHierarchyWindowSorting();
      if (!flag2)
        return;
      ProjectBrowser.ShowAssetStoreHitsWhileSearchingLocalAssetsChanged();
    }

    public void ApplyChangesToPrefs(bool force = false)
    {
      if (!GUI.changed && !force)
        return;
      this.WritePreferences();
      this.ReadPreferences();
      this.Repaint();
    }

    private void RevertKeys()
    {
      foreach (KeyValuePair<string, PrefKey> pref in Settings.Prefs<PrefKey>())
      {
        pref.Value.ResetToDefault();
        EditorPrefs.SetString(pref.Value.Name, pref.Value.ToUniqueString());
      }
    }

    private SortedDictionary<string, List<KeyValuePair<string, T>>> OrderPrefs<T>(IEnumerable<KeyValuePair<string, T>> input) where T : IPrefType
    {
      SortedDictionary<string, List<KeyValuePair<string, T>>> sortedDictionary = new SortedDictionary<string, List<KeyValuePair<string, T>>>();
      foreach (KeyValuePair<string, T> keyValuePair in input)
      {
        int length = keyValuePair.Key.IndexOf('/');
        string key1;
        string key2;
        if (length == -1)
        {
          key1 = "General";
          key2 = keyValuePair.Key;
        }
        else
        {
          key1 = keyValuePair.Key.Substring(0, length);
          key2 = keyValuePair.Key.Substring(length + 1);
        }
        if (!sortedDictionary.ContainsKey(key1))
          sortedDictionary.Add(key1, new List<KeyValuePair<string, T>>((IEnumerable<KeyValuePair<string, T>>) new List<KeyValuePair<string, T>>()
          {
            new KeyValuePair<string, T>(key2, keyValuePair.Value)
          }));
        else
          sortedDictionary[key1].Add(new KeyValuePair<string, T>(key2, keyValuePair.Value));
      }
      return sortedDictionary;
    }

    private void ShowKeys()
    {
      int controlId = GUIUtility.GetControlID(PreferencesWindow.s_KeysControlHash, FocusType.Keyboard);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical(GUILayout.Width(185f));
      GUILayout.Label("Actions", PreferencesWindow.constants.settingsBoxTitle, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      this.m_KeyScrollPos = GUILayout.BeginScrollView(this.m_KeyScrollPos, PreferencesWindow.constants.settingsBox);
      PrefKey prefKey1 = (PrefKey) null;
      PrefKey prefKey2 = (PrefKey) null;
      bool flag = false;
      foreach (KeyValuePair<string, PrefKey> pref in Settings.Prefs<PrefKey>())
      {
        if (!flag)
        {
          if (pref.Value == this.m_SelectedKey)
            flag = true;
          else
            prefKey1 = pref.Value;
        }
        else if (prefKey2 == null)
          prefKey2 = pref.Value;
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Toggle(pref.Value == this.m_SelectedKey, pref.Key, PreferencesWindow.constants.keysElement, new GUILayoutOption[0]))
        {
          if (this.m_SelectedKey != pref.Value)
            this.m_ValidKeyChange = true;
          this.m_SelectedKey = pref.Value;
        }
        if (EditorGUI.EndChangeCheck())
          GUIUtility.keyboardControl = controlId;
      }
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      if (this.m_SelectedKey != null)
      {
        Event keyboardEvent = this.m_SelectedKey.KeyboardEvent;
        GUI.changed = false;
        string[] strArray = this.m_SelectedKey.Name.Split('/');
        GUILayout.Label(strArray[0], (GUIStyle) "boldLabel", new GUILayoutOption[0]);
        GUILayout.Label(strArray[1], (GUIStyle) "boldLabel", new GUILayoutOption[0]);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Key:");
        Event @event = EditorGUILayout.KeyEventField(keyboardEvent);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Modifiers:");
        GUILayout.BeginVertical();
        if (Application.platform == RuntimePlatform.OSXEditor)
          @event.command = GUILayout.Toggle(@event.command, "Command");
        @event.control = GUILayout.Toggle(@event.control, "Control");
        @event.shift = GUILayout.Toggle(@event.shift, "Shift");
        @event.alt = GUILayout.Toggle(@event.alt, "Alt");
        if (GUI.changed)
        {
          this.m_ValidKeyChange = true;
          string str1 = this.m_SelectedKey.Name.Split('/')[0];
          foreach (KeyValuePair<string, PrefKey> pref in Settings.Prefs<PrefKey>())
          {
            string str2 = pref.Key.Split('/')[0];
            if (pref.Value.KeyboardEvent.Equals((object) @event) && str2 == str1 && pref.Key != this.m_SelectedKey.Name)
            {
              this.m_ValidKeyChange = false;
              StringBuilder stringBuilder = new StringBuilder();
              if (Application.platform == RuntimePlatform.OSXEditor && @event.command)
                stringBuilder.Append("Command+");
              if (@event.control)
                stringBuilder.Append("Ctrl+");
              if (@event.shift)
                stringBuilder.Append("Shift+");
              if (@event.alt)
                stringBuilder.Append("Alt+");
              stringBuilder.Append((object) @event.keyCode);
              this.m_InvalidKeyMessage = string.Format("Key {0} can't be used for action \"{1}\" because it's already used for action \"{2}\"", (object) stringBuilder, (object) this.m_SelectedKey.Name, (object) pref.Key);
              break;
            }
          }
          if (this.m_ValidKeyChange)
          {
            this.m_SelectedKey.KeyboardEvent = @event;
            Settings.Set<PrefKey>(this.m_SelectedKey.Name, this.m_SelectedKey);
          }
        }
        else if (GUIUtility.keyboardControl == controlId && Event.current.type == EventType.KeyDown)
        {
          switch (Event.current.keyCode)
          {
            case KeyCode.UpArrow:
              if (prefKey1 != null && prefKey1 != this.m_SelectedKey)
              {
                this.m_SelectedKey = prefKey1;
                this.m_ValidKeyChange = true;
              }
              Event.current.Use();
              break;
            case KeyCode.DownArrow:
              if (prefKey2 != null && prefKey2 != this.m_SelectedKey)
              {
                this.m_SelectedKey = prefKey2;
                this.m_ValidKeyChange = true;
              }
              Event.current.Use();
              break;
          }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        if (!this.m_ValidKeyChange)
        {
          GUILayout.BeginHorizontal();
          GUILayout.Label("", PreferencesWindow.constants.warningIcon, new GUILayoutOption[0]);
          GUILayout.Label(this.m_InvalidKeyMessage, PreferencesWindow.constants.errorLabel, new GUILayoutOption[0]);
          GUILayout.EndHorizontal();
        }
      }
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      if (!GUILayout.Button("Use Defaults", new GUILayoutOption[1]{ GUILayout.Width(120f) }))
        return;
      this.m_ValidKeyChange = true;
      this.RevertKeys();
    }

    private void RevertColors()
    {
      foreach (KeyValuePair<string, PrefColor> pref in Settings.Prefs<PrefColor>())
      {
        pref.Value.ResetToDefault();
        EditorPrefs.SetString(pref.Value.Name, pref.Value.ToUniqueString());
      }
    }

    private void ShowColors()
    {
      if (this.s_CachedColors == null)
        this.s_CachedColors = this.OrderPrefs<PrefColor>(Settings.Prefs<PrefColor>());
      bool flag = false;
      PrefColor prefColor = (PrefColor) null;
      foreach (KeyValuePair<string, List<KeyValuePair<string, PrefColor>>> cachedColor in this.s_CachedColors)
      {
        GUILayout.Label(cachedColor.Key, EditorStyles.boldLabel, new GUILayoutOption[0]);
        foreach (KeyValuePair<string, PrefColor> keyValuePair in cachedColor.Value)
        {
          EditorGUI.BeginChangeCheck();
          Color color = EditorGUILayout.ColorField(keyValuePair.Key, keyValuePair.Value.Color, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            prefColor = keyValuePair.Value;
            prefColor.Color = color;
            flag = true;
          }
        }
        if (prefColor != null)
          Settings.Set<PrefColor>(prefColor.Name, prefColor);
      }
      GUILayout.Space(5f);
      if (GUILayout.Button("Use Defaults", new GUILayoutOption[1]{ GUILayout.Width(120f) }))
      {
        this.RevertColors();
        flag = true;
      }
      if (!flag)
        return;
      EditorApplication.RequestRepaintAllViews();
    }

    private void Show2D()
    {
      EditorGUI.BeginChangeCheck();
      this.m_SpriteAtlasCacheSize = EditorGUILayout.IntSlider(PreferencesWindow.Styles.spriteMaxCacheSize, this.m_SpriteAtlasCacheSize, PreferencesWindow.kMinSpriteCacheSizeInGigabytes, PreferencesWindow.kMaxSpriteCacheSizeInGigabytes, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.WritePreferences();
    }

    private void ShowGICache()
    {
      this.m_GICacheSettings.m_MaximumSize = EditorGUILayout.IntSlider(PreferencesWindow.Styles.maxCacheSize, this.m_GICacheSettings.m_MaximumSize, 5, 200, new GUILayoutOption[0]);
      this.WritePreferences();
      GUILayout.BeginHorizontal();
      if (Lightmapping.isRunning)
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent(PreferencesWindow.Styles.cantChangeCacheSettings.text).text, MessageType.Warning, true);
      GUILayout.EndHorizontal();
      using (new EditorGUI.DisabledScope(Lightmapping.isRunning))
      {
        this.m_GICacheSettings.m_EnableCustomPath = EditorGUILayout.Toggle(PreferencesWindow.Styles.customCacheLocation, this.m_GICacheSettings.m_EnableCustomPath, new GUILayoutOption[0]);
        if (this.m_GICacheSettings.m_EnableCustomPath)
        {
          GUIStyle miniButton = EditorStyles.miniButton;
          GUILayout.BeginHorizontal();
          EditorGUILayout.PrefixLabel(PreferencesWindow.Styles.cacheFolderLocation, miniButton);
          if (EditorGUI.DropdownButton(GUILayoutUtility.GetRect(GUIContent.none, miniButton), !string.IsNullOrEmpty(this.m_GICacheSettings.m_CachePath) ? new GUIContent(this.m_GICacheSettings.m_CachePath) : PreferencesWindow.Styles.browse, FocusType.Passive, miniButton))
          {
            string cachePath = this.m_GICacheSettings.m_CachePath;
            string str = EditorUtility.OpenFolderPanel(PreferencesWindow.Styles.browseGICacheLocation.text, cachePath, "");
            if (!string.IsNullOrEmpty(str))
            {
              this.m_GICacheSettings.m_CachePath = str;
              this.WritePreferences();
            }
          }
          GUILayout.EndHorizontal();
        }
        else
          this.m_GICacheSettings.m_CachePath = "";
        this.m_GICacheSettings.m_CompressionLevel = !EditorGUILayout.Toggle(PreferencesWindow.Styles.cacheCompression, this.m_GICacheSettings.m_CompressionLevel == 1, new GUILayoutOption[0]) ? 0 : 1;
        if (GUILayout.Button(PreferencesWindow.Styles.cleanCache, new GUILayoutOption[1]{ GUILayout.Width(120f) }))
        {
          EditorUtility.DisplayProgressBar(PreferencesWindow.Styles.cleanCache.text, PreferencesWindow.Styles.pleaseWait.text, 0.0f);
          Lightmapping.Clear();
          EditorUtility.DisplayProgressBar(PreferencesWindow.Styles.cleanCache.text, PreferencesWindow.Styles.pleaseWait.text, 0.5f);
          Lightmapping.ClearDiskCache();
          EditorUtility.ClearProgressBar();
        }
        if (Lightmapping.diskCacheSize >= 0L)
          GUILayout.Label(PreferencesWindow.Styles.cacheSizeIs.text + " " + EditorUtility.FormatBytes(Lightmapping.diskCacheSize));
        else
          GUILayout.Label(PreferencesWindow.Styles.cacheSizeIs.text + " is being calculated...");
        GUILayout.Label(PreferencesWindow.Styles.cacheFolderLocation.text + ":");
        GUILayout.Label(Lightmapping.diskCachePath, PreferencesWindow.constants.cacheFolderLocation, new GUILayoutOption[0]);
      }
    }

    private void ShowLanguage()
    {
      bool flag = false;
      bool editorLocalization = this.m_EnableEditorLocalization;
      this.m_EnableEditorLocalization = EditorGUILayout.Toggle("Editor Language(Experimental)", this.m_EnableEditorLocalization, new GUILayoutOption[0]);
      if (!this.m_EnableEditorLocalization && this.m_SelectedLanguage != SystemLanguage.English)
      {
        this.m_SelectedLanguage = SystemLanguage.English;
        flag = true;
      }
      EditorGUI.BeginDisabledGroup(!this.m_EnableEditorLocalization);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreferencesWindow.\u003CShowLanguage\u003Ec__AnonStorey1 languageCAnonStorey1 = new PreferencesWindow.\u003CShowLanguage\u003Ec__AnonStorey1();
      SystemLanguage selectedLanguage1 = this.m_SelectedLanguage;
      // ISSUE: reference to a compiler-generated field
      languageCAnonStorey1.editorLanguages = LocalizationDatabase.GetAvailableEditorLanguages();
      // ISSUE: reference to a compiler-generated field
      if (this.m_EditorLanguageNames == null || this.m_EditorLanguageNames.Length != languageCAnonStorey1.editorLanguages.Length)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_EditorLanguageNames = new string[languageCAnonStorey1.editorLanguages.Length];
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PreferencesWindow.\u003CShowLanguage\u003Ec__AnonStorey2 languageCAnonStorey2 = new PreferencesWindow.\u003CShowLanguage\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        languageCAnonStorey2.\u003C\u003Ef__ref\u00241 = languageCAnonStorey1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (languageCAnonStorey2.i = 0; languageCAnonStorey2.i < languageCAnonStorey1.editorLanguages.Length; ++languageCAnonStorey2.i)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EditorLanguageNames[languageCAnonStorey2.i] = ArrayUtility.FindIndex<SystemLanguage>(this.m_stableLanguages, new Predicate<SystemLanguage>(languageCAnonStorey2.\u003C\u003Em__0)) >= 0 ? languageCAnonStorey1.editorLanguages[languageCAnonStorey2.i].ToString() : string.Format("{0} (Experimental)", (object) languageCAnonStorey1.editorLanguages[languageCAnonStorey2.i].ToString());
        }
        string str = string.Format("Default ( {0} )", (object) LocalizationDatabase.GetDefaultEditorLanguage().ToString());
        ArrayUtility.Insert<string>(ref this.m_EditorLanguageNames, 0, "");
        ArrayUtility.Insert<string>(ref this.m_EditorLanguageNames, 0, str);
      }
      EditorGUI.BeginChangeCheck();
      SystemLanguage selectedLanguage2 = this.m_SelectedLanguage;
      // ISSUE: reference to a compiler-generated field
      int num = EditorGUILayout.Popup("Editor language", 2 + Array.IndexOf<SystemLanguage>(languageCAnonStorey1.editorLanguages, this.m_SelectedLanguage), this.m_EditorLanguageNames, new GUILayoutOption[0]);
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedLanguage = num != 0 ? languageCAnonStorey1.editorLanguages[num - 2] : LocalizationDatabase.GetDefaultEditorLanguage();
      if (EditorGUI.EndChangeCheck() && selectedLanguage2 != this.m_SelectedLanguage)
        flag = true;
      GUILayout.Space(20f);
      EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
      GUILayout.Space(8f);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(8f);
      EditorGUILayout.LabelField("Do you wish to disable experimental languages?");
      GUILayout.Space(8f);
      if (GUILayout.Button("Disable"))
      {
        this.m_EnableEditorLocalization = false;
        this.m_SelectedLanguage = SystemLanguage.English;
        flag = true;
      }
      GUILayout.Space(8f);
      EditorGUILayout.EndHorizontal();
      GUILayout.Space(8f);
      EditorGUILayout.EndVertical();
      // ISSUE: reference to a compiler-generated field
      if (languageCAnonStorey1.editorLanguages.Length <= 1)
      {
        GUILayout.Space(20f);
        EditorGUILayout.HelpBox("Internet connection is required to enable non-English languages.", MessageType.Info);
      }
      EditorGUI.EndDisabledGroup();
      this.ApplyChangesToPrefs(false);
      if (editorLocalization != this.m_EnableEditorLocalization && this.m_EnableEditorLocalization)
        LocalizationDatabase.ReadEditorLocalizationResources();
      if (!flag)
        return;
      EditorGUIUtility.NotifyLanguageChanged(this.m_SelectedLanguage);
      InternalEditorUtility.RequestScriptReload();
      LocalizedEditorFontManager.UpdateSkinFont(this.m_SelectedLanguage);
    }

    private void WriteRecentAppsList(string[] paths, string path, string prefsKey)
    {
      int num = 0;
      if (path.Length != 0)
      {
        EditorPrefs.SetString(prefsKey + (object) num, path);
        ++num;
      }
      for (int index = 0; index < paths.Length && num < 10; ++index)
      {
        if (paths[index].Length != 0 && !(paths[index] == path))
        {
          EditorPrefs.SetString(prefsKey + (object) num, paths[index]);
          ++num;
        }
      }
    }

    private void WritePreferences()
    {
      ScriptEditorUtility.SetExternalScriptEditor((string) this.m_ScriptEditorPath);
      ScriptEditorUtility.SetExternalScriptEditorArgs(this.m_ScriptEditorArgs);
      EditorPrefs.SetBool("kExternalEditorSupportsUnityProj", this.m_ExternalEditorSupportsUnityProj);
      EditorPrefs.SetString("kImagesDefaultApp", (string) this.m_ImageAppPath);
      EditorPrefs.SetString("kDiffsDefaultApp", this.m_DiffTools.Length != 0 ? this.m_DiffTools[this.m_DiffToolIndex] : "");
      this.WriteRecentAppsList(this.m_ScriptApps, (string) this.m_ScriptEditorPath, "RecentlyUsedScriptApp");
      this.WriteRecentAppsList(this.m_ImageApps, (string) this.m_ImageAppPath, "RecentlyUsedImageApp");
      EditorPrefs.SetBool("kAutoRefresh", this.m_AutoRefresh);
      if (Unsupported.IsDeveloperBuild() || UnityConnect.preferencesEnabled)
        UnityConnectPrefs.StorePanelPrefs();
      EditorPrefs.SetBool("ReopenLastUsedProjectOnStartup", this.m_ReopenLastUsedProjectOnStartup);
      EditorPrefs.SetBool("UseOSColorPicker", this.m_UseOSColorPicker);
      EditorPrefs.SetBool("EnableEditorAnalytics", this.m_EnableEditorAnalytics);
      EditorPrefs.SetBool("ShowAssetStoreSearchHits", this.m_ShowAssetStoreSearchHits);
      EditorPrefs.SetBool("VerifySavingAssets", this.m_VerifySavingAssets);
      EditorPrefs.SetBool("AllowAttachedDebuggingOfEditor", this.m_AllowAttachedDebuggingOfEditor);
      LocalizationDatabase.enableEditorLocalization = this.m_EnableEditorLocalization;
      LocalizationDatabase.SetCurrentEditorLanguage(this.m_SelectedLanguage);
      EditorPrefs.SetBool("AllowAlphaNumericHierarchy", this.m_AllowAlphaNumericHierarchy);
      EditorPrefs.SetString("GpuDevice", this.m_GpuDevice);
      EditorPrefs.SetBool("GICacheEnableCustomPath", this.m_GICacheSettings.m_EnableCustomPath);
      EditorPrefs.SetInt("GICacheMaximumSizeGB", this.m_GICacheSettings.m_MaximumSize);
      EditorPrefs.SetString("GICacheFolder", this.m_GICacheSettings.m_CachePath);
      EditorPrefs.SetInt("GICacheCompressionLevel", this.m_GICacheSettings.m_CompressionLevel);
      EditorPrefs.SetInt("SpritePackerCacheMaximumSizeGB", this.m_SpriteAtlasCacheSize);
      foreach (IPreferenceWindowExtension prefWinExtension in this.prefWinExtensions)
        prefWinExtension.WritePreferences();
      Lightmapping.UpdateCachePath();
    }

    private static void SetupDefaultPreferences()
    {
    }

    private static string GetProgramFilesFolder()
    {
      return Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? Environment.GetEnvironmentVariable("ProgramFiles");
    }

    private void ReadPreferences()
    {
      this.m_ScriptEditorPath.str = ScriptEditorUtility.GetExternalScriptEditor();
      this.m_ScriptEditorArgs = ScriptEditorUtility.GetExternalScriptEditorArgs();
      this.m_ExternalEditorSupportsUnityProj = EditorPrefs.GetBool("kExternalEditorSupportsUnityProj", false);
      this.m_ImageAppPath.str = EditorPrefs.GetString("kImagesDefaultApp");
      this.m_ScriptApps = this.BuildAppPathList((string) this.m_ScriptEditorPath, "RecentlyUsedScriptApp", "internal");
      this.m_ScriptAppsEditions = new string[this.m_ScriptApps.Length];
      if (Application.platform == RuntimePlatform.WindowsEditor)
      {
        foreach (VisualStudioPath[] visualStudioPathArray in SyncVS.InstalledVisualStudios.Values)
        {
          foreach (VisualStudioPath visualStudioPath in visualStudioPathArray)
          {
            int index = Array.IndexOf<string>(this.m_ScriptApps, visualStudioPath.Path);
            if (index == -1)
            {
              ArrayUtility.Add<string>(ref this.m_ScriptApps, visualStudioPath.Path);
              ArrayUtility.Add<string>(ref this.m_ScriptAppsEditions, visualStudioPath.Edition);
            }
            else
              this.m_ScriptAppsEditions[index] = visualStudioPath.Edition;
          }
        }
      }
      foreach (string scriptEditorPath in ScriptEditorUtility.GetFoundScriptEditorPaths(Application.platform))
      {
        ArrayUtility.Add<string>(ref this.m_ScriptApps, scriptEditorPath);
        ArrayUtility.Add<string>(ref this.m_ScriptAppsEditions, (string) null);
      }
      this.m_ImageApps = this.BuildAppPathList((string) this.m_ImageAppPath, "RecentlyUsedImageApp", "");
      this.m_ScriptAppDisplayNames = this.BuildFriendlyAppNameList(this.m_ScriptApps, this.m_ScriptAppsEditions, "MonoDevelop (built-in)");
      this.m_ImageAppDisplayNames = this.BuildFriendlyAppNameList(this.m_ImageApps, (string[]) null, "Open by file extension");
      this.m_DiffTools = InternalEditorUtility.GetAvailableDiffTools();
      if ((this.m_DiffTools == null || this.m_DiffTools.Length == 0) && InternalEditorUtility.HasTeamLicense())
        this.m_noDiffToolsMessage = InternalEditorUtility.GetNoDiffToolsDetectedMessage();
      this.m_DiffToolIndex = ArrayUtility.IndexOf<string>(this.m_DiffTools, EditorPrefs.GetString("kDiffsDefaultApp"));
      if (this.m_DiffToolIndex == -1)
        this.m_DiffToolIndex = 0;
      this.m_AutoRefresh = EditorPrefs.GetBool("kAutoRefresh");
      this.m_ReopenLastUsedProjectOnStartup = EditorPrefs.GetBool("ReopenLastUsedProjectOnStartup");
      this.m_UseOSColorPicker = EditorPrefs.GetBool("UseOSColorPicker");
      this.m_EnableEditorAnalytics = EditorPrefs.GetBool("EnableEditorAnalytics", true);
      this.m_ShowAssetStoreSearchHits = EditorPrefs.GetBool("ShowAssetStoreSearchHits", true);
      this.m_VerifySavingAssets = EditorPrefs.GetBool("VerifySavingAssets", false);
      this.m_GICacheSettings.m_EnableCustomPath = EditorPrefs.GetBool("GICacheEnableCustomPath");
      this.m_GICacheSettings.m_CachePath = EditorPrefs.GetString("GICacheFolder");
      this.m_GICacheSettings.m_MaximumSize = EditorPrefs.GetInt("GICacheMaximumSizeGB", 10);
      this.m_GICacheSettings.m_CompressionLevel = EditorPrefs.GetInt("GICacheCompressionLevel");
      this.m_SpriteAtlasCacheSize = EditorPrefs.GetInt("SpritePackerCacheMaximumSizeGB");
      this.m_AllowAttachedDebuggingOfEditor = EditorPrefs.GetBool("AllowAttachedDebuggingOfEditor", true);
      this.m_EnableEditorLocalization = LocalizationDatabase.enableEditorLocalization;
      this.m_SelectedLanguage = LocalizationDatabase.GetCurrentEditorLanguage();
      this.m_AllowAlphaNumericHierarchy = EditorPrefs.GetBool("AllowAlphaNumericHierarchy", false);
      this.m_CompressAssetsOnImport = Unsupported.GetApplicationSettingCompressAssetsOnImport();
      this.m_GpuDevice = EditorPrefs.GetString("GpuDevice");
      foreach (IPreferenceWindowExtension prefWinExtension in this.prefWinExtensions)
        prefWinExtension.ReadPreferences();
    }

    private string StripMicrosoftFromVisualStudioName(string arg)
    {
      if (!arg.Contains("Visual Studio") || !arg.StartsWith("Microsoft"))
        return arg;
      return arg.Substring("Microsoft ".Length);
    }

    private void AppsListClick(object userData, string[] options, int selected)
    {
      PreferencesWindow.AppsListUserData appsListUserData = (PreferencesWindow.AppsListUserData) userData;
      if (options[selected] == "Browse...")
      {
        string str = EditorUtility.OpenFilePanel("Browse for application", "", InternalEditorUtility.GetApplicationExtensionForRuntimePlatform(Application.platform));
        if (str.Length != 0)
        {
          appsListUserData.str.str = str;
          if (appsListUserData.onChanged != null)
            appsListUserData.onChanged();
        }
      }
      else
      {
        appsListUserData.str.str = appsListUserData.paths[selected];
        if (appsListUserData.onChanged != null)
          appsListUserData.onChanged();
      }
      this.WritePreferences();
      this.ReadPreferences();
    }

    private void FilePopup(string label, string selectedString, ref string[] names, ref string[] paths, PreferencesWindow.RefString outString, string defaultString, Action onChanged)
    {
      GUIStyle popup = EditorStyles.popup;
      GUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(label, popup);
      int[] selected = new int[0];
      if (((IEnumerable<string>) paths).Contains<string>(selectedString))
        selected = new int[1]
        {
          Array.IndexOf<string>(paths, selectedString)
        };
      GUIContent content = new GUIContent(selected.Length != 0 ? names[selected[0]] : defaultString);
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, popup);
      PreferencesWindow.AppsListUserData appsListUserData = new PreferencesWindow.AppsListUserData(paths, outString, onChanged);
      if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, popup))
      {
        ArrayUtility.Add<string>(ref names, "Browse...");
        EditorUtility.DisplayCustomMenu(rect, names, selected, new EditorUtility.SelectMenuItemFunction(this.AppsListClick), (object) appsListUserData);
      }
      GUILayout.EndHorizontal();
    }

    private string[] BuildAppPathList(string userAppPath, string recentAppsKey, string stringForInternalEditor)
    {
      string[] array = new string[1]{ stringForInternalEditor };
      if (userAppPath != null && userAppPath.Length != 0 && Array.IndexOf<string>(array, userAppPath) == -1)
        ArrayUtility.Add<string>(ref array, userAppPath);
      for (int index = 0; index < 10; ++index)
      {
        string path = EditorPrefs.GetString(recentAppsKey + (object) index);
        if (!File.Exists(path))
        {
          path = "";
          EditorPrefs.SetString(recentAppsKey + (object) index, path);
        }
        if (path.Length != 0 && Array.IndexOf<string>(array, path) == -1)
          ArrayUtility.Add<string>(ref array, path);
      }
      return array;
    }

    private string[] BuildFriendlyAppNameList(string[] appPathList, string[] appEditionList, string defaultBuiltIn)
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < appPathList.Length; ++index)
      {
        string appPath = appPathList[index];
        if (appPath == "internal" || appPath == "")
        {
          stringList.Add(defaultBuiltIn);
        }
        else
        {
          string str = this.StripMicrosoftFromVisualStudioName(OSUtil.GetAppFriendlyName(appPath));
          if (appEditionList != null && !string.IsNullOrEmpty(appEditionList[index]))
            str = string.Format("{0} ({1})", (object) str, (object) appEditionList[index]);
          stringList.Add(str);
        }
      }
      return stringList.ToArray();
    }

    internal class Constants
    {
      public GUIStyle sectionScrollView = (GUIStyle) "PreferencesSectionBox";
      public GUIStyle settingsBoxTitle = (GUIStyle) "OL Title";
      public GUIStyle settingsBox = (GUIStyle) "OL Box";
      public GUIStyle errorLabel = (GUIStyle) "WordWrappedLabel";
      public GUIStyle sectionElement = (GUIStyle) "PreferencesSection";
      public GUIStyle evenRow = (GUIStyle) "CN EntryBackEven";
      public GUIStyle oddRow = (GUIStyle) "CN EntryBackOdd";
      public GUIStyle selected = (GUIStyle) "OL SelectedRow";
      public GUIStyle keysElement = (GUIStyle) "PreferencesKeysElement";
      public GUIStyle warningIcon = (GUIStyle) "CN EntryWarn";
      public GUIStyle sectionHeader = new GUIStyle(EditorStyles.largeLabel);
      public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);

      public Constants()
      {
        this.sectionScrollView = new GUIStyle(this.sectionScrollView);
        ++this.sectionScrollView.overflow.bottom;
        this.sectionHeader.fontStyle = FontStyle.Bold;
        this.sectionHeader.fontSize = 18;
        this.sectionHeader.margin.top = 10;
        ++this.sectionHeader.margin.left;
        this.sectionHeader.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f, 1f) : new Color(0.4f, 0.4f, 0.4f, 1f);
        this.cacheFolderLocation.wordWrap = true;
      }
    }

    internal class Styles
    {
      public static readonly GUIContent browse = EditorGUIUtility.TextContent("Browse...");
      public static readonly GUIContent maxCacheSize = EditorGUIUtility.TextContent("Maximum Cache Size (GB)|The size of the GI Cache folder will be kept below this maximum value when possible. A background job will periodically clean up the oldest unused files.");
      public static readonly GUIContent customCacheLocation = EditorGUIUtility.TextContent("Custom cache location|Specify the GI Cache folder location.");
      public static readonly GUIContent cacheFolderLocation = EditorGUIUtility.TextContent("Cache Folder Location|The GI Cache folder is shared between all projects.");
      public static readonly GUIContent cacheCompression = EditorGUIUtility.TextContent("Cache compression|Use fast realtime compression for the GI cache files to reduce the size of generated data. Disable it and clean the cache if you need access to the raw data generated by Enlighten.");
      public static readonly GUIContent cantChangeCacheSettings = EditorGUIUtility.TextContent("Cache settings can't be changed while lightmapping is being computed.");
      public static readonly GUIContent cleanCache = EditorGUIUtility.TextContent("Clean Cache");
      public static readonly GUIContent browseGICacheLocation = EditorGUIUtility.TextContent("Browse for GI Cache location");
      public static readonly GUIContent cacheSizeIs = EditorGUIUtility.TextContent("Cache size is");
      public static readonly GUIContent pleaseWait = EditorGUIUtility.TextContent("Please wait...");
      public static readonly GUIContent spriteMaxCacheSize = EditorGUIUtility.TextContent("Max Sprite Atlas Cache Size (GB)|The size of the Sprite Atlas Cache folder will be kept below this maximum value when possible. Change requires Editor restart");
    }

    private delegate void OnGUIDelegate();

    private class Section
    {
      public GUIContent content;
      public PreferencesWindow.OnGUIDelegate guiFunc;

      public Section(string name, PreferencesWindow.OnGUIDelegate guiFunc)
      {
        this.content = new GUIContent(name);
        this.guiFunc = guiFunc;
      }

      public Section(string name, Texture2D icon, PreferencesWindow.OnGUIDelegate guiFunc)
      {
        this.content = new GUIContent(name, (Texture) icon);
        this.guiFunc = guiFunc;
      }

      public Section(GUIContent content, PreferencesWindow.OnGUIDelegate guiFunc)
      {
        this.content = content;
        this.guiFunc = guiFunc;
      }
    }

    private struct GICacheSettings
    {
      public bool m_EnableCustomPath;
      public int m_MaximumSize;
      public string m_CachePath;
      public int m_CompressionLevel;
    }

    private class RefString
    {
      public string str;

      public RefString(string s)
      {
        this.str = s;
      }

      public static implicit operator string(PreferencesWindow.RefString s)
      {
        return s.str;
      }

      public override string ToString()
      {
        return this.str;
      }
    }

    private class AppsListUserData
    {
      public string[] paths;
      public PreferencesWindow.RefString str;
      public Action onChanged;

      public AppsListUserData(string[] paths, PreferencesWindow.RefString str, Action onChanged)
      {
        this.paths = paths;
        this.str = str;
        this.onChanged = onChanged;
      }
    }
  }
}
