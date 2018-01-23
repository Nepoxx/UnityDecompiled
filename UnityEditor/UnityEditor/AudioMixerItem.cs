// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Audio;
using UnityEditor.IMGUI.Controls;

namespace UnityEditor
{
  internal class AudioMixerItem : TreeViewItem
  {
    private const string kSuspendedText = " - Inactive";

    public AudioMixerItem(int id, int depth, TreeViewItem parent, string displayName, AudioMixerController mixer, string infoText)
      : base(id, depth, parent, displayName)
    {
      this.mixer = mixer;
      this.infoText = infoText;
      this.UpdateSuspendedString(true);
    }

    public AudioMixerController mixer { get; set; }

    public string infoText { get; set; }

    public float labelWidth { get; set; }

    private bool lastSuspendedState { get; set; }

    public void UpdateSuspendedString(bool force)
    {
      bool isSuspended = this.mixer.isSuspended;
      if (this.lastSuspendedState == isSuspended && !force)
        return;
      this.lastSuspendedState = isSuspended;
      if (isSuspended)
        this.AddSuspendedText();
      else
        this.RemoveSuspendedText();
    }

    private void RemoveSuspendedText()
    {
      int startIndex = this.infoText.IndexOf(" - Inactive", StringComparison.Ordinal);
      if (startIndex < 0)
        return;
      this.infoText = this.infoText.Remove(startIndex, " - Inactive".Length);
    }

    private void AddSuspendedText()
    {
      if (this.infoText.IndexOf(" - Inactive", StringComparison.Ordinal) >= 0)
        return;
      this.infoText += " - Inactive";
    }
  }
}
