// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemExtensionsImpl
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class ParticleSystemExtensionsImpl
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSafeCollisionEventSize(ParticleSystem ps);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetCollisionEventsDeprecated(ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSafeTriggerParticlesSize(ParticleSystem ps, int type);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetCollisionEvents(ParticleSystem ps, GameObject go, object collisionEvents);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetTriggerParticles(ParticleSystem ps, int type, object particles);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetTriggerParticles(ParticleSystem ps, int type, object particles, int offset, int count);
  }
}
