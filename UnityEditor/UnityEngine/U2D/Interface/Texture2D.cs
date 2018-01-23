// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.Interface.Texture2D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine.U2D.Interface
{
  internal class Texture2D : ITexture2D
  {
    private UnityEngine.Texture2D m_Texture;

    public Texture2D(UnityEngine.Texture2D texture)
    {
      this.m_Texture = texture;
    }

    public override int width
    {
      get
      {
        return this.m_Texture.width;
      }
    }

    public override int height
    {
      get
      {
        return this.m_Texture.height;
      }
    }

    public override TextureFormat format
    {
      get
      {
        return this.m_Texture.format;
      }
    }

    public override Color32[] GetPixels32()
    {
      return this.m_Texture.GetPixels32();
    }

    public override FilterMode filterMode
    {
      get
      {
        return this.m_Texture.filterMode;
      }
      set
      {
        this.m_Texture.filterMode = value;
      }
    }

    public override float mipMapBias
    {
      get
      {
        return this.m_Texture.mipMapBias;
      }
    }

    public override string name
    {
      get
      {
        return this.m_Texture.name;
      }
    }

    public override bool Equals(object other)
    {
      Texture2D texture2D = other as Texture2D;
      if (object.ReferenceEquals((object) texture2D, (object) null))
        return (Object) this.m_Texture == (Object) null;
      return (Object) this.m_Texture == (Object) texture2D.m_Texture;
    }

    public override int GetHashCode()
    {
      return this.m_Texture.GetHashCode();
    }

    public override void SetPixels(Color[] c)
    {
      this.m_Texture.SetPixels(c);
    }

    public override void Apply()
    {
      this.m_Texture.Apply();
    }

    protected override Object ToUnityObject()
    {
      return (Object) this.m_Texture;
    }

    protected override UnityEngine.Texture2D ToUnityTexture()
    {
      return this.m_Texture;
    }
  }
}
