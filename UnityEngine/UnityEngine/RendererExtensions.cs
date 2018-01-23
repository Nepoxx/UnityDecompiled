// Decompiled with JetBrains decompiler
// Type: UnityEngine.RendererExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Extension methods to the Renderer class, used only for the UpdateGIMaterials method used by the Global Illumination System.</para>
  /// </summary>
  public static class RendererExtensions
  {
    /// <summary>
    ///   <para>Schedules an update of the albedo and emissive Textures of a system that contains the Renderer.</para>
    /// </summary>
    /// <param name="renderer"></param>
    public static void UpdateGIMaterials(this Renderer renderer)
    {
      RendererExtensions.UpdateGIMaterialsForRenderer(renderer);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateGIMaterialsForRenderer(Renderer renderer);
  }
}
