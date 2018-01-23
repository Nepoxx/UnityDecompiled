// Decompiled with JetBrains decompiler
// Type: UnityEditor.SoftlockViewController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Collaboration;
using UnityEngine;

namespace UnityEditor
{
  internal class SoftlockViewController
  {
    public GUIStyle k_Style = (GUIStyle) null;
    public GUIStyle k_StyleEmpty = new GUIStyle();
    public GUIContent k_Content = (GUIContent) null;
    private SoftlockViewController.Cache m_Cache = (SoftlockViewController.Cache) null;
    [SerializeField]
    private SoftLockFilters m_SoftLockFilters = new SoftLockFilters();
    private static SoftlockViewController s_Instance;
    private const string k_TooltipHeader = "Unpublished changes by:";
    private const string k_TooltipPrefabHeader = "Unpublished Prefab changes by:";
    private const string k_TooltipNamePrefix = " \n •  ";

    private SoftlockViewController()
    {
    }

    ~SoftlockViewController()
    {
    }

    public SoftLockFilters softLockFilters
    {
      get
      {
        return this.m_SoftLockFilters;
      }
    }

    public static SoftlockViewController Instance
    {
      get
      {
        if (SoftlockViewController.s_Instance == null)
        {
          SoftlockViewController.s_Instance = new SoftlockViewController();
          SoftlockViewController.s_Instance.m_Cache = new SoftlockViewController.Cache();
        }
        return SoftlockViewController.s_Instance;
      }
    }

    public void TurnOn()
    {
      this.RegisterDataDelegate();
      this.RegisterDrawDelegates();
      this.Repaint();
    }

    public void TurnOff()
    {
      this.UnregisterDataDelegate();
      this.UnregisterDrawDelegates();
    }

    private void UnregisterDataDelegate()
    {
      SoftLockData.SoftlockSubscriber -= new SoftLockData.OnSoftlockUpdate(SoftlockViewController.Instance.OnSoftlockUpdate);
    }

    private void RegisterDataDelegate()
    {
      this.UnregisterDataDelegate();
      SoftLockData.SoftlockSubscriber += new SoftLockData.OnSoftlockUpdate(SoftlockViewController.Instance.OnSoftlockUpdate);
    }

    private void UnregisterDrawDelegates()
    {
      ObjectListArea.postAssetIconDrawCallback -= new ObjectListArea.OnAssetIconDrawDelegate(SoftlockViewController.Instance.DrawProjectBrowserGridUI);
      ObjectListArea.postAssetLabelDrawCallback -= new ObjectListArea.OnAssetLabelDrawDelegate(SoftlockViewController.Instance.DrawProjectBrowserListUI);
      Editor.OnPostIconGUI -= new Editor.OnEditorGUIDelegate(SoftlockViewController.Instance.DrawInspectorUI);
      GameObjectTreeViewGUI.OnPostHeaderGUI -= new GameObjectTreeViewGUI.OnHeaderGUIDelegate(SoftlockViewController.Instance.DrawSceneUI);
    }

    private void RegisterDrawDelegates()
    {
      this.UnregisterDrawDelegates();
      ObjectListArea.postAssetIconDrawCallback += new ObjectListArea.OnAssetIconDrawDelegate(SoftlockViewController.Instance.DrawProjectBrowserGridUI);
      ObjectListArea.postAssetLabelDrawCallback += new ObjectListArea.OnAssetLabelDrawDelegate(SoftlockViewController.Instance.DrawProjectBrowserListUI);
      AssetsTreeViewGUI.postAssetLabelDrawCallback += new AssetsTreeViewGUI.OnAssetLabelDrawDelegate(SoftlockViewController.Instance.DrawSingleColumnProjectBrowserUI);
      Editor.OnPostIconGUI += new Editor.OnEditorGUIDelegate(SoftlockViewController.Instance.DrawInspectorUI);
      GameObjectTreeViewGUI.OnPostHeaderGUI += new GameObjectTreeViewGUI.OnHeaderGUIDelegate(SoftlockViewController.Instance.DrawSceneUI);
    }

