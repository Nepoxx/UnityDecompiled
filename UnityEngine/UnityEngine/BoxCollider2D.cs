// Decompiled with JetBrains decompiler
// Type: UnityEngine.BoxCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class BoxCollider2D : Collider2D
  {
    public Vector2 size
    {
      get
      {
        Vector2 ret;
        this.get_size_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_size_Injected(ref value);
      }
    }

    public extern float edgeRadius { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern bool autoTiling { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("BoxCollider2D.center has been deprecated. Use BoxCollider2D.offset instead (UnityUpgradable) -> offset", true)]
    public Vector2 center
    {
      get
      {
        return Vector2.zero;
      }
      set
      {
      }
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_size_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_size_Injected(ref Vector2 value);
  }
}
