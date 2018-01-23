// Decompiled with JetBrains decompiler
// Type: UnityEngine.Playables.PlayableDirector
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
  /// <summary>
  ///   <para>Instantiates a PlayableAsset and controls playback of Playable objects.</para>
  /// </summary>
  [RequiredByNativeCode]
  public class PlayableDirector : Behaviour, IExposedPropertyTable
  {
    /// <summary>
    ///   <para>The current playing state of the component. (Read Only)</para>
    /// </summary>
    public PlayState state
    {
      get
      {
        return this.GetPlayState();
      }
    }

    /// <summary>
    ///   <para>Controls how the time is incremented when it goes beyond the duration of the playable.</para>
    /// </summary>
    public DirectorWrapMode extrapolationMode
    {
      set
      {
        this.SetWrapMode(value);
      }
      get
      {
        return this.GetWrapMode();
      }
    }

    /// <summary>
    ///   <para>The PlayableAsset that is used to instantiate a playable for playback.</para>
    /// </summary>
    public PlayableAsset playableAsset
    {
      get
      {
        return this.GetPlayableAssetInternal() as PlayableAsset;
      }
      set
      {
        this.SetPlayableAssetInternal((ScriptableObject) value);
      }
    }

    /// <summary>
    ///   <para>The PlayableGraph created by the PlayableDirector.</para>
    /// </summary>
    public PlayableGraph playableGraph
    {
      get
      {
        return this.GetGraphHandle();
      }
    }

    /// <summary>
    ///   <para>Whether the playable asset will start playing back as soon as the component awakes.</para>
    /// </summary>
    public bool playOnAwake
    {
      get
      {
        return this.GetPlayOnAwake();
      }
      set
      {
        this.SetPlayOnAwake(value);
      }
    }

    /// <summary>
    ///   <para>Tells the PlayableDirector to evaluate it's PlayableGraph on the next update.</para>
    /// </summary>
    public void DeferredEvaluate()
    {
      this.EvaluateNextFrame();
    }

    /// <summary>
    ///   <para>Instatiates a Playable using the provided PlayableAsset and starts playback.</para>
    /// </summary>
    /// <param name="asset">An asset to instantiate a playable from.</param>
    /// <param name="mode">What to do when the time passes the duration of the playable.</param>
    public void Play(PlayableAsset asset)
    {
      if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (asset));
      this.Play(asset, this.extrapolationMode);
    }

    /// <summary>
    ///   <para>Instatiates a Playable using the provided PlayableAsset and starts playback.</para>
    /// </summary>
    /// <param name="asset">An asset to instantiate a playable from.</param>
    /// <param name="mode">What to do when the time passes the duration of the playable.</param>
    public void Play(PlayableAsset asset, DirectorWrapMode mode)
    {
      if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (asset));
      this.playableAsset = asset;
      this.extrapolationMode = mode;
      this.Play();
    }

    /// <summary>
    ///   <para>Controls how time is incremented when playing back.</para>
    /// </summary>
    public extern DirectorUpdateMode timeUpdateMode { [MethodImpl(MethodImplOptions.InternalCall)] set; [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The component's current time. This value is incremented according to the PlayableDirector.timeUpdateMode when it is playing. You can also change this value manually.</para>
    /// </summary>
    public extern double time { [MethodImpl(MethodImplOptions.InternalCall)] set; [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The time at which the Playable should start when first played.</para>
    /// </summary>
    public extern double initialTime { [MethodImpl(MethodImplOptions.InternalCall)] set; [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The duration of the Playable in seconds.</para>
    /// </summary>
    public extern double duration { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Evaluates the currently playing Playable at  the current time.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Evaluate();

    /// <summary>
    ///   <para>Instatiates a Playable using the provided PlayableAsset and starts playback.</para>
    /// </summary>
    /// <param name="asset">An asset to instantiate a playable from.</param>
    /// <param name="mode">What to do when the time passes the duration of the playable.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Play();

    /// <summary>
    ///   <para>Stops playback of the current Playable and destroys the corresponding graph.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Stop();

    /// <summary>
    ///   <para>Pauses playback of the currently running playable.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Pause();

    /// <summary>
    ///   <para>Resume playing a paused playable.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Resume();

    /// <summary>
    ///   <para>Discards the existing PlayableGraph and creates a new instance.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RebuildGraph();

    /// <summary>
    ///   <para>Clears an exposed reference value.</para>
    /// </summary>
    /// <param name="id">Identifier of the ExposedReference.</param>
    public void ClearReferenceValue(PropertyName id)
    {
      this.ClearReferenceValue_Injected(ref id);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void ProcessPendingGraphChanges();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern PlayState GetPlayState();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetWrapMode(DirectorWrapMode mode);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern DirectorWrapMode GetWrapMode();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void EvaluateNextFrame();

    private PlayableGraph GetGraphHandle()
    {
      PlayableGraph ret;
      this.GetGraphHandle_Injected(out ret);
      return ret;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetPlayOnAwake(bool on);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool GetPlayOnAwake();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetPlayableAssetInternal(ScriptableObject asset);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern ScriptableObject GetPlayableAssetInternal();

    /// <summary>
    ///   <para>Sets an ExposedReference value.</para>
    /// </summary>
    /// <param name="id">Identifier of the ExposedReference.</param>
    /// <param name="value">The object to bind to set the reference value to.</param>
    public void SetReferenceValue(PropertyName id, UnityEngine.Object value)
    {
      PlayableDirector.INTERNAL_CALL_SetReferenceValue(this, ref id, value);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetReferenceValue(PlayableDirector self, ref PropertyName id, UnityEngine.Object value);

    public UnityEngine.Object GetReferenceValue(PropertyName id, out bool idValid)
    {
      return PlayableDirector.INTERNAL_CALL_GetReferenceValue(this, ref id, out idValid);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityEngine.Object INTERNAL_CALL_GetReferenceValue(PlayableDirector self, ref PropertyName id, out bool idValid);

    /// <summary>
    ///   <para>Sets the binding of a reference object from a PlayableBinding.</para>
    /// </summary>
    /// <param name="key">The source object in the PlayableBinding.</param>
    /// <param name="value">The object to bind to the key.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetGenericBinding(UnityEngine.Object key, UnityEngine.Object value);

    /// <summary>
    ///   <para>Returns a binding to a reference object.</para>
    /// </summary>
    /// <param name="key">The object that acts as a key.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern UnityEngine.Object GetGenericBinding(UnityEngine.Object key);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool HasGenericBinding(UnityEngine.Object key);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void ClearReferenceValue_Injected(ref PropertyName id);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetGraphHandle_Injected(out PlayableGraph ret);
  }
}
