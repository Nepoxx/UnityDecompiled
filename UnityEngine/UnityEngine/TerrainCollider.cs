// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainCollider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A heightmap based collider.</para>
  /// </summary>
  public sealed class TerrainCollider : Collider
  {
    /// <summary>
    ///   <para>The terrain that stores the heightmap.</para>
    /// </summary>
    public extern TerrainData terrainData { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
