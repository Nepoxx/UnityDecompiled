// Decompiled with JetBrains decompiler
// Type: UnityEngine.OcclusionArea
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class OcclusionArea : Component
  {
    public Vector3 center
    {
      get
      {
        Vector3 ret;
        this.get_center_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_center_Injected(ref value);
      }
    }

    public Vector3 size
    {
      get
      {
        Vector3 ret;
        this.get_size_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_size_Injected(ref value);
      }
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_center_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_center_Injected(ref Vector3 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_size_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_size_Injected(ref Vector3 value);
  }
}
