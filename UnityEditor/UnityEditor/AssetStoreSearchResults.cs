// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreSearchResults
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetStoreSearchResults : AssetStoreResultBase<AssetStoreSearchResults>
  {
    internal List<AssetStoreSearchResults.Group> groups = new List<AssetStoreSearchResults.Group>();

    public AssetStoreSearchResults(AssetStoreResultBase<AssetStoreSearchResults>.Callback c)
      : base(c)
    {
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      foreach (JSONValue matches in dict["groups"].AsList(true))
      {
        AssetStoreSearchResults.Group group = AssetStoreSearchResults.Group.Create();
        this.ParseList(matches, ref group);
        this.groups.Add(group);
      }
      JSONValue jsonValue1 = dict["query"]["offsets"];
      List<JSONValue> jsonValueList = dict["query"]["limits"].AsList(true);
      int index = 0;
      foreach (JSONValue jsonValue2 in jsonValue1.AsList(true))
      {
        AssetStoreSearchResults.Group group = this.groups[index];
        group.offset = (int) jsonValue2.AsFloat(true);
        group.limit = (int) jsonValueList[index].AsFloat(true);
        this.groups[index] = group;
        ++index;
      }
    }

    private static string StripExtension(string path)
    {
      if (path == null)
        return (string) null;
      int length = path.LastIndexOf(".");
      return length >= 0 ? path.Substring(0, length) : path;
    }

    private void ParseList(JSONValue matches, ref AssetStoreSearchResults.Group group)
    {
      List<AssetStoreAsset> assets = group.assets;
      if (matches.ContainsKey("error"))
        this.error = matches["error"].AsString(true);
      if (matches.ContainsKey("warnings"))
        this.warnings = matches["warnings"].AsString(true);
      if (matches.ContainsKey("name"))
        group.name = matches["name"].AsString(true);
      if (matches.ContainsKey("label"))
        group.label = matches["label"].AsString(true);
      group.label = group.label ?? group.name;
      if (matches.ContainsKey("total_found"))
        group.totalFound = (int) matches["total_found"].AsFloat(true);
      if (!matches.ContainsKey(nameof (matches)))
        return;
      foreach (JSONValue jsonValue in matches[nameof (matches)].AsList(true))
      {
        AssetStoreAsset assetStoreAsset = new AssetStoreAsset();
        if (jsonValue.ContainsKey("id") && jsonValue.ContainsKey("name") && jsonValue.ContainsKey("package_id"))
        {
          assetStoreAsset.id = (int) jsonValue["id"].AsFloat();
          assetStoreAsset.name = jsonValue["name"].AsString();
          assetStoreAsset.displayName = AssetStoreSearchResults.StripExtension(assetStoreAsset.name);
          assetStoreAsset.packageID = (int) jsonValue["package_id"].AsFloat();
          if (jsonValue.ContainsKey("static_preview_url"))
            assetStoreAsset.staticPreviewURL = jsonValue["static_preview_url"].AsString();
          if (jsonValue.ContainsKey("dynamic_preview_url"))
            assetStoreAsset.dynamicPreviewURL = jsonValue["dynamic_preview_url"].AsString();
          assetStoreAsset.className = !jsonValue.ContainsKey("class_name") ? "" : jsonValue["class_name"].AsString();
          if (jsonValue.ContainsKey("price"))
            assetStoreAsset.price = jsonValue["price"].AsString();
          assets.Add(assetStoreAsset);
        }
      }
    }

    internal struct Group
    {
      public List<AssetStoreAsset> assets;
      public int totalFound;
      public string label;
      public string name;
      public int offset;
      public int limit;

      public static AssetStoreSearchResults.Group Create()
      {
        return new AssetStoreSearchResults.Group() { assets = new List<AssetStoreAsset>(), label = "", name = "", offset = 0, limit = -1 };
      }
    }
  }
}
