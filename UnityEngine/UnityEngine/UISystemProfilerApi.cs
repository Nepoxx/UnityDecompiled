// Decompiled with JetBrains decompiler
// Type: UnityEngine.UISystemProfilerApi
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  public sealed class UISystemProfilerApi
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BeginSample(UISystemProfilerApi.SampleType type);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EndSample(UISystemProfilerApi.SampleType type);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AddMarker(string name, Object obj);

    public enum SampleType
    {
      Layout,
      Render,
    }
  }
}
