// Decompiled with JetBrains decompiler
// Type: UnityEditor.LabelGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class LabelGUI
  {
    private static int s_MaxShownLabels = 10;
    private bool m_CurrentChanged = false;
    private bool m_ChangeWasAdd = false;
    private bool m_IgnoreNextAssetLabelsChangedCall = false;
    private HashSet<UnityEngine.Object> m_CurrentAssetsSet;
    private PopupList.InputData m_AssetLabels;
    private string m_ChangedLabel;
    private static Action<UnityEngine.Object> s_AssetLabelsForObjectChangedDelegates;

    public void OnEnable()
    {
      LabelGUI.s_AssetLabelsForObjectChangedDelegates += new Action<UnityEngine.Object>(this.AssetLabelsChangedForObject);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.InvalidateLabels);
    }

    public void OnDisable()
    {
      LabelGUI.s_AssetLabelsForObjectChangedDelegates -= new Action<UnityEngine.Object>(this.AssetLabelsChangedForObject);
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.InvalidateLabels);
      this.SaveLabels();
    }

    public void OnLostFocus()
    {
      this.SaveLabels();
    }

    public void InvalidateLabels()
    {
      this.m_AssetLabels = (PopupList.InputData) null;
      this.m_CurrentAssetsSet = (HashSet<UnityEngine.Object>) null;
    }

    public void AssetLabelsChangedForObject(UnityEngine.Object asset)
    {
      if (!this.m_IgnoreNextAssetLabelsChangedCall && this.m_CurrentAssetsSet != null && this.m_CurrentAssetsSet.Contains(asset))
        this.m_AssetLabels = (PopupList.InputData) null;
      this.m_IgnoreNextAssetLabelsChangedCall = false;
    }

    public void SaveLabels()
    {
      if (!this.m_CurrentChanged || this.m_AssetLabels == null || this.m_CurrentAssetsSet == null)
        return;
      bool flag1 = false;
      foreach (UnityEngine.Object currentAssets in this.m_CurrentAssetsSet)
      {
        bool flag2 = false;
        List<string> list = ((IEnumerable<string>) AssetDatabase.GetLabels(currentAssets)).ToList<string>();
        if (this.m_ChangeWasAdd)
        {
          if (!list.Contains(this.m_ChangedLabel))
          {
            list.Add(this.m_ChangedLabel);
            flag2 = true;
          }
        }
        else if (list.Contains(this.m_ChangedLabel))
        {
          list.Remove(this.m_ChangedLabel);
          flag2 = true;
        }
        if (flag2)
        {
          AssetDatabase.SetLabels(currentAssets, list.ToArray());
          if (LabelGUI.s_AssetLabelsForObjectChangedDelegates != null)
          {
            this.m_IgnoreNextAssetLabelsChangedCall = true;
            LabelGUI.s_AssetLabelsForObjectChangedDelegates(currentAssets);
          }
          flag1 = true;
        }
      }
      if (flag1)
        EditorApplication.Internal_CallAssetLabelsHaveChanged();
      this.m_CurrentChanged = false;
    }

    public void AssetLabelListCallback(PopupList.ListElement element)
    {
      this.m_ChangedLabel = element.text;
      element.selected = !element.selected;
      this.m_ChangeWasAdd = element.selected;
      element.partiallySelected = false;
      this.m_CurrentChanged = true;
      this.SaveLabels();
      InspectorWindow.RepaintAllInspectors();
    }

    public void InitLabelCache(UnityEngine.Object[] assets)
    {
      HashSet<UnityEngine.Object> objectSet = new HashSet<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) assets);
      if (this.m_CurrentAssetsSet == null || !this.m_CurrentAssetsSet.SetEquals((IEnumerable<UnityEngine.Object>) objectSet))
      {
        List<string> all;
        List<string> partial;
        this.GetLabelsForAssets(assets, out all, out partial);
        this.m_AssetLabels = new PopupList.InputData()
        {
          m_CloseOnSelection = false,
          m_AllowCustom = true,
          m_OnSelectCallback = new PopupList.OnSelectCallback(this.AssetLabelListCallback),
          m_MaxCount = 15,
          m_SortAlphabetically = true
        };
        foreach (KeyValuePair<string, float> allLabel in AssetDatabase.GetAllLabels())
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LabelGUI.\u003CInitLabelCache\u003Ec__AnonStorey0 cacheCAnonStorey0 = new LabelGUI.\u003CInitLabelCache\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          cacheCAnonStorey0.pair = allLabel;
          // ISSUE: reference to a compiler-generated field
          PopupList.ListElement listElement = this.m_AssetLabels.NewOrMatchingElement(cacheCAnonStorey0.pair.Key);
          // ISSUE: reference to a compiler-generated field
          if ((double) listElement.filterScore < (double) cacheCAnonStorey0.pair.Value)
          {
            // ISSUE: reference to a compiler-generated field
            listElement.filterScore = cacheCAnonStorey0.pair.Value;
          }
          // ISSUE: reference to a compiler-generated method
          listElement.selected = all.Any<string>(new Func<string, bool>(cacheCAnonStorey0.\u003C\u003Em__0));
          // ISSUE: reference to a compiler-generated method
          listElement.partiallySelected = partial.Any<string>(new Func<string, bool>(cacheCAnonStorey0.\u003C\u003Em__1));
        }
      }
      this.m_CurrentAssetsSet = objectSet;
      this.m_CurrentChanged = false;
    }

    public void OnLabelGUI(UnityEngine.Object[] assets)
    {
      this.InitLabelCache(assets);
      float num1 = 1f;
      float num2 = 2f;
      float pixels = 3f;
      float num3 = 5f;
      GUIStyle assetLabelIcon = EditorStyles.assetLabelIcon;
      float num4 = (float) assetLabelIcon.margin.left + assetLabelIcon.fixedWidth + num2;
      GUILayout.Space(pixels);
      Rect rect1 = GUILayoutUtility.GetRect(0.0f, 10240f, 0.0f, 0.0f);
      rect1.width -= num4;
      EditorGUILayout.BeginHorizontal();
      GUILayoutUtility.GetRect(num1, num1, 0.0f, 0.0f);
      this.DrawLabelList(false, rect1.xMax);
      this.DrawLabelList(true, rect1.xMax);
      GUILayout.FlexibleSpace();
      Rect rect2 = GUILayoutUtility.GetRect(assetLabelIcon.fixedWidth, assetLabelIcon.fixedWidth, assetLabelIcon.fixedHeight + num3, assetLabelIcon.fixedHeight + num3);
      rect2.x = rect1.xMax + (float) assetLabelIcon.margin.left;
      if (EditorGUI.DropdownButton(rect2, GUIContent.none, FocusType.Passive, assetLabelIcon))
        PopupWindow.Show(rect2, (PopupWindowContent) new PopupList(this.m_AssetLabels), (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
      EditorGUILayout.EndHorizontal();
    }

    private void DrawLabelList(bool partiallySelected, float xMax)
    {
      GUIStyle style = !partiallySelected ? EditorStyles.assetLabel : EditorStyles.assetLabelPartial;
      Event current = Event.current;
      foreach (GUIContent content in this.m_AssetLabels.m_ListElements.Where<PopupList.ListElement>((Func<PopupList.ListElement, bool>) (i => !partiallySelected ? i.selected : i.partiallySelected)).OrderBy<PopupList.ListElement, string>((Func<PopupList.ListElement, string>) (i => i.text.ToLower())).Select<PopupList.ListElement, GUIContent>((Func<PopupList.ListElement, GUIContent>) (i => i.m_Content)).Take<GUIContent>(LabelGUI.s_MaxShownLabels))
      {
        Rect rect = GUILayoutUtility.GetRect(content, style);
        if (Event.current.type == EventType.Repaint && (double) rect.xMax >= (double) xMax)
          break;
        GUI.Label(rect, content, style);
        if ((double) rect.xMax <= (double) xMax && current.type == EventType.MouseDown && (rect.Contains(current.mousePosition) && current.button == 0) && GUI.enabled)
        {
          current.Use();
          rect.x = xMax;
          PopupWindow.Show(rect, (PopupWindowContent) new PopupList(this.m_AssetLabels, content.text), (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
        }
      }
    }

    private void GetLabelsForAssets(UnityEngine.Object[] assets, out List<string> all, out List<string> partial)
    {
      all = new List<string>();
      partial = new List<string>();
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      foreach (UnityEngine.Object asset in assets)
      {
        foreach (string label in AssetDatabase.GetLabels(asset))
          dictionary[label] = !dictionary.ContainsKey(label) ? 1 : dictionary[label] + 1;
      }
      foreach (KeyValuePair<string, int> keyValuePair in dictionary)
        (keyValuePair.Value != assets.Length ? partial : all).Add(keyValuePair.Key);
    }
  }
}
