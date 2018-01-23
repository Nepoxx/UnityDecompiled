// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderStateMapping
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.Rendering
{
  public struct RenderStateMapping
  {
    private int m_RenderTypeID;
    private RenderStateBlock m_StateBlock;

    public RenderStateMapping(string renderType, RenderStateBlock stateBlock)
    {
      this.m_RenderTypeID = Shader.TagToID(renderType);
      this.m_StateBlock = stateBlock;
    }

    public RenderStateMapping(RenderStateBlock stateBlock)
    {
      this = new RenderStateMapping((string) null, stateBlock);
    }

    public string renderType
    {
      get
      {
        return Shader.IDToTag(this.m_RenderTypeID);
      }
      set
      {
        this.m_RenderTypeID = Shader.TagToID(value);
      }
    }

    public RenderStateBlock stateBlock
    {
      get
      {
        return this.m_StateBlock;
      }
      set
      {
        this.m_StateBlock = value;
      }
    }
  }
}
