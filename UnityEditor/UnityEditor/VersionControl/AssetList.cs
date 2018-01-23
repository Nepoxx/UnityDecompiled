// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.AssetList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>A list of version control information about assets.</para>
  /// </summary>
  public class AssetList : List<Asset>
  {
    public AssetList()
    {
    }

    public AssetList(AssetList src)
    {
    }

    public AssetList Filter(bool includeFolder, params Asset.States[] states)
    {
      AssetList assetList = new AssetList();
      if (!includeFolder && (states == null || states.Length == 0))
        return assetList;
      foreach (Asset asset in (List<Asset>) this)
      {
        if (asset.isFolder)
        {
          if (includeFolder)
            assetList.Add(asset);
        }
        else if (asset.IsOneOfStates(states))
          assetList.Add(asset);
      }
      return assetList;
    }

    public int FilterCount(bool includeFolder, params Asset.States[] states)
    {
      int num = 0;
      if (!includeFolder && states == null)
        return this.Count;
      foreach (Asset asset in (List<Asset>) this)
      {
        if (asset.isFolder)
          ++num;
        else if (asset.IsOneOfStates(states))
          ++num;
      }
      return num;
    }

    /// <summary>
    ///   <para>Create an optimised list of assets by removing children of folders in the same list.</para>
    /// </summary>
    public AssetList FilterChildren()
    {
      AssetList assetList = new AssetList();
      assetList.AddRange((IEnumerable<Asset>) this);
      foreach (Asset asset in (List<Asset>) this)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        assetList.RemoveAll(new Predicate<Asset>(new AssetList.\u003CFilterChildren\u003Ec__AnonStorey0()
        {
          asset = asset
        }.\u003C\u003Em__0));
      }
      return assetList;
    }
  }
}
