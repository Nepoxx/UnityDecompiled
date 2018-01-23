// Decompiled with JetBrains decompiler
// Type: UnityEditor.CanvasEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Canvas))]
  internal class CanvasEditor : Editor
  {
    private bool m_AllNested = false;
    private bool m_AllRoot = false;
    private bool m_AllOverlay = false;
    private bool m_NoneOverlay = false;
    private string[] shaderChannelOptions = new string[5]{ "TexCoord1", "TexCoord2", "TexCoord3", "Normal", "Tangent" };
    private CanvasEditor.PixelPerfect pixelPerfect = CanvasEditor.PixelPerfect.Inherit;
    private SerializedProperty m_RenderMode;
    private SerializedProperty m_Camera;
    private SerializedProperty m_PixelPerfect;
    private SerializedProperty m_PixelPerfectOverride;
    private SerializedProperty m_PlaneDistance;
    private SerializedProperty m_SortingLayerID;
    private SerializedProperty m_SortingOrder;
    private SerializedProperty m_TargetDisplay;
    private SerializedProperty m_OverrideSorting;
    private SerializedProperty m_ShaderChannels;
    private AnimBool m_OverlayMode;
    private AnimBool m_CameraMode;
    private AnimBool m_WorldMode;
    private AnimBool m_SortingOverride;

    private void OnEnable()
    {
      this.m_RenderMode = this.serializedObject.FindProperty("m_RenderMode");
      this.m_Camera = this.serializedObject.FindProperty("m_Camera");
      this.m_PixelPerfect = this.serializedObject.FindProperty("m_PixelPerfect");
      this.m_PlaneDistance = this.serializedObject.FindProperty("m_PlaneDistance");
      this.m_SortingLayerID = this.serializedObject.FindProperty("m_SortingLayerID");
      this.m_SortingOrder = this.serializedObject.FindProperty("m_SortingOrder");
      this.m_TargetDisplay = this.serializedObject.FindProperty("m_TargetDisplay");
      this.m_OverrideSorting = this.serializedObject.FindProperty("m_OverrideSorting");
      this.m_PixelPerfectOverride = this.serializedObject.FindProperty("m_OverridePixelPerfect");
      this.m_ShaderChannels = this.serializedObject.FindProperty("m_AdditionalShaderChannelsFlag");
      this.m_OverlayMode = new AnimBool(this.m_RenderMode.intValue == 0);
      this.m_OverlayMode.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_CameraMode = new AnimBool(this.m_RenderMode.intValue == 1);
      this.m_CameraMode.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_WorldMode = new AnimBool(this.m_RenderMode.intValue == 2);
      this.m_WorldMode.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_SortingOverride = new AnimBool(this.m_OverrideSorting.boolValue);
      this.m_SortingOverride.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.pixelPerfect = !this.m_PixelPerfectOverride.boolValue ? CanvasEditor.PixelPerfect.Inherit : (!this.m_PixelPerfect.boolValue ? CanvasEditor.PixelPerfect.Off : CanvasEditor.PixelPerfect.On);
      this.m_AllNested = true;
      this.m_AllRoot = true;
      this.m_AllOverlay = true;
      this.m_NoneOverlay = true;
      for (int index = 0; index < this.targets.Length; ++index)
      {
        Canvas target = this.targets[index] as Canvas;
        if ((UnityEngine.Object) target.transform.parent == (UnityEngine.Object) null || (UnityEngine.Object) target.transform.parent.GetComponentInParent<Canvas>() == (UnityEngine.Object) null)
          this.m_AllNested = false;
        else
          this.m_AllRoot = false;
        if (target.renderMode == UnityEngine.RenderMode.ScreenSpaceOverlay)
          this.m_NoneOverlay = false;
        else
          this.m_AllOverlay = false;
      }
    }

    private void OnDisable()
    {
      this.m_OverlayMode.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_CameraMode.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_WorldMode.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_SortingOverride.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    private void AllRootCanvases()
    {
      EditorGUILayout.PropertyField(this.m_RenderMode);
      this.m_OverlayMode.target = this.m_RenderMode.intValue == 0;
      this.m_CameraMode.target = this.m_RenderMode.intValue == 1;
      this.m_WorldMode.target = this.m_RenderMode.intValue == 2;
      ++EditorGUI.indentLevel;
      if (EditorGUILayout.BeginFadeGroup(this.m_OverlayMode.faded))
      {
        EditorGUILayout.PropertyField(this.m_PixelPerfect);
        EditorGUILayout.PropertyField(this.m_SortingOrder, CanvasEditor.Styles.sortingOrder, new GUILayoutOption[0]);
        EditorGUILayout.IntPopup(this.m_TargetDisplay, DisplayUtility.GetDisplayNames(), DisplayUtility.GetDisplayIndices(), CanvasEditor.Styles.targetDisplay, new GUILayoutOption[0]);
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_CameraMode.faded))
      {
        EditorGUILayout.PropertyField(this.m_PixelPerfect);
        EditorGUILayout.PropertyField(this.m_Camera, CanvasEditor.Styles.renderCamera, new GUILayoutOption[0]);
        if (this.m_Camera.objectReferenceValue != (UnityEngine.Object) null)
          EditorGUILayout.PropertyField(this.m_PlaneDistance);
        EditorGUILayout.Space();
        if (this.m_Camera.objectReferenceValue != (UnityEngine.Object) null)
          EditorGUILayout.SortingLayerField(CanvasEditor.Styles.m_SortingLayerStyle, this.m_SortingLayerID, EditorStyles.popup, EditorStyles.label);
        EditorGUILayout.PropertyField(this.m_SortingOrder, CanvasEditor.Styles.m_SortingOrderStyle, new GUILayoutOption[0]);
        if (this.m_Camera.objectReferenceValue == (UnityEngine.Object) null)
          EditorGUILayout.HelpBox("Screen Space - A canvas with no specified camera acts like a Overlay Canvas. Please assign a camera to it in the 'Render Camera' field.", MessageType.Warning);
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_WorldMode.faded))
      {
        EditorGUILayout.PropertyField(this.m_Camera, CanvasEditor.Styles.eventCamera, new GUILayoutOption[0]);
        EditorGUILayout.Space();
        EditorGUILayout.SortingLayerField(CanvasEditor.Styles.m_SortingLayerStyle, this.m_SortingLayerID, EditorStyles.popup);
        EditorGUILayout.PropertyField(this.m_SortingOrder, CanvasEditor.Styles.m_SortingOrderStyle, new GUILayoutOption[0]);
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
    }

    private void AllNestedCanvases()
    {
      EditorGUI.BeginChangeCheck();
      this.pixelPerfect = (CanvasEditor.PixelPerfect) EditorGUILayout.EnumPopup("Pixel Perfect", (Enum) this.pixelPerfect, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (this.pixelPerfect == CanvasEditor.PixelPerfect.Inherit)
          this.m_PixelPerfectOverride.boolValue = false;
        else if (this.pixelPerfect == CanvasEditor.PixelPerfect.Off)
        {
          this.m_PixelPerfectOverride.boolValue = true;
          this.m_PixelPerfect.boolValue = false;
        }
        else
        {
          this.m_PixelPerfectOverride.boolValue = true;
          this.m_PixelPerfect.boolValue = true;
        }
      }
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_OverrideSorting);
      if (EditorGUI.EndChangeCheck())
      {
        ((Canvas) this.serializedObject.targetObject).overrideSorting = this.m_OverrideSorting.boolValue;
        this.m_SortingOverride.target = this.m_OverrideSorting.boolValue;
      }
      if (EditorGUILayout.BeginFadeGroup(this.m_SortingOverride.faded))
      {
        GUIContent label = (GUIContent) null;
        if (this.m_AllOverlay)
          label = CanvasEditor.Styles.sortingOrder;
        else if (this.m_NoneOverlay)
        {
          label = CanvasEditor.Styles.m_SortingOrderStyle;
          EditorGUILayout.SortingLayerField(CanvasEditor.Styles.m_SortingLayerStyle, this.m_SortingLayerID, EditorStyles.popup);
        }
        if (label != null)
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_SortingOrder, label, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
            ((Canvas) this.serializedObject.targetObject).sortingOrder = this.m_SortingOrder.intValue;
        }
      }
      EditorGUILayout.EndFadeGroup();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (this.m_AllRoot || this.m_AllNested)
      {
        if (this.m_AllRoot)
          this.AllRootCanvases();
        else if (this.m_AllNested)
          this.AllNestedCanvases();
        EditorGUI.BeginChangeCheck();
        int num = EditorGUILayout.MaskField(CanvasEditor.Styles.m_ShaderChannel, this.m_ShaderChannels.intValue, this.shaderChannelOptions, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          this.m_ShaderChannels.intValue = num;
        if (this.m_RenderMode.intValue == 0 && (num & 8 | num & 16) != 0)
          EditorGUILayout.HelpBox("Shader channels Normal and Tangent are most often used with lighting, which an Overlay canvas does not support. Its likely these channels are not needed.", MessageType.Warning);
      }
      else
        GUILayout.Label(CanvasEditor.Styles.s_RootAndNestedMessage, EditorStyles.helpBox, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    private static class Styles
    {
      public static GUIContent eventCamera = new GUIContent("Event Camera", "The Camera which the events are triggered through. This is used to determine clicking and hover positions if the Canvas is in World Space render mode.");
      public static GUIContent renderCamera = new GUIContent("Render Camera", "The Camera which will render the canvas. This is also the camera used to send events.");
      public static GUIContent sortingOrder = new GUIContent("Sort Order", "The order in which Screen Space - Overlay canvas will render");
      public static string s_RootAndNestedMessage = "Cannot multi-edit root Canvas together with nested Canvas.";
      public static GUIContent m_SortingLayerStyle = EditorGUIUtility.TextContent("Sorting Layer|Name of the Renderer's sorting layer");
      public static GUIContent targetDisplay = new GUIContent("Target Display", "Display on which to render the canvas when in overlay mode");
      public static GUIContent m_SortingOrderStyle = EditorGUIUtility.TextContent("Order in Layer|Renderer's order within a sorting layer");
      public static GUIContent m_ShaderChannel = EditorGUIUtility.TextContent("Additional Shader Channels");
    }

    private enum PixelPerfect
    {
      Inherit,
      On,
      Off,
    }
  }
}
