// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioManagerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioManager))]
  internal class AudioManagerInspector : ProjectSettingsBaseEditor
  {
    private SerializedProperty m_Volume;
    private SerializedProperty m_RolloffScale;
    private SerializedProperty m_DopplerFactor;
    private SerializedProperty m_DefaultSpeakerMode;
    private SerializedProperty m_SampleRate;
    private SerializedProperty m_DSPBufferSize;
    private SerializedProperty m_VirtualVoiceCount;
    private SerializedProperty m_RealVoiceCount;
    private SerializedProperty m_SpatializerPlugin;
    private SerializedProperty m_AmbisonicDecoderPlugin;
    private SerializedProperty m_DisableAudio;
    private SerializedProperty m_VirtualizeEffects;

    private void OnEnable()
    {
      this.m_Volume = this.serializedObject.FindProperty("m_Volume");
      this.m_RolloffScale = this.serializedObject.FindProperty("Rolloff Scale");
      this.m_DopplerFactor = this.serializedObject.FindProperty("Doppler Factor");
      this.m_DefaultSpeakerMode = this.serializedObject.FindProperty("Default Speaker Mode");
      this.m_SampleRate = this.serializedObject.FindProperty("m_SampleRate");
      this.m_DSPBufferSize = this.serializedObject.FindProperty("m_DSPBufferSize");
      this.m_VirtualVoiceCount = this.serializedObject.FindProperty("m_VirtualVoiceCount");
      this.m_RealVoiceCount = this.serializedObject.FindProperty("m_RealVoiceCount");
      this.m_SpatializerPlugin = this.serializedObject.FindProperty("m_SpatializerPlugin");
      this.m_AmbisonicDecoderPlugin = this.serializedObject.FindProperty("m_AmbisonicDecoderPlugin");
      this.m_DisableAudio = this.serializedObject.FindProperty("m_DisableAudio");
      this.m_VirtualizeEffects = this.serializedObject.FindProperty("m_VirtualizeEffects");
    }

    private int FindPluginStringIndex(string[] strs, string element)
    {
      for (int index = 1; index < strs.Length; ++index)
      {
        if (element == strs[index])
          return index;
      }
      return 0;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Volume, AudioManagerInspector.Styles.Volume, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RolloffScale, AudioManagerInspector.Styles.RolloffScale, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DopplerFactor, AudioManagerInspector.Styles.DopplerFactor, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DefaultSpeakerMode, AudioManagerInspector.Styles.DefaultSpeakerMode, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SampleRate, AudioManagerInspector.Styles.SampleRate, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DSPBufferSize, AudioManagerInspector.Styles.DSPBufferSize, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VirtualVoiceCount, AudioManagerInspector.Styles.VirtualVoiceCount, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RealVoiceCount, AudioManagerInspector.Styles.RealVoiceCount, new GUILayoutOption[0]);
      List<string> stringList1 = new List<string>((IEnumerable<string>) AudioSettings.GetSpatializerPluginNames());
      stringList1.Insert(0, "None");
      string[] array1 = stringList1.ToArray();
      List<GUIContent> guiContentList1 = new List<GUIContent>();
      foreach (string text in array1)
        guiContentList1.Add(new GUIContent(text));
      List<string> stringList2 = new List<string>((IEnumerable<string>) AudioUtil.GetAmbisonicDecoderPluginNames());
      stringList2.Insert(0, "None");
      string[] array2 = stringList2.ToArray();
      List<GUIContent> guiContentList2 = new List<GUIContent>();
      foreach (string text in array2)
        guiContentList2.Add(new GUIContent(text));
      EditorGUI.BeginChangeCheck();
      int pluginStringIndex1 = this.FindPluginStringIndex(array1, this.m_SpatializerPlugin.stringValue);
      int index1 = EditorGUILayout.Popup(AudioManagerInspector.Styles.SpatializerPlugin, pluginStringIndex1, guiContentList1.ToArray(), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_SpatializerPlugin.stringValue = index1 != 0 ? array1[index1] : "";
      EditorGUI.BeginChangeCheck();
      int pluginStringIndex2 = this.FindPluginStringIndex(array2, this.m_AmbisonicDecoderPlugin.stringValue);
      int index2 = EditorGUILayout.Popup(AudioManagerInspector.Styles.AmbisonicDecoderPlugin, pluginStringIndex2, guiContentList2.ToArray(), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_AmbisonicDecoderPlugin.stringValue = index2 != 0 ? array2[index2] : "";
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_DisableAudio, AudioManagerInspector.Styles.DisableAudio, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VirtualizeEffects, AudioManagerInspector.Styles.VirtualizeEffects, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public static GUIContent Volume = EditorGUIUtility.TextContent("Global Volume|Initial volume multiplier (AudioListener.volume)");
      public static GUIContent RolloffScale = EditorGUIUtility.TextContent("Volume Rolloff Scale|Global volume rolloff multiplier (applies only to logarithmic volume curves).");
      public static GUIContent DopplerFactor = EditorGUIUtility.TextContent("Doppler Factor|Global Doppler speed multiplier for sounds in motion.");
      public static GUIContent DefaultSpeakerMode = EditorGUIUtility.TextContent("Default Speaker Mode|Speaker mode at start of the game. This may be changed at runtime using the AudioSettings.Reset function.");
      public static GUIContent SampleRate = EditorGUIUtility.TextContent("System Sample Rate|Sample rate at which the output device of the audio system runs. Individual sounds may run at different sample rates and will be slowed down/sped up accordingly to match the output rate.");
      public static GUIContent DSPBufferSize = EditorGUIUtility.TextContent("DSP Buffer Size|Length of mixing buffer. This determines the output latency of the game.");
      public static GUIContent VirtualVoiceCount = EditorGUIUtility.TextContent("Max Virtual Voices|Maximum number of sounds managed by the system. Even though at most RealVoiceCount of the loudest sounds will be physically playing, the remaining sounds will still be updating their play position.");
      public static GUIContent RealVoiceCount = EditorGUIUtility.TextContent("Max Real Voices|Maximum number of actual simultanously playing sounds.");
      public static GUIContent SpatializerPlugin = EditorGUIUtility.TextContent("Spatializer Plugin|Native audio plugin performing spatialized filtering of 3D sources.");
      public static GUIContent AmbisonicDecoderPlugin = EditorGUIUtility.TextContent("Ambisonic Decoder Plugin|Native audio plugin performing ambisonic-to-binaural filtering of sources.");
      public static GUIContent DisableAudio = EditorGUIUtility.TextContent("Disable Unity Audio|Prevent allocating the output device in the runtime. Use this if you want to use other sound systems than the built-in one.");
      public static GUIContent VirtualizeEffects = EditorGUIUtility.TextContent("Virtualize Effects|When enabled dynamically turn off effects and spatializers on AudioSources that are culled in order to save CPU.");
    }
  }
}
