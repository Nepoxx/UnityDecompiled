// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SpriteRenderer))]
  internal class SpriteRendererEditor : RendererEditorBase
  {
    private SerializedProperty m_Sprite;
    private SerializedProperty m_Color;
    private SerializedProperty m_Material;
    private SerializedProperty m_FlipX;
    private SerializedProperty m_FlipY;
    private SerializedProperty m_DrawMode;
    private SerializedProperty m_SpriteTileMode;
    private SerializedProperty m_AdaptiveModeThreshold;
    private SerializedProperty m_Size;
    private AnimBool m_ShowDrawMode;
    private AnimBool m_ShowTileMode;
    private AnimBool m_ShowAdaptiveThreshold;
    private SerializedProperty m_MaskInteraction;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Sprite = this.serializedObject.FindProperty("m_Sprite");
      this.m_Color = this.serializedObject.FindProperty("m_Color");
      this.m_FlipX = this.serializedObject.FindProperty("m_FlipX");
      this.m_FlipY = this.serializedObject.FindProperty("m_FlipY");
      this.m_Material = this.serializedObject.FindProperty("m_Materials.Array");
      this.m_DrawMode = this.serializedObject.FindProperty("m_DrawMode");
      this.m_Size = this.serializedObject.FindProperty("m_Size");
      this.m_SpriteTileMode = this.serializedObject.FindProperty("m_SpriteTileMode");
      this.m_AdaptiveModeThreshold = this.serializedObject.FindProperty("m_AdaptiveModeThreshold");
      this.m_ShowDrawMode = new AnimBool(this.ShouldShowDrawMode());
      this.m_ShowTileMode = new AnimBool(this.ShouldShowTileMode());
      this.m_ShowAdaptiveThreshold = new AnimBool(this.ShouldShowAdaptiveThreshold());
      this.m_ShowDrawMode.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowTileMode.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowAdaptiveThreshold.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_MaskInteraction = this.serializedObject.FindProperty("m_MaskInteraction");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Sprite, SpriteRendererEditor.Contents.spriteLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Color, SpriteRendererEditor.Contents.colorLabel, true, new GUILayoutOption[0]);
      this.FlipToggles();
      if (this.m_Material.arraySize == 0)
        this.m_Material.InsertArrayElementAtIndex(0);
      Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f);
      EditorGUI.showMixedValue = this.m_Material.hasMultipleDifferentValues;
      Object objectReferenceValue = this.m_Material.GetArrayElementAtIndex(0).objectReferenceValue;
      Object @object = EditorGUI.ObjectField(rect, SpriteRendererEditor.Contents.materialLabel, objectReferenceValue, typeof (Material), false);
      if (@object != objectReferenceValue)
        this.m_Material.GetArrayElementAtIndex(0).objectReferenceValue = @object;
      EditorGUI.showMixedValue = false;
      EditorGUILayout.PropertyField(this.m_DrawMode, SpriteRendererEditor.Contents.drawModeLabel, new GUILayoutOption[0]);
      this.m_ShowDrawMode.target = this.ShouldShowDrawMode();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowDrawMode.faded))
      {
        string notFullRectWarning = this.GetSpriteNotFullRectWarning();
        if (notFullRectWarning != null)
          EditorGUILayout.HelpBox(notFullRectWarning, MessageType.Warning);
        ++EditorGUI.indentLevel;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(SpriteRendererEditor.Contents.sizeLabel);
        EditorGUI.showMixedValue = this.m_Size.hasMultipleDifferentValues;
        this.FloatFieldLabelAbove(SpriteRendererEditor.Contents.widthLabel, this.m_Size.FindPropertyRelative("x"));
        this.FloatFieldLabelAbove(SpriteRendererEditor.Contents.heightLabel, this.m_Size.FindPropertyRelative("y"));
        EditorGUI.showMixedValue = false;
        EditorGUILayout.EndHorizontal();
        this.m_ShowTileMode.target = this.ShouldShowTileMode();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowTileMode.faded))
        {
          EditorGUILayout.PropertyField(this.m_SpriteTileMode, SpriteRendererEditor.Contents.fullTileLabel, new GUILayoutOption[0]);
          this.m_ShowAdaptiveThreshold.target = this.ShouldShowAdaptiveThreshold();
          if (EditorGUILayout.BeginFadeGroup(this.m_ShowAdaptiveThreshold.faded))
          {
            ++EditorGUI.indentLevel;
            EditorGUILayout.Slider(this.m_AdaptiveModeThreshold, 0.0f, 1f, SpriteRendererEditor.Contents.fullTileThresholdLabel, new GUILayoutOption[0]);
            --EditorGUI.indentLevel;
          }
          EditorGUILayout.EndFadeGroup();
        }
        EditorGUILayout.EndFadeGroup();
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      this.RenderSortingLayerFields();
      EditorGUILayout.PropertyField(this.m_MaskInteraction);
      this.CheckForErrors();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void FloatFieldLabelAbove(GUIContent contentLabel, SerializedProperty sp)
    {
      EditorGUILayout.BeginVertical();
      Rect rect1 = GUILayoutUtility.GetRect(contentLabel, EditorStyles.label);
      GUIContent label = EditorGUI.BeginProperty(rect1, contentLabel, sp);
      int controlId = GUIUtility.GetControlID(SpriteRendererEditor.Contents.sizeFieldHash, FocusType.Keyboard, rect1);
      EditorGUI.HandlePrefixLabel(rect1, rect1, label, controlId);
      Rect rect2 = GUILayoutUtility.GetRect(contentLabel, EditorStyles.textField);
      EditorGUI.BeginChangeCheck();
      float num = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, rect2, rect1, controlId, sp.floatValue, EditorGUI.kFloatFieldFormatString, EditorStyles.textField, true);
      if (EditorGUI.EndChangeCheck())
        sp.floatValue = num;
      EditorGUI.EndProperty();
      EditorGUILayout.EndVertical();
    }

    private string GetSpriteNotFullRectWarning()
    {
      foreach (Object target in this.targets)
      {
        if (!(target as SpriteRenderer).shouldSupportTiling)
          return this.targets.Length != 1 ? SpriteRendererEditor.Contents.notFullRectMultiEditWarningLabel.text : SpriteRendererEditor.Contents.notFullRectWarningLabel.text;
      }
      return (string) null;
    }

    private bool ShouldShowDrawMode()
    {
      return this.m_DrawMode.intValue != 0 && !this.m_DrawMode.hasMultipleDifferentValues;
    }

    private bool ShouldShowAdaptiveThreshold()
    {
      return this.m_SpriteTileMode.intValue == 1 && !this.m_SpriteTileMode.hasMultipleDifferentValues;
    }

    private bool ShouldShowTileMode()
    {
      return this.m_DrawMode.intValue == 2 && !this.m_DrawMode.hasMultipleDifferentValues;
    }

    private void FlipToggles()
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(SpriteRendererEditor.Contents.flipToggleHash, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, SpriteRendererEditor.Contents.flipLabel);
      r.width = 30f;
      this.FlipToggle(r, SpriteRendererEditor.Contents.flipXLabel, this.m_FlipX);
      r.x += 30f;
      this.FlipToggle(r, SpriteRendererEditor.Contents.flipYLabel, this.m_FlipY);
      GUILayout.EndHorizontal();
    }

    private void FlipToggle(Rect r, GUIContent label, SerializedProperty property)
    {
      EditorGUI.BeginProperty(r, label, property);
      bool boolValue = property.boolValue;
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      bool flag = EditorGUI.ToggleLeft(r, label, boolValue);
      EditorGUI.indentLevel = indentLevel;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Edit Constraints");
        property.boolValue = flag;
      }
      EditorGUI.EndProperty();
    }

    private void CheckForErrors()
    {
      if (this.IsMaterialTextureAtlasConflict())
        SpriteRendererEditor.ShowError("Material has CanUseSpriteAtlas=False tag. Sprite texture has atlasHint set. Rendering artifacts possible.");
      bool tiled;
      if (!this.DoesMaterialHaveSpriteTexture(out tiled))
        SpriteRendererEditor.ShowError("Material does not have a _MainTex texture property. It is required for SpriteRenderer.");
      else if (tiled)
        SpriteRendererEditor.ShowError("Material texture property _MainTex has offset/scale set. It is incompatible with SpriteRenderer.");
    }

    private bool IsMaterialTextureAtlasConflict()
    {
      Material sharedMaterial = (this.target as SpriteRenderer).sharedMaterial;
      if ((Object) sharedMaterial == (Object) null || !(sharedMaterial.GetTag("CanUseSpriteAtlas", false).ToLower() == "false"))
        return false;
      TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object) (this.m_Sprite.objectReferenceValue as Sprite))) as TextureImporter;
      return (Object) atPath != (Object) null && atPath.spritePackingTag != null && atPath.spritePackingTag.Length > 0;
    }

    private bool DoesMaterialHaveSpriteTexture(out bool tiled)
    {
      tiled = false;
      Material sharedMaterial = (this.target as SpriteRenderer).sharedMaterial;
      if ((Object) sharedMaterial == (Object) null)
        return true;
      if (sharedMaterial.HasProperty("_MainTex"))
      {
        Vector2 textureOffset = sharedMaterial.GetTextureOffset("_MainTex");
        Vector2 textureScale = sharedMaterial.GetTextureScale("_MainTex");
        if ((double) textureOffset.x != 0.0 || (double) textureOffset.y != 0.0 || ((double) textureScale.x != 1.0 || (double) textureScale.y != 1.0))
          tiled = true;
      }
      return sharedMaterial.HasProperty("_MainTex");
    }

    private static void ShowError(string error)
    {
      GUIContent content = new GUIContent(error) { image = (Texture) SpriteRendererEditor.Contents.warningIcon };
      GUILayout.Space(5f);
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label(content, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
    }

    private static class Contents
    {
      public static readonly GUIContent flipLabel = EditorGUIUtility.TextContent("Flip|Sprite flipping");
      public static readonly GUIContent flipXLabel = EditorGUIUtility.TextContent("X|Sprite horizontal flipping");
      public static readonly GUIContent flipYLabel = EditorGUIUtility.TextContent("Y|Sprite vertical flipping");
      public static readonly int flipToggleHash = "FlipToggleHash".GetHashCode();
      public static readonly GUIContent fullTileLabel = EditorGUIUtility.TextContent("Tile Mode|Specify the 9 slice tiling behaviour");
      public static readonly GUIContent fullTileThresholdLabel = EditorGUIUtility.TextContent("Stretch Value|This value defines how much the center portion will stretch before it tiles.");
      public static readonly GUIContent drawModeLabel = EditorGUIUtility.TextContent("Draw Mode|Specify the draw mode for the sprite");
      public static readonly GUIContent widthLabel = EditorGUIUtility.TextContent("Width|The width dimension value for the sprite");
      public static readonly GUIContent heightLabel = EditorGUIUtility.TextContent("Height|The height dimension value for the sprite");
      public static readonly GUIContent sizeLabel = EditorGUIUtility.TextContent("Size|The rendering dimension for the sprite");
      public static readonly GUIContent notFullRectWarningLabel = EditorGUIUtility.TextContent("Sprite Tiling might not appear correctly because the Sprite used is not generated with Full Rect or Sprite Mode is set to Polygon mode. To fix this, change the Mesh Type in the Sprite's import setting to Full Rect and Sprite Mode is either Single or Multiple");
      public static readonly GUIContent notFullRectMultiEditWarningLabel = EditorGUIUtility.TextContent("Sprite Tiling might not appear correctly because some of the Sprites used are not generated with Full Rect. To fix this, change the Mesh Type in the Sprite's import setting to Full Rect");
      public static readonly int sizeFieldHash = "SpriteRendererSizeField".GetHashCode();
      public static readonly GUIContent materialLabel = EditorGUIUtility.TextContent("Material|Material to be used by SpriteRenderer");
      public static readonly GUIContent spriteLabel = EditorGUIUtility.TextContent("Sprite|The Sprite to render");
      public static readonly GUIContent colorLabel = EditorGUIUtility.TextContent("Color|Rendering color for the Sprite graphic");
      public static readonly Texture2D warningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
    }
  }
}
