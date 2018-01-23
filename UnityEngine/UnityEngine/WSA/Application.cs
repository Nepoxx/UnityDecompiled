// Decompiled with JetBrains decompiler
// Type: UnityEngine.WSA.Application
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.WSA
{
  /// <summary>
  ///   <para>Provides essential methods related to Window Store application.</para>
  /// </summary>
  public sealed class Application
  {
    public static event WindowSizeChanged windowSizeChanged;

    public static event WindowActivated windowActivated;

    /// <summary>
    ///   <para>Arguments passed to application.</para>
    /// </summary>
    public static string arguments
    {
      get
      {
        return Application.GetAppArguments();
      }
    }

    /// <summary>
    ///   <para>Advertising ID.</para>
    /// </summary>
    public static string advertisingIdentifier
    {
      get
      {
        string advertisingIdentifier = Application.GetAdvertisingIdentifier();
        UnityEngine.Application.InvokeOnAdvertisingIdentifierCallback(advertisingIdentifier, true);
        return advertisingIdentifier;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetAdvertisingIdentifier();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetAppArguments();

    internal static void InvokeWindowSizeChangedEvent(int width, int height)
    {
      // ISSUE: reference to a compiler-generated field
      if (Application.windowSizeChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      Application.windowSizeChanged(width, height);
    }

    internal static void InvokeWindowActivatedEvent(WindowActivationState state)
    {
      // ISSUE: reference to a compiler-generated field
      if (Application.windowActivated == null)
        return;
      // ISSUE: reference to a compiler-generated field
      Application.windowActivated(state);
    }

    /// <summary>
    ///   <para>Executes callback item on application thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    public static void InvokeOnAppThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
    }

    /// <summary>
    ///   <para>Executes callback item on UI thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    public static void InvokeOnUIThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
    }

    /// <summary>
    ///   <para>[OBSOLETE] Tries to execute callback item on application thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    [Obsolete("TryInvokeOnAppThread is deprecated, use InvokeOnAppThread")]
    public static bool TryInvokeOnAppThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
      return true;
    }

    /// <summary>
    ///   <para>[OBSOLETE] Tries to execute callback item on UI thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    [Obsolete("TryInvokeOnUIThread is deprecated, use InvokeOnUIThread")]
    public static bool TryInvokeOnUIThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
      return true;
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalTryInvokeOnAppThread(AppCallbackItem item, bool waitUntilDone);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalTryInvokeOnUIThread(AppCallbackItem item, bool waitUntilDone);

    /// <summary>
    ///   <para>Returns true if you're running on application thread.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RunningOnAppThread();

    /// <summary>
    ///   <para>Returns true if you're running on UI thread.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RunningOnUIThread();
  }
}
