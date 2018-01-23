// Decompiled with JetBrains decompiler
// Type: UnityEngine.ShaderVariantCollection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>ShaderVariantCollection records which shader variants are actually used in each shader.</para>
  /// </summary>
  public sealed class ShaderVariantCollection : Object
  {
    /// <summary>
    ///   <para>Create a new empty shader variant collection.</para>
    /// </summary>
    public ShaderVariantCollection()
    {
      ShaderVariantCollection.Internal_Create(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] ShaderVariantCollection mono);

    /// <summary>
    ///   <para>Number of shaders in this collection (Read Only).</para>
    /// </summary>
    public extern int shaderCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of total varians in this collection (Read Only).</para>
    /// </summary>
    public extern int variantCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool Add(ShaderVariantCollection.ShaderVariant variant)
    {
      return this.AddInternal(variant.shader, variant.passType, variant.keywords);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool AddInternal(Shader shader, PassType passType, string[] keywords);

    public bool Remove(ShaderVariantCollection.ShaderVariant variant)
    {
      return this.RemoveInternal(variant.shader, variant.passType, variant.keywords);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool RemoveInternal(Shader shader, PassType passType, string[] keywords);

    public bool Contains(ShaderVariantCollection.ShaderVariant variant)
    {
      return this.ContainsInternal(variant.shader, variant.passType, variant.keywords);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool ContainsInternal(Shader shader, PassType passType, string[] keywords);

    /// <summary>
    ///   <para>Remove all shader variants from the collection.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Clear();

    /// <summary>
    ///   <para>Is this ShaderVariantCollection already warmed up? (Read Only)</para>
    /// </summary>
    public extern bool isWarmedUp { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Fully load shaders in ShaderVariantCollection.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void WarmUp();

    /// <summary>
    ///   <para>Identifies a specific variant of a shader.</para>
    /// </summary>
    public struct ShaderVariant
    {
      /// <summary>
      ///   <para>Shader to use in this variant.</para>
      /// </summary>
      public Shader shader;
      /// <summary>
      ///   <para>Pass type to use in this variant.</para>
      /// </summary>
      public PassType passType;
      /// <summary>
      ///   <para>Array of shader keywords to use in this variant.</para>
      /// </summary>
      public string[] keywords;

      /// <summary>
      ///   <para>Creates a ShaderVariant structure.</para>
      /// </summary>
      /// <param name="shader"></param>
      /// <param name="passType"></param>
      /// <param name="keywords"></param>
      public ShaderVariant(Shader shader, PassType passType, params string[] keywords)
      {
        this.shader = shader;
        this.passType = passType;
        this.keywords = keywords;
        ShaderVariantCollection.ShaderVariant.Internal_CheckVariant(shader, passType, keywords);
      }

      [GeneratedByOldBindingsGenerator]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void Internal_CheckVariant(Shader shader, PassType passType, string[] keywords);
    }
  }
}
