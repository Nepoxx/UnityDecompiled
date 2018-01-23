// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Animations.GameObjectRecorder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Experimental.Animations
{
  /// <summary>
  ///   <para>Records the changing properties of a GameObject as the Scene runs and saves the information into an AnimationClip.</para>
  /// </summary>
  [NativeType]
  public class GameObjectRecorder : UnityEngine.Object
  {
    /// <summary>
    ///   <para>Create a new GameObjectRecorder.</para>
    /// </summary>
    public GameObjectRecorder()
    {
      GameObjectRecorder.Internal_Create(this);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] GameObjectRecorder notSelf);

    /// <summary>
    ///   <para>The GameObject used for the recording.</para>
    /// </summary>
    public extern GameObject root { [MethodImpl(MethodImplOptions.InternalCall)] set; [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Binds a GameObject's property as defined by EditorCurveBinding.</para>
    /// </summary>
    /// <param name="binding">The binding definition.</param>
    public void Bind(EditorCurveBinding binding)
    {
      this.Bind_Injected(ref binding);
    }

    /// <summary>
    ///   <para>Adds bindings for all of target's properties, and also for all the target's children if recursive is true.</para>
    /// </summary>
    /// <param name="target">.root or any of its children.</param>
    /// <param name="recursive">Binds also all the target's children properties when set to true.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void BindAll(GameObject target, bool recursive);

    /// <summary>
    ///   <para>Adds bindings for all the properties of the first component of type T found in target, and also for all the target's children if recursive is true.</para>
    /// </summary>
    /// <param name="target">.root or any of its children.</param>
    /// <param name="recursive">Binds also the target's children transform properties when set to true.</param>
    /// <param name="componentType">Type of the component.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void BindComponent(GameObject target, System.Type componentType, bool recursive);

    public void BindComponent<T>(GameObject target, bool recursive) where T : Component
    {
      this.BindComponent(target, typeof (T), recursive);
    }

    /// <summary>
    ///   <para>Returns an array of all the bindings added to the recorder.</para>
    /// </summary>
    /// <returns>
    ///   <para>Array of bindings.</para>
    /// </returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern EditorCurveBinding[] GetBindings();

    /// <summary>
    ///   <para>Forwards the animation by dt seconds, then record the values of the added bindings.</para>
    /// </summary>
    /// <param name="dt">Delta time.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void TakeSnapshot(float dt);

    /// <summary>
    ///   <para>Returns the current time of the recording.</para>
    /// </summary>
    public extern float currentTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true when the recorder is recording.</para>
    /// </summary>
    public extern bool isRecording { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Saves the recorded animation into clip.</para>
    /// </summary>
    /// <param name="clip">Destination clip.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SaveToClip(AnimationClip clip);

    /// <summary>
    ///   <para>Reset the recording.</para>
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ResetRecording();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Bind_Injected(ref EditorCurveBinding binding);
  }
}
