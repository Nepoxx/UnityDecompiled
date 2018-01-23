// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Shader))]
  internal class ShaderInspector : Editor
  {
    private static readonly string[] kPropertyTypes = new string[5]{ "Color: ", "Vector: ", "Float: ", "Range: ", "Texture: " };
    private static readonly string[] kTextureTypes = new string[7]{ "No Texture?: ", "Any texture: ", "2D: ", "3D: ", "Cube: ", "2DArray: ", "CubeArray: " };
    private static readonly int kErrorViewHash = "ShaderErrorView".GetHashCode();
    private Vector2 m_ScrollPosition = Vector2.zero;
    private const float kSpace = 5f;

    public virtual void OnEnable()
    {
      ShaderUtil.FetchCachedErrors(this.target as Shader);
    }

    private static string GetPropertyType(Shader s, int index)
    {
      ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(s, index);
      if (propertyType == ShaderUtil.ShaderPropertyType.TexEnv)
        return ShaderInspector.kTextureTypes[(int) ShaderUtil.GetTexDim(s, index)];
      return ShaderInspector.kPropertyTypes[(int) propertyType];
    }

    public override void OnInspectorGUI()
    {
      Shader target = this.target as Shader;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      GUI.enabled = true;
      EditorGUI.indentLevel = 0;
      this.ShowShaderCodeArea(target);
      if (!target.isSupported)
        return;
      EditorGUILayout.LabelField("Cast shadows", !ShaderUtil.HasShadowCasterPass(target) ? "no" : "yes", new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Render queue", ShaderUtil.GetRenderQueue(target).ToString((IFormatProvider) CultureInfo.InvariantCulture), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("LOD", ShaderUtil.GetLOD(target).ToString((IFormatProvider) CultureInfo.InvariantCulture), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Ignore projector", !ShaderUtil.DoesIgnoreProjector(target) ? "no" : "yes", new GUILayoutOption[0]);
      string label2;
      switch (target.disableBatching)
      {
        case DisableBatchingType.False:
          label2 = "no";
          break;
        case DisableBatchingType.True:
          label2 = "yes";
          break;
        case DisableBatchingType.WhenLODFading:
          label2 = "when LOD fading is on";
          break;
        default:
          label2 = "unknown";
          break;
      }
      EditorGUILayout.LabelField("Disable batching", label2, new GUILayoutOption[0]);
      ShaderInspector.ShowShaderProperties(target);
    }

    private void ShowShaderCodeArea(Shader s)
    {
      ShaderInspector.ShowSurfaceShaderButton(s);
      ShaderInspector.ShowFixedFunctionShaderButton(s);
      this.ShowCompiledCodeButton(s);
      this.ShowShaderErrors(s);
    }

    private static void ShowShaderProperties(Shader s)
    {
      GUILayout.Space(5f);
      GUILayout.Label("Properties:", EditorStyles.boldLabel, new GUILayoutOption[0]);
      int propertyCount = ShaderUtil.GetPropertyCount(s);
      for (int index = 0; index < propertyCount; ++index)
        EditorGUILayout.LabelField(ShaderUtil.GetPropertyName(s, index), ShaderInspector.GetPropertyType(s, index) + ShaderUtil.GetPropertyDescription(s, index), new GUILayoutOption[0]);
    }

    internal static void ShaderErrorListUI(UnityEngine.Object shader, ShaderError[] errors, ref Vector2 scrollPosition)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ShaderInspector.\u003CShaderErrorListUI\u003Ec__AnonStorey0 listUiCAnonStorey0 = new ShaderInspector.\u003CShaderErrorListUI\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      listUiCAnonStorey0.errors = errors;
      // ISSUE: reference to a compiler-generated field
      int length = listUiCAnonStorey0.errors.Length;
      GUILayout.Space(5f);
      GUILayout.Label(string.Format("Errors ({0}):", (object) length), EditorStyles.boldLabel, new GUILayoutOption[0]);
      int controlId = GUIUtility.GetControlID(ShaderInspector.kErrorViewHash, FocusType.Passive);
      float minHeight = Mathf.Min((float) ((double) length * 20.0 + 40.0), 150f);
      scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUISkin.current.box, GUILayout.MinHeight(minHeight));
      EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
      float height = ShaderInspector.Styles.messageStyle.CalcHeight(EditorGUIUtility.TempContent((Texture) ShaderInspector.Styles.errorIcon), 100f);
      Event current = Event.current;
      for (int index = 0; index < length; ++index)
      {
        Rect controlRect = EditorGUILayout.GetControlRect(false, height, new GUILayoutOption[0]);
        // ISSUE: reference to a compiler-generated field
        string message = listUiCAnonStorey0.errors[index].message;
        // ISSUE: reference to a compiler-generated field
        string platform = listUiCAnonStorey0.errors[index].platform;
        // ISSUE: reference to a compiler-generated field
        bool flag = listUiCAnonStorey0.errors[index].warning != 0;
        // ISSUE: reference to a compiler-generated field
        string pathNameComponent = FileUtil.GetLastPathNameComponent(listUiCAnonStorey0.errors[index].file);
        // ISSUE: reference to a compiler-generated field
        int line = listUiCAnonStorey0.errors[index].line;
        if (current.type == EventType.MouseDown && current.button == 0 && controlRect.Contains(current.mousePosition))
        {
          GUIUtility.keyboardControl = controlId;
          if (current.clickCount == 2)
          {
            // ISSUE: reference to a compiler-generated field
            string file = listUiCAnonStorey0.errors[index].file;
            UnityEngine.Object @object = !string.IsNullOrEmpty(file) ? AssetDatabase.LoadMainAssetAtPath(file) : (UnityEngine.Object) null;
            if (@object == (UnityEngine.Object) null && Path.IsPathRooted(file))
            {
              ShaderUtil.OpenSystemShaderIncludeError(file, line);
            }
            else
            {
              UnityEngine.Object target = @object;
              if ((object) target == null)
                target = shader;
              AssetDatabase.OpenAsset(target, line);
            }
            GUIUtility.ExitGUI();
          }
          current.Use();
        }
        if (current.type == EventType.ContextClick && controlRect.Contains(current.mousePosition))
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ShaderInspector.\u003CShaderErrorListUI\u003Ec__AnonStorey1 listUiCAnonStorey1 = new ShaderInspector.\u003CShaderErrorListUI\u003Ec__AnonStorey1();
          // ISSUE: reference to a compiler-generated field
          listUiCAnonStorey1.\u003C\u003Ef__ref\u00240 = listUiCAnonStorey0;
          current.Use();
          GenericMenu genericMenu = new GenericMenu();
          // ISSUE: reference to a compiler-generated field
          listUiCAnonStorey1.errorIndex = index;
          // ISSUE: reference to a compiler-generated method
          genericMenu.AddItem(new GUIContent("Copy error text"), false, new GenericMenu.MenuFunction(listUiCAnonStorey1.\u003C\u003Em__0));
          genericMenu.ShowAsContext();
        }
        if (current.type == EventType.Repaint && (index & 1) == 0)
          ShaderInspector.Styles.evenBackground.Draw(controlRect, false, false, false, false);
        Rect position1 = controlRect;
        position1.xMin = position1.xMax;
        if (line > 0)
        {
          GUIContent content = !string.IsNullOrEmpty(pathNameComponent) ? EditorGUIUtility.TempContent(pathNameComponent + ":" + line.ToString((IFormatProvider) CultureInfo.InvariantCulture)) : EditorGUIUtility.TempContent(line.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          Vector2 vector2 = EditorStyles.miniLabel.CalcSize(content);
          position1.xMin -= vector2.x;
          GUI.Label(position1, content, EditorStyles.miniLabel);
          position1.xMin -= 2f;
          if ((double) position1.width < 30.0)
            position1.xMin = position1.xMax - 30f;
        }
        Rect position2 = position1;
        position2.width = 0.0f;
        if (platform.Length > 0)
        {
          GUIContent content = EditorGUIUtility.TempContent(platform);
          Vector2 vector2 = EditorStyles.miniLabel.CalcSize(content);
          position2.xMin -= vector2.x;
          Color contentColor = GUI.contentColor;
          GUI.contentColor = new Color(1f, 1f, 1f, 0.5f);
          GUI.Label(position2, content, EditorStyles.miniLabel);
          GUI.contentColor = contentColor;
          position2.xMin -= 2f;
        }
        Rect position3 = controlRect;
        position3.xMax = position2.xMin;
        GUI.Label(position3, EditorGUIUtility.TempContent(message, !flag ? (Texture) ShaderInspector.Styles.errorIcon : (Texture) ShaderInspector.Styles.warningIcon), ShaderInspector.Styles.messageStyle);
      }
      EditorGUIUtility.SetIconSize(Vector2.zero);
      GUILayout.EndScrollView();
    }

    private void ShowShaderErrors(Shader s)
    {
      if (ShaderUtil.GetShaderErrorCount(s) < 1)
        return;
      ShaderInspector.ShaderErrorListUI((UnityEngine.Object) s, ShaderUtil.GetShaderErrors(s), ref this.m_ScrollPosition);
    }

    private void ShowCompiledCodeButton(Shader s)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel("Compiled code", EditorStyles.miniButton);
      if (ShaderUtil.HasShaderSnippets(s) || ShaderUtil.HasSurfaceShaders(s) || ShaderUtil.HasFixedFunctionShaders(s))
      {
        GUIContent showCurrent = ShaderInspector.Styles.showCurrent;
        Rect rect = GUILayoutUtility.GetRect(showCurrent, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) });
        if (EditorGUI.DropdownButton(new Rect(rect.xMax - 16f, rect.y, 16f, rect.height), GUIContent.none, FocusType.Passive, GUIStyle.none))
        {
          PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new ShaderInspectorPlatformsPopup(s));
          GUIUtility.ExitGUI();
        }
        if (GUI.Button(rect, showCurrent, EditorStyles.miniButton))
        {
          ShaderUtil.OpenCompiledShader(s, ShaderInspectorPlatformsPopup.currentMode, ShaderInspectorPlatformsPopup.currentPlatformMask, ShaderInspectorPlatformsPopup.currentVariantStripping == 0);
          GUIUtility.ExitGUI();
        }
      }
      else
        GUILayout.Button("none (precompiled shader)", GUI.skin.label, new GUILayoutOption[0]);
      EditorGUILayout.EndHorizontal();
    }

    private static void ShowSurfaceShaderButton(Shader s)
    {
      bool flag = ShaderUtil.HasSurfaceShaders(s);
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel("Surface shader", EditorStyles.miniButton);
      if (flag)
      {
        if (!((UnityEngine.Object) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) s)) == (UnityEngine.Object) null))
        {
          if (GUILayout.Button(ShaderInspector.Styles.showSurface, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
          {
            ShaderUtil.OpenParsedSurfaceShader(s);
            GUIUtility.ExitGUI();
          }
        }
        else
          GUILayout.Button(ShaderInspector.Styles.builtinShader, GUI.skin.label, new GUILayoutOption[0]);
      }
      else
        GUILayout.Button(ShaderInspector.Styles.no, GUI.skin.label, new GUILayoutOption[0]);
      EditorGUILayout.EndHorizontal();
    }

    private static void ShowFixedFunctionShaderButton(Shader s)
    {
      bool flag = ShaderUtil.HasFixedFunctionShaders(s);
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel("Fixed function", EditorStyles.miniButton);
      if (flag)
      {
        if (!((UnityEngine.Object) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) s)) == (UnityEngine.Object) null))
        {
          if (GUILayout.Button(ShaderInspector.Styles.showFF, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
          {
            ShaderUtil.OpenGeneratedFixedFunctionShader(s);
            GUIUtility.ExitGUI();
          }
        }
        else
          GUILayout.Button(ShaderInspector.Styles.builtinShader, GUI.skin.label, new GUILayoutOption[0]);
      }
      else
        GUILayout.Button(ShaderInspector.Styles.no, GUI.skin.label, new GUILayoutOption[0]);
      EditorGUILayout.EndHorizontal();
    }

    internal class Styles
    {
      public static Texture2D errorIcon = EditorGUIUtility.LoadIcon("console.erroricon.sml");
      public static Texture2D warningIcon = EditorGUIUtility.LoadIcon("console.warnicon.sml");
      public static GUIContent showSurface = EditorGUIUtility.TextContent("Show generated code|Show generated code of a surface shader");
      public static GUIContent showFF = EditorGUIUtility.TextContent("Show generated code|Show generated code of a fixed function shader");
      public static GUIContent showCurrent = new GUIContent("Compile and show code | ▾");
      public static GUIStyle messageStyle = (GUIStyle) "CN StatusInfo";
      public static GUIStyle evenBackground = (GUIStyle) "CN EntryBackEven";
      public static GUIContent no = EditorGUIUtility.TextContent(nameof (no));
      public static GUIContent builtinShader = EditorGUIUtility.TextContent("Built-in shader");
    }
  }
}
