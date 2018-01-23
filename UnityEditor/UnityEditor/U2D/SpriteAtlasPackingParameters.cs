// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpriteAtlasPackingParameters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEditor.U2D
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class SpriteAtlasPackingParameters
  {
    internal uint m_BlockOffset;
    internal uint m_Padding;
    internal int m_AllowAlphaSplitting;
    internal int m_EnableRotation;
    internal int m_EnableTightPacking;

    public uint blockOffset
    {
      get
      {
        return this.m_BlockOffset;
      }
      set
      {
        this.m_BlockOffset = value;
      }
    }

    public uint padding
    {
      get
      {
        return this.m_Padding;
      }
      set
      {
        this.m_Padding = value;
      }
    }

    public bool allowAlphaSplitting
    {
      get
      {
        return this.m_AllowAlphaSplitting != 0;
      }
      set
      {
        this.m_AllowAlphaSplitting = !value ? 0 : 1;
      }
    }

    public bool enableRotation
    {
      get
      {
        return this.m_EnableRotation != 0;
      }
      set
      {
        this.m_EnableRotation = !value ? 0 : 1;
      }
    }

    public bool enableTightPacking
    {
      get
      {
        return this.m_EnableTightPacking != 0;
      }
      set
      {
        this.m_EnableTightPacking = !value ? 0 : 1;
      }
    }
  }
}
