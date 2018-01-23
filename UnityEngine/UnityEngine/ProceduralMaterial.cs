// Decompiled with JetBrains decompiler
// Type: UnityEngine.ProceduralMaterial
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for ProceduralMaterial handling.</para>
  /// </summary>
  [Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install a suitable third-party external importer from the Asset Store.", false)]
  public sealed class ProceduralMaterial : Material
  {
    internal ProceduralMaterial()
      : base((Material) null)
    {
    }

    /// <summary>
    ///   <para>Get an array of descriptions of all the ProceduralProperties this ProceduralMaterial has.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ProceduralPropertyDescription[] GetProceduralPropertyDescriptions();

    /// <summary>
    ///   <para>Checks if the ProceduralMaterial has a ProceduralProperty of a given name.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool HasProceduralProperty(string inputName);

    /// <summary>
    ///   <para>Get a named Procedural boolean property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetProceduralBoolean(string inputName);

    /// <summary>
    ///   <para>Checks if a given ProceduralProperty is visible according to the values of this ProceduralMaterial's other ProceduralProperties and to the ProceduralProperty's visibleIf expression.</para>
    /// </summary>
    /// <param name="inputName">The name of the ProceduralProperty whose visibility is evaluated.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsProceduralPropertyVisible(string inputName);

    /// <summary>
    ///   <para>Set a named Procedural boolean property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetProceduralBoolean(string inputName, bool value);

    /// <summary>
    ///   <para>Get a named Procedural float property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern float GetProceduralFloat(string inputName);

    /// <summary>
    ///   <para>Set a named Procedural float property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetProceduralFloat(string inputName, float value);

    /// <summary>
    ///   <para>Get a named Procedural vector property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    public Vector4 GetProceduralVector(string inputName)
    {
      Vector4 vector4;
      ProceduralMaterial.INTERNAL_CALL_GetProceduralVector(this, inputName, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetProceduralVector(ProceduralMaterial self, string inputName, out Vector4 value);

    /// <summary>
    ///   <para>Set a named Procedural vector property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    public void SetProceduralVector(string inputName, Vector4 value)
    {
      ProceduralMaterial.INTERNAL_CALL_SetProceduralVector(this, inputName, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetProceduralVector(ProceduralMaterial self, string inputName, ref Vector4 value);

    /// <summary>
    ///   <para>Get a named Procedural color property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    public Color GetProceduralColor(string inputName)
    {
      Color color;
      ProceduralMaterial.INTERNAL_CALL_GetProceduralColor(this, inputName, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetProceduralColor(ProceduralMaterial self, string inputName, out Color value);

    /// <summary>
    ///   <para>Set a named Procedural color property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    public void SetProceduralColor(string inputName, Color value)
    {
      ProceduralMaterial.INTERNAL_CALL_SetProceduralColor(this, inputName, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetProceduralColor(ProceduralMaterial self, string inputName, ref Color value);

    /// <summary>
    ///   <para>Get a named Procedural enum property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetProceduralEnum(string inputName);

    /// <summary>
    ///   <para>Set a named Procedural enum property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetProceduralEnum(string inputName, int value);

    /// <summary>
    ///   <para>Get a named Procedural texture property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Texture2D GetProceduralTexture(string inputName);

    /// <summary>
    ///   <para>Set a named Procedural texture property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetProceduralTexture(string inputName, Texture2D value);

    /// <summary>
    ///   <para>Get a named Procedural string property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetProceduralString(string inputName);

    /// <summary>
    ///   <para>Set a named Procedural string property.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetProceduralString(string inputName, string value);

    /// <summary>
    ///   <para>Checks if a named ProceduralProperty is cached for efficient runtime tweaking.</para>
    /// </summary>
    /// <param name="inputName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsProceduralPropertyCached(string inputName);

    /// <summary>
    ///   <para>Specifies if a named ProceduralProperty should be cached for efficient runtime tweaking.</para>
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CacheProceduralProperty(string inputName, bool value);

    /// <summary>
    ///   <para>Clear the Procedural cache.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearCache();

    /// <summary>
    ///   <para>Set or get the Procedural cache budget.</para>
    /// </summary>
    public extern ProceduralCacheSize cacheSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set or get the update rate in millisecond of the animated substance.</para>
    /// </summary>
    public extern int animationUpdateRate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Triggers an asynchronous rebuild of this ProceduralMaterial's dirty textures.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RebuildTextures();

    /// <summary>
    ///   <para>Triggers an immediate (synchronous) rebuild of this ProceduralMaterial's dirty textures.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RebuildTexturesImmediately();

    /// <summary>
    ///   <para>Check if the ProceduralTextures from this ProceduralMaterial are currently being rebuilt.</para>
    /// </summary>
    public extern bool isProcessing { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Discard all the queued ProceduralMaterial rendering operations that have not started yet.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopRebuilds();

    /// <summary>
    ///   <para>Indicates whether cached data is available for this ProceduralMaterial's textures (only relevant for Cache and DoNothingAndCache loading behaviors).</para>
    /// </summary>
    public extern bool isCachedDataAvailable { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Should the ProceduralMaterial be generated at load time?</para>
    /// </summary>
    public extern bool isLoadTimeGenerated { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get ProceduralMaterial loading behavior.</para>
    /// </summary>
    public extern ProceduralLoadingBehavior loadingBehavior { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Check if ProceduralMaterials are supported on the current platform.</para>
    /// </summary>
    public static extern bool isSupported { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Used to specify the Substance engine CPU usage.</para>
    /// </summary>
    public static extern ProceduralProcessorUsage substanceProcessorUsage { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set or get an XML string of "input/value" pairs (setting the preset rebuilds the textures).</para>
    /// </summary>
    public extern string preset { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get generated textures.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Texture[] GetGeneratedTextures();

    /// <summary>
    ///   <para>This allows to get a reference to a ProceduralTexture generated by a ProceduralMaterial using its name.</para>
    /// </summary>
    /// <param name="textureName">The name of the ProceduralTexture to get.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ProceduralTexture GetGeneratedTexture(string textureName);

    /// <summary>
    ///   <para>Set or get the "Readable" flag for a ProceduralMaterial.</para>
    /// </summary>
    public extern bool isReadable { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Render a ProceduralMaterial immutable and release the underlying data to decrease the memory footprint.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void FreezeAndReleaseSourceData();

    /// <summary>
    ///   <para>Returns true if FreezeAndReleaseSourceData was called on this ProceduralMaterial.</para>
    /// </summary>
    public extern bool isFrozen { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
