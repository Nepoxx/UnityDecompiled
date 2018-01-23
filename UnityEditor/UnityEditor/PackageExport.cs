// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageExport
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class PackageExport : EditorWindow
  {
    [SerializeField]
    private bool m_IncludeDependencies = true;
    [NonSerialized]
    private bool m_DidScheduleUpdate = false;
    [SerializeField]
    private ExportPackageItem[] m_ExportPackageItems;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    [NonSerialized]
    private PackageExportTreeView m_Tree;

    public PackageExport()
    {
      this.position = new Rect(100f, 100f, 400f, 300f);
      this.minSize = new Vector2(350f, 350f);
    }

    public ExportPackageItem[] items
    {
      get
      {
        return this.m_ExportPackageItems;
      }
    }

    internal static void ShowExportPackage()
    {
      EditorWindow.GetWindow<PackageExport>(true, "Exporting package");
    }

    internal static IEnumerable<ExportPackageItem> GetAssetItemsForExport(ICollection<string> guids, bool includeDependencies)
    {
      if (guids.Count == 0)
        guids = (ICollection<string>) new HashSet<string>((IEnumerable<string>) AssetDatabase.CollectAllChildren(AssetDatabase.assetFolderGUID, new string[0]));
      ExportPackageItem[] exportPackageItemArray = PackageUtility.BuildExportPackageItemsList(guids.ToArray<string>(), includeDependencies);
      if (includeDependencies && ((IEnumerable<ExportPackageItem>) exportPackageItemArray).Any<ExportPackageItem>((Func<ExportPackageItem, bool>) (asset => InternalEditorUtility.IsScriptOrAssembly(asset.assetPath))))
        exportPackageItemArray = PackageUtility.BuildExportPackageItemsList(guids.Union<string>(InternalEditorUtility.GetAllScriptGUIDs()).ToArray<string>(), includeDependencies);
      return (IEnumerable<ExportPackageItem>) ((IEnumerable<ExportPackageItem>) exportPackageItemArray).Where<ExportPackageItem>((Func<ExportPackageItem, bool>) (val => val.assetPath != "Assets")).ToArray<ExportPackageItem>();
    }

    private void RefreshAssetList()
    {
      this.m_ExportPackageItems = (ExportPackageItem[]) null;
    }

    private bool HasValidAssetList()
    {
      return this.m_ExportPackageItems != null;
    }

    private bool CheckAssetExportList()
    {
      if (this.m_ExportPackageItems.Length != 0)
        return false;
      GUILayout.Space(20f);
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label("Nothing to export!", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.Label("No assets to export were found in your project.", (GUIStyle) "WordWrappedLabel", new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("OK"))
      {
        this.Close();
        GUIUtility.ExitGUI();
      }
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      return true;
    }

    public void OnDestroy()
    {
      this.UnscheduleBuildAssetList();
    }

    public void OnGUI()
    {
      if (!this.HasValidAssetList())
        this.ScheduleBuildAssetList();
      else if (this.CheckAssetExportList())
        return;
      using (new EditorGUI.DisabledScope(!this.HasValidAssetList()))
        this.TopArea();
      this.TreeViewArea(!this.HasValidAssetList());
      using (new EditorGUI.DisabledScope(!this.HasValidAssetList()))
        this.BottomArea();
    }

    private void TopArea()
    {
      Rect rect = GUILayoutUtility.GetRect(this.position.width, 53f);
      GUI.Label(rect, GUIContent.none, PackageExport.Styles.topBarBg);
      GUI.Label(new Rect(rect.x + 5f, rect.yMin, rect.width, rect.height), PackageExport.Styles.header, PackageExport.Styles.title);
    }

    private void BottomArea()
    {
      GUILayout.BeginVertical(PackageExport.Styles.bottomBarBg, new GUILayoutOption[0]);
      GUILayout.Space(8f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      if (GUILayout.Button(PackageExport.Styles.allText, new GUILayoutOption[1]{ GUILayout.Width(50f) }))
        this.m_Tree.SetAllEnabled(PackageExportTreeView.EnabledState.All);
      if (GUILayout.Button(PackageExport.Styles.noneText, new GUILayoutOption[1]{ GUILayout.Width(50f) }))
        this.m_Tree.SetAllEnabled(PackageExportTreeView.EnabledState.None);
      GUILayout.Space(10f);
      EditorGUI.BeginChangeCheck();
      this.m_IncludeDependencies = GUILayout.Toggle(this.m_IncludeDependencies, PackageExport.Styles.includeDependenciesText);
      if (EditorGUI.EndChangeCheck())
        this.RefreshAssetList();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(EditorGUIUtility.TextContent("Export...")))
      {
        this.Export();
        GUIUtility.ExitGUI();
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.EndVertical();
    }

    private void TreeViewArea(bool showLoadingScreen)
    {
      Rect rect = GUILayoutUtility.GetRect(1f, 9999f, 1f, 99999f);
      if (showLoadingScreen)
      {
        GUI.Label(rect, "Loading...", PackageExport.Styles.loadingTextStyle);
      }
      else
      {
        if (this.m_ExportPackageItems == null || this.m_ExportPackageItems.Length <= 0)
          return;
        if (this.m_TreeViewState == null)
          this.m_TreeViewState = new TreeViewState();
        if (this.m_Tree == null)
          this.m_Tree = new PackageExportTreeView(this, this.m_TreeViewState, new Rect());
        this.m_Tree.OnGUI(rect);
      }
    }

    private void Export()
    {
      string fileName = EditorUtility.SaveFilePanel("Export package ...", "", "", "unitypackage");
      if (!(fileName != ""))
        return;
      List<string> stringList = new List<string>();
      foreach (ExportPackageItem exportPackageItem in this.m_ExportPackageItems)
      {
        if (exportPackageItem.enabledStatus > 0)
          stringList.Add(exportPackageItem.guid);
      }
      PackageUtility.ExportPackage(stringList.ToArray(), fileName);
      this.Close();
      GUIUtility.ExitGUI();
    }

    private void ScheduleBuildAssetList()
    {
      if (this.m_DidScheduleUpdate)
        return;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.BuildAssetList);
      this.m_DidScheduleUpdate = true;
    }

    private void UnscheduleBuildAssetList()
    {
      if (!this.m_DidScheduleUpdate)
        return;
      this.m_DidScheduleUpdate = false;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.BuildAssetList);
    }

    private void BuildAssetList()
    {
      this.UnscheduleBuildAssetList();
      this.m_ExportPackageItems = PackageExport.GetAssetItemsForExport((ICollection<string>) Selection.assetGUIDsDeepSelection, this.m_IncludeDependencies).ToArray<ExportPackageItem>();
      this.m_Tree = (PackageExportTreeView) null;
      this.m_TreeViewState = (TreeViewState) null;
      this.Repaint();
    }

    internal static class Styles
    {
      public static GUIStyle title = new GUIStyle(EditorStyles.largeLabel);
      public static GUIStyle bottomBarBg = (GUIStyle) "ProjectBrowserBottomBarBg";
      public static GUIStyle topBarBg = new GUIStyle((GUIStyle) "ProjectBrowserHeaderBgTop");
      public static GUIStyle loadingTextStyle = new GUIStyle(EditorStyles.label);
      public static GUIContent allText = EditorGUIUtility.TextContent("All");
      public static GUIContent noneText = EditorGUIUtility.TextContent("None");
      public static GUIContent includeDependenciesText = EditorGUIUtility.TextContent("Include dependencies");
      public static GUIContent header = new GUIContent("Items to Export");

      static Styles()
      {
        PackageExport.Styles.topBarBg.fixedHeight = 0.0f;
        RectOffset border = PackageExport.Styles.topBarBg.border;
        int num1 = 2;
        PackageExport.Styles.topBarBg.border.bottom = num1;
        int num2 = num1;
        border.top = num2;
        PackageExport.Styles.title.fontStyle = FontStyle.Bold;
        PackageExport.Styles.title.alignment = TextAnchor.MiddleLeft;
        PackageExport.Styles.loadingTextStyle.alignment = TextAnchor.MiddleCenter;
      }
    }
  }
}
