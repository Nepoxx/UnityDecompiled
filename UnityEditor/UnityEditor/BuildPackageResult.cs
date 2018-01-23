// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPackageResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class BuildPackageResult : AssetStoreResultBase<BuildPackageResult>
  {
    internal AssetStoreAsset asset;
    internal int packageID;

    internal BuildPackageResult(AssetStoreAsset asset, AssetStoreResultBase<BuildPackageResult>.Callback c)
      : base(c)
    {
      this.asset = asset;
      this.packageID = -1;
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      dict = dict["download"].AsDict();
      this.packageID = int.Parse(dict["id"].AsString());
      if (this.packageID != this.asset.packageID)
      {
        Debug.LogError((object) "Got asset store server build result from mismatching package");
      }
      else
      {
        this.asset.previewInfo.packageUrl = !dict.ContainsKey("url") ? "" : dict["url"].AsString(true);
        this.asset.previewInfo.encryptionKey = !dict.ContainsKey("key") ? "" : dict["key"].AsString(true);
        this.asset.previewInfo.buildProgress = (float) ((!dict["progress"].IsFloat() ? (double) float.Parse(dict["progress"].AsString(true), (IFormatProvider) CultureInfo.InvariantCulture) : (double) dict["progress"].AsFloat(true)) / 100.0);
      }
    }
  }
}
