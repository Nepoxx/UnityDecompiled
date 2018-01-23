// Decompiled with JetBrains decompiler
// Type: UnityEditor.Networking.PlayerConnection.ConnectedPlayer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Networking.PlayerConnection
{
  /// <summary>
  ///   <para>Information of the connected player.</para>
  /// </summary>
  [Serializable]
  public class ConnectedPlayer
  {
    [SerializeField]
    private int m_PlayerId;
    [SerializeField]
    private string m_PlayerName;

    public ConnectedPlayer()
    {
    }

    public ConnectedPlayer(int playerId)
    {
      this.m_PlayerId = playerId;
    }

    public ConnectedPlayer(int playerId, string name)
    {
      this.m_PlayerId = playerId;
      this.m_PlayerName = name;
    }

    [Obsolete("Use playerId instead (UnityUpgradable) -> playerId", true)]
    public int PlayerId
    {
      get
      {
        return this.m_PlayerId;
      }
    }

    /// <summary>
    ///   <para>The Id of the player connected.</para>
    /// </summary>
    public int playerId
    {
      get
      {
        return this.m_PlayerId;
      }
    }

    /// <summary>
    ///   <para>The name of the connected player.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_PlayerName;
      }
    }
  }
}
