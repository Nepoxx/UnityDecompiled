// Decompiled with JetBrains decompiler
// Type: UnityEngine.WSA.Launcher
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.WSA
{
  /// <summary>
  ///   <para>Class which is capable of launching user's default app for file type or a protocol. See also PlayerSettings where you can specify file or URI associations.</para>
  /// </summary>
  public sealed class Launcher
  {
    /// <summary>
    ///   <para>Launches the default app associated with specified file.</para>
    /// </summary>
    /// <param name="folder">Folder type where the file is located.</param>
    /// <param name="relativeFilePath">Relative file path inside the specified folder.</param>
    /// <param name="showWarning">Shows user a warning that application will be switched.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LaunchFile(Folder folder, string relativeFilePath, bool showWarning);

    /// <summary>
    ///   <para>Opens a dialog for picking the file.</para>
    /// </summary>
    /// <param name="fileExtension">File extension.</param>
    public static void LaunchFileWithPicker(string fileExtension)
    {
      Process.Start("explorer.exe");
    }

    /// <summary>
    ///   <para>Starts the default app associated with the URI scheme name for the specified URI, using the specified options.</para>
    /// </summary>
    /// <param name="uri">The URI.</param>
    /// <param name="showWarning">Displays a warning that the URI is potentially unsafe.</param>
    public static void LaunchUri(string uri, bool showWarning)
    {
      Process.Start(uri);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InternalLaunchFileWithPicker(string fileExtension);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InternalLaunchUri(string uri, bool showWarning);
  }
}
