// Decompiled with JetBrains decompiler
// Type: UnityEngine.Effector2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public class Effector2D : Behaviour
  {
    public extern bool useColliderMask { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern int colliderMask { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal extern bool requiresCollider { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern bool designedForTrigger { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern bool designedForNonTrigger { [MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
