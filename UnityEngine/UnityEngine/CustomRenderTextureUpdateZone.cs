// Decompiled with JetBrains decompiler
// Type: UnityEngine.CustomRenderTextureUpdateZone
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Structure describing an Update Zone.</para>
  /// </summary>
  [UsedByNativeCode]
  [Serializable]
  public struct CustomRenderTextureUpdateZone
  {
    /// <summary>
    ///   <para>Position of the center of the Update Zone within the Custom Render Texture.</para>
    /// </summary>
    public Vector3 updateZoneCenter;
    /// <summary>
    ///   <para>Size of the Update Zone.</para>
    /// </summary>
    public Vector3 updateZoneSize;
    /// <summary>
    ///   <para>Rotation of the Update Zone.</para>
    /// </summary>
    public float rotation;
    /// <summary>
    ///   <para>Shader Pass used to update the Custom Render Texture for this Update Zone.</para>
    /// </summary>
    public int passIndex;
    /// <summary>
    ///   <para>If true, and if the texture is double buffered, a request is made to swap the buffers before the next update. Otherwise, the buffers will not be swapped.</para>
    /// </summary>
    public bool needSwap;
  }
}
