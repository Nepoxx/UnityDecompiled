// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.GameCenter.GcLeaderboard
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms.GameCenter
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class GcLeaderboard
  {
    private IntPtr m_InternalLeaderboard;
    private Leaderboard m_GenericLeaderboard;

    internal GcLeaderboard(Leaderboard board)
    {
      this.m_GenericLeaderboard = board;
    }

    ~GcLeaderboard()
    {
      this.Dispose();
    }

    internal bool Contains(Leaderboard board)
    {
      return this.m_GenericLeaderboard == board;
    }

    internal void SetScores(GcScoreData[] scoreDatas)
    {
      if (this.m_GenericLeaderboard == null)
        return;
      Score[] scoreArray = new Score[scoreDatas.Length];
      for (int index = 0; index < scoreDatas.Length; ++index)
        scoreArray[index] = scoreDatas[index].ToScore();
      this.m_GenericLeaderboard.SetScores((IScore[]) scoreArray);
    }

    internal void SetLocalScore(GcScoreData scoreData)
    {
      if (this.m_GenericLeaderboard == null)
        return;
      this.m_GenericLeaderboard.SetLocalUserScore((IScore) scoreData.ToScore());
    }

    internal void SetMaxRange(uint maxRange)
    {
      if (this.m_GenericLeaderboard == null)
        return;
      this.m_GenericLeaderboard.SetMaxRange(maxRange);
    }

    internal void SetTitle(string title)
    {
      if (this.m_GenericLeaderboard == null)
        return;
      this.m_GenericLeaderboard.SetTitle(title);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Internal_LoadScores(string category, int from, int count, string[] userIDs, int playerScope, int timeScope, object callback);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool Loading();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Dispose();
  }
}
