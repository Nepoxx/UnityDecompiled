// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlagSet`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Serializable]
  internal struct FlagSet<T> where T : IConvertible
  {
    private ulong m_Flags;

    public FlagSet(T flags)
    {
      this.m_Flags = Convert.ToUInt64((object) flags);
    }

    public bool HasFlags(T flags)
    {
      return ((long) this.m_Flags & (long) Convert.ToUInt64((object) flags)) != 0L;
    }

    public void SetFlags(T flags, bool value)
    {
      if (value)
        this.m_Flags |= Convert.ToUInt64((object) flags);
      else
        this.m_Flags &= ~Convert.ToUInt64((object) flags);
    }

    public static implicit operator FlagSet<T>(T flags)
    {
      return new FlagSet<T>(flags);
    }
  }
}
