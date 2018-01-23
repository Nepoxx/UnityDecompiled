// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExposablePopupMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ExposablePopupMenu
  {
    private Action<ExposablePopupMenu.ItemData> m_SelectionChangedCallback = (Action<ExposablePopupMenu.ItemData>) null;
    private List<ExposablePopupMenu.ItemData> m_Items;
    private float m_WidthOfButtons;
    private float m_ItemSpacing;
    private ExposablePopupMenu.PopupButtonData m_PopupButtonData;
    private float m_WidthOfPopup;
    private float m_MinWidthOfPopup;

    public void Init(List<ExposablePopupMenu.ItemData> items, float itemSpacing, float minWidthOfPopup, ExposablePopupMenu.PopupButtonData popupButtonData, Action<ExposablePopupMenu.ItemData> selectionChangedCallback)
    {
      this.m_Items = items;
      this.m_ItemSpacing = itemSpacing;
      this.m_PopupButtonData = popupButtonData;
      this.m_SelectionChangedCallback = selectionChangedCallback;
      this.m_MinWidthOfPopup = minWidthOfPopup;
      this.CalcWidths();
    }

    public float OnGUI(Rect rect)
    {
      if ((double) rect.width >= (double) this.m_WidthOfButtons && (double) rect.width > (double) this.m_MinWidthOfPopup)
      {
        Rect position = rect;
        foreach (ExposablePopupMenu.ItemData itemData in this.m_Items)
        {
          position.width = itemData.m_Width;
          EditorGUI.BeginChangeCheck();
          using (new EditorGUI.DisabledScope(!itemData.m_Enabled))
            GUI.Toggle(position, itemData.m_On, itemData.m_GUIContent, itemData.m_Style);
          if (EditorGUI.EndChangeCheck())
          {
            this.SelectionChanged(itemData);
            GUIUtility.ExitGUI();
          }
          position.x += itemData.m_Width + this.m_ItemSpacing;
        }
        return this.m_WidthOfButtons;
      }
      if ((double) this.m_WidthOfPopup < (double) rect.width)
        rect.width = this.m_WidthOfPopup;
      if (EditorGUI.DropdownButton(rect, this.m_PopupButtonData.m_GUIContent, FocusType.Passive, this.m_PopupButtonData.m_Style))
        ExposablePopupMenu.PopUpMenu.Show(rect, this.m_Items, this);
      return this.m_WidthOfPopup;
    }

    private void CalcWidths()
    {
      this.m_WidthOfButtons = 0.0f;
      foreach (ExposablePopupMenu.ItemData itemData in this.m_Items)
      {
        itemData.m_Width = itemData.m_Style.CalcSize(itemData.m_GUIContent).x;
        this.m_WidthOfButtons += itemData.m_Width;
      }
      this.m_WidthOfButtons += (float) (this.m_Items.Count - 1) * this.m_ItemSpacing;
      Vector2 vector2 = this.m_PopupButtonData.m_Style.CalcSize(this.m_PopupButtonData.m_GUIContent);
      vector2.x += 3f;
      this.m_WidthOfPopup = vector2.x;
    }

    private void SelectionChanged(ExposablePopupMenu.ItemData item)
    {
      if (this.m_SelectionChangedCallback != null)
        this.m_SelectionChangedCallback(item);
      else
        Debug.LogError((object) "Callback is null");
    }

    public class ItemData
    {
      public GUIContent m_GUIContent;
      public GUIStyle m_Style;
      public bool m_On;
      public bool m_Enabled;
      public object m_UserData;
      public float m_Width;

      public ItemData(GUIContent content, GUIStyle style, bool on, bool enabled, object userData)
      {
        this.m_GUIContent = content;
        this.m_Style = style;
        this.m_On = on;
        this.m_Enabled = enabled;
        this.m_UserData = userData;
      }
    }

    public class PopupButtonData
    {
      public GUIContent m_GUIContent;
      public GUIStyle m_Style;

      public PopupButtonData(GUIContent content, GUIStyle style)
      {
        this.m_GUIContent = content;
        this.m_Style = style;
      }
    }

    internal class PopUpMenu
    {
      private static List<ExposablePopupMenu.ItemData> m_Data;
      private static ExposablePopupMenu m_Caller;

      internal static void Show(Rect activatorRect, List<ExposablePopupMenu.ItemData> buttonData, ExposablePopupMenu caller)
      {
        ExposablePopupMenu.PopUpMenu.m_Data = buttonData;
        ExposablePopupMenu.PopUpMenu.m_Caller = caller;
        GenericMenu genericMenu1 = new GenericMenu();
        foreach (ExposablePopupMenu.ItemData itemData1 in ExposablePopupMenu.PopUpMenu.m_Data)
        {
          if (itemData1.m_Enabled)
          {
            GenericMenu genericMenu2 = genericMenu1;
            GUIContent guiContent = itemData1.m_GUIContent;
            int num = itemData1.m_On ? 1 : 0;
            // ISSUE: reference to a compiler-generated field
            if (ExposablePopupMenu.PopUpMenu.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ExposablePopupMenu.PopUpMenu.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(ExposablePopupMenu.PopUpMenu.SelectionCallback);
            }
            // ISSUE: reference to a compiler-generated field
            GenericMenu.MenuFunction2 fMgCache0 = ExposablePopupMenu.PopUpMenu.\u003C\u003Ef__mg\u0024cache0;
            ExposablePopupMenu.ItemData itemData2 = itemData1;
            genericMenu2.AddItem(guiContent, num != 0, fMgCache0, (object) itemData2);
          }
          else
            genericMenu1.AddDisabledItem(itemData1.m_GUIContent);
        }
        genericMenu1.DropDown(activatorRect);
      }

      private static void SelectionCallback(object userData)
      {
        ExposablePopupMenu.ItemData itemData = (ExposablePopupMenu.ItemData) userData;
        ExposablePopupMenu.PopUpMenu.m_Caller.SelectionChanged(itemData);
        ExposablePopupMenu.PopUpMenu.m_Caller = (ExposablePopupMenu) null;
        ExposablePopupMenu.PopUpMenu.m_Data = (List<ExposablePopupMenu.ItemData>) null;
      }
    }
  }
}
