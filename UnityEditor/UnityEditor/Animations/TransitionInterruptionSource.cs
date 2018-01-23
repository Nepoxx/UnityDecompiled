// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.TransitionInterruptionSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Which AnimatorState transitions can interrupt the Transition.</para>
  /// </summary>
  public enum TransitionInterruptionSource
  {
    None,
    Source,
    Destination,
    SourceThenDestination,
    DestinationThenSource,
  }
}
