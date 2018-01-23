// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Video.VideoClipPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;
using UnityEngine.Video;

namespace UnityEngine.Experimental.Video
{
  [RequiredByNativeCode]
  public struct VideoClipPlayable : IPlayable, IEquatable<VideoClipPlayable>
  {
    private PlayableHandle m_Handle;

    internal VideoClipPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<VideoClipPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an VideoClipPlayable.");
      this.m_Handle = handle;
    }

    public static VideoClipPlayable Create(PlayableGraph graph, VideoClip clip, bool looping)
    {
      VideoClipPlayable playable = new VideoClipPlayable(VideoClipPlayable.CreateHandle(graph, clip, looping));
      if ((UnityEngine.Object) clip != (UnityEngine.Object) null)
        playable.SetDuration<VideoClipPlayable>(clip.length);
      return playable;
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, VideoClip clip, bool looping)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!VideoClipPlayable.InternalCreateVideoClipPlayable(ref graph, clip, looping, ref handle))
        return PlayableHandle.Null;
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(VideoClipPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator VideoClipPlayable(Playable playable)
    {
      return new VideoClipPlayable(playable.GetHandle());
    }

    public bool Equals(VideoClipPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    public VideoClip GetClip()
    {
      return VideoClipPlayable.GetClipInternal(ref this.m_Handle);
    }

    public void SetClip(VideoClip value)
    {
      VideoClipPlayable.SetClipInternal(ref this.m_Handle, value);
    }

    public bool GetLooped()
    {
      return VideoClipPlayable.GetLoopedInternal(ref this.m_Handle);
    }

    public void SetLooped(bool value)
    {
      VideoClipPlayable.SetLoopedInternal(ref this.m_Handle, value);
    }

    public bool IsPlaying()
    {
      return VideoClipPlayable.GetIsPlayingInternal(ref this.m_Handle);
    }

    public double GetStartDelay()
    {
      return VideoClipPlayable.GetStartDelayInternal(ref this.m_Handle);
    }

    internal void SetStartDelay(double value)
    {
      this.ValidateStartDelayInternal(value);
      VideoClipPlayable.SetStartDelayInternal(ref this.m_Handle, value);
    }

    public double GetPauseDelay()
    {
      return VideoClipPlayable.GetPauseDelayInternal(ref this.m_Handle);
    }

    internal void GetPauseDelay(double value)
    {
      double pauseDelayInternal = VideoClipPlayable.GetPauseDelayInternal(ref this.m_Handle);
      if (this.m_Handle.GetPlayState() == PlayState.Playing && (value < 0.05 || pauseDelayInternal != 0.0 && pauseDelayInternal < 0.05))
        throw new ArgumentException("VideoClipPlayable.pauseDelay: Setting new delay when existing delay is too small or 0.0 (" + (object) pauseDelayInternal + "), Video system will not be able to change in time");
      VideoClipPlayable.SetPauseDelayInternal(ref this.m_Handle, value);
    }

    public void Seek(double startTime, double startDelay)
    {
      this.Seek(startTime, startDelay, 0.0);
    }

    public void Seek(double startTime, double startDelay, [DefaultValue(0)] double duration)
    {
      this.ValidateStartDelayInternal(startDelay);
      VideoClipPlayable.SetStartDelayInternal(ref this.m_Handle, startDelay);
      if (duration > 0.0)
      {
        this.m_Handle.SetDuration(duration + startTime);
        VideoClipPlayable.SetPauseDelayInternal(ref this.m_Handle, startDelay + duration);
      }
      else
      {
        this.m_Handle.SetDuration(double.MaxValue);
        VideoClipPlayable.SetPauseDelayInternal(ref this.m_Handle, 0.0);
      }
      this.m_Handle.SetTime(startTime);
      this.m_Handle.Play();
    }

    private void ValidateStartDelayInternal(double startDelay)
    {
      double startDelayInternal = VideoClipPlayable.GetStartDelayInternal(ref this.m_Handle);
      if (!this.IsPlaying() || startDelay >= 0.05 && (startDelayInternal < 1E-05 || startDelayInternal >= 0.05))
        return;
      Debug.LogWarning((object) ("VideoClipPlayable.StartDelay: Setting new delay when existing delay is too small or 0.0 (" + (object) startDelayInternal + "), Video system will not be able to change in time"));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern VideoClip GetClipInternal(ref PlayableHandle hdl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetClipInternal(ref PlayableHandle hdl, VideoClip clip);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetLoopedInternal(ref PlayableHandle hdl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetLoopedInternal(ref PlayableHandle hdl, bool looped);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetIsPlayingInternal(ref PlayableHandle hdl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern double GetStartDelayInternal(ref PlayableHandle hdl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetStartDelayInternal(ref PlayableHandle hdl, double delay);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern double GetPauseDelayInternal(ref PlayableHandle hdl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetPauseDelayInternal(ref PlayableHandle hdl, double delay);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool InternalCreateVideoClipPlayable(ref PlayableGraph graph, VideoClip clip, bool looping, ref PlayableHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool ValidateType(ref PlayableHandle hdl);
  }
}
