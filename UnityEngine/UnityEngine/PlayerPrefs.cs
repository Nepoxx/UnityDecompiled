// Decompiled with JetBrains decompiler
// Type: UnityEngine.PlayerPrefs
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Stores and accesses player preferences between game sessions.</para>
  /// </summary>
  public sealed class PlayerPrefs
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TrySetInt(string key, int value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TrySetFloat(string key, float value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TrySetSetString(string key, string value);

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetInt(string key, int value)
    {
      if (!PlayerPrefs.TrySetInt(key, value))
        throw new PlayerPrefsException("Could not store preference value");
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static int GetInt(string key)
    {
      int defaultValue = 0;
      return PlayerPrefs.GetInt(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetFloat(string key, float value)
    {
      if (!PlayerPrefs.TrySetFloat(key, value))
        throw new PlayerPrefsException("Could not store preference value");
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static float GetFloat(string key)
    {
      float defaultValue = 0.0f;
      return PlayerPrefs.GetFloat(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetString(string key, string value)
    {
      if (!PlayerPrefs.TrySetSetString(key, value))
        throw new PlayerPrefsException("Could not store preference value");
    }

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
      return PlayerPrefs.GetString(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns true if key exists in the preferences.</para>
    /// </summary>
    /// <param name="key"></param>
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

    /// <summary>
    ///   <para>Writes all modified preferences to disk.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Save();
  }
}
