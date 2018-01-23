// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioLowPassFilterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AudioLowPassFilter))]
  internal class AudioLowPassFilterInspector : Editor
  {
    private SerializedProperty m_LowpassResonanceQ;
    private SerializedProperty m_LowpassLevelCustomCurve;

    private void OnEnable()
    {
      this.m_LowpassResonanceQ = this.serializedObject.FindProperty("m_LowpassResonanceQ");
      this.m_LowpassLevelCustomCurve = this.serializedObject.FindProperty("lowpassLevelCustomCurve");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      AudioSourceInspector.AnimProp(new GUIContent("Cutoff Frequency"), this.m_LowpassLevelCustomCurve, 0.0f, 22000f, true);
      EditorGUILayout.PropertyField(this.m_LowpassResonanceQ);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