    private bool HasSoftlockSupport(Editor editor)
    {
      if (!Collab.instance.IsCollabEnabledForCurrentProject() || (UnityEngine.Object) editor == (UnityEngine.Object) null || editor.targets.Length > 1 || (editor.target == (UnityEngine.Object) null || !SoftLockData.AllowsSoftLocks(editor.target)))
        return false;
      bool flag = true;
      System.Type type = editor.GetType();
      if (type != typeof (GameObjectInspector) && type != typeof (GenericInspector))
        flag = false;
      return flag;
    }

    private bool HasSoftlocks(string assetGUID)
    {
      if (!Collab.instance.IsCollabEnabledForCurrentProject())
        return false;
      bool hasSoftLocks;
      return SoftLockData.TryHasSoftLocks(assetGUID, out hasSoftLocks) && hasSoftLocks;
    }

    public void OnSoftlockUpdate(string[] assetGUIDs)
    {
      this.m_Cache.InvalidateAssetGUIDs(assetGUIDs);
      this.Repaint();
    }

    public void Repaint()
    {
      this.RepaintInspectors();
      this.RepaintSceneHierarchy();
      this.RepaintProjectBrowsers();
    }

    private void RepaintSceneHierarchy()
    {
      foreach (EditorWindow sceneHierarchyWindow in SceneHierarchyWindow.GetAllSceneHierarchyWindows())
        sceneHierarchyWindow.Repaint();
    }

    private void RepaintInspectors()
    {
      foreach (Editor editor in this.m_Cache.GetEditors())
        editor.Repaint();
    }

