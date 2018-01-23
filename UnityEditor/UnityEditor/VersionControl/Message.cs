// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Message
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Messages from the version control system.</para>
  /// </summary>
  public sealed class Message
  {
    private IntPtr m_thisDummy;

    internal Message()
    {
    }

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~Message()
    {
      this.Dispose();
    }

    /// <summary>
    ///   <para>The severity of the message.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern Message.Severity severity { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The message text.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern string message { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Write the message to the console.</para>
    /// </summary>
    public void Show()
    {
      Message.Info(this.message);
    }

    private static void Info(string message)
    {
      Debug.Log((object) ("Version control:\n" + message));
    }

    /// <summary>
    ///   <para>Severity of a version control message.</para>
    /// </summary>
    public enum Severity
    {
      Data,
      Verbose,
      Info,
      Warning,
      Error,
    }
  }
}
