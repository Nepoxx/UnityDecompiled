// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.GraphicsSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Script interface for.</para>
  /// </summary>
  public sealed class GraphicsSettings : Object
  {
    /// <summary>
    ///   <para>Set built-in shader mode.</para>
    /// </summary>
    /// <param name="type">Built-in shader type to change.</param>
    /// <param name="mode">Mode to use for built-in shader.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetShaderMode(BuiltinShaderType type, BuiltinShaderMode mode);

    /// <summary>
    ///   <para>Get built-in shader mode.</para>
    /// </summary>
    /// <param name="type">Built-in shader type to query.</param>
    /// <returns>
    ///   <para>Mode used for built-in shader.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern BuiltinShaderMode GetShaderMode(BuiltinShaderType type);

    /// <summary>
    ///   <para>The RenderPipelineAsset that describes how the Scene should be rendered.</para>
    /// </summary>
    public static RenderPipelineAsset renderPipelineAsset
    {
      get
      {
        return GraphicsSettings.INTERNAL_renderPipelineAsset as RenderPipelineAsset;
      }
      set
      {
        GraphicsSettings.INTERNAL_renderPipelineAsset = (ScriptableObject) value;
      }
    }

    private static extern ScriptableObject INTERNAL_renderPipelineAsset { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set custom shader to use instead of a built-in shader.</para>
    /// </summary>
    /// <param name="type">Built-in shader type to set custom shader to.</param>
    /// <param name="shader">The shader to use.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetCustomShader(BuiltinShaderType type, Shader shader);

    /// <summary>
    ///   <para>Get custom shader used instead of a built-in shader.</para>
    /// </summary>
    /// <param name="type">Built-in shader type to query custom shader for.</param>
    /// <returns>
    ///   <para>The shader used.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Shader GetCustomShader(BuiltinShaderType type);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Object GetGraphicsSettings();

    /// <summary>
    ///   <para>Transparent object sorting mode.</para>
    /// </summary>
    public static extern TransparencySortMode transparencySortMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An axis that describes the direction along which the distances of objects are measured for the purpose of sorting.</para>
    /// </summary>
    public static Vector3 transparencySortAxis
    {
      get
      {
        Vector3 vector3;
        GraphicsSettings.INTERNAL_get_transparencySortAxis(out vector3);
        return vector3;
      }
      set
      {
        GraphicsSettings.INTERNAL_set_transparencySortAxis(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_transparencySortAxis(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_transparencySortAxis(ref Vector3 value);

    /// <summary>
    ///   <para>If this is true, Light intensity is multiplied against linear color values. If it is false, gamma color values are used.</para>
    /// </summary>
    public static extern bool lightsUseLinearIntensity { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether to use a Light's color temperature when calculating the final color of that Light."</para>
    /// </summary>
    public static extern bool lightsUseColorTemperature { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool HasShaderDefineImpl(GraphicsTier tier, BuiltinShaderDefine defineHash);

    /// <summary>
    ///   <para>Returns true if shader define was set when compiling shaders for current GraphicsTier.</para>
    /// </summary>
    /// <param name="tier"></param>
    /// <param name="defineHash"></param>
    public static bool HasShaderDefine(GraphicsTier tier, BuiltinShaderDefine defineHash)
    {
      return GraphicsSettings.HasShaderDefineImpl(tier, defineHash);
    }

    /// <summary>
    ///   <para>Returns true if shader define was set when compiling shaders for given tier.</para>
    /// </summary>
    /// <param name="defineHash"></param>
    public static bool HasShaderDefine(BuiltinShaderDefine defineHash)
    {
      return GraphicsSettings.HasShaderDefine(Graphics.activeTier, defineHash);
    }
  }
}
