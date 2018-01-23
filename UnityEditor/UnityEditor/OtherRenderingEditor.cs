// Decompiled with JetBrains decompiler
// Type: UnityEditor.OtherRenderingEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderSettings))]
  internal class OtherRenderingEditor : Editor
  {
    protected SerializedProperty m_HaloStrength;
    protected SerializedProperty m_FlareStrength;
    protected SerializedProperty m_FlareFadeSpeed;
    protected SerializedProperty m_HaloTexture;
    protected SerializedProperty m_SpotCookie;

    public virtual void OnEnable()
    {
      this.m_HaloStrength = this.serializedObject.FindProperty("m_HaloStrength");
      this.m_FlareStrength = this.serializedObject.FindProperty("m_FlareStrength");
      this.m_FlareFadeSpeed = this.serializedObject.FindProperty("m_FlareFadeSpeed");
      this.m_HaloTexture = this.serializedObject.FindProperty("m_HaloTexture");
      this.m_SpotCookie = this.serializedObject.FindProperty("m_SpotCookie");
    }

    public virtual void OnDisable()
    {
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_HaloTexture, OtherRenderingEditor.Styles.HaloTexture, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_HaloStrength, 0.0f, 1f, OtherRenderingEditor.Styles.HaloStrength, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_FlareFadeSpeed, OtherRenderingEditor.Styles.FlareFadeSpeed, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_FlareStrength, 0.0f, 1f, OtherRenderingEditor.Styles.FlareStrength, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SpotCookie, OtherRenderingEditor.Styles.SpotCookie, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    internal static class Styles
    {
      public static readonly GUIContent HaloStrength = EditorGUIUtility.TextContent("Halo Strength|Controls the visibility of the halo effect around lights in the Scene.");
      public static readonly GUIContent HaloTexture = EditorGUIUtility.TextContent("Halo Texture|Specifies the Texture used when drawing the halo effect around lights in the Scene");
      public static readonly GUIContent FlareStrength = EditorGUIUtility.TextContent("Flare Strength|Controls the visibility of lens flares from lights in the Scene.");
      public static readonly GUIContent FlareFadeSpeed = EditorGUIUtility.TextContent("Flare Fade Speed|Controls the time over which lens flares fade from view after initially appearing.");
      public static readonly GUIContent SpotCookie = EditorGUIUtility.TextContent("Spot Cookie|Specifies the Texture mask used to cast shadows, create silhouettes, or patterned illumination when using spot lights.");
    }
  }
}
