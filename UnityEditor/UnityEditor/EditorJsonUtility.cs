// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorJsonUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Utility functions for working with JSON data and engine objects.</para>
  /// </summary>
  public static class EditorJsonUtility
  {
    /// <summary>
    ///   <para>Generate a JSON representation of an object.</para>
    /// </summary>
    /// <param name="obj">The object to convert to JSON form.</param>
    /// <param name="prettyPrint">If true, format the output for readability. If false, format the output for minimum size. Default is false.</param>
    /// <returns>
    ///   <para>The object's data in JSON format.</para>
    /// </returns>
    public static string ToJson(object obj)
    {
      return EditorJsonUtility.ToJson(obj, false);
    }

    /// <summary>
    ///   <para>Generate a JSON representation of an object.</para>
    /// </summary>
    /// <param name="obj">The object to convert to JSON form.</param>
    /// <param name="prettyPrint">If true, format the output for readability. If false, format the output for minimum size. Default is false.</param>
    /// <returns>
    ///   <para>The object's data in JSON format.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToJson(object obj, bool prettyPrint);

    /// <summary>
    ///   <para>Overwrite data in an object by reading from its JSON representation.</para>
    /// </summary>
    /// <param name="json">The JSON representation of the object.</param>
    /// <param name="objectToOverwrite">The object to overwrite.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FromJsonOverwrite(string json, object objectToOverwrite);
  }
}
