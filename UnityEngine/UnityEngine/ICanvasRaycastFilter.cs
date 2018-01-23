// Decompiled with JetBrains decompiler
// Type: UnityEngine.ICanvasRaycastFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public interface ICanvasRaycastFilter
  {
    /// <summary>
    ///   <para>Given a point and a camera is the raycast valid.</para>
    /// </summary>
    /// <param name="sp">Screen position.</param>
    /// <param name="eventCamera">Raycast camera.</param>
    /// <returns>
    ///   <para>Valid.</para>
    /// </returns>
    bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera);
  }
}
