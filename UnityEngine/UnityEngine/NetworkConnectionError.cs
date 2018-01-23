// Decompiled with JetBrains decompiler
// Type: UnityEngine.NetworkConnectionError
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Possible status messages returned by Network.Connect and in MonoBehaviour.OnFailedToConnect|OnFailedToConnect in case the error was not immediate.</para>
  /// </summary>
  public enum NetworkConnectionError
  {
    InternalDirectConnectFailed = -5,
    EmptyConnectTarget = -4,
    IncorrectParameters = -3,
    CreateSocketOrThreadFailure = -2,
    AlreadyConnectedToAnotherServer = -1,
    NoError = 0,
    ConnectionFailed = 15, // 0x0000000F
    AlreadyConnectedToServer = 16, // 0x00000010
    TooManyConnectedPlayers = 18, // 0x00000012
    RSAPublicKeyMismatch = 21, // 0x00000015
    ConnectionBanned = 22, // 0x00000016
    InvalidPassword = 23, // 0x00000017
    NATTargetNotConnected = 69, // 0x00000045
    NATTargetConnectionLost = 71, // 0x00000047
    NATPunchthroughFailed = 73, // 0x00000049
  }
}
