// Decompiled with JetBrains decompiler
// Type: UnityEngine.EdgeCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  public sealed class EdgeCollider2D : Collider2D
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Reset();

    public extern float edgeRadius { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern int edgeCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int pointCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern Vector2[] points { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
