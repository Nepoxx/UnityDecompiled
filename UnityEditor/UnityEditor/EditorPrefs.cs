// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorPrefs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores and accesses Unity editor preferences.</para>
  /// </summary>
  public sealed class EditorPrefs
  {
    /// <summary>
    ///   <para>Sets the value of the preference identified by key as an integer.</para>
    /// </summary>
    /// <param name="key">Name of key to write integer to.</param>
    /// <param name="value">Value of the integer to write into the storage.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetInt(string key, int value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key">Name of key to read integer from.</param>
    /// <param name="defaultValue">Integer value to return if the key is not in the storage.</param>
    /// <returns>
    ///   <para>The value stored in the preference file.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key">Name of key to read integer from.</param>
    /// <param name="defaultValue">Integer value to return if the key is not in the storage.</param>
    /// <returns>
    ///   <para>The value stored in the preference file.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int GetInt(string key)
    {
      int defaultValue = 0;
      return EditorPrefs.GetInt(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the float value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key">Name of key to write float into.</param>
    /// <param name="value">Float value to write into the storage.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetFloat(string key, float value);

    /// <summary>
    ///   <para>Returns the float value corresponding to key if it exists in the preference file.</para>
    /// </summary>
    /// <param name="key">Name of key to read float from.</param>
    /// <param name="defaultValue">Float value to return if the key is not in the storage.</param>
    /// <returns>
    ///   <para>The float value stored in the preference file or the defaultValue id the
    ///   requested float does not exist.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

    /// <summary>
    ///   <para>Returns the float value corresponding to key if it exists in the preference file.</para>
    /// </summary>
    /// <param name="key">Name of key to read float from.</param>
    /// <param name="defaultValue">Float value to return if the key is not in the storage.</param>
    /// <returns>
    ///   <para>The float value stored in the preference file or the defaultValue id the
    ///   requested float does not exist.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static float GetFloat(string key)
    {
      float defaultValue = 0.0f;
      return EditorPrefs.GetFloat(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key. Note that EditorPrefs does not support null strings and will store an empty string instead.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetString(string key, string value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetString(string key, [DefaultValue("\"\"")] string defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static string GetString(string key)
    {
      string defaultValue = "";
      return EditorPrefs.GetString(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBool(string key, bool value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetBool(string key, [DefaultValue("false")] bool defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static bool GetBool(string key)
    {
      bool defaultValue = false;
      return EditorPrefs.GetBool(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns true if key exists in the preferences file.</para>
    /// </summary>
    /// <param name="key">Name of key to check for.</param>
    /// <returns>
    ///   <para>The existence or not of the key.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasKey(string key);

    /// <summary>
    ///   <para>Removes key and its corresponding value from the preferences.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DeleteKey(string key);

    /// <summary>
    ///   <para>Removes all keys and values from the preferences. Use with caution.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DeleteAll();
  }
}
