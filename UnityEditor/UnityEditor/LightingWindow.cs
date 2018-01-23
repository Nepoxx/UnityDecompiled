// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Lighting", title = "Lighting")]
  internal class LightingWindow : EditorWindow
  {
    private static string[] s_BakeModeOptions = new string[2]{ "Bake Reflection Probes", "Clear Baked Data" };
    private static bool s_IsVisible = false;
    private LightingWindow.Mode m_Mode = LightingWindow.Mode.LightingSettings;
    private Vector2 m_ScrollPositionLighting = Vector2.zero;
    private Vector2 m_ScrollPositionOutputMaps = Vector2.zero;
    private PreviewResizer m_PreviewResizer = new PreviewResizer();
    private float m_ToolbarPadding = -1f;
    public const float kButtonWidth = 150f;
    private const string kGlobalIlluminationUnityManualPage = "file:///unity/Manual/GlobalIllumination.html";
    private LightingWindowObjectTab m_ObjectTab;
    public LightingWindowLightingTab m_LightingTab;
    private LightingWindowLightmapPreviewTab m_LightmapPreviewTab;

    private float toolbarPadding
    {
      get
      {
        if ((double) this.m_ToolbarPadding == -1.0)
          this.m_ToolbarPadding = (float) ((double) EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon).x * 2.0 + 6.0);
        return this.m_ToolbarPadding;
      }
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      this.m_LightingTab = new LightingWindowLightingTab();
      this.m_LightingTab.OnEnable();
      this.m_LightmapPreviewTab = new LightingWindowLightmapPreviewTab();
      this.m_ObjectTab = new LightingWindowObjectTab();
      this.m_ObjectTab.OnEnable((EditorWindow) this);
      this.autoRepaintOnSceneChange = false;
      this.m_PreviewResizer.Init("LightmappingPreview");
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      this.Repaint();
    }

    private void OnDisable()
    {
      this.m_LightingTab.OnDisable();
      this.m_ObjectTab.OnDisable();
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
    }

    private void OnBecameVisible()
    {
      if (LightingWindow.s_IsVisible)
        return;
      LightingWindow.s_IsVisible = true;
      LightingWindow.RepaintSceneAndGameViews();
    }

    private void OnBecameInvisible()
    {
      LightingWindow.s_IsVisible = false;
      LightingWindow.RepaintSceneAndGameViews();
    }

    private void OnSelectionChange()
    {
      this.m_LightmapPreviewTab.UpdateLightmapSelection();
      this.Repaint();
    }

    internal static void RepaintSceneAndGameViews()
    {
      SceneView.RepaintAll();
      GameView.RepaintAll();
    }

    private void OnGUI()
    {
      LightModeUtil.Get().Load();
      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(this.toolbarPadding);
      this.ModeToggle();
      this.DrawHelpGUI();
      if (this.m_Mode == LightingWindow.Mode.LightingSettings)
        this.DrawSettingsGUI();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
      switch (this.m_Mode)
      {
        case LightingWindow.Mode.LightingSettings:
          this.m_ScrollPositionLighting = EditorGUILayout.BeginScrollView(this.m_ScrollPositionLighting);
          this.m_LightingTab.OnGUI();
          EditorGUILayout.EndScrollView();
          EditorGUILayout.Space();
          break;
        case LightingWindow.Mode.OutputMaps:
          this.m_ScrollPositionOutputMaps = EditorGUILayout.BeginScrollView(this.m_ScrollPositionOutputMaps);
          this.m_LightmapPreviewTab.Maps();
          EditorGUILayout.EndScrollView();
          EditorGUILayout.Space();
          break;
      }
      this.Buttons();
      this.Summary();
      this.PreviewSection();
      if (!LightModeUtil.Get().Flush())
        return;
      InspectorWindow.RepaintAllInspectors();
    }

    private void DrawHelpGUI()
    {
      Vector2 vector2 = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
      if (!GUI.Button(GUILayoutUtility.GetRect(vector2.x, vector2.y), EditorGUI.GUIContents.helpIcon, EditorStyles.iconButton))
        return;
      Help.ShowHelpPage("file:///unity/Manual/GlobalIllumination.html");
    }

    private void DrawSettingsGUI()
    {
      Vector2 vector2 = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.titleSettingsIcon);
      Rect rect = GUILayoutUtility.GetRect(vector2.x, vector2.y);
      if (!EditorGUI.DropdownButton(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Passive, EditorStyles.iconButton))
        return;
      EditorUtility.DisplayCustomMenu(rect, new GUIContent[1]
      {
        new GUIContent("Reset")
      }, -1, new EditorUtility.SelectMenuItemFunction(this.ResetSettings), (object) null);
    }

    private void ResetSettings(object userData, string[] options, int selected)
    {
      RenderSettings.Reset();
      LightmapEditorSettings.Reset();
      LightmapSettings.Reset();
    }

    private void PreviewSection()
    {
      if (this.m_Mode == LightingWindow.Mode.OutputMaps)
      {
        EditorGUILayout.BeginHorizontal(GUIContent.none, LightingWindow.Styles.ToolbarStyle, GUILayout.Height(17f));
        GUILayout.FlexibleSpace();
        GUI.Label(GUILayoutUtility.GetLastRect(), "Preview", LightingWindow.Styles.ToolbarTitleStyle);
        EditorGUILayout.EndHorizontal();
      }
      switch (this.m_Mode)
      {
        case LightingWindow.Mode.OutputMaps:
          float height = this.m_PreviewResizer.ResizeHandle(this.position, 100f, 250f, 17f);
          Rect r1 = new Rect(0.0f, this.position.height - height, this.position.width, height);
          if ((double) height <= 0.0)
            break;
          this.m_LightmapPreviewTab.LightmapPreview(r1);
          break;
        case LightingWindow.Mode.ObjectSettings:
          int num = LightmapEditorSettings.lightmapper != LightmapEditorSettings.Lightmapper.PathTracer ? 115 : 185;
          Rect r2 = new Rect(0.0f, (float) num, this.position.width, this.position.height - (float) num);
          if (!(bool) ((UnityEngine.Object) Selection.activeGameObject))
            break;
          this.m_ObjectTab.ObjectPreview(r2);
          break;
      }
    }

    private void ModeToggle()
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.m_Mode = (LightingWindow.Mode) GUILayout.Toolbar((int) this.m_Mode, LightingWindow.Styles.ModeToggles, LightingWindow.Styles.ButtonStyle, GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void BakeDropDownCallback(object data)
    {
      switch ((LightingWindow.BakeMode) data)
      {
        case LightingWindow.BakeMode.BakeReflectionProbes:
          this.DoBakeReflectionProbes();
          break;
        case LightingWindow.BakeMode.Clear:
          this.DoClear();
          break;
      }
    }

    private void Buttons()
    {
      bool enabled = GUI.enabled;
      GUI.enabled &= !EditorApplication.isPlayingOrWillChangePlaymode;
      bool flag1 = LightModeUtil.Get().IsWorkflowAuto();
      if ((bool) ((UnityEngine.Object) Lightmapping.lightingDataAsset) && !Lightmapping.lightingDataAsset.isValid)
        EditorGUILayout.HelpBox(Lightmapping.lightingDataAsset.validityErrorMessage, MessageType.Warning);
      EditorGUILayout.Space();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      bool flag2 = GUILayout.Toggle(flag1, LightingWindow.Styles.ContinuousBakeLabel);
      if (EditorGUI.EndChangeCheck())
        LightModeUtil.Get().SetWorkflow(flag2);
      using (new EditorGUI.DisabledScope(flag2))
      {
        if (flag2 || !Lightmapping.isRunning)
        {
          if (EditorGUI.ButtonWithDropdownList(LightingWindow.Styles.BuildLabel, LightingWindow.s_BakeModeOptions, new GenericMenu.MenuFunction2(this.BakeDropDownCallback), new GUILayoutOption[1]{ GUILayout.Width(170f) }))
          {
            this.DoBake();
            GUIUtility.ExitGUI();
          }
        }
        else
        {
          if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer && LightModeUtil.Get().AreBakedLightmapsEnabled())
          {
            if (GUILayout.Button("Force Stop", new GUILayoutOption[1]{ GUILayout.Width(150f) }))
              Lightmapping.ForceStop();
          }
          if (GUILayout.Button("Cancel", new GUILayoutOption[1]{ GUILayout.Width(150f) }))
          {
            Lightmapping.Cancel();
            UsabilityAnalytics.Track("/LightMapper/Cancel");
          }
        }
      }
      GUILayout.EndHorizontal();
      EditorGUILayout.Space();
      GUI.enabled = enabled;
    }

    private void DoBake()
    {
      UsabilityAnalytics.Track("/LightMapper/Start");
      UsabilityAnalytics.Event("LightMapper", "Mode", LightmapSettings.lightmapsMode.ToString(), 1);
      UsabilityAnalytics.Event("LightMapper", "Button", "BakeScene", 1);
      Lightmapping.BakeAsync();
    }

    private void DoClear()
    {
      Lightmapping.ClearLightingDataAsset();
      Lightmapping.Clear();
      UsabilityAnalytics.Track("/LightMapper/Clear");
    }

    private void DoBakeReflectionProbes()
    {
      Lightmapping.BakeAllReflectionProbesSnapshots();
      UsabilityAnalytics.Track("/LightMapper/BakeAllReflectionProbesSnapshots");
    }

    private void Summary()
    {
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      long bytes = 0;
      int num1 = 0;
      Dictionary<Vector2, int> dictionary1 = new Dictionary<Vector2, int>();
      bool flag1 = false;
      bool flag2 = false;
      foreach (LightmapData lightmap in LightmapSettings.lightmaps)
      {
        if (!((UnityEngine.Object) lightmap.lightmapColor == (UnityEngine.Object) null))
        {
          ++num1;
          Vector2 key = new Vector2((float) lightmap.lightmapColor.width, (float) lightmap.lightmapColor.height);
          if (dictionary1.ContainsKey(key))
          {
            Dictionary<Vector2, int> dictionary2;
            Vector2 index;
            (dictionary2 = dictionary1)[index = key] = dictionary2[index] + 1;
          }
          else
            dictionary1.Add(key, 1);
          bytes += TextureUtil.GetStorageMemorySizeLong((Texture) lightmap.lightmapColor);
          if ((bool) ((UnityEngine.Object) lightmap.lightmapDir))
          {
            bytes += TextureUtil.GetStorageMemorySizeLong((Texture) lightmap.lightmapDir);
            flag1 = true;
          }
          if ((bool) ((UnityEngine.Object) lightmap.shadowMask))
          {
            bytes += TextureUtil.GetStorageMemorySizeLong((Texture) lightmap.shadowMask);
            flag2 = true;
          }
        }
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(num1);
      stringBuilder.Append(!flag1 ? " Non-Directional" : " Directional");
      stringBuilder.Append(" Lightmap");
      if (num1 != 1)
        stringBuilder.Append("s");
      if (flag2)
      {
        stringBuilder.Append(" with Shadowmask");
        if (num1 != 1)
          stringBuilder.Append("s");
      }
      bool flag3 = true;
      foreach (KeyValuePair<Vector2, int> keyValuePair in dictionary1)
      {
        stringBuilder.Append(!flag3 ? ", " : ": ");
        flag3 = false;
        if (keyValuePair.Value > 1)
        {
          stringBuilder.Append(keyValuePair.Value);
          stringBuilder.Append("x");
        }
        stringBuilder.Append(keyValuePair.Key.x);
        stringBuilder.Append("x");
        stringBuilder.Append(keyValuePair.Key.y);
        stringBuilder.Append("px");
      }
      stringBuilder.Append(" ");
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Label(stringBuilder.ToString(), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label(EditorUtility.FormatBytes(bytes), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
      GUILayout.Label(num1 != 0 ? "" : "No Lightmaps", LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer)
      {
        GUILayout.BeginVertical();
        GUILayout.Label("Occupied Texels: " + InternalEditorUtility.CountToString(Lightmapping.occupiedTexelCount), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
        if (Lightmapping.isRunning)
        {
          int num2 = 0;
          int num3 = 0;
          int num4 = 0;
          int num5 = 0;
          int num6 = 0;
          int num7 = 0;
          int num8 = 0;
          int length = LightmapSettings.lightmaps.Length;
          for (int lightmapIndex = 0; lightmapIndex < length; ++lightmapIndex)
          {
            LightmapConvergence lightmapConvergence = Lightmapping.GetLightmapConvergence(lightmapIndex);
            if (!lightmapConvergence.IsValid())
              ++num8;
            else if (Lightmapping.GetVisibleTexelCount(lightmapIndex) > 0UL)
            {
              ++num2;
              if (lightmapConvergence.IsConverged())
                ++num3;
              else
                ++num4;
            }
            else
            {
              ++num5;
              if (lightmapConvergence.IsConverged())
                ++num6;
              else
                ++num7;
            }
          }
          EditorGUILayout.LabelField("Lightmaps in view: " + (object) num2, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          ++EditorGUI.indentLevel;
          EditorGUILayout.LabelField("Converged: " + (object) num3, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Not Converged: " + (object) num4, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          --EditorGUI.indentLevel;
          EditorGUILayout.LabelField("Lightmaps not in view: " + (object) num5, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          ++EditorGUI.indentLevel;
          EditorGUILayout.LabelField("Converged: " + (object) num6, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Not Converged: " + (object) num7, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          --EditorGUI.indentLevel;
        }
        float lightmapBakeTimeTotal = Lightmapping.GetLightmapBakeTimeTotal();
        float performanceTotal = Lightmapping.GetLightmapBakePerformanceTotal();
        if ((double) performanceTotal >= 0.0)
          GUILayout.Label("Bake Performance: " + performanceTotal.ToString("0.00") + " mrays/sec", LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
        if (!Lightmapping.isRunning)
        {
          float lightmapBakeTimeRaw = Lightmapping.GetLightmapBakeTimeRaw();
          if ((double) lightmapBakeTimeTotal >= 0.0)
          {
            int num2 = (int) lightmapBakeTimeTotal;
            int num3 = num2 / 3600;
            int num4 = num2 - 3600 * num3;
            int num5 = num4 / 60;
            int num6 = num4 - 60 * num5;
            int num7 = (int) lightmapBakeTimeRaw;
            int num8 = num7 / 3600;
            int num9 = num7 - 3600 * num8;
            int num10 = num9 / 60;
            int num11 = num9 - 60 * num10;
            int num12 = Math.Max(0, (int) ((double) lightmapBakeTimeTotal - (double) lightmapBakeTimeRaw));
            int num13 = num12 / 3600;
            int num14 = num12 - 3600 * num13;
            int num15 = num14 / 60;
            int num16 = num14 - 60 * num15;
            GUILayout.Label("Total Bake Time: " + num3.ToString("0") + ":" + num5.ToString("00") + ":" + num6.ToString("00"), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
            if (Unsupported.IsDeveloperBuild())
              GUILayout.Label("(Raw Bake Time: " + num8.ToString("0") + ":" + num10.ToString("00") + ":" + num11.ToString("00") + ", Overhead: " + num13.ToString("0") + ":" + num15.ToString("00") + ":" + num16.ToString("00") + ")", LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
          }
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndVertical();
    }

    [MenuItem("Window/Lighting/Settings", false, 2098)]
    private static void CreateLightingWindow()
    {
      LightingWindow window = EditorWindow.GetWindow<LightingWindow>();
      window.minSize = new Vector2(360f, 390f);
      window.Show();
    }

    private enum Mode
    {
      LightingSettings,
      OutputMaps,
      ObjectSettings,
    }

    private enum BakeMode
    {
      BakeReflectionProbes,
      Clear,
    }

    private static class Styles
    {
      public static readonly GUIContent[] ModeToggles = new GUIContent[3]{ EditorGUIUtility.TextContent("Scene"), EditorGUIUtility.TextContent("Global Maps"), EditorGUIUtility.TextContent("Object Maps") };
      public static readonly GUIContent ContinuousBakeLabel = EditorGUIUtility.TextContent("Auto Generate|Automatically generates lighting data in the Scene when any changes are made to the lighting systems.");
      public static readonly GUIContent BuildLabel = EditorGUIUtility.TextContent("Generate Lighting|Generates the lightmap data for the current master scene.  This lightmap data (for realtime and baked global illumination) is stored in the GI Cache. For GI Cache settings see the Preferences panel.");
      public static readonly GUIStyle LabelStyle = EditorStyles.wordWrappedMiniLabel;
      public static readonly GUIStyle ToolbarStyle = (GUIStyle) "preToolbar";
      public static readonly GUIStyle ToolbarTitleStyle = (GUIStyle) "preToolbar";
      public static readonly GUIStyle ButtonStyle = (GUIStyle) "LargeButton";
    }
  }
}
