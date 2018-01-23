// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupHierarchyGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupHierarchyGUI : TreeViewGUI
  {
    private GUIStyle plusButtonStyle = new GUIStyle((GUIStyle) "OL Plus");
    private GUIStyle plusButtonBackgroundStyle = new GUIStyle((GUIStyle) "Tag MenuItem");
    public EditorWindow owner;
    private const float plusButtonWidth = 17f;

    public AddCurvesPopupHierarchyGUI(TreeViewController treeView, EditorWindow owner)
      : base(treeView, true)
    {
      this.owner = owner;
    }

    public bool showPlusButton { get; set; }

    public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
    {
      base.OnRowGUI(rowRect, node, row, selected, focused);
      AddCurvesPopupPropertyNode node1 = node as AddCurvesPopupPropertyNode;
      if (node1 == null || node1.curveBindings == null || node1.curveBindings.Length == 0)
        return;
      Rect position = new Rect(rowRect.width - 17f, rowRect.yMin, 17f, this.plusButtonStyle.fixedHeight);
      GUI.Box(position, GUIContent.none, this.plusButtonBackgroundStyle);
      if (!GUI.Button(position, GUIContent.none, this.plusButtonStyle))
        return;
      AddCurvesPopup.AddNewCurve(node1);
      this.owner.Close();
    }

    protected override void SyncFakeItem()
    {
    }

    protected override void RenameEnded()
    {
    }

    protected override bool IsRenaming(int id)
    {
      return false;
    }

    public override bool BeginRename(TreeViewItem item, float delay)
    {
      return false;
    }

    protected override Texture GetIconForItem(TreeViewItem item)
    {
      if (item != null)
        return (Texture) item.icon;
      return (Texture) null;
    }
  }
}
