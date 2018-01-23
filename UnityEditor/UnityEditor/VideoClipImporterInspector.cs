// Decompiled with JetBrains decompiler
// Type: UnityEditor.VideoClipImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (VideoClipImporter))]
  internal class VideoClipImporterInspector : AssetImporterEditor
  {
    private static string[] s_LegacyFileTypes = new string[7]{ ".ogg", ".ogv", ".mov", ".asf", ".mpg", ".mpeg", ".mp4" };
    private bool m_IsPlaying = false;
    private Vector2 m_Position = Vector2.zero;
    private AnimBool m_ShowResizeModeOptions = new AnimBool();
    private SerializedProperty m_UseLegacyImporter;
    private SerializedProperty m_Quality;
    private SerializedProperty m_IsColorLinear;
    private SerializedProperty m_EncodeAlpha;
    private SerializedProperty m_Deinterlace;
    private SerializedProperty m_FlipVertical;
    private SerializedProperty m_FlipHorizontal;
    private SerializedProperty m_ImportAudio;
    private VideoClipImporterInspector.InspectorTargetSettings[,] m_TargetSettings;
    private bool m_ModifiedTargetSettings;
    private GUIContent m_PreviewTitle;
    private static VideoClipImporterInspector.Styles s_Styles;
    private const int kNarrowLabelWidth = 42;
    private const int kToggleButtonWidth = 16;
    private const int kMinCustomWidth = 1;
    private const int kMaxCustomWidth = 16384;
    private const int kMinCustomHeight = 1;
    private const int kMaxCustomHeight = 16384;

    public override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    private void ResetSettingsFromBackend()
    {
      this.m_TargetSettings = (VideoClipImporterInspector.InspectorTargetSettings[,]) null;
      if (this.targets.Length > 0)
      {
        List<BuildPlatform> validPlatforms = BuildPlatforms.instance.GetValidPlatforms();
        this.m_TargetSettings = new VideoClipImporterInspector.InspectorTargetSettings[this.targets.Length, validPlatforms.Count + 1];
        for (int index1 = 0; index1 < this.targets.Length; ++index1)
        {
          VideoClipImporter target = (VideoClipImporter) this.targets[index1];
          this.m_TargetSettings[index1, 0] = new VideoClipImporterInspector.InspectorTargetSettings();
          this.m_TargetSettings[index1, 0].overridePlatform = true;
          this.m_TargetSettings[index1, 0].settings = target.defaultTargetSettings;
          for (int index2 = 1; index2 < validPlatforms.Count + 1; ++index2)
          {
            BuildTargetGroup targetGroup = validPlatforms[index2 - 1].targetGroup;
            this.m_TargetSettings[index1, index2] = new VideoClipImporterInspector.InspectorTargetSettings();
            this.m_TargetSettings[index1, index2].settings = target.Internal_GetTargetSettings(targetGroup);
            this.m_TargetSettings[index1, index2].overridePlatform = this.m_TargetSettings[index1, index2].settings != null;
          }
        }
      }
      this.m_ModifiedTargetSettings = false;
    }

    private void WriteSettingsToBackend()
    {
      if (this.m_TargetSettings != null)
      {
        List<BuildPlatform> validPlatforms = BuildPlatforms.instance.GetValidPlatforms();
        for (int index1 = 0; index1 < this.targets.Length; ++index1)
        {
          VideoClipImporter target = (VideoClipImporter) this.targets[index1];
          target.defaultTargetSettings = this.m_TargetSettings[index1, 0].settings;
          for (int index2 = 1; index2 < validPlatforms.Count + 1; ++index2)
          {
            BuildTargetGroup targetGroup = validPlatforms[index2 - 1].targetGroup;
            if (this.m_TargetSettings[index1, index2].settings != null && this.m_TargetSettings[index1, index2].overridePlatform)
              target.Internal_SetTargetSettings(targetGroup, this.m_TargetSettings[index1, index2].settings);
            else
              target.Internal_ClearTargetSettings(targetGroup);
          }
        }
      }
      this.m_ModifiedTargetSettings = false;
    }

    public override void OnEnable()
    {
      if (VideoClipImporterInspector.s_Styles == null)
        VideoClipImporterInspector.s_Styles = new VideoClipImporterInspector.Styles();
      this.m_UseLegacyImporter = this.serializedObject.FindProperty("m_UseLegacyImporter");
      this.m_Quality = this.serializedObject.FindProperty("m_Quality");
      this.m_IsColorLinear = this.serializedObject.FindProperty("m_IsColorLinear");
      this.m_EncodeAlpha = this.serializedObject.FindProperty("m_EncodeAlpha");
      this.m_Deinterlace = this.serializedObject.FindProperty("m_Deinterlace");
      this.m_FlipVertical = this.serializedObject.FindProperty("m_FlipVertical");
      this.m_FlipHorizontal = this.serializedObject.FindProperty("m_FlipHorizontal");
      this.m_ImportAudio = this.serializedObject.FindProperty("m_ImportAudio");
      this.ResetSettingsFromBackend();
      VideoClipImporterInspector.MultiTargetSettingState targetSettingState = this.CalculateMultiTargetSettingState(0);
      this.m_ShowResizeModeOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowResizeModeOptions.value = targetSettingState.mixedResizeMode || targetSettingState.firstResizeMode != VideoResizeMode.OriginalSize;
    }

    public override void OnDisable()
    {
      VideoClipImporter target = this.target as VideoClipImporter;
      if ((bool) ((UnityEngine.Object) target))
        target.StopPreview();
      base.OnDisable();
    }

    private List<GUIContent> GetResizeModeList()
    {
      List<GUIContent> guiContentList = new List<GUIContent>();
      VideoClipImporter target = (VideoClipImporter) this.target;
      IEnumerator enumerator = Enum.GetValues(typeof (VideoResizeMode)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          VideoResizeMode current = (VideoResizeMode) enumerator.Current;
          guiContentList.Add(EditorGUIUtility.TextContent(target.GetResizeModeName(current)));
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return guiContentList;
    }

    private bool AnySettingsNotTranscoded()
    {
      if (this.m_TargetSettings != null)
      {
        for (int index1 = 0; index1 < this.m_TargetSettings.GetLength(0); ++index1)
        {
          for (int index2 = 0; index2 < this.m_TargetSettings.GetLength(1); ++index2)
          {
            if (this.m_TargetSettings[index1, index2].settings != null && !this.m_TargetSettings[index1, index2].settings.enableTranscoding)
              return true;
          }
        }
      }
      return false;
    }

    private VideoClipImporterInspector.MultiTargetSettingState CalculateMultiTargetSettingState(int platformIndex)
    {
      VideoClipImporterInspector.MultiTargetSettingState targetSettingState = new VideoClipImporterInspector.MultiTargetSettingState();
      targetSettingState.Init();
      if (this.m_TargetSettings == null || this.m_TargetSettings.Length == 0)
        return targetSettingState;
      int num = -1;
      for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
      {
        if (this.m_TargetSettings[index, platformIndex].overridePlatform)
        {
          if (num == -1)
          {
            num = index;
            targetSettingState.firstTranscoding = this.m_TargetSettings[index, platformIndex].settings.enableTranscoding;
            targetSettingState.firstCodec = this.m_TargetSettings[index, platformIndex].settings.codec;
            targetSettingState.firstResizeMode = this.m_TargetSettings[index, platformIndex].settings.resizeMode;
            targetSettingState.firstAspectRatio = this.m_TargetSettings[index, platformIndex].settings.aspectRatio;
            targetSettingState.firstCustomWidth = this.m_TargetSettings[index, platformIndex].settings.customWidth;
            targetSettingState.firstCustomHeight = this.m_TargetSettings[index, platformIndex].settings.customHeight;
            targetSettingState.firstBitrateMode = this.m_TargetSettings[index, platformIndex].settings.bitrateMode;
            targetSettingState.firstSpatialQuality = this.m_TargetSettings[index, platformIndex].settings.spatialQuality;
          }
          else
          {
            targetSettingState.mixedTranscoding = targetSettingState.firstTranscoding != this.m_TargetSettings[index, platformIndex].settings.enableTranscoding;
            targetSettingState.mixedCodec = targetSettingState.firstCodec != this.m_TargetSettings[index, platformIndex].settings.codec;
            targetSettingState.mixedResizeMode = targetSettingState.firstResizeMode != this.m_TargetSettings[index, platformIndex].settings.resizeMode;
            targetSettingState.mixedAspectRatio = targetSettingState.firstAspectRatio != this.m_TargetSettings[index, platformIndex].settings.aspectRatio;
            targetSettingState.mixedCustomWidth = targetSettingState.firstCustomWidth != this.m_TargetSettings[index, platformIndex].settings.customWidth;
            targetSettingState.mixedCustomHeight = targetSettingState.firstCustomHeight != this.m_TargetSettings[index, platformIndex].settings.customHeight;
            targetSettingState.mixedBitrateMode = targetSettingState.firstBitrateMode != this.m_TargetSettings[index, platformIndex].settings.bitrateMode;
            targetSettingState.mixedSpatialQuality = targetSettingState.firstSpatialQuality != this.m_TargetSettings[index, platformIndex].settings.spatialQuality;
          }
        }
      }
      if (num == -1)
      {
        targetSettingState.firstTranscoding = this.m_TargetSettings[0, 0].settings.enableTranscoding;
        targetSettingState.firstCodec = this.m_TargetSettings[0, 0].settings.codec;
        targetSettingState.firstResizeMode = this.m_TargetSettings[0, 0].settings.resizeMode;
        targetSettingState.firstAspectRatio = this.m_TargetSettings[0, 0].settings.aspectRatio;
        targetSettingState.firstCustomWidth = this.m_TargetSettings[0, 0].settings.customWidth;
        targetSettingState.firstCustomHeight = this.m_TargetSettings[0, 0].settings.customHeight;
        targetSettingState.firstBitrateMode = this.m_TargetSettings[0, 0].settings.bitrateMode;
        targetSettingState.firstSpatialQuality = this.m_TargetSettings[0, 0].settings.spatialQuality;
      }
      return targetSettingState;
    }

    private void OnCrossTargetInspectorGUI()
    {
      bool flag1 = true;
      bool flag2 = true;
      for (int index = 0; index < this.targets.Length; ++index)
      {
        VideoClipImporter target = (VideoClipImporter) this.targets[index];
        flag1 &= target.sourceHasAlpha;
        flag2 &= (int) target.sourceAudioTrackCount > 0;
      }
      if (flag1)
        EditorGUILayout.PropertyField(this.m_EncodeAlpha, VideoClipImporterInspector.s_Styles.keepAlphaContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Deinterlace, VideoClipImporterInspector.s_Styles.deinterlaceContent, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_FlipHorizontal, VideoClipImporterInspector.s_Styles.flipHorizontalContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_FlipVertical, VideoClipImporterInspector.s_Styles.flipVerticalContent, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      using (new EditorGUI.DisabledScope(!flag2))
        EditorGUILayout.PropertyField(this.m_ImportAudio, VideoClipImporterInspector.s_Styles.importAudioContent, new GUILayoutOption[0]);
    }

    private void FrameSettingsGUI(int platformIndex, VideoClipImporterInspector.MultiTargetSettingState multiState)
    {
      EditorGUI.showMixedValue = multiState.mixedResizeMode;
      EditorGUI.BeginChangeCheck();
      VideoResizeMode videoResizeMode = (VideoResizeMode) EditorGUILayout.Popup(VideoClipImporterInspector.s_Styles.dimensionsContent, (int) multiState.firstResizeMode, this.GetResizeModeList().ToArray(), new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
        {
          if (this.m_TargetSettings[index, platformIndex].settings != null)
          {
            this.m_TargetSettings[index, platformIndex].settings.resizeMode = videoResizeMode;
            this.m_ModifiedTargetSettings = true;
          }
        }
      }
      this.m_ShowResizeModeOptions.target = videoResizeMode != VideoResizeMode.OriginalSize;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowResizeModeOptions.faded))
      {
        ++EditorGUI.indentLevel;
        if (videoResizeMode == VideoResizeMode.CustomSize)
        {
          EditorGUI.showMixedValue = multiState.mixedCustomWidth;
          EditorGUI.BeginChangeCheck();
          int num1 = Mathf.Clamp(EditorGUILayout.IntField(VideoClipImporterInspector.s_Styles.widthContent, multiState.firstCustomWidth, new GUILayoutOption[0]), 1, 16384);
          EditorGUI.showMixedValue = false;
          if (EditorGUI.EndChangeCheck())
          {
            for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
            {
              if (this.m_TargetSettings[index, platformIndex].settings != null)
              {
                this.m_TargetSettings[index, platformIndex].settings.customWidth = num1;
                this.m_ModifiedTargetSettings = true;
              }
            }
          }
          EditorGUI.showMixedValue = multiState.mixedCustomHeight;
          EditorGUI.BeginChangeCheck();
          int num2 = Mathf.Clamp(EditorGUILayout.IntField(VideoClipImporterInspector.s_Styles.heightContent, multiState.firstCustomHeight, new GUILayoutOption[0]), 1, 16384);
          EditorGUI.showMixedValue = false;
          if (EditorGUI.EndChangeCheck())
          {
            for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
            {
              if (this.m_TargetSettings[index, platformIndex].settings != null)
              {
                this.m_TargetSettings[index, platformIndex].settings.customHeight = num2;
                this.m_ModifiedTargetSettings = true;
              }
            }
          }
        }
        EditorGUI.showMixedValue = multiState.mixedAspectRatio;
        EditorGUI.BeginChangeCheck();
        VideoEncodeAspectRatio encodeAspectRatio = (VideoEncodeAspectRatio) EditorGUILayout.EnumPopup(VideoClipImporterInspector.s_Styles.aspectRatioContent, (Enum) multiState.firstAspectRatio, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
          {
            if (this.m_TargetSettings[index, platformIndex].settings != null)
            {
              this.m_TargetSettings[index, platformIndex].settings.aspectRatio = encodeAspectRatio;
              this.m_ModifiedTargetSettings = true;
            }
          }
        }
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
    }

    private void EncodingSettingsGUI(int platformIndex, VideoClipImporterInspector.MultiTargetSettingState multiState)
    {
      EditorGUI.showMixedValue = multiState.mixedCodec;
      EditorGUI.BeginChangeCheck();
      VideoCodec videoCodec = (VideoCodec) EditorGUILayout.EnumPopup(VideoClipImporterInspector.s_Styles.codecContent, (Enum) multiState.firstCodec, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
        {
          if (this.m_TargetSettings[index, platformIndex].settings != null)
          {
            this.m_TargetSettings[index, platformIndex].settings.codec = videoCodec;
            this.m_ModifiedTargetSettings = true;
          }
        }
      }
      EditorGUI.showMixedValue = multiState.mixedBitrateMode;
      EditorGUI.BeginChangeCheck();
      VideoBitrateMode videoBitrateMode = (VideoBitrateMode) EditorGUILayout.EnumPopup(VideoClipImporterInspector.s_Styles.bitrateContent, (Enum) multiState.firstBitrateMode, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
        {
          if (this.m_TargetSettings[index, platformIndex].settings != null)
          {
            this.m_TargetSettings[index, platformIndex].settings.bitrateMode = videoBitrateMode;
            this.m_ModifiedTargetSettings = true;
          }
        }
      }
      EditorGUI.showMixedValue = multiState.mixedSpatialQuality;
      EditorGUI.BeginChangeCheck();
      VideoSpatialQuality videoSpatialQuality = (VideoSpatialQuality) EditorGUILayout.EnumPopup(VideoClipImporterInspector.s_Styles.spatialQualityContent, (Enum) multiState.firstSpatialQuality, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
      {
        if (this.m_TargetSettings[index, platformIndex].settings != null)
        {
          this.m_TargetSettings[index, platformIndex].settings.spatialQuality = videoSpatialQuality;
          this.m_ModifiedTargetSettings = true;
        }
      }
    }

    private bool HasMixedOverrideStatus(int platformIndex, out bool overrideState)
    {
      overrideState = false;
      if (this.m_TargetSettings == null || this.m_TargetSettings.Length == 0)
        return false;
      overrideState = this.m_TargetSettings[0, platformIndex].overridePlatform;
      for (int index = 1; index < this.m_TargetSettings.GetLength(0); ++index)
      {
        if (this.m_TargetSettings[index, platformIndex].overridePlatform != overrideState)
          return true;
      }
      return false;
    }

    private VideoImporterTargetSettings CloneTargetSettings(VideoImporterTargetSettings settings)
    {
      return new VideoImporterTargetSettings() { enableTranscoding = settings.enableTranscoding, codec = settings.codec, resizeMode = settings.resizeMode, aspectRatio = settings.aspectRatio, customWidth = settings.customWidth, customHeight = settings.customHeight, bitrateMode = settings.bitrateMode, spatialQuality = settings.spatialQuality };
    }

    private void OnTargetSettingsInspectorGUI(int platformIndex, VideoClipImporterInspector.MultiTargetSettingState multiState)
    {
      EditorGUI.showMixedValue = multiState.mixedTranscoding;
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(VideoClipImporterInspector.s_Styles.transcodeContent, multiState.firstTranscoding, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
        {
          if (this.m_TargetSettings[index, platformIndex].settings != null)
          {
            this.m_TargetSettings[index, platformIndex].settings.enableTranscoding = flag;
            this.m_ModifiedTargetSettings = true;
          }
        }
      }
      ++EditorGUI.indentLevel;
      using (new EditorGUI.DisabledScope((flag ? 1 : (multiState.mixedTranscoding ? 1 : 0)) == 0))
      {
        this.FrameSettingsGUI(platformIndex, multiState);
        this.EncodingSettingsGUI(platformIndex, multiState);
      }
      --EditorGUI.indentLevel;
    }

    private void OnTargetInspectorGUI(int platformIndex, string platformName)
    {
      bool flag1 = true;
      if (platformIndex != 0)
      {
        bool overrideState;
        EditorGUI.showMixedValue = this.HasMixedOverrideStatus(platformIndex, out overrideState);
        EditorGUI.BeginChangeCheck();
        bool flag2 = EditorGUILayout.Toggle("Override for " + platformName, overrideState, new GUILayoutOption[0]);
        flag1 = flag2 || EditorGUI.showMixedValue;
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          for (int index = 0; index < this.m_TargetSettings.GetLength(0); ++index)
          {
            this.m_TargetSettings[index, platformIndex].overridePlatform = flag2;
            this.m_ModifiedTargetSettings = true;
            if (this.m_TargetSettings[index, platformIndex].settings == null)
              this.m_TargetSettings[index, platformIndex].settings = this.CloneTargetSettings(this.m_TargetSettings[index, 0].settings);
          }
        }
      }
      EditorGUILayout.Space();
      VideoClipImporterInspector.MultiTargetSettingState targetSettingState = this.CalculateMultiTargetSettingState(platformIndex);
      using (new EditorGUI.DisabledScope(!flag1))
        this.OnTargetSettingsInspectorGUI(platformIndex, targetSettingState);
    }

    private void OnTargetsInspectorGUI()
    {
      BuildPlatform[] array = BuildPlatforms.instance.GetValidPlatforms().ToArray();
      int index = EditorGUILayout.BeginPlatformGrouping(array, GUIContent.Temp("Default"));
      string platformName = index != -1 ? array[index].name : "Default";
      this.OnTargetInspectorGUI(index + 1, platformName);
      EditorGUILayout.EndPlatformGrouping();
    }

    internal override void OnHeaderControlsGUI()
    {
      this.serializedObject.UpdateIfRequiredOrScript();
      bool flag = true;
      for (int index = 0; flag && index < this.targets.Length; ++index)
      {
        VideoClipImporter target = (VideoClipImporter) this.targets[index];
        flag &= this.IsFileSupportedByLegacy(target.assetPath);
      }
      if (!flag)
      {
        base.OnHeaderControlsGUI();
      }
      else
      {
        EditorGUI.showMixedValue = this.m_UseLegacyImporter.hasMultipleDifferentValues;
        EditorGUI.BeginChangeCheck();
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 100f;
        int num = EditorGUILayout.Popup(VideoClipImporterInspector.s_Styles.importerVersionContent, !this.m_UseLegacyImporter.boolValue ? 0 : 1, VideoClipImporterInspector.s_Styles.importerVersionOptions, EditorStyles.popup, new GUILayoutOption[1]{ GUILayout.MaxWidth(230f) });
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          this.m_UseLegacyImporter.boolValue = num == 1;
        GUILayout.FlexibleSpace();
        if (!GUILayout.Button("Open", EditorStyles.miniButton, new GUILayoutOption[0]))
          return;
        AssetDatabase.OpenAsset(this.assetEditor.targets);
        GUIUtility.ExitGUI();
      }
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.UpdateIfRequiredOrScript();
      if (this.m_UseLegacyImporter.boolValue)
      {
        EditorGUILayout.PropertyField(this.m_IsColorLinear, MovieImporterInspector.linearTextureContent, new GUILayoutOption[0]);
        EditorGUILayout.Slider(this.m_Quality, 0.0f, 1f);
      }
      else
      {
        this.OnCrossTargetInspectorGUI();
        EditorGUILayout.Space();
        this.OnTargetsInspectorGUI();
        if (this.AnySettingsNotTranscoded())
          EditorGUILayout.HelpBox(VideoClipImporterInspector.s_Styles.transcodeWarning.text, MessageType.Info);
      }
      foreach (UnityEngine.Object target in this.targets)
      {
        VideoClipImporter videoClipImporter = target as VideoClipImporter;
        if ((bool) ((UnityEngine.Object) videoClipImporter) && videoClipImporter.transcodeSkipped)
        {
          EditorGUILayout.HelpBox(this.targets.Length != 1 ? VideoClipImporterInspector.s_Styles.multipleTranscodeSkippedWarning.text : VideoClipImporterInspector.s_Styles.transcodeSkippedWarning.text, MessageType.Error);
          break;
        }
      }
      this.ApplyRevertGUI();
    }

    public override bool HasModified()
    {
      if (base.HasModified())
        return true;
      return this.m_ModifiedTargetSettings;
    }

    protected override void Apply()
    {
      base.Apply();
      this.WriteSettingsToBackend();
      foreach (EditorWindow allProjectBrowser in ProjectBrowser.GetAllProjectBrowsers())
        allProjectBrowser.Repaint();
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (UnityEngine.Object) null;
    }

    protected override bool useAssetDrawPreview
    {
      get
      {
        return false;
      }
    }

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_PreviewTitle != null)
        return this.m_PreviewTitle;
      this.m_PreviewTitle = new GUIContent();
      this.m_PreviewTitle.text = this.targets.Length != 1 ? this.targets.Length.ToString() + " Video Clips" : Path.GetFileName(((AssetImporter) this.target).assetPath);
      return this.m_PreviewTitle;
    }

    protected override void ResetValues()
    {
      base.ResetValues();
      this.OnEnable();
    }

    public override void OnPreviewSettings()
    {
      EditorGUI.BeginDisabledGroup(Application.isPlaying || this.HasModified() || ((VideoClipImporter) this.target).useLegacyImporter);
      this.m_IsPlaying = PreviewGUI.CycleButton(!this.m_IsPlaying ? 0 : 1, VideoClipImporterInspector.s_Styles.playIcons) != 0;
      EditorGUI.EndDisabledGroup();
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      background.Draw(r, false, false, false, false);
      VideoClipImporter target = (VideoClipImporter) this.target;
      if (this.m_IsPlaying && !target.isPlayingPreview)
        target.PlayPreview();
      else if (!this.m_IsPlaying && target.isPlayingPreview)
        target.StopPreview();
      Texture previewTexture = target.GetPreviewTexture();
      if (!(bool) ((UnityEngine.Object) previewTexture) || previewTexture.width == 0 || previewTexture.height == 0)
        return;
      float num1 = (float) previewTexture.width;
      float num2 = (float) previewTexture.height;
      if (target.defaultTargetSettings.enableTranscoding)
      {
        VideoResizeMode resizeMode = target.defaultTargetSettings.resizeMode;
        num1 = (float) target.GetResizeWidth(resizeMode);
        num2 = (float) target.GetResizeHeight(resizeMode);
      }
      if (target.pixelAspectRatioDenominator > 0)
        num1 *= (float) target.pixelAspectRatioNumerator / (float) target.pixelAspectRatioDenominator;
      float num3 = Mathf.Clamp01((double) r.width / (double) num1 * (double) num2 <= (double) r.height ? r.width / num1 : r.height / num2);
      Rect rect = new Rect(r.x, r.y, num1 * num3, num2 * num3);
      PreviewGUI.BeginScrollView(r, this.m_Position, rect, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
      EditorGUI.DrawTextureTransparent(rect, previewTexture, ScaleMode.StretchToFill);
      this.m_Position = PreviewGUI.EndScrollView();
      if (!this.m_IsPlaying)
        return;
      GUIView.current.Repaint();
    }

    private bool IsFileSupportedByLegacy(string assetPath)
    {
      return Array.IndexOf<string>(VideoClipImporterInspector.s_LegacyFileTypes, Path.GetExtension(assetPath).ToLower()) != -1;
    }

    internal struct MultiTargetSettingState
    {
      public bool mixedTranscoding;
      public bool mixedCodec;
      public bool mixedResizeMode;
      public bool mixedAspectRatio;
      public bool mixedCustomWidth;
      public bool mixedCustomHeight;
      public bool mixedBitrateMode;
      public bool mixedSpatialQuality;
      public bool firstTranscoding;
      public VideoCodec firstCodec;
      public VideoResizeMode firstResizeMode;
      public VideoEncodeAspectRatio firstAspectRatio;
      public int firstCustomWidth;
      public int firstCustomHeight;
      public VideoBitrateMode firstBitrateMode;
      public VideoSpatialQuality firstSpatialQuality;

      public void Init()
      {
        this.mixedTranscoding = false;
        this.mixedCodec = false;
        this.mixedResizeMode = false;
        this.mixedAspectRatio = false;
        this.mixedCustomWidth = false;
        this.mixedCustomHeight = false;
        this.mixedBitrateMode = false;
        this.mixedSpatialQuality = false;
        this.firstTranscoding = false;
        this.firstCodec = VideoCodec.Auto;
        this.firstResizeMode = VideoResizeMode.OriginalSize;
        this.firstAspectRatio = VideoEncodeAspectRatio.NoScaling;
        this.firstCustomWidth = -1;
        this.firstCustomHeight = -1;
        this.firstBitrateMode = VideoBitrateMode.High;
        this.firstSpatialQuality = VideoSpatialQuality.HighSpatialQuality;
      }
    }

    internal class InspectorTargetSettings
    {
      public bool overridePlatform;
      public VideoImporterTargetSettings settings;
    }

    private class Styles
    {
      public GUIContent[] playIcons = new GUIContent[2]{ EditorGUIUtility.IconContent("preAudioPlayOff"), EditorGUIUtility.IconContent("preAudioPlayOn") };
      public GUIContent keepAlphaContent = EditorGUIUtility.TextContent("Keep Alpha|If the source clip has alpha, this will encode it in the resulting clip so that transparency is usable during render.");
      public GUIContent deinterlaceContent = EditorGUIUtility.TextContent("Deinterlace|Remove interlacing on this video.");
      public GUIContent flipHorizontalContent = EditorGUIUtility.TextContent("Flip Horizontally|Flip the video horizontally during transcoding.");
      public GUIContent flipVerticalContent = EditorGUIUtility.TextContent("Flip Vertically|Flip the video vertically during transcoding.");
      public GUIContent importAudioContent = EditorGUIUtility.TextContent("Import Audio|Defines if the audio tracks will be imported during transcoding.");
      public GUIContent transcodeContent = EditorGUIUtility.TextContent("Transcode|Transcoding a clip gives more flexibility through the options below, but takes more time.");
      public GUIContent dimensionsContent = EditorGUIUtility.TextContent("Dimensions|Pixel size of the resulting video.");
      public GUIContent widthContent = EditorGUIUtility.TextContent("Width|Width in pixels of the resulting video.");
      public GUIContent heightContent = EditorGUIUtility.TextContent("Height|Height in pixels of the resulting video.");
      public GUIContent aspectRatioContent = EditorGUIUtility.TextContent("Aspect Ratio|How the original video is mapped into the target dimensions.");
      public GUIContent codecContent = EditorGUIUtility.TextContent("Codec|Codec for the resulting clip. Automatic will make the best choice for the target platform.");
      public GUIContent bitrateContent = EditorGUIUtility.TextContent("Bitrate Mode|Higher bit rates give a better quality, but impose higher load on network connections or storage.");
      public GUIContent spatialQualityContent = EditorGUIUtility.TextContent("Spatial Quality|Adds a downsize during import to reduce bitrate using resolution.");
      public GUIContent importerVersionContent = EditorGUIUtility.TextContent("Importer Version|Selects the type of video asset produced.");
      public GUIContent[] importerVersionOptions = new GUIContent[2]{ EditorGUIUtility.TextContent("VideoClip|Produce VideoClip asset (for use with VideoPlayer)"), EditorGUIUtility.TextContent("MovieTexture (Deprecated)|Produce MovieTexture asset (deprecated in factor of VideoClip)") };
      public GUIContent transcodeWarning = EditorGUIUtility.TextContent("Not all platforms transcoded. Clip is not guaranteed to be compatible on platforms without transcoding.");
      public GUIContent transcodeSkippedWarning = EditorGUIUtility.TextContent("Transcode was skipped. Current clip does not match import settings. Reimport to resolve.");
      public GUIContent multipleTranscodeSkippedWarning = EditorGUIUtility.TextContent("Transcode was skipped for some clips and they don't match import settings. Reimport to resolve.");
    }
  }
}
