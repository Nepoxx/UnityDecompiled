// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightmapData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class LightmapData
  {
    internal Texture2D m_Light;
    internal Texture2D m_Dir;
    internal Texture2D m_ShadowMask;

    [Obsolete("Use lightmapColor property (UnityUpgradable) -> lightmapColor", false)]
    public Texture2D lightmapLight
    {
      get
      {
        return this.m_Light;
      }
      set
      {
        this.m_Light = value;
      }
    }

    public Texture2D lightmapColor
    {
      get
      {
        return this.m_Light;
      }
      set
      {
        this.m_Light = value;
      }
    }

    public Texture2D lightmapDir
    {
      get
      {
        return this.m_Dir;
      }
      set
      {
        this.m_Dir = value;
      }
    }

    public Texture2D shadowMask
    {
      get
      {
        return this.m_ShadowMask;
      }
      set
      {
        this.m_ShadowMask = value;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property LightmapData.lightmap has been deprecated. Use LightmapData.lightmapColor instead (UnityUpgradable) -> lightmapColor", true)]
    public Texture2D lightmap
    {
      get
      {
        return (Texture2D) null;
      }
      set
      {
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property LightmapData.lightmapFar has been deprecated. Use LightmapData.lightmapColor instead (UnityUpgradable) -> lightmapColor", true)]
    public Texture2D lightmapFar
    {
      get
      {
        return (Texture2D) null;
      }
      set
      {
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property LightmapData.lightmapNear has been deprecated. Use LightmapData.lightmapDir instead (UnityUpgradable) -> lightmapDir", true)]
    public Texture2D lightmapNear
    {
      get
      {
        return (Texture2D) null;
      }
      set
      {
      }
    }
  }
}
