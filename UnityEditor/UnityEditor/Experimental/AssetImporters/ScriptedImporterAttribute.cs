// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.AssetImporters.ScriptedImporterAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Experimental.AssetImporters
{
  /// <summary>
  ///   <para>Class attribute used to register a custom asset importer derived from ScriptedImporter with Unity's Asset import pipeline.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class ScriptedImporterAttribute : Attribute
  {
    /// <summary>
    ///         <para>Use the ScriptedImporter attribute to register a custom importer derived from ScriptedImporter with Unity's Asset import pipeline.
    /// 
    /// It is best practice to always increment a scripted importer's version number whenever the script is changed. This forces assets imported with lower version numbers to be re-imported.
    /// 
    /// If the Unity Editor setting "Auto-Update" is set to enabled, editing a script outside of the editor and saving it will trigger both a re-import of the script and all assets of the corresponding type.</para>
    ///       </summary>
    /// <param name="version">A number that is used by the import pipeline to detect new versions of the importer script. Changing this number will trigger a re-import of all assets matching the listed extensions.</param>
    /// <param name="exts">List of file extensions (without leading period character) that the scripted importer handles.</param>
    /// <param name="ext">Single file extension (without leading period character) that the scripted importer handles.</param>
    /// <param name="importQueueOffset">Gives control over ordering of asset import based on types. Positive values delay the processing of source asset files while negative values place them earlier in the import process.</param>
    public ScriptedImporterAttribute(int version, string[] exts)
    {
      this.Init(version, exts, 0);
    }

    /// <summary>
    ///         <para>Use the ScriptedImporter attribute to register a custom importer derived from ScriptedImporter with Unity's Asset import pipeline.
    /// 
    /// It is best practice to always increment a scripted importer's version number whenever the script is changed. This forces assets imported with lower version numbers to be re-imported.
    /// 
    /// If the Unity Editor setting "Auto-Update" is set to enabled, editing a script outside of the editor and saving it will trigger both a re-import of the script and all assets of the corresponding type.</para>
    ///       </summary>
    /// <param name="version">A number that is used by the import pipeline to detect new versions of the importer script. Changing this number will trigger a re-import of all assets matching the listed extensions.</param>
    /// <param name="exts">List of file extensions (without leading period character) that the scripted importer handles.</param>
    /// <param name="ext">Single file extension (without leading period character) that the scripted importer handles.</param>
    /// <param name="importQueueOffset">Gives control over ordering of asset import based on types. Positive values delay the processing of source asset files while negative values place them earlier in the import process.</param>
    public ScriptedImporterAttribute(int version, string ext)
    {
      this.Init(version, new string[1]{ ext }, 0);
    }

    /// <summary>
    ///         <para>Use the ScriptedImporter attribute to register a custom importer derived from ScriptedImporter with Unity's Asset import pipeline.
    /// 
    /// It is best practice to always increment a scripted importer's version number whenever the script is changed. This forces assets imported with lower version numbers to be re-imported.
    /// 
    /// If the Unity Editor setting "Auto-Update" is set to enabled, editing a script outside of the editor and saving it will trigger both a re-import of the script and all assets of the corresponding type.</para>
    ///       </summary>
    /// <param name="version">A number that is used by the import pipeline to detect new versions of the importer script. Changing this number will trigger a re-import of all assets matching the listed extensions.</param>
    /// <param name="exts">List of file extensions (without leading period character) that the scripted importer handles.</param>
    /// <param name="ext">Single file extension (without leading period character) that the scripted importer handles.</param>
    /// <param name="importQueueOffset">Gives control over ordering of asset import based on types. Positive values delay the processing of source asset files while negative values place them earlier in the import process.</param>
    public ScriptedImporterAttribute(int version, string[] exts, int importQueueOffset)
    {
      this.Init(version, exts, importQueueOffset);
    }

    /// <summary>
    ///         <para>Use the ScriptedImporter attribute to register a custom importer derived from ScriptedImporter with Unity's Asset import pipeline.
    /// 
    /// It is best practice to always increment a scripted importer's version number whenever the script is changed. This forces assets imported with lower version numbers to be re-imported.
    /// 
    /// If the Unity Editor setting "Auto-Update" is set to enabled, editing a script outside of the editor and saving it will trigger both a re-import of the script and all assets of the corresponding type.</para>
    ///       </summary>
    /// <param name="version">A number that is used by the import pipeline to detect new versions of the importer script. Changing this number will trigger a re-import of all assets matching the listed extensions.</param>
    /// <param name="exts">List of file extensions (without leading period character) that the scripted importer handles.</param>
    /// <param name="ext">Single file extension (without leading period character) that the scripted importer handles.</param>
    /// <param name="importQueueOffset">Gives control over ordering of asset import based on types. Positive values delay the processing of source asset files while negative values place them earlier in the import process.</param>
    public ScriptedImporterAttribute(int version, string ext, int importQueueOffset)
    {
      this.Init(version, new string[1]{ ext }, importQueueOffset);
    }

    /// <summary>
    ///   <para>Importer version number that is used by the import layer to detect new version of the importer and trigger re-imports when such events occur, to apply latest changes made to the scripted imrpoter.</para>
    /// </summary>
    public int version { get; private set; }

    /// <summary>
    ///   <para>Gives control over ordering of asset import based on types. Positive values delay the processing of source asset files while Negative values place them earlier in the import process.</para>
    /// </summary>
    public int importQueuePriority { get; private set; }

    /// <summary>
    ///   <para>List of file extensions, without leading period character, that the scripted importer handles.</para>
    /// </summary>
    public string[] fileExtensions { get; private set; }

    private void Init(int version, string[] exts, int importQueueOffset)
    {
      if (exts != null)
      {
        string[] strArray = exts;
        // ISSUE: reference to a compiler-generated field
        if (ScriptedImporterAttribute.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ScriptedImporterAttribute.\u003C\u003Ef__mg\u0024cache0 = new Func<string, bool>(string.IsNullOrEmpty);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, bool> fMgCache0 = ScriptedImporterAttribute.\u003C\u003Ef__mg\u0024cache0;
        if (!((IEnumerable<string>) strArray).Any<string>(fMgCache0))
        {
          this.version = version;
          this.importQueuePriority = importQueueOffset;
          this.fileExtensions = exts;
          return;
        }
      }
      throw new ArgumentException("Must provide valid, none null, file extension strings.");
    }
  }
}
