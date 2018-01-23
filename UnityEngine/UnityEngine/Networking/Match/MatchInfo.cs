// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.MatchInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>Details about a UNET MatchMaker match.</para>
  /// </summary>
  public class MatchInfo
  {
    public MatchInfo()
    {
    }

    internal MatchInfo(CreateMatchResponse matchResponse)
    {
      this.address = matchResponse.address;
      this.port = matchResponse.port;
      this.domain = matchResponse.domain;
      this.networkId = matchResponse.networkId;
      this.accessToken = new NetworkAccessToken(matchResponse.accessTokenString);
      this.nodeId = matchResponse.nodeId;
      this.usingRelay = matchResponse.usingRelay;
    }

    internal MatchInfo(JoinMatchResponse matchResponse)
    {
      this.address = matchResponse.address;
      this.port = matchResponse.port;
      this.domain = matchResponse.domain;
      this.networkId = matchResponse.networkId;
      this.accessToken = new NetworkAccessToken(matchResponse.accessTokenString);
      this.nodeId = matchResponse.nodeId;
      this.usingRelay = matchResponse.usingRelay;
    }

    /// <summary>
    ///   <para>IP address of the host of the match,.</para>
    /// </summary>
    public string address { get; private set; }

    /// <summary>
    ///   <para>Port of the host of the match.</para>
    /// </summary>
    public int port { get; private set; }

    /// <summary>
    ///   <para>The numeric domain for the match.</para>
    /// </summary>
    public int domain { get; private set; }

    /// <summary>
    ///   <para>The unique ID of this match.</para>
    /// </summary>
    public NetworkID networkId { get; private set; }

    /// <summary>
    ///   <para>The binary access token this client uses to authenticate its session for future commands.</para>
    /// </summary>
    public NetworkAccessToken accessToken { get; private set; }

    /// <summary>
    ///   <para>NodeID for this member client in the match.</para>
    /// </summary>
    public NodeID nodeId { get; private set; }

    /// <summary>
    ///   <para>This flag indicates whether or not the match is using a Relay server.</para>
    /// </summary>
    public bool usingRelay { get; private set; }

    public override string ToString()
    {
      return UnityString.Format("{0} @ {1}:{2} [{3},{4}]", (object) this.networkId, (object) this.address, (object) this.port, (object) this.nodeId, (object) this.usingRelay);
    }
  }
}
