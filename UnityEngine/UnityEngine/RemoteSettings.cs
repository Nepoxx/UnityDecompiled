// Decompiled with JetBrains decompiler
// Type: UnityEngine.RemoteSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Accesses remote settings (common for all game instances).</para>
  /// </summary>
  public static class RemoteSettings
  {
    public static event RemoteSettings.UpdatedEventHandler Updated;

    [RequiredByNativeCode]
    public static void CallOnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      RemoteSettings.UpdatedEventHandler updated = RemoteSettings.Updated;
      if (updated == null)
        return;
      updated();
    }

    /// <summary>
    ///   <para>Forces the game to download the newest settings from the server and update its values.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ForceUpdate();

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static int GetInt(string key)
    {
      int defaultValue = 0;
      return RemoteSettings.GetInt(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static float GetFloat(string key)
    {
      float defaultValue = 0.0f;
      return RemoteSettings.GetFloat(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetString(string key, [DefaultValue("\"\"")] string defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static string GetString(string key)
    {
      string defaultValue = "";
      return RemoteSettings.GetString(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetBool(string key, [DefaultValue("false")] bool defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the remote settings if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static bool GetBool(string key)
    {
      bool defaultValue = false;
      return RemoteSettings.GetBool(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns true if key exists in the remote settings.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasKey(string key);

    /// <summary>
    ///   <para>Returns number of keys in remote settings.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCount();

    /// <summary>
    ///   <para>Returns all the keys in remote settings.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetKeys();

    /// <summary>
    ///   <para>This event occurs when a new RemoteSettings is fetched and successfully parsed from the server.</para>
    /// </summary>
    public delegate void UpdatedEventHandler();
  }
}
