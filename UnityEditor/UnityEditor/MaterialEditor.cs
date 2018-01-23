// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The Unity Material Editor.</para>
  /// </summary>
  [CustomEditor(typeof (Material))]
  [CanEditMultipleObjects]
  public class MaterialEditor : Editor
  {
    private static readonly List<MaterialEditor> s_MaterialEditors = new List<MaterialEditor>(4);
    private static int s_ControlHash = "EditorTextField".GetHashCode();
    private static readonly GUIContent s_TilingText = new GUIContent("Tiling");
    private static readonly GUIContent s_OffsetText = new GUIContent("Offset");
    private static Stack<MaterialEditor.AnimatedCheckData> s_AnimatedCheckStack = new Stack<MaterialEditor.AnimatedCheckData>();
    private static readonly Mesh[] s_Meshes = new Mesh[5];
    private static readonly GUIContent[] s_MeshIcons = new GUIContent[5];
    private static readonly GUIContent[] s_LightIcons = new GUIContent[2];
    private static readonly GUIContent[] s_TimeIcons = new GUIContent[2];
    private Vector2 m_PreviewDir = new Vector2(0.0f, -20f);
    private int m_LightMode = 1;
    private MaterialEditor.ReflectionProbePicker m_ReflectionProbePicker = new MaterialEditor.ReflectionProbePicker();
    private bool m_IsVisible;
    private bool m_CheckSetup;
    private const float kSpacingUnderTexture = 6f;
    private const float kMiniWarningMessageHeight = 27f;
    private MaterialPropertyBlock m_PropertyBlock;
    private Shader m_Shader;
    private SerializedProperty m_EnableInstancing;
    private SerializedProperty m_DoubleSidedGI;
    private string m_InfoMessage;
    private int m_SelectedMesh;
    private int m_TimeUpdate;
    private ShaderGUI m_CustomShaderGUI;
    private string m_CustomEditorClassName;
    private bool m_InsidePropertiesGUI;
    private Renderer[] m_RenderersForAnimationMode;
    internal static MaterialEditor.MaterialPropertyCallbackFunction contextualPropertyMenu;
    private TextureDimension m_DesiredTexdim;
    private PreviewRenderUtility m_PreviewUtility;
    private static Mesh s_PlaneMesh;
    /// <summary>
    ///   <para>Useful for indenting shader properties that need the same indent as mini texture field.</para>
    /// </summary>
    public const int kMiniTextureFieldLabelIndentLevel = 2;
    private const float kSpaceBetweenFlexibleAreaAndField = 5f;
    private const float kQueuePopupWidth = 100f;
    private const float kCustomQueuePopupWidth = 115f;

    internal bool forceVisible { get; set; }

    private static MaterialEditor.PreviewType GetPreviewType(Material mat)
    {
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null)
        return MaterialEditor.PreviewType.Mesh;
      string lower = mat.GetTag("PreviewType", false, string.Empty).ToLower();
      if (lower == "plane")
        return MaterialEditor.PreviewType.Plane;
      return lower == "skybox" || (UnityEngine.Object) mat.shader != (UnityEngine.Object) null && mat.shader.name.Contains("Skybox") ? MaterialEditor.PreviewType.Skybox : MaterialEditor.PreviewType.Mesh;
    }

    private static bool DoesPreviewAllowRotation(MaterialEditor.PreviewType type)
    {
      return type != MaterialEditor.PreviewType.Plane;
    }

    /// <summary>
    ///   <para>Is the current material expanded.</para>
    /// </summary>
    public bool isVisible
    {
      get
      {
        return this.forceVisible || this.m_IsVisible;
      }
    }

    private Renderer rendererForAnimationMode
    {
      get
      {
        if (this.m_RenderersForAnimationMode == null || this.m_RenderersForAnimationMode.Length == 0)
          return (Renderer) null;
        return this.m_RenderersForAnimationMode[0];
      }
    }

    /// <summary>
    ///   <para>Set the shader of the material.</para>
    /// </summary>
    /// <param name="shader">Shader to set.</param>
    /// <param name="registerUndo">Should undo be registered.</param>
    /// <param name="newShader"></param>
    public void SetShader(Shader shader)
    {
      this.SetShader(shader, true);
    }

    /// <summary>
    ///   <para>Set the shader of the material.</para>
    /// </summary>
    /// <param name="shader">Shader to set.</param>
    /// <param name="registerUndo">Should undo be registered.</param>
    /// <param name="newShader"></param>
    public void SetShader(Shader newShader, bool registerUndo)
    {
      bool flag = false;
      ShaderGUI customShaderGui = this.m_CustomShaderGUI;
      this.CreateCustomShaderEditorIfNeeded(newShader);
      this.m_Shader = newShader;
      if (customShaderGui != this.m_CustomShaderGUI)
        flag = true;
      foreach (Material target in this.targets)
      {
        Shader shader = target.shader;
        Undo.RecordObject((UnityEngine.Object) target, "Assign shader");
        if (this.m_CustomShaderGUI != null)
          this.m_CustomShaderGUI.AssignNewShaderToMaterial(target, shader, newShader);
        else
          target.shader = newShader;
        EditorMaterialUtility.ResetDefaultTextures(target, false);
        MaterialEditor.ApplyMaterialPropertyDrawers(target);
      }
      if (flag)
        this.UpdateAllOpenMaterialEditors();
      else
        this.OnShaderChanged();
    }

    private void UpdateAllOpenMaterialEditors()
    {
      foreach (MaterialEditor materialEditor in MaterialEditor.s_MaterialEditors.ToArray())
        materialEditor.DetectShaderEditorNeedsUpdate();
    }

    internal void OnSelectedShaderPopup(string command, Shader shader)
    {
      this.serializedObject.Update();
      if ((UnityEngine.Object) shader != (UnityEngine.Object) null)
        this.SetShader(shader);
      this.PropertiesChanged();
    }

    private bool HasMultipleMixedShaderValues()
    {
      bool flag = false;
      Shader shader = (this.targets[0] as Material).shader;
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if ((UnityEngine.Object) shader != (UnityEngine.Object) (this.targets[index] as Material).shader)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void ShaderPopup(GUIStyle style)
    {
      bool enabled = GUI.enabled;
      Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), 47385, EditorGUIUtility.TempContent("Shader"));
      EditorGUI.showMixedValue = this.HasMultipleMixedShaderValues();
      GUIContent content = EditorGUIUtility.TempContent(!((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) ? "No Shader Selected" : this.m_Shader.name);
      if (EditorGUI.DropdownButton(position, content, FocusType.Keyboard, style))
      {
        EditorGUI.showMixedValue = false;
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
        InternalEditorUtility.SetupShaderMenu(this.target as Material);
        EditorUtility.Internal_DisplayPopupMenu(new Rect(screenPoint.x, screenPoint.y, position.width, position.height), "CONTEXT/ShaderPopup", (UnityEngine.Object) this, 0);
        Event.current.Use();
      }
      EditorGUI.showMixedValue = false;
      GUI.enabled = enabled;
    }

    /// <summary>
    ///   <para>Called when the Editor is woken up.</para>
    /// </summary>
    public virtual void Awake()
    {
      this.m_IsVisible = InternalEditorUtility.GetIsInspectorExpanded(this.target);
      if (MaterialEditor.GetPreviewType(this.target as Material) != MaterialEditor.PreviewType.Skybox)
        return;
      this.m_PreviewDir = new Vector2(0.0f, 50f);
    }

    private void DetectShaderEditorNeedsUpdate()
    {
      Material target = this.target as Material;
      bool flag1 = (bool) ((UnityEngine.Object) target) && (UnityEngine.Object) target.shader != (UnityEngine.Object) this.m_Shader;
      bool flag2 = (bool) ((UnityEngine.Object) target) && (bool) ((UnityEngine.Object) target.shader) && this.m_CustomEditorClassName != target.shader.customEditor;
      if (!flag1 && !flag2)
        return;
      this.CreateCustomShaderEditorIfNeeded(target.shader);
      if (flag1)
      {
        this.m_Shader = target.shader;
        this.OnShaderChanged();
      }
      InspectorWindow.RepaintAllInspectors();
    }

    /// <summary>
    ///   <para>Implement specific MaterialEditor GUI code here. If you want to simply extend the existing editor call the base OnInspectorGUI () before doing any custom GUI code.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.CheckSetup();
      this.DetectShaderEditorNeedsUpdate();
      if (!this.isVisible || !((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) || (this.HasMultipleMixedShaderValues() || !this.PropertiesGUI()))
        return;
      this.PropertiesChanged();
    }

    private void CheckSetup()
    {
      if (!this.m_CheckSetup || (UnityEngine.Object) this.m_Shader == (UnityEngine.Object) null)
        return;
      this.m_CheckSetup = false;
      if (this.m_CustomShaderGUI != null || this.IsMaterialEditor(this.m_Shader.customEditor))
        return;
      Debug.LogWarningFormat("Could not create a custom UI for the shader '{0}'. The shader has the following: 'CustomEditor = {1}'. Does the custom editor specified include its namespace? And does the class either derive from ShaderGUI or MaterialEditor?", new object[2]
      {
        (object) this.m_Shader.name,
        (object) this.m_Shader.customEditor
      });
    }

    internal override void OnAssetStoreInspectorGUI()
    {
      this.OnInspectorGUI();
    }

    /// <summary>
    ///   <para>Whenever a material property is changed call this function. This will rebuild the inspector and validate the properties.</para>
    /// </summary>
    public void PropertiesChanged()
    {
      this.m_InfoMessage = (string) null;
      if (this.targets.Length != 1)
        return;
      this.m_InfoMessage = PerformanceChecks.CheckMaterial(this.target as Material, EditorUserBuildSettings.activeBuildTarget);
    }

    /// <summary>
    ///   <para>A callback that is invoked when a Material's Shader is changed in the Inspector.</para>
    /// </summary>
    protected virtual void OnShaderChanged()
    {
    }

    protected override void OnHeaderGUI()
    {
      Rect rect = Editor.DrawHeaderGUI((Editor) this, this.targetTitle, !this.forceVisible ? 10f : 0.0f);
      int controlId = GUIUtility.GetControlID(45678, FocusType.Passive);
      if (this.forceVisible)
        return;
      Rect foldoutRenderRect = EditorGUI.GetInspectorTitleBarObjectFoldoutRenderRect(rect);
      foldoutRenderRect.y = rect.yMax - 17f;
      bool isExpanded = EditorGUI.DoObjectFoldout(this.m_IsVisible, rect, foldoutRenderRect, this.targets, controlId);
      if (isExpanded != this.m_IsVisible)
      {
        this.m_IsVisible = isExpanded;
        InternalEditorUtility.SetIsInspectorExpanded(this.target, isExpanded);
      }
    }

    internal override void OnHeaderControlsGUI()
    {
      this.serializedObject.Update();
      using (new EditorGUI.DisabledScope(!this.IsEnabled()))
      {
        EditorGUIUtility.labelWidth = 50f;
        this.ShaderPopup((GUIStyle) "MiniPulldown");
        if (!((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) || !this.HasMultipleMixedShaderValues() || (this.m_Shader.hideFlags & HideFlags.DontSave) != HideFlags.None)
          return;
        if (GUILayout.Button("Edit...", EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
          AssetDatabase.OpenAsset((UnityEngine.Object) this.m_Shader);
      }
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public float GetFloat(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      float num = ((Material) this.targets[0]).GetFloat(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if ((double) ((Material) this.targets[index]).GetFloat(propertyName) != (double) num)
        {
          hasMixedValue = true;
          break;
        }
      }
      return num;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetFloat(string propertyName, float value)
    {
      foreach (Material target in this.targets)
        target.SetFloat(propertyName, value);
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public Color GetColor(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      Color color = ((Material) this.targets[0]).GetColor(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if (((Material) this.targets[index]).GetColor(propertyName) != color)
        {
          hasMixedValue = true;
          break;
        }
      }
      return color;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetColor(string propertyName, Color value)
    {
      foreach (Material target in this.targets)
        target.SetColor(propertyName, value);
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public Vector4 GetVector(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      Vector4 vector = ((Material) this.targets[0]).GetVector(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if (((Material) this.targets[index]).GetVector(propertyName) != vector)
        {
          hasMixedValue = true;
          break;
        }
      }
      return vector;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetVector(string propertyName, Vector4 value)
    {
      foreach (Material target in this.targets)
        target.SetVector(propertyName, value);
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public Texture GetTexture(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      Texture texture = ((Material) this.targets[0]).GetTexture(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if ((UnityEngine.Object) ((Material) this.targets[index]).GetTexture(propertyName) != (UnityEngine.Object) texture)
        {
          hasMixedValue = true;
          break;
        }
      }
      return texture;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetTexture(string propertyName, Texture value)
    {
      foreach (Material target in this.targets)
        target.SetTexture(propertyName, value);
    }

    [Obsolete("Use MaterialProperty instead.")]
    public Vector2 GetTextureScale(string propertyName, out bool hasMixedValueX, out bool hasMixedValueY)
    {
      hasMixedValueX = false;
      hasMixedValueY = false;
      Vector2 textureScale1 = ((Material) this.targets[0]).GetTextureScale(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        Vector2 textureScale2 = ((Material) this.targets[index]).GetTextureScale(propertyName);
        if ((double) textureScale2.x != (double) textureScale1.x)
          hasMixedValueX = true;
        if ((double) textureScale2.y != (double) textureScale1.y)
          hasMixedValueY = true;
        if (hasMixedValueX && hasMixedValueY)
          break;
      }
      return textureScale1;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public Vector2 GetTextureOffset(string propertyName, out bool hasMixedValueX, out bool hasMixedValueY)
    {
      hasMixedValueX = false;
      hasMixedValueY = false;
      Vector2 textureOffset1 = ((Material) this.targets[0]).GetTextureOffset(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        Vector2 textureOffset2 = ((Material) this.targets[index]).GetTextureOffset(propertyName);
        if ((double) textureOffset2.x != (double) textureOffset1.x)
          hasMixedValueX = true;
        if ((double) textureOffset2.y != (double) textureOffset1.y)
          hasMixedValueY = true;
        if (hasMixedValueX && hasMixedValueY)
          break;
      }
      return textureOffset1;
    }

    /// <summary>
    ///   <para>Set the scale of a given texture property.</para>
    /// </summary>
    /// <param name="propertyName">Name of the texture property that you wish to modify the scale of.</param>
    /// <param name="value">Scale to set.</param>
    /// <param name="coord">Set the x or y component of the scale (0 for x, 1 for y).</param>
    [Obsolete("Use MaterialProperty instead.")]
    public void SetTextureScale(string propertyName, Vector2 value, int coord)
    {
      foreach (Material target in this.targets)
      {
        Vector2 textureScale = target.GetTextureScale(propertyName);
        textureScale[coord] = value[coord];
        target.SetTextureScale(propertyName, textureScale);
      }
    }

    /// <summary>
    ///   <para>Set the offset of a given texture property.</para>
    /// </summary>
    /// <param name="propertyName">Name of the texture property that you wish to modify the offset of.</param>
    /// <param name="value">Scale to set.</param>
    /// <param name="coord">Set the x or y component of the offset (0 for x, 1 for y).</param>
    [Obsolete("Use MaterialProperty instead.")]
    public void SetTextureOffset(string propertyName, Vector2 value, int coord)
    {
      foreach (Material target in this.targets)
      {
        Vector2 textureOffset = target.GetTextureOffset(propertyName);
        textureOffset[coord] = value[coord];
        target.SetTextureOffset(propertyName, textureOffset);
      }
    }

    /// <summary>
    ///   <para>Draw a range slider for a range shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop">The property to edit.</param>
    /// <param name="position">Position and size of the range slider control.</param>
    public float RangeProperty(MaterialProperty prop, string label)
    {
      return this.RangePropertyInternal(prop, new GUIContent(label));
    }

    internal float RangePropertyInternal(MaterialProperty prop, GUIContent label)
    {
      return this.RangePropertyInternal(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a range slider for a range shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop">The property to edit.</param>
    /// <param name="position">Position and size of the range slider control.</param>
    public float RangeProperty(Rect position, MaterialProperty prop, string label)
    {
      return this.RangePropertyInternal(position, prop, new GUIContent(label));
    }

    internal float RangePropertyInternal(Rect position, MaterialProperty prop, GUIContent label)
    {
      float power = !(prop.name == "_Shininess") ? 1f : 5f;
      return MaterialEditor.DoPowerRangeProperty(position, prop, label, power);
    }

    internal static float DoPowerRangeProperty(Rect position, MaterialProperty prop, GUIContent label, float power)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 0.0f;
      float num = EditorGUI.PowerSlider(position, label, prop.floatValue, prop.rangeLimits.x, prop.rangeLimits.y, power);
      EditorGUI.showMixedValue = false;
      EditorGUIUtility.labelWidth = labelWidth;
      if (EditorGUI.EndChangeCheck())
        prop.floatValue = num;
      return prop.floatValue;
    }

    internal static int DoIntRangeProperty(Rect position, MaterialProperty prop, GUIContent label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 0.0f;
      int num = EditorGUI.IntSlider(position, label, (int) prop.floatValue, (int) prop.rangeLimits.x, (int) prop.rangeLimits.y);
      EditorGUI.showMixedValue = false;
      EditorGUIUtility.labelWidth = labelWidth;
      if (EditorGUI.EndChangeCheck())
        prop.floatValue = (float) num;
      return (int) prop.floatValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a float shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public float FloatProperty(MaterialProperty prop, string label)
    {
      return this.FloatPropertyInternal(prop, new GUIContent(label));
    }

    internal float FloatPropertyInternal(MaterialProperty prop, GUIContent label)
    {
      return this.FloatPropertyInternal(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a property field for a float shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public float FloatProperty(Rect position, MaterialProperty prop, string label)
    {
      return this.FloatPropertyInternal(position, prop, new GUIContent(label));
    }

    internal float FloatPropertyInternal(Rect position, MaterialProperty prop, GUIContent label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      float num = EditorGUI.FloatField(position, label, prop.floatValue);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.floatValue = num;
      return prop.floatValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a color shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="position"></param>
    /// <param name="prop"></param>
    public Color ColorProperty(MaterialProperty prop, string label)
    {
      return this.ColorPropertyInternal(prop, new GUIContent(label));
    }

    internal Color ColorPropertyInternal(MaterialProperty prop, GUIContent label)
    {
      return this.ColorPropertyInternal(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a property field for a color shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="position"></param>
    /// <param name="prop"></param>
    public Color ColorProperty(Rect position, MaterialProperty prop, string label)
    {
      return this.ColorPropertyInternal(position, prop, new GUIContent(label));
    }

    internal Color ColorPropertyInternal(Rect position, MaterialProperty prop, GUIContent label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      bool hdr = (prop.flags & MaterialProperty.PropFlags.HDR) != MaterialProperty.PropFlags.None;
      bool showAlpha = true;
      Color color = EditorGUI.ColorField(position, label, prop.colorValue, true, showAlpha, hdr, (ColorPickerHDRConfig) null);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.colorValue = color;
      return prop.colorValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a vector shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public Vector4 VectorProperty(MaterialProperty prop, string label)
    {
      return this.VectorProperty(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a property field for a vector shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public Vector4 VectorProperty(Rect position, MaterialProperty prop, string label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 0.0f;
      Vector4 vector4 = EditorGUI.Vector4Field(position, label, prop.vectorValue);
      EditorGUIUtility.labelWidth = labelWidth;
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.vectorValue = vector4;
      return prop.vectorValue;
    }

    public void TextureScaleOffsetProperty(MaterialProperty property)
    {
      double num = (double) this.TextureScaleOffsetProperty(EditorGUILayout.GetControlRect(true, 32f, EditorStyles.layerMaskField, new GUILayoutOption[0]), property, false);
    }

    /// <summary>
    ///   <para>Draws tiling and offset properties for a texture.</para>
    /// </summary>
    /// <param name="position">Rect to draw this control in.</param>
    /// <param name="property">Property to draw.</param>
    /// <param name="partOfTexturePropertyControl">If this control should be rendered under large texture property control use 'true'. If this control should be shown seperately use 'false'.</param>
    public float TextureScaleOffsetProperty(Rect position, MaterialProperty property)
    {
      return this.TextureScaleOffsetProperty(position, property, true);
    }

    /// <summary>
    ///   <para>Draws tiling and offset properties for a texture.</para>
    /// </summary>
    /// <param name="position">Rect to draw this control in.</param>
    /// <param name="property">Property to draw.</param>
    /// <param name="partOfTexturePropertyControl">If this control should be rendered under large texture property control use 'true'. If this control should be shown seperately use 'false'.</param>
    public float TextureScaleOffsetProperty(Rect position, MaterialProperty property, bool partOfTexturePropertyControl)
    {
      this.BeginAnimatedCheck(position, property);
      EditorGUI.BeginChangeCheck();
      int mixedValueMask = property.mixedValueMask >> 1;
      Vector4 vector4 = MaterialEditor.TextureScaleOffsetProperty(position, property.textureScaleAndOffset, mixedValueMask, partOfTexturePropertyControl);
      if (EditorGUI.EndChangeCheck())
        property.textureScaleAndOffset = vector4;
      this.EndAnimatedCheck();
      return 32f;
    }

    private Texture TexturePropertyBody(Rect position, MaterialProperty prop)
    {
      if (prop.type != MaterialProperty.PropType.Texture)
        throw new ArgumentException(string.Format("The MaterialProperty '{0}' should be of type 'Texture' (its type is '{1})'", (object) prop.name, (object) prop.type));
      this.m_DesiredTexdim = prop.textureDimension;
      System.Type typeFromDimension = MaterialEditor.GetTextureTypeFromDimension(this.m_DesiredTexdim);
      bool enabled = GUI.enabled;
      EditorGUI.BeginChangeCheck();
      if ((prop.flags & MaterialProperty.PropFlags.PerRendererData) != MaterialProperty.PropFlags.None)
        GUI.enabled = false;
      EditorGUI.showMixedValue = prop.hasMixedValue;
      int controlId = GUIUtility.GetControlID(12354, FocusType.Keyboard, position);
      Texture texture = EditorGUI.DoObjectField(position, position, controlId, (UnityEngine.Object) prop.textureValue, typeFromDimension, (SerializedProperty) null, new EditorGUI.ObjectFieldValidator(this.TextureValidator), false) as Texture;
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.textureValue = texture;
      GUI.enabled = enabled;
      return prop.textureValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(MaterialProperty prop, string label)
    {
      bool scaleOffset = (prop.flags & MaterialProperty.PropFlags.NoScaleOffset) == MaterialProperty.PropFlags.None;
      return this.TextureProperty(prop, label, scaleOffset);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(MaterialProperty prop, string label, bool scaleOffset)
    {
      return this.TextureProperty(this.GetPropertyRect(prop, label, true), prop, label, scaleOffset);
    }

    /// <summary>
    ///   <para>Make a help box with a message and button. Returns true, if button was pressed.</para>
    /// </summary>
    /// <param name="messageContent">The message text.</param>
    /// <param name="buttonContent">The button text.</param>
    /// <returns>
    ///   <para>Returns true, if button was pressed.</para>
    /// </returns>
    public bool HelpBoxWithButton(GUIContent messageContent, GUIContent buttonContent)
    {
      Rect rect = GUILayoutUtility.GetRect(messageContent, EditorStyles.helpBox);
      GUILayoutUtility.GetRect(1f, 25f);
      rect.height += 25f;
      GUI.Label(rect, messageContent, EditorStyles.helpBox);
      return GUI.Button(new Rect((float) ((double) rect.xMax - 60.0 - 4.0), (float) ((double) rect.yMax - 20.0 - 4.0), 60f, 20f), buttonContent);
    }

    /// <summary>
    ///   <para>Checks if particular property has incorrect type of texture specified by the material, displays appropriate warning and suggests the user to automatically fix the problem.</para>
    /// </summary>
    /// <param name="prop">The texture property to check and display warning for, if necessary.</param>
    public void TextureCompatibilityWarning(MaterialProperty prop)
    {
      if (!InternalEditorUtility.BumpMapTextureNeedsFixing(prop) || !this.HelpBoxWithButton(EditorGUIUtility.TextContent("This texture is not marked as a normal map"), EditorGUIUtility.TextContent("Fix Now")))
        return;
      InternalEditorUtility.FixNormalmapTexture(prop);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property that only takes up a single line height.</para>
    /// </summary>
    /// <param name="position">Rect that this control should be rendered in.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="prop"></param>
    /// <param name="tooltip"></param>
    /// <returns>
    ///   <para>Returns total height used by this control.</para>
    /// </returns>
    public Texture TexturePropertyMiniThumbnail(Rect position, MaterialProperty prop, string label, string tooltip)
    {
      this.BeginAnimatedCheck(position, prop);
      Rect thumbRect;
      Rect labelRect;
      EditorGUI.GetRectsForMiniThumbnailField(position, out thumbRect, out labelRect);
      EditorGUI.HandlePrefixLabel(position, labelRect, new GUIContent(label, tooltip), 0, EditorStyles.label);
      this.EndAnimatedCheck();
      Texture texture = this.TexturePropertyBody(thumbRect, prop);
      Rect rect = position;
      rect.y += position.height;
      rect.height = 27f;
      this.TextureCompatibilityWarning(prop);
      return texture;
    }

    /// <summary>
    ///   <para>Returns the free rect below the label and before the large thumb object field. Is used for e.g. tiling and offset properties.</para>
    /// </summary>
    /// <param name="position">The total rect of the texture property.</param>
    public Rect GetTexturePropertyCustomArea(Rect position)
    {
      ++EditorGUI.indentLevel;
      position.height = MaterialEditor.GetTextureFieldHeight();
      Rect source = position;
      source.yMin += 16f;
      source.xMax -= EditorGUIUtility.fieldWidth + 2f;
      Rect rect = EditorGUI.IndentedRect(source);
      --EditorGUI.indentLevel;
      return rect;
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(Rect position, MaterialProperty prop, string label)
    {
      bool scaleOffset = (prop.flags & MaterialProperty.PropFlags.NoScaleOffset) == MaterialProperty.PropFlags.None;
      return this.TextureProperty(position, prop, label, scaleOffset);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(Rect position, MaterialProperty prop, string label, bool scaleOffset)
    {
      return this.TextureProperty(position, prop, label, string.Empty, scaleOffset);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(Rect position, MaterialProperty prop, string label, string tooltip, bool scaleOffset)
    {
      EditorGUI.PrefixLabel(position, new GUIContent(label, tooltip));
      position.height = MaterialEditor.GetTextureFieldHeight();
      Rect position1 = position;
      position1.xMin = position1.xMax - EditorGUIUtility.fieldWidth;
      Texture texture = this.TexturePropertyBody(position1, prop);
      if (scaleOffset)
      {
        double num = (double) this.TextureScaleOffsetProperty(this.GetTexturePropertyCustomArea(position), prop);
      }
      GUILayout.Space(-6f);
      this.TextureCompatibilityWarning(prop);
      GUILayout.Space(6f);
      return texture;
    }

    /// <summary>
    ///   <para>TODO.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="scaleOffset"></param>
    /// <param name="partOfTexturePropertyControl"></param>
    public static Vector4 TextureScaleOffsetProperty(Rect position, Vector4 scaleOffset)
    {
      return MaterialEditor.TextureScaleOffsetProperty(position, scaleOffset, 0, false);
    }

    /// <summary>
    ///   <para>TODO.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="scaleOffset"></param>
    /// <param name="partOfTexturePropertyControl"></param>
    public static Vector4 TextureScaleOffsetProperty(Rect position, Vector4 scaleOffset, bool partOfTexturePropertyControl)
    {
      return MaterialEditor.TextureScaleOffsetProperty(position, scaleOffset, 0, partOfTexturePropertyControl);
    }

    internal static Vector4 TextureScaleOffsetProperty(Rect position, Vector4 scaleOffset, int mixedValueMask, bool partOfTexturePropertyControl)
    {
      Vector2 vector2_1 = new Vector2(scaleOffset.x, scaleOffset.y);
      Vector2 vector2_2 = new Vector2(scaleOffset.z, scaleOffset.w);
      float width = EditorGUIUtility.labelWidth;
      float x1 = position.x + width;
      float x2 = position.x + EditorGUI.indent;
      if (partOfTexturePropertyControl)
      {
        width = 65f;
        x1 = position.x + width;
        x2 = position.x;
        position.y = position.yMax - 32f;
      }
      Rect totalPosition = new Rect(x2, position.y, width, 16f);
      Rect position1 = new Rect(x1, position.y, position.width - width, 16f);
      EditorGUI.PrefixLabel(totalPosition, MaterialEditor.s_TilingText);
      Vector2 vector2_3 = EditorGUI.Vector2Field(position1, GUIContent.none, vector2_1);
      totalPosition.y += 16f;
      position1.y += 16f;
      EditorGUI.PrefixLabel(totalPosition, MaterialEditor.s_OffsetText);
      Vector2 vector2_4 = EditorGUI.Vector2Field(position1, GUIContent.none, vector2_2);
      return new Vector4(vector2_3.x, vector2_3.y, vector2_4.x, vector2_4.y);
    }

    /// <summary>
    ///   <para>Calculate height needed for the property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    public float GetPropertyHeight(MaterialProperty prop)
    {
      return this.GetPropertyHeight(prop, prop.displayName);
    }

    /// <summary>
    ///   <para>Calculate height needed for the property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    public float GetPropertyHeight(MaterialProperty prop, string label)
    {
      float num = 0.0f;
      MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(((Material) this.target).shader, prop.name);
      if (handler != null)
      {
        num = handler.GetPropertyHeight(prop, label ?? prop.displayName, this);
        if (handler.propertyDrawer != null)
          return num;
      }
      return num + MaterialEditor.GetDefaultPropertyHeight(prop);
    }

    private static float GetTextureFieldHeight()
    {
      return 64f;
    }

    /// <summary>
    ///   <para>Calculate height needed for the property, ignoring custom drawers.</para>
    /// </summary>
    /// <param name="prop"></param>
    public static float GetDefaultPropertyHeight(MaterialProperty prop)
    {
      if (prop.type == MaterialProperty.PropType.Vector)
        return 32f;
      if (prop.type == MaterialProperty.PropType.Texture)
        return MaterialEditor.GetTextureFieldHeight() + 6f;
      return 16f;
    }

    private Rect GetPropertyRect(MaterialProperty prop, GUIContent label, bool ignoreDrawer)
    {
      return this.GetPropertyRect(prop, label.text, ignoreDrawer);
    }

    private Rect GetPropertyRect(MaterialProperty prop, string label, bool ignoreDrawer)
    {
      float height = 0.0f;
      if (!ignoreDrawer)
      {
        MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(((Material) this.target).shader, prop.name);
        if (handler != null)
        {
          height = handler.GetPropertyHeight(prop, label ?? prop.displayName, this);
          if (handler.propertyDrawer != null)
            return EditorGUILayout.GetControlRect(true, height, EditorStyles.layerMaskField, new GUILayoutOption[0]);
        }
      }
      return EditorGUILayout.GetControlRect(true, height + MaterialEditor.GetDefaultPropertyHeight(prop), EditorStyles.layerMaskField, new GUILayoutOption[0]);
    }

    /// <summary>
    ///   <para>Creates a Property wrapper, useful for making regular GUI controls work with MaterialProperty.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use for the control, including label if applicable.</param>
    /// <param name="prop">The MaterialProperty to use for the control.</param>
    public void BeginAnimatedCheck(Rect totalPosition, MaterialProperty prop)
    {
      if ((UnityEngine.Object) this.rendererForAnimationMode == (UnityEngine.Object) null)
        return;
      MaterialEditor.s_AnimatedCheckStack.Push(new MaterialEditor.AnimatedCheckData(prop, totalPosition, GUI.backgroundColor));
      Color color;
      if (!MaterialAnimationUtility.OverridePropertyColor(prop, this.rendererForAnimationMode, out color))
        return;
      GUI.backgroundColor = color;
    }

    /// <summary>
    ///   <para>Creates a Property wrapper, useful for making regular GUI controls work with MaterialProperty.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use for the control, including label if applicable.</param>
    /// <param name="prop">The MaterialProperty to use for the control.</param>
    public void BeginAnimatedCheck(MaterialProperty prop)
    {
      this.BeginAnimatedCheck(Rect.zero, prop);
    }

    /// <summary>
    ///   <para>Ends a Property wrapper started with BeginAnimatedCheck.</para>
    /// </summary>
    public void EndAnimatedCheck()
    {
      if ((UnityEngine.Object) this.rendererForAnimationMode == (UnityEngine.Object) null)
        return;
      MaterialEditor.AnimatedCheckData animatedCheckData = MaterialEditor.s_AnimatedCheckStack.Pop();
      if (Event.current.type == EventType.ContextClick && animatedCheckData.totalPosition.Contains(Event.current.mousePosition))
        this.DoPropertyContextMenu(animatedCheckData.property);
      GUI.backgroundColor = animatedCheckData.color;
    }

    private void DoPropertyContextMenu(MaterialProperty prop)
    {
      if (MaterialEditor.contextualPropertyMenu == null)
        return;
      GenericMenu menu = new GenericMenu();
      MaterialEditor.contextualPropertyMenu(menu, prop, this.m_RenderersForAnimationMode);
      if (menu.GetItemCount() > 0)
        menu.ShowAsContext();
    }

    /// <summary>
    ///   <para>Handes UI for one shader property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void ShaderProperty(MaterialProperty prop, string label)
    {
      this.ShaderProperty(prop, new GUIContent(label));
    }

    public void ShaderProperty(MaterialProperty prop, GUIContent label)
    {
      this.ShaderProperty(prop, label, 0);
    }

    public void ShaderProperty(MaterialProperty prop, string label, int labelIndent)
    {
      this.ShaderProperty(prop, new GUIContent(label), labelIndent);
    }

    public void ShaderProperty(MaterialProperty prop, GUIContent label, int labelIndent)
    {
      this.ShaderProperty(this.GetPropertyRect(prop, label, false), prop, label, labelIndent);
    }

    /// <summary>
    ///   <para>Handes UI for one shader property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void ShaderProperty(Rect position, MaterialProperty prop, string label)
    {
      this.ShaderProperty(position, prop, new GUIContent(label));
    }

    public void ShaderProperty(Rect position, MaterialProperty prop, GUIContent label)
    {
      this.ShaderProperty(position, prop, label, 0);
    }

    public void ShaderProperty(Rect position, MaterialProperty prop, string label, int labelIndent)
    {
      this.ShaderProperty(position, prop, new GUIContent(label), labelIndent);
    }

    public void ShaderProperty(Rect position, MaterialProperty prop, GUIContent label, int labelIndent)
    {
      this.BeginAnimatedCheck(position, prop);
      EditorGUI.indentLevel += labelIndent;
      this.ShaderPropertyInternal(position, prop, label);
      EditorGUI.indentLevel -= labelIndent;
      this.EndAnimatedCheck();
    }

    /// <summary>
    ///         <para>This function will draw the UI for the lightmap emission property. (None, Realtime, baked)
    /// 
    /// See Also: MaterialLightmapFlags.</para>
    ///       </summary>
    public void LightmapEmissionProperty()
    {
      this.LightmapEmissionProperty(0);
    }

    public void LightmapEmissionProperty(int labelIndent)
    {
      this.LightmapEmissionProperty(EditorGUILayout.GetControlRect(true, 16f, EditorStyles.layerMaskField, new GUILayoutOption[0]), labelIndent);
    }

    private static MaterialGlobalIlluminationFlags GetGlobalIlluminationFlags(MaterialGlobalIlluminationFlags flags)
    {
      MaterialGlobalIlluminationFlags illuminationFlags = MaterialGlobalIlluminationFlags.None;
      if ((flags & MaterialGlobalIlluminationFlags.RealtimeEmissive) != MaterialGlobalIlluminationFlags.None)
        illuminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
      else if ((flags & MaterialGlobalIlluminationFlags.BakedEmissive) != MaterialGlobalIlluminationFlags.None)
        illuminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
      return illuminationFlags;
    }

    public void LightmapEmissionProperty(Rect position, int labelIndent)
    {
      EditorGUI.indentLevel += labelIndent;
      UnityEngine.Object[] targets = this.targets;
      MaterialGlobalIlluminationFlags illuminationFlags1 = MaterialEditor.GetGlobalIlluminationFlags(((Material) this.target).globalIlluminationFlags);
      bool flag = false;
      for (int index = 1; index < targets.Length; ++index)
      {
        if (MaterialEditor.GetGlobalIlluminationFlags(((Material) targets[index]).globalIlluminationFlags) != illuminationFlags1)
          flag = true;
      }
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = flag;
      MaterialGlobalIlluminationFlags illuminationFlags2 = (MaterialGlobalIlluminationFlags) EditorGUI.IntPopup(position, MaterialEditor.Styles.lightmapEmissiveLabel, (int) illuminationFlags1, MaterialEditor.Styles.lightmapEmissiveStrings, MaterialEditor.Styles.lightmapEmissiveValues);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        foreach (Material material in targets)
        {
          MaterialGlobalIlluminationFlags illuminationFlags3 = material.globalIlluminationFlags & ~MaterialGlobalIlluminationFlags.AnyEmissive | illuminationFlags2;
          material.globalIlluminationFlags = illuminationFlags3;
        }
      }
      EditorGUI.indentLevel -= labelIndent;
    }

    /// <summary>
    ///   <para>This function will draw the UI for controlling whether emission is enabled or not on a material.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if enabled, or false if disabled or mixed due to multi-editing.</para>
    /// </returns>
    public bool EmissionEnabledProperty()
    {
      Material[] materialArray = Array.ConvertAll<UnityEngine.Object, Material>(this.targets, (Converter<UnityEngine.Object, Material>) (o => (Material) o));
      LightModeUtil lightModeUtil = LightModeUtil.Get();
      MaterialGlobalIlluminationFlags illuminationFlags = !lightModeUtil.IsRealtimeGIEnabled() ? (!lightModeUtil.AreBakedLightmapsEnabled() ? MaterialGlobalIlluminationFlags.None : MaterialGlobalIlluminationFlags.BakedEmissive) : MaterialGlobalIlluminationFlags.RealtimeEmissive;
      bool flag1 = materialArray[0].globalIlluminationFlags != MaterialGlobalIlluminationFlags.EmissiveIsBlack;
      bool flag2 = false;
      for (int index = 1; index < materialArray.Length; ++index)
      {
        if (materialArray[index].globalIlluminationFlags != MaterialGlobalIlluminationFlags.EmissiveIsBlack != flag1)
        {
          flag2 = true;
          break;
        }
      }
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = flag2;
      bool flag3 = EditorGUILayout.Toggle(MaterialEditor.Styles.emissionLabel, flag1, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return !flag2 && flag3;
      foreach (Material material in materialArray)
        material.globalIlluminationFlags = !flag3 ? MaterialGlobalIlluminationFlags.EmissiveIsBlack : illuminationFlags;
      return flag3;
    }

    /// <summary>
    ///   <para>Properly sets up the globalIllumination flag on the given Material depending on the current flag's state and the material's emission property.</para>
    /// </summary>
    /// <param name="mat">The material to be fixed up.</param>
    public static void FixupEmissiveFlag(Material mat)
    {
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (mat));
      mat.globalIlluminationFlags = MaterialEditor.FixupEmissiveFlag(mat.GetColor("_EmissionColor"), mat.globalIlluminationFlags);
    }

    /// <summary>
    ///   <para>Returns a properly set global illlumination flag based on the passed in flag and the given color.</para>
    /// </summary>
    /// <param name="col">Emission color.</param>
    /// <param name="flags">Current global illumination flag.</param>
    /// <returns>
    ///   <para>The fixed up flag.</para>
    /// </returns>
    public static MaterialGlobalIlluminationFlags FixupEmissiveFlag(Color col, MaterialGlobalIlluminationFlags flags)
    {
      if ((flags & MaterialGlobalIlluminationFlags.BakedEmissive) != MaterialGlobalIlluminationFlags.None && (double) col.maxColorComponent == 0.0)
        flags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;
      else if (flags != MaterialGlobalIlluminationFlags.EmissiveIsBlack)
        flags &= MaterialGlobalIlluminationFlags.AnyEmissive;
      return flags;
    }

    /// <summary>
    ///   <para>Draws the UI for setting the global illumination flag of a material.</para>
    /// </summary>
    /// <param name="indent">Level of indentation for the property.</param>
    /// <param name="enabled">True if emission is enabled for the material, false otherwise.</param>
    public void LightmapEmissionFlagsProperty(int indent, bool enabled)
    {
      Material[] materialArray = Array.ConvertAll<UnityEngine.Object, Material>(this.targets, (Converter<UnityEngine.Object, Material>) (o => (Material) o));
      MaterialGlobalIlluminationFlags illuminationFlags1 = MaterialGlobalIlluminationFlags.AnyEmissive;
      MaterialGlobalIlluminationFlags illuminationFlags2 = materialArray[0].globalIlluminationFlags & illuminationFlags1;
      bool flag1 = false;
      for (int index = 1; index < materialArray.Length; ++index)
        flag1 = flag1 || (materialArray[index].globalIlluminationFlags & illuminationFlags1) != illuminationFlags2;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = flag1;
      EditorGUI.indentLevel += indent;
      int[] optionValues = new int[2]{ MaterialEditor.Styles.lightmapEmissiveValues[0], MaterialEditor.Styles.lightmapEmissiveValues[1] };
      GUIContent[] displayedOptions = new GUIContent[2]{ MaterialEditor.Styles.lightmapEmissiveStrings[0], MaterialEditor.Styles.lightmapEmissiveStrings[1] };
      MaterialGlobalIlluminationFlags illuminationFlags3 = (MaterialGlobalIlluminationFlags) EditorGUILayout.IntPopup(MaterialEditor.Styles.lightmapEmissiveLabel, (int) illuminationFlags2, displayedOptions, optionValues, new GUILayoutOption[0]);
      EditorGUI.indentLevel -= indent;
      EditorGUI.showMixedValue = false;
      bool flag2 = EditorGUI.EndChangeCheck();
      foreach (Material mat in materialArray)
      {
        mat.globalIlluminationFlags = !flag2 ? mat.globalIlluminationFlags : illuminationFlags3;
        MaterialEditor.FixupEmissiveFlag(mat);
      }
    }

    private void ShaderPropertyInternal(Rect position, MaterialProperty prop, GUIContent label)
    {
      MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(((Material) this.target).shader, prop.name);
      if (handler != null)
      {
        handler.OnGUI(ref position, prop, label.text == null ? new GUIContent(prop.displayName) : label, this);
        if (handler.propertyDrawer != null)
          return;
      }
      this.DefaultShaderPropertyInternal(position, prop, label);
    }

    /// <summary>
    ///   <para>Handles UI for one shader property ignoring any custom drawers.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void DefaultShaderProperty(MaterialProperty prop, string label)
    {
      this.DefaultShaderPropertyInternal(prop, new GUIContent(label));
    }

    internal void DefaultShaderPropertyInternal(MaterialProperty prop, GUIContent label)
    {
      this.DefaultShaderPropertyInternal(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Handles UI for one shader property ignoring any custom drawers.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void DefaultShaderProperty(Rect position, MaterialProperty prop, string label)
    {
      this.DefaultShaderPropertyInternal(position, prop, new GUIContent(label));
    }

    internal void DefaultShaderPropertyInternal(Rect position, MaterialProperty prop, GUIContent label)
    {
      switch (prop.type)
      {
        case MaterialProperty.PropType.Color:
          this.ColorPropertyInternal(position, prop, label);
          break;
        case MaterialProperty.PropType.Vector:
          this.VectorProperty(position, prop, label.text);
          break;
        case MaterialProperty.PropType.Float:
          double num1 = (double) this.FloatPropertyInternal(position, prop, label);
          break;
        case MaterialProperty.PropType.Range:
          double num2 = (double) this.RangePropertyInternal(position, prop, label);
          break;
        case MaterialProperty.PropType.Texture:
          this.TextureProperty(position, prop, label.text);
          break;
        default:
          GUI.Label(position, "Unknown property type: " + prop.name + ": " + (object) prop.type);
          break;
      }
    }

    [Obsolete("Use RangeProperty with MaterialProperty instead.")]
    public float RangeProperty(string propertyName, string label, float v2, float v3)
    {
      return this.RangeProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use FloatProperty with MaterialProperty instead.")]
    public float FloatProperty(string propertyName, string label)
    {
      return this.FloatProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use ColorProperty with MaterialProperty instead.")]
    public Color ColorProperty(string propertyName, string label)
    {
      return this.ColorProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use VectorProperty with MaterialProperty instead.")]
    public Vector4 VectorProperty(string propertyName, string label)
    {
      return this.VectorProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use TextureProperty with MaterialProperty instead.")]
    public Texture TextureProperty(string propertyName, string label, ShaderUtil.ShaderPropertyTexDim texDim)
    {
      return this.TextureProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use TextureProperty with MaterialProperty instead.")]
    public Texture TextureProperty(string propertyName, string label, ShaderUtil.ShaderPropertyTexDim texDim, bool scaleOffset)
    {
      return this.TextureProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label, scaleOffset);
    }

    [Obsolete("Use ShaderProperty that takes MaterialProperty parameter instead.")]
    public void ShaderProperty(Shader shader, int propertyIndex)
    {
      MaterialProperty materialProperty = MaterialEditor.GetMaterialProperty(this.targets, propertyIndex);
      this.ShaderProperty(materialProperty, materialProperty.displayName);
    }

    /// <summary>
    ///   <para>Get shader property information of the passed materials.</para>
    /// </summary>
    /// <param name="mats"></param>
    public static MaterialProperty[] GetMaterialProperties(UnityEngine.Object[] mats)
    {
      if (mats == null)
        throw new ArgumentNullException(nameof (mats));
      if (Array.IndexOf<UnityEngine.Object>(mats, (UnityEngine.Object) null) >= 0)
        throw new ArgumentException("List of materials contains null");
      return ShaderUtil.GetMaterialProperties(mats);
    }

    /// <summary>
    ///   <para>Get information about a single shader property.</para>
    /// </summary>
    /// <param name="mats">Selected materials.</param>
    /// <param name="name">Property name.</param>
    /// <param name="propertyIndex">Property index.</param>
    public static MaterialProperty GetMaterialProperty(UnityEngine.Object[] mats, string name)
    {
      if (mats == null)
        throw new ArgumentNullException(nameof (mats));
      if (Array.IndexOf<UnityEngine.Object>(mats, (UnityEngine.Object) null) >= 0)
        throw new ArgumentException("List of materials contains null");
      return ShaderUtil.GetMaterialProperty(mats, name);
    }

    /// <summary>
    ///   <para>Get information about a single shader property.</para>
    /// </summary>
    /// <param name="mats">Selected materials.</param>
    /// <param name="name">Property name.</param>
    /// <param name="propertyIndex">Property index.</param>
    public static MaterialProperty GetMaterialProperty(UnityEngine.Object[] mats, int propertyIndex)
    {
      if (mats == null)
        throw new ArgumentNullException(nameof (mats));
      if (Array.IndexOf<UnityEngine.Object>(mats, (UnityEngine.Object) null) >= 0)
        throw new ArgumentException("List of materials contains null");
      return ShaderUtil.GetMaterialProperty_Index(mats, propertyIndex);
    }

    private static Renderer[] GetAssociatedRenderersFromInspector()
    {
      List<Renderer> rendererList = new List<Renderer>();
      if ((bool) ((UnityEngine.Object) InspectorWindow.s_CurrentInspectorWindow))
      {
        foreach (Editor activeEditor in InspectorWindow.s_CurrentInspectorWindow.tracker.activeEditors)
        {
          foreach (UnityEngine.Object target in activeEditor.targets)
          {
            Renderer renderer = target as Renderer;
            if ((bool) ((UnityEngine.Object) renderer))
              rendererList.Add(renderer);
          }
        }
      }
      return rendererList.ToArray();
    }

    public static Renderer PrepareMaterialPropertiesForAnimationMode(MaterialProperty[] properties, bool isMaterialEditable)
    {
      Renderer[] rendererArray = MaterialEditor.PrepareMaterialPropertiesForAnimationMode(properties, MaterialEditor.GetAssociatedRenderersFromInspector(), isMaterialEditable);
      return rendererArray == null || rendererArray.Length <= 0 ? (Renderer) null : rendererArray[0];
    }

    internal static Renderer PrepareMaterialPropertiesForAnimationMode(MaterialProperty[] properties, Renderer renderer, bool isMaterialEditable)
    {
      Renderer[] rendererArray = MaterialEditor.PrepareMaterialPropertiesForAnimationMode(properties, new Renderer[1]{ renderer }, (isMaterialEditable ? 1 : 0) != 0);
      return rendererArray == null || rendererArray.Length <= 0 ? (Renderer) null : rendererArray[0];
    }

    internal static Renderer[] PrepareMaterialPropertiesForAnimationMode(MaterialProperty[] properties, Renderer[] renderers, bool isMaterialEditable)
    {
      bool flag = AnimationMode.InAnimationMode();
      if (renderers != null && renderers.Length > 0)
      {
        MaterialEditor.ForwardApplyMaterialModification materialModification = new MaterialEditor.ForwardApplyMaterialModification(renderers, isMaterialEditable);
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        renderers[0].GetPropertyBlock(materialPropertyBlock);
        foreach (MaterialProperty property in properties)
        {
          property.ReadFromMaterialPropertyBlock(materialPropertyBlock);
          if (flag)
            property.applyPropertyCallback = new MaterialProperty.ApplyPropertyCallback(materialModification.DidModifyAnimationModeMaterialProperty);
        }
      }
      if (flag)
        return renderers;
      return (Renderer[]) null;
    }

    /// <summary>
    ///   <para>Set EditorGUIUtility.fieldWidth and labelWidth to the default values that PropertiesGUI uses.</para>
    /// </summary>
    public void SetDefaultGUIWidths()
    {
      EditorGUIUtility.fieldWidth = 64f;
      EditorGUIUtility.labelWidth = (float) ((double) GUIClip.visibleRect.width - (double) EditorGUIUtility.fieldWidth - 17.0);
    }

    private bool IsMaterialEditor(string customEditorName)
    {
      string str = "UnityEditor." + customEditorName;
      foreach (Assembly loadedAssembly in EditorAssemblies.loadedAssemblies)
      {
        foreach (System.Type c in AssemblyHelper.GetTypesFromAssembly(loadedAssembly))
        {
          if ((c.FullName.Equals(customEditorName, StringComparison.Ordinal) || c.FullName.Equals(str, StringComparison.Ordinal)) && typeof (MaterialEditor).IsAssignableFrom(c))
            return true;
        }
      }
      return false;
    }

    private void CreateCustomShaderEditorIfNeeded(Shader shader)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null || string.IsNullOrEmpty(shader.customEditor))
      {
        this.m_CustomEditorClassName = "";
        this.m_CustomShaderGUI = (ShaderGUI) null;
      }
      else
      {
        if (this.m_CustomEditorClassName == shader.customEditor)
          return;
        this.m_CustomEditorClassName = shader.customEditor;
        this.m_CustomShaderGUI = ShaderGUIUtility.CreateShaderGUI(this.m_CustomEditorClassName);
        this.m_CheckSetup = true;
      }
    }

    /// <summary>
    ///   <para>Render the standard material properties. This method will either render properties using a IShaderGUI instance if found otherwise it uses PropertiesDefaultGUI.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if any value was changed.</para>
    /// </returns>
    public bool PropertiesGUI()
    {
      if (this.m_InsidePropertiesGUI)
      {
        Debug.LogWarning((object) "PropertiesGUI() is being called recursively. If you want to render the default gui for shader properties then call PropertiesDefaultGUI() instead");
        return false;
      }
      EditorGUI.BeginChangeCheck();
      MaterialProperty[] materialProperties = MaterialEditor.GetMaterialProperties(this.targets);
      this.m_RenderersForAnimationMode = MaterialEditor.PrepareMaterialPropertiesForAnimationMode(materialProperties, MaterialEditor.GetAssociatedRenderersFromInspector(), GUI.enabled);
      bool enabled = GUI.enabled;
      if (this.m_RenderersForAnimationMode != null)
        GUI.enabled = true;
      this.m_InsidePropertiesGUI = true;
      try
      {
        if (this.m_CustomShaderGUI != null)
          this.m_CustomShaderGUI.OnGUI(this, materialProperties);
        else
          this.PropertiesDefaultGUI(materialProperties);
        Renderer[] renderersFromInspector = MaterialEditor.GetAssociatedRenderersFromInspector();
        if (renderersFromInspector != null)
        {
          if (renderersFromInspector.Length > 0)
          {
            if (Event.current.type == EventType.Layout)
              renderersFromInspector[0].GetPropertyBlock(this.m_PropertyBlock);
            if (this.m_PropertyBlock != null && !this.m_PropertyBlock.isEmpty)
              EditorGUILayout.HelpBox(MaterialEditor.Styles.propBlockInfo, MessageType.Info);
          }
        }
      }
      catch (Exception ex)
      {
        GUI.enabled = enabled;
        this.m_InsidePropertiesGUI = false;
        this.m_RenderersForAnimationMode = (Renderer[]) null;
        throw;
      }
      GUI.enabled = enabled;
      this.m_InsidePropertiesGUI = false;
      this.m_RenderersForAnimationMode = (Renderer[]) null;
      return EditorGUI.EndChangeCheck();
    }

    /// <summary>
    ///   <para>Default rendering of shader properties.</para>
    /// </summary>
    /// <param name="props">Array of material properties.</param>
    public void PropertiesDefaultGUI(MaterialProperty[] props)
    {
      this.SetDefaultGUIWidths();
      if (this.m_InfoMessage != null)
        EditorGUILayout.HelpBox(this.m_InfoMessage, MessageType.Info);
      else
        GUIUtility.GetControlID(MaterialEditor.s_ControlHash, FocusType.Passive, new Rect(0.0f, 0.0f, 0.0f, 0.0f));
      for (int index = 0; index < props.Length; ++index)
      {
        if ((props[index].flags & (MaterialProperty.PropFlags.HideInInspector | MaterialProperty.PropFlags.PerRendererData)) == MaterialProperty.PropFlags.None)
          this.ShaderProperty(EditorGUILayout.GetControlRect(true, this.GetPropertyHeight(props[index], props[index].displayName), EditorStyles.layerMaskField, new GUILayoutOption[0]), props[index], props[index].displayName);
      }
      EditorGUILayout.Space();
      EditorGUILayout.Space();
      this.RenderQueueField();
      this.EnableInstancingField();
      this.DoubleSidedGIField();
    }

    /// <summary>
    ///   <para>Apply initial MaterialPropertyDrawer values.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="targets"></param>
    public static void ApplyMaterialPropertyDrawers(Material material)
    {
      MaterialEditor.ApplyMaterialPropertyDrawers(new UnityEngine.Object[1]
      {
        (UnityEngine.Object) material
      });
    }

    /// <summary>
    ///   <para>Apply initial MaterialPropertyDrawer values.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="targets"></param>
    public static void ApplyMaterialPropertyDrawers(UnityEngine.Object[] targets)
    {
      if (targets == null || targets.Length == 0)
        return;
      Material target = targets[0] as Material;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Shader shader = target.shader;
      MaterialProperty[] materialProperties = MaterialEditor.GetMaterialProperties(targets);
      for (int index = 0; index < materialProperties.Length; ++index)
      {
        MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(shader, materialProperties[index].name);
        if (handler != null && handler.propertyDrawer != null)
          handler.propertyDrawer.Apply(materialProperties[index]);
      }
    }

    /// <summary>
    ///   <para>Call this when you change a material property. It will add an undo for the action.</para>
    /// </summary>
    /// <param name="label">Undo Label.</param>
    public void RegisterPropertyChangeUndo(string label)
    {
      Undo.RecordObjects(this.targets, "Modify " + label + " of " + this.targetTitle);
    }

    private UnityEngine.Object TextureValidator(UnityEngine.Object[] references, System.Type objType, SerializedProperty property, EditorGUI.ObjectFieldValidatorOptions options)
    {
      foreach (UnityEngine.Object reference in references)
      {
        Texture texture = reference as Texture;
        if ((bool) ((UnityEngine.Object) texture) && (texture.dimension == this.m_DesiredTexdim || this.m_DesiredTexdim == TextureDimension.Any))
          return (UnityEngine.Object) texture;
      }
      return (UnityEngine.Object) null;
    }

    private void Init()
    {
      if (this.m_PreviewUtility == null)
      {
        this.m_PreviewUtility = new PreviewRenderUtility();
        EditorUtility.SetCameraAnimateMaterials(this.m_PreviewUtility.camera, true);
      }
      if (!((UnityEngine.Object) MaterialEditor.s_Meshes[0] == (UnityEngine.Object) null))
        return;
      GameObject gameObject = (GameObject) EditorGUIUtility.LoadRequired("Previews/PreviewMaterials.fbx");
      gameObject.SetActive(false);
      IEnumerator enumerator = gameObject.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          MeshFilter component = current.GetComponent<MeshFilter>();
          switch (current.name)
          {
            case "sphere":
              MaterialEditor.s_Meshes[0] = component.sharedMesh;
              break;
            case "cube":
              MaterialEditor.s_Meshes[1] = component.sharedMesh;
              break;
            case "cylinder":
              MaterialEditor.s_Meshes[2] = component.sharedMesh;
              break;
            case "torus":
              MaterialEditor.s_Meshes[3] = component.sharedMesh;
              break;
            default:
              Debug.Log((object) ("Something is wrong, weird object found: " + current.name));
              break;
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      MaterialEditor.s_MeshIcons[0] = EditorGUIUtility.IconContent("PreMatSphere");
      MaterialEditor.s_MeshIcons[1] = EditorGUIUtility.IconContent("PreMatCube");
      MaterialEditor.s_MeshIcons[2] = EditorGUIUtility.IconContent("PreMatCylinder");
      MaterialEditor.s_MeshIcons[3] = EditorGUIUtility.IconContent("PreMatTorus");
      MaterialEditor.s_MeshIcons[4] = EditorGUIUtility.IconContent("PreMatQuad");
      MaterialEditor.s_LightIcons[0] = EditorGUIUtility.IconContent("PreMatLight0");
      MaterialEditor.s_LightIcons[1] = EditorGUIUtility.IconContent("PreMatLight1");
      MaterialEditor.s_TimeIcons[0] = EditorGUIUtility.IconContent("PlayButton");
      MaterialEditor.s_TimeIcons[1] = EditorGUIUtility.IconContent("PauseButton");
      Mesh builtinResource = UnityEngine.Resources.GetBuiltinResource(typeof (Mesh), "Quad.fbx") as Mesh;
      MaterialEditor.s_Meshes[4] = builtinResource;
      MaterialEditor.s_PlaneMesh = builtinResource;
    }

    public override void OnPreviewSettings()
    {
      if (this.m_CustomShaderGUI != null)
        this.m_CustomShaderGUI.OnMaterialPreviewSettingsGUI(this);
      else
        this.DefaultPreviewSettingsGUI();
    }

    private bool PreviewSettingsMenuButton(out Rect buttonRect)
    {
      buttonRect = GUILayoutUtility.GetRect(14f, 24f, 14f, 20f);
      Rect position = new Rect(buttonRect.x + (float) (((double) buttonRect.width - 16.0) / 2.0), buttonRect.y + (float) (((double) buttonRect.height - 6.0) / 2.0), 16f, 6f);
      if (Event.current.type == EventType.Repaint)
        MaterialEditor.Styles.kReflectionProbePickerStyle.Draw(position, false, false, false, false);
      return EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive, GUIStyle.none);
    }

    /// <summary>
    ///   <para>Default toolbar for material preview area.</para>
    /// </summary>
    public void DefaultPreviewSettingsGUI()
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return;
      this.Init();
      if (this.targets.Length <= 1 && MaterialEditor.GetPreviewType(this.target as Material) != MaterialEditor.PreviewType.Mesh)
        return;
      this.m_TimeUpdate = PreviewGUI.CycleButton(this.m_TimeUpdate, MaterialEditor.s_TimeIcons);
      this.m_SelectedMesh = PreviewGUI.CycleButton(this.m_SelectedMesh, MaterialEditor.s_MeshIcons);
      this.m_LightMode = PreviewGUI.CycleButton(this.m_LightMode, MaterialEditor.s_LightIcons);
      Rect buttonRect;
      if (this.PreviewSettingsMenuButton(out buttonRect))
        PopupWindow.Show(buttonRect, (PopupWindowContent) this.m_ReflectionProbePicker);
    }

    public override sealed Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      this.Init();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview();
      return this.m_PreviewUtility.EndStaticPreview();
    }

    private void DoRenderPreview()
    {
      if (this.m_PreviewUtility.renderTexture.width <= 0 || this.m_PreviewUtility.renderTexture.height <= 0)
        return;
      Material target = this.target as Material;
      MaterialEditor.PreviewType previewType = MaterialEditor.GetPreviewType(target);
      this.m_PreviewUtility.camera.transform.position = -Vector3.forward * 5f;
      this.m_PreviewUtility.camera.transform.rotation = Quaternion.identity;
      if (this.m_LightMode == 0)
      {
        this.m_PreviewUtility.lights[0].intensity = 1f;
        this.m_PreviewUtility.lights[0].transform.rotation = Quaternion.Euler(30f, 30f, 0.0f);
        this.m_PreviewUtility.lights[1].intensity = 0.0f;
      }
      else
      {
        this.m_PreviewUtility.lights[0].intensity = 1f;
        this.m_PreviewUtility.lights[0].transform.rotation = Quaternion.Euler(50f, 50f, 0.0f);
        this.m_PreviewUtility.lights[1].intensity = 1f;
      }
      this.m_PreviewUtility.ambientColor = new Color(0.2f, 0.2f, 0.2f, 0.0f);
      Quaternion quaternion = Quaternion.identity;
      if (MaterialEditor.DoesPreviewAllowRotation(previewType))
        quaternion = Quaternion.Euler(this.m_PreviewDir.y, 0.0f, 0.0f) * Quaternion.Euler(0.0f, this.m_PreviewDir.x, 0.0f);
      Mesh mesh = MaterialEditor.s_Meshes[this.m_SelectedMesh];
      switch (previewType)
      {
        case MaterialEditor.PreviewType.Mesh:
          this.m_PreviewUtility.camera.transform.position = Quaternion.Inverse(quaternion) * this.m_PreviewUtility.camera.transform.position;
          this.m_PreviewUtility.camera.transform.LookAt(Vector3.zero);
          quaternion = Quaternion.identity;
          break;
        case MaterialEditor.PreviewType.Plane:
          mesh = MaterialEditor.s_PlaneMesh;
          break;
        case MaterialEditor.PreviewType.Skybox:
          mesh = (Mesh) null;
          this.m_PreviewUtility.camera.transform.rotation = Quaternion.Inverse(quaternion);
          this.m_PreviewUtility.camera.fieldOfView = 120f;
          break;
      }
      if ((UnityEngine.Object) mesh != (UnityEngine.Object) null)
        this.m_PreviewUtility.DrawMesh(mesh, Vector3.zero, quaternion, target, 0, (MaterialPropertyBlock) null, this.m_ReflectionProbePicker.Target, false);
      this.m_PreviewUtility.Render(true, true);
      if (previewType != MaterialEditor.PreviewType.Skybox)
        return;
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      InternalEditorUtility.DrawSkyboxMaterial(target, this.m_PreviewUtility.camera);
      GL.sRGBWrite = false;
    }

    /// <summary>
    ///   <para>Can this component be Previewed in its current state?</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public override sealed bool HasPreviewGUI()
    {
      return true;
    }

    /// <summary>
    ///   <para>Does this edit require to be repainted constantly in its current state?</para>
    /// </summary>
    public override bool RequiresConstantRepaint()
    {
      return this.m_TimeUpdate == 1;
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_CustomShaderGUI != null)
        this.m_CustomShaderGUI.OnMaterialInteractivePreviewGUI(this, r, background);
      else
        base.OnInteractivePreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Custom preview for Image component.</para>
    /// </summary>
    /// <param name="r">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_CustomShaderGUI != null)
        this.m_CustomShaderGUI.OnMaterialPreviewGUI(this, r, background);
      else
        this.DefaultPreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Default handling of preview area for materials.</para>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="background"></param>
    public void DefaultPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "Material preview \nnot available");
      }
      else
      {
        this.Init();
        if (MaterialEditor.DoesPreviewAllowRotation(MaterialEditor.GetPreviewType(this.target as Material)))
          this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        this.m_PreviewUtility.BeginPreview(r, background);
        this.DoRenderPreview();
        this.m_PreviewUtility.EndAndDrawPreview(r);
      }
    }

    /// <summary>
    ///   <para>Called when the editor is enabled, if overridden please call the base OnEnable() to ensure that the material inspector is set up properly.</para>
    /// </summary>
    public virtual void OnEnable()
    {
      if (!(bool) this.target)
        return;
      this.m_Shader = this.serializedObject.FindProperty("m_Shader").objectReferenceValue as Shader;
      this.m_CustomEditorClassName = "";
      this.CreateCustomShaderEditorIfNeeded(this.m_Shader);
      this.m_EnableInstancing = this.serializedObject.FindProperty("m_EnableInstancingVariants");
      this.m_DoubleSidedGI = this.serializedObject.FindProperty("m_DoubleSidedGI");
      MaterialEditor.s_MaterialEditors.Add(this);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.PropertiesChanged();
      this.m_PropertyBlock = new MaterialPropertyBlock();
      this.m_ReflectionProbePicker.OnEnable();
    }

    public virtual void UndoRedoPerformed()
    {
      this.UpdateAllOpenMaterialEditors();
      this.PropertiesChanged();
    }

    /// <summary>
    ///   <para>Called when the editor is disabled, if overridden please call the base OnDisable() to ensure that the material inspector is set up properly.</para>
    /// </summary>
    public virtual void OnDisable()
    {
      this.m_ReflectionProbePicker.OnDisable();
      if (this.m_PreviewUtility != null)
      {
        this.m_PreviewUtility.Cleanup();
        this.m_PreviewUtility = (PreviewRenderUtility) null;
      }
      MaterialEditor.s_MaterialEditors.Remove(this);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    internal void OnSceneDrag(SceneView sceneView)
    {
      Event current = Event.current;
      if (current.type == EventType.Repaint)
        return;
      int materialIndex = -1;
      GameObject go = HandleUtility.PickGameObject(current.mousePosition, out materialIndex);
      if (EditorMaterialUtility.IsBackgroundMaterial(this.target as Material))
      {
        this.HandleSkybox(go, current);
      }
      else
      {
        if (!(bool) ((UnityEngine.Object) go) || !(bool) ((UnityEngine.Object) go.GetComponent<Renderer>()))
          return;
        this.HandleRenderer(go.GetComponent<Renderer>(), materialIndex, current);
      }
    }

    internal void HandleSkybox(GameObject go, Event evt)
    {
      bool flag1 = !(bool) ((UnityEngine.Object) go);
      bool flag2 = false;
      if (!flag1 || evt.type == EventType.DragExited)
      {
        evt.Use();
      }
      else
      {
        switch (evt.type)
        {
          case EventType.DragUpdated:
            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            flag2 = true;
            break;
          case EventType.DragPerform:
            DragAndDrop.AcceptDrag();
            flag2 = true;
            break;
        }
      }
      if (!flag2)
        return;
      Undo.RecordObject((UnityEngine.Object) UnityEngine.Object.FindObjectOfType<RenderSettings>(), "Assign Skybox Material");
      RenderSettings.skybox = this.target as Material;
      evt.Use();
    }

    internal void HandleRenderer(Renderer r, int materialIndex, Event evt)
    {
      if (r.GetType().GetCustomAttributes(typeof (RejectDragAndDropMaterial), true).Length > 0)
        return;
      bool flag = false;
      switch (evt.type)
      {
        case EventType.DragUpdated:
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          flag = true;
          break;
        case EventType.DragPerform:
          DragAndDrop.AcceptDrag();
          flag = true;
          break;
      }
      if (!flag)
        return;
      Undo.RecordObject((UnityEngine.Object) r, "Assign Material");
      Material[] sharedMaterials = r.sharedMaterials;
      if (!evt.alt && (materialIndex >= 0 && materialIndex < r.sharedMaterials.Length))
      {
        sharedMaterials[materialIndex] = this.target as Material;
      }
      else
      {
        for (int index = 0; index < sharedMaterials.Length; ++index)
          sharedMaterials[index] = this.target as Material;
      }
      r.sharedMaterials = sharedMaterials;
      evt.Use();
    }

    private bool HasMultipleMixedQueueValues()
    {
      int materialRawRenderQueue = ShaderUtil.GetMaterialRawRenderQueue(this.targets[0] as Material);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if (materialRawRenderQueue != ShaderUtil.GetMaterialRawRenderQueue(this.targets[index] as Material))
          return true;
      }
      return false;
    }

    /// <summary>
    ///   <para>Display UI for editing material's render queue setting.</para>
    /// </summary>
    /// <param name="r"></param>
    public void RenderQueueField()
    {
      this.RenderQueueField(this.GetControlRectForSingleLine());
    }

    /// <summary>
    ///   <para>Display UI for editing material's render queue setting.</para>
    /// </summary>
    /// <param name="r"></param>
    public void RenderQueueField(Rect r)
    {
      EditorGUI.showMixedValue = this.HasMultipleMixedQueueValues();
      Material target1 = this.targets[0] as Material;
      int materialRawRenderQueue = ShaderUtil.GetMaterialRawRenderQueue(target1);
      int renderQueue = target1.renderQueue;
      GUIContent[] displayedOptions;
      int[] optionValues;
      float num1;
      if (Array.IndexOf<int>(MaterialEditor.Styles.queueValues, materialRawRenderQueue) < 0)
      {
        if (Array.IndexOf((Array) MaterialEditor.Styles.customQueueNames, (object) materialRawRenderQueue) < 0)
        {
          int queueIndexToValue = this.CalculateClosestQueueIndexToValue(materialRawRenderQueue);
          string text = MaterialEditor.Styles.queueNames[queueIndexToValue].text;
          int num2 = materialRawRenderQueue - MaterialEditor.Styles.queueValues[queueIndexToValue];
          string str = string.Format(num2 <= 0 ? "{0}{1}" : "{0}+{1}", (object) text, (object) num2);
          MaterialEditor.Styles.customQueueNames[4].text = str;
          MaterialEditor.Styles.customQueueValues[4] = materialRawRenderQueue;
        }
        displayedOptions = MaterialEditor.Styles.customQueueNames;
        optionValues = MaterialEditor.Styles.customQueueValues;
        num1 = 115f;
      }
      else
      {
        displayedOptions = MaterialEditor.Styles.queueNames;
        optionValues = MaterialEditor.Styles.queueValues;
        num1 = 100f;
      }
      float labelWidth = EditorGUIUtility.labelWidth;
      float fieldWidth = EditorGUIUtility.fieldWidth;
      this.SetDefaultGUIWidths();
      EditorGUIUtility.labelWidth -= num1;
      Rect position1 = r;
      position1.width -= EditorGUIUtility.fieldWidth + 2f;
      Rect position2 = r;
      position2.xMin = position2.xMax - EditorGUIUtility.fieldWidth;
      int num3 = materialRawRenderQueue;
      int num4 = EditorGUI.IntPopup(position1, MaterialEditor.Styles.queueLabel, materialRawRenderQueue, displayedOptions, optionValues);
      int num5 = EditorGUI.DelayedIntField(position2, renderQueue);
      if (num3 != num4 || renderQueue != num5)
      {
        this.RegisterPropertyChangeUndo("Render Queue");
        int num2 = num5;
        if (num4 != num3)
          num2 = num4;
        int num6 = Mathf.Clamp(num2, -1, 5000);
        foreach (Material target2 in this.targets)
          target2.renderQueue = num6;
      }
      EditorGUIUtility.labelWidth = labelWidth;
      EditorGUIUtility.fieldWidth = fieldWidth;
      EditorGUI.showMixedValue = false;
    }

    /// <summary>
    ///   <para>Display UI for editing material's render queue setting.</para>
    /// </summary>
    public bool EnableInstancingField()
    {
      if (!ShaderUtil.HasInstancing(this.m_Shader))
        return false;
      this.EnableInstancingField(this.GetControlRectForSingleLine());
      return true;
    }

    /// <summary>
    ///   <para>Display UI for editing material's render queue setting within the specified rect.</para>
    /// </summary>
    /// <param name="r"></param>
    public void EnableInstancingField(Rect r)
    {
      EditorGUI.PropertyField(r, this.m_EnableInstancing, MaterialEditor.Styles.enableInstancingLabel);
      this.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    ///         <para>Display UI for editing a material's Double Sided Global Illumination setting.
    /// Returns true if the UI is indeed displayed i.e. the material supports the Double Sided Global Illumination setting.
    /// +See Also: Material.doubleSidedGI.</para>
    ///       </summary>
    /// <returns>
    ///   <para>True if the UI is displayed, false otherwise.</para>
    /// </returns>
    public bool DoubleSidedGIField()
    {
      Rect rectForSingleLine = this.GetControlRectForSingleLine();
      if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer)
      {
        EditorGUI.PropertyField(rectForSingleLine, this.m_DoubleSidedGI, MaterialEditor.Styles.doubleSidedGILabel);
        this.serializedObject.ApplyModifiedProperties();
        return true;
      }
      using (new EditorGUI.DisabledScope(LightmapEditorSettings.lightmapper != LightmapEditorSettings.Lightmapper.PathTracer))
        EditorGUI.Toggle(rectForSingleLine, MaterialEditor.Styles.doubleSidedGILabel, false);
      return false;
    }

    private int CalculateClosestQueueIndexToValue(int requestedValue)
    {
      int num1 = int.MaxValue;
      int num2 = 1;
      for (int index = 1; index < MaterialEditor.Styles.queueValues.Length; ++index)
      {
        int num3 = Mathf.Abs(MaterialEditor.Styles.queueValues[index] - requestedValue);
        if (num3 < num1)
        {
          num2 = index;
          num1 = num3;
        }
      }
      return num2;
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with additional inlined properites.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First optional property inlined after the texture property.</param>
    /// <param name="extraProperty2">Second optional property inlined after the extraProperty1.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertySingleLine(GUIContent label, MaterialProperty textureProp)
    {
      return this.TexturePropertySingleLine(label, textureProp, (MaterialProperty) null, (MaterialProperty) null);
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with additional inlined properites.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First optional property inlined after the texture property.</param>
    /// <param name="extraProperty2">Second optional property inlined after the extraProperty1.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertySingleLine(GUIContent label, MaterialProperty textureProp, MaterialProperty extraProperty1)
    {
      return this.TexturePropertySingleLine(label, textureProp, extraProperty1, (MaterialProperty) null);
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with additional inlined properites.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First optional property inlined after the texture property.</param>
    /// <param name="extraProperty2">Second optional property inlined after the extraProperty1.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertySingleLine(GUIContent label, MaterialProperty textureProp, MaterialProperty extraProperty1, MaterialProperty extraProperty2)
    {
      Rect rectForSingleLine = this.GetControlRectForSingleLine();
      this.TexturePropertyMiniThumbnail(rectForSingleLine, textureProp, label.text, label.tooltip);
      if (extraProperty1 == null && extraProperty2 == null)
        return rectForSingleLine;
      if (extraProperty1 == null || extraProperty2 == null)
      {
        MaterialProperty property = extraProperty1 ?? extraProperty2;
        if (property.type == MaterialProperty.PropType.Color)
          this.ExtraPropertyAfterTexture(MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine), property);
        else
          this.ExtraPropertyAfterTexture(MaterialEditor.GetRectAfterLabelWidth(rectForSingleLine), property);
      }
      else if (extraProperty1.type == MaterialProperty.PropType.Color)
      {
        this.ExtraPropertyAfterTexture(MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(rectForSingleLine), extraProperty2);
        this.ExtraPropertyAfterTexture(MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine), extraProperty1);
      }
      else
      {
        this.ExtraPropertyAfterTexture(MaterialEditor.GetRightAlignedFieldRect(rectForSingleLine), extraProperty2);
        this.ExtraPropertyAfterTexture(MaterialEditor.GetFlexibleRectBetweenLabelAndField(rectForSingleLine), extraProperty1);
      }
      return rectForSingleLine;
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with a HDR color field and its color brightness float field.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="colorProperty">The color property (will be treated as a HDR color).</param>
    /// <param name="hdrConfig">The HDR color configuration used by the HDR Color Picker.</param>
    /// <param name="showAlpha">If false then the alpha channel information will be hidden in the GUI.</param>
    /// <returns>
    ///   <para>Return the Rect used.</para>
    /// </returns>
    public Rect TexturePropertyWithHDRColor(GUIContent label, MaterialProperty textureProp, MaterialProperty colorProperty, ColorPickerHDRConfig hdrConfig, bool showAlpha)
    {
      Rect rectForSingleLine = this.GetControlRectForSingleLine();
      this.TexturePropertyMiniThumbnail(rectForSingleLine, textureProp, label.text, label.tooltip);
      if (colorProperty.type != MaterialProperty.PropType.Color)
      {
        Debug.LogError((object) ("Assuming MaterialProperty.PropType.Color (was " + (object) colorProperty.type + ")"));
        return rectForSingleLine;
      }
      this.BeginAnimatedCheck(rectForSingleLine, colorProperty);
      ColorPickerHDRConfig hdrConfig1 = hdrConfig ?? ColorPicker.defaultHDRConfig;
      Rect alignedFieldRect = MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine);
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = colorProperty.hasMixedValue;
      Color color1 = EditorGUI.ColorField(alignedFieldRect, GUIContent.none, colorProperty.colorValue, true, showAlpha, true, hdrConfig1);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        colorProperty.colorValue = color1;
      Rect fieldAndRightEdge = MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(rectForSingleLine);
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = fieldAndRightEdge.width - EditorGUIUtility.fieldWidth;
      EditorGUI.BeginChangeCheck();
      Color color2 = EditorGUI.ColorBrightnessField(fieldAndRightEdge, GUIContent.Temp(" "), colorProperty.colorValue, hdrConfig1.minBrightness, hdrConfig1.maxBrightness);
      if (EditorGUI.EndChangeCheck())
        colorProperty.colorValue = color2;
      EditorGUIUtility.labelWidth = labelWidth;
      this.EndAnimatedCheck();
      return rectForSingleLine;
    }

    /// <summary>
    ///   <para>Method for showing a compact layout of properties.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First extra property inlined after the texture property.</param>
    /// <param name="label2">Label for the second extra property (on a new line and indented).</param>
    /// <param name="extraProperty2">Second property on a new line below the texture.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertyTwoLines(GUIContent label, MaterialProperty textureProp, MaterialProperty extraProperty1, GUIContent label2, MaterialProperty extraProperty2)
    {
      if (extraProperty2 == null)
        return this.TexturePropertySingleLine(label, textureProp, extraProperty1);
      Rect rectForSingleLine1 = this.GetControlRectForSingleLine();
      this.TexturePropertyMiniThumbnail(rectForSingleLine1, textureProp, label.text, label.tooltip);
      Rect r = MaterialEditor.GetRectAfterLabelWidth(rectForSingleLine1);
      if (extraProperty1.type == MaterialProperty.PropType.Color)
        r = MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine1);
      this.ExtraPropertyAfterTexture(r, extraProperty1);
      Rect rectForSingleLine2 = this.GetControlRectForSingleLine();
      this.ShaderProperty(rectForSingleLine2, extraProperty2, label2.text, 3);
      rectForSingleLine1.height += rectForSingleLine2.height;
      return rectForSingleLine1;
    }

    private Rect GetControlRectForSingleLine()
    {
      return EditorGUILayout.GetControlRect(true, 18f, EditorStyles.layerMaskField, new GUILayoutOption[0]);
    }

    private void ExtraPropertyAfterTexture(Rect r, MaterialProperty property)
    {
      if ((property.type == MaterialProperty.PropType.Float || property.type == MaterialProperty.PropType.Color) && (double) r.width > (double) EditorGUIUtility.fieldWidth)
      {
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = r.width - EditorGUIUtility.fieldWidth;
        this.ShaderProperty(r, property, " ");
        EditorGUIUtility.labelWidth = labelWidth;
      }
      else
        this.ShaderProperty(r, property, string.Empty);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetRightAlignedFieldRect(Rect r)
    {
      return new Rect(r.xMax - EditorGUIUtility.fieldWidth, r.y, EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetLeftAlignedFieldRect(Rect r)
    {
      return new Rect(r.x + EditorGUIUtility.labelWidth, r.y, EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetFlexibleRectBetweenLabelAndField(Rect r)
    {
      return new Rect(r.x + EditorGUIUtility.labelWidth, r.y, (float) ((double) r.width - (double) EditorGUIUtility.labelWidth - (double) EditorGUIUtility.fieldWidth - 5.0), EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI. Used e.g for the rect after a left aligned Color field.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetFlexibleRectBetweenFieldAndRightEdge(Rect r)
    {
      Rect rectAfterLabelWidth = MaterialEditor.GetRectAfterLabelWidth(r);
      rectAfterLabelWidth.xMin += EditorGUIUtility.fieldWidth + 5f;
      return rectAfterLabelWidth;
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI. This is the rect after the label which can be used for multiple properties. The input rect can be fetched by calling: EditorGUILayout.GetControlRect.</para>
    /// </summary>
    /// <param name="r">Line Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetRectAfterLabelWidth(Rect r)
    {
      return new Rect(r.x + EditorGUIUtility.labelWidth, r.y, r.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
    }

    internal static System.Type GetTextureTypeFromDimension(TextureDimension dim)
    {
      switch (dim)
      {
        case TextureDimension.Any:
          return typeof (Texture);
        case TextureDimension.Tex2D:
          return typeof (Texture);
        case TextureDimension.Tex3D:
          return typeof (Texture3D);
        case TextureDimension.Cube:
          return typeof (Cubemap);
        case TextureDimension.Tex2DArray:
          return typeof (Texture2DArray);
        case TextureDimension.CubeArray:
          return typeof (CubemapArray);
        default:
          return (System.Type) null;
      }
    }

    private static class Styles
    {
      public static readonly GUIStyle kReflectionProbePickerStyle = (GUIStyle) "PaneOptions";
      public static readonly GUIContent lightmapEmissiveLabel = EditorGUIUtility.TextContent("Global Illumination|Controls if the emission is baked or realtime.\n\nBaked only has effect in scenes where baked global illumination is enabled.\n\nRealtime uses realtime global illumination if enabled in the scene. Otherwise the emission won't light up other objects.");
      public static GUIContent[] lightmapEmissiveStrings = new GUIContent[3]{ EditorGUIUtility.TextContent("Realtime"), EditorGUIUtility.TextContent("Baked"), EditorGUIUtility.TextContent("None") };
      public static int[] lightmapEmissiveValues = new int[3]{ 1, 2, 0 };
      public static string propBlockInfo = EditorGUIUtility.TextContent("MaterialPropertyBlock is used to modify these values").text;
      public static readonly GUIContent queueLabel = EditorGUIUtility.TextContent("Render Queue");
      public static readonly GUIContent[] queueNames = new GUIContent[4]{ EditorGUIUtility.TextContent("From Shader"), EditorGUIUtility.TextContent("Geometry|Queue 2000"), EditorGUIUtility.TextContent("AlphaTest|Queue 2450"), EditorGUIUtility.TextContent("Transparent|Queue 3000") };
      public static readonly int[] queueValues = new int[4]{ -1, 2000, 2450, 3000 };
      public static GUIContent[] customQueueNames = new GUIContent[5]{ MaterialEditor.Styles.queueNames[0], MaterialEditor.Styles.queueNames[1], MaterialEditor.Styles.queueNames[2], MaterialEditor.Styles.queueNames[3], EditorGUIUtility.TextContent("") };
      public static int[] customQueueValues = new int[5]{ MaterialEditor.Styles.queueValues[0], MaterialEditor.Styles.queueValues[1], MaterialEditor.Styles.queueValues[2], MaterialEditor.Styles.queueValues[3], 0 };
      public static readonly GUIContent enableInstancingLabel = EditorGUIUtility.TextContent("Enable GPU Instancing");
      public static readonly GUIContent doubleSidedGILabel = EditorGUIUtility.TextContent("Double Sided Global Illumination|When enabled, the lightmapper accounts for both sides of the geometry when calculating Global Illumination. Backfaces are not rendered or added to lightmaps, but get treated as valid when seen from other objects. When using the Progressive Lightmapper backfaces bounce light using the same emission and albedo as frontfaces.");
      public static readonly GUIContent emissionLabel = EditorGUIUtility.TextContent("Emission");
      public const int kNewShaderQueueValue = -1;
      public const int kCustomQueueIndex = 4;
    }

    private enum PreviewType
    {
      Mesh,
      Plane,
      Skybox,
    }

    private struct AnimatedCheckData
    {
      public MaterialProperty property;
      public Rect totalPosition;
      public Color color;

      public AnimatedCheckData(MaterialProperty property, Rect totalPosition, Color color)
      {
        this.property = property;
        this.totalPosition = totalPosition;
        this.color = color;
      }
    }

    internal delegate void MaterialPropertyCallbackFunction(GenericMenu menu, MaterialProperty property, Renderer[] renderers);

    internal class ReflectionProbePicker : PopupWindowContent
    {
      private UnityEngine.ReflectionProbe m_SelectedReflectionProbe;

      public Transform Target
      {
        get
        {
          return !((UnityEngine.Object) this.m_SelectedReflectionProbe != (UnityEngine.Object) null) ? (Transform) null : this.m_SelectedReflectionProbe.transform;
        }
      }

      public override Vector2 GetWindowSize()
      {
        return new Vector2(170f, 48f);
      }

      public void OnEnable()
      {
        this.m_SelectedReflectionProbe = EditorUtility.InstanceIDToObject(SessionState.GetInt("PreviewReflectionProbe", 0)) as UnityEngine.ReflectionProbe;
      }

      public void OnDisable()
      {
        SessionState.SetInt("PreviewReflectionProbe", !(bool) ((UnityEngine.Object) this.m_SelectedReflectionProbe) ? 0 : this.m_SelectedReflectionProbe.GetInstanceID());
      }

      public override void OnGUI(Rect rc)
      {
        EditorGUILayout.LabelField("Select Reflection Probe", EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.Space();
        this.m_SelectedReflectionProbe = EditorGUILayout.ObjectField("", (UnityEngine.Object) this.m_SelectedReflectionProbe, typeof (UnityEngine.ReflectionProbe), true, new GUILayoutOption[0]) as UnityEngine.ReflectionProbe;
      }
    }

    private class ForwardApplyMaterialModification
    {
      private readonly Renderer[] renderers;
      private bool isMaterialEditable;

      public ForwardApplyMaterialModification(Renderer[] r, bool inIsMaterialEditable)
      {
        this.renderers = r;
        this.isMaterialEditable = inIsMaterialEditable;
      }

      public bool DidModifyAnimationModeMaterialProperty(MaterialProperty property, int changedMask, object previousValue)
      {
        bool flag = false;
        foreach (Renderer renderer in this.renderers)
          flag |= MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(property, changedMask, renderer, previousValue);
        if (flag)
          return true;
        return !this.isMaterialEditable;
      }
    }
  }
}
