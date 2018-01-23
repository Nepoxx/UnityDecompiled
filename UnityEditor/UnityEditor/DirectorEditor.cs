// Decompiled with JetBrains decompiler
// Type: UnityEditor.DirectorEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEditor
{
  [CustomEditor(typeof (PlayableDirector))]
  [CanEditMultipleObjects]
  internal class DirectorEditor : Editor
  {
    private List<DirectorEditor.BindingPropertyPair> m_BindingPropertiesCache = new List<DirectorEditor.BindingPropertyPair>();
    private PlayableBinding[] m_SynchedPlayableBindings = (PlayableBinding[]) null;
    private SerializedProperty m_PlayableAsset;
    private SerializedProperty m_InitialState;
    private SerializedProperty m_WrapMode;
    private SerializedProperty m_InitialTime;
    private SerializedProperty m_UpdateMethod;
    private SerializedProperty m_SceneBindings;
    private GUIContent m_AnimatorContent;
    private GUIContent m_AudioContent;
    private GUIContent m_VideoContent;
    private GUIContent m_ScriptContent;
    private Texture m_DefaultScriptContentTexture;

    public void OnEnable()
    {
      this.m_PlayableAsset = this.serializedObject.FindProperty("m_PlayableAsset");
      this.m_InitialState = this.serializedObject.FindProperty("m_InitialState");
      this.m_WrapMode = this.serializedObject.FindProperty("m_WrapMode");
      this.m_UpdateMethod = this.serializedObject.FindProperty("m_DirectorUpdateMode");
      this.m_InitialTime = this.serializedObject.FindProperty("m_InitialTime");
      this.m_SceneBindings = this.serializedObject.FindProperty("m_SceneBindings");
      this.m_AnimatorContent = new GUIContent((Texture) AssetPreview.GetMiniTypeThumbnail(typeof (Animator)));
      this.m_AudioContent = new GUIContent((Texture) AssetPreview.GetMiniTypeThumbnail(typeof (AudioSource)));
      this.m_VideoContent = new GUIContent((Texture) AssetPreview.GetMiniTypeThumbnail(typeof (RenderTexture)));
      this.m_ScriptContent = new GUIContent((Texture) EditorGUIUtility.LoadIcon("ScriptableObject Icon"));
      this.m_DefaultScriptContentTexture = this.m_ScriptContent.image;
    }

    public override void OnInspectorGUI()
    {
      if (this.PlayableAssetOutputsChanged())
        this.SynchSceneBindings();
      this.serializedObject.Update();
      if (DirectorEditor.PropertyFieldAsObject(this.m_PlayableAsset, DirectorEditor.Styles.PlayableText, typeof (PlayableAsset), false, false))
      {
        this.serializedObject.ApplyModifiedProperties();
        this.SynchSceneBindings();
        InternalEditorUtility.RepaintAllViews();
      }
      EditorGUILayout.PropertyField(this.m_UpdateMethod, DirectorEditor.Styles.UpdateMethod, new GUILayoutOption[0]);
      Rect controlRect = EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]);
      GUIContent label = EditorGUI.BeginProperty(controlRect, DirectorEditor.Styles.InitialStateContent, this.m_InitialState);
      bool flag1 = this.m_InitialState.enumValueIndex != 0;
      EditorGUI.BeginChangeCheck();
      bool flag2 = EditorGUI.Toggle(controlRect, label, flag1);
      if (EditorGUI.EndChangeCheck())
        this.m_InitialState.enumValueIndex = !flag2 ? 0 : 1;
      EditorGUI.EndProperty();
      EditorGUILayout.PropertyField(this.m_WrapMode, DirectorEditor.Styles.WrapModeContent, new GUILayoutOption[0]);
      DirectorEditor.PropertyFieldAsFloat(this.m_InitialTime, DirectorEditor.Styles.InitialTimeContent);
      if (Application.isPlaying)
        this.CurrentTimeField();
      if (this.targets.Length == 1 && (UnityEngine.Object) (this.m_PlayableAsset.objectReferenceValue as PlayableAsset) != (UnityEngine.Object) null)
        this.DoDirectorBindingInspector();
      this.serializedObject.ApplyModifiedProperties();
    }

    private bool PlayableAssetOutputsChanged()
    {
      PlayableAsset objectReferenceValue = this.m_PlayableAsset.objectReferenceValue as PlayableAsset;
      if (this.m_SynchedPlayableBindings == null)
        return (UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null;
      if ((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null || objectReferenceValue.outputs.Count<PlayableBinding>() != this.m_SynchedPlayableBindings.Length)
        return true;
      return objectReferenceValue.outputs.Where<PlayableBinding>((Func<PlayableBinding, int, bool>) ((t, i) => t.sourceObject != this.m_SynchedPlayableBindings[i].sourceObject)).Any<PlayableBinding>();
    }

    private void BindingInspector(SerializedProperty bindingProperty, PlayableBinding binding)
    {
      if (binding.sourceObject == (UnityEngine.Object) null)
        return;
      UnityEngine.Object objectReferenceValue = bindingProperty.objectReferenceValue;
      if (binding.streamType == DataStreamType.Audio)
      {
        this.m_AudioContent.text = binding.streamName;
        this.m_AudioContent.tooltip = !(objectReferenceValue == (UnityEngine.Object) null) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text;
        DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_AudioContent, typeof (AudioSource), true, false);
      }
      else if (binding.streamType == DataStreamType.Animation)
      {
        this.m_AnimatorContent.text = binding.streamName;
        this.m_AnimatorContent.tooltip = !(objectReferenceValue is GameObject) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text;
        DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_AnimatorContent, typeof (Animator), true, true);
      }
      if (binding.streamType == DataStreamType.Texture)
      {
        this.m_VideoContent.text = binding.streamName;
        this.m_VideoContent.tooltip = !(objectReferenceValue == (UnityEngine.Object) null) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text;
        DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_VideoContent, typeof (RenderTexture), false, false);
      }
      else
      {
        if (binding.streamType != DataStreamType.None)
          return;
        this.m_ScriptContent.text = binding.streamName;
        this.m_ScriptContent.tooltip = !(objectReferenceValue == (UnityEngine.Object) null) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text;
        this.m_ScriptContent.image = (Texture) AssetPreview.GetMiniTypeThumbnail(binding.sourceBindingType) ?? this.m_DefaultScriptContentTexture;
        if (binding.sourceBindingType != null && typeof (UnityEngine.Object).IsAssignableFrom(binding.sourceBindingType))
          DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_ScriptContent, binding.sourceBindingType, true, false);
      }
    }

    private void DoDirectorBindingInspector()
    {
      if (!this.m_BindingPropertiesCache.Any<DirectorEditor.BindingPropertyPair>())
        return;
      this.m_SceneBindings.isExpanded = EditorGUILayout.Foldout(this.m_SceneBindings.isExpanded, DirectorEditor.Styles.BindingsTitleContent);
      if (!this.m_SceneBindings.isExpanded)
        return;
      ++EditorGUI.indentLevel;
      foreach (DirectorEditor.BindingPropertyPair bindingPropertyPair in this.m_BindingPropertiesCache)
        this.BindingInspector(bindingPropertyPair.property, bindingPropertyPair.binding);
      --EditorGUI.indentLevel;
    }

    private void SynchSceneBindings()
    {
      if (this.targets.Length > 1)
        return;
      PlayableDirector target = (PlayableDirector) this.target;
      PlayableAsset objectReferenceValue = this.m_PlayableAsset.objectReferenceValue as PlayableAsset;
      this.m_BindingPropertiesCache.Clear();
      this.m_SynchedPlayableBindings = (PlayableBinding[]) null;
      if ((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null)
        return;
      this.m_SynchedPlayableBindings = objectReferenceValue.outputs.ToArray<PlayableBinding>();
      foreach (PlayableBinding synchedPlayableBinding in this.m_SynchedPlayableBindings)
      {
        if (!target.HasGenericBinding(synchedPlayableBinding.sourceObject))
          target.SetGenericBinding(synchedPlayableBinding.sourceObject, (UnityEngine.Object) null);
      }
      this.serializedObject.Update();
      SerializedProperty[] serializedPropertyArray = new SerializedProperty[this.m_SceneBindings.arraySize];
      for (int index = 0; index < this.m_SceneBindings.arraySize; ++index)
        serializedPropertyArray[index] = this.m_SceneBindings.GetArrayElementAtIndex(index);
      foreach (PlayableBinding synchedPlayableBinding in this.m_SynchedPlayableBindings)
      {
        foreach (SerializedProperty serializedProperty in serializedPropertyArray)
        {
          if (serializedProperty.FindPropertyRelative("key").objectReferenceValue == synchedPlayableBinding.sourceObject)
          {
            this.m_BindingPropertiesCache.Add(new DirectorEditor.BindingPropertyPair()
            {
              binding = synchedPlayableBinding,
              property = serializedProperty.FindPropertyRelative("value")
            });
            break;
          }
        }
      }
    }

    public override bool RequiresConstantRepaint()
    {
      return Application.isPlaying;
    }

    private static void PropertyFieldAsFloat(SerializedProperty property, GUIContent title)
    {
      Rect controlRect = EditorGUILayout.GetControlRect();
      title = EditorGUI.BeginProperty(controlRect, title, property);
      EditorGUI.BeginChangeCheck();
      float num = EditorGUI.FloatField(controlRect, title, (float) property.doubleValue);
      if (EditorGUI.EndChangeCheck())
        property.doubleValue = (double) num;
      EditorGUI.EndProperty();
    }

    private static bool PropertyFieldAsObject(SerializedProperty property, GUIContent title, System.Type objType, bool allowSceneObjects, bool useBehaviourGameObject = false)
    {
      Rect controlRect = EditorGUILayout.GetControlRect();
      GUIContent label = EditorGUI.BeginProperty(controlRect, title, property);
      EditorGUI.BeginChangeCheck();
      UnityEngine.Object @object = EditorGUI.ObjectField(controlRect, label, property.objectReferenceValue, objType, allowSceneObjects);
      bool flag = EditorGUI.EndChangeCheck();
      if (flag)
      {
        if (useBehaviourGameObject)
        {
          Behaviour behaviour = @object as Behaviour;
          property.objectReferenceValue = !((UnityEngine.Object) behaviour != (UnityEngine.Object) null) ? (UnityEngine.Object) null : (UnityEngine.Object) behaviour.gameObject;
        }
        else
          property.objectReferenceValue = @object;
      }
      EditorGUI.EndProperty();
      return flag;
    }

    private void CurrentTimeField()
    {
      if (this.targets.Length == 1)
      {
        PlayableDirector target = (PlayableDirector) this.target;
        EditorGUI.BeginChangeCheck();
        float num = EditorGUILayout.FloatField(DirectorEditor.Styles.TimeContent, (float) target.time, new GUILayoutOption[0]);
        if (!EditorGUI.EndChangeCheck())
          return;
        target.time = (double) num;
      }
      else
        EditorGUILayout.TextField(DirectorEditor.Styles.TimeContent, EditorGUI.mixedValueContent.text, new GUILayoutOption[0]);
    }

    private static class Styles
    {
      public static readonly GUIContent PlayableText = EditorGUIUtility.TextContent("Playable");
      public static readonly GUIContent InitialTimeContent = EditorGUIUtility.TextContent("Initial Time|The time at which the Playable will begin playing");
      public static readonly GUIContent TimeContent = EditorGUIUtility.TextContent("Current Time|The current Playable time");
      public static readonly GUIContent InitialStateContent = EditorGUIUtility.TextContent("Play On Awake|Whether the Playable should be playing after it loads");
      public static readonly GUIContent UpdateMethod = EditorGUIUtility.TextContent("Update Method|Controls how the Playable updates every frame");
      public static readonly GUIContent WrapModeContent = EditorGUIUtility.TextContent("Wrap Mode|Controls the behaviour of evaluating the Playable outside its duration");
      public static readonly GUIContent NoBindingsContent = EditorGUIUtility.TextContent("This channel will not playback because it is not currently assigned");
      public static readonly GUIContent BindingsTitleContent = EditorGUIUtility.TextContent("Bindings");
    }

    private struct BindingPropertyPair
    {
      public PlayableBinding binding;
      public SerializedProperty property;
    }
  }
}
