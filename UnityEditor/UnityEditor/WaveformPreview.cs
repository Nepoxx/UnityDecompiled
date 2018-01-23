// Decompiled with JetBrains decompiler
// Type: UnityEditor.WaveformPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class WaveformPreview : IDisposable
  {
    private static int s_BaseTextureWidth = 4096;
    protected bool m_ClearTexture = true;
    private static Material s_Material;
    public UnityEngine.Object presentedObject;
    protected double m_Start;
    protected double m_Length;
    private Texture2D m_Texture;
    private Vector2 m_Size;
    private int m_Channels;
    private int m_Samples;
    private int m_SpecificChannel;
    private WaveformPreview.ChannelMode m_ChannelMode;
    private bool m_Looping;
    private bool m_Optimized;
    private bool m_Dirty;
    private bool m_Disposed;
    private WaveformPreview.MessageFlags m_Flags;

    protected WaveformPreview(UnityEngine.Object presentedObject, int samplesAndWidth, int channels)
    {
      this.presentedObject = presentedObject;
      this.optimized = true;
      this.m_Samples = samplesAndWidth;
      this.m_Channels = channels;
      this.backgroundColor = new Color(0.1568628f, 0.1568628f, 0.1568628f, 1f);
      this.waveColor = new Color(1f, 0.5490196f, 0.0f, 1f);
      this.UpdateTexture(samplesAndWidth, channels);
    }

    protected internal WaveformPreview(UnityEngine.Object presentedObject, int samplesAndWidth, int channels, bool deferTextureCreation)
    {
      this.presentedObject = presentedObject;
      this.optimized = true;
      this.m_Samples = samplesAndWidth;
      this.m_Channels = channels;
      this.backgroundColor = new Color(0.1568628f, 0.1568628f, 0.1568628f, 1f);
      this.waveColor = new Color(1f, 0.5490196f, 0.0f, 1f);
      if (deferTextureCreation)
        return;
      this.UpdateTexture(samplesAndWidth, channels);
    }

    public double start
    {
      get
      {
        return this.m_Start;
      }
    }

    public double length
    {
      get
      {
        return this.m_Length;
      }
    }

    public Color backgroundColor { get; set; }

    public Color waveColor { get; set; }

    public event Action updated;

    public bool optimized
    {
      get
      {
        return this.m_Optimized;
      }
      set
      {
        if (this.m_Optimized == value)
          return;
        if (value)
          this.m_Dirty = true;
        this.m_Optimized = value;
        this.m_Flags |= WaveformPreview.MessageFlags.Optimization;
      }
    }

    public bool looping
    {
      get
      {
        return this.m_Looping;
      }
      set
      {
        if (this.m_Looping == value)
          return;
        this.m_Dirty = true;
        this.m_Looping = value;
        this.m_Flags |= WaveformPreview.MessageFlags.Looping;
      }
    }

    protected static bool HasFlag(WaveformPreview.MessageFlags flags, WaveformPreview.MessageFlags test)
    {
      return (flags & test) != WaveformPreview.MessageFlags.None;
    }

    protected Vector2 Size
    {
      get
      {
        return this.m_Size;
      }
    }

    public void Dispose()
    {
      if (this.m_Disposed)
        return;
      this.m_Disposed = true;
      this.InternalDispose();
      if ((UnityEngine.Object) this.m_Texture != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Texture);
      this.m_Texture = (Texture2D) null;
    }

    protected virtual void InternalDispose()
    {
    }

    public void Render(Rect rect)
    {
      if ((UnityEngine.Object) WaveformPreview.s_Material == (UnityEngine.Object) null)
        WaveformPreview.s_Material = EditorGUIUtility.LoadRequired("Previews/PreviewAudioClipWaveform.mat") as Material;
      WaveformPreview.s_Material.SetTexture("_WavTex", (Texture) this.m_Texture);
      WaveformPreview.s_Material.SetFloat("_SampCount", (float) this.m_Samples);
      WaveformPreview.s_Material.SetFloat("_ChanCount", (float) this.m_Channels);
      WaveformPreview.s_Material.SetFloat("_RecPixelSize", 1f / rect.height);
      WaveformPreview.s_Material.SetColor("_BacCol", this.backgroundColor);
      WaveformPreview.s_Material.SetColor("_ForCol", this.waveColor);
      int num = -2;
      if (this.m_ChannelMode == WaveformPreview.ChannelMode.Separate)
        num = -1;
      else if (this.m_ChannelMode == WaveformPreview.ChannelMode.SpecificChannel)
        num = this.m_SpecificChannel;
      WaveformPreview.s_Material.SetInt("_ChanDrawMode", num);
      Graphics.DrawTexture(rect, (Texture) this.m_Texture, WaveformPreview.s_Material);
    }

    public bool ApplyModifications()
    {
      if (!this.m_Dirty && !((UnityEngine.Object) this.m_Texture == (UnityEngine.Object) null))
        return false;
      this.m_Flags |= !this.UpdateTexture((int) this.m_Size.x, this.m_Channels) ? WaveformPreview.MessageFlags.None : WaveformPreview.MessageFlags.TextureChanged;
      this.OnModifications(this.m_Flags);
      this.m_Flags = WaveformPreview.MessageFlags.None;
      this.m_Texture.Apply();
      this.m_Dirty = false;
      return true;
    }

    public void SetChannelMode(WaveformPreview.ChannelMode mode, int specificChannelToRender)
    {
      this.m_ChannelMode = mode;
      this.m_SpecificChannel = specificChannelToRender;
    }

    public void SetChannelMode(WaveformPreview.ChannelMode mode)
    {
      this.SetChannelMode(mode, 0);
    }

    private bool UpdateTexture(int width, int channels)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaveformPreview.\u003CUpdateTexture\u003Ec__AnonStorey0 textureCAnonStorey0 = new WaveformPreview.\u003CUpdateTexture\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      textureCAnonStorey0.\u0024this = this;
      int num = width * channels;
      // ISSUE: reference to a compiler-generated field
      textureCAnonStorey0.textureHeight = 1 + num / WaveformPreview.s_BaseTextureWidth;
      // ISSUE: reference to a compiler-generated method
      Action<bool> action = new Action<bool>(textureCAnonStorey0.\u003C\u003Em__0);
      if (width == this.m_Samples && channels == this.m_Channels && (UnityEngine.Object) this.m_Texture != (UnityEngine.Object) null)
        return false;
      action(this.m_ClearTexture);
      this.m_Samples = width;
      this.m_Channels = channels;
      return this.m_Dirty = true;
    }

    public void OptimizeForSize(Vector2 newSize)
    {
      newSize = new Vector2(Mathf.Ceil(newSize.x), Mathf.Ceil(newSize.y));
      if ((double) newSize.x == (double) this.m_Size.x)
        return;
      this.m_Size = newSize;
      this.m_Flags |= WaveformPreview.MessageFlags.Size;
      this.m_Dirty = true;
    }

    protected virtual void OnModifications(WaveformPreview.MessageFlags changedFlags)
    {
    }

    public void SetTimeInfo(double start, double length)
    {
      if (start == this.m_Start && length == this.m_Length)
        return;
      this.m_Start = start;
      this.m_Length = length;
      this.m_Dirty = true;
      this.m_Flags |= WaveformPreview.MessageFlags.Length | WaveformPreview.MessageFlags.Start;
    }

    public virtual void SetMMWaveData(int interleavedOffset, float[] data)
    {
      int index = 0;
      while (index < data.Length)
      {
        this.m_Texture.SetPixel(interleavedOffset % WaveformPreview.s_BaseTextureWidth, interleavedOffset / WaveformPreview.s_BaseTextureWidth, new Color(data[index], data[index + 1], 0.0f, 0.0f));
        ++interleavedOffset;
        index += 2;
      }
      this.m_Dirty = true;
      this.m_Flags |= WaveformPreview.MessageFlags.ContentsChanged;
      // ISSUE: reference to a compiler-generated field
      if (this.updated == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.updated();
    }

    public enum ChannelMode
    {
      MonoSum,
      Separate,
      SpecificChannel,
    }

    [System.Flags]
    protected enum MessageFlags
    {
      None = 0,
      Size = 1,
      Length = 2,
      Start = 4,
      Optimization = 8,
      TextureChanged = 16, // 0x00000010
      ContentsChanged = 32, // 0x00000020
      Looping = 64, // 0x00000040
    }
  }
}
