// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindowLightingTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class LightingWindowLightingTab
  {
    private bool m_ShowOtherSettings = true;
    private bool m_ShowDebugSettings = false;
    private bool m_ShowProbeDebugSettings = false;
    private bool m_ShouldUpdateStatistics = true;
    private UnityEngine.Object m_RenderSettings = (UnityEngine.Object) null;
    private Editor m_LightingEditor;
    private Editor m_FogEditor;
    private Editor m_OtherRenderingEditor;
    private const string kShowOtherSettings = "kShowOtherSettings";
    private const string kShowDebugSettings = "kShowDebugSettings";
    private const string kUpdateStatistics = "kUpdateStatistics";
    private LightModeValidator.Stats m_Stats;
    private LightingWindowBakeSettings m_BakeSettings;

    private UnityEngine.Object renderSettings
    {
      get
      {
        if (this.m_RenderSettings == (UnityEngine.Object) null)
          this.m_RenderSettings = RenderSettings.GetRenderSettings();
        return this.m_RenderSettings;
      }
    }

    private Editor lightingEditor
    {
      get
      {
        if ((UnityEngine.Object) this.m_LightingEditor == (UnityEngine.Object) null || this.m_LightingEditor.target == (UnityEngine.Object) null)
          Editor.CreateCachedEditor(this.renderSettings, typeof (LightingEditor), ref this.m_LightingEditor);
        return this.m_LightingEditor;
      }
    }

    private Editor fogEditor
    {
      get
      {
        if ((UnityEngine.Object) this.m_FogEditor == (UnityEngine.Object) null || this.m_FogEditor.target == (UnityEngine.Object) null)
          Editor.CreateCachedEditor(this.renderSettings, typeof (FogEditor), ref this.m_FogEditor);
        return this.m_FogEditor;
      }
    }

    private Editor otherRenderingEditor
    {
      get
      {
        if ((UnityEngine.Object) this.m_OtherRenderingEditor == (UnityEngine.Object) null || this.m_OtherRenderingEditor.target == (UnityEngine.Object) null)
          Editor.CreateCachedEditor(this.renderSettings, typeof (OtherRenderingEditor), ref this.m_OtherRenderingEditor);
        return this.m_OtherRenderingEditor;
      }
    }

    public void OnEnable()
    {
      this.m_BakeSettings = new LightingWindowBakeSettings();
      this.m_BakeSettings.OnEnable();
      this.m_ShowOtherSettings = SessionState.GetBool("kShowOtherSettings", true);
      this.m_ShowDebugSettings = SessionState.GetBool("kShowDebugSettings", false);
      this.m_ShouldUpdateStatistics = SessionState.GetBool("kUpdateStatistics", false);
    }

    public void OnDisable()
    {
      this.m_BakeSettings.OnDisable();
      SessionState.SetBool("kShowOtherSettings", this.m_ShowOtherSettings);
      SessionState.SetBool("kShowDebugSettings", this.m_ShowDebugSettings);
      SessionState.SetBool("kUpdateStatistics", this.m_ShouldUpdateStatistics);
      this.ClearCachedProperties();
    }

    private void ClearCachedProperties()
    {
      if ((UnityEngine.Object) this.m_LightingEditor != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_LightingEditor);
        this.m_LightingEditor = (Editor) null;
      }
      if ((UnityEngine.Object) this.m_FogEditor != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_FogEditor);
        this.m_FogEditor = (Editor) null;
      }
      if (!((UnityEngine.Object) this.m_OtherRenderingEditor != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_OtherRenderingEditor);
      this.m_OtherRenderingEditor = (Editor) null;
    }

    private void DebugSettingsGUI()
    {
      this.m_ShowDebugSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowDebugSettings, LightingWindowLightingTab.Styles.DebugSettings, true);
      if (!this.m_ShowDebugSettings)
        return;
      ++EditorGUI.indentLevel;
      this.m_ShowProbeDebugSettings = EditorGUILayout.Foldout(this.m_ShowProbeDebugSettings, LightingWindowLightingTab.Styles.LightProbeVisualization, true);
      if (this.m_ShowProbeDebugSettings)
      {
        EditorGUI.BeginChangeCheck();
        ++EditorGUI.indentLevel;
        LightProbeVisualization.lightProbeVisualizationMode = (LightProbeVisualization.LightProbeVisualizationMode) EditorGUILayout.EnumPopup((Enum) LightProbeVisualization.lightProbeVisualizationMode);
        LightProbeVisualization.showInterpolationWeights = EditorGUILayout.Toggle("Display Weights", LightProbeVisualization.showInterpolationWeights, new GUILayoutOption[0]);
        LightProbeVisualization.showOcclusions = EditorGUILayout.Toggle("Display Occlusion", LightProbeVisualization.showOcclusions, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
        if (EditorGUI.EndChangeCheck())
          EditorApplication.SetSceneRepaintDirty();
      }
      EditorGUILayout.Space();
      this.m_BakeSettings.DeveloperBuildSettingsGUI();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }

    private void OtherSettingsGUI()
    {
      this.m_ShowOtherSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowOtherSettings, LightingWindowLightingTab.Styles.OtherSettings, true);
      if (!this.m_ShowOtherSettings)
        return;
      ++EditorGUI.indentLevel;
      this.fogEditor.OnInspectorGUI();
      this.otherRenderingEditor.OnInspectorGUI();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }

    public void OnGUI()
    {
      EditorGUIUtility.hierarchyMode = true;
      this.lightingEditor.OnInspectorGUI();
      this.m_BakeSettings.OnGUI();
      this.OtherSettingsGUI();
      this.DebugSettingsGUI();
    }

    public void StatisticsPreview(Rect r)
    {
      GUI.Box(r, "", (GUIStyle) "PreBackground");
      LightingWindowLightingTab.Styles.StatsTableHeader.alignment = TextAnchor.MiddleLeft;
      EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.Height(r.height));
      bool flag1 = this.m_ShouldUpdateStatistics || !EditorApplication.isPlayingOrWillChangePlaymode;
      if (flag1)
      {
        LightModeUtil.Get().AnalyzeScene(ref this.m_Stats);
      }
      else
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.HelpBox(LightingWindowLightingTab.Styles.StatisticsWarning, MessageType.Info);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.Space();
      bool flag2 = LightModeUtil.Get().AreBakedLightmapsEnabled() && this.m_Stats.receiverMask == LightModeValidator.Receivers.None;
      using (new EditorGUI.DisabledScope(!flag1))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LightingWindowLightingTab.\u003CStatisticsPreview\u003Ec__AnonStorey0 previewCAnonStorey0 = new LightingWindowLightingTab.\u003CStatisticsPreview\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        previewCAnonStorey0.opts_icon = new GUILayoutOption[2]
        {
          GUILayout.MinWidth(16f),
          GUILayout.MaxWidth(16f)
        };
        // ISSUE: reference to a compiler-generated field
        previewCAnonStorey0.opts_name = new GUILayoutOption[2]
        {
          GUILayout.MinWidth(175f),
          GUILayout.MaxWidth(200f)
        };
        // ISSUE: reference to a compiler-generated field
        previewCAnonStorey0.opts = new GUILayoutOption[2]
        {
          GUILayout.MinWidth(10f),
          GUILayout.MaxWidth(65f)
        };
        using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
        {
          // ISSUE: reference to a compiler-generated field
          EditorGUILayout.LabelField(GUIContent.none, LightingWindowLightingTab.Styles.StatsTableHeader, previewCAnonStorey0.opts_icon);
          // ISSUE: reference to a compiler-generated field
          EditorGUILayout.LabelField(LightingWindowLightingTab.Styles.StatisticsCategory, LightingWindowLightingTab.Styles.StatsTableHeader, previewCAnonStorey0.opts_name);
          // ISSUE: reference to a compiler-generated field
          EditorGUILayout.LabelField(LightingWindowLightingTab.Styles.StatisticsEnabled, LightingWindowLightingTab.Styles.StatsTableHeader, previewCAnonStorey0.opts);
          // ISSUE: reference to a compiler-generated field
          EditorGUILayout.LabelField(LightingWindowLightingTab.Styles.StatisticsDisabled, LightingWindowLightingTab.Styles.StatsTableHeader, previewCAnonStorey0.opts);
          // ISSUE: reference to a compiler-generated field
          EditorGUILayout.LabelField(LightingWindowLightingTab.Styles.StatisticsInactive, LightingWindowLightingTab.Styles.StatsTableHeader, previewCAnonStorey0.opts);
        }
        // ISSUE: reference to a compiler-generated method
        LightingWindowLightingTab.DrawStats drawStats = new LightingWindowLightingTab.DrawStats(previewCAnonStorey0.\u003C\u003Em__0);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.RealtimeLights, (int) this.m_Stats.enabled.realtimeLightsCount, (int) this.m_Stats.active.realtimeLightsCount, (int) this.m_Stats.inactive.realtimeLightsCount);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.MixedLights, (int) this.m_Stats.enabled.mixedLightsCount, (int) this.m_Stats.active.mixedLightsCount, (int) this.m_Stats.inactive.mixedLightsCount);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.BakedLights, (int) this.m_Stats.enabled.bakedLightsCount, (int) this.m_Stats.active.bakedLightsCount, (int) this.m_Stats.inactive.bakedLightsCount);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.DynamicMeshes, (int) this.m_Stats.enabled.dynamicMeshesCount, (int) this.m_Stats.active.dynamicMeshesCount, (int) this.m_Stats.inactive.dynamicMeshesCount);
        drawStats(!flag2 ? GUIContent.none : LightingWindowLightingTab.Styles.StaticMeshesIconWarning, LightingWindowLightingTab.Styles.StaticMeshes, (int) this.m_Stats.enabled.staticMeshesCount, (int) this.m_Stats.active.staticMeshesCount, (int) this.m_Stats.inactive.staticMeshesCount);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.RealtimeEmissiveMaterials, (int) this.m_Stats.enabled.staticMeshesRealtimeEmissive, (int) this.m_Stats.active.staticMeshesRealtimeEmissive, (int) this.m_Stats.inactive.staticMeshesRealtimeEmissive);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.BakedEmissiveMaterials, (int) this.m_Stats.enabled.staticMeshesBakedEmissive, (int) this.m_Stats.active.staticMeshesBakedEmissive, (int) this.m_Stats.inactive.staticMeshesBakedEmissive);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.LightProbeGroups, (int) this.m_Stats.enabled.lightProbeGroupsCount, (int) this.m_Stats.active.lightProbeGroupsCount, (int) this.m_Stats.inactive.lightProbeGroupsCount);
        drawStats(GUIContent.none, LightingWindowLightingTab.Styles.ReflectionProbes, (int) this.m_Stats.enabled.reflectionProbesCount, (int) this.m_Stats.active.reflectionProbesCount, (int) this.m_Stats.inactive.reflectionProbesCount);
      }
      EditorGUILayout.Space();
      EditorGUILayout.EndScrollView();
    }

    private static class Styles
    {
      public static readonly GUIContent OtherSettings = EditorGUIUtility.TextContent("Other Settings");
      public static readonly GUIContent DebugSettings = EditorGUIUtility.TextContent("Debug Settings");
      public static readonly GUIContent LightProbeVisualization = EditorGUIUtility.TextContent("Light Probe Visualization");
      public static readonly GUIContent StatisticsCategory = EditorGUIUtility.TextContent("Category");
      public static readonly GUIContent StatisticsEnabled = EditorGUIUtility.TextContent("Enabled");
      public static readonly GUIContent StatisticsDisabled = EditorGUIUtility.TextContent("Disabled|The Light’s GameObject is active, but the Light component is disabled. These lights have no effect on the Scene.");
      public static readonly GUIContent StatisticsInactive = EditorGUIUtility.TextContent("Inactive|The Light’s GameObject is inactive. These lights have no effect on the Scene.");
      public static readonly GUIContent UpdateStatistics = EditorGUIUtility.TextContent("Update Statistics|Turn off to prevent statistics from being updated during play mode to improve performance.");
      public static readonly string StatisticsWarning = "Statistics are not updated during play mode. This behavior can be changed via Settings -> Debug Settings -> \"Update Statistics\"";
      public static GUIStyle StatsTableHeader = new GUIStyle((GUIStyle) "preLabel");
      public static GUIStyle StatsTableContent = new GUIStyle(EditorStyles.whiteLabel);
      public static readonly GUIContent RealtimeLights = EditorGUIUtility.TextContent("Realtime Lights");
      public static readonly GUIContent MixedLights = EditorGUIUtility.TextContent("Mixed Lights");
      public static readonly GUIContent BakedLights = EditorGUIUtility.TextContent("Baked Lights");
      public static readonly GUIContent DynamicMeshes = EditorGUIUtility.TextContent("Dynamic Meshes");
      public static readonly GUIContent StaticMeshes = EditorGUIUtility.TextContent("Static Meshes");
      public static readonly GUIContent StaticMeshesIconWarning = EditorGUIUtility.TextContentWithIcon("|Baked Global Illumination is Enabled but there are no Static Meshes or Terrains in the Scene. Please enable the Lightmap Static property on the meshes you want included in baked lighting.", "console.warnicon");
      public static readonly GUIContent RealtimeEmissiveMaterials = EditorGUIUtility.TextContent("Realtime Emissive Materials");
      public static readonly GUIContent BakedEmissiveMaterials = EditorGUIUtility.TextContent("Baked Emissive Materials");
      public static readonly GUIContent LightProbeGroups = EditorGUIUtility.TextContent("Light Probe Groups");
      public static readonly GUIContent ReflectionProbes = EditorGUIUtility.TextContent("Reflection Probes");
    }

    private delegate void DrawStats(GUIContent icon, GUIContent label, int enabled, int active, int inactive);
  }
}
