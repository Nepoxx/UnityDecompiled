// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.AssetImporters.AssetImportContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.AssetImporters
{
  /// <summary>
  ///   <para>Defines the import context for scripted importers during an import event.</para>
  /// </summary>
  public class AssetImportContext
  {
    private List<ImportedObject> m_ImportedObjects = new List<ImportedObject>();

    internal AssetImportContext()
    {
    }

    /// <summary>
    ///   <para>The path of the source asset file to be imported.</para>
    /// </summary>
    public string assetPath { get; internal set; }

    /// <summary>
    ///   <para>This indicates what platform the import event is targetting.</para>
    /// </summary>
    public BuildTarget selectedBuildTarget { get; internal set; }

    internal List<ImportedObject> importedObjects
    {
      get
      {
        return this.m_ImportedObjects;
      }
    }

    /// <summary>
    ///   <para>Specifies which object in the asset file should become the main object of the import opperation.</para>
    /// </summary>
    /// <param name="obj">The Unity Object that is to be marked as the main object.</param>
    public void SetMainObject(UnityEngine.Object obj)
    {
      if (obj == (UnityEngine.Object) null)
        return;
      ImportedObject importedObject1 = this.m_ImportedObjects.FirstOrDefault<ImportedObject>((Func<ImportedObject, bool>) (x => x.mainAssetObject));
      if (importedObject1 != null)
      {
        if (importedObject1.obj == obj)
          return;
        Debug.LogWarning((object) string.Format("An object was already set as the main object: \"{0}\" conflicting on \"{1}\"", (object) this.assetPath, (object) importedObject1.localIdentifier));
        importedObject1.mainAssetObject = false;
      }
      ImportedObject importedObject2 = this.m_ImportedObjects.FirstOrDefault<ImportedObject>((Func<ImportedObject, bool>) (x => x.obj == obj));
      if (importedObject2 == null)
        throw new Exception("Before an object can be set as main, it must first be added using AddObjectToAsset.");
      importedObject2.mainAssetObject = true;
      this.m_ImportedObjects.Remove(importedObject2);
      this.m_ImportedObjects.Insert(0, importedObject2);
    }

    /// <summary>
    ///   <para>Adds an object to the result of the import operation.</para>
    /// </summary>
    /// <param name="identifier">A unique identifier associated to this object.</param>
    /// <param name="obj">The Unity Object to add to the asset.</param>
    /// <param name="thumbnail">An optional 2D texture to use as the tumbnail for this object.</param>
    public void AddObjectToAsset(string identifier, UnityEngine.Object obj)
    {
      this.AddObjectToAsset(identifier, obj, (Texture2D) null);
    }

    /// <summary>
    ///   <para>Adds an object to the result of the import operation.</para>
    /// </summary>
    /// <param name="identifier">A unique identifier associated to this object.</param>
    /// <param name="obj">The Unity Object to add to the asset.</param>
    /// <param name="thumbnail">An optional 2D texture to use as the tumbnail for this object.</param>
    public void AddObjectToAsset(string identifier, UnityEngine.Object obj, Texture2D thumbnail)
    {
      if (obj == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (obj), "Cannot add a null object : " + (identifier ?? "<null>"));
      this.m_ImportedObjects.Add(new ImportedObject()
      {
        mainAssetObject = false,
        localIdentifier = identifier,
        obj = obj,
        thumbnail = thumbnail
      });
    }
  }
}
