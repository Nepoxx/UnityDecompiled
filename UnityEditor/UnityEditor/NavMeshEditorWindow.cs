// Decompiled with JetBrains decompiler
// Type: UnityEditor.NavMeshEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Navigation", title = "Navigation")]
  internal class NavMeshEditorWindow : EditorWindow, IHasCustomMenu
  {
    private Vector2 m_ScrollPos = Vector2.zero;
    private int m_SelectedNavMeshAgentCount = 0;
    private int m_SelectedNavMeshObstacleCount = 0;
    private bool m_HasPendingAgentDebugInfo = false;
    private bool m_HasRepaintedForPendingAgentDebugInfo = false;
    private ReorderableList m_AreasList = (ReorderableList) null;
    private ReorderableList m_AgentsList = (ReorderableList) null;
    private NavMeshEditorWindow.Mode m_Mode = NavMeshEditorWindow.Mode.ObjectSettings;
    private bool m_BecameVisibleCalled = false;
    private static NavMeshEditorWindow s_NavMeshEditorWindow;
    private SerializedObject m_SettingsObject;
    private SerializedProperty m_AgentRadius;
    private SerializedProperty m_AgentHeight;
    private SerializedProperty m_AgentSlope;
    private SerializedProperty m_AgentClimb;
    private SerializedProperty m_LedgeDropHeight;
    private SerializedProperty m_MaxJumpAcrossDistance;
    private SerializedProperty m_MinRegionArea;
    private SerializedProperty m_ManualCellSize;
    private SerializedProperty m_CellSize;
    private SerializedProperty m_AccuratePlacement;
    private SerializedObject m_NavMeshProjectSettingsObject;
    private SerializedProperty m_Areas;
    private SerializedProperty m_Agents;
    private SerializedProperty m_SettingNames;
    private const string kRootPath = "m_BuildSettings.";
    private static NavMeshEditorWindow.Styles s_Styles;
    private bool m_Advanced;

    [MenuItem("Window/Navigation", false, 2100)]
    public static void SetupWindow()
    {
      EditorWindow.GetWindow<NavMeshEditorWindow>(new System.Type[1]
      {
        typeof (InspectorWindow)
      }).minSize = new Vector2(300f, 360f);
    }

    public static void OpenAreaSettings()
    {
      NavMeshEditorWindow.SetupWindow();
      if ((UnityEngine.Object) NavMeshEditorWindow.s_NavMeshEditorWindow == (UnityEngine.Object) null)
        return;
      NavMeshEditorWindow.s_NavMeshEditorWindow.m_Mode = NavMeshEditorWindow.Mode.AreaSettings;
      NavMeshEditorWindow.s_NavMeshEditorWindow.InitProjectSettings();
      NavMeshEditorWindow.s_NavMeshEditorWindow.InitAgents();
    }

    public static void OpenAgentSettings(int agentTypeID)
    {
      NavMeshEditorWindow.SetupWindow();
      if ((UnityEngine.Object) NavMeshEditorWindow.s_NavMeshEditorWindow == (UnityEngine.Object) null)
        return;
      NavMeshEditorWindow.s_NavMeshEditorWindow.m_Mode = NavMeshEditorWindow.Mode.AgentSettings;
      NavMeshEditorWindow.s_NavMeshEditorWindow.InitProjectSettings();
      NavMeshEditorWindow.s_NavMeshEditorWindow.InitAgents();
      NavMeshEditorWindow.s_NavMeshEditorWindow.m_AgentsList.index = -1;
      for (int index = 0; index < NavMeshEditorWindow.s_NavMeshEditorWindow.m_Agents.arraySize; ++index)
      {
        if (NavMeshEditorWindow.s_NavMeshEditorWindow.m_Agents.GetArrayElementAtIndex(index).FindPropertyRelative(nameof (agentTypeID)).intValue == agentTypeID)
        {
          NavMeshEditorWindow.s_NavMeshEditorWindow.m_AgentsList.index = index;
          break;
        }
      }
    }

    public void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      NavMeshEditorWindow.s_NavMeshEditorWindow = this;
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      this.UpdateSelectedAgentAndObstacleState();
      this.Repaint();
    }

    private void InitProjectSettings()
    {
      if (this.m_NavMeshProjectSettingsObject != null)
        return;
      this.m_NavMeshProjectSettingsObject = new SerializedObject(Unsupported.GetSerializedAssetInterfaceSingleton("NavMeshProjectSettings"));
    }

    private void InitSceneBakeSettings()
    {
      this.m_SettingsObject = new SerializedObject(UnityEditor.AI.NavMeshBuilder.navMeshSettingsObject);
      this.m_AgentRadius = this.m_SettingsObject.FindProperty("m_BuildSettings.agentRadius");
      this.m_AgentHeight = this.m_SettingsObject.FindProperty("m_BuildSettings.agentHeight");
      this.m_AgentSlope = this.m_SettingsObject.FindProperty("m_BuildSettings.agentSlope");
      this.m_LedgeDropHeight = this.m_SettingsObject.FindProperty("m_BuildSettings.ledgeDropHeight");
      this.m_AgentClimb = this.m_SettingsObject.FindProperty("m_BuildSettings.agentClimb");
      this.m_MaxJumpAcrossDistance = this.m_SettingsObject.FindProperty("m_BuildSettings.maxJumpAcrossDistance");
      this.m_MinRegionArea = this.m_SettingsObject.FindProperty("m_BuildSettings.minRegionArea");
      this.m_ManualCellSize = this.m_SettingsObject.FindProperty("m_BuildSettings.manualCellSize");
      this.m_CellSize = this.m_SettingsObject.FindProperty("m_BuildSettings.cellSize");
      this.m_AccuratePlacement = this.m_SettingsObject.FindProperty("m_BuildSettings.accuratePlacement");
    }

    private void InitAreas()
    {
      if (this.m_Areas == null)
        this.m_Areas = this.m_NavMeshProjectSettingsObject.FindProperty("areas");
      if (this.m_AreasList != null)
        return;
      this.m_AreasList = new ReorderableList(this.m_NavMeshProjectSettingsObject, this.m_Areas, false, false, false, false);
      this.m_AreasList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawAreaListElement);
      this.m_AreasList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawAreaListHeader);
      this.m_AreasList.elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    private void InitAgents()
    {
      if (this.m_Agents == null)
      {
        this.m_Agents = this.m_NavMeshProjectSettingsObject.FindProperty("m_Settings");
        this.m_SettingNames = this.m_NavMeshProjectSettingsObject.FindProperty("m_SettingNames");
      }
      if (this.m_AgentsList != null)
        return;
      this.m_AgentsList = new ReorderableList(this.m_NavMeshProjectSettingsObject, this.m_Agents, false, false, true, true);
      this.m_AgentsList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawAgentListElement);
      this.m_AgentsList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawAgentListHeader);
      this.m_AgentsList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddAgent);
      this.m_AgentsList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveAgent);
      this.m_AgentsList.elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    private int Bit(int a, int b)
    {
      return (a & 1 << b) >> b;
    }

    private Color GetAreaColor(int i)
    {
      if (i == 0)
        return new Color(0.0f, 0.75f, 1f, 0.5f);
      return new Color((float) ((this.Bit(i, 4) + this.Bit(i, 1) * 2 + 1) * 63) / (float) byte.MaxValue, (float) ((this.Bit(i, 3) + this.Bit(i, 2) * 2 + 1) * 63) / (float) byte.MaxValue, (float) ((this.Bit(i, 5) + this.Bit(i, 0) * 2 + 1) * 63) / (float) byte.MaxValue, 0.5f);
    }

    public void OnDisable()
    {
      NavMeshEditorWindow.s_NavMeshEditorWindow = (NavMeshEditorWindow) null;
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneViewGUI);
    }

    private void UpdateSelectedAgentAndObstacleState()
    {
      UnityEngine.Object[] filtered1 = Selection.GetFiltered(typeof (NavMeshAgent), SelectionMode.ExcludePrefab | SelectionMode.Editable);
      UnityEngine.Object[] filtered2 = Selection.GetFiltered(typeof (NavMeshObstacle), SelectionMode.ExcludePrefab | SelectionMode.Editable);
      this.m_SelectedNavMeshAgentCount = filtered1.Length;
      this.m_SelectedNavMeshObstacleCount = filtered2.Length;
    }

    private void OnSelectionChange()
    {
      this.UpdateSelectedAgentAndObstacleState();
      this.m_ScrollPos = Vector2.zero;
      if (this.m_Mode != NavMeshEditorWindow.Mode.ObjectSettings)
        return;
      this.Repaint();
    }

    private void ModeToggle()
    {
      if (NavMeshEditorWindow.s_Styles == null)
        NavMeshEditorWindow.s_Styles = new NavMeshEditorWindow.Styles();
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.m_Mode = (NavMeshEditorWindow.Mode) GUILayout.Toolbar((int) this.m_Mode, NavMeshEditorWindow.s_Styles.m_ModeToggles, (GUIStyle) "LargeButton", GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void GetAreaListRects(Rect rect, out Rect stripeRect, out Rect labelRect, out Rect nameRect, out Rect costRect)
    {
      float num1 = EditorGUIUtility.singleLineHeight * 0.8f;
      float num2 = EditorGUIUtility.singleLineHeight * 5f;
      float width = EditorGUIUtility.singleLineHeight * 4f;
      float num3 = rect.width - num1 - num2 - width;
      float x1 = rect.x;
      stripeRect = new Rect(x1, rect.y, num1 - 4f, rect.height);
      float x2 = x1 + num1;
      labelRect = new Rect(x2, rect.y, num2 - 4f, rect.height);
      float x3 = x2 + num2;
      nameRect = new Rect(x3, rect.y, num3 - 4f, rect.height);
      float x4 = x3 + num3;
      costRect = new Rect(x4, rect.y, width, rect.height);
    }

    private void DrawAreaListHeader(Rect rect)
    {
      Rect stripeRect;
      Rect labelRect;
      Rect nameRect;
      Rect costRect;
      this.GetAreaListRects(rect, out stripeRect, out labelRect, out nameRect, out costRect);
      GUI.Label(nameRect, NavMeshEditorWindow.s_Styles.m_NameLabel);
      GUI.Label(costRect, NavMeshEditorWindow.s_Styles.m_CostLabel);
    }

    private void DrawAreaListElement(Rect rect, int index, bool selected, bool focused)
    {
      SerializedProperty arrayElementAtIndex = this.m_Areas.GetArrayElementAtIndex(index);
      if (arrayElementAtIndex == null)
        return;
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("name");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("cost");
      if (propertyRelative1 == null || propertyRelative2 == null)
        return;
      rect.height -= 2f;
      bool flag1;
      bool flag2;
      bool flag3;
      switch (index)
      {
        case 0:
          flag1 = true;
          flag2 = false;
          flag3 = true;
          break;
        case 1:
          flag1 = true;
          flag2 = false;
          flag3 = false;
          break;
        case 2:
          flag1 = true;
          flag2 = false;
          flag3 = true;
          break;
        default:
          flag1 = false;
          flag2 = true;
          flag3 = true;
          break;
      }
      Rect stripeRect;
      Rect labelRect;
      Rect nameRect;
      Rect costRect;
      this.GetAreaListRects(rect, out stripeRect, out labelRect, out nameRect, out costRect);
      bool enabled = GUI.enabled;
      Color areaColor = this.GetAreaColor(index);
      Color color = new Color(areaColor.r * 0.1f, areaColor.g * 0.1f, areaColor.b * 0.1f, 0.6f);
      EditorGUI.DrawRect(stripeRect, areaColor);
      EditorGUI.DrawRect(new Rect(stripeRect.x, stripeRect.y, 1f, stripeRect.height), color);
      EditorGUI.DrawRect(new Rect((float) ((double) stripeRect.x + (double) stripeRect.width - 1.0), stripeRect.y, 1f, stripeRect.height), color);
      EditorGUI.DrawRect(new Rect(stripeRect.x + 1f, stripeRect.y, stripeRect.width - 2f, 1f), color);
      EditorGUI.DrawRect(new Rect(stripeRect.x + 1f, (float) ((double) stripeRect.y + (double) stripeRect.height - 1.0), stripeRect.width - 2f, 1f), color);
      if (flag1)
        GUI.Label(labelRect, EditorGUIUtility.TempContent("Built-in " + (object) index));
      else
        GUI.Label(labelRect, EditorGUIUtility.TempContent("User " + (object) index));
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      GUI.enabled = enabled && flag2;
      EditorGUI.PropertyField(nameRect, propertyRelative1, GUIContent.none);
      GUI.enabled = enabled && flag3;
      EditorGUI.PropertyField(costRect, propertyRelative2, GUIContent.none);
      GUI.enabled = enabled;
      EditorGUI.indentLevel = indentLevel;
    }

    private void AddAgent(ReorderableList list)
    {
      NavMesh.CreateSettings();
      list.index = NavMesh.GetSettingsCount() - 1;
    }

    private void RemoveAgent(ReorderableList list)
    {
      SerializedProperty arrayElementAtIndex = this.m_Agents.GetArrayElementAtIndex(list.index);
      if (arrayElementAtIndex == null)
        return;
      SerializedProperty propertyRelative = arrayElementAtIndex.FindPropertyRelative("agentTypeID");
      if (propertyRelative == null || propertyRelative.intValue == 0)
        return;
      this.m_SettingNames.DeleteArrayElementAtIndex(list.index);
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
    }

    private void DrawAgentListHeader(Rect rect)
    {
      GUI.Label(rect, NavMeshEditorWindow.s_Styles.m_AgentTypesHeader);
    }

    private void DrawAgentListElement(Rect rect, int index, bool selected, bool focused)
    {
      SerializedProperty arrayElementAtIndex = this.m_Agents.GetArrayElementAtIndex(index);
      if (arrayElementAtIndex == null)
        return;
      SerializedProperty propertyRelative = arrayElementAtIndex.FindPropertyRelative("agentTypeID");
      if (propertyRelative == null)
        return;
      rect.height -= 2f;
      using (new EditorGUI.DisabledScope(propertyRelative.intValue == 0))
      {
        string settingsNameFromId = NavMesh.GetSettingsNameFromID(propertyRelative.intValue);
        GUI.Label(rect, EditorGUIUtility.TempContent(settingsNameFromId));
      }
    }

    public void OnGUI()
    {
      EditorGUILayout.Space();
      this.ModeToggle();
      EditorGUILayout.Space();
      this.InitProjectSettings();
      this.m_ScrollPos = EditorGUILayout.BeginScrollView(this.m_ScrollPos);
      switch (this.m_Mode)
      {
        case NavMeshEditorWindow.Mode.AgentSettings:
          this.AgentSettings();
          break;
        case NavMeshEditorWindow.Mode.AreaSettings:
          this.AreaSettings();
          break;
        case NavMeshEditorWindow.Mode.SceneBakeSettings:
          this.SceneBakeSettings();
          break;
        case NavMeshEditorWindow.Mode.ObjectSettings:
          NavMeshEditorWindow.ObjectSettings();
          break;
      }
      EditorGUILayout.EndScrollView();
    }

    public void OnBecameVisible()
    {
      if (this.m_BecameVisibleCalled)
        return;
      bool flag = NavMeshVisualizationSettings.showNavigation == 0;
      ++NavMeshVisualizationSettings.showNavigation;
      if (flag)
        NavMeshEditorWindow.RepaintSceneAndGameViews();
      this.m_BecameVisibleCalled = true;
    }

    public void OnBecameInvisible()
    {
      if (!this.m_BecameVisibleCalled)
        return;
      --NavMeshVisualizationSettings.showNavigation;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
      this.m_BecameVisibleCalled = false;
    }

    private static void RepaintSceneAndGameViews()
    {
      SceneView.RepaintAll();
      foreach (EditorWindow editorWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (GameView)))
        editorWindow.Repaint();
    }

    public void OnSceneViewGUI(SceneView sceneView)
    {
      if (NavMeshVisualizationSettings.showNavigation == 0)
        return;
      GUIContent title1 = new GUIContent("Navmesh Display");
      // ISSUE: reference to a compiler-generated field
      if (NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache0 = new SceneViewOverlay.WindowFunction(NavMeshEditorWindow.DisplayControls);
      }
      // ISSUE: reference to a compiler-generated field
      SceneViewOverlay.WindowFunction fMgCache0 = NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache0;
      int order1 = 400;
      int num1 = 1;
      SceneViewOverlay.Window(title1, fMgCache0, order1, (SceneViewOverlay.WindowDisplayOption) num1);
      if (this.m_SelectedNavMeshAgentCount > 0)
      {
        GUIContent title2 = new GUIContent("Agent Display");
        // ISSUE: reference to a compiler-generated field
        if (NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache1 = new SceneViewOverlay.WindowFunction(NavMeshEditorWindow.DisplayAgentControls);
        }
        // ISSUE: reference to a compiler-generated field
        SceneViewOverlay.WindowFunction fMgCache1 = NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache1;
        int order2 = 400;
        int num2 = 1;
        SceneViewOverlay.Window(title2, fMgCache1, order2, (SceneViewOverlay.WindowDisplayOption) num2);
      }
      if (this.m_SelectedNavMeshObstacleCount <= 0)
        return;
      GUIContent title3 = new GUIContent("Obstacle Display");
      // ISSUE: reference to a compiler-generated field
      if (NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache2 = new SceneViewOverlay.WindowFunction(NavMeshEditorWindow.DisplayObstacleControls);
      }
      // ISSUE: reference to a compiler-generated field
      SceneViewOverlay.WindowFunction fMgCache2 = NavMeshEditorWindow.\u003C\u003Ef__mg\u0024cache2;
      int order3 = 400;
      int num3 = 1;
      SceneViewOverlay.Window(title3, fMgCache2, order3, (SceneViewOverlay.WindowDisplayOption) num3);
    }

    private static void DisplayControls(UnityEngine.Object target, SceneView sceneView)
    {
      EditorGUIUtility.labelWidth = 150f;
      bool flag = false;
      bool showNavMesh = NavMeshVisualizationSettings.showNavMesh;
      if (showNavMesh != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show NavMesh"), showNavMesh, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showNavMesh = !showNavMesh;
        flag = true;
      }
      using (new EditorGUI.DisabledScope(!NavMeshVisualizationSettings.hasHeightMesh))
      {
        bool showHeightMesh = NavMeshVisualizationSettings.showHeightMesh;
        if (showHeightMesh != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show HeightMesh"), showHeightMesh, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showHeightMesh = !showHeightMesh;
          flag = true;
        }
      }
      if (Unsupported.IsDeveloperBuild())
      {
        GUILayout.Label("Internal");
        bool showNavMeshPortals = NavMeshVisualizationSettings.showNavMeshPortals;
        if (showNavMeshPortals != EditorGUILayout.Toggle(new GUIContent("Show NavMesh Portals"), showNavMeshPortals, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showNavMeshPortals = !showNavMeshPortals;
          flag = true;
        }
        bool showNavMeshLinks = NavMeshVisualizationSettings.showNavMeshLinks;
        if (showNavMeshLinks != EditorGUILayout.Toggle(new GUIContent("Show NavMesh Links"), showNavMeshLinks, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showNavMeshLinks = !showNavMeshLinks;
          flag = true;
        }
        bool showProximityGrid = NavMeshVisualizationSettings.showProximityGrid;
        if (showProximityGrid != EditorGUILayout.Toggle(new GUIContent("Show Proximity Grid"), showProximityGrid, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showProximityGrid = !showProximityGrid;
          flag = true;
        }
        bool heightMeshBvTree = NavMeshVisualizationSettings.showHeightMeshBVTree;
        if (heightMeshBvTree != EditorGUILayout.Toggle(new GUIContent("Show HeightMesh BV-Tree"), heightMeshBvTree, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showHeightMeshBVTree = !heightMeshBvTree;
          flag = true;
        }
      }
      if (!flag)
        return;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    private void OnInspectorUpdate()
    {
      if (this.m_SelectedNavMeshAgentCount <= 0)
        return;
      if (this.m_HasPendingAgentDebugInfo != NavMeshVisualizationSettings.hasPendingAgentDebugInfo)
      {
        if (!this.m_HasRepaintedForPendingAgentDebugInfo)
        {
          this.m_HasRepaintedForPendingAgentDebugInfo = true;
          NavMeshEditorWindow.RepaintSceneAndGameViews();
        }
      }
      else
        this.m_HasRepaintedForPendingAgentDebugInfo = false;
    }

    private static void DisplayAgentControls(UnityEngine.Object target, SceneView sceneView)
    {
      EditorGUIUtility.labelWidth = 150f;
      bool flag = false;
      if (Event.current.type == EventType.Layout)
        NavMeshEditorWindow.s_NavMeshEditorWindow.m_HasPendingAgentDebugInfo = NavMeshVisualizationSettings.hasPendingAgentDebugInfo;
      bool showAgentPath = NavMeshVisualizationSettings.showAgentPath;
      if (showAgentPath != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Path Polygons|Shows the polygons leading to goal."), showAgentPath, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentPath = !showAgentPath;
        flag = true;
      }
      bool showAgentPathInfo = NavMeshVisualizationSettings.showAgentPathInfo;
      if (showAgentPathInfo != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Path Query Nodes|Shows the nodes expanded during last path query."), showAgentPathInfo, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentPathInfo = !showAgentPathInfo;
        flag = true;
      }
      bool showAgentNeighbours = NavMeshVisualizationSettings.showAgentNeighbours;
      if (showAgentNeighbours != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Neighbours|Show the agent neighbours cosidered during simulation."), showAgentNeighbours, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentNeighbours = !showAgentNeighbours;
        flag = true;
      }
      bool showAgentWalls = NavMeshVisualizationSettings.showAgentWalls;
      if (showAgentWalls != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Walls|Shows the wall segments handled during simulation."), showAgentWalls, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentWalls = !showAgentWalls;
        flag = true;
      }
      bool showAgentAvoidance = NavMeshVisualizationSettings.showAgentAvoidance;
      if (showAgentAvoidance != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Avoidance|Shows the processed avoidance geometry from simulation."), showAgentAvoidance, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentAvoidance = !showAgentAvoidance;
        flag = true;
      }
      if (showAgentAvoidance)
      {
        if (NavMeshEditorWindow.s_NavMeshEditorWindow.m_HasPendingAgentDebugInfo)
        {
          EditorGUILayout.BeginVertical(GUILayout.MaxWidth(165f));
          EditorGUILayout.HelpBox("Avoidance display is not valid until after next game update.", MessageType.Warning);
          EditorGUILayout.EndVertical();
        }
        if (NavMeshEditorWindow.s_NavMeshEditorWindow.m_SelectedNavMeshAgentCount > 10)
        {
          EditorGUILayout.BeginVertical(GUILayout.MaxWidth(165f));
          EditorGUILayout.HelpBox(string.Format("Avoidance visualization can be drawn for {0} agents ({1} selected).", (object) 10, (object) NavMeshEditorWindow.s_NavMeshEditorWindow.m_SelectedNavMeshAgentCount), MessageType.Warning);
          EditorGUILayout.EndVertical();
        }
      }
      if (!flag)
        return;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    private static void DisplayObstacleControls(UnityEngine.Object target, SceneView sceneView)
    {
      EditorGUIUtility.labelWidth = 150f;
      bool flag = false;
      bool obstacleCarveHull = NavMeshVisualizationSettings.showObstacleCarveHull;
      if (obstacleCarveHull != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Carve Hull|Shows the hull used to carve the obstacle from navmesh."), obstacleCarveHull, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showObstacleCarveHull = !obstacleCarveHull;
        flag = true;
      }
      if (!flag)
        return;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Reset Legacy Bake Settings"), false, new GenericMenu.MenuFunction(this.ResetBakeSettings));
    }

    private void ResetBakeSettings()
    {
      Unsupported.SmartReset(UnityEditor.AI.NavMeshBuilder.navMeshSettingsObject);
    }

    public static void BackgroundTaskStatusChanged()
    {
      if (!((UnityEngine.Object) NavMeshEditorWindow.s_NavMeshEditorWindow != (UnityEngine.Object) null))
        return;
      NavMeshEditorWindow.s_NavMeshEditorWindow.Repaint();
    }

    private static IEnumerable<GameObject> GetObjectsRecurse(GameObject root)
    {
      List<GameObject> gameObjectList = new List<GameObject>() { root };
      IEnumerator enumerator = root.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          gameObjectList.AddRange(NavMeshEditorWindow.GetObjectsRecurse(current.gameObject));
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return (IEnumerable<GameObject>) gameObjectList;
    }

    private static List<GameObject> GetObjects(bool includeChildren)
    {
      if (!includeChildren)
        return new List<GameObject>((IEnumerable<GameObject>) Selection.gameObjects);
      List<GameObject> gameObjectList = new List<GameObject>();
      foreach (GameObject gameObject in Selection.gameObjects)
        gameObjectList.AddRange(NavMeshEditorWindow.GetObjectsRecurse(gameObject));
      return gameObjectList;
    }

    private static bool SelectionHasChildren()
    {
      return ((IEnumerable<GameObject>) Selection.gameObjects).Any<GameObject>((Func<GameObject, bool>) (obj => obj.transform.childCount > 0));
    }

    private static void SetNavMeshArea(int area, bool includeChildren)
    {
      List<GameObject> objects = NavMeshEditorWindow.GetObjects(includeChildren);
      if (objects.Count <= 0)
        return;
      Undo.RecordObjects((UnityEngine.Object[]) objects.ToArray(), "Change NavMesh area");
      foreach (GameObject go in objects)
        GameObjectUtility.SetNavMeshArea(go, area);
    }

    private static void ObjectSettings()
    {
      bool flag = true;
      SceneModeUtility.SearchBar(typeof (MeshRenderer), typeof (Terrain));
      EditorGUILayout.Space();
      GameObject[] gameObjects;
      MeshRenderer[] selectedObjectsOfType1 = SceneModeUtility.GetSelectedObjectsOfType<MeshRenderer>(out gameObjects);
      if (gameObjects.Length > 0)
      {
        flag = false;
        NavMeshEditorWindow.ObjectSettings((UnityEngine.Object[]) selectedObjectsOfType1, gameObjects);
      }
      Terrain[] selectedObjectsOfType2 = SceneModeUtility.GetSelectedObjectsOfType<Terrain>(out gameObjects);
      if (gameObjects.Length > 0)
      {
        flag = false;
        NavMeshEditorWindow.ObjectSettings((UnityEngine.Object[]) selectedObjectsOfType2, gameObjects);
      }
      if (!flag)
        return;
      GUILayout.Label("Select a MeshRenderer or a Terrain from the scene.", EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    private static void ComponentBasedWorkflowButton()
    {
      GUILayout.BeginHorizontal();
      if (EditorGUILayout.LinkLabel(NavMeshEditorWindow.s_Styles.m_LearnAboutComponent))
        Help.BrowseURL("https://github.com/Unity-Technologies/NavMeshComponents");
      GUILayout.EndHorizontal();
    }

    private static void ObjectSettings(UnityEngine.Object[] components, GameObject[] gos)
    {
      NavMeshEditorWindow.ComponentBasedWorkflowButton();
      EditorGUILayout.MultiSelectionObjectTitleBar(components);
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object[]) gos);
      using (new EditorGUI.DisabledScope(!SceneModeUtility.StaticFlagField("Navigation Static", serializedObject.FindProperty("m_StaticEditorFlags"), 8)))
      {
        SceneModeUtility.StaticFlagField("Generate OffMeshLinks", serializedObject.FindProperty("m_StaticEditorFlags"), 32);
        SerializedProperty property = serializedObject.FindProperty("m_NavMeshLayer");
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
        string[] navMeshAreaNames = GameObjectUtility.GetNavMeshAreaNames();
        int navMeshArea = GameObjectUtility.GetNavMeshArea(gos[0]);
        int selectedIndex = -1;
        for (int index = 0; index < navMeshAreaNames.Length; ++index)
        {
          if (GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index]) == navMeshArea)
          {
            selectedIndex = index;
            break;
          }
        }
        int index1 = EditorGUILayout.Popup("Navigation Area", selectedIndex, navMeshAreaNames, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          int meshAreaFromName = GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index1]);
          GameObjectUtility.ShouldIncludeChildren shouldIncludeChildren = GameObjectUtility.DisplayUpdateChildrenDialogIfNeeded((IEnumerable<GameObject>) Selection.gameObjects, "Change Navigation Area", "Do you want change the navigation area to " + navMeshAreaNames[index1] + " for all the child objects as well?");
          if (shouldIncludeChildren != GameObjectUtility.ShouldIncludeChildren.Cancel)
          {
            property.intValue = meshAreaFromName;
            NavMeshEditorWindow.SetNavMeshArea(meshAreaFromName, shouldIncludeChildren == GameObjectUtility.ShouldIncludeChildren.IncludeChildren);
          }
        }
      }
      serializedObject.ApplyModifiedProperties();
    }

    private void SceneBakeSettings()
    {
      NavMeshEditorWindow.ComponentBasedWorkflowButton();
      if (this.m_SettingsObject == null || this.m_SettingsObject.targetObject == (UnityEngine.Object) null)
        this.InitSceneBakeSettings();
      this.m_SettingsObject.Update();
      EditorGUILayout.LabelField(NavMeshEditorWindow.s_Styles.m_AgentSizeHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      NavMeshEditorHelpers.DrawAgentDiagram(EditorGUILayout.GetControlRect(false, 120f, new GUILayoutOption[0]), this.m_AgentRadius.floatValue, this.m_AgentHeight.floatValue, this.m_AgentClimb.floatValue, this.m_AgentSlope.floatValue);
      float num1 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentRadiusContent, this.m_AgentRadius.floatValue, new GUILayoutOption[0]);
      if ((double) num1 >= 1.0 / 1000.0 && !Mathf.Approximately(num1 - this.m_AgentRadius.floatValue, 0.0f))
      {
        this.m_AgentRadius.floatValue = num1;
        if (!this.m_ManualCellSize.boolValue)
          this.m_CellSize.floatValue = (float) (2.0 * (double) this.m_AgentRadius.floatValue / 6.0);
      }
      if ((double) this.m_AgentRadius.floatValue < 0.0500000007450581 && !this.m_ManualCellSize.boolValue)
        EditorGUILayout.HelpBox("The agent radius you've set is really small, this can slow down the build.\nIf you intended to allow the agent to move close to the borders and walls, please adjust voxel size in advaced settings to ensure correct bake.", MessageType.Warning);
      float num2 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentHeightContent, this.m_AgentHeight.floatValue, new GUILayoutOption[0]);
      if ((double) num2 >= 1.0 / 1000.0 && !Mathf.Approximately(num2 - this.m_AgentHeight.floatValue, 0.0f))
        this.m_AgentHeight.floatValue = num2;
      EditorGUILayout.Slider(this.m_AgentSlope, 0.0f, 60f, NavMeshEditorWindow.s_Styles.m_AgentSlopeContent, new GUILayoutOption[0]);
      if ((double) this.m_AgentSlope.floatValue > 60.0)
        EditorGUILayout.HelpBox("The maximum slope should be set to less than " + (object) 60f + " degrees to prevent NavMesh build artifacts on slopes. ", MessageType.Warning);
      float num3 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentClimbContent, this.m_AgentClimb.floatValue, new GUILayoutOption[0]);
      if ((double) num3 >= 0.0 && !Mathf.Approximately(this.m_AgentClimb.floatValue - num3, 0.0f))
        this.m_AgentClimb.floatValue = num3;
      if ((double) this.m_AgentClimb.floatValue > (double) this.m_AgentHeight.floatValue)
        EditorGUILayout.HelpBox("Step height should be less than agent height.\nClamping step height to " + (object) this.m_AgentHeight.floatValue + " internally when baking.", MessageType.Warning);
      float floatValue = this.m_CellSize.floatValue;
      float num4 = floatValue * 0.5f;
      int num5 = (int) Mathf.Ceil(this.m_AgentClimb.floatValue / num4);
      int num6 = (int) Mathf.Ceil(Mathf.Tan((float) ((double) this.m_AgentSlope.floatValue / 180.0 * 3.14159274101257)) * floatValue * 2f / num4);
      if (num6 > num5)
        EditorGUILayout.HelpBox("Step Height conflicts with Max Slope. This makes some slopes unwalkable.\nConsider decreasing Max Slope to < " + ((float) ((double) Mathf.Atan((float) ((double) num5 * (double) num4 / ((double) floatValue * 2.0))) / 3.14159274101257 * 180.0)).ToString("0.0") + " degrees.\nOr, increase Step Height to > " + ((float) (num6 - 1) * num4).ToString("0.00") + ".", MessageType.Warning);
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(NavMeshEditorWindow.s_Styles.m_OffmeshHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      float num7 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentDropContent, this.m_LedgeDropHeight.floatValue, new GUILayoutOption[0]);
      if ((double) num7 >= 0.0 && !Mathf.Approximately(num7 - this.m_LedgeDropHeight.floatValue, 0.0f))
        this.m_LedgeDropHeight.floatValue = num7;
      float num8 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentJumpContent, this.m_MaxJumpAcrossDistance.floatValue, new GUILayoutOption[0]);
      if ((double) num8 >= 0.0 && !Mathf.Approximately(num8 - this.m_MaxJumpAcrossDistance.floatValue, 0.0f))
        this.m_MaxJumpAcrossDistance.floatValue = num8;
      EditorGUILayout.Space();
      this.m_Advanced = GUILayout.Toggle(this.m_Advanced, NavMeshEditorWindow.s_Styles.m_AdvancedHeader, EditorStyles.foldout, new GUILayoutOption[0]);
      if (this.m_Advanced)
      {
        ++EditorGUI.indentLevel;
        bool flag1 = EditorGUILayout.Toggle(NavMeshEditorWindow.s_Styles.m_ManualCellSizeContent, this.m_ManualCellSize.boolValue, new GUILayoutOption[0]);
        if (flag1 != this.m_ManualCellSize.boolValue)
        {
          this.m_ManualCellSize.boolValue = flag1;
          if (!flag1)
            this.m_CellSize.floatValue = (float) (2.0 * (double) this.m_AgentRadius.floatValue / 6.0);
        }
        ++EditorGUI.indentLevel;
        using (new EditorGUI.DisabledScope(!this.m_ManualCellSize.boolValue))
        {
          float val2 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_CellSizeContent, this.m_CellSize.floatValue, new GUILayoutOption[0]);
          if ((double) val2 > 0.0 && !Mathf.Approximately(val2 - this.m_CellSize.floatValue, 0.0f))
            this.m_CellSize.floatValue = Math.Max(0.01f, val2);
          if ((double) val2 < 0.00999999977648258)
            EditorGUILayout.HelpBox("The voxel size should be larger than 0.01.", MessageType.Warning);
          float num9 = (double) this.m_CellSize.floatValue <= 0.0 ? 0.0f : this.m_AgentRadius.floatValue / this.m_CellSize.floatValue;
          EditorGUILayout.LabelField(" ", num9.ToString("0.00") + " voxels per agent radius", EditorStyles.miniLabel, new GUILayoutOption[0]);
          if (this.m_ManualCellSize.boolValue)
          {
            if ((int) Mathf.Floor(this.m_AgentHeight.floatValue / (this.m_CellSize.floatValue * 0.5f)) > 250)
              EditorGUILayout.HelpBox("The number of voxels per agent height is too high. This will reduce the accuracy of the navmesh. Consider using voxel size of at least " + ((float) ((double) this.m_AgentHeight.floatValue / 250.0 / 0.5)).ToString("0.000") + ".", MessageType.Warning);
            if ((double) num9 < 1.0)
              EditorGUILayout.HelpBox("The number of voxels per agent radius is too small. The agent may not avoid walls and ledges properly. Consider using a voxel size less than " + (this.m_AgentRadius.floatValue / 2f).ToString("0.000") + " (2 voxels per agent radius).", MessageType.Warning);
            else if ((double) num9 > 8.0)
              EditorGUILayout.HelpBox("The number of voxels per agent radius is too high. It can cause excessive build times. Consider using voxel size closer to " + (this.m_AgentRadius.floatValue / 8f).ToString("0.000") + " (8 voxels per radius).", MessageType.Warning);
          }
          if (this.m_ManualCellSize.boolValue)
            EditorGUILayout.HelpBox("Voxel size controls how accurately the navigation mesh is generated from the level geometry. A good voxel size is 2-4 voxels per agent radius. Making voxel size smaller will increase build time.", MessageType.None);
        }
        --EditorGUI.indentLevel;
        EditorGUILayout.Space();
        float num10 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_MinRegionAreaContent, this.m_MinRegionArea.floatValue, new GUILayoutOption[0]);
        if ((double) num10 >= 0.0 && (double) num10 != (double) this.m_MinRegionArea.floatValue)
          this.m_MinRegionArea.floatValue = num10;
        EditorGUILayout.Space();
        bool flag2 = EditorGUILayout.Toggle(NavMeshEditorWindow.s_Styles.m_AgentPlacementContent, this.m_AccuratePlacement.boolValue, new GUILayoutOption[0]);
        if (flag2 != this.m_AccuratePlacement.boolValue)
          this.m_AccuratePlacement.boolValue = flag2;
        --EditorGUI.indentLevel;
      }
      this.m_SettingsObject.ApplyModifiedProperties();
      NavMeshEditorWindow.BakeButtons();
    }

    private void AreaSettings()
    {
      if (this.m_Areas == null)
        this.InitAreas();
      this.m_NavMeshProjectSettingsObject.Update();
      this.m_AreasList.DoLayoutList();
      this.m_NavMeshProjectSettingsObject.ApplyModifiedProperties();
    }

    private void AgentSettings()
    {
      if (this.m_Agents == null)
        this.InitAgents();
      this.m_NavMeshProjectSettingsObject.Update();
      if (this.m_AgentsList.index < 0)
        this.m_AgentsList.index = 0;
      this.m_AgentsList.DoLayoutList();
      if (this.m_AgentsList.index >= 0 && this.m_AgentsList.index < this.m_Agents.arraySize)
      {
        SerializedProperty arrayElementAtIndex1 = this.m_SettingNames.GetArrayElementAtIndex(this.m_AgentsList.index);
        SerializedProperty arrayElementAtIndex2 = this.m_Agents.GetArrayElementAtIndex(this.m_AgentsList.index);
        SerializedProperty propertyRelative1 = arrayElementAtIndex2.FindPropertyRelative("agentRadius");
        SerializedProperty propertyRelative2 = arrayElementAtIndex2.FindPropertyRelative("agentHeight");
        SerializedProperty propertyRelative3 = arrayElementAtIndex2.FindPropertyRelative("agentClimb");
        SerializedProperty propertyRelative4 = arrayElementAtIndex2.FindPropertyRelative("agentSlope");
        NavMeshEditorHelpers.DrawAgentDiagram(EditorGUILayout.GetControlRect(false, 120f, new GUILayoutOption[0]), propertyRelative1.floatValue, propertyRelative2.floatValue, propertyRelative3.floatValue, propertyRelative4.floatValue);
        EditorGUILayout.PropertyField(arrayElementAtIndex1, EditorGUIUtility.TempContent("Name"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(propertyRelative1, EditorGUIUtility.TempContent("Radius"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(propertyRelative2, EditorGUIUtility.TempContent("Height"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(propertyRelative3, EditorGUIUtility.TempContent("Step Height"), new GUILayoutOption[0]);
        EditorGUILayout.Slider(propertyRelative4, 0.0f, 60f, EditorGUIUtility.TempContent("Max Slope"), new GUILayoutOption[0]);
      }
      EditorGUILayout.Space();
      this.m_NavMeshProjectSettingsObject.ApplyModifiedProperties();
    }

    private static void BakeButtons()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool enabled1 = GUI.enabled;
      GUI.enabled &= !Application.isPlaying;
      if (GUILayout.Button("Clear", new GUILayoutOption[1]{ GUILayout.Width(95f) }))
        UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
      GUI.enabled = enabled1;
      if (UnityEditor.AI.NavMeshBuilder.isRunning)
      {
        if (GUILayout.Button("Cancel", new GUILayoutOption[1]{ GUILayout.Width(95f) }))
          UnityEditor.AI.NavMeshBuilder.Cancel();
      }
      else
      {
        bool enabled2 = GUI.enabled;
        GUI.enabled &= !Application.isPlaying;
        if (GUILayout.Button("Bake", new GUILayoutOption[1]{ GUILayout.Width(95f) }))
          UnityEditor.AI.NavMeshBuilder.BuildNavMeshAsync();
        GUI.enabled = enabled2;
      }
      GUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }

    private enum Mode
    {
      AgentSettings,
      AreaSettings,
      SceneBakeSettings,
      ObjectSettings,
    }

    private class Styles
    {
      public readonly GUIContent m_AgentRadiusContent = EditorGUIUtility.TextContent("Agent Radius|How close to the walls navigation mesh exist.");
      public readonly GUIContent m_AgentHeightContent = EditorGUIUtility.TextContent("Agent Height|How much vertical clearance space must exist.");
      public readonly GUIContent m_AgentSlopeContent = EditorGUIUtility.TextContent("Max Slope|Maximum slope the agent can walk up.");
      public readonly GUIContent m_AgentDropContent = EditorGUIUtility.TextContent("Drop Height|Maximum agent drop height.");
      public readonly GUIContent m_AgentClimbContent = EditorGUIUtility.TextContent("Step Height|The height of discontinuities in the level the agent can climb over (i.e. steps and stairs).");
      public readonly GUIContent m_AgentJumpContent = EditorGUIUtility.TextContent("Jump Distance|Maximum agent jump distance.");
      public readonly GUIContent m_AgentPlacementContent = EditorGUIUtility.TextContent("Height Mesh|Generate an accurate height mesh for precise agent placement (slower).");
      public readonly GUIContent m_MinRegionAreaContent = EditorGUIUtility.TextContent("Min Region Area|Minimum area that a navmesh region can be.");
      public readonly GUIContent m_ManualCellSizeContent = EditorGUIUtility.TextContent("Manual Voxel Size|Enable to set voxel size manually.");
      public readonly GUIContent m_CellSizeContent = EditorGUIUtility.TextContent("Voxel Size|Specifies at the voxelization resolution at which the NavMesh is build.");
      public readonly GUIContent m_LearnAboutComponent = EditorGUIUtility.TextContent("Learn instead about the component workflow.|Components available for building and using navmesh data for different agent types.");
      public readonly GUIContent m_AgentSizeHeader = new GUIContent("Baked Agent Size");
      public readonly GUIContent m_OffmeshHeader = new GUIContent("Generated Off Mesh Links");
      public readonly GUIContent m_AdvancedHeader = new GUIContent("Advanced");
      public readonly GUIContent m_AgentTypesHeader = new GUIContent("Agent Types");
      public readonly GUIContent m_NameLabel = new GUIContent("Name");
      public readonly GUIContent m_CostLabel = new GUIContent("Cost");
      public readonly GUIContent[] m_ModeToggles = new GUIContent[4]{ EditorGUIUtility.TextContent("Agents|Navmesh agent settings."), EditorGUIUtility.TextContent("Areas|Navmesh area settings."), EditorGUIUtility.TextContent("Bake|Navmesh bake settings."), EditorGUIUtility.TextContent("Object|Bake settings for the currently selected object.") };
    }
  }
}
