// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreAssetsInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetStoreAssetsInfo : AssetStoreResultBase<AssetStoreAssetsInfo>
  {
    internal Dictionary<int, AssetStoreAsset> assets = new Dictionary<int, AssetStoreAsset>();
    internal AssetStoreAssetsInfo.Status status;
    internal bool paymentTokenAvailable;
    internal string paymentMethodCard;
    internal string paymentMethodExpire;
    internal float price;
    internal float vat;
    internal string currency;
    internal string priceText;
    internal string vatText;
    internal string message;

    internal AssetStoreAssetsInfo(AssetStoreResultBase<AssetStoreAssetsInfo>.Callback c, List<AssetStoreAsset> assets)
      : base(c)
    {
      foreach (AssetStoreAsset asset in assets)
        this.assets[asset.id] = asset;
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      Dictionary<string, JSONValue> dictionary = dict["purchase_info"].AsDict(true);
      string str = dictionary["status"].AsString(true);
      if (str == "basket-not-empty")
        this.status = AssetStoreAssetsInfo.Status.BasketNotEmpty;
      else if (str == "service-disabled")
        this.status = AssetStoreAssetsInfo.Status.ServiceDisabled;
      else if (str == "user-anonymous")
        this.status = AssetStoreAssetsInfo.Status.AnonymousUser;
      else if (str == "ok")
        this.status = AssetStoreAssetsInfo.Status.Ok;
      this.paymentTokenAvailable = dictionary["payment_token_available"].AsBool();
      if (dictionary.ContainsKey("payment_method_card"))
        this.paymentMethodCard = dictionary["payment_method_card"].AsString(true);
      if (dictionary.ContainsKey("payment_method_expire"))
        this.paymentMethodExpire = dictionary["payment_method_expire"].AsString(true);
      this.price = dictionary["price"].AsFloat(true);
      this.vat = dictionary["vat"].AsFloat(true);
      this.priceText = dictionary["price_text"].AsString(true);
      this.vatText = dictionary["vat_text"].AsString(true);
      this.currency = dictionary["currency"].AsString(true);
      this.message = !dictionary.ContainsKey("message") ? (string) null : dictionary["message"].AsString(true);
      foreach (JSONValue jsonValue in dict["results"].AsList(true))
      {
        AssetStoreAsset assetStoreAsset;
        if (this.assets.TryGetValue(!jsonValue["id"].IsString() ? (int) jsonValue["id"].AsFloat() : int.Parse(jsonValue["id"].AsString()), out assetStoreAsset))
        {
          if (assetStoreAsset.previewInfo == null)
            assetStoreAsset.previewInfo = new AssetStoreAsset.PreviewInfo();
          AssetStoreAsset.PreviewInfo previewInfo = assetStoreAsset.previewInfo;
          assetStoreAsset.className = jsonValue["class_names"].AsString(true).Trim();
          previewInfo.packageName = jsonValue["package_name"].AsString(true).Trim();
          previewInfo.packageShortUrl = jsonValue["short_url"].AsString(true).Trim();
          assetStoreAsset.price = !jsonValue.ContainsKey("price_text") ? (string) null : jsonValue["price_text"].AsString(true).Trim();
          previewInfo.packageSize = int.Parse(!jsonValue.Get("package_size").IsNull() ? jsonValue["package_size"].AsString(true) : "-1");
          assetStoreAsset.packageID = int.Parse(jsonValue["package_id"].AsString());
          previewInfo.packageVersion = jsonValue["package_version"].AsString();
          previewInfo.packageRating = int.Parse(jsonValue.Get("rating").IsNull() || jsonValue["rating"].AsString(true).Length == 0 ? "-1" : jsonValue["rating"].AsString(true));
          previewInfo.packageAssetCount = int.Parse(!jsonValue["package_asset_count"].IsNull() ? jsonValue["package_asset_count"].AsString(true) : "-1");
          previewInfo.isPurchased = jsonValue.ContainsKey("purchased") && jsonValue["purchased"].AsBool(true);
          previewInfo.isDownloadable = previewInfo.isPurchased || assetStoreAsset.price == null;
          previewInfo.publisherName = jsonValue["publisher_name"].AsString(true).Trim();
          previewInfo.packageUrl = !jsonValue.Get("package_url").IsNull() ? jsonValue["package_url"].AsString(true) : "";
          previewInfo.encryptionKey = !jsonValue.Get("encryption_key").IsNull() ? jsonValue["encryption_key"].AsString(true) : "";
          previewInfo.categoryName = !jsonValue.Get("category_name").IsNull() ? jsonValue["category_name"].AsString(true) : "";
          previewInfo.buildProgress = -1f;
          previewInfo.downloadProgress = -1f;
        }
      }
    }

    internal enum Status
    {
      BasketNotEmpty,
      ServiceDisabled,
      AnonymousUser,
      Ok,
    }
  }
}
