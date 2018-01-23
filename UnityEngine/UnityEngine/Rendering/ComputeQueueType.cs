// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.ComputeQueueType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Describes the desired characteristics with respect to prioritisation and load balancing of the queue that a command buffer being submitted via Graphics.ExecuteCommandBufferAsync or [[ScriptableRenderContext.ExecuteCommandBufferAsync] should be sent to.</para>
  /// </summary>
  public enum ComputeQueueType
  {
    Default,
    Background,
    Urgent,
  }
}
