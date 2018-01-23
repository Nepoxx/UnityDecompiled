// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingExplorerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Lighting", title = "Light Explorer")]
  internal class LightingExplorerWindow : EditorWindow
  {
    private float m_ToolbarPadding = -1f;
    private LightingExplorerWindow.TabType m_SelectedTab = LightingExplorerWindow.TabType.Lights;
    private List<LightingExplorerWindowTab> m_TableTabs;

    [MenuItem("Window/Lighting/Light Explorer", false, 2099)]
    private static void CreateLightingExplorerWindow()
    {
      LightingExplorerWindow window = EditorWindow.GetWindow<LightingExplorerWindow>();
      window.minSize = new Vector2(500f, 250f);
      window.Show();
    }

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
      if (this.m_TableTabs == null || this.m_TableTabs.Count != 4)
      {
        List<LightingExplorerWindowTab> explorerWindowTabList1 = new List<LightingExplorerWindowTab>();
        List<LightingExplorerWindowTab> explorerWindowTabList2 = explorerWindowTabList1;
        string serializationUID1 = "LightTable";
        SerializedPropertyDataStore.GatherDelegate gatherDelegate1 = (SerializedPropertyDataStore.GatherDelegate) (() => (UnityEngine.Object[]) UnityEngine.Object.FindObjectsOfType<Light>());
        // ISSUE: reference to a compiler-generated field
        if (LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache0 = new SerializedPropertyTable.HeaderDelegate(LightTableColumns.CreateLightColumns);
        }
        // ISSUE: reference to a compiler-generated field
        SerializedPropertyTable.HeaderDelegate fMgCache0 = LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache0;
        LightingExplorerWindowTab explorerWindowTab1 = new LightingExplorerWindowTab(new SerializedPropertyTable(serializationUID1, gatherDelegate1, fMgCache0));
        explorerWindowTabList2.Add(explorerWindowTab1);
        List<LightingExplorerWindowTab> explorerWindowTabList3 = explorerWindowTabList1;
        string serializationUID2 = "ReflectionTable";
        SerializedPropertyDataStore.GatherDelegate gatherDelegate2 = (SerializedPropertyDataStore.GatherDelegate) (() => (UnityEngine.Object[]) UnityEngine.Object.FindObjectsOfType<UnityEngine.ReflectionProbe>());
        // ISSUE: reference to a compiler-generated field
        if (LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache1 = new SerializedPropertyTable.HeaderDelegate(LightTableColumns.CreateReflectionColumns);
        }
        // ISSUE: reference to a compiler-generated field
        SerializedPropertyTable.HeaderDelegate fMgCache1 = LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache1;
        LightingExplorerWindowTab explorerWindowTab2 = new LightingExplorerWindowTab(new SerializedPropertyTable(serializationUID2, gatherDelegate2, fMgCache1));
        explorerWindowTabList3.Add(explorerWindowTab2);
        List<LightingExplorerWindowTab> explorerWindowTabList4 = explorerWindowTabList1;
        string serializationUID3 = "LightProbeTable";
        SerializedPropertyDataStore.GatherDelegate gatherDelegate3 = (SerializedPropertyDataStore.GatherDelegate) (() => (UnityEngine.Object[]) UnityEngine.Object.FindObjectsOfType<LightProbeGroup>());
        // ISSUE: reference to a compiler-generated field
        if (LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache2 = new SerializedPropertyTable.HeaderDelegate(LightTableColumns.CreateLightProbeColumns);
        }
        // ISSUE: reference to a compiler-generated field
        SerializedPropertyTable.HeaderDelegate fMgCache2 = LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache2;
        LightingExplorerWindowTab explorerWindowTab3 = new LightingExplorerWindowTab(new SerializedPropertyTable(serializationUID3, gatherDelegate3, fMgCache2));
        explorerWindowTabList4.Add(explorerWindowTab3);
        List<LightingExplorerWindowTab> explorerWindowTabList5 = explorerWindowTabList1;
        string serializationUID4 = "EmissiveMaterialTable";
        SerializedPropertyDataStore.GatherDelegate gatherDelegate4 = this.StaticEmissivesGatherDelegate();
        // ISSUE: reference to a compiler-generated field
        if (LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache3 = new SerializedPropertyTable.HeaderDelegate(LightTableColumns.CreateEmissivesColumns);
        }
        // ISSUE: reference to a compiler-generated field
        SerializedPropertyTable.HeaderDelegate fMgCache3 = LightingExplorerWindow.\u003C\u003Ef__mg\u0024cache3;
        LightingExplorerWindowTab explorerWindowTab4 = new LightingExplorerWindowTab(new SerializedPropertyTable(serializationUID4, gatherDelegate4, fMgCache3));
        explorerWindowTabList5.Add(explorerWindowTab4);
        this.m_TableTabs = explorerWindowTabList1;
      }
      for (int index = 0; index < this.m_TableTabs.Count; ++index)
        this.m_TableTabs[index].OnEnable();
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      this.Repaint();
    }

    private void OnDisable()
    {
      if (this.m_TableTabs != null)
      {
        for (int index = 0; index < this.m_TableTabs.Count; ++index)
          this.m_TableTabs[index].OnDisable();
      }
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
    }

    private void OnInspectorUpdate()
    {
      if (this.m_TableTabs == null || this.m_SelectedTab < LightingExplorerWindow.TabType.Lights || this.m_SelectedTab >= (LightingExplorerWindow.TabType) this.m_TableTabs.Count)
        return;
      this.m_TableTabs[(int) this.m_SelectedTab].OnInspectorUpdate();
    }

    private void OnSelectionChange()
    {
      if (this.m_TableTabs != null)
      {
        for (int index = 0; index < this.m_TableTabs.Count; ++index)
        {
          if (index == this.m_TableTabs.Count - 1)
          {
            int[] array = ((IEnumerable<MeshRenderer>) UnityEngine.Object.FindObjectsOfType<MeshRenderer>()).Where<MeshRenderer>((Func<MeshRenderer, bool>) (mr => ((IEnumerable<int>) Selection.instanceIDs).Contains<int>(mr.gameObject.GetInstanceID()))).SelectMany<MeshRenderer, Material>((Func<MeshRenderer, IEnumerable<Material>>) (meshRenderer => (IEnumerable<Material>) meshRenderer.sharedMaterials)).Where<Material>((Func<Material, bool>) (m => (UnityEngine.Object) m != (UnityEngine.Object) null && (m.globalIlluminationFlags & MaterialGlobalIlluminationFlags.AnyEmissive) != MaterialGlobalIlluminationFlags.None)).Select<Material, int>((Func<Material, int>) (m => m.GetInstanceID())).Union<int>((IEnumerable<int>) Selection.instanceIDs).Distinct<int>().ToArray<int>();
            this.m_TableTabs[index].OnSelectionChange(array);
          }
          else
            this.m_TableTabs[index].OnSelectionChange();
        }
      }
      this.Repaint();
    }

    private void OnHierarchyChange()
    {
      if (this.m_TableTabs == null)
        return;
      for (int index = 0; index < this.m_TableTabs.Count; ++index)
        this.m_TableTabs[index].OnHierarchyChange();
    }

    private void OnGUI()
    {
      EditorGUIUtility.labelWidth = 130f;
      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.m_SelectedTab = (LightingExplorerWindow.TabType) GUILayout.Toolbar((int) this.m_SelectedTab, LightingExplorerWindow.Styles.TabTypes, (GUIStyle) "LargeButton", GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      if (this.m_TableTabs != null && this.m_SelectedTab >= LightingExplorerWindow.TabType.Lights && this.m_SelectedTab < (LightingExplorerWindow.TabType) this.m_TableTabs.Count)
        this.m_TableTabs[(int) this.m_SelectedTab].OnGUI();
      EditorGUILayout.Space();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }

    private SerializedPropertyDataStore.GatherDelegate StaticEmissivesGatherDelegate()
    {
      return (SerializedPropertyDataStore.GatherDelegate) (() => (UnityEngine.Object[]) ((IEnumerable<MeshRenderer>) UnityEngine.Object.FindObjectsOfType<MeshRenderer>()).Where<MeshRenderer>((Func<MeshRenderer, bool>) (mr => GameObjectUtility.AreStaticEditorFlagsSet(mr.gameObject, StaticEditorFlags.LightmapStatic))).SelectMany<MeshRenderer, Material>((Func<MeshRenderer, IEnumerable<Material>>) (meshRenderer => (IEnumerable<Material>) meshRenderer.sharedMaterials)).Where<Material>((Func<Material, bool>) (m => (UnityEngine.Object) m != (UnityEngine.Object) null && (m.globalIlluminationFlags & MaterialGlobalIlluminationFlags.AnyEmissive) != MaterialGlobalIlluminationFlags.None && m.HasProperty("_EmissionColor"))).Distinct<Material>().ToArray<Material>());
    }

    private static class Styles
    {
      public static readonly GUIContent[] TabTypes = new GUIContent[4]{ EditorGUIUtility.TextContent("Lights"), EditorGUIUtility.TextContent("Reflection Probes"), EditorGUIUtility.TextContent("Light Probes"), EditorGUIUtility.TextContent("Static Emissives") };
    }

    private enum TabType
    {
      Lights,
      Reflections,
      LightProbes,
      Emissives,
      Count,
    }
  }
}
