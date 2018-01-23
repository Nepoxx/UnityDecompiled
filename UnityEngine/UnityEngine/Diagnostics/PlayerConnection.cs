// Decompiled with JetBrains decompiler
// Type: UnityEngine.Diagnostics.PlayerConnection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Diagnostics
{
  public static class PlayerConnection
  {
    [Obsolete("Use UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.isConnected instead.")]
    public static bool connected
    {
      get
      {
        return UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.isConnected;
      }
    }

    [Obsolete("PlayerConnection.SendFile is no longer supported.", true)]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SendFile(string remoteFilePath, byte[] data);
  }
}
