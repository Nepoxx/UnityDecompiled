// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomRenderTextureEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (CustomRenderTexture))]
  internal class CustomRenderTextureEditor : RenderTextureEditor
  {
    private static CustomRenderTextureEditor.Styles s_Styles = (CustomRenderTextureEditor.Styles) null;
    private readonly AnimBool m_ShowInitSourceAsMaterial = new AnimBool();
    private SerializedProperty m_Material;
    private SerializedProperty m_ShaderPass;
    private SerializedProperty m_InitializationMode;
    private SerializedProperty m_InitSource;
    private SerializedProperty m_InitColor;
    private SerializedProperty m_InitTexture;
    private SerializedProperty m_InitMaterial;
    private SerializedProperty m_UpdateMode;
    private SerializedProperty m_UpdatePeriod;
    private SerializedProperty m_UpdateZoneSpace;
    private SerializedProperty m_UpdateZones;
    private SerializedProperty m_WrapUpdateZones;
    private SerializedProperty m_DoubleBuffered;
    private SerializedProperty m_CubeFaceMask;
    private ReorderableList m_RectList;
    private const float kCubefaceToggleWidth = 70f;
    private const float kRListAddButtonOffset = 16f;
    private const float kIndentSize = 15f;
    private const float kToggleWidth = 100f;

    private static CustomRenderTextureEditor.Styles styles
    {
      get
      {
        if (CustomRenderTextureEditor.s_Styles == null)
          CustomRenderTextureEditor.s_Styles = new CustomRenderTextureEditor.Styles();
        return CustomRenderTextureEditor.s_Styles;
      }
    }

    private bool multipleEditing
    {
      get
      {
        return this.targets.Length > 1;
      }
    }

    private void UpdateZoneVec3PropertyField(Rect rect, SerializedProperty prop, GUIContent label, bool as2D)
    {
      EditorGUI.BeginProperty(rect, label, prop);
      if (!as2D)
      {
        prop.vector3Value = EditorGUI.Vector3Field(rect, label, prop.vector3Value);
      }
      else
      {
        Vector2 vector2 = EditorGUI.Vector2Field(rect, label, new Vector2(prop.vector3Value.x, prop.vector3Value.y));
        prop.vector3Value = new Vector3(vector2.x, vector2.y, prop.vector3Value.z);
      }
      EditorGUI.EndProperty();
    }

    private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
      CustomRenderTexture target = this.target as CustomRenderTexture;
      bool flag = target.dimension == TextureDimension.Tex3D;
      bool doubleBuffered = target.doubleBuffered;
      SerializedProperty arrayElementAtIndex = this.m_RectList.serializedProperty.GetArrayElementAtIndex(index);
      float singleLineHeight = EditorGUIUtility.singleLineHeight;
      rect.y += EditorGUIUtility.standardVerticalSpacing;
      rect.height = singleLineHeight;
      EditorGUI.LabelField(rect, string.Format("Update Zone {0}", (object) index));
      rect.y += singleLineHeight;
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("updateZoneCenter");
      this.UpdateZoneVec3PropertyField(rect, propertyRelative1, CustomRenderTextureEditor.styles.updateZoneCenter, !flag);
      rect.y += singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("updateZoneSize");
      this.UpdateZoneVec3PropertyField(rect, propertyRelative2, CustomRenderTextureEditor.styles.updateZoneSize, !flag);
      if (!flag)
      {
        rect.y += EditorGUIUtility.standardVerticalSpacing + singleLineHeight;
        EditorGUI.PropertyField(rect, arrayElementAtIndex.FindPropertyRelative("rotation"), CustomRenderTextureEditor.styles.updateZoneRotation);
      }
      List<GUIContent> names = new List<GUIContent>();
      List<int> values = new List<int>();
      Material objectReferenceValue = this.m_Material.objectReferenceValue as Material;
      if ((Object) objectReferenceValue != (Object) null)
        this.BuildShaderPassPopup(objectReferenceValue, names, values, true);
      using (new EditorGUI.DisabledScope(names.Count == 0))
      {
        SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("passIndex");
        rect.y += EditorGUIUtility.standardVerticalSpacing + singleLineHeight;
        EditorGUI.IntPopup(rect, propertyRelative3, names.ToArray(), values.ToArray(), CustomRenderTextureEditor.styles.shaderPass);
      }
      if (!doubleBuffered)
        return;
      rect.y += EditorGUIUtility.standardVerticalSpacing + singleLineHeight;
      EditorGUI.PropertyField(rect, arrayElementAtIndex.FindPropertyRelative("needSwap"), CustomRenderTextureEditor.styles.updateZoneRotation);
    }

    private void OnDrawHeader(Rect rect)
    {
      GUI.Label(rect, CustomRenderTextureEditor.styles.updateZoneList);
    }

    private void OnAdd(ReorderableList l)
    {
      CustomRenderTexture target = this.target as CustomRenderTexture;
      int arraySize = l.serializedProperty.arraySize;
      ++l.serializedProperty.arraySize;
      l.index = arraySize;
      SerializedProperty arrayElementAtIndex = l.serializedProperty.GetArrayElementAtIndex(arraySize);
      Vector3 vector3_1 = new Vector3(0.5f, 0.5f, 0.5f);
      Vector3 vector3_2 = new Vector3(1f, 1f, 1f);
      if (target.updateZoneSpace == CustomRenderTextureUpdateZoneSpace.Pixel)
      {
        Vector3 scale = new Vector3((float) target.width, (float) target.height, (float) target.volumeDepth);
        vector3_1.Scale(scale);
        vector3_2.Scale(scale);
      }
      arrayElementAtIndex.FindPropertyRelative("updateZoneCenter").vector3Value = vector3_1;
      arrayElementAtIndex.FindPropertyRelative("updateZoneSize").vector3Value = vector3_2;
      arrayElementAtIndex.FindPropertyRelative("rotation").floatValue = 0.0f;
      arrayElementAtIndex.FindPropertyRelative("passIndex").intValue = -1;
      arrayElementAtIndex.FindPropertyRelative("needSwap").boolValue = false;
    }

    private void OnRemove(ReorderableList l)
    {
      --l.serializedProperty.arraySize;
      if (l.index != l.serializedProperty.arraySize)
        return;
      --l.index;
    }

    private float OnElementHeight(int index)
    {
      CustomRenderTexture target = this.target as CustomRenderTexture;
      bool flag = target.dimension == TextureDimension.Tex3D;
      bool doubleBuffered = target.doubleBuffered;
      int num = 4;
      if (!flag)
        ++num;
      if (doubleBuffered)
        ++num;
      return (EditorGUIUtility.singleLineHeight + 2f) * (float) num;
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_Material = this.serializedObject.FindProperty("m_Material");
      this.m_ShaderPass = this.serializedObject.FindProperty("m_ShaderPass");
      this.m_InitializationMode = this.serializedObject.FindProperty("m_InitializationMode");
      this.m_InitSource = this.serializedObject.FindProperty("m_InitSource");
      this.m_InitColor = this.serializedObject.FindProperty("m_InitColor");
      this.m_InitTexture = this.serializedObject.FindProperty("m_InitTexture");
      this.m_InitMaterial = this.serializedObject.FindProperty("m_InitMaterial");
      this.m_UpdateMode = this.serializedObject.FindProperty("m_UpdateMode");
      this.m_UpdatePeriod = this.serializedObject.FindProperty("m_UpdatePeriod");
      this.m_UpdateZoneSpace = this.serializedObject.FindProperty("m_UpdateZoneSpace");
      this.m_UpdateZones = this.serializedObject.FindProperty("m_UpdateZones");
      this.m_WrapUpdateZones = this.serializedObject.FindProperty("m_WrapUpdateZones");
      this.m_DoubleBuffered = this.serializedObject.FindProperty("m_DoubleBuffered");
      this.m_CubeFaceMask = this.serializedObject.FindProperty("m_CubemapFaceMask");
      this.m_RectList = new ReorderableList(this.serializedObject, this.m_UpdateZones);
      this.m_RectList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.OnDrawElement);
      this.m_RectList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.OnDrawHeader);
      this.m_RectList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.OnAdd);
      this.m_RectList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.OnRemove);
      this.m_RectList.elementHeightCallback = new ReorderableList.ElementHeightCallbackDelegate(this.OnElementHeight);
      this.m_RectList.footerHeight = 0.0f;
      this.m_ShowInitSourceAsMaterial.value = !this.m_InitSource.hasMultipleDifferentValues && this.m_InitSource.intValue == 1;
      this.m_ShowInitSourceAsMaterial.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowInitSourceAsMaterial.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    private void DisplayRenderTextureGUI()
    {
      this.OnRenderTextureGUI(RenderTextureEditor.GUIElements.RenderTargetNoneGUI);
      GUILayout.Space(10f);
    }

    private void BuildShaderPassPopup(Material material, List<GUIContent> names, List<int> values, bool addDefaultPass)
    {
      names.Clear();
      values.Clear();
      int passCount = material.passCount;
      for (int pass = 0; pass < passCount; ++pass)
      {
        string textAndTooltip = material.GetPassName(pass);
        if (textAndTooltip.Length == 0)
          textAndTooltip = string.Format("Unnamed Pass {0}", (object) pass);
        names.Add(EditorGUIUtility.TextContent(textAndTooltip));
        values.Add(pass);
      }
      if (!addDefaultPass)
        return;
      CustomRenderTexture target = this.target as CustomRenderTexture;
      GUIContent guiContent = EditorGUIUtility.TextContent(string.Format("Default ({0})", (object) names[target.shaderPass].text));
      names.Insert(0, guiContent);
      values.Insert(0, -1);
    }

    private void DisplayMaterialGUI()
    {
      EditorGUILayout.PropertyField(this.m_Material, true, new GUILayoutOption[0]);
      ++EditorGUI.indentLevel;
      List<GUIContent> names = new List<GUIContent>();
      List<int> values = new List<int>();
      Material objectReferenceValue = this.m_Material.objectReferenceValue as Material;
      if ((Object) objectReferenceValue != (Object) null)
        this.BuildShaderPassPopup(objectReferenceValue, names, values, false);
      using (new EditorGUI.DisabledScope(names.Count == 0 || this.m_Material.hasMultipleDifferentValues))
      {
        if ((Object) objectReferenceValue != (Object) null)
          EditorGUILayout.IntPopup(this.m_ShaderPass, names.ToArray(), values.ToArray(), CustomRenderTextureEditor.styles.shaderPass, new GUILayoutOption[0]);
      }
      --EditorGUI.indentLevel;
    }

    private void DisplayInitializationGUI()
    {
      this.m_ShowInitSourceAsMaterial.target = !this.m_InitSource.hasMultipleDifferentValues && this.m_InitSource.intValue == 1;
      EditorGUILayout.IntPopup(this.m_InitializationMode, CustomRenderTextureEditor.styles.updateModeStrings, CustomRenderTextureEditor.styles.updateModeValues, CustomRenderTextureEditor.styles.initializationMode, new GUILayoutOption[0]);
      ++EditorGUI.indentLevel;
      EditorGUILayout.IntPopup(this.m_InitSource, CustomRenderTextureEditor.styles.initSourceStrings, CustomRenderTextureEditor.styles.initSourceValues, CustomRenderTextureEditor.styles.initSource, new GUILayoutOption[0]);
      if (!this.m_InitSource.hasMultipleDifferentValues)
      {
        ++EditorGUI.indentLevel;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowInitSourceAsMaterial.faded))
          EditorGUILayout.PropertyField(this.m_InitMaterial, CustomRenderTextureEditor.styles.initMaterial, new GUILayoutOption[0]);
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(1f - this.m_ShowInitSourceAsMaterial.faded))
        {
          EditorGUILayout.PropertyField(this.m_InitColor, CustomRenderTextureEditor.styles.initColor, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_InitTexture, CustomRenderTextureEditor.styles.initTexture, new GUILayoutOption[0]);
        }
        EditorGUILayout.EndFadeGroup();
        --EditorGUI.indentLevel;
      }
      --EditorGUI.indentLevel;
    }

    private void DisplayUpdateGUI()
    {
      EditorGUILayout.IntPopup(this.m_UpdateMode, CustomRenderTextureEditor.styles.updateModeStrings, CustomRenderTextureEditor.styles.updateModeValues, CustomRenderTextureEditor.styles.updateMode, new GUILayoutOption[0]);
      ++EditorGUI.indentLevel;
      if (this.m_UpdateMode.intValue == 1)
        EditorGUILayout.PropertyField(this.m_UpdatePeriod, CustomRenderTextureEditor.styles.updatePeriod, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DoubleBuffered, CustomRenderTextureEditor.styles.doubleBuffered, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_WrapUpdateZones, CustomRenderTextureEditor.styles.wrapUpdateZones, new GUILayoutOption[0]);
      bool flag = true;
      foreach (Object target in this.targets)
      {
        CustomRenderTexture customRenderTexture = target as CustomRenderTexture;
        if ((Object) customRenderTexture != (Object) null && customRenderTexture.dimension != TextureDimension.Cube)
          flag = false;
      }
      if (flag)
      {
        int num = 0;
        int intValue = this.m_CubeFaceMask.intValue;
        Rect rect = GUILayoutUtility.GetRect(0.0f, (float) ((double) EditorGUIUtility.singleLineHeight * 3.0 + (double) EditorGUIUtility.standardVerticalSpacing * 2.0));
        EditorGUI.BeginProperty(rect, GUIContent.none, this.m_CubeFaceMask);
        Rect position = rect;
        position.width = 100f;
        position.height = EditorGUIUtility.singleLineHeight;
        int index1 = 0;
        EditorGUI.LabelField(rect, CustomRenderTextureEditor.styles.cubemapFacesLabel);
        EditorGUI.BeginChangeCheck();
        for (int index2 = 0; index2 < 3; ++index2)
        {
          position.x = (float) ((double) rect.x + (double) EditorGUIUtility.labelWidth - 15.0);
          for (int index3 = 0; index3 < 2; ++index3)
          {
            if (EditorGUI.ToggleLeft(position, CustomRenderTextureEditor.styles.cubemapFaces[index1], (intValue & 1 << index1) != 0))
              num |= 1 << index1;
            ++index1;
            position.x += 100f;
          }
          position.y += EditorGUIUtility.singleLineHeight;
        }
        if (EditorGUI.EndChangeCheck())
          this.m_CubeFaceMask.intValue = num;
        EditorGUI.EndProperty();
      }
      EditorGUILayout.IntPopup(this.m_UpdateZoneSpace, CustomRenderTextureEditor.styles.updateZoneSpaceStrings, CustomRenderTextureEditor.styles.updateZoneSpaceValues, CustomRenderTextureEditor.styles.updateZoneSpace, new GUILayoutOption[0]);
      if (!this.multipleEditing)
      {
        Rect rect = GUILayoutUtility.GetRect(0.0f, (float) ((double) this.m_RectList.GetHeight() + 16.0), new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
        float num = 15f;
        rect.x += num;
        rect.width -= num;
        this.m_RectList.DoList(rect);
      }
      else
        EditorGUILayout.HelpBox("Update Zones cannot be changed while editing multiple Custom Textures.", MessageType.Info);
      --EditorGUI.indentLevel;
    }

    private void DisplayCustomRenderTextureGUI()
    {
      CustomRenderTexture target = this.target as CustomRenderTexture;
      this.DisplayMaterialGUI();
      EditorGUILayout.Space();
      this.DisplayInitializationGUI();
      EditorGUILayout.Space();
      this.DisplayUpdateGUI();
      EditorGUILayout.Space();
      if (target.updateMode == CustomRenderTextureUpdateMode.Realtime || target.initializationMode != CustomRenderTextureUpdateMode.Realtime)
        return;
      EditorGUILayout.HelpBox("Initialization Mode is set to Realtime but Update Mode is not. This will result in update never being visible.", MessageType.Warning);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.DisplayRenderTextureGUI();
      this.DisplayCustomRenderTextureGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    [MenuItem("CONTEXT/CustomRenderTexture/Export", false)]
    private static void SaveToDisk(MenuCommand command)
    {
      CustomRenderTexture context = command.context as CustomRenderTexture;
      int width1 = context.width;
      int height = context.height;
      int volumeDepth = context.volumeDepth;
      bool flag1 = RenderTextureEditor.IsHDRFormat(context.format);
      bool flag2 = context.format == RenderTextureFormat.ARGBFloat || context.format == RenderTextureFormat.RFloat;
      TextureFormat format = !flag1 ? TextureFormat.RGBA32 : TextureFormat.RGBAFloat;
      int width2 = width1;
      if (context.dimension == TextureDimension.Tex3D)
        width2 = width1 * volumeDepth;
      else if (context.dimension == TextureDimension.Cube)
        width2 = width1 * 6;
      Texture2D tex = new Texture2D(width2, height, format, false);
      if (context.dimension == TextureDimension.Tex2D)
      {
        Graphics.SetRenderTarget((RenderTexture) context);
        tex.ReadPixels(new Rect(0.0f, 0.0f, (float) width1, (float) height), 0, 0);
        tex.Apply();
      }
      else if (context.dimension == TextureDimension.Tex3D)
      {
        int destX = 0;
        for (int depthSlice = 0; depthSlice < volumeDepth; ++depthSlice)
        {
          Graphics.SetRenderTarget((RenderTexture) context, 0, CubemapFace.Unknown, depthSlice);
          tex.ReadPixels(new Rect(0.0f, 0.0f, (float) width1, (float) height), destX, 0);
          tex.Apply();
          destX += width1;
        }
      }
      else
      {
        int destX = 0;
        for (int index = 0; index < 6; ++index)
        {
          Graphics.SetRenderTarget((RenderTexture) context, 0, (CubemapFace) index);
          tex.ReadPixels(new Rect(0.0f, 0.0f, (float) width1, (float) height), destX, 0);
          tex.Apply();
          destX += width1;
        }
      }
      byte[] bytes = !flag1 ? tex.EncodeToPNG() : tex.EncodeToEXR((Texture2D.EXRFlags) (2 | (!flag2 ? 0 : 1)));
      Object.DestroyImmediate((Object) tex);
      string extension = !flag1 ? "png" : "exr";
      string path = EditorUtility.SaveFilePanel("Save Custom Render Texture", Path.GetDirectoryName(AssetDatabase.GetAssetPath(context.GetInstanceID())), context.name, extension);
      if (string.IsNullOrEmpty(path))
        return;
      File.WriteAllBytes(path, bytes);
      AssetDatabase.Refresh();
    }

    public override string GetInfoString()
    {
      return base.GetInfoString();
    }

    private class Styles
    {
      public readonly GUIStyle separator = (GUIStyle) "sv_iconselector_sep";
      public readonly GUIContent materials = EditorGUIUtility.TextContent("Materials");
      public readonly GUIContent shaderPass = EditorGUIUtility.TextContent("Shader Pass|Shader Pass used to update the Custom Render Texture.");
      public readonly GUIContent needSwap = EditorGUIUtility.TextContent("Swap (Double Buffer)|If ticked, and if the texture is double buffered, a request is made to swap the buffers before the next update. If this is not ticked, the buffers will not be swapped.");
      public readonly GUIContent updateMode = EditorGUIUtility.TextContent("Update Mode|Specify how the texture should be updated.");
      public readonly GUIContent updatePeriod = EditorGUIUtility.TextContent("Period|Period in seconds at which real-time textures are updated (0.0 will update every frame).");
      public readonly GUIContent doubleBuffered = EditorGUIUtility.TextContent("Double Buffered|If ticked, the Custom Render Texture is double buffered so that you can access it during its own update. If unticked, the Custom Render Texture will be not be double buffered.");
      public readonly GUIContent initializationMode = EditorGUIUtility.TextContent("Initialization Mode|Specify how the texture should be initialized.");
      public readonly GUIContent initSource = EditorGUIUtility.TextContent("Source|Specify if the texture is initialized by a Material or by a Texture and a Color.");
      public readonly GUIContent initColor = EditorGUIUtility.TextContent("Color|Color with which the Custom Render Texture is initialized.");
      public readonly GUIContent initTexture = EditorGUIUtility.TextContent("Texture|Texture with which the Custom Render Texture is initialized (multiplied by the initialization color).");
      public readonly GUIContent initMaterial = EditorGUIUtility.TextContent("Material|Material with which the Custom Render Texture is initialized.");
      public readonly GUIContent updateZoneSpace = EditorGUIUtility.TextContent("Update Zone Space|Space in which the update zones are expressed (Normalized or Pixel space).");
      public readonly GUIContent updateZoneList = EditorGUIUtility.TextContent("Update Zones|List of partial update zones.");
      public readonly GUIContent cubemapFacesLabel = EditorGUIUtility.TextContent("Cubemap Faces|Enable or disable rendering on each face of the cubemap.");
      public readonly GUIContent updateZoneCenter = EditorGUIUtility.TextContent("Center|Center of the partial update zone.");
      public readonly GUIContent updateZoneSize = EditorGUIUtility.TextContent("Size|Size of the partial update zone.");
      public readonly GUIContent updateZoneRotation = EditorGUIUtility.TextContent("Rotation|Rotation of the update zone.");
      public readonly GUIContent wrapUpdateZones = EditorGUIUtility.TextContent("Wrap Update Zones|If ticked, Update zones will wrap around the border of the Custom Render Texture. If unticked, Update zones will be clamped at the border of the Custom Render Texture.");
      public readonly GUIContent saveButton = EditorGUIUtility.TextContent("Save Texture|Save the content of the Custom Render Texture to an EXR or PNG file.");
      public readonly GUIContent[] updateModeStrings = new GUIContent[3]{ EditorGUIUtility.TextContent("OnLoad"), EditorGUIUtility.TextContent("Realtime"), EditorGUIUtility.TextContent("OnDemand") };
      public readonly int[] updateModeValues = new int[3]{ 0, 1, 2 };
      public readonly GUIContent[] initSourceStrings = new GUIContent[2]{ EditorGUIUtility.TextContent("Texture and Color"), EditorGUIUtility.TextContent("Material") };
      public readonly int[] initSourceValues = new int[2]{ 0, 1 };
      public readonly GUIContent[] updateZoneSpaceStrings = new GUIContent[2]{ EditorGUIUtility.TextContent("Normalized"), EditorGUIUtility.TextContent("Pixel") };
      public readonly int[] updateZoneSpaceValues = new int[2]{ 0, 1 };
      public readonly GUIContent[] cubemapFaces = new GUIContent[6]{ EditorGUIUtility.TextContent("+X"), EditorGUIUtility.TextContent("-X"), EditorGUIUtility.TextContent("+Y"), EditorGUIUtility.TextContent("-Y"), EditorGUIUtility.TextContent("+Z"), EditorGUIUtility.TextContent("-Z") };
    }
  }
}
