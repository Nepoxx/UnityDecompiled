// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderInspectorPlatformsPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ShaderInspectorPlatformsPopup : PopupWindowContent
  {
    internal static readonly string[] s_PlatformModes = new string[4]{ "Current graphics device", "Current build platform", "All platforms", "Custom:" };
    private static int s_CurrentMode = -1;
    private static int s_CurrentPlatformMask = -1;
    private static int s_CurrentVariantStripping = -1;
    private static string[] s_ShaderPlatformNames;
    private static int[] s_ShaderPlatformIndices;
    private const float kFrameWidth = 1f;
    private const float kSeparatorHeight = 6f;
    private readonly Shader m_Shader;

    public ShaderInspectorPlatformsPopup(Shader shader)
    {
      this.m_Shader = shader;
      ShaderInspectorPlatformsPopup.InitializeShaderPlatforms();
    }

    public static int currentMode
    {
      get
      {
        if (ShaderInspectorPlatformsPopup.s_CurrentMode < 0)
          ShaderInspectorPlatformsPopup.s_CurrentMode = EditorPrefs.GetInt("ShaderInspectorPlatformMode", 1);
        return ShaderInspectorPlatformsPopup.s_CurrentMode;
      }
      set
      {
        ShaderInspectorPlatformsPopup.s_CurrentMode = value;
        EditorPrefs.SetInt("ShaderInspectorPlatformMode", value);
      }
    }

    public static int currentPlatformMask
    {
      get
      {
        if (ShaderInspectorPlatformsPopup.s_CurrentPlatformMask < 0)
          ShaderInspectorPlatformsPopup.s_CurrentPlatformMask = EditorPrefs.GetInt("ShaderInspectorPlatformMask", 1048575);
        return ShaderInspectorPlatformsPopup.s_CurrentPlatformMask;
      }
      set
      {
        ShaderInspectorPlatformsPopup.s_CurrentPlatformMask = value;
        EditorPrefs.SetInt("ShaderInspectorPlatformMask", value);
      }
    }

    public static int currentVariantStripping
    {
      get
      {
        if (ShaderInspectorPlatformsPopup.s_CurrentVariantStripping < 0)
          ShaderInspectorPlatformsPopup.s_CurrentVariantStripping = EditorPrefs.GetInt("ShaderInspectorVariantStripping", 1);
        return ShaderInspectorPlatformsPopup.s_CurrentVariantStripping;
      }
      set
      {
        ShaderInspectorPlatformsPopup.s_CurrentVariantStripping = value;
        EditorPrefs.SetInt("ShaderInspectorVariantStripping", value);
      }
    }

    private static void InitializeShaderPlatforms()
    {
      if (ShaderInspectorPlatformsPopup.s_ShaderPlatformNames != null)
        return;
      int compilerPlatforms = ShaderUtil.GetAvailableShaderCompilerPlatforms();
      List<string> stringList = new List<string>();
      List<int> intList = new List<int>();
      for (int index = 0; index < 32; ++index)
      {
        if ((compilerPlatforms & 1 << index) != 0)
        {
          stringList.Add(((ShaderUtil.ShaderCompilerPlatformType) index).ToString());
          intList.Add(index);
        }
      }
      ShaderInspectorPlatformsPopup.s_ShaderPlatformNames = stringList.ToArray();
      ShaderInspectorPlatformsPopup.s_ShaderPlatformIndices = intList.ToArray();
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(210f, (float) ((double) (ShaderInspectorPlatformsPopup.s_PlatformModes.Length + ShaderInspectorPlatformsPopup.s_ShaderPlatformNames.Length + 2) * 16.0 + 18.0) + 2f);
    }

    public override void OnGUI(Rect rect)
    {
      if ((Object) this.m_Shader == (Object) null || Event.current.type == EventType.Layout)
        return;
      this.Draw(this.editorWindow, rect.width);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.editorWindow.Close();
      GUIUtility.ExitGUI();
    }

    private void DrawSeparator(ref Rect rect)
    {
      GUI.Label(new Rect(rect.x + 5f, rect.y + 3f, rect.width - 10f, 3f), GUIContent.none, ShaderInspectorPlatformsPopup.Styles.separator);
      rect.y += 6f;
    }

    private void Draw(EditorWindow caller, float listElementWidth)
    {
      Rect rect = new Rect(0.0f, 0.0f, listElementWidth, 16f);
      for (int index = 0; index < ShaderInspectorPlatformsPopup.s_PlatformModes.Length; ++index)
      {
        this.DoOneMode(rect, index);
        rect.y += 16f;
      }
      Color color = GUI.color;
      if (ShaderInspectorPlatformsPopup.currentMode != 3)
        GUI.color *= new Color(1f, 1f, 1f, 0.7f);
      rect.xMin += 16f;
      for (int index = 0; index < ShaderInspectorPlatformsPopup.s_ShaderPlatformNames.Length; ++index)
      {
        this.DoCustomPlatformBit(rect, index);
        rect.y += 16f;
      }
      GUI.color = color;
      rect.xMin -= 16f;
      this.DrawSeparator(ref rect);
      this.DoShaderVariants(caller, ref rect);
    }

    private void DoOneMode(Rect rect, int index)
    {
      EditorGUI.BeginChangeCheck();
      GUI.Toggle(rect, ShaderInspectorPlatformsPopup.currentMode == index, EditorGUIUtility.TempContent(ShaderInspectorPlatformsPopup.s_PlatformModes[index]), ShaderInspectorPlatformsPopup.Styles.menuItem);
      if (!EditorGUI.EndChangeCheck())
        return;
      ShaderInspectorPlatformsPopup.currentMode = index;
    }

    private void DoCustomPlatformBit(Rect rect, int index)
    {
      EditorGUI.BeginChangeCheck();
      int num = 1 << ShaderInspectorPlatformsPopup.s_ShaderPlatformIndices[index];
      bool flag1 = (ShaderInspectorPlatformsPopup.currentPlatformMask & num) != 0;
      bool flag2 = GUI.Toggle(rect, flag1, EditorGUIUtility.TempContent(ShaderInspectorPlatformsPopup.s_ShaderPlatformNames[index]), ShaderInspectorPlatformsPopup.Styles.menuItem);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (flag2)
        ShaderInspectorPlatformsPopup.currentPlatformMask |= num;
      else
        ShaderInspectorPlatformsPopup.currentPlatformMask &= ~num;
      ShaderInspectorPlatformsPopup.currentMode = 3;
    }

    private static string FormatCount(ulong count)
    {
      if (count > 1000000000UL)
        return ((double) count / 1000000000.0).ToString("f2") + "B";
      if (count > 1000000UL)
        return ((double) count / 1000000.0).ToString("f2") + "M";
      if (count > 1000UL)
        return ((double) count / 1000.0).ToString("f2") + "k";
      return count.ToString();
    }

    private void DoShaderVariants(EditorWindow caller, ref Rect drawPos)
    {
      EditorGUI.BeginChangeCheck();
      bool usedBySceneOnly = GUI.Toggle(drawPos, ShaderInspectorPlatformsPopup.currentVariantStripping == 1, EditorGUIUtility.TempContent("Skip unused shader_features"), ShaderInspectorPlatformsPopup.Styles.menuItem);
      drawPos.y += 16f;
      if (EditorGUI.EndChangeCheck())
        ShaderInspectorPlatformsPopup.currentVariantStripping = !usedBySceneOnly ? 0 : 1;
      drawPos.y += 6f;
      string text = ShaderInspectorPlatformsPopup.FormatCount(ShaderUtil.GetVariantCount(this.m_Shader, usedBySceneOnly)) + (!usedBySceneOnly ? " variants total" : " variants included");
      Rect position = drawPos;
      position.x += (float) ShaderInspectorPlatformsPopup.Styles.menuItem.padding.left;
      position.width -= (float) (ShaderInspectorPlatformsPopup.Styles.menuItem.padding.left + 4);
      GUI.Label(position, text);
      position.xMin = position.xMax - 40f;
      if (!GUI.Button(position, "Show", EditorStyles.miniButton))
        return;
      ShaderUtil.OpenShaderCombinations(this.m_Shader, usedBySceneOnly);
      caller.Close();
      GUIUtility.ExitGUI();
    }

    private class Styles
    {
      public static readonly GUIStyle menuItem = (GUIStyle) "MenuItem";
      public static readonly GUIStyle separator = (GUIStyle) "sv_iconselector_sep";
    }
  }
}
