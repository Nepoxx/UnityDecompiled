// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSBackgroundMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Background modes supported by the application corresponding to project settings in Xcode.</para>
  /// </summary>
  [System.Flags]
  public enum iOSBackgroundMode : uint
  {
    None = 0,
    Audio = 1,
    Location = 2,
    VOIP = 4,
    NewsstandContent = 8,
    ExternalAccessory = 16, // 0x00000010
    BluetoothCentral = 32, // 0x00000020
    BluetoothPeripheral = 64, // 0x00000040
    Fetch = 128, // 0x00000080
    RemoteNotification = 256, // 0x00000100
  }
}
