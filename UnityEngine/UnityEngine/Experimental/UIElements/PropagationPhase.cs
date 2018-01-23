// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.PropagationPhase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>The propagation phases of an event.</para>
  /// </summary>
  public enum PropagationPhase
  {
    None,
    Capture,
    AtTarget,
    BubbleUp,
    DefaultAction,
  }
}
