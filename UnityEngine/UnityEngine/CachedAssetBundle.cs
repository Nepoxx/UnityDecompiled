// Decompiled with JetBrains decompiler
// Type: UnityEngine.CachedAssetBundle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct CachedAssetBundle
  {
    private string m_Name;
    private Hash128 m_Hash;

    public CachedAssetBundle(string name, Hash128 hash)
    {
      this.m_Name = name;
      this.m_Hash = hash;
    }

    public string name
    {
      get
      {
        return this.m_Name;
      }
      set
      {
        this.m_Name = value;
      }
    }

    public Hash128 hash
    {
      get
      {
        return this.m_Hash;
      }
      set
      {
        this.m_Hash = value;
      }
    }
  }
}
