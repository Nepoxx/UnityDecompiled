// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlexibleMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class FlexibleMenu : PopupWindowContent
  {
    private Vector2 m_ScrollPosition = Vector2.zero;
    private int m_ShowEditWindowForIndex = -1;
    private float m_CachedWidth = -1f;
    private float m_MinTextWidth = 200f;
    private static FlexibleMenu.Styles s_Styles;
    private IFlexibleMenuItemProvider m_ItemProvider;
    private FlexibleMenuModifyItemUI m_ModifyItemUI;
    private readonly Action<int, object> m_ItemClickedCallback;
    private bool m_ShowAddNewPresetItem;
    private int m_HoverIndex;
    private int[] m_SeperatorIndices;
    private const float lineHeight = 18f;
    private const float seperatorHeight = 8f;
    private const float leftMargin = 25f;

    public FlexibleMenu(IFlexibleMenuItemProvider itemProvider, int selectionIndex, FlexibleMenuModifyItemUI modifyItemUi, Action<int, object> itemClickedCallback)
    {
      this.m_ItemProvider = itemProvider;
      this.m_ModifyItemUI = modifyItemUi;
      this.m_ItemClickedCallback = itemClickedCallback;
      this.m_SeperatorIndices = this.m_ItemProvider.GetSeperatorIndices();
      this.selectedIndex = selectionIndex;
      this.m_ShowAddNewPresetItem = this.m_ModifyItemUI != null;
    }

    private int maxIndex
    {
      get
      {
        return !this.m_ShowAddNewPresetItem ? this.m_ItemProvider.Count() - 1 : this.m_ItemProvider.Count();
      }
    }

    public int selectedIndex { get; set; }

    protected float minTextWidth
    {
      get
      {
        return this.m_MinTextWidth;
      }
      set
      {
        this.m_MinTextWidth = value;
        this.ClearCachedWidth();
      }
    }

    public override Vector2 GetWindowSize()
    {
      return this.CalcSize();
    }

    private bool IsDeleteModiferPressed()
    {
      return Event.current.alt;
    }

    private bool AllowDeleteClick(int index)
    {
      return this.IsDeleteModiferPressed() && this.m_ItemProvider.IsModificationAllowed(index) && GUIUtility.hotControl == 0;
    }

    public override void OnGUI(Rect rect)
    {
      if (FlexibleMenu.s_Styles == null)
        FlexibleMenu.s_Styles = new FlexibleMenu.Styles();
      Event current = Event.current;
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, this.CalcSize().y);
      this.m_ScrollPosition = GUI.BeginScrollView(rect, this.m_ScrollPosition, viewRect);
      float y = 0.0f;
      for (int index = 0; index <= this.maxIndex; ++index)
      {
        int num = index + 1000000;
        Rect rect1 = new Rect(0.0f, y, rect.width, 18f);
        bool flag = Array.IndexOf<int>(this.m_SeperatorIndices, index) >= 0;
        if (this.m_ShowAddNewPresetItem && index == this.m_ItemProvider.Count())
        {
          this.CreateNewItemButton(rect1);
        }
        else
        {
          if (this.m_ShowEditWindowForIndex == index)
          {
            this.m_ShowEditWindowForIndex = -1;
            this.EditExistingItem(rect1, index);
          }
          EventType type = current.type;
          switch (type)
          {
            case EventType.MouseDown:
              if (current.button == 0 && rect1.Contains(current.mousePosition))
              {
                GUIUtility.hotControl = num;
                if (!this.IsDeleteModiferPressed() && current.clickCount == 1)
                {
                  GUIUtility.hotControl = 0;
                  this.SelectItem(index);
                  this.editorWindow.Close();
                  current.Use();
                }
                break;
              }
              break;
            case EventType.MouseUp:
              if (GUIUtility.hotControl == num)
              {
                GUIUtility.hotControl = 0;
                if (current.button == 0 && rect1.Contains(current.mousePosition) && this.AllowDeleteClick(index))
                {
                  this.DeleteItem(index);
                  current.Use();
                }
                break;
              }
              break;
            case EventType.MouseMove:
              if (rect1.Contains(current.mousePosition))
              {
                if (this.m_HoverIndex != index)
                {
                  this.m_HoverIndex = index;
                  this.Repaint();
                  break;
                }
                break;
              }
              if (this.m_HoverIndex == index)
              {
                this.m_HoverIndex = -1;
                this.Repaint();
                break;
              }
              break;
            case EventType.Repaint:
              bool isHover = false;
              if (this.m_HoverIndex == index)
              {
                if (rect1.Contains(current.mousePosition))
                  isHover = true;
                else
                  this.m_HoverIndex = -1;
              }
              if (this.m_ModifyItemUI != null && this.m_ModifyItemUI.IsShowing())
                isHover = this.m_ItemProvider.GetItem(index) == this.m_ModifyItemUI.m_Object;
              FlexibleMenu.s_Styles.menuItem.Draw(rect1, GUIContent.Temp(this.m_ItemProvider.GetName(index)), isHover, false, index == this.selectedIndex, false);
              if (flag)
                FlexibleMenu.DrawRect(new Rect(rect1.x + 4f, (float) ((double) rect1.y + (double) rect1.height + 4.0), rect1.width - 8f, 1f), !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f, 1.333f) : new Color(0.32f, 0.32f, 0.32f, 1.333f));
              if (this.AllowDeleteClick(index))
              {
                EditorGUIUtility.AddCursorRect(rect1, MouseCursor.ArrowMinus);
                break;
              }
              break;
            default:
              if (type == EventType.ContextClick && rect1.Contains(current.mousePosition))
              {
                current.Use();
                if (this.m_ModifyItemUI != null && this.m_ItemProvider.IsModificationAllowed(index))
                  FlexibleMenu.ItemContextMenu.Show(index, this);
                break;
              }
              break;
          }
          y += 18f;
          if (flag)
            y += 8f;
        }
      }
      GUI.EndScrollView();
    }

    private void SelectItem(int index)
    {
      this.selectedIndex = index;
      if (this.m_ItemClickedCallback == null || index < 0)
        return;
      this.m_ItemClickedCallback(index, this.m_ItemProvider.GetItem(index));
    }

    protected Vector2 CalcSize()
    {
      float y = (float) ((double) (this.maxIndex + 1) * 18.0 + (double) this.m_SeperatorIndices.Length * 8.0);
      if ((double) this.m_CachedWidth < 0.0)
        this.m_CachedWidth = Math.Max(this.m_MinTextWidth, this.CalcWidth());
      return new Vector2(this.m_CachedWidth, y);
    }

    private void ClearCachedWidth()
    {
      this.m_CachedWidth = -1f;
    }

    private float CalcWidth()
    {
      if (FlexibleMenu.s_Styles == null)
        FlexibleMenu.s_Styles = new FlexibleMenu.Styles();
      float b = 0.0f;
      for (int index = 0; index < this.m_ItemProvider.Count(); ++index)
        b = Mathf.Max(FlexibleMenu.s_Styles.menuItem.CalcSize(GUIContent.Temp(this.m_ItemProvider.GetName(index))).x, b);
      return b + 6f;
    }

    private void Repaint()
    {
      HandleUtility.Repaint();
    }

    private void CreateNewItemButton(Rect itemRect)
    {
      if (this.m_ModifyItemUI == null)
        return;
      Rect rect = new Rect(itemRect.x + 25f, itemRect.y, 15f, 15f);
      if (!GUI.Button(rect, FlexibleMenu.s_Styles.plusButtonText, (GUIStyle) "OL Plus"))
        return;
      rect.y -= 15f;
      this.m_ModifyItemUI.Init(FlexibleMenuModifyItemUI.MenuType.Add, this.m_ItemProvider.Create(), (Action<object>) (obj =>
      {
        this.ClearCachedWidth();
        this.SelectItem(this.m_ItemProvider.Add(obj));
        EditorApplication.RequestRepaintAllViews();
      }));
      PopupWindow.Show(rect, (PopupWindowContent) this.m_ModifyItemUI, (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
    }

    private void EditExistingItem(Rect itemRect, int index)
    {
      if (this.m_ModifyItemUI == null)
        return;
      itemRect.y -= itemRect.height;
      itemRect.x += itemRect.width;
      this.m_ModifyItemUI.Init(FlexibleMenuModifyItemUI.MenuType.Edit, this.m_ItemProvider.GetItem(index), (Action<object>) (obj =>
      {
        this.ClearCachedWidth();
        this.m_ItemProvider.Replace(index, obj);
        EditorApplication.RequestRepaintAllViews();
      }));
      PopupWindow.Show(itemRect, (PopupWindowContent) this.m_ModifyItemUI, (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
    }

    private void DeleteItem(int index)
    {
      this.ClearCachedWidth();
      this.m_ItemProvider.Remove(index);
      this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.m_ItemProvider.Count() - 1);
    }

    public static void DrawRect(Rect rect, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color1 = GUI.color;
      GUI.color *= color;
      GUI.DrawTexture(rect, (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color1;
    }

    private class Styles
    {
      public GUIStyle menuItem = (GUIStyle) "MenuItem";
      public GUIContent plusButtonText = new GUIContent("", "Add New Item");
    }

    internal static class ItemContextMenu
    {
      private static FlexibleMenu s_Caller;

      public static void Show(int itemIndex, FlexibleMenu caller)
      {
        FlexibleMenu.ItemContextMenu.s_Caller = caller;
        GenericMenu genericMenu1 = new GenericMenu();
        GenericMenu genericMenu2 = genericMenu1;
        GUIContent content1 = new GUIContent("Edit...");
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        if (FlexibleMenu.ItemContextMenu.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FlexibleMenu.ItemContextMenu.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(FlexibleMenu.ItemContextMenu.Edit);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache0 = FlexibleMenu.ItemContextMenu.\u003C\u003Ef__mg\u0024cache0;
        // ISSUE: variable of a boxed type
        __Boxed<int> local1 = (ValueType) itemIndex;
        genericMenu2.AddItem(content1, num1 != 0, fMgCache0, (object) local1);
        GenericMenu genericMenu3 = genericMenu1;
        GUIContent content2 = new GUIContent("Delete");
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        if (FlexibleMenu.ItemContextMenu.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FlexibleMenu.ItemContextMenu.\u003C\u003Ef__mg\u0024cache1 = new GenericMenu.MenuFunction2(FlexibleMenu.ItemContextMenu.Delete);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache1 = FlexibleMenu.ItemContextMenu.\u003C\u003Ef__mg\u0024cache1;
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (ValueType) itemIndex;
        genericMenu3.AddItem(content2, num2 != 0, fMgCache1, (object) local2);
        genericMenu1.ShowAsContext();
        GUIUtility.ExitGUI();
      }

      private static void Delete(object userData)
      {
        int index = (int) userData;
        FlexibleMenu.ItemContextMenu.s_Caller.DeleteItem(index);
      }

      private static void Edit(object userData)
      {
        int num = (int) userData;
        FlexibleMenu.ItemContextMenu.s_Caller.m_ShowEditWindowForIndex = num;
      }
    }
  }
}
