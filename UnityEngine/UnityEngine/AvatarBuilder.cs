// Decompiled with JetBrains decompiler
// Type: UnityEngine.AvatarBuilder
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class to build avatars from user scripts.</para>
  /// </summary>
  public sealed class AvatarBuilder
  {
    /// <summary>
    ///   <para>Create a humanoid avatar.</para>
    /// </summary>
    /// <param name="go">Root object of your transform hierachy. It must be the top most gameobject when you create the avatar.</param>
    /// <param name="humanDescription">Humanoid description of the avatar.</param>
    /// <returns>
    ///   <para>Returns the Avatar, you must always always check the avatar is valid before using it with Avatar.isValid.</para>
    /// </returns>
    public static Avatar BuildHumanAvatar(GameObject go, HumanDescription humanDescription)
    {
      if ((Object) go == (Object) null)
        throw new NullReferenceException();
      return AvatarBuilder.BuildHumanAvatarMono(go, humanDescription);
    }

    private static Avatar BuildHumanAvatarMono(GameObject go, HumanDescription monoHumanDescription)
    {
      return AvatarBuilder.INTERNAL_CALL_BuildHumanAvatarMono(go, ref monoHumanDescription);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Avatar INTERNAL_CALL_BuildHumanAvatarMono(GameObject go, ref HumanDescription monoHumanDescription);

    /// <summary>
    ///   <para>Create a new generic avatar.</para>
    /// </summary>
    /// <param name="go">Root object of your transform hierarchy.</param>
    /// <param name="rootMotionTransformName">Transform name of the root motion transform. If empty no root motion is defined and you must take care of avatar movement yourself.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Avatar BuildGenericAvatar(GameObject go, string rootMotionTransformName);
  }
}
