// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IAnimationWindowControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal abstract class IAnimationWindowControl : ScriptableObject
  {
    public virtual void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
    }

    public abstract void OnSelectionChanged();

    public abstract AnimationKeyTime time { get; }

    public abstract void GoToTime(float time);

    public abstract void GoToFrame(int frame);

    public abstract void StartScrubTime();

    public abstract void ScrubTime(float time);

    public abstract void EndScrubTime();

    public abstract void GoToPreviousFrame();

    public abstract void GoToNextFrame();

    public abstract void GoToPreviousKeyframe();

    public abstract void GoToNextKeyframe();

    public abstract void GoToFirstKeyframe();

    public abstract void GoToLastKeyframe();

    public abstract bool canPlay { get; }

    public abstract bool playing { get; }

    public abstract bool StartPlayback();

    public abstract void StopPlayback();

    public abstract bool PlaybackUpdate();

    public abstract bool canPreview { get; }

    public abstract bool previewing { get; }

    public abstract bool StartPreview();

    public abstract void StopPreview();

    public abstract bool canRecord { get; }

    public abstract bool recording { get; }

    public abstract bool StartRecording(Object targetObject);

    public abstract void StopRecording();

    public abstract void ResampleAnimation();

    public abstract void ProcessCandidates();

    public abstract void ClearCandidates();
  }
}
