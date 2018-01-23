// Decompiled with JetBrains decompiler
// Type: UnityEditor.PluginImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Modules;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (PluginImporter))]
  internal class PluginImporterInspector : AssetImporterEditor
  {
    private static readonly BuildTarget[] m_StandaloneTargets = new BuildTarget[6]{ BuildTarget.StandaloneOSX, BuildTarget.StandaloneWindows, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneLinux, BuildTarget.StandaloneLinux64, BuildTarget.StandaloneLinuxUniversal };
    private PluginImporterInspector.Compatibility[] m_CompatibleWithPlatform = new PluginImporterInspector.Compatibility[PluginImporterInspector.GetPlatformGroupArraySize()];
    private Vector2 m_InformationScrollPosition = Vector2.zero;
    private EditorPluginImporterExtension m_EditorExtension = (EditorPluginImporterExtension) null;
    private DesktopPluginImporterExtension m_DesktopExtension = (DesktopPluginImporterExtension) null;
    private bool m_HasModified;
    private PluginImporterInspector.Compatibility m_CompatibleWithAnyPlatform;
    private PluginImporterInspector.Compatibility m_CompatibleWithEditor;
    private Dictionary<string, string> m_PluginInformation;

    public override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    internal EditorPluginImporterExtension editorExtension
    {
      get
      {
        if (this.m_EditorExtension == null)
          this.m_EditorExtension = new EditorPluginImporterExtension();
        return this.m_EditorExtension;
      }
    }

    internal DesktopPluginImporterExtension desktopExtension
    {
      get
      {
        if (this.m_DesktopExtension == null)
          this.m_DesktopExtension = new DesktopPluginImporterExtension();
        return this.m_DesktopExtension;
      }
    }

    internal IPluginImporterExtension[] additionalExtensions
    {
      get
      {
        return new IPluginImporterExtension[2]{ (IPluginImporterExtension) this.editorExtension, (IPluginImporterExtension) this.desktopExtension };
      }
    }

    internal PluginImporter importer
    {
      get
      {
        return this.target as PluginImporter;
      }
    }

    internal PluginImporter[] importers
    {
      get
      {
        return this.targets.Cast<PluginImporter>().ToArray<PluginImporter>();
      }
    }

    private static bool IgnorePlatform(BuildTarget platform)
    {
      return false;
    }

    private bool IsEditingPlatformSettingsSupported()
    {
      return this.targets.Length == 1;
    }

    private static int GetPlatformGroupArraySize()
    {
      int num = 0;
      foreach (BuildTarget nonObsoleteValue in typeof (BuildTarget).EnumGetNonObsoleteValues())
      {
        if ((BuildTarget) num < nonObsoleteValue + 1)
          num = (int) (nonObsoleteValue + 1);
      }
      return num;
    }

    private static bool IsStandaloneTarget(BuildTarget buildTarget)
    {
      return ((IEnumerable<BuildTarget>) PluginImporterInspector.m_StandaloneTargets).Contains<BuildTarget>(buildTarget);
    }

    private PluginImporterInspector.Compatibility compatibleWithStandalone
    {
      get
      {
        bool flag = false;
        foreach (BuildTarget standaloneTarget in PluginImporterInspector.m_StandaloneTargets)
        {
          if (this.m_CompatibleWithPlatform[(int) standaloneTarget] == PluginImporterInspector.Compatibility.Mixed)
            return PluginImporterInspector.Compatibility.Mixed;
          flag |= this.m_CompatibleWithPlatform[(int) standaloneTarget] > PluginImporterInspector.Compatibility.NotCompatible;
        }
        return !flag ? PluginImporterInspector.Compatibility.NotCompatible : PluginImporterInspector.Compatibility.Compatible;
      }
      set
      {
        foreach (int standaloneTarget in PluginImporterInspector.m_StandaloneTargets)
          this.m_CompatibleWithPlatform[standaloneTarget] = value;
      }
    }

    internal static bool IsValidBuildTarget(BuildTarget buildTarget)
    {
      return buildTarget > ~BuildTarget.iPhone;
    }

    internal PluginImporterInspector.Compatibility GetPlatformCompatibility(string platformName)
    {
      BuildTarget buildTargetByName = BuildPipeline.GetBuildTargetByName(platformName);
      if (!PluginImporterInspector.IsValidBuildTarget(buildTargetByName))
        return PluginImporterInspector.Compatibility.NotCompatible;
      return this.m_CompatibleWithPlatform[(int) buildTargetByName];
    }

    internal void SetPlatformCompatibility(string platformName, bool compatible)
    {
      this.SetPlatformCompatibility(platformName, !compatible ? PluginImporterInspector.Compatibility.NotCompatible : PluginImporterInspector.Compatibility.Compatible);
    }

    internal void SetPlatformCompatibility(string platformName, PluginImporterInspector.Compatibility compatibility)
    {
      if (compatibility == PluginImporterInspector.Compatibility.Mixed)
        throw new ArgumentException("compatibility value cannot be Mixed");
      BuildTarget buildTargetByName = BuildPipeline.GetBuildTargetByName(platformName);
      if (!PluginImporterInspector.IsValidBuildTarget(buildTargetByName) || this.m_CompatibleWithPlatform[(int) buildTargetByName] == compatibility)
        return;
      this.m_CompatibleWithPlatform[(int) buildTargetByName] = compatibility;
      this.m_HasModified = true;
    }

    private static List<BuildTarget> GetValidBuildTargets()
    {
      List<BuildTarget> buildTargetList = new List<BuildTarget>();
      foreach (BuildTarget nonObsoleteValue in typeof (BuildTarget).EnumGetNonObsoleteValues())
      {
        if (PluginImporterInspector.IsValidBuildTarget(nonObsoleteValue) && !PluginImporterInspector.IgnorePlatform(nonObsoleteValue) && (!ModuleManager.IsPlatformSupported(nonObsoleteValue) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(nonObsoleteValue)) || PluginImporterInspector.IsStandaloneTarget(nonObsoleteValue)))
          buildTargetList.Add(nonObsoleteValue);
      }
      return buildTargetList;
    }

    private BuildPlatform[] GetBuildPlayerValidPlatforms()
    {
      List<BuildPlatform> validPlatforms = BuildPlatforms.instance.GetValidPlatforms();
      List<BuildPlatform> buildPlatformList = new List<BuildPlatform>();
      if (this.m_CompatibleWithEditor > PluginImporterInspector.Compatibility.NotCompatible)
        buildPlatformList.Add(new BuildPlatform("Editor settings", "Editor Settings", "BuildSettings.Editor", BuildTargetGroup.Unknown, true)
        {
          name = BuildPipeline.GetEditorTargetName()
        });
      foreach (BuildPlatform buildPlatform in validPlatforms)
      {
        if (!PluginImporterInspector.IgnorePlatform(buildPlatform.defaultTarget))
        {
          if (buildPlatform.targetGroup == BuildTargetGroup.Standalone)
          {
            if (this.compatibleWithStandalone < PluginImporterInspector.Compatibility.Compatible)
              continue;
          }
          else if (this.m_CompatibleWithPlatform[(int) buildPlatform.defaultTarget] < PluginImporterInspector.Compatibility.Compatible || ModuleManager.GetPluginImporterExtension(buildPlatform.targetGroup) == null)
            continue;
          buildPlatformList.Add(buildPlatform);
        }
      }
      return buildPlatformList.ToArray();
    }

    private void ResetCompatability(ref PluginImporterInspector.Compatibility value, PluginImporterInspector.GetComptability getComptability)
    {
      value = !getComptability(this.importer) ? PluginImporterInspector.Compatibility.NotCompatible : PluginImporterInspector.Compatibility.Compatible;
      foreach (PluginImporter importer in this.importers)
      {
        if (value != (!getComptability(importer) ? PluginImporterInspector.Compatibility.NotCompatible : PluginImporterInspector.Compatibility.Compatible))
        {
          value = PluginImporterInspector.Compatibility.Mixed;
          break;
        }
      }
    }

    protected override void ResetValues()
    {
      base.ResetValues();
      this.m_HasModified = false;
      this.ResetCompatability(ref this.m_CompatibleWithAnyPlatform, (PluginImporterInspector.GetComptability) (imp => imp.GetCompatibleWithAnyPlatform()));
      this.ResetCompatability(ref this.m_CompatibleWithEditor, (PluginImporterInspector.GetComptability) (imp => imp.GetCompatibleWithEditor()));
      if (this.m_CompatibleWithAnyPlatform < PluginImporterInspector.Compatibility.Compatible)
      {
        this.ResetCompatability(ref this.m_CompatibleWithEditor, (PluginImporterInspector.GetComptability) (imp => imp.GetCompatibleWithEditor("", "")));
        foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PluginImporterInspector.\u003CResetValues\u003Ec__AnonStorey0 valuesCAnonStorey0 = new PluginImporterInspector.\u003CResetValues\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          valuesCAnonStorey0.platform = validBuildTarget;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ResetCompatability(ref this.m_CompatibleWithPlatform[(int) valuesCAnonStorey0.platform], new PluginImporterInspector.GetComptability(valuesCAnonStorey0.\u003C\u003Em__0));
        }
      }
      else
      {
        this.ResetCompatability(ref this.m_CompatibleWithEditor, (PluginImporterInspector.GetComptability) (imp => !imp.GetExcludeEditorFromAnyPlatform()));
        foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PluginImporterInspector.\u003CResetValues\u003Ec__AnonStorey1 valuesCAnonStorey1 = new PluginImporterInspector.\u003CResetValues\u003Ec__AnonStorey1();
          // ISSUE: reference to a compiler-generated field
          valuesCAnonStorey1.platform = validBuildTarget;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ResetCompatability(ref this.m_CompatibleWithPlatform[(int) valuesCAnonStorey1.platform], new PluginImporterInspector.GetComptability(valuesCAnonStorey1.\u003C\u003Em__0));
        }
      }
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.ResetValues(this);
      foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
      {
        IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(validBuildTarget);
        if (importerExtension != null)
          importerExtension.ResetValues(this);
      }
    }

    public override bool HasModified()
    {
      bool flag = this.m_HasModified || base.HasModified();
      if (!this.IsEditingPlatformSettingsSupported())
        return flag;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        flag |= additionalExtension.HasModified(this);
      foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
      {
        IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(validBuildTarget);
        if (importerExtension != null)
          flag |= importerExtension.HasModified(this);
      }
      return flag;
    }

    protected override void Apply()
    {
      base.Apply();
      foreach (PluginImporter importer in this.importers)
      {
        if (this.m_CompatibleWithAnyPlatform > PluginImporterInspector.Compatibility.Mixed)
          importer.SetCompatibleWithAnyPlatform(this.m_CompatibleWithAnyPlatform == PluginImporterInspector.Compatibility.Compatible);
        if (this.m_CompatibleWithEditor > PluginImporterInspector.Compatibility.Mixed)
          importer.SetCompatibleWithEditor(this.m_CompatibleWithEditor == PluginImporterInspector.Compatibility.Compatible);
        foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
        {
          if (this.m_CompatibleWithPlatform[(int) validBuildTarget] > PluginImporterInspector.Compatibility.Mixed)
            importer.SetCompatibleWithPlatform(validBuildTarget, this.m_CompatibleWithPlatform[(int) validBuildTarget] == PluginImporterInspector.Compatibility.Compatible);
        }
        if (this.m_CompatibleWithEditor > PluginImporterInspector.Compatibility.Mixed)
          importer.SetExcludeEditorFromAnyPlatform(this.m_CompatibleWithEditor == PluginImporterInspector.Compatibility.NotCompatible);
        foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
        {
          if (this.m_CompatibleWithPlatform[(int) validBuildTarget] > PluginImporterInspector.Compatibility.Mixed)
            importer.SetExcludeFromAnyPlatform(validBuildTarget, this.m_CompatibleWithPlatform[(int) validBuildTarget] == PluginImporterInspector.Compatibility.NotCompatible);
        }
      }
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.Apply(this);
      foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
      {
        IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(validBuildTarget);
        if (importerExtension != null)
          importerExtension.Apply(this);
      }
    }

    protected override void Awake()
    {
      this.m_EditorExtension = new EditorPluginImporterExtension();
      this.m_DesktopExtension = new DesktopPluginImporterExtension();
      base.Awake();
    }

    public override void OnEnable()
    {
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.OnEnable(this);
      foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
      {
        IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(validBuildTarget);
        if (importerExtension != null)
        {
          importerExtension.OnEnable(this);
          importerExtension.ResetValues(this);
        }
      }
      this.m_PluginInformation = new Dictionary<string, string>();
      this.m_PluginInformation["Path"] = this.importer.assetPath;
      this.m_PluginInformation["Type"] = !this.importer.isNativePlugin ? "Managed" : "Native";
      if (this.importer.isNativePlugin)
        return;
      string str;
      switch (this.importer.dllType)
      {
        case DllType.UnknownManaged:
          str = "Targets Unknown .NET";
          break;
        case DllType.ManagedNET35:
          str = "Targets .NET 3.5";
          break;
        case DllType.ManagedNET40:
          str = "Targets .NET 4.x";
          break;
        case DllType.WinMDNative:
          str = "Native WinMD";
          break;
        case DllType.WinMDNET40:
          str = "Managed WinMD";
          break;
        default:
          throw new Exception("Unknown managed dll type: " + this.importer.dllType.ToString());
      }
      this.m_PluginInformation["Assembly Info"] = str;
    }

    private new void OnDisable()
    {
      base.OnDisable();
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.OnDisable(this);
      foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
      {
        IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(validBuildTarget);
        if (importerExtension != null)
          importerExtension.OnDisable(this);
      }
    }

    private PluginImporterInspector.Compatibility ToggleWithMixedValue(PluginImporterInspector.Compatibility value, string title)
    {
      EditorGUI.showMixedValue = value == PluginImporterInspector.Compatibility.Mixed;
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(title, value == PluginImporterInspector.Compatibility.Compatible, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        return !flag ? PluginImporterInspector.Compatibility.NotCompatible : PluginImporterInspector.Compatibility.Compatible;
      EditorGUI.showMixedValue = false;
      return value;
    }

    private void ShowPlatforms(PluginImporterInspector.ValueSwitcher switcher)
    {
      this.m_CompatibleWithEditor = switcher(this.ToggleWithMixedValue(switcher(this.m_CompatibleWithEditor), "Editor"));
      EditorGUI.BeginChangeCheck();
      PluginImporterInspector.Compatibility compatibility = this.ToggleWithMixedValue(switcher(this.compatibleWithStandalone), "Standalone");
      if (EditorGUI.EndChangeCheck())
      {
        this.compatibleWithStandalone = switcher(compatibility);
        if (this.compatibleWithStandalone != PluginImporterInspector.Compatibility.Mixed)
          this.desktopExtension.ValidateSingleCPUTargets(this);
      }
      foreach (BuildTarget validBuildTarget in PluginImporterInspector.GetValidBuildTargets())
      {
        if (!PluginImporterInspector.IsStandaloneTarget(validBuildTarget))
          this.m_CompatibleWithPlatform[(int) validBuildTarget] = switcher(this.ToggleWithMixedValue(switcher(this.m_CompatibleWithPlatform[(int) validBuildTarget]), validBuildTarget.ToString()));
      }
    }

    private PluginImporterInspector.Compatibility SwitchToInclude(PluginImporterInspector.Compatibility value)
    {
      return value;
    }

    private PluginImporterInspector.Compatibility SwitchToExclude(PluginImporterInspector.Compatibility value)
    {
      switch (value + 1)
      {
        case PluginImporterInspector.Compatibility.NotCompatible:
          return PluginImporterInspector.Compatibility.Mixed;
        case PluginImporterInspector.Compatibility.Compatible:
          return PluginImporterInspector.Compatibility.Compatible;
        case (PluginImporterInspector.Compatibility) 2:
          return PluginImporterInspector.Compatibility.NotCompatible;
        default:
          throw new InvalidEnumArgumentException("Invalid value: " + value.ToString());
      }
    }

    private void ShowGeneralOptions()
    {
      EditorGUI.BeginChangeCheck();
      this.m_CompatibleWithAnyPlatform = this.ToggleWithMixedValue(this.m_CompatibleWithAnyPlatform, "Any Platform");
      if (this.m_CompatibleWithAnyPlatform == PluginImporterInspector.Compatibility.Compatible)
      {
        GUILayout.Label("Exclude Platforms", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.ShowPlatforms(new PluginImporterInspector.ValueSwitcher(this.SwitchToExclude));
      }
      else if (this.m_CompatibleWithAnyPlatform == PluginImporterInspector.Compatibility.NotCompatible)
      {
        GUILayout.Label("Include Platforms", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.ShowPlatforms(new PluginImporterInspector.ValueSwitcher(this.SwitchToInclude));
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_HasModified = true;
    }

    private void ShowEditorSettings()
    {
      this.editorExtension.OnPlatformSettingsGUI(this);
    }

    private void ShowPlatformSettings()
    {
      BuildPlatform[] playerValidPlatforms = this.GetBuildPlayerValidPlatforms();
      if (playerValidPlatforms.Length <= 0)
        return;
      GUILayout.Label("Platform settings", EditorStyles.boldLabel, new GUILayoutOption[0]);
      int index = EditorGUILayout.BeginPlatformGrouping(playerValidPlatforms, (GUIContent) null);
      if (playerValidPlatforms[index].name == BuildPipeline.GetEditorTargetName())
      {
        this.ShowEditorSettings();
      }
      else
      {
        BuildTargetGroup targetGroup = playerValidPlatforms[index].targetGroup;
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          this.desktopExtension.OnPlatformSettingsGUI(this);
        }
        else
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(targetGroup);
          if (importerExtension != null)
            importerExtension.OnPlatformSettingsGUI(this);
        }
      }
      EditorGUILayout.EndPlatformGrouping();
    }

    public override void OnInspectorGUI()
    {
      using (new EditorGUI.DisabledScope(false))
      {
        GUILayout.Label("Select platforms for plugin", EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
        this.ShowGeneralOptions();
        EditorGUILayout.EndVertical();
        GUILayout.Space(10f);
        if (this.IsEditingPlatformSettingsSupported())
          this.ShowPlatformSettings();
      }
      this.ApplyRevertGUI();
      if (this.targets.Length > 1)
        return;
      GUILayout.Label("Information", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_InformationScrollPosition = EditorGUILayout.BeginVerticalScrollView(this.m_InformationScrollPosition);
      foreach (KeyValuePair<string, string> keyValuePair in this.m_PluginInformation)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(keyValuePair.Key, new GUILayoutOption[1]
        {
          GUILayout.Width(85f)
        });
        EditorGUILayout.SelectableLabel(keyValuePair.Value, GUILayout.Height(16f));
        GUILayout.EndHorizontal();
      }
      EditorGUILayout.EndScrollView();
      GUILayout.FlexibleSpace();
      if (this.importer.isNativePlugin)
        EditorGUILayout.HelpBox("Once a native plugin is loaded from script, it's never unloaded. If you deselect a native plugin and it's already loaded, please restart Unity.", MessageType.Warning);
      if (EditorApplication.scriptingRuntimeVersion != ScriptingRuntimeVersion.Legacy || this.importer.dllType != DllType.ManagedNET40 || this.m_CompatibleWithEditor != PluginImporterInspector.Compatibility.Compatible)
        return;
      EditorGUILayout.HelpBox("Plugin targets .NET 4.x and is marked as compatible with Editor, Editor can only use assemblies targeting .NET 3.5 or lower, please unselect Editor as compatible platform.", MessageType.Error);
    }

    private delegate PluginImporterInspector.Compatibility ValueSwitcher(PluginImporterInspector.Compatibility value);

    internal enum Compatibility
    {
      Mixed = -1,
      NotCompatible = 0,
      Compatible = 1,
    }

    private delegate bool GetComptability(PluginImporter imp);
  }
}
