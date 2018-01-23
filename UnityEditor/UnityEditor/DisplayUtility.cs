// Decompiled with JetBrains decompiler
// Type: UnityEditor.DisplayUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Modules;
using UnityEngine;

namespace UnityEditor
{
  internal class DisplayUtility
  {
    private static string s_DisplayStr = "Display {0}";
    private static GUIContent[] s_GenericDisplayNames = new GUIContent[8]{ EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 1)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 2)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 3)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 4)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 5)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 6)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 7)), EditorGUIUtility.TextContent(string.Format(DisplayUtility.s_DisplayStr, (object) 8)) };
    private static readonly int[] s_DisplayIndices = new int[8]{ 0, 1, 2, 3, 4, 5, 6, 7 };

    public static GUIContent[] GetGenericDisplayNames()
    {
      return DisplayUtility.s_GenericDisplayNames;
    }

    public static int[] GetDisplayIndices()
    {
      return DisplayUtility.s_DisplayIndices;
    }

    public static GUIContent[] GetDisplayNames()
    {
      GUIContent[] displayNames = ModuleManager.GetDisplayNames(EditorUserBuildSettings.activeBuildTarget.ToString());
      return displayNames == null ? DisplayUtility.s_GenericDisplayNames : displayNames;
    }
  }
}
