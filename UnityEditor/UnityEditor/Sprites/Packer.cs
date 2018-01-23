// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.Packer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor.Sprites
{
  /// <summary>
  ///   <para>Sprite Packer helpers.</para>
  /// </summary>
  public sealed class Packer
  {
    /// <summary>
    ///   <para>Name of the default Sprite Packer policy.</para>
    /// </summary>
    public static string kDefaultPolicy = typeof (DefaultPackerPolicy).Name;
    private static string[] m_policies = (string[]) null;
    private static string m_selectedPolicy = (string) null;
    private static Dictionary<string, System.Type> m_policyTypeCache = (Dictionary<string, System.Type>) null;

    /// <summary>
    ///   <para>Array of Sprite atlas names found in the current atlas cache.</para>
    /// </summary>
    public static extern string[] atlasNames { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns all atlas textures generated for the specified atlas.</para>
    /// </summary>
    /// <param name="atlasName">Atlas name.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D[] GetTexturesForAtlas(string atlasName);

    /// <summary>
    ///   <para>Returns all alpha atlas textures generated for the specified atlas.</para>
    /// </summary>
    /// <param name="atlasName">Name of the atlas.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D[] GetAlphaTexturesForAtlas(string atlasName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RebuildAtlasCacheIfNeeded(BuildTarget target, [DefaultValue("false")] bool displayProgressBar, [DefaultValue("Execution.Normal")] Packer.Execution execution);

    [ExcludeFromDocs]
    public static void RebuildAtlasCacheIfNeeded(BuildTarget target, bool displayProgressBar)
    {
      Packer.Execution execution = Packer.Execution.Normal;
      Packer.RebuildAtlasCacheIfNeeded(target, displayProgressBar, execution);
    }

    [ExcludeFromDocs]
    public static void RebuildAtlasCacheIfNeeded(BuildTarget target)
    {
      Packer.Execution execution = Packer.Execution.Normal;
      bool displayProgressBar = false;
      Packer.RebuildAtlasCacheIfNeeded(target, displayProgressBar, execution);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetAtlasDataForSprite(Sprite sprite, out string atlasName, out Texture2D atlasTexture);

    /// <summary>
    ///   <para>Available Sprite Packer policies for this project.</para>
    /// </summary>
    public static string[] Policies
    {
      get
      {
        Packer.RegenerateList();
        return Packer.m_policies;
      }
    }

    private static void SetSelectedPolicy(string value)
    {
      Packer.m_selectedPolicy = value;
      PlayerSettings.spritePackerPolicy = Packer.m_selectedPolicy;
    }

    /// <summary>
    ///   <para>The active Sprite Packer policy for this project.</para>
    /// </summary>
    public static string SelectedPolicy
    {
      get
      {
        Packer.RegenerateList();
        return Packer.m_selectedPolicy;
      }
      set
      {
        Packer.RegenerateList();
        if (value == null)
          throw new ArgumentNullException();
        if (!((IEnumerable<string>) Packer.m_policies).Contains<string>(value))
          throw new ArgumentException("Specified policy {0} is not in the policy list.", value);
        Packer.SetSelectedPolicy(value);
      }
    }

    private static void RegenerateList()
    {
      if (Packer.m_policies != null)
        return;
      List<System.Type> source = new List<System.Type>();
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        try
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (typeof (IPackerPolicy).IsAssignableFrom(type) && type != typeof (IPackerPolicy))
              source.Add(type);
          }
        }
        catch (Exception ex)
        {
          Debug.Log((object) string.Format("SpritePacker failed to get types from {0}. Error: {1}", (object) assembly.FullName, (object) ex.Message));
        }
      }
      Packer.m_policies = source.Select<System.Type, string>((Func<System.Type, string>) (t => t.Name)).ToArray<string>();
      Packer.m_policyTypeCache = new Dictionary<string, System.Type>();
      foreach (System.Type type1 in source)
      {
        if (Packer.m_policyTypeCache.ContainsKey(type1.Name))
        {
          System.Type type2 = Packer.m_policyTypeCache[type1.Name];
          Debug.LogError((object) string.Format("Duplicate Sprite Packer policies found: {0} and {1}. Please rename one.", (object) type1.FullName, (object) type2.FullName));
        }
        else
          Packer.m_policyTypeCache[type1.Name] = type1;
      }
      Packer.m_selectedPolicy = !string.IsNullOrEmpty(PlayerSettings.spritePackerPolicy) ? PlayerSettings.spritePackerPolicy : Packer.kDefaultPolicy;
      if (((IEnumerable<string>) Packer.m_policies).Contains<string>(Packer.m_selectedPolicy))
        return;
      Packer.SetSelectedPolicy(Packer.kDefaultPolicy);
    }

    internal static string GetSelectedPolicyId()
    {
      Packer.RegenerateList();
      System.Type type = Packer.m_policyTypeCache[Packer.m_selectedPolicy];
      IPackerPolicy instance = Activator.CreateInstance(type) as IPackerPolicy;
      return string.Format("{0}::{1}", (object) type.AssemblyQualifiedName, (object) instance.GetVersion());
    }

    internal static bool AllowSequentialPacking()
    {
      Packer.RegenerateList();
      return (Activator.CreateInstance(Packer.m_policyTypeCache[Packer.m_selectedPolicy]) as IPackerPolicy).AllowSequentialPacking;
    }

    internal static void ExecuteSelectedPolicy(BuildTarget target, int[] textureImporterInstanceIDs)
    {
      Packer.RegenerateList();
      (Activator.CreateInstance(Packer.m_policyTypeCache[Packer.m_selectedPolicy]) as IPackerPolicy).OnGroupAtlases(target, new PackerJob(), textureImporterInstanceIDs);
    }

    internal static void SaveUnappliedTextureImporterSettings()
    {
      foreach (InspectorWindow allInspectorWindow in InspectorWindow.GetAllInspectorWindows())
      {
        foreach (Editor activeEditor in allInspectorWindow.tracker.activeEditors)
        {
          TextureImporterInspector importerInspector = activeEditor as TextureImporterInspector;
          if (!((UnityEngine.Object) importerInspector == (UnityEngine.Object) null) && importerInspector.HasModified() && EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + (importerInspector.target as TextureImporter).assetPath + "'", "Apply", "Revert"))
            importerInspector.ApplyAndImport();
        }
      }
    }

    /// <summary>
    ///   <para>Sprite Packer execution mode.</para>
    /// </summary>
    public enum Execution
    {
      Normal,
      ForceRegroup,
    }
  }
}
