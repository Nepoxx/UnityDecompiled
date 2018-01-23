// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.DevDevice
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Hardware
{
  [RequiredByNativeCode]
  public struct DevDevice
  {
    public readonly string id;
    public readonly string name;
    public readonly string type;
    public readonly string module;
    public readonly DevDeviceState state;
    public readonly DevDeviceFeatures features;

    public DevDevice(string id, string name, string type, string module, DevDeviceState state, DevDeviceFeatures features)
    {
      this.id = id;
      this.name = name;
      this.type = type;
      this.module = module;
      this.state = state;
      this.features = features;
    }

    public bool isConnected
    {
      get
      {
        return this.state == DevDeviceState.Connected;
      }
    }

    public static DevDevice none
    {
      get
      {
        return new DevDevice("None", "None", nameof (none), "internal", DevDeviceState.Disconnected, DevDeviceFeatures.None);
      }
    }

    public override string ToString()
    {
      return this.name + " (id:" + this.id + ", type: " + this.type + ", module: " + this.module + ", state: " + (object) this.state + ", features: " + (object) this.features + ")";
    }
  }
}
