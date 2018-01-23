// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using AOT;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Provides access to user input from hands, controllers, and system voice commands.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public sealed class InteractionManager
  {
    private static InteractionManager.InternalSourceEventHandler m_OnSourceEventHandler;

    static InteractionManager()
    {
      // ISSUE: reference to a compiler-generated field
      if (InteractionManager.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractionManager.\u003C\u003Ef__mg\u0024cache0 = new InteractionManager.InternalSourceEventHandler(InteractionManager.OnSourceEvent);
      }
      // ISSUE: reference to a compiler-generated field
      InteractionManager.m_OnSourceEventHandler = InteractionManager.\u003C\u003Ef__mg\u0024cache0;
      InteractionManager.Initialize(Marshal.GetFunctionPointerForDelegate((Delegate) InteractionManager.m_OnSourceEventHandler));
    }

    public static event Action<InteractionSourceDetectedEventArgs> InteractionSourceDetected;

    public static event Action<InteractionSourceLostEventArgs> InteractionSourceLost;

    public static event Action<InteractionSourcePressedEventArgs> InteractionSourcePressed;

    public static event Action<InteractionSourceReleasedEventArgs> InteractionSourceReleased;

    public static event Action<InteractionSourceUpdatedEventArgs> InteractionSourceUpdated;

    /// <summary>
    ///   <para>Allows retrieving the current source states without allocating an array. The number of retrieved source states will be returned, up to a maximum of the size of the array.</para>
    /// </summary>
    /// <param name="sourceStates">An array for storing InteractionSourceState snapshots.</param>
    /// <returns>
    ///   <para>The number of snapshots stored in the array, up to the size of the array.</para>
    /// </returns>
    public static int GetCurrentReading(InteractionSourceState[] sourceStates)
    {
      if (sourceStates == null)
        throw new ArgumentNullException(nameof (sourceStates));
      if (sourceStates.Length > 0)
        return InteractionManager.GetCurrentReading_Internal(sourceStates);
      return 0;
    }

    /// <summary>
    ///   <para>Get the current SourceState.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of InteractionSourceState snapshots.</para>
    /// </returns>
    public static InteractionSourceState[] GetCurrentReading()
    {
      InteractionSourceState[] sourceStates = new InteractionSourceState[InteractionManager.numSourceStates];
      if (sourceStates.Length > 0)
        InteractionManager.GetCurrentReading_Internal(sourceStates);
      return sourceStates;
    }

    [MonoPInvokeCallback(typeof (InteractionManager.InternalSourceEventHandler))]
    private static void OnSourceEvent(InteractionManager.EventType eventType, InteractionSourceState state, InteractionSourcePressType pressType)
    {
      switch (eventType)
      {
        case InteractionManager.EventType.SourceDetected:
          // ISSUE: reference to a compiler-generated field
          InteractionManager.SourceEventHandler sourceDetectedLegacy = InteractionManager.InteractionSourceDetectedLegacy;
          if (sourceDetectedLegacy != null)
            sourceDetectedLegacy(state);
          // ISSUE: reference to a compiler-generated field
          Action<InteractionSourceDetectedEventArgs> interactionSourceDetected = InteractionManager.InteractionSourceDetected;
          if (interactionSourceDetected == null)
            break;
          interactionSourceDetected(new InteractionSourceDetectedEventArgs(state));
          break;
        case InteractionManager.EventType.SourceLost:
          // ISSUE: reference to a compiler-generated field
          InteractionManager.SourceEventHandler sourceLostLegacy = InteractionManager.InteractionSourceLostLegacy;
          if (sourceLostLegacy != null)
            sourceLostLegacy(state);
          // ISSUE: reference to a compiler-generated field
          Action<InteractionSourceLostEventArgs> interactionSourceLost = InteractionManager.InteractionSourceLost;
          if (interactionSourceLost == null)
            break;
          interactionSourceLost(new InteractionSourceLostEventArgs(state));
          break;
        case InteractionManager.EventType.SourceUpdated:
          // ISSUE: reference to a compiler-generated field
          InteractionManager.SourceEventHandler sourceUpdatedLegacy = InteractionManager.InteractionSourceUpdatedLegacy;
          if (sourceUpdatedLegacy != null)
            sourceUpdatedLegacy(state);
          // ISSUE: reference to a compiler-generated field
          Action<InteractionSourceUpdatedEventArgs> interactionSourceUpdated = InteractionManager.InteractionSourceUpdated;
          if (interactionSourceUpdated == null)
            break;
          interactionSourceUpdated(new InteractionSourceUpdatedEventArgs(state));
          break;
        case InteractionManager.EventType.SourcePressed:
          // ISSUE: reference to a compiler-generated field
          InteractionManager.SourceEventHandler sourcePressedLegacy = InteractionManager.InteractionSourcePressedLegacy;
          if (sourcePressedLegacy != null)
            sourcePressedLegacy(state);
          // ISSUE: reference to a compiler-generated field
          Action<InteractionSourcePressedEventArgs> interactionSourcePressed = InteractionManager.InteractionSourcePressed;
          if (interactionSourcePressed == null)
            break;
          interactionSourcePressed(new InteractionSourcePressedEventArgs(state, pressType));
          break;
        case InteractionManager.EventType.SourceReleased:
          // ISSUE: reference to a compiler-generated field
          InteractionManager.SourceEventHandler sourceReleasedLegacy = InteractionManager.InteractionSourceReleasedLegacy;
          if (sourceReleasedLegacy != null)
            sourceReleasedLegacy(state);
          // ISSUE: reference to a compiler-generated field
          Action<InteractionSourceReleasedEventArgs> interactionSourceReleased = InteractionManager.InteractionSourceReleased;
          if (interactionSourceReleased == null)
            break;
          interactionSourceReleased(new InteractionSourceReleasedEventArgs(state, pressType));
          break;
        default:
          throw new ArgumentException("OnSourceEvent: Invalid EventType");
      }
    }

    [Obsolete("SourceDetected is deprecated, and will be removed in a future release. Use InteractionSourceDetected instead. (UnityUpgradable) -> InteractionSourceDetectedLegacy", true)]
    public static event InteractionManager.SourceEventHandler SourceDetected;

    [Obsolete("SourceLost is deprecated, and will be removed in a future release. Use InteractionSourceLost instead. (UnityUpgradable) -> InteractionSourceLostLegacy", true)]
    public static event InteractionManager.SourceEventHandler SourceLost;

    [Obsolete("SourcePressed is deprecated, and will be removed in a future release. Use InteractionSourcePressed instead. (UnityUpgradable) -> InteractionSourcePressedLegacy", true)]
    public static event InteractionManager.SourceEventHandler SourcePressed;

    [Obsolete("SourceReleased is deprecated, and will be removed in a future release. Use InteractionSourceReleased instead. (UnityUpgradable) -> InteractionSourceReleasedLegacy", true)]
    public static event InteractionManager.SourceEventHandler SourceReleased;

    [Obsolete("SourceUpdated is deprecated, and will be removed in a future release. Use InteractionSourceUpdated instead. (UnityUpgradable) -> InteractionSourceUpdatedLegacy", true)]
    public static event InteractionManager.SourceEventHandler SourceUpdated;

    [Obsolete("InteractionSourceDetectedLegacy is deprecated, and will be removed in a future release. Use InteractionSourceDetected instead.", false)]
    public static event InteractionManager.SourceEventHandler InteractionSourceDetectedLegacy;

    [Obsolete("InteractionSourceLostLegacy is deprecated, and will be removed in a future release. Use InteractionSourceLost instead.", false)]
    public static event InteractionManager.SourceEventHandler InteractionSourceLostLegacy;

    [Obsolete("InteractionSourcePressedLegacy has been deprecated, and will be removed in a future release. Use InteractionSourcePressed instead.", false)]
    public static event InteractionManager.SourceEventHandler InteractionSourcePressedLegacy;

    [Obsolete("InteractionSourceReleasedLegacy has been deprecated, and will be removed in a future release. Use InteractionSourceReleased instead.", false)]
    public static event InteractionManager.SourceEventHandler InteractionSourceReleasedLegacy;

    [Obsolete("InteractionSourceUpdatedLegacy has been deprecated, and will be removed in a future release. Use InteractionSourceUpdated instead.", false)]
    public static event InteractionManager.SourceEventHandler InteractionSourceUpdatedLegacy;

    /// <summary>
    ///   <para>(Read Only) The number of InteractionSourceState snapshots available for reading with InteractionManager.GetCurrentReading.</para>
    /// </summary>
    public static extern int numSourceStates { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetCurrentReading_Internal(InteractionSourceState[] sourceStates);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Initialize(IntPtr internalSourceEventHandler);

    private enum EventType
    {
      SourceDetected,
      SourceLost,
      SourceUpdated,
      SourcePressed,
      SourceReleased,
    }

    private delegate void InternalSourceEventHandler(InteractionManager.EventType eventType, InteractionSourceState state, InteractionSourcePressType pressType);

    /// <summary>
    ///   <para>Callback to handle InteractionManager events.</para>
    /// </summary>
    /// <param name="state"></param>
    public delegate void SourceEventHandler(InteractionSourceState state);
  }
}
