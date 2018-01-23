// Decompiled with JetBrains decompiler
// Type: UnityEditor.IDeviceUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Modules;

namespace UnityEditor
{
  internal static class IDeviceUtils
  {
    internal static RemoteAddress StartRemoteSupport(string deviceId)
    {
      return ModuleManager.GetDevice(deviceId).StartRemoteSupport();
    }

    internal static void StopRemoteSupport(string deviceId)
    {
      ModuleManager.GetDevice(deviceId).StopRemoteSupport();
    }

    internal static RemoteAddress StartPlayerConnectionSupport(string deviceId)
    {
      return ModuleManager.GetDevice(deviceId).StartPlayerConnectionSupport();
    }

    internal static void StopPlayerConnectionSupport(string deviceId)
    {
      ModuleManager.GetDevice(deviceId).StopPlayerConnectionSupport();
    }
  }
}
