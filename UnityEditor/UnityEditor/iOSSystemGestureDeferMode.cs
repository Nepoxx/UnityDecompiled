// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSSystemGestureDeferMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Bit-mask used to control the deferring of system gestures on iOS.</para>
  /// </summary>
  [System.Flags]
  public enum iOSSystemGestureDeferMode : uint
  {
    None = 0,
    TopEdge = 1,
    LeftEdge = 2,
    BottomEdge = 4,
    RightEdge = 8,
    All = RightEdge | BottomEdge | LeftEdge | TopEdge, // 0x0000000F
  }
}
