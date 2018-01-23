// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.AssetImporters.ScriptedImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.AssetImporters
{
  /// <summary>
  ///   <para>Abstract base class for custom Asset importers.</para>
  /// </summary>
  public abstract class ScriptedImporter : AssetImporter
  {
    [RequiredByNativeCode]
    private ScriptedImporter.ImportResult GenerateAssetData(ScriptedImporter.ImportRequest request)
    {
      AssetImportContext ctx = new AssetImportContext() { assetPath = request.m_AssetSourcePath, selectedBuildTarget = request.m_SelectedBuildTarget };
      this.OnImportAsset(ctx);
      return new ScriptedImporter.ImportResult() { m_Assets = ctx.importedObjects.Select<ImportedObject, UnityEngine.Object>((Func<ImportedObject, UnityEngine.Object>) (x => x.obj)).ToArray<UnityEngine.Object>(), m_AssetNames = ctx.importedObjects.Select<ImportedObject, string>((Func<ImportedObject, string>) (x => x.localIdentifier)).ToArray<string>(), m_Thumbnails = ctx.importedObjects.Select<ImportedObject, Texture2D>((Func<ImportedObject, Texture2D>) (x => x.thumbnail)).ToArray<Texture2D>() };
    }

    /// <summary>
    ///   <para>This method must by overriden by the derived class and is called by the Asset pipeline to import files.</para>
    /// </summary>
    /// <param name="ctx">This argument contains all the contextual information needed to process the import event and is also used by the custom importer to store the resulting Unity Asset.</param>
    public abstract void OnImportAsset(AssetImportContext ctx);

    [RequiredByNativeCode]
    internal static void RegisterScriptedImporters()
    {
      ArrayList classesWithAttribute = AttributeHelper.FindEditorClassesWithAttribute(typeof (ScriptedImporterAttribute));
      IEnumerator enumerator1 = classesWithAttribute.GetEnumerator();
      try
      {
        while (enumerator1.MoveNext())
        {
          object current1 = enumerator1.Current;
          System.Type importer = current1 as System.Type;
          ScriptedImporterAttribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) importer, typeof (ScriptedImporterAttribute)) as ScriptedImporterAttribute;
          SortedDictionary<string, bool> extensionsByImporter = ScriptedImporter.GetHandledExtensionsByImporter(customAttribute);
          IEnumerator enumerator2 = classesWithAttribute.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current2 = enumerator2.Current;
              if (current2 != current1)
              {
                foreach (KeyValuePair<string, bool> keyValuePair in ScriptedImporter.GetHandledExtensionsByImporter(Attribute.GetCustomAttribute((MemberInfo) (current2 as System.Type), typeof (ScriptedImporterAttribute)) as ScriptedImporterAttribute))
                {
                  if (extensionsByImporter.ContainsKey(keyValuePair.Key))
                  {
                    Debug.LogError((object) string.Format("Scripted importers {0} and {1} are targeting the {2} extension, rejecting both.", (object) importer.FullName, (object) (current2 as System.Type).FullName, (object) keyValuePair.Key));
                    extensionsByImporter.Remove(keyValuePair.Key);
                  }
                }
              }
            }
          }
          finally
          {
            IDisposable disposable;
            if ((disposable = enumerator2 as IDisposable) != null)
              disposable.Dispose();
          }
          foreach (KeyValuePair<string, bool> keyValuePair in extensionsByImporter)
            AssetImporter.RegisterImporter(importer, customAttribute.version, customAttribute.importQueuePriority, keyValuePair.Key);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator1 as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private static SortedDictionary<string, bool> GetHandledExtensionsByImporter(ScriptedImporterAttribute attribute)
    {
      SortedDictionary<string, bool> sortedDictionary = new SortedDictionary<string, bool>();
      if (attribute.fileExtensions != null)
      {
        foreach (string key in attribute.fileExtensions)
        {
          if (key.StartsWith("."))
            key = key.Substring(1);
          sortedDictionary.Add(key, true);
        }
      }
      return sortedDictionary;
    }

    private struct ImportRequest
    {
      public readonly string m_AssetSourcePath;
      public readonly BuildTarget m_SelectedBuildTarget;
    }

    private struct ImportResult
    {
      public UnityEngine.Object[] m_Assets;
      public string[] m_AssetNames;
      public Texture2D[] m_Thumbnails;
    }
  }
}
