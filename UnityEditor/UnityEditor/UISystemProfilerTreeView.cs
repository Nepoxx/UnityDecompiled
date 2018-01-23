// Decompiled with JetBrains decompiler
// Type: UnityEditor.UISystemProfilerTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class UISystemProfilerTreeView : TreeView
  {
    private readonly UISystemProfilerTreeView.CanvasBatchComparer m_Comparer;
    public ProfilerProperty property;
    private UISystemProfilerTreeView.RootTreeViewItem m_AllCanvasesItem;

    public UISystemProfilerTreeView(UISystemProfilerTreeView.State state, MultiColumnHeader multiColumnHeader)
      : base((TreeViewState) state, multiColumnHeader)
    {
      this.m_Comparer = new UISystemProfilerTreeView.CanvasBatchComparer();
      this.showBorder = false;
      this.showAlternatingRowBackgrounds = true;
    }

    public UISystemProfilerTreeView.State profilerState
    {
      get
      {
        return (UISystemProfilerTreeView.State) this.state;
      }
    }

    protected override TreeViewItem BuildRoot()
    {
      return new TreeViewItem(0, -1);
    }

    protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
    {
      this.profilerState.lastFrame = this.profilerState.profilerWindow.GetActiveVisibleFrameIndex();
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      if (this.property == null || !this.property.frameDataReady)
        return (IList<TreeViewItem>) treeViewItemList;
      this.m_AllCanvasesItem = new UISystemProfilerTreeView.RootTreeViewItem();
      this.SetExpanded(this.m_AllCanvasesItem.id, true);
      root.AddChild((TreeViewItem) this.m_AllCanvasesItem);
      UISystemProfilerInfo[] systemProfilerInfo = this.property.GetUISystemProfilerInfo();
      int[] batchInstanceIds = this.property.GetUISystemBatchInstanceIDs();
      if (systemProfilerInfo != null)
      {
        Dictionary<int, TreeViewItem> dictionary = new Dictionary<int, TreeViewItem>();
        int num = 0;
        foreach (UISystemProfilerInfo info in systemProfilerInfo)
        {
          TreeViewItem allCanvasesItem;
          if (!dictionary.TryGetValue(info.parentId, out allCanvasesItem))
          {
            allCanvasesItem = (TreeViewItem) this.m_AllCanvasesItem;
            this.m_AllCanvasesItem.totalBatchCount += info.totalBatchCount;
            this.m_AllCanvasesItem.totalVertexCount += info.totalVertexCount;
            this.m_AllCanvasesItem.gameObjectCount += info.instanceIDsCount;
          }
          UISystemProfilerTreeView.BaseTreeViewItem baseTreeViewItem;
          if (info.isBatch)
          {
            string displayName = "Batch " + (object) num++;
            baseTreeViewItem = (UISystemProfilerTreeView.BaseTreeViewItem) new UISystemProfilerTreeView.BatchTreeViewItem(info, allCanvasesItem.depth + 1, displayName, batchInstanceIds);
          }
          else
          {
            string profilerNameByOffset = this.property.GetUISystemProfilerNameByOffset(info.objectNameOffset);
            baseTreeViewItem = (UISystemProfilerTreeView.BaseTreeViewItem) new UISystemProfilerTreeView.CanvasTreeViewItem(info, allCanvasesItem.depth + 1, profilerNameByOffset);
            num = 0;
            dictionary[info.objectInstanceId] = (TreeViewItem) baseTreeViewItem;
          }
          if (!this.IsExpanded(allCanvasesItem.id))
          {
            if (!allCanvasesItem.hasChildren)
              allCanvasesItem.children = TreeView.CreateChildListForCollapsedParent();
          }
          else
            allCanvasesItem.AddChild((TreeViewItem) baseTreeViewItem);
        }
        this.m_Comparer.Col = UISystemProfilerTreeView.Column.Element;
        if (this.multiColumnHeader.sortedColumnIndex != -1)
          this.m_Comparer.Col = (UISystemProfilerTreeView.Column) this.multiColumnHeader.sortedColumnIndex;
        this.m_Comparer.isAscending = this.multiColumnHeader.GetColumn((int) this.m_Comparer.Col).sortedAscending;
        this.SetupRows((TreeViewItem) this.m_AllCanvasesItem, (IList<TreeViewItem>) treeViewItemList);
      }
      return (IList<TreeViewItem>) treeViewItemList;
    }

    protected override void RowGUI(TreeView.RowGUIArgs args)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      int visibleColumnIndex = 0;
      for (int numVisibleColumns = args.GetNumVisibleColumns(); visibleColumnIndex < numVisibleColumns; ++visibleColumnIndex)
      {
        int column = args.GetColumn(visibleColumnIndex);
        Rect cellRect = args.GetCellRect(visibleColumnIndex);
        if (column == 0)
        {
          GUIStyle label = TreeView.DefaultStyles.label;
          cellRect.xMin += (float) label.margin.left + this.GetContentIndent(args.item);
          int num1 = 16;
          int num2 = 2;
          Rect position = cellRect;
          position.width = (float) num1;
          Texture icon = (Texture) args.item.icon;
          if ((UnityEngine.Object) icon != (UnityEngine.Object) null)
            GUI.DrawTexture(position, icon, ScaleMode.ScaleToFit);
          label.padding.left = !((UnityEngine.Object) icon == (UnityEngine.Object) null) ? num1 + num2 : 0;
          label.Draw(cellRect, args.item.displayName, false, false, args.selected, args.focused);
        }
        else
        {
          string itemcontent = this.GetItemcontent(args, column);
          if (itemcontent != null)
          {
            TreeView.DefaultGUI.LabelRightAligned(cellRect, itemcontent, args.selected, args.focused);
          }
          else
          {
            GUI.enabled = false;
            TreeView.DefaultGUI.LabelRightAligned(cellRect, "-", false, false);
            GUI.enabled = true;
          }
        }
      }
    }

    protected override void ContextClickedItem(int id)
    {
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Find matching objects in scene"), false, (GenericMenu.MenuFunction) (() => this.DoubleClickedItem(id)));
      genericMenu.ShowAsContext();
    }

    protected override void DoubleClickedItem(int id)
    {
      UISystemProfilerTreeView.HighlightRowsMatchingObjects(this.GetRowsFromIDs((IList<int>) new List<int>()
      {
        id
      }));
    }

    private static void HighlightRowsMatchingObjects(IList<TreeViewItem> rows)
    {
      List<int> intList = new List<int>();
      foreach (TreeViewItem row in (IEnumerable<TreeViewItem>) rows)
      {
        UISystemProfilerTreeView.BatchTreeViewItem batchTreeViewItem = row as UISystemProfilerTreeView.BatchTreeViewItem;
        if (batchTreeViewItem != null)
        {
          intList.AddRange((IEnumerable<int>) batchTreeViewItem.instanceIDs);
        }
        else
        {
          UISystemProfilerTreeView.CanvasTreeViewItem canvasTreeViewItem = row as UISystemProfilerTreeView.CanvasTreeViewItem;
          if (canvasTreeViewItem != null)
          {
            Canvas canvas = EditorUtility.InstanceIDToObject(canvasTreeViewItem.info.objectInstanceId) as Canvas;
            if (!((UnityEngine.Object) canvas == (UnityEngine.Object) null) && !((UnityEngine.Object) canvas.gameObject == (UnityEngine.Object) null))
              intList.Add(canvas.gameObject.GetInstanceID());
          }
        }
      }
      if (intList.Count <= 0)
        return;
      Selection.instanceIDs = intList.ToArray();
    }

    private void SetupRows(TreeViewItem item, IList<TreeViewItem> rows)
    {
      rows.Add(item);
      if (!item.hasChildren || TreeView.IsChildListForACollapsedParent((IList<TreeViewItem>) item.children))
        return;
      if (this.m_Comparer.Col != UISystemProfilerTreeView.Column.Element || this.m_Comparer.isAscending)
        item.children.Sort((IComparer<TreeViewItem>) this.m_Comparer);
      foreach (TreeViewItem child in item.children)
        this.SetupRows(child, rows);
    }

    private string GetItemcontent(TreeView.RowGUIArgs args, int column)
    {
      if (this.m_AllCanvasesItem != null && args.item.id == this.m_AllCanvasesItem.id)
      {
        switch (column)
        {
          case 2:
            return this.m_AllCanvasesItem.totalBatchCount.ToString();
          case 4:
            return this.m_AllCanvasesItem.totalVertexCount.ToString();
          case 6:
            return this.m_AllCanvasesItem.gameObjectCount.ToString();
          default:
            return (string) null;
        }
      }
      else
      {
        UISystemProfilerTreeView.BatchTreeViewItem batchTreeViewItem = args.item as UISystemProfilerTreeView.BatchTreeViewItem;
        if (batchTreeViewItem != null)
        {
          UISystemProfilerInfo info = batchTreeViewItem.info;
          switch (column)
          {
            case 0:
            case 1:
            case 2:
              return (string) null;
            case 3:
              return info.vertexCount.ToString();
            case 4:
              return info.totalVertexCount.ToString();
            case 5:
              if (info.batchBreakingReason != BatchBreakingReason.NoBreaking)
                return UISystemProfilerTreeView.FormatBatchBreakingReason(info);
              goto case 0;
            case 6:
              return info.instanceIDsCount.ToString();
            case 7:
              if (batchTreeViewItem.instanceIDs.Length > 5)
                return string.Format("{0} objects", (object) batchTreeViewItem.instanceIDs.Length);
              StringBuilder stringBuilder = new StringBuilder();
              for (int index = 0; index < batchTreeViewItem.instanceIDs.Length; ++index)
              {
                if (index != 0)
                  stringBuilder.Append(", ");
                int instanceId = batchTreeViewItem.instanceIDs[index];
                UnityEngine.Object @object = EditorUtility.InstanceIDToObject(instanceId);
                if (@object == (UnityEngine.Object) null)
                  stringBuilder.Append(instanceId);
                else
                  stringBuilder.Append(@object.name);
              }
              return stringBuilder.ToString();
            case 8:
              return info.renderDataIndex.ToString();
            default:
              return "Missing";
          }
        }
        else
        {
          UISystemProfilerTreeView.CanvasTreeViewItem canvasTreeViewItem = args.item as UISystemProfilerTreeView.CanvasTreeViewItem;
          if (canvasTreeViewItem == null)
            return (string) null;
          UISystemProfilerInfo info = canvasTreeViewItem.info;
          switch (column)
          {
            case 0:
            case 3:
            case 5:
            case 7:
              return (string) null;
            case 1:
              return info.batchCount.ToString();
            case 2:
              return info.totalBatchCount.ToString();
            case 4:
              return info.totalVertexCount.ToString();
            case 6:
              return info.instanceIDsCount.ToString();
            case 8:
              return info.renderDataIndex.ToString() + " : " + (object) info.renderDataCount;
            default:
              return "Missing";
          }
        }
      }
    }

    internal IList<TreeViewItem> GetRowsFromIDs(IList<int> selection)
    {
      return this.FindRows(selection);
    }

    private static string FormatBatchBreakingReason(UISystemProfilerInfo info)
    {
      BatchBreakingReason batchBreakingReason = info.batchBreakingReason;
      switch (batchBreakingReason)
      {
        case BatchBreakingReason.NoBreaking:
          return "NoBreaking";
        case BatchBreakingReason.NotCoplanarWithCanvas:
          return "Not Coplanar With Canvas";
        case BatchBreakingReason.CanvasInjectionIndex:
          return "Canvas Injection Index";
        case BatchBreakingReason.DifferentMaterialInstance:
          return "Different Material Instance";
        case BatchBreakingReason.DifferentRectClipping:
          return "Different Rect Clipping";
        default:
          if (batchBreakingReason == BatchBreakingReason.DifferentTexture)
            return "Different Texture";
          if (batchBreakingReason == BatchBreakingReason.DifferentA8TextureUsage)
            return "Different A8 Texture Usage";
          if (batchBreakingReason == BatchBreakingReason.DifferentClipRect)
            return "Different Clip Rect";
          if (batchBreakingReason == BatchBreakingReason.Unknown)
            return "Unknown";
          throw new ArgumentOutOfRangeException();
      }
    }

    internal class State : TreeViewState
    {
      public int lastFrame;
      public ProfilerWindow profilerWindow;
    }

    internal class CanvasBatchComparer : IComparer<TreeViewItem>
    {
      internal UISystemProfilerTreeView.Column Col;
      internal bool isAscending;

      public int Compare(TreeViewItem x, TreeViewItem y)
      {
        int num = !this.isAscending ? -1 : 1;
        UISystemProfilerTreeView.BaseTreeViewItem baseTreeViewItem1 = (UISystemProfilerTreeView.BaseTreeViewItem) x;
        UISystemProfilerTreeView.BaseTreeViewItem baseTreeViewItem2 = (UISystemProfilerTreeView.BaseTreeViewItem) y;
        if (baseTreeViewItem1.info.isBatch != baseTreeViewItem2.info.isBatch)
          return !baseTreeViewItem1.info.isBatch ? -1 : 1;
        switch (this.Col)
        {
          case UISystemProfilerTreeView.Column.Element:
            if (baseTreeViewItem1.info.isBatch)
              return -1;
            return baseTreeViewItem1.displayName.CompareTo(baseTreeViewItem2.displayName) * num;
          case UISystemProfilerTreeView.Column.BatchCount:
            if (baseTreeViewItem1.info.isBatch)
              return -1;
            return baseTreeViewItem1.info.batchCount.CompareTo(baseTreeViewItem2.info.batchCount) * num;
          case UISystemProfilerTreeView.Column.TotalBatchCount:
            if (baseTreeViewItem1.info.isBatch)
              return -1;
            return baseTreeViewItem1.info.totalBatchCount.CompareTo(baseTreeViewItem2.info.totalBatchCount) * num;
          case UISystemProfilerTreeView.Column.VertexCount:
            if (baseTreeViewItem1.info.isBatch)
              return baseTreeViewItem1.info.vertexCount.CompareTo(baseTreeViewItem2.info.vertexCount) * num;
            return string.CompareOrdinal(baseTreeViewItem1.displayName, baseTreeViewItem2.displayName);
          case UISystemProfilerTreeView.Column.TotalVertexCount:
            return baseTreeViewItem1.info.totalVertexCount.CompareTo(baseTreeViewItem2.info.totalVertexCount) * num;
          case UISystemProfilerTreeView.Column.GameObjectCount:
            return baseTreeViewItem1.info.instanceIDsCount.CompareTo(baseTreeViewItem2.info.instanceIDsCount) * num;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    internal class RootTreeViewItem : TreeViewItem
    {
      public int gameObjectCount;
      public int totalBatchCount;
      public int totalVertexCount;

      public RootTreeViewItem()
        : base(1, 0, (TreeViewItem) null, "All Canvases")
      {
      }
    }

    internal class BaseTreeViewItem : TreeViewItem
    {
      protected static readonly Texture2D s_CanvasIcon = EditorGUIUtility.LoadIcon("RectTool On");
      public UISystemProfilerInfo info;
      public int renderDataIndex;

      internal BaseTreeViewItem(UISystemProfilerInfo info, int depth, string displayName)
        : base(info.objectInstanceId, depth, displayName)
      {
        this.info = info;
      }
    }

    internal sealed class CanvasTreeViewItem : UISystemProfilerTreeView.BaseTreeViewItem
    {
      public CanvasTreeViewItem(UISystemProfilerInfo info, int depth, string displayName)
        : base(info, depth, displayName)
      {
        this.icon = UISystemProfilerTreeView.BaseTreeViewItem.s_CanvasIcon;
      }
    }

    internal sealed class BatchTreeViewItem : UISystemProfilerTreeView.BaseTreeViewItem
    {
      public int[] instanceIDs;

      public BatchTreeViewItem(UISystemProfilerInfo info, int depth, string displayName, int[] allBatchesInstanceIDs)
        : base(info, depth, displayName)
      {
        this.icon = (Texture2D) null;
        this.instanceIDs = new int[info.instanceIDsCount];
        Array.Copy((Array) allBatchesInstanceIDs, info.instanceIDsIndex, (Array) this.instanceIDs, 0, info.instanceIDsCount);
        this.renderDataIndex = info.renderDataIndex;
      }
    }

    internal enum Column
    {
      Element,
      BatchCount,
      TotalBatchCount,
      VertexCount,
      TotalVertexCount,
      BatchBreakingReason,
      GameObjectCount,
      InstanceIds,
      Rerender,
    }
  }
}
