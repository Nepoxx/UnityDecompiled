// Decompiled with JetBrains decompiler
// Type: UnityEditor.SessionState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>SessionState is a Key-Value Store intended for storing and retrieving Editor session state that should survive assembly reloading.</para>
  /// </summary>
  public static class SessionState
  {
    /// <summary>
    ///   <para>Store a Boolean value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBool(string key, bool value);

    /// <summary>
    ///   <para>Retrieve a Boolean value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetBool(string key, bool defaultValue);

    /// <summary>
    ///   <para>Erase a Boolean entry in the key-value store.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EraseBool(string key);

    /// <summary>
    ///   <para>Store a Float value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetFloat(string key, float value);

    /// <summary>
    ///   <para>Retrieve a Float value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetFloat(string key, float defaultValue);

    /// <summary>
    ///   <para>Erase a Float entry in the key-value store.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EraseFloat(string key);

    /// <summary>
    ///   <para>Store an Integer value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetInt(string key, int value);

    /// <summary>
    ///   <para>Retrieve an Integer value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetInt(string key, int defaultValue);

    /// <summary>
    ///   <para>Erase an Integer entry in the key-value store.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EraseInt(string key);

    /// <summary>
    ///   <para>Store a String value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetString(string key, string value);

    /// <summary>
    ///   <para>Retrieve a String value.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetString(string key, string defaultValue);

    /// <summary>
    ///   <para>Erase a String entry in the key-value store.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EraseString(string key);

    /// <summary>
    ///   <para>Store a Vector3.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetVector3(string key, Vector3 value)
    {
      SessionState.INTERNAL_CALL_SetVector3(key, ref value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetVector3(string key, ref Vector3 value);

    /// <summary>
    ///   <para>Retrieve a Vector3.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    public static Vector3 GetVector3(string key, Vector3 defaultValue)
    {
      Vector3 vector3;
      SessionState.INTERNAL_CALL_GetVector3(key, ref defaultValue, out vector3);
      return vector3;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetVector3(string key, ref Vector3 defaultValue, out Vector3 value);

    /// <summary>
    ///   <para>Erase a Vector3 entry in the key-value store.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EraseVector3(string key);

    /// <summary>
    ///   <para>Store an Integer array.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetIntArray(string key, int[] value);

    /// <summary>
    ///   <para>Retrieve an Integer array.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int[] GetIntArray(string key, int[] defaultValue);

    /// <summary>
    ///   <para>Erase an Integer array entry in the key-value store.</para>
    /// </summary>
    /// <param name="key"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EraseIntArray(string key);
  }
}
