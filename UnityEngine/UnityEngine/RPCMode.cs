// Decompiled with JetBrains decompiler
// Type: UnityEngine.RPCMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Option for who will receive an RPC, used by NetworkView.RPC.</para>
  /// </summary>
  public enum RPCMode
  {
    Server = 0,
    Others = 1,
    All = 2,
    OthersBuffered = 5,
    AllBuffered = 6,
  }
}
