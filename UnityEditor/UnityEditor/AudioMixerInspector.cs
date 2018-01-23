// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioMixer))]
  [CanEditMultipleObjects]
  internal class AudioMixerInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      GUILayout.Space(10f);
      EditorGUILayout.HelpBox("Modification and inspection of built AudioMixer assets is disabled. Please modify the source asset and re-build.", MessageType.Info);
    }
  }
}
