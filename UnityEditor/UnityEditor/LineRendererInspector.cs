// Decompiled with JetBrains decompiler
// Type: UnityEditor.LineRendererInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (LineRenderer))]
  [CanEditMultipleObjects]
  internal class LineRendererInspector : RendererEditorBase
  {
    private LineRendererCurveEditor m_CurveEditor = new LineRendererCurveEditor();
    private string[] m_ExcludedProperties;
    private SerializedProperty m_ColorGradient;
    private SerializedProperty m_NumCornerVertices;
    private SerializedProperty m_NumCapVertices;
    private SerializedProperty m_Alignment;
    private SerializedProperty m_TextureMode;
    private SerializedProperty m_GenerateLightingData;

    public override void OnEnable()
    {
      base.OnEnable();
      List<string> stringList = new List<string>();
      stringList.Add("m_Parameters");
      stringList.AddRange((IEnumerable<string>) RendererEditorBase.Probes.GetFieldsStringArray());
      this.m_ExcludedProperties = stringList.ToArray();
      this.m_CurveEditor.OnEnable(this.serializedObject);
      this.m_ColorGradient = this.serializedObject.FindProperty("m_Parameters.colorGradient");
      this.m_NumCornerVertices = this.serializedObject.FindProperty("m_Parameters.numCornerVertices");
      this.m_NumCapVertices = this.serializedObject.FindProperty("m_Parameters.numCapVertices");
      this.m_Alignment = this.serializedObject.FindProperty("m_Parameters.alignment");
      this.m_TextureMode = this.serializedObject.FindProperty("m_Parameters.textureMode");
      this.m_GenerateLightingData = this.serializedObject.FindProperty("m_Parameters.generateLightingData");
      this.InitializeProbeFields();
    }

    public void OnDisable()
    {
      this.m_CurveEditor.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Editor.DrawPropertiesExcluding(this.m_SerializedObject, this.m_ExcludedProperties);
      this.m_CurveEditor.CheckCurveChangedExternally();
      this.m_CurveEditor.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_ColorGradient, LineRendererInspector.Styles.colorGradient, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_NumCornerVertices, LineRendererInspector.Styles.numCornerVertices, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_NumCapVertices, LineRendererInspector.Styles.numCapVertices, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Alignment, LineRendererInspector.Styles.alignment, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_TextureMode, LineRendererInspector.Styles.textureMode, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_GenerateLightingData, LineRendererInspector.Styles.generateLightingData, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      this.RenderSortingLayerFields();
      this.m_Probes.OnGUI(this.targets, (Renderer) this.target, false);
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public static GUIContent colorGradient = EditorGUIUtility.TextContent("Color|The gradient describing the color along the line.");
      public static GUIContent numCornerVertices = EditorGUIUtility.TextContent("Corner Vertices|How many vertices to add for each corner.");
      public static GUIContent numCapVertices = EditorGUIUtility.TextContent("End Cap Vertices|How many vertices to add at each end.");
      public static GUIContent alignment = EditorGUIUtility.TextContent("Alignment|Lines can rotate to face their transform component or the camera. Note that when using Local mode, lines will face the XY plane of the Transform.");
      public static GUIContent textureMode = EditorGUIUtility.TextContent("Texture Mode|Should the U coordinate be stretched or tiled?");
      public static GUIContent generateLightingData = EditorGUIUtility.TextContent("Generate Lighting Data|Toggle generation of normal and tangent data, for use in lit shaders.");
    }
  }
}
