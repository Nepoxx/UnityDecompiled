// Decompiled with JetBrains decompiler
// Type: UnityEngine.CharacterController
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A CharacterController allows you to easily do movement constrained by collisions without having to deal with a rigidbody.</para>
  /// </summary>
  public sealed class CharacterController : Collider
  {
    /// <summary>
    ///   <para>Moves the character with speed.</para>
    /// </summary>
    /// <param name="speed"></param>
    public bool SimpleMove(Vector3 speed)
    {
      return CharacterController.INTERNAL_CALL_SimpleMove(this, ref speed);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SimpleMove(CharacterController self, ref Vector3 speed);

    /// <summary>
    ///   <para>A more complex move function taking absolute movement deltas.</para>
    /// </summary>
    /// <param name="motion"></param>
    public CollisionFlags Move(Vector3 motion)
    {
      return CharacterController.INTERNAL_CALL_Move(this, ref motion);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern CollisionFlags INTERNAL_CALL_Move(CharacterController self, ref Vector3 motion);

    /// <summary>
    ///   <para>Was the CharacterController touching the ground during the last move?</para>
    /// </summary>
    public extern bool isGrounded { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current relative velocity of the Character (see notes).</para>
    /// </summary>
    public Vector3 velocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_velocity(out vector3);
        return vector3;
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_velocity(out Vector3 value);

    /// <summary>
    ///   <para>What part of the capsule collided with the environment during the last CharacterController.Move call.</para>
    /// </summary>
    public extern CollisionFlags collisionFlags { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The radius of the character's capsule.</para>
    /// </summary>
    public extern float radius { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The height of the character's capsule.</para>
    /// </summary>
    public extern float height { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The center of the character's capsule relative to the transform's position.</para>
    /// </summary>
    public Vector3 center
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_center(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_center(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_center(out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_center(ref Vector3 value);

    /// <summary>
    ///   <para>The character controllers slope limit in degrees.</para>
    /// </summary>
    public extern float slopeLimit { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The character controllers step offset in meters.</para>
    /// </summary>
    public extern float stepOffset { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The character's collision skin width.</para>
    /// </summary>
    public extern float skinWidth { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets or sets the minimum move distance of the character controller.</para>
    /// </summary>
    public extern float minMoveDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines whether other rigidbodies or character controllers collide with this character controller (by default this is always enabled).</para>
    /// </summary>
    public extern bool detectCollisions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///        <para>Enables or disables overlap recovery.
    /// Enables or disables overlap recovery. Used to depenetrate character controllers from static objects when an overlap is detected.</para>
    ///      </summary>
    public extern bool enableOverlapRecovery { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
