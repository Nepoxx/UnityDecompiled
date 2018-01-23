// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Component added to a camera to make it render 2D GUI elements.</para>
  /// </summary>
  [RequireComponent(typeof (Camera))]
  [Obsolete("This component is part of the legacy UI system and will be removed in a future release.")]
  public sealed class GUILayer : Behaviour
  {
    /// <summary>
    ///   <para>Get the GUI element at a specific screen position.</para>
    /// </summary>
    /// <param name="screenPosition"></param>
    public GUIElement HitTest(Vector3 screenPosition)
    {
      return GUILayer.INTERNAL_CALL_HitTest(this, ref screenPosition);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern GUIElement INTERNAL_CALL_HitTest(GUILayer self, ref Vector3 screenPosition);
  }
}
