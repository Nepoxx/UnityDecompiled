// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class GameObjectTreeViewGUI : TreeViewGUI
  {
    internal static GameObjectTreeViewGUI.OnHeaderGUIDelegate OnPostHeaderGUI = (GameObjectTreeViewGUI.OnHeaderGUIDelegate) null;
    private float m_PrevScollPos;
    private float m_PrevTotalHeight;

    public GameObjectTreeViewGUI(TreeViewController treeView, bool useHorizontalScroll)
      : base(treeView, useHorizontalScroll)
    {
      this.k_TopRowMargin = 0.0f;
    }

    public override void OnInitialize()
    {
      this.m_PrevScollPos = this.m_TreeView.state.scrollPos.y;
      this.m_PrevTotalHeight = this.m_TreeView.GetTotalRect().height;
    }

    public bool DetectUserInput()
    {
      return this.DetectScrollChange() || this.DetectTotalRectChange() || this.DetectMouseDownInTreeViewRect();
    }

    private bool DetectScrollChange()
    {
      bool flag = false;
      float y = this.m_TreeView.state.scrollPos.y;
      if (!Mathf.Approximately(y, this.m_PrevScollPos))
        flag = true;
      this.m_PrevScollPos = y;
      return flag;
    }

    private bool DetectTotalRectChange()
    {
      bool flag = false;
      float height = this.m_TreeView.GetTotalRect().height;
      if (!Mathf.Approximately(height, this.m_PrevTotalHeight))
        flag = true;
      this.m_PrevTotalHeight = height;
      return flag;
    }

    private bool DetectMouseDownInTreeViewRect()
    {
      Event current = Event.current;
      bool flag1 = current.type == EventType.MouseDown || current.type == EventType.MouseUp;
      bool flag2 = current.type == EventType.KeyDown || current.type == EventType.KeyUp;
      return flag1 && this.m_TreeView.GetTotalRect().Contains(current.mousePosition) || flag2;
    }

    private bool showingStickyHeaders
    {
      get
      {
        return SceneManager.sceneCount > 1;
      }
    }

    private void DoStickySceneHeaders()
    {
      int firstRowVisible;
      int lastRowVisible;
      this.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      if (firstRowVisible < 0 || lastRowVisible < 0)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameObjectTreeViewGUI.\u003CDoStickySceneHeaders\u003Ec__AnonStorey0 headersCAnonStorey0 = new GameObjectTreeViewGUI.\u003CDoStickySceneHeaders\u003Ec__AnonStorey0();
      float y = this.m_TreeView.state.scrollPos.y;
      if (firstRowVisible == 0 && (double) y <= (double) this.topRowMargin)
        return;
      // ISSUE: reference to a compiler-generated field
      headersCAnonStorey0.firstItem = (GameObjectTreeViewItem) this.m_TreeView.data.GetItem(firstRowVisible);
      GameObjectTreeViewItem objectTreeViewItem = (GameObjectTreeViewItem) this.m_TreeView.data.GetItem(firstRowVisible + 1);
      // ISSUE: reference to a compiler-generated field
      bool flag = headersCAnonStorey0.firstItem.scene != objectTreeViewItem.scene;
      float width = GUIClip.visibleRect.width;
      Rect rowRect = this.GetRowRect(firstRowVisible, width);
      // ISSUE: reference to a compiler-generated field
      if (headersCAnonStorey0.firstItem.isSceneHeader && Mathf.Approximately(y, rowRect.y))
        return;
      if (!flag)
        rowRect.y = y;
      // ISSUE: reference to a compiler-generated method
      GameObjectTreeViewItem sceneHeaderItem = ((GameObjectTreeViewDataSource) this.m_TreeView.data).sceneHeaderItems.FirstOrDefault<GameObjectTreeViewItem>(new Func<GameObjectTreeViewItem, bool>(headersCAnonStorey0.\u003C\u003Em__0));
      if (sceneHeaderItem != null)
      {
        bool selected = this.m_TreeView.IsItemDragSelectedOrSelected((TreeViewItem) sceneHeaderItem);
        bool focused = this.m_TreeView.HasFocus();
        bool useBoldFont = sceneHeaderItem.scene == SceneManager.GetActiveScene();
        this.DoItemGUI(rowRect, firstRowVisible, (TreeViewItem) sceneHeaderItem, selected, focused, useBoldFont);
        if (GUI.Button(new Rect(rowRect.x, rowRect.y, rowRect.height, rowRect.height), GUIContent.none, GUIStyle.none))
          this.m_TreeView.Frame(sceneHeaderItem.id, true, false);
        this.m_TreeView.HandleUnusedMouseEventsForItem(rowRect, (TreeViewItem) sceneHeaderItem, firstRowVisible);
        this.HandleStickyHeaderContextClick(rowRect, sceneHeaderItem);
      }
    }

    private void HandleStickyHeaderContextClick(Rect rect, GameObjectTreeViewItem sceneHeaderItem)
    {
      Event current = Event.current;
      switch (Application.platform)
      {
        case RuntimePlatform.OSXEditor:
          if ((current.type != EventType.MouseDown || current.button != 1) && current.type != EventType.ContextClick || !rect.Contains(Event.current.mousePosition))
            break;
          current.Use();
          this.m_TreeView.contextClickItemCallback(sceneHeaderItem.id);
          break;
        case RuntimePlatform.WindowsEditor:
          if (current.type == EventType.MouseDown && current.button == 1 && rect.Contains(Event.current.mousePosition))
            current.Use();
          break;
      }
    }

    public override void BeginRowGUI()
    {
      if (this.DetectUserInput())
        ((GameObjectTreeViewDataSource) this.m_TreeView.data).EnsureFullyInitialized();
      base.BeginRowGUI();
      if (!this.showingStickyHeaders || Event.current.type == EventType.Repaint)
        return;
      this.DoStickySceneHeaders();
    }

    public override void EndRowGUI()
    {
      base.EndRowGUI();
      if (!this.showingStickyHeaders || Event.current.type != EventType.Repaint)
        return;
      this.DoStickySceneHeaders();
    }

    public override Rect GetRectForFraming(int row)
    {
      Rect rectForFraming = base.GetRectForFraming(row);
      if (this.showingStickyHeaders && row < this.m_TreeView.data.rowCount)
      {
        GameObjectTreeViewItem objectTreeViewItem = this.m_TreeView.data.GetItem(row) as GameObjectTreeViewItem;
        if (objectTreeViewItem != null && !objectTreeViewItem.isSceneHeader)
        {
          rectForFraming.y -= this.k_LineHeight;
          rectForFraming.height = 2f * this.k_LineHeight;
        }
      }
      return rectForFraming;
    }

    public override bool BeginRename(TreeViewItem item, float delay)
    {
      GameObjectTreeViewItem objectTreeViewItem = item as GameObjectTreeViewItem;
      if (objectTreeViewItem == null || objectTreeViewItem.isSceneHeader)
        return false;
      if ((objectTreeViewItem.objectPPTR.hideFlags & HideFlags.NotEditable) == HideFlags.None)
        return base.BeginRename(item, delay);
      Debug.LogWarning((object) "Unable to rename a GameObject with HideFlags.NotEditable.");
      return false;
    }

    protected override void RenameEnded()
    {
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      ObjectNames.SetNameSmartWithInstanceID(userData, name);
      TreeViewItem treeViewItem = this.m_TreeView.data.FindItem(userData);
      if (treeViewItem != null)
        treeViewItem.displayName = name;
      EditorApplication.RepaintAnimationWindow();
    }

    protected override void DoItemGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
    {
      GameObjectTreeViewItem goItem = item as GameObjectTreeViewItem;
      if (goItem == null)
        return;
      if (goItem.isSceneHeader)
      {
        Color color = GUI.color;
        GUI.color *= new Color(1f, 1f, 1f, 0.9f);
        GUI.Label(rect, GUIContent.none, GameObjectTreeViewGUI.GameObjectStyles.sceneHeaderBg);
        GUI.color = color;
      }
      base.DoItemGUI(rect, row, item, selected, focused, useBoldFont);
      if (goItem.isSceneHeader)
        this.DoAdditionalSceneHeaderGUI(goItem, rect);
      if (!SceneHierarchyWindow.s_Debug)
        return;
      GUI.Label(new Rect(rect.xMax - 70f, rect.y, 70f, rect.height), "" + (object) row + " (" + (object) goItem.id + ")", EditorStyles.boldLabel);
    }

    protected void DoAdditionalSceneHeaderGUI(GameObjectTreeViewItem goItem, Rect rect)
    {
      Rect position = new Rect((float) ((double) rect.width - 16.0 - 4.0), rect.y + (float) (((double) rect.height - 6.0) * 0.5), 16f, rect.height);
      if (Event.current.type == EventType.Repaint)
        GameObjectTreeViewGUI.GameObjectStyles.optionsButtonStyle.Draw(position, false, false, false, false);
      position.y = rect.y;
      position.height = rect.height;
      position.width = 24f;
      if (EditorGUI.DropdownButton(position, GUIContent.none, FocusType.Passive, GUIStyle.none))
      {
        this.m_TreeView.SelectionClick((TreeViewItem) goItem, true);
        this.m_TreeView.contextClickItemCallback(goItem.id);
      }
      if (GameObjectTreeViewGUI.OnPostHeaderGUI == null)
        return;
      float num = rect.width - position.x;
      float width = (float) ((double) rect.width - (double) num - 4.0);
      float x = 0.0f;
      float y = rect.y;
      float height = rect.height;
      Rect availableRect = new Rect(x, y, width, height);
      GameObjectTreeViewGUI.OnPostHeaderGUI(availableRect, goItem.scene.path);
    }

    protected override void OnContentGUI(Rect rect, int row, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      GameObjectTreeViewItem objectTreeViewItem = item as GameObjectTreeViewItem;
      if (objectTreeViewItem == null)
        return;
      if (objectTreeViewItem.isSceneHeader)
      {
        if (objectTreeViewItem.scene.isDirty)
          label += "*";
        switch (objectTreeViewItem.scene.loadingState)
        {
          case Scene.LoadingState.NotLoaded:
            label += " (not loaded)";
            break;
          case Scene.LoadingState.Loading:
            label += " (is loading)";
            break;
        }
        bool useBoldFont1 = objectTreeViewItem.scene == SceneManager.GetActiveScene();
        using (new EditorGUI.DisabledScope(!objectTreeViewItem.scene.isLoaded))
          base.OnContentGUI(rect, row, item, label, selected, focused, useBoldFont1, isPinging);
      }
      else
      {
        if (!isPinging)
          rect.xMin += this.GetContentIndent(item) + this.extraSpaceBeforeIconAndLabel;
        int colorCode = objectTreeViewItem.colorCode;
        if (string.IsNullOrEmpty(item.displayName))
        {
          if (objectTreeViewItem.objectPPTR != (UnityEngine.Object) null)
            objectTreeViewItem.displayName = objectTreeViewItem.objectPPTR.name;
          else
            objectTreeViewItem.displayName = "deleted gameobject";
          label = objectTreeViewItem.displayName;
        }
        GUIStyle guiStyle = TreeViewGUI.Styles.lineStyle;
        if (!objectTreeViewItem.shouldDisplay)
          guiStyle = GameObjectTreeViewGUI.GameObjectStyles.disabledLabel;
        else if ((colorCode & 3) == 0)
          guiStyle = colorCode >= 4 ? GameObjectTreeViewGUI.GameObjectStyles.disabledLabel : TreeViewGUI.Styles.lineStyle;
        else if ((colorCode & 3) == 1)
          guiStyle = colorCode >= 4 ? GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel : GameObjectTreeViewGUI.GameObjectStyles.prefabLabel;
        else if ((colorCode & 3) == 2)
          guiStyle = colorCode >= 4 ? GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel : GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel;
        Texture iconForItem = this.GetIconForItem(item);
        guiStyle.padding.left = 0;
        if ((UnityEngine.Object) iconForItem != (UnityEngine.Object) null)
        {
          Rect position = rect;
          position.width = this.k_IconWidth;
          GUI.DrawTexture(position, iconForItem, ScaleMode.ScaleToFit);
          rect.xMin += this.iconTotalPadding + this.k_IconWidth + this.k_SpaceBetweenIconAndText;
        }
        guiStyle.Draw(rect, label, false, false, selected, focused);
      }
    }

    private enum GameObjectColorType
    {
      Normal,
      Prefab,
      BrokenPrefab,
      Count,
    }

    internal static class GameObjectStyles
    {
      public static GUIStyle disabledLabel = new GUIStyle((GUIStyle) "PR DisabledLabel");
      public static GUIStyle prefabLabel = new GUIStyle((GUIStyle) "PR PrefabLabel");
      public static GUIStyle disabledPrefabLabel = new GUIStyle((GUIStyle) "PR DisabledPrefabLabel");
      public static GUIStyle brokenPrefabLabel = new GUIStyle((GUIStyle) "PR BrokenPrefabLabel");
      public static GUIStyle disabledBrokenPrefabLabel = new GUIStyle((GUIStyle) "PR DisabledBrokenPrefabLabel");
      public static GUIContent loadSceneGUIContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("SceneLoadIn"), "Load scene");
      public static GUIContent unloadSceneGUIContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("SceneLoadOut"), "Unload scene");
      public static GUIContent saveSceneGUIContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("SceneSave"), "Save scene");
      public static GUIStyle optionsButtonStyle = (GUIStyle) "PaneOptions";
      public static GUIStyle sceneHeaderBg = (GUIStyle) "ProjectBrowserTopBarBg";
      public static readonly int kSceneHeaderIconsInterval = 2;

      static GameObjectStyles()
      {
        GameObjectTreeViewGUI.GameObjectStyles.disabledLabel.alignment = TextAnchor.MiddleLeft;
        GameObjectTreeViewGUI.GameObjectStyles.prefabLabel.alignment = TextAnchor.MiddleLeft;
        GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel.alignment = TextAnchor.MiddleLeft;
        GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel.alignment = TextAnchor.MiddleLeft;
        GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel.alignment = TextAnchor.MiddleLeft;
        GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.disabledLabel);
        GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.prefabLabel);
        GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.disabledPrefabLabel);
        GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.brokenPrefabLabel);
        GameObjectTreeViewGUI.GameObjectStyles.ClearSelectionTexture(GameObjectTreeViewGUI.GameObjectStyles.disabledBrokenPrefabLabel);
      }

      private static void ClearSelectionTexture(GUIStyle style)
      {
        Texture2D background = style.hover.background;
        style.onNormal.background = background;
        style.onActive.background = background;
        style.onFocused.background = background;
      }
    }

    internal delegate void OnHeaderGUIDelegate(Rect availableRect, string scenePath);
  }
}
