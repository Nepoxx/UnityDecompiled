// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.PlayableBinding
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;

namespace UnityEngine.Playables
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct PlayableBinding
  {
    public static readonly PlayableBinding[] None = new PlayableBinding[0];
    public static readonly double DefaultDuration = double.PositiveInfinity;

    public string streamName { get; set; }

    public DataStreamType streamType { get; set; }

    public UnityEngine.Object sourceObject { get; set; }

    public System.Type sourceBindingType { get; set; }
  }
}
