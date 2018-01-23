// Decompiled with JetBrains decompiler
// Type: UnityEngine.PhysicsMaterial2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class PhysicsMaterial2D : Object
  {
    public PhysicsMaterial2D()
    {
      PhysicsMaterial2D.Create_Internal(this, (string) null);
    }

    public PhysicsMaterial2D(string name)
    {
      PhysicsMaterial2D.Create_Internal(this, name);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Create_Internal([Writable] PhysicsMaterial2D scriptMaterial, string name);

    public extern float bounciness { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float friction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
