// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioListenerExtensionEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AudioListenerExtensionEditor : AudioExtensionEditor
  {
    public virtual void OnAudioListenerGUI()
    {
    }

    protected override int GetNumSerializedExtensionProperties(Object obj)
    {
      AudioListener audioListener = obj as AudioListener;
      return !(bool) ((Object) audioListener) ? 0 : audioListener.GetNumExtensionProperties();
    }
  }
}
