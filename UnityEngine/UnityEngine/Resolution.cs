// Decompiled with JetBrains decompiler
// Type: UnityEngine.Resolution
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  public struct Resolution
  {
    private int m_Width;
    private int m_Height;
    private int m_RefreshRate;

    public int width
    {
      get
      {
        return this.m_Width;
      }
      set
      {
        this.m_Width = value;
      }
    }

    public int height
    {
      get
      {
        return this.m_Height;
      }
      set
      {
        this.m_Height = value;
      }
    }

    public int refreshRate
    {
      get
      {
        return this.m_RefreshRate;
      }
      set
      {
        this.m_RefreshRate = value;
      }
    }

    public override string ToString()
    {
      return UnityString.Format("{0} x {1} @ {2}Hz", (object) this.m_Width, (object) this.m_Height, (object) this.m_RefreshRate);
    }
  }
}
