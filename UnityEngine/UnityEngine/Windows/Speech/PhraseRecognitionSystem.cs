// Decompiled with JetBrains decompiler
// Type: UnityEngine.Windows.Speech.PhraseRecognitionSystem
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
  /// <summary>
  ///   <para>Phrase recognition system is responsible for managing phrase recognizers and dispatching recognition events to them.</para>
  /// </summary>
  public static class PhraseRecognitionSystem
  {
    /// <summary>
    ///   <para>Returns whether speech recognition is supported on the machine that the application is running on.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern bool isSupported { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the current status of the phrase recognition system.</para>
    /// </summary>
    public static extern SpeechSystemStatus Status { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Attempts to restart the phrase recognition system.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Restart();

    /// <summary>
    ///   <para>Shuts phrase recognition system down.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Shutdown();

    public static event PhraseRecognitionSystem.ErrorDelegate OnError;

    public static event PhraseRecognitionSystem.StatusDelegate OnStatusChanged;

    [RequiredByNativeCode]
    private static void PhraseRecognitionSystem_InvokeErrorEvent(SpeechError errorCode)
    {
      // ISSUE: reference to a compiler-generated field
      PhraseRecognitionSystem.ErrorDelegate onError = PhraseRecognitionSystem.OnError;
      if (onError == null)
        return;
      onError(errorCode);
    }

    [RequiredByNativeCode]
    private static void PhraseRecognitionSystem_InvokeStatusChangedEvent(SpeechSystemStatus status)
    {
      // ISSUE: reference to a compiler-generated field
      PhraseRecognitionSystem.StatusDelegate onStatusChanged = PhraseRecognitionSystem.OnStatusChanged;
      if (onStatusChanged == null)
        return;
      onStatusChanged(status);
    }

    /// <summary>
    ///   <para>Delegate for OnError event.</para>
    /// </summary>
    /// <param name="errorCode">Error code for the error that occurred.</param>
    public delegate void ErrorDelegate(SpeechError errorCode);

    /// <summary>
    ///   <para>Delegate for OnStatusChanged event.</para>
    /// </summary>
    /// <param name="status">The new status of the phrase recognition system.</param>
    public delegate void StatusDelegate(SpeechSystemStatus status);
  }
}
