// Decompiled with JetBrains decompiler
// Type: UnityEngine.CapsuleCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class CapsuleCollider2D : Collider2D
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

    public extern CapsuleDirection2D direction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_size_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_size_Injected(ref Vector2 value);
  }
}
