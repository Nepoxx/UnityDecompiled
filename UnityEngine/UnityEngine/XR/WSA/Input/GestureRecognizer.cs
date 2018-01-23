// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.GestureRecognizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Manager class with API for recognizing user gestures.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  public sealed class GestureRecognizer : IDisposable
  {
    private IntPtr m_Recognizer;

    /// <summary>
    ///   <para>Create a GestureRecognizer.</para>
    /// </summary>
    public GestureRecognizer()
    {
      this.m_Recognizer = this.Internal_Create();
    }

    public event Action<HoldCanceledEventArgs> HoldCanceled;

    public event Action<HoldCompletedEventArgs> HoldCompleted;

    public event Action<HoldStartedEventArgs> HoldStarted;

    public event Action<TappedEventArgs> Tapped;

    public event Action<ManipulationCanceledEventArgs> ManipulationCanceled;

    public event Action<ManipulationCompletedEventArgs> ManipulationCompleted;

    public event Action<ManipulationStartedEventArgs> ManipulationStarted;

    public event Action<ManipulationUpdatedEventArgs> ManipulationUpdated;

    public event Action<NavigationCanceledEventArgs> NavigationCanceled;

    public event Action<NavigationCompletedEventArgs> NavigationCompleted;

    public event Action<NavigationStartedEventArgs> NavigationStarted;

    public event Action<NavigationUpdatedEventArgs> NavigationUpdated;

    public event Action<RecognitionEndedEventArgs> RecognitionEnded;

    public event Action<RecognitionStartedEventArgs> RecognitionStarted;

    public event Action<GestureErrorEventArgs> GestureError;

    ~GestureRecognizer()
    {
      if (!(this.m_Recognizer != IntPtr.Zero))
        return;
      GestureRecognizer.DestroyThreaded(this.m_Recognizer);
      this.m_Recognizer = IntPtr.Zero;
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///   <para>Disposes the resources used by gesture recognizer.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_Recognizer != IntPtr.Zero)
      {
        GestureRecognizer.Destroy(this.m_Recognizer);
        this.m_Recognizer = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///   <para>Set the recognizable gestures to the ones specified in newMaskValues and return the old settings.</para>
    /// </summary>
    /// <param name="newMaskValue">A mask indicating which gestures are now recognizable.</param>
    /// <returns>
    ///   <para>The previous value.</para>
    /// </returns>
    public GestureSettings SetRecognizableGestures(GestureSettings newMaskValue)
    {
      if (this.m_Recognizer != IntPtr.Zero)
        return (GestureSettings) this.Internal_SetRecognizableGestures(this.m_Recognizer, (int) newMaskValue);
      return GestureSettings.None;
    }

    /// <summary>
    ///   <para>Retrieve a mask of the currently enabled gestures.</para>
    /// </summary>
    /// <returns>
    ///   <para>A mask indicating which Gestures are currently recognizable.</para>
    /// </returns>
    public GestureSettings GetRecognizableGestures()
    {
      if (this.m_Recognizer != IntPtr.Zero)
        return (GestureSettings) this.Internal_GetRecognizableGestures(this.m_Recognizer);
      return GestureSettings.None;
    }

    /// <summary>
    ///   <para>Call to begin receiving gesture events on this recognizer.  No events will be received until this method is called.</para>
    /// </summary>
    public void StartCapturingGestures()
    {
      if (!(this.m_Recognizer != IntPtr.Zero))
        return;
      this.Internal_StartCapturingGestures(this.m_Recognizer);
    }

    /// <summary>
    ///   <para>Call to stop receiving gesture events on this recognizer.</para>
    /// </summary>
    public void StopCapturingGestures()
    {
      if (!(this.m_Recognizer != IntPtr.Zero))
        return;
      this.Internal_StopCapturingGestures(this.m_Recognizer);
    }

    /// <summary>
    ///   <para>Used to query if the GestureRecognizer is currently receiving Gesture events.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if the GestureRecognizer is receiving events or false otherwise.</para>
    /// </returns>
    public bool IsCapturingGestures()
    {
      if (this.m_Recognizer != IntPtr.Zero)
        return this.Internal_IsCapturingGestures(this.m_Recognizer);
      return false;
    }

    /// <summary>
    ///   <para>Cancels any pending gesture events.  Additionally this will call StopCapturingGestures.</para>
    /// </summary>
    public void CancelGestures()
    {
      if (!(this.m_Recognizer != IntPtr.Zero))
        return;
      this.Internal_CancelGestures(this.m_Recognizer);
    }

    [RequiredByNativeCode]
    private void InvokeHoldCanceled(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.HoldCanceledEventDelegate holdCanceledEvent = this.HoldCanceledEvent;
      if (holdCanceledEvent != null)
        holdCanceledEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<HoldCanceledEventArgs> holdCanceled = this.HoldCanceled;
      if (holdCanceled == null)
        return;
      HoldCanceledEventArgs canceledEventArgs;
      canceledEventArgs.m_Source = source;
      canceledEventArgs.m_SourcePose = sourcePose;
      canceledEventArgs.m_HeadPose = headPose;
      holdCanceled(canceledEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeHoldCompleted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.HoldCompletedEventDelegate holdCompletedEvent = this.HoldCompletedEvent;
      if (holdCompletedEvent != null)
        holdCompletedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<HoldCompletedEventArgs> holdCompleted = this.HoldCompleted;
      if (holdCompleted == null)
        return;
      HoldCompletedEventArgs completedEventArgs;
      completedEventArgs.m_Source = source;
      completedEventArgs.m_SourcePose = sourcePose;
      completedEventArgs.m_HeadPose = headPose;
      holdCompleted(completedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeHoldStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.HoldStartedEventDelegate holdStartedEvent = this.HoldStartedEvent;
      if (holdStartedEvent != null)
        holdStartedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<HoldStartedEventArgs> holdStarted = this.HoldStarted;
      if (holdStarted == null)
        return;
      HoldStartedEventArgs startedEventArgs;
      startedEventArgs.m_Source = source;
      startedEventArgs.m_SourcePose = sourcePose;
      startedEventArgs.m_HeadPose = headPose;
      holdStarted(startedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeTapped(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, int tapCount)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.TappedEventDelegate tappedEvent = this.TappedEvent;
      if (tappedEvent != null)
        tappedEvent(source.m_SourceKind, tapCount, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<TappedEventArgs> tapped = this.Tapped;
      if (tapped == null)
        return;
      TappedEventArgs tappedEventArgs;
      tappedEventArgs.m_Source = source;
      tappedEventArgs.m_SourcePose = sourcePose;
      tappedEventArgs.m_HeadPose = headPose;
      tappedEventArgs.m_TapCount = tapCount;
      tapped(tappedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeManipulationCanceled(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.ManipulationCanceledEventDelegate manipulationCanceledEvent = this.ManipulationCanceledEvent;
      if (manipulationCanceledEvent != null)
        manipulationCanceledEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<ManipulationCanceledEventArgs> manipulationCanceled = this.ManipulationCanceled;
      if (manipulationCanceled == null)
        return;
      ManipulationCanceledEventArgs canceledEventArgs;
      canceledEventArgs.m_Source = source;
      canceledEventArgs.m_SourcePose = sourcePose;
      canceledEventArgs.m_HeadPose = headPose;
      manipulationCanceled(canceledEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeManipulationCompleted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 cumulativeDelta)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.ManipulationCompletedEventDelegate manipulationCompletedEvent = this.ManipulationCompletedEvent;
      if (manipulationCompletedEvent != null)
        manipulationCompletedEvent(source.m_SourceKind, cumulativeDelta, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<ManipulationCompletedEventArgs> manipulationCompleted = this.ManipulationCompleted;
      if (manipulationCompleted == null)
        return;
      ManipulationCompletedEventArgs completedEventArgs;
      completedEventArgs.m_Source = source;
      completedEventArgs.m_SourcePose = sourcePose;
      completedEventArgs.m_HeadPose = headPose;
      completedEventArgs.m_CumulativeDelta = cumulativeDelta;
      manipulationCompleted(completedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeManipulationStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.ManipulationStartedEventDelegate manipulationStartedEvent = this.ManipulationStartedEvent;
      if (manipulationStartedEvent != null)
        manipulationStartedEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<ManipulationStartedEventArgs> manipulationStarted = this.ManipulationStarted;
      if (manipulationStarted == null)
        return;
      ManipulationStartedEventArgs startedEventArgs;
      startedEventArgs.m_Source = source;
      startedEventArgs.m_SourcePose = sourcePose;
      startedEventArgs.m_HeadPose = headPose;
      manipulationStarted(startedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeManipulationUpdated(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 cumulativeDelta)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.ManipulationUpdatedEventDelegate manipulationUpdatedEvent = this.ManipulationUpdatedEvent;
      if (manipulationUpdatedEvent != null)
        manipulationUpdatedEvent(source.m_SourceKind, cumulativeDelta, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<ManipulationUpdatedEventArgs> manipulationUpdated = this.ManipulationUpdated;
      if (manipulationUpdated == null)
        return;
      ManipulationUpdatedEventArgs updatedEventArgs;
      updatedEventArgs.m_Source = source;
      updatedEventArgs.m_SourcePose = sourcePose;
      updatedEventArgs.m_HeadPose = headPose;
      updatedEventArgs.m_CumulativeDelta = cumulativeDelta;
      manipulationUpdated(updatedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeNavigationCanceled(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.NavigationCanceledEventDelegate navigationCanceledEvent = this.NavigationCanceledEvent;
      if (navigationCanceledEvent != null)
        navigationCanceledEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<NavigationCanceledEventArgs> navigationCanceled = this.NavigationCanceled;
      if (navigationCanceled == null)
        return;
      NavigationCanceledEventArgs canceledEventArgs;
      canceledEventArgs.m_Source = source;
      canceledEventArgs.m_SourcePose = sourcePose;
      canceledEventArgs.m_HeadPose = headPose;
      navigationCanceled(canceledEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeNavigationCompleted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 normalizedOffset)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.NavigationCompletedEventDelegate navigationCompletedEvent = this.NavigationCompletedEvent;
      if (navigationCompletedEvent != null)
        navigationCompletedEvent(source.m_SourceKind, normalizedOffset, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<NavigationCompletedEventArgs> navigationCompleted = this.NavigationCompleted;
      if (navigationCompleted == null)
        return;
      NavigationCompletedEventArgs completedEventArgs;
      completedEventArgs.m_Source = source;
      completedEventArgs.m_SourcePose = sourcePose;
      completedEventArgs.m_HeadPose = headPose;
      completedEventArgs.m_NormalizedOffset = normalizedOffset;
      navigationCompleted(completedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeNavigationStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.NavigationStartedEventDelegate navigationStartedEvent = this.NavigationStartedEvent;
      if (navigationStartedEvent != null)
        navigationStartedEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<NavigationStartedEventArgs> navigationStarted = this.NavigationStarted;
      if (navigationStarted == null)
        return;
      NavigationStartedEventArgs startedEventArgs;
      startedEventArgs.m_Source = source;
      startedEventArgs.m_SourcePose = sourcePose;
      startedEventArgs.m_HeadPose = headPose;
      navigationStarted(startedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeNavigationUpdated(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 normalizedOffset)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.NavigationUpdatedEventDelegate navigationUpdatedEvent = this.NavigationUpdatedEvent;
      if (navigationUpdatedEvent != null)
        navigationUpdatedEvent(source.m_SourceKind, normalizedOffset, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<NavigationUpdatedEventArgs> navigationUpdated = this.NavigationUpdated;
      if (navigationUpdated == null)
        return;
      NavigationUpdatedEventArgs updatedEventArgs;
      updatedEventArgs.m_Source = source;
      updatedEventArgs.m_SourcePose = sourcePose;
      updatedEventArgs.m_HeadPose = headPose;
      updatedEventArgs.m_NormalizedOffset = normalizedOffset;
      navigationUpdated(updatedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeRecognitionEnded(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.RecognitionEndedEventDelegate recognitionEndedEvent = this.RecognitionEndedEvent;
      if (recognitionEndedEvent != null)
        recognitionEndedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<RecognitionEndedEventArgs> recognitionEnded = this.RecognitionEnded;
      if (recognitionEnded == null)
        return;
      RecognitionEndedEventArgs recognitionEndedEventArgs;
      recognitionEndedEventArgs.m_Source = source;
      recognitionEndedEventArgs.m_SourcePose = sourcePose;
      recognitionEndedEventArgs.m_HeadPose = headPose;
      recognitionEnded(recognitionEndedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeRecognitionStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.RecognitionStartedEventDelegate recognitionStartedEvent = this.RecognitionStartedEvent;
      if (recognitionStartedEvent != null)
        recognitionStartedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
      // ISSUE: reference to a compiler-generated field
      Action<RecognitionStartedEventArgs> recognitionStarted = this.RecognitionStarted;
      if (recognitionStarted == null)
        return;
      RecognitionStartedEventArgs startedEventArgs;
      startedEventArgs.m_Source = source;
      startedEventArgs.m_SourcePose = sourcePose;
      startedEventArgs.m_HeadPose = headPose;
      recognitionStarted(startedEventArgs);
    }

    [RequiredByNativeCode]
    private void InvokeErrorEvent(string error, int hresult)
    {
      // ISSUE: reference to a compiler-generated field
      GestureRecognizer.GestureErrorDelegate gestureErrorEvent = this.GestureErrorEvent;
      if (gestureErrorEvent != null)
        gestureErrorEvent(error, hresult);
      // ISSUE: reference to a compiler-generated field
      Action<GestureErrorEventArgs> gestureError = this.GestureError;
      if (gestureError == null)
        return;
      gestureError(new GestureErrorEventArgs(error, hresult));
    }

    [Obsolete("HoldCanceledEvent is deprecated, and will be removed in a future release. Use OnHoldCanceledEvent instead.", false)]
    public event GestureRecognizer.HoldCanceledEventDelegate HoldCanceledEvent;

    [Obsolete("HoldCompletedEvent is deprecated, and will be removed in a future release. Use OnHoldCompletedEvent instead.", false)]
    public event GestureRecognizer.HoldCompletedEventDelegate HoldCompletedEvent;

    [Obsolete("HoldStartedEvent is deprecated, and will be removed in a future release. Use OnHoldStartedEvent instead.", false)]
    public event GestureRecognizer.HoldStartedEventDelegate HoldStartedEvent;

    [Obsolete("TappedEvent is deprecated, and will be removed in a future release. Use OnTappedEvent instead.", false)]
    public event GestureRecognizer.TappedEventDelegate TappedEvent;

    [Obsolete("ManipulationCanceledEvent is deprecated, and will be removed in a future release. Use OnManipulationCanceledEvent instead.", false)]
    public event GestureRecognizer.ManipulationCanceledEventDelegate ManipulationCanceledEvent;

    [Obsolete("ManipulationCompletedEvent is deprecated, and will be removed in a future release. Use OnManipulationCompletedEvent instead.", false)]
    public event GestureRecognizer.ManipulationCompletedEventDelegate ManipulationCompletedEvent;

    [Obsolete("ManipulationStartedEvent is deprecated, and will be removed in a future release. Use OnManipulationStartedEvent instead.", false)]
    public event GestureRecognizer.ManipulationStartedEventDelegate ManipulationStartedEvent;

    [Obsolete("ManipulationUpdatedEvent is deprecated, and will be removed in a future release. Use OnManipulationUpdatedEvent instead.", false)]
    public event GestureRecognizer.ManipulationUpdatedEventDelegate ManipulationUpdatedEvent;

    [Obsolete("NavigationCanceledEvent is deprecated, and will be removed in a future release. Use OnNavigationCanceledEvent instead.", false)]
    public event GestureRecognizer.NavigationCanceledEventDelegate NavigationCanceledEvent;

    [Obsolete("NavigationCompletedEvent is deprecated, and will be removed in a future release. Use OnNavigationCompletedEvent instead.", false)]
    public event GestureRecognizer.NavigationCompletedEventDelegate NavigationCompletedEvent;

    [Obsolete("NavigationStartedEvent is deprecated, and will be removed in a future release. Use OnNavigationStartedEvent instead.", false)]
    public event GestureRecognizer.NavigationStartedEventDelegate NavigationStartedEvent;

    [Obsolete("NavigationUpdatedEvent is deprecated, and will be removed in a future release. Use OnNavigationUpdatedEvent instead.", false)]
    public event GestureRecognizer.NavigationUpdatedEventDelegate NavigationUpdatedEvent;

    [Obsolete("RecognitionEndedEvent is deprecated, and will be removed in a future release. Use OnRecognitionEndedEvent instead.", false)]
    public event GestureRecognizer.RecognitionEndedEventDelegate RecognitionEndedEvent;

    [Obsolete("RecognitionStartedEvent is deprecated, and will be removed in a future release. Use OnRecognitionStartedEvent instead.", false)]
    public event GestureRecognizer.RecognitionStartedEventDelegate RecognitionStartedEvent;

    [Obsolete("GestureErrorEvent is deprecated, and will be removed in a future release. Use OnGestureErrorEvent instead.", false)]
    public event GestureRecognizer.GestureErrorDelegate GestureErrorEvent;

    private IntPtr Internal_Create()
    {
      IntPtr num;
      GestureRecognizer.INTERNAL_CALL_Internal_Create(this, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_Create(GestureRecognizer self, out IntPtr value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Destroy(IntPtr recognizer);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DestroyThreaded(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_StartCapturingGestures(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_StopCapturingGestures(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_IsCapturingGestures(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int Internal_SetRecognizableGestures(IntPtr recognizer, int newMaskValue);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int Internal_GetRecognizableGestures(IntPtr recognizer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_CancelGestures(IntPtr recognizer);

    /// <summary>
    ///   <para>Callback indicating a cancel event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void HoldCanceledEventDelegate(InteractionSourceKind source, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a hold completed event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void HoldCompletedEventDelegate(InteractionSourceKind source, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a hold started event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void HoldStartedEventDelegate(InteractionSourceKind source, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a tap event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="tapCount">The count of taps (1 for single tap, 2 for double tap).</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this event interaction began.</param>
    public delegate void TappedEventDelegate(InteractionSourceKind source, int tapCount, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a cancel event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="cumulativeDelta">Total distance moved since the beginning of the manipulation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void ManipulationCanceledEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a completed event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="cumulativeDelta">Total distance moved since the beginning of the manipulation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void ManipulationCompletedEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a started event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="cumulativeDelta">Total distance moved since the beginning of the manipulation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void ManipulationStartedEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a updated event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="cumulativeDelta">Total distance moved since the beginning of the manipulation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void ManipulationUpdatedEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a cancel event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="normalizedOffset">The last known normalized offset of the input within the unit cube for the navigation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void NavigationCanceledEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a completed event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="normalizedOffset">The last known normalized offset, since the navigation gesture began, of the input within the unit cube for the navigation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void NavigationCompletedEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a started event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="normalizedOffset">The normalized offset, since the navigation gesture began, of the input within the unit cube for the navigation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void NavigationStartedEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating a update event.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="normalizedOffset">The last known normalized offset, since the navigation gesture began, of the input within the unit cube for the navigation gesture.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time this gesture began.</param>
    public delegate void NavigationUpdatedEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating the gesture event has completed.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time a gesture began.</param>
    public delegate void RecognitionEndedEventDelegate(InteractionSourceKind source, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating the gesture event has started.</para>
    /// </summary>
    /// <param name="source">Indicates which input medium triggered this event.</param>
    /// <param name="headRay">Ray (with normalized direction) from user at the time a gesture began.</param>
    public delegate void RecognitionStartedEventDelegate(InteractionSourceKind source, Ray headRay);

    /// <summary>
    ///   <para>Callback indicating an error or warning occurred.</para>
    /// </summary>
    /// <param name="error">A readable error string (when possible).</param>
    /// <param name="hresult">The HRESULT code from the platform.</param>
    public delegate void GestureErrorDelegate([MarshalAs(UnmanagedType.LPStr)] string error, int hresult);
  }
}
