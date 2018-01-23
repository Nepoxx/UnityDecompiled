// Decompiled with JetBrains decompiler
// Type: UnityEngine.Sprites.DataUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Sprites
{
  /// <summary>
  ///   <para>Helper utilities for accessing Sprite data.</para>
  /// </summary>
  public sealed class DataUtility
  {
    /// <summary>
    ///   <para>Inner UV's of the Sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector4 GetInnerUV(Sprite sprite)
    {
      Vector4 vector4;
      DataUtility.INTERNAL_CALL_GetInnerUV(sprite, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetInnerUV(Sprite sprite, out Vector4 value);

    /// <summary>
    ///   <para>Outer UV's of the Sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector4 GetOuterUV(Sprite sprite)
    {
      Vector4 vector4;
      DataUtility.INTERNAL_CALL_GetOuterUV(sprite, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetOuterUV(Sprite sprite, out Vector4 value);

    /// <summary>
    ///   <para>Return the padding on the sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector4 GetPadding(Sprite sprite)
    {
      Vector4 vector4;
      DataUtility.INTERNAL_CALL_GetPadding(sprite, out vector4);
      return vector4;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPadding(Sprite sprite, out Vector4 value);

    /// <summary>
    ///   <para>Minimum width and height of the Sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector2 GetMinSize(Sprite sprite)
    {
      Vector2 output;
      DataUtility.Internal_GetMinSize(sprite, out output);
      return output;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_GetMinSize(Sprite sprite, out Vector2 output);
  }
}
