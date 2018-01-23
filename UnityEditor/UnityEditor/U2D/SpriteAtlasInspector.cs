// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpriteAtlasInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.U2D.Common;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEditor.U2D
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SpriteAtlas))]
  internal class SpriteAtlasInspector : Editor
  {
    private readonly string kDefaultPlatformName = "default";
    private int m_PreviewPage = 0;
    private int m_TotalPages = 0;
    private int[] m_OptionValues = (int[]) null;
    private string[] m_OptionDisplays = (string[]) null;
    private Texture2D[] m_PreviewTextures = (Texture2D[]) null;
    private bool m_PackableListExpanded = true;
    private float m_MipLevel = 0.0f;
    private static SpriteAtlasInspector.Styles s_Styles;
    private SerializedProperty m_MaxTextureSize;
    private SerializedProperty m_TextureCompression;
    private SerializedProperty m_UseCrunchedCompression;
    private SerializedProperty m_CompressionQuality;
    private SerializedProperty m_FilterMode;
    private SerializedProperty m_AnisoLevel;
    private SerializedProperty m_GenerateMipMaps;
    private SerializedProperty m_Readable;
    private SerializedProperty m_UseSRGB;
    private SerializedProperty m_EnableTightPacking;
    private SerializedProperty m_EnableRotation;
    private SerializedProperty m_Padding;
    private SerializedProperty m_BindAsDefault;
    private SerializedProperty m_Packables;
    private SerializedProperty m_MasterAtlas;
    private SerializedProperty m_VariantMultiplier;
    private string m_Hash;
    private ReorderableList m_PackableList;
    private bool m_ShowAlpha;
    private List<BuildPlatform> m_ValidPlatforms;
    private Dictionary<string, List<TextureImporterPlatformSettings>> m_TempPlatformSettings;
    private ITexturePlatformSettingsView m_TexturePlatformSettingsView;
    private ITexturePlatformSettingsFormatHelper m_TexturePlatformSettingTextureHelper;
    private ITexturePlatformSettingsController m_TexturePlatformSettingsController;

    private SpriteAtlas spriteAtlas
    {
      get
      {
        return this.target as SpriteAtlas;
      }
    }

    private static bool IsPackable(UnityEngine.Object o)
    {
      return o != (UnityEngine.Object) null && (o.GetType() == typeof (Sprite) || o.GetType() == typeof (Texture2D) || o.GetType() == typeof (DefaultAsset) && ProjectWindowUtil.IsFolder(o.GetInstanceID()));
    }

    private static UnityEngine.Object ValidateObjectForPackableFieldAssignment(UnityEngine.Object[] references, System.Type objType, SerializedProperty property, EditorGUI.ObjectFieldValidatorOptions options)
    {
      if (references.Length > 0 && SpriteAtlasInspector.IsPackable(references[0]))
        return references[0];
      return (UnityEngine.Object) null;
    }

    private bool AllTargetsAreVariant()
    {
      foreach (SpriteAtlas target in this.targets)
      {
        if (!target.isVariant)
          return false;
      }
      return true;
    }

    private bool AllTargetsAreMaster()
    {
      foreach (SpriteAtlas target in this.targets)
      {
        if (target.isVariant)
          return false;
      }
      return true;
    }

    private void OnEnable()
    {
      this.m_MaxTextureSize = this.serializedObject.FindProperty("m_EditorData.textureSettings.maxTextureSize");
      this.m_TextureCompression = this.serializedObject.FindProperty("m_EditorData.textureSettings.textureCompression");
      this.m_UseCrunchedCompression = this.serializedObject.FindProperty("m_EditorData.textureSettings.crunchedCompression");
      this.m_CompressionQuality = this.serializedObject.FindProperty("m_EditorData.textureSettings.compressionQuality");
      this.m_FilterMode = this.serializedObject.FindProperty("m_EditorData.textureSettings.filterMode");
      this.m_AnisoLevel = this.serializedObject.FindProperty("m_EditorData.textureSettings.anisoLevel");
      this.m_GenerateMipMaps = this.serializedObject.FindProperty("m_EditorData.textureSettings.generateMipMaps");
      this.m_Readable = this.serializedObject.FindProperty("m_EditorData.textureSettings.readable");
      this.m_UseSRGB = this.serializedObject.FindProperty("m_EditorData.textureSettings.sRGB");
      this.m_EnableTightPacking = this.serializedObject.FindProperty("m_EditorData.packingParameters.enableTightPacking");
      this.m_EnableRotation = this.serializedObject.FindProperty("m_EditorData.packingParameters.enableRotation");
      this.m_Padding = this.serializedObject.FindProperty("m_EditorData.packingParameters.padding");
      this.m_MasterAtlas = this.serializedObject.FindProperty("m_MasterAtlas");
      this.m_BindAsDefault = this.serializedObject.FindProperty("m_EditorData.bindAsDefault");
      this.m_VariantMultiplier = this.serializedObject.FindProperty("m_EditorData.variantMultiplier");
      this.m_Packables = this.serializedObject.FindProperty("m_EditorData.packables");
      this.m_PackableList = new ReorderableList(this.serializedObject, this.m_Packables, true, true, true, true);
      this.m_PackableList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddPackable);
      this.m_PackableList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemovePackable);
      this.m_PackableList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawPackableElement);
      this.m_PackableList.elementHeight = EditorGUIUtility.singleLineHeight;
      this.m_PackableList.headerHeight = 0.0f;
      this.SyncPlatformSettings();
      TextureImporterInspector.InitializeTextureFormatStrings();
      this.m_TexturePlatformSettingsView = (ITexturePlatformSettingsView) new SpriteAtlasInspector.SpriteAtlasInspectorPlatformSettingView(this.AllTargetsAreMaster());
      this.m_TexturePlatformSettingTextureHelper = (ITexturePlatformSettingsFormatHelper) new TexturePlatformSettingsFormatHelper();
      this.m_TexturePlatformSettingsController = (ITexturePlatformSettingsController) new TexturePlatformSettingsViewController();
    }

    private void SyncPlatformSettings()
    {
      this.m_TempPlatformSettings = new Dictionary<string, List<TextureImporterPlatformSettings>>();
      List<TextureImporterPlatformSettings> platformSettingsList1 = new List<TextureImporterPlatformSettings>();
      this.m_TempPlatformSettings.Add(this.kDefaultPlatformName, platformSettingsList1);
      foreach (UnityEngine.Object target in this.targets)
      {
        TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
        platformSettings.name = this.kDefaultPlatformName;
        SerializedObject serializedObject = new SerializedObject(target);
        platformSettings.maxTextureSize = serializedObject.FindProperty("m_EditorData.textureSettings.maxTextureSize").intValue;
        platformSettings.textureCompression = (TextureImporterCompression) serializedObject.FindProperty("m_EditorData.textureSettings.textureCompression").enumValueIndex;
        platformSettings.crunchedCompression = serializedObject.FindProperty("m_EditorData.textureSettings.crunchedCompression").boolValue;
        platformSettings.compressionQuality = serializedObject.FindProperty("m_EditorData.textureSettings.compressionQuality").intValue;
        platformSettingsList1.Add(platformSettings);
      }
      this.m_ValidPlatforms = BuildPlatforms.instance.GetValidPlatforms();
      foreach (BuildPlatform validPlatform in this.m_ValidPlatforms)
      {
        List<TextureImporterPlatformSettings> platformSettingsList2 = new List<TextureImporterPlatformSettings>();
        this.m_TempPlatformSettings.Add(validPlatform.name, platformSettingsList2);
        foreach (SpriteAtlas target in this.targets)
        {
          TextureImporterPlatformSettings dest = new TextureImporterPlatformSettings();
          dest.name = validPlatform.name;
          target.CopyPlatformSettingsIfAvailable(validPlatform.name, dest);
          platformSettingsList2.Add(dest);
        }
      }
    }

    private void AddPackable(ReorderableList list)
    {
      ObjectSelector.get.Show((UnityEngine.Object) null, typeof (UnityEngine.Object), (SerializedProperty) null, false);
      ObjectSelector.get.searchFilter = "t:sprite t:texture2d t:folder";
      ObjectSelector.get.objectSelectorID = SpriteAtlasInspector.s_Styles.packableSelectorHash;
    }

    private void RemovePackable(ReorderableList list)
    {
      int index = list.index;
      if (index == -1)
        return;
      this.spriteAtlas.RemoveAt(index);
    }

    private void DrawPackableElement(Rect rect, int index, bool selected, bool focused)
    {
      SerializedProperty arrayElementAtIndex = this.m_Packables.GetArrayElementAtIndex(index);
      int controlId = GUIUtility.GetControlID(SpriteAtlasInspector.s_Styles.packableElementHash, FocusType.Passive);
      UnityEngine.Object objectReferenceValue = arrayElementAtIndex.objectReferenceValue;
      EditorGUI.BeginChangeCheck();
      Rect position = rect;
      Rect dropRect = rect;
      int id = controlId;
      UnityEngine.Object object1 = objectReferenceValue;
      System.Type objType = typeof (UnityEngine.Object);
      // ISSUE: variable of the null type
      __Null local = null;
      // ISSUE: reference to a compiler-generated field
      if (SpriteAtlasInspector.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SpriteAtlasInspector.\u003C\u003Ef__mg\u0024cache0 = new EditorGUI.ObjectFieldValidator(SpriteAtlasInspector.ValidateObjectForPackableFieldAssignment);
      }
      // ISSUE: reference to a compiler-generated field
      EditorGUI.ObjectFieldValidator fMgCache0 = SpriteAtlasInspector.\u003C\u003Ef__mg\u0024cache0;
      int num = 0;
      UnityEngine.Object object2 = EditorGUI.DoObjectField(position, dropRect, id, object1, objType, (SerializedProperty) local, fMgCache0, num != 0);
      if (EditorGUI.EndChangeCheck())
      {
        if (objectReferenceValue != (UnityEngine.Object) null)
          this.spriteAtlas.Remove(new UnityEngine.Object[1]
          {
            objectReferenceValue
          });
        arrayElementAtIndex.objectReferenceValue = object2;
      }
      if (GUIUtility.keyboardControl != controlId || selected)
        return;
      this.m_PackableList.index = index;
    }

    public override void OnInspectorGUI()
    {
      SpriteAtlasInspector.s_Styles = SpriteAtlasInspector.s_Styles ?? new SpriteAtlasInspector.Styles();
      this.serializedObject.Update();
      this.HandleCommonSettingUI();
      GUILayout.Space(5f);
      if (this.AllTargetsAreVariant())
        this.HandleVariantSettingUI();
      else if (this.AllTargetsAreMaster())
        this.HandleMasterSettingUI();
      GUILayout.Space(5f);
      this.HandleTextureSettingUI();
      GUILayout.Space(5f);
      if (this.targets.Length == 1 && this.AllTargetsAreMaster())
        this.HandlePackableListUI();
      if (EditorSettings.spritePackerMode == SpritePackerMode.BuildTimeOnlyAtlas || EditorSettings.spritePackerMode == SpritePackerMode.AlwaysOnAtlas)
      {
        if (GUILayout.Button(SpriteAtlasInspector.s_Styles.packButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        {
          SpriteAtlas[] atlases = new SpriteAtlas[this.targets.Length];
          for (int index = 0; index < atlases.Length; ++index)
            atlases[index] = (SpriteAtlas) this.targets[index];
          SpriteAtlasUtility.PackAtlases(atlases, EditorUserBuildSettings.activeBuildTarget);
          this.SyncPlatformSettings();
          GUIUtility.ExitGUI();
        }
      }
      else
        EditorGUILayout.HelpBox(SpriteAtlasInspector.s_Styles.disabledPackLabel.text, MessageType.Info);
      this.serializedObject.ApplyModifiedProperties();
    }

    private void HandleCommonSettingUI()
    {
      SpriteAtlasInspector.AtlasType atlasType1 = SpriteAtlasInspector.AtlasType.Undefined;
      if (this.AllTargetsAreMaster())
        atlasType1 = SpriteAtlasInspector.AtlasType.Master;
      else if (this.AllTargetsAreVariant())
        atlasType1 = SpriteAtlasInspector.AtlasType.Variant;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = atlasType1 == SpriteAtlasInspector.AtlasType.Undefined;
      SpriteAtlasInspector.AtlasType atlasType2 = (SpriteAtlasInspector.AtlasType) EditorGUILayout.IntPopup(SpriteAtlasInspector.s_Styles.atlasTypeLabel, (int) atlasType1, SpriteAtlasInspector.s_Styles.atlasTypeOptions, SpriteAtlasInspector.s_Styles.atlasTypeValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        bool flag = atlasType2 == SpriteAtlasInspector.AtlasType.Variant;
        foreach (SpriteAtlas target in this.targets)
          target.SetIsVariant(flag);
        this.m_TexturePlatformSettingsView = (ITexturePlatformSettingsView) new SpriteAtlasInspector.SpriteAtlasInspectorPlatformSettingView(this.AllTargetsAreMaster());
      }
      if (atlasType2 == SpriteAtlasInspector.AtlasType.Variant)
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_MasterAtlas, SpriteAtlasInspector.s_Styles.masterAtlasLabel, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          this.serializedObject.ApplyModifiedProperties();
          foreach (SpriteAtlas target in this.targets)
          {
            target.CopyMasterAtlasSettings();
            this.SyncPlatformSettings();
          }
        }
      }
      EditorGUILayout.PropertyField(this.m_BindAsDefault, SpriteAtlasInspector.s_Styles.bindAsDefaultLabel, new GUILayoutOption[0]);
    }

    private void HandleVariantSettingUI()
    {
      EditorGUILayout.LabelField(SpriteAtlasInspector.s_Styles.variantSettingLabel, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VariantMultiplier, SpriteAtlasInspector.s_Styles.variantMultiplierLabel, new GUILayoutOption[0]);
      if (Mathf.IsPowerOfTwo((int) ((double) this.m_VariantMultiplier.floatValue * 1024.0)))
        return;
      EditorGUILayout.HelpBox(SpriteAtlasInspector.s_Styles.notPowerOfTwoWarning.text, MessageType.Warning, true);
    }

    private void HandleBoolToIntPropertyField(SerializedProperty prop, GUIContent content)
    {
      Rect controlRect = EditorGUILayout.GetControlRect();
      EditorGUI.BeginProperty(controlRect, content, prop);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUI.Toggle(controlRect, content, prop.boolValue);
      if (EditorGUI.EndChangeCheck())
        prop.boolValue = flag;
      EditorGUI.EndProperty();
    }

    private void HandleMasterSettingUI()
    {
      EditorGUILayout.LabelField(SpriteAtlasInspector.s_Styles.packingParametersLabel, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.HandleBoolToIntPropertyField(this.m_EnableRotation, SpriteAtlasInspector.s_Styles.enableRotationLabel);
      this.HandleBoolToIntPropertyField(this.m_EnableTightPacking, SpriteAtlasInspector.s_Styles.enableTightPackingLabel);
      EditorGUILayout.IntPopup(this.m_Padding, SpriteAtlasInspector.s_Styles.paddingOptions, SpriteAtlasInspector.s_Styles.paddingValues, SpriteAtlasInspector.s_Styles.paddingLabel, new GUILayoutOption[0]);
      GUILayout.Space(5f);
    }

    private void HandleTextureSettingUI()
    {
      EditorGUILayout.LabelField(SpriteAtlasInspector.s_Styles.textureSettingLabel, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.HandleBoolToIntPropertyField(this.m_Readable, SpriteAtlasInspector.s_Styles.readWrite);
      this.HandleBoolToIntPropertyField(this.m_GenerateMipMaps, SpriteAtlasInspector.s_Styles.generateMipMapLabel);
      this.HandleBoolToIntPropertyField(this.m_UseSRGB, SpriteAtlasInspector.s_Styles.sRGBLabel);
      EditorGUILayout.PropertyField(this.m_FilterMode);
      if (!this.m_FilterMode.hasMultipleDifferentValues && !this.m_GenerateMipMaps.hasMultipleDifferentValues && this.m_FilterMode.intValue != 0 && this.m_GenerateMipMaps.boolValue)
        EditorGUILayout.IntSlider(this.m_AnisoLevel, 0, 16);
      GUILayout.Space(5f);
      this.HandlePlatformSettingUI();
    }

    private void HandlePlatformSettingUI()
    {
      int index1 = EditorGUILayout.BeginPlatformGrouping(this.m_ValidPlatforms.ToArray(), SpriteAtlasInspector.s_Styles.defaultPlatformLabel);
      if (index1 == -1)
      {
        List<TextureImporterPlatformSettings> tempPlatformSetting = this.m_TempPlatformSettings[this.kDefaultPlatformName];
        List<TextureImporterPlatformSettings> platformSettings = new List<TextureImporterPlatformSettings>(tempPlatformSetting.Count);
        for (int index2 = 0; index2 < tempPlatformSetting.Count; ++index2)
        {
          TextureImporterPlatformSettings target = new TextureImporterPlatformSettings();
          tempPlatformSetting[index2].CopyTo(target);
          platformSettings.Add(target);
        }
        if (this.m_TexturePlatformSettingsController.HandleDefaultSettings(platformSettings, this.m_TexturePlatformSettingsView))
        {
          for (int index2 = 0; index2 < platformSettings.Count; ++index2)
          {
            if (tempPlatformSetting[index2].maxTextureSize != platformSettings[index2].maxTextureSize)
              this.m_MaxTextureSize.intValue = platformSettings[index2].maxTextureSize;
            if (tempPlatformSetting[index2].textureCompression != platformSettings[index2].textureCompression)
              this.m_TextureCompression.enumValueIndex = (int) platformSettings[index2].textureCompression;
            if (tempPlatformSetting[index2].crunchedCompression != platformSettings[index2].crunchedCompression)
              this.m_UseCrunchedCompression.boolValue = platformSettings[index2].crunchedCompression;
            if (tempPlatformSetting[index2].compressionQuality != platformSettings[index2].compressionQuality)
              this.m_CompressionQuality.intValue = platformSettings[index2].compressionQuality;
            platformSettings[index2].CopyTo(tempPlatformSetting[index2]);
          }
        }
      }
      else
      {
        BuildPlatform validPlatform = this.m_ValidPlatforms[index1];
        List<TextureImporterPlatformSettings> tempPlatformSetting = this.m_TempPlatformSettings[validPlatform.name];
        for (int index2 = 0; index2 < tempPlatformSetting.Count; ++index2)
        {
          TextureImporterPlatformSettings platformSettings = tempPlatformSetting[index2];
          if (!platformSettings.overridden)
          {
            SpriteAtlas target = (SpriteAtlas) this.targets[index2];
            platformSettings.format = target.FormatDetermineByAtlasSettings(validPlatform.defaultTarget);
          }
        }
        this.m_TexturePlatformSettingsView.buildPlatformTitle = validPlatform.title.text;
        if (this.m_TexturePlatformSettingsController.HandlePlatformSettings(validPlatform.defaultTarget, tempPlatformSetting, this.m_TexturePlatformSettingsView, this.m_TexturePlatformSettingTextureHelper))
        {
          for (int index2 = 0; index2 < tempPlatformSetting.Count; ++index2)
            ((SpriteAtlas) this.targets[index2]).SetPlatformSettings(tempPlatformSetting[index2]);
        }
      }
      EditorGUILayout.EndPlatformGrouping();
    }

    private void HandlePackableListUI()
    {
      Event current = Event.current;
      bool flag1 = false;
      Rect controlRect = EditorGUILayout.GetControlRect();
      int lastControlId = EditorGUIUtility.s_LastControlID;
      switch (current.type)
      {
        case EventType.DragUpdated:
        case EventType.DragPerform:
          if (controlRect.Contains(current.mousePosition) && GUI.enabled)
          {
            bool flag2 = false;
            foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
            {
              if (SpriteAtlasInspector.IsPackable(objectReference))
              {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (current.type == EventType.DragPerform)
                {
                  this.m_Packables.AppendFoldoutPPtrValue(objectReference);
                  flag2 = true;
                  DragAndDrop.activeControlID = 0;
                }
                else
                  DragAndDrop.activeControlID = lastControlId;
              }
            }
            if (flag2)
            {
              GUI.changed = true;
              DragAndDrop.AcceptDrag();
              flag1 = true;
            }
            break;
          }
          break;
        case EventType.ValidateCommand:
          if (current.commandName == "ObjectSelectorClosed" && ObjectSelector.get.objectSelectorID == SpriteAtlasInspector.s_Styles.packableSelectorHash)
          {
            flag1 = true;
            break;
          }
          break;
        case EventType.ExecuteCommand:
          if (current.commandName == "ObjectSelectorClosed" && ObjectSelector.get.objectSelectorID == SpriteAtlasInspector.s_Styles.packableSelectorHash)
          {
            UnityEngine.Object currentObject = ObjectSelector.GetCurrentObject();
            if (SpriteAtlasInspector.IsPackable(currentObject))
            {
              this.m_Packables.AppendFoldoutPPtrValue(currentObject);
              this.m_PackableList.index = this.m_Packables.arraySize - 1;
            }
            flag1 = true;
            break;
          }
          break;
        case EventType.DragExited:
          if (GUI.enabled)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
      }
      this.m_PackableListExpanded = EditorGUI.Foldout(controlRect, this.m_PackableListExpanded, SpriteAtlasInspector.s_Styles.packableListLabel, true);
      if (flag1)
        current.Use();
      if (!this.m_PackableListExpanded)
        return;
      ++EditorGUI.indentLevel;
      this.m_PackableList.DoLayoutList();
      --EditorGUI.indentLevel;
    }

    private void CachePreviewTexture()
    {
      if (this.m_PreviewTextures != null && !(this.m_Hash != this.spriteAtlas.GetHashString()))
        return;
      this.m_PreviewTextures = this.spriteAtlas.GetPreviewTextures();
      this.m_Hash = this.spriteAtlas.GetHashString();
      if (this.m_PreviewTextures != null && this.m_PreviewTextures.Length > 0 && this.m_TotalPages != this.m_PreviewTextures.Length)
      {
        this.m_TotalPages = this.m_PreviewTextures.Length;
        this.m_OptionDisplays = new string[this.m_TotalPages];
        this.m_OptionValues = new int[this.m_TotalPages];
        for (int index = 0; index < this.m_TotalPages; ++index)
        {
          this.m_OptionDisplays[index] = string.Format("# {0}", (object) (index + 1));
          this.m_OptionValues[index] = index;
        }
      }
    }

    public override string GetInfoString()
    {
      if (this.m_PreviewTextures == null || this.m_PreviewPage >= this.m_PreviewTextures.Length)
        return "";
      Texture2D previewTexture = this.m_PreviewTextures[this.m_PreviewPage];
      TextureFormat textureFormat = TextureUtil.GetTextureFormat((Texture) previewTexture);
      return string.Format("{0}x{1} {2}\n{3}", (object) previewTexture.width, (object) previewTexture.height, (object) TextureUtil.GetTextureFormatString(textureFormat), (object) EditorUtility.FormatBytes(TextureUtil.GetStorageMemorySizeLong((Texture) previewTexture)));
    }

    public override bool HasPreviewGUI()
    {
      this.CachePreviewTexture();
      return this.m_PreviewTextures != null && this.m_PreviewTextures.Length > 0;
    }

    public override void OnPreviewSettings()
    {
      if (this.targets.Length == 1 && this.m_OptionDisplays != null && (this.m_OptionValues != null && this.m_TotalPages > 1))
        this.m_PreviewPage = EditorGUILayout.IntPopup(this.m_PreviewPage, this.m_OptionDisplays, this.m_OptionValues, SpriteAtlasInspector.s_Styles.preDropDown, new GUILayoutOption[1]
        {
          GUILayout.MaxWidth(50f)
        });
      else
        this.m_PreviewPage = 0;
      if (this.m_PreviewTextures == null)
        return;
      Texture2D previewTexture = this.m_PreviewTextures[this.m_PreviewPage];
      if (TextureUtil.HasAlphaTextureFormat(previewTexture.format))
        this.m_ShowAlpha = GUILayout.Toggle(this.m_ShowAlpha, !this.m_ShowAlpha ? SpriteAtlasInspector.s_Styles.RGBIcon : SpriteAtlasInspector.s_Styles.alphaIcon, SpriteAtlasInspector.s_Styles.previewButton, new GUILayoutOption[0]);
      int num = Mathf.Max(1, TextureUtil.GetMipmapCount((Texture) previewTexture));
      if (num > 1)
      {
        GUILayout.Box(SpriteAtlasInspector.s_Styles.smallZoom, SpriteAtlasInspector.s_Styles.previewLabel, new GUILayoutOption[0]);
        this.m_MipLevel = Mathf.Round(GUILayout.HorizontalSlider(this.m_MipLevel, (float) (num - 1), 0.0f, SpriteAtlasInspector.s_Styles.previewSlider, SpriteAtlasInspector.s_Styles.previewSliderThumb, GUILayout.MaxWidth(64f)));
        GUILayout.Box(SpriteAtlasInspector.s_Styles.largeZoom, SpriteAtlasInspector.s_Styles.previewLabel, new GUILayoutOption[0]);
      }
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      this.CachePreviewTexture();
      if (this.m_PreviewTextures == null || this.m_PreviewPage >= this.m_PreviewTextures.Length)
        return;
      Texture2D previewTexture = this.m_PreviewTextures[this.m_PreviewPage];
      float mipMapBias = previewTexture.mipMapBias;
      float bias = this.m_MipLevel - (float) (Math.Log((double) previewTexture.width / (double) r.width) / Math.Log(2.0));
      TextureUtil.SetMipMapBiasNoDirty((Texture) previewTexture, bias);
      if (this.m_ShowAlpha)
        EditorGUI.DrawTextureAlpha(r, (Texture) previewTexture, ScaleMode.ScaleToFit);
      else
        EditorGUI.DrawTextureTransparent(r, (Texture) previewTexture, ScaleMode.ScaleToFit);
      TextureUtil.SetMipMapBiasNoDirty((Texture) previewTexture, mipMapBias);
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      SpriteAtlas spriteAtlas = AssetDatabase.LoadMainAssetAtPath(assetPath) as SpriteAtlas;
      if ((UnityEngine.Object) spriteAtlas == (UnityEngine.Object) null)
        return (Texture2D) null;
      Texture2D[] previewTextures = spriteAtlas.GetPreviewTextures();
      if (previewTextures == null || previewTextures.Length == 0)
        return (Texture2D) null;
      Texture2D original = previewTextures[0];
      PreviewHelpers.AdjustWidthAndHeightForStaticPreview(original.width, original.height, ref width, ref height);
      return SpriteUtility.CreateTemporaryDuplicate(original, width, height);
    }

    private class SpriteAtlasInspectorPlatformSettingView : TexturePlatformSettingsView
    {
      private bool m_ShowMaxSizeOption;

      public SpriteAtlasInspectorPlatformSettingView(bool showMaxSizeOption)
      {
        this.m_ShowMaxSizeOption = showMaxSizeOption;
      }

      public override int DrawMaxSize(int defaultValue, bool isMixedValue, out bool changed)
      {
        if (this.m_ShowMaxSizeOption)
          return base.DrawMaxSize(defaultValue, isMixedValue, out changed);
        changed = false;
        return defaultValue;
      }
    }

    private class Styles
    {
      public readonly GUIStyle dropzoneStyle = new GUIStyle((GUIStyle) "BoldLabel");
      public readonly GUIStyle preDropDown = (GUIStyle) nameof (preDropDown);
      public readonly GUIStyle previewButton = (GUIStyle) "preButton";
      public readonly GUIStyle previewSlider = (GUIStyle) "preSlider";
      public readonly GUIStyle previewSliderThumb = (GUIStyle) "preSliderThumb";
      public readonly GUIStyle previewLabel = new GUIStyle((GUIStyle) "preLabel");
      public readonly GUIContent textureSettingLabel = EditorGUIUtility.TextContent("Texture");
      public readonly GUIContent variantSettingLabel = EditorGUIUtility.TextContent("Variant");
      public readonly GUIContent packingParametersLabel = EditorGUIUtility.TextContent("Packing");
      public readonly GUIContent atlasTypeLabel = EditorGUIUtility.TextContent("Type");
      public readonly GUIContent defaultPlatformLabel = EditorGUIUtility.TextContent("Default");
      public readonly GUIContent masterAtlasLabel = EditorGUIUtility.TextContent("Master Atlas|Assigning another Sprite Atlas asset will make this atlas a variant of it.");
      public readonly GUIContent bindAsDefaultLabel = EditorGUIUtility.TextContent("Include in Build|Packed textures will be included in the build by default.");
      public readonly GUIContent enableRotationLabel = EditorGUIUtility.TextContent("Allow Rotation|Try rotating the sprite to fit better during packing.");
      public readonly GUIContent enableTightPackingLabel = EditorGUIUtility.TextContent("Tight Packing|Use the mesh outline to fit instead of the whole texture rect during packing.");
      public readonly GUIContent paddingLabel = EditorGUIUtility.TextContent("Padding|The amount of extra padding between packed sprites.");
      public readonly GUIContent generateMipMapLabel = EditorGUIUtility.TextContent("Generate Mip Maps");
      public readonly GUIContent sRGBLabel = EditorGUIUtility.TextContent("sRGB|Texture content is stored in gamma space.");
      public readonly GUIContent readWrite = EditorGUIUtility.TextContent("Read/Write Enabled|Enable to be able to access the raw pixel data from code.");
      public readonly GUIContent variantMultiplierLabel = EditorGUIUtility.TextContent("Scale|Down scale ratio.");
      public readonly GUIContent copyMasterButton = EditorGUIUtility.TextContent("Copy Master's Settings|Copy all master's settings into this variant.");
      public readonly GUIContent packButton = EditorGUIUtility.TextContent("Pack Preview|Pack this atlas.");
      public readonly GUIContent disabledPackLabel = EditorGUIUtility.TextContent("Sprite Atlas packing is disabled. Enable it in Edit > Project Settings > Editor.");
      public readonly GUIContent packableListLabel = EditorGUIUtility.TextContent("Objects for Packing|Only accept Folder, Sprite Sheet(Texture) and Sprite.");
      public readonly GUIContent notPowerOfTwoWarning = EditorGUIUtility.TextContent("This scale will produce a Sprite Atlas variant with a packed texture that is NPOT (non - power of two). This may cause visual artifacts in certain compression/texture formats.");
      public readonly GUIContent smallZoom = EditorGUIUtility.IconContent("PreTextureMipMapLow");
      public readonly GUIContent largeZoom = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
      public readonly GUIContent alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
      public readonly GUIContent RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
      public readonly int packableElementHash = "PackableElement".GetHashCode();
      public readonly int packableSelectorHash = "PackableSelector".GetHashCode();
      public readonly int[] atlasTypeValues = new int[2]{ 0, 1 };
      public readonly GUIContent[] atlasTypeOptions = new GUIContent[2]{ EditorGUIUtility.TextContent("Master"), EditorGUIUtility.TextContent("Variant") };
      public readonly int[] paddingValues = new int[3]{ 2, 4, 8 };
      public readonly GUIContent[] paddingOptions;

      public Styles()
      {
        this.dropzoneStyle.alignment = TextAnchor.MiddleCenter;
        this.dropzoneStyle.border = new RectOffset(10, 10, 10, 10);
        this.paddingOptions = new GUIContent[this.paddingValues.Length];
        for (int index = 0; index < this.paddingValues.Length; ++index)
          this.paddingOptions[index] = EditorGUIUtility.TextContent(this.paddingValues[index].ToString());
      }
    }

    private enum AtlasType
    {
      Undefined = -1,
      Master = 0,
      Variant = 1,
    }
  }
}
