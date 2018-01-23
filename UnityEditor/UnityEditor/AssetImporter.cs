// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class from which asset importers for specific asset types derive.</para>
  /// </summary>
  public class AssetImporter : UnityEngine.Object
  {
    /// <summary>
    ///   <para>The path name of the asset for this importer. (Read Only)</para>
    /// </summary>
    public extern string assetPath { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern ulong assetTimeStamp { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get or set any user data.</para>
    /// </summary>
    public extern string userData { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get or set the AssetBundle name.</para>
    /// </summary>
    public extern string assetBundleName { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get or set the AssetBundle variant.</para>
    /// </summary>
    public extern string assetBundleVariant { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the AssetBundle name and variant.</para>
    /// </summary>
    /// <param name="assetBundleName">AssetBundle name.</param>
    /// <param name="assetBundleVariant">AssetBundle variant.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetAssetBundleNameAndVariant(string assetBundleName, string assetBundleVariant);

    /// <summary>
    ///   <para>Retrieves the asset importer for the asset at path.</para>
    /// </summary>
    /// <param name="path"></param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AssetImporter GetAtPath(string path);

    /// <summary>
    ///   <para>Save asset importer settings if asset importer is dirty.</para>
    /// </summary>
    public void SaveAndReimport()
    {
      AssetDatabase.ImportAsset(this.assetPath);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int LocalFileIDToClassID(long fileId);

    public void AddRemap(AssetImporter.SourceAssetIdentifier identifier, UnityEngine.Object externalObject)
    {
      this.AddRemap_Injected(ref identifier, externalObject);
    }

    public bool RemoveRemap(AssetImporter.SourceAssetIdentifier identifier)
    {
      return this.RemoveRemap_Injected(ref identifier);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AssetImporter.SourceAssetIdentifier[] GetIdentifiers(AssetImporter self);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityEngine.Object[] GetExternalObjects(AssetImporter self);

    /// <summary>
    ///   <para>Gets a copy of the external object map used by the AssetImporter.</para>
    /// </summary>
    /// <returns>
    ///   <para>The map between a sub-asset and an external Asset.</para>
    /// </returns>
    public Dictionary<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> GetExternalObjectMap()
    {
      AssetImporter.SourceAssetIdentifier[] identifiers = AssetImporter.GetIdentifiers(this);
      UnityEngine.Object[] externalObjects = AssetImporter.GetExternalObjects(this);
      Dictionary<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> dictionary = new Dictionary<AssetImporter.SourceAssetIdentifier, UnityEngine.Object>();
      for (int index = 0; index < identifiers.Length; ++index)
        dictionary.Add(identifiers[index], externalObjects[index]);
      return dictionary;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RegisterImporter(System.Type importer, int importerVersion, int queuePos, string fileExt);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void AddRemap_Injected(ref AssetImporter.SourceAssetIdentifier identifier, UnityEngine.Object externalObject);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool RemoveRemap_Injected(ref AssetImporter.SourceAssetIdentifier identifier);

    /// <summary>
    ///   <para>Represents a unique identifier for a sub-asset embedded in an imported Asset (such as an FBX file).</para>
    /// </summary>
    [NativeType(CodegenOptions.Custom, "MonoSourceAssetIdentifier")]
    public struct SourceAssetIdentifier
    {
      /// <summary>
      ///   <para>The type of the Asset.</para>
      /// </summary>
      public System.Type type;
      /// <summary>
      ///   <para>The name of the Asset.</para>
      /// </summary>
      public string name;

      /// <summary>
      ///   <para>Constructs a SourceAssetIdentifier.</para>
      /// </summary>
      /// <param name="asset">The the sub-asset embedded in the imported Asset.</param>
      /// <param name="type">The type of the sub-asset embedded in the imported Asset.</param>
      /// <param name="name">The name of the sub-asset embedded in the imported Asset.</param>
      public SourceAssetIdentifier(UnityEngine.Object asset)
      {
        if (asset == (UnityEngine.Object) null)
          throw new ArgumentNullException(nameof (asset));
        this.type = asset.GetType();
        this.name = asset.name;
      }

      /// <summary>
      ///   <para>Constructs a SourceAssetIdentifier.</para>
      /// </summary>
      /// <param name="asset">The the sub-asset embedded in the imported Asset.</param>
      /// <param name="type">The type of the sub-asset embedded in the imported Asset.</param>
      /// <param name="name">The name of the sub-asset embedded in the imported Asset.</param>
      public SourceAssetIdentifier(System.Type type, string name)
      {
        if (type == null)
          throw new ArgumentNullException(nameof (type));
        if (name == null)
          throw new ArgumentNullException(nameof (name));
        if (string.IsNullOrEmpty(name))
          throw new ArgumentException("The name is empty", nameof (name));
        this.type = type;
        this.name = name;
      }
    }
  }
}
