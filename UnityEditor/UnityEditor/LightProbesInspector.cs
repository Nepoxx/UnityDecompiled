// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbesInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (LightProbes))]
  internal class LightProbesInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      LightProbes target = this.target as LightProbes;
      GUIStyle wrappedMiniLabel = EditorStyles.wordWrappedMiniLabel;
      GUILayout.Label("Light probe count: " + (object) target.count, wrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.Label("Cell count: " + (object) target.cellCount, wrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
    }
  }
}
