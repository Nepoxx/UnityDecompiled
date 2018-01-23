// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewExamples.TestGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.TreeViewExamples
{
  internal class TestGUI : TreeViewGUI
  {
    private Texture2D m_FolderIcon = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
    private Texture2D m_Icon = EditorGUIUtility.FindTexture("boo Script Icon");
    private GUIStyle m_LabelStyle;
    private GUIStyle m_LabelStyleRightAlign;

    public TestGUI(TreeViewController treeView)
      : base(treeView)
    {
    }

    protected override Texture GetIconForItem(TreeViewItem item)
    {
      return !item.hasChildren ? (Texture) this.m_Icon : (Texture) this.m_FolderIcon;
    }

    protected override void RenameEnded()
    {
    }

    protected override void SyncFakeItem()
    {
    }

    private float[] columnWidths
    {
      get
      {
        return ((TreeViewStateWithColumns) this.m_TreeView.state).columnWidths;
      }
    }

    protected override void OnContentGUI(Rect rect, int row, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      if (Event.current.rawType != EventType.Repaint)
        return;
      if (this.m_LabelStyle == null)
      {
        this.m_LabelStyle = new GUIStyle(TreeViewGUI.Styles.lineStyle);
        RectOffset padding1 = this.m_LabelStyle.padding;
        int num1 = 6;
        this.m_LabelStyle.padding.right = num1;
        int num2 = num1;
        padding1.left = num2;
        this.m_LabelStyleRightAlign = new GUIStyle(TreeViewGUI.Styles.lineStyle);
        RectOffset padding2 = this.m_LabelStyleRightAlign.padding;
        int num3 = 6;
        this.m_LabelStyleRightAlign.padding.left = num3;
        int num4 = num3;
        padding2.right = num4;
        this.m_LabelStyleRightAlign.alignment = TextAnchor.MiddleRight;
      }
      if (selected)
        TreeViewGUI.Styles.selectionStyle.Draw(rect, false, false, true, focused);
      if (isPinging || this.columnWidths == null || this.columnWidths.Length == 0)
      {
        base.OnContentGUI(rect, row, item, label, selected, focused, useBoldFont, isPinging);
      }
      else
      {
        Rect rect1 = rect;
        for (int index = 0; index < this.columnWidths.Length; ++index)
        {
          rect1.width = this.columnWidths[index];
          if (index == 0)
            base.OnContentGUI(rect1, row, item, label, selected, focused, useBoldFont, isPinging);
          else
            GUI.Label(rect1, "Zksdf SDFS DFASDF ", index % 2 != 0 ? this.m_LabelStyleRightAlign : this.m_LabelStyle);
          rect1.x += rect1.width;
        }
      }
    }
  }
}
