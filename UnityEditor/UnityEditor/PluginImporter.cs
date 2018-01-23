// Decompiled with JetBrains decompiler
// Type: UnityEditor.PluginImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Modules;
using UnityEditorInternal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Represents plugin importer.</para>
  /// </summary>
  public sealed class PluginImporter : AssetImporter
  {
    private static Dictionary<string, PluginImporter.IncludeInBuildDelegate> s_includeInBuildDelegateMap = new Dictionary<string, PluginImporter.IncludeInBuildDelegate>();
    private static Dictionary<string, Predicate<string>> s_shouldOverridePredicateMap = new Dictionary<string, Predicate<string>>();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool GetCompatibleWithPlatformOrAnyPlatformBuildTarget(string buildTarget);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool GetCompatibleWithPlatformOrAnyPlatformBuildGroupAndTarget(string buildTargetGroup, string buildTarget);

    /// <summary>
    ///   <para>Returns all plugin importers for specfied platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName">Name of the target platform.</param>
    public static PluginImporter[] GetImporters(string platformName)
    {
      List<PluginImporter> pluginImporterList = new List<PluginImporter>();
      Dictionary<string, PluginImporter> dictionary = new Dictionary<string, PluginImporter>();
      PluginImporter[] array = ((IEnumerable<PluginImporter>) PluginImporter.GetAllImporters()).Where<PluginImporter>((Func<PluginImporter, bool>) (imp => imp.GetCompatibleWithPlatformOrAnyPlatformBuildTarget(platformName))).ToArray<PluginImporter>();
      IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(platformName) ?? ModuleManager.GetPluginImporterExtension(BuildPipeline.GetBuildTargetByName(platformName));
      if (importerExtension == null)
        return array;
      for (int index = 0; index < array.Length; ++index)
      {
        PluginImporter imp = array[index];
        string finalPluginPath = importerExtension.CalculateFinalPluginPath(platformName, imp);
        if (!string.IsNullOrEmpty(finalPluginPath))
        {
          PluginImporter pluginImporter;
          if (!dictionary.TryGetValue(finalPluginPath, out pluginImporter))
            dictionary.Add(finalPluginPath, imp);
          else if (pluginImporter.GetIsOverridable() && !imp.GetIsOverridable())
          {
            dictionary[finalPluginPath] = imp;
            pluginImporterList.Remove(pluginImporter);
          }
          else if (imp.GetIsOverridable())
            continue;
        }
        pluginImporterList.Add(imp);
      }
      return pluginImporterList.ToArray();
    }

    /// <summary>
    ///   <para>Returns all plugin importers for specfied platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName">Name of the target platform.</param>
    public static PluginImporter[] GetImporters(BuildTarget platform)
    {
      return PluginImporter.GetImporters(BuildPipeline.GetBuildTargetName(platform));
    }

    public static PluginImporter[] GetImporters(string buildTargetGroup, string buildTarget)
    {
      return ((IEnumerable<PluginImporter>) PluginImporter.GetAllImporters()).Where<PluginImporter>((Func<PluginImporter, bool>) (imp => imp.GetCompatibleWithPlatformOrAnyPlatformBuildGroupAndTarget(buildTargetGroup, buildTarget))).ToArray<PluginImporter>();
    }

    public static PluginImporter[] GetImporters(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
    {
      return PluginImporter.GetImporters(BuildPipeline.GetBuildTargetGroupName(buildTargetGroup), BuildPipeline.GetBuildTargetName(buildTarget));
    }

    [DebuggerHidden]
    internal static IEnumerable<PluginDesc> GetExtensionPlugins(BuildTarget target)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PluginImporter.\u003CGetExtensionPlugins\u003Ec__Iterator0 pluginsCIterator0 = new PluginImporter.\u003CGetExtensionPlugins\u003Ec__Iterator0() { target = target };
      // ISSUE: reference to a compiler-generated field
      pluginsCIterator0.\u0024PC = -2;
      return (IEnumerable<PluginDesc>) pluginsCIterator0;
    }

    /// <summary>
    ///   <para>Clear all plugin settings and set the compatability with Any Platform to true.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearSettings();

    /// <summary>
    ///   <para>Set compatiblity with any platform.</para>
    /// </summary>
    /// <param name="enable">Is plugin compatible with any platform.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetCompatibleWithAnyPlatform(bool enable);

    /// <summary>
    ///   <para>Is plugin comptabile with any platform.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetCompatibleWithAnyPlatform();

    /// <summary>
    ///   <para>Exclude platform from compatible platforms when Any Platform is set to true.</para>
    /// </summary>
    /// <param name="platformName">Target platform.</param>
    /// <param name="excludedFromAny"></param>
    /// <param name="platform"></param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetExcludeFromAnyPlatform(string platformName, bool excludedFromAny);

    /// <summary>
    ///   <para>Is platform excluded when Any Platform set to true.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName"></param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetExcludeFromAnyPlatform(string platformName);

    public void SetIncludeInBuildDelegate(PluginImporter.IncludeInBuildDelegate includeInBuildDelegate)
    {
      PluginImporter.s_includeInBuildDelegateMap[this.assetPath] = includeInBuildDelegate;
    }

    [RequiredByNativeCode]
    private bool InvokeIncludeInBuildDelegate()
    {
      if (PluginImporter.s_includeInBuildDelegateMap.ContainsKey(this.assetPath))
        return PluginImporter.s_includeInBuildDelegateMap[this.assetPath](this.assetPath);
      return true;
    }

    internal void SetShouldOverridePredicate(Predicate<string> shouldOverridePredicate)
    {
      if (shouldOverridePredicate != null)
        PluginImporter.s_shouldOverridePredicateMap[this.assetPath] = shouldOverridePredicate;
      else if (PluginImporter.s_shouldOverridePredicateMap.ContainsKey(this.assetPath))
        PluginImporter.s_shouldOverridePredicateMap.Remove(this.assetPath);
    }

    [RequiredByNativeCode]
    private bool InvokeShouldOverridePredicate()
    {
      if (PluginImporter.s_shouldOverridePredicateMap.ContainsKey(this.assetPath))
      {
        try
        {
          return PluginImporter.s_shouldOverridePredicateMap[this.assetPath](this.assetPath);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogWarning((object) ("Exception occurred while invoking ShouldOverridePredicate for " + this.assetPath));
        }
      }
      return false;
    }

    /// <summary>
    ///   <para>Exclude platform from compatible platforms when Any Platform is set to true.</para>
    /// </summary>
    /// <param name="platformName">Target platform.</param>
    /// <param name="excludedFromAny"></param>
    /// <param name="platform"></param>
    public void SetExcludeFromAnyPlatform(BuildTarget platform, bool excludedFromAny)
    {
      this.SetExcludeFromAnyPlatform(BuildPipeline.GetBuildTargetName(platform), excludedFromAny);
    }

    /// <summary>
    ///   <para>Is platform excluded when Any Platform set to true.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName"></param>
    public bool GetExcludeFromAnyPlatform(BuildTarget platform)
    {
      return this.GetExcludeFromAnyPlatform(BuildPipeline.GetBuildTargetName(platform));
    }

    /// <summary>
    ///   <para>Exclude Editor from compatible platforms when Any Platform is set to true.</para>
    /// </summary>
    /// <param name="excludedFromAny"></param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetExcludeEditorFromAnyPlatform(bool excludedFromAny);

    /// <summary>
    ///   <para>Is Editor excluded when Any Platform is set to true.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetExcludeEditorFromAnyPlatform();

    /// <summary>
    ///   <para>Set compatiblity with any editor.</para>
    /// </summary>
    /// <param name="enable">Is plugin compatible with editor.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetCompatibleWithEditor(bool enable);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetCompatibleWithEditorWithBuildTargetsInternal(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, bool enable);

    internal void SetCompatibleWithEditor(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, bool enable)
    {
      this.SetCompatibleWithEditorWithBuildTargetsInternal(buildTargetGroup, buildTarget, enable);
    }

    /// <summary>
    ///   <para>Is plugin compatible with editor.</para>
    /// </summary>
    public bool GetCompatibleWithEditor()
    {
      return this.GetCompatibleWithEditor("", "");
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetCompatibleWithEditor(string buildTargetGroup, string buildTarget);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetIsPreloaded(bool isPreloaded);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool GetIsPreloaded();

    /// <summary>
    ///   <para>Identifies whether or not this plugin will be overridden if a plugin of the same name is placed in your project folder.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetIsOverridable();

    /// <summary>
    ///   <para>Identifies whether or not this plugin should be included in the current build target.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ShouldIncludeInBuild();

    /// <summary>
    ///   <para>Set compatiblity with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="enable">Is plugin compatible with specified platform.</param>
    /// <param name="platformName">Target platform.</param>
    public void SetCompatibleWithPlatform(BuildTarget platform, bool enable)
    {
      this.SetCompatibleWithPlatform(BuildPipeline.GetBuildTargetName(platform), enable);
    }

    internal void SetCompatibleWithPlatform(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, bool enable)
    {
      this.SetCompatibleWithPlatform(BuildPipeline.GetBuildTargetGroupName(buildTargetGroup), BuildPipeline.GetBuildTargetName(buildTarget), enable);
    }

    /// <summary>
    ///   <para>Is plugin compatible with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName"></param>
    public bool GetCompatibleWithPlatform(BuildTarget platform)
    {
      return this.GetCompatibleWithPlatform(BuildPipeline.GetBuildTargetName(platform));
    }

    internal bool GetCompatibleWithPlatform(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
    {
      return this.GetCompatibleWithPlatform(BuildPipeline.GetBuildTargetGroupName(buildTargetGroup), BuildPipeline.GetBuildTargetName(buildTarget));
    }

    /// <summary>
    ///   <para>Set compatiblity with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="enable">Is plugin compatible with specified platform.</param>
    /// <param name="platformName">Target platform.</param>
    public void SetCompatibleWithPlatform(string platformName, bool enable)
    {
      this.SetCompatibleWithPlatform(BuildPipeline.GetBuildTargetGroupName(BuildPipeline.GetBuildTargetByName(platformName)), platformName, enable);
    }

    /// <summary>
    ///   <para>Is plugin compatible with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName"></param>
    public bool GetCompatibleWithPlatform(string platformName)
    {
      return this.GetCompatibleWithPlatform(BuildPipeline.GetBuildTargetGroupName(BuildPipeline.GetBuildTargetByName(platformName)), platformName);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetCompatibleWithPlatform(string buildTargetGroup, string buildTarget, bool enable);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool GetCompatibleWithPlatform(string buildTargetGroup, string buildTarget);

    /// <summary>
    ///   <para>Set platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="value">Data.</param>
    /// <param name="platformName"></param>
    public void SetPlatformData(BuildTarget platform, string key, string value)
    {
      this.SetPlatformData(BuildPipeline.GetBuildTargetName(platform), key, value);
    }

    /// <summary>
    ///   <para>Get platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="platformName"></param>
    public string GetPlatformData(BuildTarget platform, string key)
    {
      return this.GetPlatformData(BuildPipeline.GetBuildTargetName(platform), key);
    }

    /// <summary>
    ///   <para>Set platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="value">Data.</param>
    /// <param name="platformName"></param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPlatformData(string platformName, string key, string value);

    /// <summary>
    ///   <para>Get platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="platformName"></param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetPlatformData(string platformName, string key);

    /// <summary>
    ///   <para>Set editor specific data.</para>
    /// </summary>
    /// <param name="key">Key value for data.</param>
    /// <param name="value">Data.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetEditorData(string key, string value);

    /// <summary>
    ///   <para>Returns editor specific data for specified key.</para>
    /// </summary>
    /// <param name="key">Key value for data.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetEditorData(string key);

    /// <summary>
    ///   <para>Is plugin native or managed? Note: C++ libraries with CLR support are treated as native plugins, because Unity cannot load such libraries. You can still access them via P/Invoke.</para>
    /// </summary>
    public extern bool isNativePlugin { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern DllType dllType { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns all plugin importers for all platforms.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern PluginImporter[] GetAllImporters();

    /// <summary>
    ///   <para>Delegate to be used with SetIncludeInBuildDelegate.</para>
    /// </summary>
    /// <param name="path"></param>
    public delegate bool IncludeInBuildDelegate(string path);
  }
}