    private void RepaintProjectBrowsers()
    {
      foreach (ProjectBrowser allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
      {
        allProjectBrowser.RefreshSearchIfFilterContains("s:");
        allProjectBrowser.Repaint();
      }
    }

    public void DrawSceneUI(Rect availableRect, string scenePath)
    {
      string guid = AssetDatabase.AssetPathToGUID(scenePath);
      if (!this.HasSoftlocks(guid))
        return;
      int count;
      SoftLockData.TryGetSoftlockCount(guid, out count);
      GUIContent guiContent = this.GetGUIContent();
      guiContent.image = SoftLockUIData.GetIconForSection(SoftLockUIData.SectionEnum.Scene);
      guiContent.text = SoftlockViewController.GetDisplayCount(count);
      guiContent.tooltip = SoftlockViewController.Instance.GetTooltip(guid);
      Vector2 size = this.GetStyle().CalcSize(guiContent);
      Rect position = new Rect(availableRect.position, size);
      position.x = (float) ((double) availableRect.width - (double) position.width - 4.0);
      EditorGUI.LabelField(position, guiContent);
    }

    private void DrawInspectorUI(Editor editor, Rect drawRect)
    {
      if (!this.HasSoftlockSupport(editor))
        return;
      this.m_Cache.StoreEditor(editor);
      string assetGUID = (string) null;
      AssetAccess.TryGetAssetGUIDFromObject(editor.target, out assetGUID);
      if (!this.HasSoftlocks(assetGUID))
        return;
      Texture iconForSection = SoftLockUIData.GetIconForSection(SoftLockUIData.SectionEnum.ProjectBrowser);
      if (!((UnityEngine.Object) iconForSection != (UnityEngine.Object) null))
        return;
      this.DrawIconWithTooltips(drawRect, iconForSection, assetGUID);
    }

    private void DrawProjectBrowserGridUI(Rect iconRect, string assetGUID, bool isListMode)
    {
      if (isListMode || !this.HasSoftlocks(assetGUID))
        return;
      Rect zero = Rect.zero;
      Texture iconForSection = SoftLockUIData.GetIconForSection(SoftLockUIData.SectionEnum.ProjectBrowser);
      if (!((UnityEngine.Object) iconForSection != (UnityEngine.Object) null))
        return;
      this.DrawIconWithTooltips(Overlay.GetRectForBottomRight(iconRect, 0.35), iconForSection, assetGUID);
    }

    private bool DrawProjectBrowserListUI(Rect drawRect, string assetGUID, bool isListMode)
    {
      if (!isListMode || !this.HasSoftlocks(assetGUID))
        return false;
      Rect iconRect = drawRect;
      iconRect.width = drawRect.height;
      iconRect.x = (float) Math.Round((double) drawRect.center.x - (double) iconRect.width / 2.0);
      return this.DrawInProjectBrowserListMode(iconRect, assetGUID);
    }

    private bool DrawSingleColumnProjectBrowserUI(Rect drawRect, string assetGUID)
    {
      if (ProjectBrowser.s_LastInteractedProjectBrowser.IsTwoColumns() || !this.HasSoftlocks(assetGUID))
        return false;
      Rect iconRect = drawRect;
      iconRect.width = drawRect.height;
      float num = iconRect.width / 2f;
      iconRect.x = (float) Math.Round((double) drawRect.xMax - (double) iconRect.width - (double) num);
      return this.DrawInProjectBrowserListMode(iconRect, assetGUID);
    }

    private bool DrawInProjectBrowserListMode(Rect iconRect, string assetGUID)
    {
      Texture iconForSection = SoftLockUIData.GetIconForSection(SoftLockUIData.SectionEnum.ProjectBrowser);
      bool flag = false;
      if ((UnityEngine.Object) iconForSection != (UnityEngine.Object) null)
      {
        this.DrawIconWithTooltips(iconRect, iconForSection, assetGUID);
        flag = true;
      }
      return flag;
    }

    private void DrawIconWithTooltips(Rect iconRect, Texture icon, string assetGUID)
    {
      GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
      this.DrawTooltip(iconRect, this.GetTooltip(assetGUID));
    }

    private void DrawTooltip(Rect frame, string tooltip)
    {
      GUIContent guiContent = this.GetGUIContent();
      guiContent.tooltip = tooltip;
      GUI.Label(frame, guiContent, this.k_StyleEmpty);
    }

    private string GetTooltip(string assetGUID)
    {
      string tooltipText;
      if (!this.m_Cache.TryGetTooltipForGUID(assetGUID, out tooltipText))
      {
        List<string> locksNamesOnAsset = SoftLockUIData.GetLocksNamesOnAsset(assetGUID);
        tooltipText = !SoftLockData.IsPrefab(assetGUID) ? "Unpublished changes by:" : "Unpublished Prefab changes by:";
        foreach (string str in locksNamesOnAsset)
          tooltipText = tooltipText + " \n •  " + str + " ";
        this.m_Cache.StoreTooltipForGUID(assetGUID, tooltipText);
      }
      return tooltipText;
    }

    private static string GetDisplayCount(int count)
    {
      string displayText;
      if (!SoftlockViewController.Instance.m_Cache.TryGetDisplayCount(count, out displayText))
      {
        displayText = count.ToString();
        SoftlockViewController.Instance.m_Cache.StoreDisplayCount(count, displayText);
      }
      return displayText;
    }

    private string FitTextToWidth(string text, float width, GUIStyle style)
    {
      int thatFitWithinWidth = style.GetNumCharactersThatFitWithinWidth(text, width);
      if (thatFitWithinWidth <= 1 || thatFitWithinWidth == text.Length)
        return text;
      int num = thatFitWithinWidth - 1;
      string ellipsedNames;
      if (!SoftlockViewController.Instance.m_Cache.TryGetEllipsedNames(text, num, out ellipsedNames))
      {
        ellipsedNames = text.Substring(0, num) + " …";
        SoftlockViewController.Instance.m_Cache.StoreEllipsedNames(text, ellipsedNames, num);
      }
      return ellipsedNames;
    }

    public GUIContent GetGUIContent()
    {
      if (this.k_Content == null)
        this.k_Content = new GUIContent();
      this.k_Content.tooltip = string.Empty;
      this.k_Content.text = (string) null;
      this.k_Content.image = (Texture) null;
      return this.k_Content;
    }

    public GUIStyle GetStyle()
    {
      if (this.k_Style == null)
      {
        this.k_Style = new GUIStyle(EditorStyles.label);
        this.k_Style.normal.background = (Texture2D) null;
      }
      return this.k_Style;
    }

    private class Cache
    {
      private static Dictionary<int, string> s_CachedStringCount = new Dictionary<int, string>();
      private List<WeakReference> m_EditorReferences = new List<WeakReference>();
      private List<WeakReference> m_CachedWeakReferences = new List<WeakReference>();
      private Dictionary<string, string> m_AssetGUIDToTooltip = new Dictionary<string, string>();
      private Dictionary<string, Dictionary<int, string>> m_NamesListToEllipsedNames = new Dictionary<string, Dictionary<int, string>>();

      public void InvalidateAssetGUIDs(string[] assetGUIDs)
      {
        for (int index = 0; index < assetGUIDs.Length; ++index)
          this.m_AssetGUIDToTooltip.Remove(assetGUIDs[index]);
      }

      public bool TryGetEllipsedNames(string allNames, int characterLength, out string ellipsedNames)
      {
        Dictionary<int, string> dictionary;
        if (this.m_NamesListToEllipsedNames.TryGetValue(allNames, out dictionary))
          return dictionary.TryGetValue(characterLength, out ellipsedNames);
        ellipsedNames = "";
        return false;
      }

      public void StoreEllipsedNames(string allNames, string ellipsedNames, int characterLength)
      {
        Dictionary<int, string> dictionary;
        if (!this.m_NamesListToEllipsedNames.TryGetValue(allNames, out dictionary))
          dictionary = new Dictionary<int, string>();
        dictionary[characterLength] = ellipsedNames;
        this.m_NamesListToEllipsedNames[allNames] = dictionary;
      }

      public bool TryGetTooltipForGUID(string assetGUID, out string tooltipText)
      {
        return this.m_AssetGUIDToTooltip.TryGetValue(assetGUID, out tooltipText);
      }

      public void StoreTooltipForGUID(string assetGUID, string tooltipText)
      {
        this.m_AssetGUIDToTooltip[assetGUID] = tooltipText;
      }

      public bool TryGetDisplayCount(int count, out string displayText)
      {
        return SoftlockViewController.Cache.s_CachedStringCount.TryGetValue(count, out displayText);
      }

      public void StoreDisplayCount(int count, string displayText)
      {
        SoftlockViewController.Cache.s_CachedStringCount.Add(count, displayText);
      }

      public List<Editor> GetEditors()
      {
        List<Editor> editorList = new List<Editor>();
        for (int index = 0; index < this.m_EditorReferences.Count; ++index)
        {
          WeakReference editorReference = this.m_EditorReferences[index];
          Editor target = editorReference.Target as Editor;
          if ((UnityEngine.Object) target == (UnityEngine.Object) null)
          {
            this.m_EditorReferences.RemoveAt(index);
            this.m_CachedWeakReferences.Add(editorReference);
            --index;
          }
          else
            editorList.Add(target);
        }
        return editorList;
      }

      public void StoreEditor(Editor editor)
      {
        bool flag = true;
        for (int index = 0; flag && index < this.m_EditorReferences.Count; ++index)
        {
          WeakReference editorReference = this.m_EditorReferences[index];
          Editor target = editorReference.Target as Editor;
          if ((UnityEngine.Object) target == (UnityEngine.Object) null)
          {
            this.m_EditorReferences.RemoveAt(index);
            this.m_CachedWeakReferences.Add(editorReference);
            --index;
          }
          else if ((UnityEngine.Object) target == (UnityEngine.Object) editor)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          return;
        WeakReference weakReference;
        if (this.m_CachedWeakReferences.Count > 0)
        {
          weakReference = this.m_CachedWeakReferences[0];
          this.m_CachedWeakReferences.RemoveAt(0);
        }
        else
          weakReference = new WeakReference((object) null);
        weakReference.Target = (object) editor;
        this.m_EditorReferences.Add(weakReference);
      }
    }
  }
}
