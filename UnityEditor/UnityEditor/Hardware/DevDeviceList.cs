// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.DevDeviceList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor.Hardware
{
  public sealed class DevDeviceList
  {
    public static event DevDeviceList.OnChangedHandler Changed;

    public static void OnChanged()
    {
      // ISSUE: reference to a compiler-generated field
      if (DevDeviceList.Changed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      DevDeviceList.Changed();
    }

    public static bool FindDevice(string deviceId, out DevDevice device)
    {
      foreach (DevDevice device1 in DevDeviceList.GetDevices())
      {
        if (device1.id == deviceId)
        {
          device = device1;
          return true;
        }
      }
      device = new DevDevice();
      return false;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DevDevice[] GetDevices();

    internal static void Update(string target, DevDevice[] devices)
    {
      DevDeviceList.UpdateInternal(target, devices);
      DevDeviceList.OnChanged();
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateInternal(string target, DevDevice[] devices);

    public delegate void OnChangedHandler();
  }
}
