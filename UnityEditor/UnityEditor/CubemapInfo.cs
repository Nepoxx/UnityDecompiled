// Decompiled with JetBrains decompiler
// Type: UnityEditor.CubemapInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [Serializable]
  internal class CubemapInfo
  {
    public float angleOffset = 0.0f;
    public ShadowInfo shadowInfo = new ShadowInfo();
    private const float kDefaultShadowIntensity = 0.3f;
    public Cubemap cubemap;
    public CubemapInfo cubemapShadowInfo;
    public SphericalHarmonicsL2 ambientProbe;
    public int serialIndexMain;
    public int serialIndexShadow;
    [NonSerialized]
    public bool alreadyComputed;

    public void SetCubemapShadowInfo(CubemapInfo newCubemapShadowInfo)
    {
      this.cubemapShadowInfo = newCubemapShadowInfo;
      this.shadowInfo.shadowIntensity = newCubemapShadowInfo != this ? 1f : 0.3f;
      this.shadowInfo.shadowColor = Color.white;
    }

    public void ResetEnvInfos()
    {
      this.angleOffset = 0.0f;
    }
  }
}
