// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioSourceExtensionEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AudioSourceExtensionEditor : AudioExtensionEditor
  {
    public virtual void OnAudioSourceGUI()
    {
    }

    public virtual void OnAudioSourceSceneGUI(AudioSource source)
    {
    }

    protected override int GetNumSerializedExtensionProperties(Object obj)
    {
      AudioSource audioSource = obj as AudioSource;
      return !(bool) ((Object) audioSource) ? 0 : audioSource.GetNumExtensionProperties();
    }
  }
}
