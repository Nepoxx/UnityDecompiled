// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.Usb
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Hardware
{
  public sealed class Usb
  {
    public static event Usb.OnDevicesChangedHandler DevicesChanged;

    public static void OnDevicesChanged(UsbDevice[] devices)
    {
      // ISSUE: reference to a compiler-generated field
      if (Usb.DevicesChanged == null || devices == null)
        return;
      // ISSUE: reference to a compiler-generated field
      Usb.DevicesChanged(devices);
    }

    public delegate void OnDevicesChangedHandler(UsbDevice[] devices);
  }
}
