// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaskFieldGUIDeprecated
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Obsolete("MaskFieldGUIDeprecated is deprecated. Use MaskFieldGUI instead.")]
  internal static class MaskFieldGUIDeprecated
  {
    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, GUIStyle style)
    {
      int changedFlags;
      bool changedToValue;
      return MaskFieldGUIDeprecated.DoMaskField(position, controlID, mask, flagNames, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, int[] flagValues, GUIStyle style)
    {
      int changedFlags;
      bool changedToValue;
      return MaskFieldGUIDeprecated.DoMaskField(position, controlID, mask, flagNames, flagValues, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, GUIStyle style, out int changedFlags, out bool changedToValue)
    {
      int[] flagValues = new int[flagNames.Length];
      for (int index = 0; index < flagValues.Length; ++index)
        flagValues[index] = 1 << index;
      return MaskFieldGUIDeprecated.DoMaskField(position, controlID, mask, flagNames, flagValues, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, int[] flagValues, GUIStyle style, out int changedFlags, out bool changedToValue)
    {
      mask = MaskFieldGUIDeprecated.MaskCallbackInfo.GetSelectedValueForControl(controlID, mask, out changedFlags, out changedToValue);
      List<int> intList = new List<int>();
      List<string> stringList = new List<string>() { "Nothing", "Everything" };
      for (int index = 0; index < flagNames.Length; ++index)
      {
        if ((mask & flagValues[index]) != 0)
          intList.Add(index + 2);
      }
      stringList.AddRange((IEnumerable<string>) flagNames);
      GUIContent content = EditorGUI.mixedValueContent;
      if (!EditorGUI.showMixedValue)
      {
        switch (intList.Count)
        {
          case 0:
            content = EditorGUIUtility.TempContent("Nothing");
            intList.Add(0);
            break;
          case 1:
            content = new GUIContent(stringList[intList[0]]);
            break;
          default:
            if (intList.Count >= flagNames.Length)
            {
              content = EditorGUIUtility.TempContent("Everything");
              intList.Add(1);
              mask = -1;
              break;
            }
            content = EditorGUIUtility.TempContent("Mixed ...");
            break;
        }
      }
      Event current = Event.current;
      if (current.type == EventType.Repaint)
        style.Draw(position, content, controlID, false);
      else if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlID))
      {
        MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance = new MaskFieldGUIDeprecated.MaskCallbackInfo(controlID);
        current.Use();
        EditorUtility.DisplayCustomMenu(position, stringList.ToArray(), !EditorGUI.showMixedValue ? intList.ToArray() : new int[0], new EditorUtility.SelectMenuItemFunction(MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.SetMaskValueDelegate), (object) flagValues);
        GUIUtility.keyboardControl = controlID;
      }
      return mask;
    }

    private class MaskCallbackInfo
    {
      public static MaskFieldGUIDeprecated.MaskCallbackInfo m_Instance;
      private const string kMaskMenuChangedMessage = "MaskMenuChanged";
      private readonly int m_ControlID;
      private int m_Mask;
      private bool m_SetAll;
      private bool m_ClearAll;
      private bool m_DoNothing;
      private readonly GUIView m_SourceView;

      public MaskCallbackInfo(int controlID)
      {
        this.m_ControlID = controlID;
        this.m_SourceView = GUIView.current;
      }

      public static int GetSelectedValueForControl(int controlID, int mask, out int changedFlags, out bool changedToValue)
      {
        Event current = Event.current;
        changedFlags = 0;
        changedToValue = false;
        if (current.type == EventType.ExecuteCommand && current.commandName == "MaskMenuChanged")
        {
          if (MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance == null)
          {
            Debug.LogError((object) "Mask menu has no instance");
            return mask;
          }
          if (MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_ControlID == controlID)
          {
            if (!MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_DoNothing)
            {
              if (MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_ClearAll)
              {
                mask = 0;
                changedFlags = -1;
                changedToValue = false;
              }
              else if (MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_SetAll)
              {
                mask = -1;
                changedFlags = -1;
                changedToValue = true;
              }
              else
              {
                mask ^= MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_Mask;
                changedFlags = MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_Mask;
                changedToValue = (mask & MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_Mask) != 0;
              }
              GUI.changed = true;
            }
            MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_DoNothing = false;
            MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_ClearAll = false;
            MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance.m_SetAll = false;
            MaskFieldGUIDeprecated.MaskCallbackInfo.m_Instance = (MaskFieldGUIDeprecated.MaskCallbackInfo) null;
            current.Use();
          }
        }
        return mask;
      }

      internal void SetMaskValueDelegate(object userData, string[] options, int selected)
      {
        switch (selected)
        {
          case 0:
            this.m_ClearAll = true;
            break;
          case 1:
            this.m_SetAll = true;
            break;
          default:
            this.m_Mask = ((int[]) userData)[selected - 2];
            break;
        }
        if (!(bool) ((UnityEngine.Object) this.m_SourceView))
          return;
        this.m_SourceView.SendEvent(EditorGUIUtility.CommandEvent("MaskMenuChanged"));
      }
    }
  }
}
