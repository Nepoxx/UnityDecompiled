// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.DrawShadowsSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
  [UsedByNativeCode]
  public struct DrawShadowsSettings
  {
    private IntPtr _cullResults;
    public int lightIndex;
    public ShadowSplitData splitData;

    public DrawShadowsSettings(CullResults cullResults, int lightIndex)
    {
      this._cullResults = cullResults.cullResults;
      this.lightIndex = lightIndex;
      this.splitData.cullingPlaneCount = 0;
      this.splitData.cullingSphere = Vector4.zero;
    }

    public CullResults cullResults
    {
      set
      {
        this._cullResults = value.cullResults;
      }
    }
  }
}
