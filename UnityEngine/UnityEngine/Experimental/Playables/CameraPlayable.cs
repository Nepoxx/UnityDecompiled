// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Playables.CameraPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
  [RequiredByNativeCode]
  public struct CameraPlayable : IPlayable, IEquatable<CameraPlayable>
  {
    private PlayableHandle m_Handle;

    internal CameraPlayable(PlayableHandle handle)
    {
      if (handle.IsValid() && !handle.IsPlayableOfType<CameraPlayable>())
        throw new InvalidCastException("Can't set handle: the playable is not an CameraPlayable.");
      this.m_Handle = handle;
    }

    public static CameraPlayable Create(PlayableGraph graph, Camera camera)
    {
      return new CameraPlayable(CameraPlayable.CreateHandle(graph, camera));
    }

    private static PlayableHandle CreateHandle(PlayableGraph graph, Camera camera)
    {
      PlayableHandle handle = PlayableHandle.Null;
      if (!CameraPlayable.InternalCreateCameraPlayable(ref graph, camera, ref handle))
        return PlayableHandle.Null;
      return handle;
    }

    public PlayableHandle GetHandle()
    {
      return this.m_Handle;
    }

    public static implicit operator Playable(CameraPlayable playable)
    {
      return new Playable(playable.GetHandle());
    }

    public static explicit operator CameraPlayable(Playable playable)
    {
      return new CameraPlayable(playable.GetHandle());
    }

    public bool Equals(CameraPlayable other)
    {
      return this.GetHandle() == other.GetHandle();
    }

    public Camera GetCamera()
    {
      return CameraPlayable.GetCameraInternal(ref this.m_Handle);
    }

    public void SetCamera(Camera value)
    {
      CameraPlayable.SetCameraInternal(ref this.m_Handle, value);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Camera GetCameraInternal(ref PlayableHandle hdl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetCameraInternal(ref PlayableHandle hdl, Camera camera);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool InternalCreateCameraPlayable(ref PlayableGraph graph, Camera camera, ref PlayableHandle handle);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool ValidateType(ref PlayableHandle hdl);
  }
}
