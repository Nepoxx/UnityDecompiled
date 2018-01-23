// Decompiled with JetBrains decompiler
// Type: UnityEditor.Help
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Helper class to access Unity documentation.</para>
  /// </summary>
  public sealed class Help
  {
    /// <summary>
    ///   <para>Is there a help page for this object?</para>
    /// </summary>
    /// <param name="obj"></param>
    public static bool HasHelpForObject(Object obj)
    {
      return Help.HasHelpForObject(obj, true);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasHelpForObject(Object obj, bool defaultToMonoBehaviour);

    internal static string GetNiceHelpNameForObject(Object obj)
    {
      return Help.GetNiceHelpNameForObject(obj, true);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetNiceHelpNameForObject(Object obj, bool defaultToMonoBehaviour);

    /// <summary>
    ///   <para>Get the URL for this object's documentation.</para>
    /// </summary>
    /// <param name="obj">The object to retrieve documentation for.</param>
    /// <returns>
    ///   <para>The documentation URL for the object. Note that this could use the http: or file: schemas.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetHelpURLForObject(Object obj);

    /// <summary>
    ///   <para>Show help page for this object.</para>
    /// </summary>
    /// <param name="obj"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ShowHelpForObject(Object obj);

    /// <summary>
    ///   <para>Show a help page.</para>
    /// </summary>
    /// <param name="page"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ShowHelpPage(string page);

    /// <summary>
    ///   <para>Open url in the default web browser.</para>
    /// </summary>
    /// <param name="url"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BrowseURL(string url);
  }
}
