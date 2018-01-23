// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaskFieldGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class MaskFieldGUI
  {
    private static readonly List<string[]> s_OptionNames = new List<string[]>();
    private static readonly List<int[]> s_OptionValues = new List<int[]>();
    private static readonly List<int[]> s_SelectedOptions = new List<int[]>();
    private static readonly HashSet<int> s_SelectedOptionsSet = new HashSet<int>();

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, GUIStyle style)
    {
      int changedFlags;
      bool changedToValue;
      return MaskFieldGUI.DoMaskField(position, controlID, mask, flagNames, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, int[] flagValues, GUIStyle style)
    {
      int changedFlags;
      bool changedToValue;
      return MaskFieldGUI.DoMaskField(position, controlID, mask, flagNames, flagValues, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, GUIStyle style, out int changedFlags, out bool changedToValue)
    {
      int[] flagValues = new int[flagNames.Length];
      for (int index = 0; index < flagValues.Length; ++index)
        flagValues[index] = 1 << index;
      return MaskFieldGUI.DoMaskField(position, controlID, mask, flagNames, flagValues, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, int[] flagValues, GUIStyle style, out int changedFlags, out bool changedToValue)
    {
      mask = MaskFieldGUI.MaskCallbackInfo.GetSelectedValueForControl(controlID, mask, out changedFlags, out changedToValue);
      string buttonText;
      string[] optionNames;
      int[] optionMaskValues;
      int[] selectedOptions;
      MaskFieldGUI.GetMenuOptions(mask, flagNames, flagValues, out buttonText, out optionNames, out optionMaskValues, out selectedOptions);
      Event current = Event.current;
      if (current.type == EventType.Repaint)
      {
        GUIContent content = !EditorGUI.showMixedValue ? EditorGUIUtility.TempContent(buttonText) : EditorGUI.mixedValueContent;
        style.Draw(position, content, controlID, false);
      }
      else if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlID))
      {
        MaskFieldGUI.MaskCallbackInfo.m_Instance = new MaskFieldGUI.MaskCallbackInfo(controlID);
        current.Use();
        EditorUtility.DisplayCustomMenu(position, optionNames, !EditorGUI.showMixedValue ? selectedOptions : new int[0], new EditorUtility.SelectMenuItemFunction(MaskFieldGUI.MaskCallbackInfo.m_Instance.SetMaskValueDelegate), optionMaskValues.Clone());
        GUIUtility.keyboardControl = controlID;
      }
      return mask;
    }

    private static T[] GetBuffer<T>(List<T[]> pool, int bufferLength)
    {
      for (int count = pool.Count; count <= bufferLength; ++count)
        pool.Add((T[]) null);
      if (pool[bufferLength] == null)
        pool[bufferLength] = new T[bufferLength];
      T[] objArray = pool[bufferLength];
      int index = 0;
      for (int length = objArray.Length; index < length; ++index)
        objArray[index] = default (T);
      return objArray;
    }

    internal static void GetMenuOptions(int mask, string[] flagNames, int[] flagValues, out string buttonText, out string[] optionNames, out int[] optionMaskValues, out int[] selectedOptions)
    {
      bool flag1 = flagValues[0] == 0;
      bool flag2 = flagValues[flagValues.Length - 1] == -1;
      string str1 = !flag1 ? "Nothing" : flagNames[0];
      string str2 = !flag2 ? "Everything" : flagNames[flagValues.Length - 1];
      int bufferLength = flagNames.Length + (!flag1 ? 1 : 0) + (!flag2 ? 1 : 0);
      int num1 = flagNames.Length - (!flag1 ? 0 : 1) - (!flag2 ? 0 : 1);
      int num2 = !flag1 ? 0 : 1;
      int num3 = num2 + num1;
      buttonText = "Mixed ...";
      switch (mask)
      {
        case -1:
          buttonText = str2;
          break;
        case 0:
          buttonText = str1;
          break;
        default:
          for (int index = num2; index < num3; ++index)
          {
            if (mask == flagValues[index])
              buttonText = flagNames[index];
          }
          break;
      }
      optionNames = MaskFieldGUI.GetBuffer<string>(MaskFieldGUI.s_OptionNames, bufferLength);
      optionNames[0] = str1;
      optionNames[1] = str2;
      for (int index1 = num2; index1 < num3; ++index1)
      {
        int index2 = index1 - num2 + 2;
        optionNames[index2] = flagNames[index1];
      }
      int num4 = 0;
      int num5 = 0;
      MaskFieldGUI.s_SelectedOptionsSet.Clear();
      if (mask == 0)
        MaskFieldGUI.s_SelectedOptionsSet.Add(0);
      if (mask == -1)
        MaskFieldGUI.s_SelectedOptionsSet.Add(1);
      for (int index = num2; index < num3; ++index)
      {
        int flagValue = flagValues[index];
        num4 |= flagValue;
        if ((mask & flagValue) == flagValue)
        {
          int num6 = index - num2 + 2;
          MaskFieldGUI.s_SelectedOptionsSet.Add(num6);
          num5 |= flagValue;
        }
      }
      selectedOptions = MaskFieldGUI.GetBuffer<int>(MaskFieldGUI.s_SelectedOptions, MaskFieldGUI.s_SelectedOptionsSet.Count);
      int index3 = 0;
      foreach (int selectedOptions1 in MaskFieldGUI.s_SelectedOptionsSet)
      {
        selectedOptions[index3] = selectedOptions1;
        ++index3;
      }
      optionMaskValues = MaskFieldGUI.GetBuffer<int>(MaskFieldGUI.s_OptionValues, bufferLength);
      optionMaskValues[0] = 0;
      optionMaskValues[1] = -1;
      for (int index1 = num2; index1 < num3; ++index1)
      {
        int index2 = index1 - num2 + 2;
        int flagValue = flagValues[index1];
        int num6 = (num5 & flagValue) != flagValue ? num5 | flagValue : num5 & ~flagValue;
        if (num6 == num4)
          num6 = -1;
        optionMaskValues[index2] = num6;
      }
    }

    private class MaskCallbackInfo
    {
      public static MaskFieldGUI.MaskCallbackInfo m_Instance;
      private const string kMaskMenuChangedMessage = "MaskMenuChanged";
      private readonly int m_ControlID;
      private int m_NewMask;
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
          if (MaskFieldGUI.MaskCallbackInfo.m_Instance == null)
          {
            Debug.LogError((object) "Mask menu has no instance");
            return mask;
          }
          if (MaskFieldGUI.MaskCallbackInfo.m_Instance.m_ControlID == controlID)
          {
            changedFlags = mask ^ MaskFieldGUI.MaskCallbackInfo.m_Instance.m_NewMask;
            changedToValue = (MaskFieldGUI.MaskCallbackInfo.m_Instance.m_NewMask & changedFlags) != 0;
            if (changedFlags != 0)
            {
              mask = MaskFieldGUI.MaskCallbackInfo.m_Instance.m_NewMask;
              GUI.changed = true;
            }
            MaskFieldGUI.MaskCallbackInfo.m_Instance = (MaskFieldGUI.MaskCallbackInfo) null;
            current.Use();
          }
        }
        return mask;
      }

      internal void SetMaskValueDelegate(object userData, string[] options, int selected)
      {
        this.m_NewMask = ((int[]) userData)[selected];
        if (!(bool) ((Object) this.m_SourceView))
          return;
        this.m_SourceView.SendEvent(EditorGUIUtility.CommandEvent("MaskMenuChanged"));
      }
    }
  }
}
