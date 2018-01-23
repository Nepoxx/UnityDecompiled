// Decompiled with JetBrains decompiler
// Type: UnityEditor.VideoPlayerEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (VideoPlayer))]
  internal class VideoPlayerEditor : Editor
  {
    private readonly AnimBool m_ShowRenderTexture = new AnimBool();
    private readonly AnimBool m_ShowTargetCamera = new AnimBool();
    private readonly AnimBool m_ShowRenderer = new AnimBool();
    private readonly AnimBool m_ShowMaterialProperty = new AnimBool();
    private readonly AnimBool m_DataSourceIsClip = new AnimBool();
    private readonly AnimBool m_ShowAspectRatio = new AnimBool();
    private readonly AnimBool m_ShowAudioControls = new AnimBool();
    private ushort m_AudioTrackCountCached = 0;
    private string m_MultiMaterialInfo = (string) null;
    private static VideoPlayerEditor.Styles s_Styles;
    private SerializedProperty m_DataSource;
    private SerializedProperty m_VideoClip;
    private SerializedProperty m_Url;
    private SerializedProperty m_PlayOnAwake;
    private SerializedProperty m_WaitForFirstFrame;
    private SerializedProperty m_Looping;
    private SerializedProperty m_PlaybackSpeed;
    private SerializedProperty m_RenderMode;
    private SerializedProperty m_TargetTexture;
    private SerializedProperty m_TargetCamera;
    private SerializedProperty m_TargetMaterialRenderer;
    private SerializedProperty m_TargetMaterialProperty;
    private SerializedProperty m_AspectRatio;
    private SerializedProperty m_TargetCameraAlpha;
    private SerializedProperty m_TargetCamera3DLayout;
    private SerializedProperty m_AudioOutputMode;
    private SerializedProperty m_ControlledAudioTrackCount;
    private SerializedProperty m_EnabledAudioTracks;
    private SerializedProperty m_TargetAudioSources;
    private SerializedProperty m_DirectAudioVolumes;
    private SerializedProperty m_DirectAudioMutes;
    private GUIContent m_ControlledAudioTrackCountContent;
    private List<VideoPlayerEditor.AudioTrackInfo> m_AudioTrackInfos;
    private int m_MaterialPropertyPopupContentHash;
    private GUIContent[] m_MaterialPropertyPopupContent;
    private int m_MaterialPropertyPopupSelection;
    private int m_MaterialPropertyPopupInvalidSelections;

    private void OnEnable()
    {
      this.m_ShowRenderTexture.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowTargetCamera.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowRenderer.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowMaterialProperty.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_DataSourceIsClip.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowAspectRatio.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowAudioControls.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_DataSource = this.serializedObject.FindProperty("m_DataSource");
      this.m_VideoClip = this.serializedObject.FindProperty("m_VideoClip");
      this.m_Url = this.serializedObject.FindProperty("m_Url");
      this.m_PlayOnAwake = this.serializedObject.FindProperty("m_PlayOnAwake");
      this.m_WaitForFirstFrame = this.serializedObject.FindProperty("m_WaitForFirstFrame");
      this.m_Looping = this.serializedObject.FindProperty("m_Looping");
      this.m_PlaybackSpeed = this.serializedObject.FindProperty("m_PlaybackSpeed");
      this.m_RenderMode = this.serializedObject.FindProperty("m_RenderMode");
      this.m_TargetTexture = this.serializedObject.FindProperty("m_TargetTexture");
      this.m_TargetCamera = this.serializedObject.FindProperty("m_TargetCamera");
      this.m_TargetMaterialRenderer = this.serializedObject.FindProperty("m_TargetMaterialRenderer");
      this.m_TargetMaterialProperty = this.serializedObject.FindProperty("m_TargetMaterialProperty");
      this.m_AspectRatio = this.serializedObject.FindProperty("m_AspectRatio");
      this.m_TargetCameraAlpha = this.serializedObject.FindProperty("m_TargetCameraAlpha");
      this.m_TargetCamera3DLayout = this.serializedObject.FindProperty("m_TargetCamera3DLayout");
      this.m_AudioOutputMode = this.serializedObject.FindProperty("m_AudioOutputMode");
      this.m_ControlledAudioTrackCount = this.serializedObject.FindProperty("m_ControlledAudioTrackCount");
      this.m_EnabledAudioTracks = this.serializedObject.FindProperty("m_EnabledAudioTracks");
      this.m_TargetAudioSources = this.serializedObject.FindProperty("m_TargetAudioSources");
      this.m_DirectAudioVolumes = this.serializedObject.FindProperty("m_DirectAudioVolumes");
      this.m_DirectAudioMutes = this.serializedObject.FindProperty("m_DirectAudioMutes");
      this.m_ShowRenderTexture.value = this.m_RenderMode.intValue == 2;
      this.m_ShowTargetCamera.value = this.m_RenderMode.intValue == 0 || this.m_RenderMode.intValue == 1;
      this.m_ShowRenderer.value = this.m_RenderMode.intValue == 3;
      UnityEngine.Object[] targets = this.targets;
      // ISSUE: reference to a compiler-generated field
      if (VideoPlayerEditor.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        VideoPlayerEditor.\u003C\u003Ef__mg\u0024cache0 = new VideoPlayerEditor.EntryGenerator(VideoPlayerEditor.GetMaterialPropertyNames);
      }
      // ISSUE: reference to a compiler-generated field
      VideoPlayerEditor.EntryGenerator fMgCache0 = VideoPlayerEditor.\u003C\u003Ef__mg\u0024cache0;
      this.m_MaterialPropertyPopupContent = VideoPlayerEditor.BuildPopupEntries(targets, fMgCache0, out this.m_MaterialPropertyPopupSelection, out this.m_MaterialPropertyPopupInvalidSelections);
      this.m_MaterialPropertyPopupContentHash = VideoPlayerEditor.GetMaterialPropertyPopupHash(this.targets);
      this.m_ShowMaterialProperty.value = ((IEnumerable<UnityEngine.Object>) this.targets).Count<UnityEngine.Object>() > 1 || this.m_MaterialPropertyPopupSelection >= 0 && this.m_MaterialPropertyPopupContent.Length > 0;
      this.m_DataSourceIsClip.value = this.m_DataSource.intValue == 0;
      this.m_ShowAspectRatio.value = this.m_RenderMode.intValue != 3 && this.m_RenderMode.intValue != 4;
      this.m_ShowAudioControls.value = this.m_AudioOutputMode.intValue != 0;
      (this.target as VideoPlayer).prepareCompleted += new VideoPlayer.EventHandler(this.PrepareCompleted);
      this.m_AudioTrackInfos = new List<VideoPlayerEditor.AudioTrackInfo>();
    }

    private void OnDisable()
    {
      this.m_ShowRenderTexture.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowTargetCamera.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowRenderer.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowMaterialProperty.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_DataSourceIsClip.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowAspectRatio.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowAudioControls.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      if (VideoPlayerEditor.s_Styles == null)
        VideoPlayerEditor.s_Styles = new VideoPlayerEditor.Styles();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_DataSource, VideoPlayerEditor.s_Styles.dataSourceContent, new GUILayoutOption[0]);
      this.HandleDataSourceField();
      EditorGUILayout.PropertyField(this.m_PlayOnAwake, VideoPlayerEditor.s_Styles.playOnAwakeContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_WaitForFirstFrame, VideoPlayerEditor.s_Styles.waitForFirstFrameContent, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Looping, VideoPlayerEditor.s_Styles.loopContent, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_PlaybackSpeed, 0.0f, 10f, VideoPlayerEditor.s_Styles.playbackSpeedContent, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_RenderMode, VideoPlayerEditor.s_Styles.renderModeContent, new GUILayoutOption[0]);
      if (this.m_RenderMode.hasMultipleDifferentValues)
        EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.selectUniformVideoRenderModeHelp, MessageType.Warning, false);
      else
        this.HandleTargetField((VideoRenderMode) this.m_RenderMode.intValue);
      this.HandleAudio();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void HandleDataSourceField()
    {
      this.m_DataSourceIsClip.target = this.m_DataSource.intValue == 0;
      if (this.m_DataSource.hasMultipleDifferentValues)
        EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.selectUniformVideoSourceHelp, MessageType.Warning, false);
      else if (EditorGUILayout.BeginFadeGroup(this.m_DataSourceIsClip.faded))
      {
        EditorGUILayout.PropertyField(this.m_VideoClip, VideoPlayerEditor.s_Styles.videoClipContent, new GUILayoutOption[0]);
      }
      else
      {
        EditorGUILayout.PropertyField(this.m_Url, VideoPlayerEditor.s_Styles.urlContent, new GUILayoutOption[0]);
        Rect controlRect = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
        controlRect.xMin += EditorGUIUtility.labelWidth;
        controlRect.xMax = (float) ((double) controlRect.xMin + (double) GUI.skin.label.CalcSize(VideoPlayerEditor.s_Styles.browseContent).x + 10.0);
        if (EditorGUI.DropdownButton(controlRect, VideoPlayerEditor.s_Styles.browseContent, FocusType.Passive, GUISkin.current.button))
        {
          string[] filters = new string[4]{ "Movie files", "dv,mp4,mpg,mpeg,m4v,ogv,vp8,webm", "All files", "*" };
          string str = EditorUtility.OpenFilePanelWithFilters(VideoPlayerEditor.s_Styles.selectMovieFile, "", filters);
          if (!string.IsNullOrEmpty(str))
            this.m_Url.stringValue = "file://" + str;
        }
      }
      EditorGUILayout.EndFadeGroup();
    }

    private static int GetMaterialPropertyPopupHash(UnityEngine.Object[] objects)
    {
      int num = 0;
      foreach (VideoPlayer vp in objects)
      {
        if ((bool) ((UnityEngine.Object) vp))
        {
          Renderer targetRenderer = VideoPlayerEditor.GetTargetRenderer(vp);
          if ((bool) ((UnityEngine.Object) targetRenderer))
          {
            num ^= vp.targetMaterialProperty.GetHashCode();
            foreach (Material sharedMaterial in targetRenderer.sharedMaterials)
            {
              if ((bool) ((UnityEngine.Object) sharedMaterial))
              {
                num ^= sharedMaterial.name.GetHashCode();
                int propertyIdx = 0;
                for (int propertyCount = ShaderUtil.GetPropertyCount(sharedMaterial.shader); propertyIdx < propertyCount; ++propertyIdx)
                {
                  if (ShaderUtil.GetPropertyType(sharedMaterial.shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
                    num ^= ShaderUtil.GetPropertyName(sharedMaterial.shader, propertyIdx).GetHashCode();
                }
              }
            }
          }
        }
      }
      return num;
    }

    private static List<string> GetMaterialPropertyNames(UnityEngine.Object obj, bool multiSelect, out int selection, out bool invalidSelection)
    {
      selection = -1;
      invalidSelection = true;
      List<string> source = new List<string>();
      VideoPlayer vp = obj as VideoPlayer;
      if (!(bool) ((UnityEngine.Object) vp))
        return source;
      Renderer targetRenderer = VideoPlayerEditor.GetTargetRenderer(vp);
      if (!(bool) ((UnityEngine.Object) targetRenderer))
        return source;
      foreach (Material sharedMaterial in targetRenderer.sharedMaterials)
      {
        if ((bool) ((UnityEngine.Object) sharedMaterial))
        {
          int propertyIdx = 0;
          for (int propertyCount = ShaderUtil.GetPropertyCount(sharedMaterial.shader); propertyIdx < propertyCount; ++propertyIdx)
          {
            if (ShaderUtil.GetPropertyType(sharedMaterial.shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
              string propertyName = ShaderUtil.GetPropertyName(sharedMaterial.shader, propertyIdx);
              if (!source.Contains(propertyName))
                source.Add(propertyName);
            }
          }
          selection = source.IndexOf(vp.targetMaterialProperty);
          invalidSelection = selection < 0 && source.Count<string>() > 0;
          if (invalidSelection && !multiSelect)
          {
            selection = source.Count<string>();
            source.Add(vp.targetMaterialProperty);
          }
        }
      }
      return source;
    }

    private static GUIContent[] BuildPopupEntries(UnityEngine.Object[] objects, VideoPlayerEditor.EntryGenerator func, out int selection, out int invalidSelections)
    {
      selection = -1;
      invalidSelections = 0;
      List<string> stringList1 = (List<string>) null;
      foreach (UnityEngine.Object @object in objects)
      {
        int selection1;
        bool invalidSelection;
        List<string> stringList2 = func(@object, ((IEnumerable<UnityEngine.Object>) objects).Count<UnityEngine.Object>() > 1, out selection1, out invalidSelection);
        if (stringList2 != null)
        {
          if (invalidSelection)
            ++invalidSelections;
          List<string> stringList3 = stringList1 != null ? new List<string>(stringList1.Intersect<string>((IEnumerable<string>) stringList2)) : stringList2;
          selection = stringList1 != null ? (selection < 0 || selection1 < 0 || stringList1[selection] != stringList2[selection1] ? -1 : stringList3.IndexOf(stringList1[selection])) : selection1;
          stringList1 = stringList3;
        }
      }
      if (stringList1 == null)
        stringList1 = new List<string>();
      return stringList1.Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
    }

    private static void HandlePopup(GUIContent content, SerializedProperty property, GUIContent[] entries, int selection)
    {
      Rect controlRect = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
      GUIContent label = EditorGUI.BeginProperty(controlRect, content, property);
      EditorGUI.BeginChangeCheck();
      EditorGUI.BeginDisabledGroup(((IEnumerable<GUIContent>) entries).Count<GUIContent>() == 0);
      selection = EditorGUI.Popup(controlRect, label, selection, entries);
      EditorGUI.EndDisabledGroup();
      if (EditorGUI.EndChangeCheck())
        property.stringValue = entries[selection].text;
      EditorGUI.EndProperty();
    }

    private void HandleTargetField(VideoRenderMode currentRenderMode)
    {
      this.m_ShowRenderTexture.target = currentRenderMode == VideoRenderMode.RenderTexture;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowRenderTexture.faded))
        EditorGUILayout.PropertyField(this.m_TargetTexture, VideoPlayerEditor.s_Styles.textureContent, new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
      this.m_ShowTargetCamera.target = currentRenderMode == VideoRenderMode.CameraFarPlane || currentRenderMode == VideoRenderMode.CameraNearPlane;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowTargetCamera.faded))
      {
        EditorGUILayout.PropertyField(this.m_TargetCamera, VideoPlayerEditor.s_Styles.cameraContent, new GUILayoutOption[0]);
        EditorGUILayout.Slider(this.m_TargetCameraAlpha, 0.0f, 1f, VideoPlayerEditor.s_Styles.alphaContent, new GUILayoutOption[0]);
        foreach (BuildPlatform buildPlatform in BuildPlatforms.instance.buildPlatforms)
        {
          if (VREditor.GetVREnabledOnTargetGroup(buildPlatform.targetGroup))
          {
            EditorGUILayout.PropertyField(this.m_TargetCamera3DLayout, VideoPlayerEditor.s_Styles.camera3DLayout, new GUILayoutOption[0]);
            break;
          }
        }
      }
      EditorGUILayout.EndFadeGroup();
      this.m_ShowRenderer.target = currentRenderMode == VideoRenderMode.MaterialOverride;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowRenderer.faded))
      {
        bool flag = ((IEnumerable<UnityEngine.Object>) this.targets).Count<UnityEngine.Object>() > 1;
        if (flag)
        {
          EditorGUILayout.PropertyField(this.m_TargetMaterialRenderer, VideoPlayerEditor.s_Styles.materialRendererContent, new GUILayoutOption[0]);
        }
        else
        {
          Rect controlRect = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
          GUIContent label = EditorGUI.BeginProperty(controlRect, VideoPlayerEditor.s_Styles.materialRendererContent, this.m_TargetMaterialRenderer);
          EditorGUI.BeginChangeCheck();
          UnityEngine.Object @object = EditorGUI.ObjectField(controlRect, label, (UnityEngine.Object) VideoPlayerEditor.GetTargetRenderer((VideoPlayer) this.target), typeof (Renderer), true);
          if (EditorGUI.EndChangeCheck())
            this.m_TargetMaterialRenderer.objectReferenceValue = @object;
          EditorGUI.EndProperty();
        }
        int propertyPopupHash = VideoPlayerEditor.GetMaterialPropertyPopupHash(this.targets);
        if (this.m_MaterialPropertyPopupContentHash != propertyPopupHash)
        {
          UnityEngine.Object[] targets = this.targets;
          // ISSUE: reference to a compiler-generated field
          if (VideoPlayerEditor.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            VideoPlayerEditor.\u003C\u003Ef__mg\u0024cache1 = new VideoPlayerEditor.EntryGenerator(VideoPlayerEditor.GetMaterialPropertyNames);
          }
          // ISSUE: reference to a compiler-generated field
          VideoPlayerEditor.EntryGenerator fMgCache1 = VideoPlayerEditor.\u003C\u003Ef__mg\u0024cache1;
          this.m_MaterialPropertyPopupContent = VideoPlayerEditor.BuildPopupEntries(targets, fMgCache1, out this.m_MaterialPropertyPopupSelection, out this.m_MaterialPropertyPopupInvalidSelections);
        }
        VideoPlayerEditor.HandlePopup(VideoPlayerEditor.s_Styles.materialPropertyContent, this.m_TargetMaterialProperty, this.m_MaterialPropertyPopupContent, this.m_MaterialPropertyPopupSelection);
        if (this.m_MaterialPropertyPopupInvalidSelections > 0 || this.m_MaterialPropertyPopupContent.Length == 0)
        {
          GUILayout.BeginHorizontal();
          GUILayout.Space(EditorGUIUtility.labelWidth);
          if (this.m_MaterialPropertyPopupContent.Length == 0)
          {
            if (!flag)
              EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.rendererMaterialsHaveNoTexPropsHelp, MessageType.Warning);
            else
              EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.someRendererMaterialsHaveNoTexPropsHelp, MessageType.Warning);
          }
          else if (!flag)
            EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.invalidTexPropSelectionHelp, MessageType.Warning);
          else if (this.m_MaterialPropertyPopupInvalidSelections == 1)
            EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.oneInvalidTexPropSelectionHelp, MessageType.Warning);
          else
            EditorGUILayout.HelpBox(string.Format(VideoPlayerEditor.s_Styles.someInvalidTexPropSelectionsHelp, (object) this.m_MaterialPropertyPopupInvalidSelections), MessageType.Warning);
          GUILayout.EndHorizontal();
        }
        else
          this.DisplayMultiMaterialInformation(this.m_MaterialPropertyPopupContentHash != propertyPopupHash);
        this.m_MaterialPropertyPopupContentHash = propertyPopupHash;
      }
      EditorGUILayout.EndFadeGroup();
      this.m_ShowAspectRatio.target = currentRenderMode != VideoRenderMode.MaterialOverride && currentRenderMode != VideoRenderMode.APIOnly;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAspectRatio.faded))
        EditorGUILayout.PropertyField(this.m_AspectRatio, VideoPlayerEditor.s_Styles.aspectRatioLabel, new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
    }

    private void DisplayMultiMaterialInformation(bool refreshInfo)
    {
      if (refreshInfo || this.m_MultiMaterialInfo == null)
        this.m_MultiMaterialInfo = this.GenerateMultiMaterialinformation();
      if (string.IsNullOrEmpty(this.m_MultiMaterialInfo))
        return;
      GUILayout.BeginHorizontal();
      GUILayout.Space(EditorGUIUtility.labelWidth);
      EditorGUILayout.HelpBox(this.m_MultiMaterialInfo, MessageType.Info);
      GUILayout.EndHorizontal();
    }

    private string GenerateMultiMaterialinformation()
    {
      if (((IEnumerable<UnityEngine.Object>) this.targets).Count<UnityEngine.Object>() > 1)
        return "";
      VideoPlayer target = this.target as VideoPlayer;
      if (!(bool) ((UnityEngine.Object) target))
        return "";
      Renderer targetRenderer = VideoPlayerEditor.GetTargetRenderer(target);
      if (!(bool) ((UnityEngine.Object) targetRenderer))
        return "";
      Material[] sharedMaterials = targetRenderer.sharedMaterials;
      if (sharedMaterials == null || ((IEnumerable<Material>) sharedMaterials).Count<Material>() <= 1)
        return "";
      List<string> source = new List<string>();
      foreach (Material material in sharedMaterials)
      {
        if ((bool) ((UnityEngine.Object) material))
        {
          int propertyIdx = 0;
          for (int propertyCount = ShaderUtil.GetPropertyCount(material.shader); propertyIdx < propertyCount; ++propertyIdx)
          {
            if (ShaderUtil.GetPropertyType(material.shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv && ShaderUtil.GetPropertyName(material.shader, propertyIdx) == this.m_TargetMaterialProperty.stringValue)
            {
              source.Add(material.name);
              break;
            }
          }
        }
      }
      if (source.Count<string>() == ((IEnumerable<Material>) sharedMaterials).Count<Material>())
        return VideoPlayerEditor.s_Styles.texPropInAllMaterialsHelp;
      return string.Format(VideoPlayerEditor.s_Styles.texPropInSomeMaterialsHelp, (object) source.Count<string>(), (object) ((IEnumerable<Material>) sharedMaterials).Count<Material>()) + ": " + string.Join(", ", source.ToArray());
    }

    private void HandleAudio()
    {
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_AudioOutputMode, VideoPlayerEditor.s_Styles.audioOutputModeContent, new GUILayoutOption[0]);
      this.m_ShowAudioControls.target = this.m_AudioOutputMode.intValue != 0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAudioControls.faded))
      {
        if (this.serializedObject.isEditingMultipleObjects)
          EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.audioControlsNotEditableHelp, MessageType.Warning, false);
        else if (this.m_AudioOutputMode.hasMultipleDifferentValues)
        {
          EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.selectUniformAudioOutputModeHelp, MessageType.Warning, false);
        }
        else
        {
          ushort intValue1 = (ushort) this.m_ControlledAudioTrackCount.intValue;
          this.HandleControlledAudioTrackCount();
          if (this.m_ControlledAudioTrackCount.hasMultipleDifferentValues)
          {
            EditorGUILayout.HelpBox(VideoPlayerEditor.s_Styles.selectUniformAudioTracksHelp, MessageType.Warning, false);
          }
          else
          {
            VideoAudioOutputMode intValue2 = (VideoAudioOutputMode) this.m_AudioOutputMode.intValue;
            ushort num = (ushort) Math.Min((int) Math.Min((ushort) this.m_ControlledAudioTrackCount.intValue, intValue1), this.m_EnabledAudioTracks.arraySize);
            for (ushort trackIdx = 0; (int) trackIdx < (int) num; ++trackIdx)
            {
              EditorGUILayout.PropertyField(this.m_EnabledAudioTracks.GetArrayElementAtIndex((int) trackIdx), this.GetAudioTrackEnabledContent(trackIdx), new GUILayoutOption[0]);
              ++EditorGUI.indentLevel;
              switch (intValue2)
              {
                case VideoAudioOutputMode.AudioSource:
                  EditorGUILayout.PropertyField(this.m_TargetAudioSources.GetArrayElementAtIndex((int) trackIdx), VideoPlayerEditor.s_Styles.audioSourceContent, new GUILayoutOption[0]);
                  break;
                case VideoAudioOutputMode.Direct:
                  EditorGUILayout.PropertyField(this.m_DirectAudioMutes.GetArrayElementAtIndex((int) trackIdx), VideoPlayerEditor.s_Styles.muteLabel, new GUILayoutOption[0]);
                  EditorGUILayout.Slider(this.m_DirectAudioVolumes.GetArrayElementAtIndex((int) trackIdx), 0.0f, 1f, VideoPlayerEditor.s_Styles.volumeLabel, new GUILayoutOption[0]);
                  break;
              }
              --EditorGUI.indentLevel;
            }
          }
        }
      }
      EditorGUILayout.EndFadeGroup();
    }

    private GUIContent GetAudioTrackEnabledContent(ushort trackIdx)
    {
      while (this.m_AudioTrackInfos.Count <= (int) trackIdx)
        this.m_AudioTrackInfos.Add(new VideoPlayerEditor.AudioTrackInfo());
      VideoPlayerEditor.AudioTrackInfo audioTrackInfo = this.m_AudioTrackInfos[(int) trackIdx];
      VideoPlayer videoPlayer = (VideoPlayer) null;
      if (!this.serializedObject.isEditingMultipleObjects)
        videoPlayer = (VideoPlayer) this.target;
      string str1 = !(bool) ((UnityEngine.Object) videoPlayer) ? "" : videoPlayer.GetAudioLanguageCode(trackIdx);
      ushort num = !(bool) ((UnityEngine.Object) videoPlayer) ? (ushort) 0 : videoPlayer.GetAudioChannelCount(trackIdx);
      if (str1 != audioTrackInfo.language || (int) num != (int) audioTrackInfo.channelCount || audioTrackInfo.content == null)
      {
        string str2 = "";
        if (str1.Length > 0)
          str2 += str1;
        if ((int) num > 0)
        {
          if (str2.Length > 0)
            str2 += ", ";
          str2 = str2 + num.ToString() + " ch";
        }
        if (str2.Length > 0)
          str2 = " [" + str2 + "]";
        audioTrackInfo.content = EditorGUIUtility.TextContent("Track " + (object) trackIdx + str2);
        audioTrackInfo.content.tooltip = VideoPlayerEditor.s_Styles.enableDecodingTooltip;
      }
      return audioTrackInfo.content;
    }

    private void HandleControlledAudioTrackCount()
    {
      if (this.m_DataSourceIsClip.value || this.m_DataSource.hasMultipleDifferentValues)
        return;
      ushort num = !this.serializedObject.isEditingMultipleObjects ? ((VideoPlayer) this.target).audioTrackCount : (ushort) 0;
      GUIContent trackCountContent;
      if ((int) num == 0)
      {
        trackCountContent = VideoPlayerEditor.s_Styles.controlledAudioTrackCountContent;
      }
      else
      {
        if ((int) num != (int) this.m_AudioTrackCountCached)
        {
          this.m_AudioTrackCountCached = num;
          this.m_ControlledAudioTrackCountContent = EditorGUIUtility.TextContent(VideoPlayerEditor.s_Styles.controlledAudioTrackCountContent.text + " [" + (object) num + " found]");
          this.m_ControlledAudioTrackCountContent.tooltip = VideoPlayerEditor.s_Styles.controlledAudioTrackCountContent.tooltip;
        }
        trackCountContent = this.m_ControlledAudioTrackCountContent;
      }
      EditorGUILayout.PropertyField(this.m_ControlledAudioTrackCount, trackCountContent, new GUILayoutOption[0]);
    }

    private void PrepareCompleted(VideoPlayer vp)
    {
      this.Repaint();
    }

    private static Renderer GetTargetRenderer(VideoPlayer vp)
    {
      Renderer materialRenderer = vp.targetMaterialRenderer;
      if ((bool) ((UnityEngine.Object) materialRenderer))
        return materialRenderer;
      return vp.gameObject.GetComponent<Renderer>();
    }

    private class Styles
    {
      public GUIContent dataSourceContent = EditorGUIUtility.TextContent("Source|Type of source the movie will be read from.");
      public GUIContent videoClipContent = EditorGUIUtility.TextContent("Video Clip|VideoClips can be imported using the asset pipeline.");
      public GUIContent urlContent = EditorGUIUtility.TextContent("URL|URLs can be http:// or file://. File URLs can be relative [file://] or absolute [file:///].  For file URLs, the prefix is optional.");
      public GUIContent browseContent = EditorGUIUtility.TextContent("Browse...|Click to set a file:// URL.  http:// URLs have to be written or copy-pasted manually.");
      public GUIContent playOnAwakeContent = EditorGUIUtility.TextContent("Play On Awake|Start playback as soon as the game is started.");
      public GUIContent waitForFirstFrameContent = EditorGUIUtility.TextContent("Wait For First Frame|Wait for first frame to be ready before starting playback. When on, player time will only start increasing when the first image is ready.  When off, the first few frames may be skipped while clip preparation is ongoing.");
      public GUIContent loopContent = EditorGUIUtility.TextContent("Loop|Start playback at the beginning when end is reached.");
      public GUIContent playbackSpeedContent = EditorGUIUtility.TextContent("Playback Speed|Increase or decrease the playback speed. 1.0 is the normal speed.");
      public GUIContent renderModeContent = EditorGUIUtility.TextContent("Render Mode|Type of object on which the played images will be drawn.");
      public GUIContent cameraContent = EditorGUIUtility.TextContent("Camera|Camera where the images will be drawn, behind (Back Plane) or in front of (Front Plane) of the scene.");
      public GUIContent textureContent = EditorGUIUtility.TextContent("Target Texture|RenderTexture where the images will be drawn.  RenderTextures can be created under the Assets folder and the used on other objects.");
      public GUIContent alphaContent = EditorGUIUtility.TextContent("Alpha|A value less than 1.0 will reveal the content behind the video.");
      public GUIContent camera3DLayout = EditorGUIUtility.TextContent("3D Layout|Layout of 3D content in the source video.");
      public GUIContent audioOutputModeContent = EditorGUIUtility.TextContent("Audio Output Mode|Where the audio in the movie will be output.");
      public GUIContent audioSourceContent = EditorGUIUtility.TextContent("Audio Source|AudioSource component that will receive this track's audio samples.");
      public GUIContent aspectRatioLabel = EditorGUIUtility.TextContent("Aspect Ratio");
      public GUIContent muteLabel = EditorGUIUtility.TextContent("Mute");
      public GUIContent volumeLabel = EditorGUIUtility.TextContent("Volume");
      public GUIContent controlledAudioTrackCountContent = EditorGUIUtility.TextContent("Controlled Tracks|How many audio tracks will the player control.  The actual number of tracks is only known during playback when the source is a URL.");
      public GUIContent materialRendererContent = EditorGUIUtility.TextContent("Renderer|Renderer that will receive the images. Defaults to the first renderer on the game object.");
      public GUIContent materialPropertyContent = EditorGUIUtility.TextContent("Material Property|Texture property of the current Material that will receive the images.");
      public string selectUniformVideoSourceHelp = "Select a uniform video source type before a video clip or URL can be selected.";
      public string rendererMaterialsHaveNoTexPropsHelp = "Renderer materials have no texture properties.";
      public string someRendererMaterialsHaveNoTexPropsHelp = "Some selected renderers have materials with no texture properties.";
      public string invalidTexPropSelectionHelp = "Invalid texture property selection.";
      public string oneInvalidTexPropSelectionHelp = "1 selected object has an invalid texture property selection.";
      public string someInvalidTexPropSelectionsHelp = "{0} selected objects have invalid texture property selections.";
      public string texPropInAllMaterialsHelp = "Texture property appears in all renderer materials.";
      public string texPropInSomeMaterialsHelp = "Texture property appears in {0} out of {1} renderer materials.";
      public string selectUniformVideoRenderModeHelp = "Select a uniform video render mode type before a target camera, render texture or material parameter can be selected.";
      public string selectUniformAudioOutputModeHelp = "Select a uniform audio target before audio settings can be edited.";
      public string selectUniformAudioTracksHelp = "Only sources with the same number of audio tracks can be edited during multi-selection.";
      public string selectMovieFile = "Select movie file.";
      public string audioControlsNotEditableHelp = "Audio controls not editable when using muliple selection.";
      public string enableDecodingTooltip = "Enable decoding for this track.  Only effective when not playing.  When playing from a URL, track details are shown only while playing back.";
    }

    internal class AudioTrackInfo
    {
      public string language;
      public ushort channelCount;
      public GUIContent content;

      public AudioTrackInfo()
      {
        this.language = "";
        this.channelCount = (ushort) 0;
      }
    }

    private delegate List<string> EntryGenerator(UnityEngine.Object obj, bool multiSelect, out int selection, out bool invalidSelection);
  }
}
