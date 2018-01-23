// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.VisibleLight
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
  /// <summary>
  ///   <para>Holds data of a visible light.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct VisibleLight
  {
    /// <summary>
    ///   <para>Light type.</para>
    /// </summary>
    public LightType lightType;
    /// <summary>
    ///   <para>Light color multiplied by intensity.</para>
    /// </summary>
    public Color finalColor;
    /// <summary>
    ///   <para>Light's influence rectangle on screen.</para>
    /// </summary>
    public Rect screenRect;
    /// <summary>
    ///   <para>Light transformation matrix.</para>
    /// </summary>
    public Matrix4x4 localToWorld;
    /// <summary>
    ///   <para>Light range.</para>
    /// </summary>
    public float range;
    /// <summary>
    ///   <para>Spot light angle.</para>
    /// </summary>
    public float spotAngle;
    private int instanceId;
    /// <summary>
    ///   <para>Light flags, see VisibleLightFlags.</para>
    /// </summary>
    public VisibleLightFlags flags;

    /// <summary>
    ///   <para>Accessor to Light component.</para>
    /// </summary>
    public Light light
    {
      get
      {
        return VisibleLight.GetLightObject(this.instanceId);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Light GetLightObject(int instanceId);
  }
}
