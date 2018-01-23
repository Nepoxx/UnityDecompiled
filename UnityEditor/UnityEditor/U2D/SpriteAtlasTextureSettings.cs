// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpriteAtlasTextureSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.U2D
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class SpriteAtlasTextureSettings
  {
    internal uint m_AnisoLevel;
    internal uint m_CompressionQuality;
    internal uint m_MaxTextureSize;
    internal TextureImporterCompression m_TextureCompression;
    internal UnityEngine.FilterMode m_FilterMode;
    internal int m_GenerateMipMaps;
    internal int m_Readable;
    internal int m_CrunchedCompression;
    internal int m_sRGB;

    public uint anisoLevel
    {
      get
      {
        return this.m_AnisoLevel;
      }
      set
      {
        this.m_AnisoLevel = value;
      }
    }

    public uint compressionQuality
    {
      get
      {
        return this.m_CompressionQuality;
      }
      set
      {
        this.m_CompressionQuality = value;
      }
    }

    public uint maxTextureSize
    {
      get
      {
        return this.m_MaxTextureSize;
      }
      set
      {
        this.m_MaxTextureSize = value;
      }
    }

    public TextureImporterCompression textureCompression
    {
      get
      {
        return this.m_TextureCompression;
      }
      set
      {
        this.m_TextureCompression = value;
      }
    }

    public UnityEngine.FilterMode filterMode
    {
      get
      {
        return this.m_FilterMode;
      }
      set
      {
        this.m_FilterMode = value;
      }
    }

    public bool generateMipMaps
    {
      get
      {
        return this.m_GenerateMipMaps != 0;
      }
      set
      {
        this.m_GenerateMipMaps = !value ? 0 : 1;
      }
    }

    public bool readable
    {
      get
      {
        return this.m_Readable != 0;
      }
      set
      {
        this.m_Readable = !value ? 0 : 1;
      }
    }

    public bool crunchedCompression
    {
      get
      {
        return this.m_CrunchedCompression != 0;
      }
      set
      {
        this.m_CrunchedCompression = !value ? 0 : 1;
      }
    }

    public bool sRGB
    {
      get
      {
        return this.m_sRGB != 0;
      }
      set
      {
        this.m_sRGB = !value ? 0 : 1;
      }
    }
  }
}
