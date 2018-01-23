// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.UsbDevice
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Hardware
{
  public struct UsbDevice
  {
    public readonly int vendorId;
    public readonly int productId;
    public readonly int revision;
    public readonly string udid;
    public readonly string name;

    public override string ToString()
    {
      return this.name + " (udid:" + this.udid + ", vid: " + this.vendorId.ToString("X4") + ", pid: " + this.productId.ToString("X4") + ", rev: " + this.revision.ToString("X4") + ")";
    }
  }
}
