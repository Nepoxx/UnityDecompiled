// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.AssetBundle.BuildInput
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
  [UsedByNativeCode]
  [Serializable]
  public struct BuildInput
  {
    [NativeName("definitions")]
    internal BuildInput.Definition[] m_Definitions;

    public BuildInput.Definition[] definitions
    {
      get
      {
        return this.m_Definitions;
      }
      set
      {
        this.m_Definitions = value;
      }
    }

    [UsedByNativeCode]
    [Serializable]
    public struct AssetIdentifier
    {
      [NativeName("asset")]
      internal GUID m_Asset;
      [NativeName("address")]
      internal string m_Address;

      public GUID asset
      {
        get
        {
          return this.m_Asset;
        }
        set
        {
          this.m_Asset = value;
        }
      }

      public string address
      {
        get
        {
          return this.m_Address;
        }
        set
        {
          this.m_Address = value;
        }
      }
    }

    [UsedByNativeCode]
    [Serializable]
    public struct Definition
    {
      [NativeName("assetBundleName")]
      internal string m_AssetBundleName;
      [NativeName("explicitAssets")]
      internal BuildInput.AssetIdentifier[] m_ExplicitAssets;

      public string assetBundleName
      {
        get
        {
          return this.m_AssetBundleName;
        }
        set
        {
          this.m_AssetBundleName = value;
        }
      }

      public BuildInput.AssetIdentifier[] explicitAssets
      {
        get
        {
          return this.m_ExplicitAssets;
        }
        set
        {
          this.m_ExplicitAssets = value;
        }
      }
    }
  }
}
