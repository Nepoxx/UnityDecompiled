// Decompiled with JetBrains decompiler
// Type: UnityEditor.DiagnosticSwitchPreferences
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class DiagnosticSwitchPreferences
  {
    private static string s_FilterString = string.Empty;
    private static readonly DiagnosticSwitchPreferences.Resources s_Resources = new DiagnosticSwitchPreferences.Resources();
    private static Vector2 s_ScrollOffset;
    private const uint kMaxRangeForSlider = 10;

    private static void DoTopBar()
    {
      using (new EditorGUILayout.HorizontalScope(DiagnosticSwitchPreferences.s_Resources.title, new GUILayoutOption[0]))
      {
        GUILayout.FlexibleSpace();
        DiagnosticSwitchPreferences.s_FilterString = GUILayout.TextField(DiagnosticSwitchPreferences.s_FilterString, EditorStyles.toolbarSearchField, new GUILayoutOption[1]
        {
          GUILayout.Width(200f)
        });
        if (!GUILayout.Button(GUIContent.none, !string.IsNullOrEmpty(DiagnosticSwitchPreferences.s_FilterString) ? EditorStyles.toolbarSearchFieldCancelButton : EditorStyles.toolbarSearchFieldCancelButtonEmpty, new GUILayoutOption[0]))
          return;
        DiagnosticSwitchPreferences.s_FilterString = string.Empty;
      }
    }

    private static bool PassesFilter(DiagnosticSwitch diagnosticSwitch, string filterString)
    {
      return string.IsNullOrEmpty(DiagnosticSwitchPreferences.s_FilterString) || diagnosticSwitch.name.ToLowerInvariant().Contains(filterString) || diagnosticSwitch.description.ToLowerInvariant().Contains(filterString);
    }

    private static bool DisplaySwitch(DiagnosticSwitch diagnosticSwitch)
    {
      GUIContent label = new GUIContent(diagnosticSwitch.name, diagnosticSwitch.name + "\n\n" + diagnosticSwitch.description);
      bool flag = !object.Equals(diagnosticSwitch.value, diagnosticSwitch.persistentValue);
      EditorGUI.BeginChangeCheck();
      using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
        Rect position = new Rect(rect.x, rect.y, rect.height, rect.height);
        rect.xMin += position.width + 3f;
        if (flag && Event.current.type == EventType.Repaint)
          GUI.DrawTexture(position, (Texture) DiagnosticSwitchPreferences.s_Resources.smallWarningIcon);
        if (diagnosticSwitch.value is bool)
          diagnosticSwitch.persistentValue = (object) EditorGUI.Toggle(rect, label, (bool) diagnosticSwitch.persistentValue);
        else if (diagnosticSwitch.enumInfo != null)
        {
          if (diagnosticSwitch.enumInfo.isFlags)
          {
            int num1 = 0;
            foreach (int num2 in diagnosticSwitch.enumInfo.values)
              num1 |= num2;
            string[] displayedOptions = diagnosticSwitch.enumInfo.names;
            int[] optionValues = diagnosticSwitch.enumInfo.values;
            if (diagnosticSwitch.enumInfo.values[0] == 0)
            {
              displayedOptions = new string[displayedOptions.Length - 1];
              optionValues = new int[optionValues.Length - 1];
              Array.Copy((Array) diagnosticSwitch.enumInfo.names, 1, (Array) displayedOptions, 0, displayedOptions.Length);
              Array.Copy((Array) diagnosticSwitch.enumInfo.values, 1, (Array) optionValues, 0, optionValues.Length);
            }
            diagnosticSwitch.persistentValue = (object) (EditorGUI.MaskFieldInternal(rect, label, (int) diagnosticSwitch.persistentValue, displayedOptions, optionValues, EditorStyles.popup) & num1);
          }
          else
          {
            GUIContent[] displayedOptions = new GUIContent[diagnosticSwitch.enumInfo.names.Length];
            for (int index = 0; index < diagnosticSwitch.enumInfo.names.Length; ++index)
              displayedOptions[index] = new GUIContent(diagnosticSwitch.enumInfo.names[index], diagnosticSwitch.enumInfo.annotations[index]);
            diagnosticSwitch.persistentValue = (object) EditorGUI.IntPopup(rect, label, (int) diagnosticSwitch.persistentValue, displayedOptions, diagnosticSwitch.enumInfo.values);
          }
        }
        else if (diagnosticSwitch.value is uint)
        {
          uint minValue = (uint) diagnosticSwitch.minValue;
          uint maxValue = (uint) diagnosticSwitch.maxValue;
          diagnosticSwitch.persistentValue = maxValue - minValue > 10U || maxValue - minValue <= 0U || (minValue >= (uint) int.MaxValue || maxValue >= (uint) int.MaxValue) ? (object) (uint) EditorGUI.IntField(rect, label, (int) (uint) diagnosticSwitch.persistentValue) : (object) (uint) EditorGUI.IntSlider(rect, label, (int) (uint) diagnosticSwitch.persistentValue, (int) minValue, (int) maxValue);
        }
        else if (diagnosticSwitch.value is int)
        {
          int minValue = (int) diagnosticSwitch.minValue;
          int maxValue = (int) diagnosticSwitch.maxValue;
          diagnosticSwitch.persistentValue = (long) (maxValue - minValue) > 10L || maxValue - minValue <= 0 || (minValue >= int.MaxValue || maxValue >= int.MaxValue) ? (object) EditorGUI.IntField(rect, label, (int) diagnosticSwitch.persistentValue) : (object) EditorGUI.IntSlider(rect, label, (int) diagnosticSwitch.persistentValue, minValue, maxValue);
        }
        else if (diagnosticSwitch.value is string)
          diagnosticSwitch.persistentValue = (object) EditorGUI.TextField(rect, label, (string) diagnosticSwitch.persistentValue);
        else
          EditorGUI.LabelField(rect, label, new GUIContent("Unsupported type: " + diagnosticSwitch.value.GetType().Name), new GUIStyle()
          {
            normal = {
              textColor = Color.red
            }
          });
      }
      if (EditorGUI.EndChangeCheck())
        Debug.SetDiagnosticSwitch(diagnosticSwitch.name, diagnosticSwitch.persistentValue, true);
      return flag;
    }

    [PreferenceItem("Diagnostics")]
    private static void OnGUI()
    {
      List<DiagnosticSwitch> results = new List<DiagnosticSwitch>();
      Debug.GetDiagnosticSwitches(results);
      results.Sort((Comparison<DiagnosticSwitch>) ((a, b) => Comparer<string>.Default.Compare(a.name, b.name)));
      DiagnosticSwitchPreferences.DoTopBar();
      bool flag = false;
      using (EditorGUILayout.VerticalScrollViewScope verticalScrollViewScope = new EditorGUILayout.VerticalScrollViewScope(DiagnosticSwitchPreferences.s_ScrollOffset, false, GUI.skin.verticalScrollbar, DiagnosticSwitchPreferences.s_Resources.scrollArea, new GUILayoutOption[0]))
      {
        string filterString = DiagnosticSwitchPreferences.s_FilterString.ToLowerInvariant().Trim();
        for (int index = 0; index < results.Count; ++index)
        {
          if (DiagnosticSwitchPreferences.PassesFilter(results[index], filterString))
            flag |= DiagnosticSwitchPreferences.DisplaySwitch(results[index]);
        }
        DiagnosticSwitchPreferences.s_ScrollOffset = verticalScrollViewScope.scrollPosition;
      }
      Rect rect = GUILayoutUtility.GetRect(DiagnosticSwitchPreferences.s_Resources.restartNeededWarning, EditorStyles.helpBox, new GUILayoutOption[1]{ GUILayout.MinHeight(40f) });
      if (!flag)
        return;
      EditorGUI.HelpBox(rect, DiagnosticSwitchPreferences.s_Resources.restartNeededWarning.text, MessageType.Warning);
    }

    private class Resources
    {
      public GUIStyle title = (GUIStyle) "OL Title";
      public GUIStyle scrollArea = (GUIStyle) "OL Box";
      public GUIContent restartNeededWarning = new GUIContent("Some settings will not take effect until you restart Unity.");
      public Texture2D smallWarningIcon;

      public Resources()
      {
        this.smallWarningIcon = EditorGUIUtility.LoadIconRequired("console.warnicon.sml");
      }
    }
  }
}
