// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterPlatformSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores platform specifics settings of a TextureImporter.</para>
  /// </summary>
  [Serializable]
  public sealed class TextureImporterPlatformSettings
  {
    [SerializeField]
    private string m_Name = TextureImporterInspector.s_DefaultPlatformName;
    [SerializeField]
    private int m_Overridden = 0;
    [SerializeField]
    private int m_MaxTextureSize = 2048;
    [SerializeField]
    private int m_ResizeAlgorithm = 0;
    [SerializeField]
    private int m_TextureFormat = -1;
    [SerializeField]
    private int m_TextureCompression = 1;
    [SerializeField]
    private int m_CompressionQuality = 50;
    [SerializeField]
    private int m_CrunchedCompression = 0;
    [SerializeField]
    private int m_AllowsAlphaSplitting = 0;
    [SerializeField]
    private int m_AndroidETC2FallbackOverride = 0;

    /// <summary>
    ///   <para>Name of the build target.</para>
    /// </summary>
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

    /// <summary>
    ///   <para>Set to true in order to override the Default platform parameters by those provided in the TextureImporterPlatformSettings structure.</para>
    /// </summary>
    public bool overridden
    {
      get
      {
        return this.m_Overridden != 0;
      }
      set
      {
        this.m_Overridden = !value ? 0 : 1;
      }
    }

    /// <summary>
    ///   <para>Maximum texture size.</para>
    /// </summary>
    public int maxTextureSize
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

    /// <summary>
    ///   <para>For Texture to be scaled down choose resize algorithm. ( Applyed only when Texture dimension is bigger than Max Size ).</para>
    /// </summary>
    public TextureResizeAlgorithm resizeAlgorithm
    {
      get
      {
        return (TextureResizeAlgorithm) this.m_ResizeAlgorithm;
      }
      set
      {
        this.m_ResizeAlgorithm = (int) value;
      }
    }

    /// <summary>
    ///   <para>Format of imported texture.</para>
    /// </summary>
    public TextureImporterFormat format
    {
      get
      {
        return (TextureImporterFormat) this.m_TextureFormat;
      }
      set
      {
        this.m_TextureFormat = (int) value;
      }
    }

    /// <summary>
    ///   <para>Compression of imported texture.</para>
    /// </summary>
    public TextureImporterCompression textureCompression
    {
      get
      {
        return (TextureImporterCompression) this.m_TextureCompression;
      }
      set
      {
        this.m_TextureCompression = (int) value;
      }
    }

    /// <summary>
    ///   <para>Quality of texture compression in the range [0..100].</para>
    /// </summary>
    public int compressionQuality
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

    /// <summary>
    ///   <para>Use crunch compression when available.</para>
    /// </summary>
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

    /// <summary>
    ///   <para>Allows Alpha splitting on the imported texture when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    public bool allowsAlphaSplitting
    {
      get
      {
        return this.m_AllowsAlphaSplitting != 0;
      }
      set
      {
        this.m_AllowsAlphaSplitting = !value ? 0 : 1;
      }
    }

    /// <summary>
    ///   <para>Override for ETC2 decompression fallback on Android devices that don't support ETC2.</para>
    /// </summary>
    public AndroidETC2FallbackOverride androidETC2FallbackOverride
    {
      get
      {
        return (AndroidETC2FallbackOverride) this.m_AndroidETC2FallbackOverride;
      }
      set
      {
        this.m_AndroidETC2FallbackOverride = (int) value;
      }
    }

    /// <summary>
    ///   <para>Copy parameters into another TextureImporterPlatformSettings object.</para>
    /// </summary>
    /// <param name="target">TextureImporterPlatformSettings object to copy settings to.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CopyTo(TextureImporterPlatformSettings target);
  }
}
