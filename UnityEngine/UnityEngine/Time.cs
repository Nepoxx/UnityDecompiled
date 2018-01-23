// Decompiled with JetBrains decompiler
// Type: UnityEngine.Time
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The interface to get time information from Unity.</para>
  /// </summary>
  public sealed class Time
  {
    /// <summary>
    ///   <para>The time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game.</para>
    /// </summary>
    public static extern float time { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded.</para>
    /// </summary>
    public static extern float timeSinceLevelLoad { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The time in seconds it took to complete the last frame (Read Only).</para>
    /// </summary>
    public static extern float deltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The time the latest MonoBehaviour.FixedUpdate has started (Read Only). This is the time in seconds since the start of the game.</para>
    /// </summary>
    public static extern float fixedTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The timeScale-independant time for this frame (Read Only). This is the time in seconds since the start of the game.</para>
    /// </summary>
    public static extern float unscaledTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The TimeScale-independant time the latest MonoBehaviour.FixedUpdate has started (Read Only). This is the time in seconds since the start of the game.</para>
    /// </summary>
    public static extern float fixedUnscaledTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The timeScale-independent interval in seconds from the last frame to the current one (Read Only).</para>
    /// </summary>
    public static extern float unscaledDeltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The timeScale-independent interval in seconds from the last fixed frame to the current one (Read Only).</para>
    /// </summary>
    public static extern float fixedUnscaledDeltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour.FixedUpdate) are performed.</para>
    /// </summary>
    public static extern float fixedDeltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour.FixedUpdate).</para>
    /// </summary>
    public static extern float maximumDeltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A smoothed out Time.deltaTime (Read Only).</para>
    /// </summary>
    public static extern float smoothDeltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The maximum time a frame can spend on particle updates. If the frame takes longer than this, then updates are split into multiple smaller updates.</para>
    /// </summary>
    public static extern float maximumParticleDeltaTime { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The scale at which the time is passing. This can be used for slow motion effects.</para>
    /// </summary>
    public static extern float timeScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The total number of frames that have passed (Read Only).</para>
    /// </summary>
    public static extern int frameCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int renderedFrameCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The real time in seconds since the game started (Read Only).</para>
    /// </summary>
    public static extern float realtimeSinceStartup { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Slows game playback time to allow screenshots to be saved between frames.</para>
    /// </summary>
    public static extern int captureFramerate { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true if called inside a fixed time step callback (like MonoBehaviour's MonoBehaviour.FixedUpdate), otherwise returns false.</para>
    /// </summary>
    public static extern bool inFixedTimeStep { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
