// Decompiled with JetBrains decompiler
// Type: UnityEditor.FogEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderSettings))]
  internal class FogEditor : Editor
  {
    protected SerializedProperty m_Fog;
    protected SerializedProperty m_FogColor;
    protected SerializedProperty m_FogMode;
    protected SerializedProperty m_FogDensity;
    protected SerializedProperty m_LinearFogStart;
    protected SerializedProperty m_LinearFogEnd;

    public virtual void OnEnable()
    {
      this.m_Fog = this.serializedObject.FindProperty("m_Fog");
      this.m_FogColor = this.serializedObject.FindProperty("m_FogColor");
      this.m_FogMode = this.serializedObject.FindProperty("m_FogMode");
      this.m_FogDensity = this.serializedObject.FindProperty("m_FogDensity");
      this.m_LinearFogStart = this.serializedObject.FindProperty("m_LinearFogStart");
      this.m_LinearFogEnd = this.serializedObject.FindProperty("m_LinearFogEnd");
    }

    public virtual void OnDisable()
    {
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Fog, FogEditor.Styles.FogEnable, new GUILayoutOption[0]);
      if (this.m_Fog.boolValue)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_FogColor, FogEditor.Styles.FogColor, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_FogMode, FogEditor.Styles.FogMode, new GUILayoutOption[0]);
        if (this.m_FogMode.intValue != 1)
        {
          EditorGUILayout.PropertyField(this.m_FogDensity, FogEditor.Styles.FogDensity, new GUILayoutOption[0]);
        }
        else
        {
          EditorGUILayout.PropertyField(this.m_LinearFogStart, FogEditor.Styles.FogLinearStart, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LinearFogEnd, FogEditor.Styles.FogLinearEnd, new GUILayoutOption[0]);
        }
        if (SceneView.IsUsingDeferredRenderingPath())
          EditorGUILayout.HelpBox(FogEditor.Styles.FogWarning.text, MessageType.Info);
        --EditorGUI.indentLevel;
        EditorGUILayout.Space();
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    internal class Styles
    {
      public static readonly GUIContent FogWarning = EditorGUIUtility.TextContent("Fog has no effect on opaque objects when using Deferred Shading rendering. Use the Global Fog image effect instead, which supports opaque objects.");
      public static readonly GUIContent FogDensity = EditorGUIUtility.TextContent("Density|Controls the density of the fog effect in the Scene when using Exponential or Exponential Squared modes.");
      public static readonly GUIContent FogLinearStart = EditorGUIUtility.TextContent("Start|Controls the distance from the camera where the fog will start in the Scene.");
      public static readonly GUIContent FogLinearEnd = EditorGUIUtility.TextContent("End|Controls the distance from the camera where the fog will completely obscure objects in the Scene.");
      public static readonly GUIContent FogEnable = EditorGUIUtility.TextContent("Fog|Specifies whether fog is used in the Scene or not.");
      public static readonly GUIContent FogColor = EditorGUIUtility.TextContent("Color|Controls the color of that fog drawn in the Scene.");
      public static readonly GUIContent FogMode = EditorGUIUtility.TextContent("Mode|Controls the mathematical function determining the way fog accumulates with distance from the camera. Options are Linear, Exponential, and Exponential Squared.");
    }
  }
}
