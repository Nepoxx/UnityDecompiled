// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnchoredJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public class AnchoredJoint2D : Joint2D
  {
    public Vector2 anchor
    {
      get
      {
        Vector2 ret;
        this.get_anchor_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_anchor_Injected(ref value);
      }
    }

    public Vector2 connectedAnchor
    {
      get
      {
        Vector2 ret;
        this.get_connectedAnchor_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_connectedAnchor_Injected(ref value);
      }
    }

    public extern bool autoConfigureConnectedAnchor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_anchor_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_anchor_Injected(ref Vector2 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_connectedAnchor_Injected(out Vector2 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_connectedAnchor_Injected(ref Vector2 value);
  }
}
