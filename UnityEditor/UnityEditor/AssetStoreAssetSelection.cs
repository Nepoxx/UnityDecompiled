// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreAssetSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class AssetStoreAssetSelection
  {
    internal static Dictionary<int, AssetStoreAsset> s_SelectedAssets;

    public static void AddAsset(AssetStoreAsset searchResult, Texture2D placeholderPreviewImage)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey1 assetCAnonStorey1 = new AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      assetCAnonStorey1.searchResult = searchResult;
      if ((UnityEngine.Object) placeholderPreviewImage != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey1.searchResult.previewImage = AssetStoreAssetSelection.ScaleImage(placeholderPreviewImage, 256, 256);
      }
      // ISSUE: reference to a compiler-generated field
      assetCAnonStorey1.searchResult.previewInfo = (AssetStoreAsset.PreviewInfo) null;
      // ISSUE: reference to a compiler-generated field
      assetCAnonStorey1.searchResult.previewBundleRequest = (AssetBundleCreateRequest) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!string.IsNullOrEmpty(assetCAnonStorey1.searchResult.dynamicPreviewURL) && (UnityEngine.Object) assetCAnonStorey1.searchResult.previewBundle == (UnityEngine.Object) null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey0 assetCAnonStorey0 = new AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey0.\u003C\u003Ef__ref\u00241 = assetCAnonStorey1;
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey1.searchResult.disposed = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey0.client = new AsyncHTTPClient(assetCAnonStorey1.searchResult.dynamicPreviewURL);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        assetCAnonStorey0.client.doneCallback = new AsyncHTTPClient.DoneCallback(assetCAnonStorey0.\u003C\u003Em__0);
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey0.client.Begin();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (!string.IsNullOrEmpty(assetCAnonStorey1.searchResult.staticPreviewURL))
        {
          // ISSUE: reference to a compiler-generated field
          AssetStoreAssetSelection.DownloadStaticPreview(assetCAnonStorey1.searchResult);
        }
      }
      // ISSUE: reference to a compiler-generated field
      AssetStoreAssetSelection.AddAssetInternal(assetCAnonStorey1.searchResult);
      AssetStoreAssetSelection.RefreshFromServer((AssetStoreAssetSelection.AssetsRefreshed) null);
    }

    internal static void AddAssetInternal(AssetStoreAsset searchResult)
    {
      if (AssetStoreAssetSelection.s_SelectedAssets == null)
        AssetStoreAssetSelection.s_SelectedAssets = new Dictionary<int, AssetStoreAsset>();
      AssetStoreAssetSelection.s_SelectedAssets[searchResult.id] = searchResult;
    }

    private static void DownloadStaticPreview(AssetStoreAsset searchResult)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreAssetSelection.\u003CDownloadStaticPreview\u003Ec__AnonStorey3 previewCAnonStorey3 = new AssetStoreAssetSelection.\u003CDownloadStaticPreview\u003Ec__AnonStorey3();
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey3.searchResult = searchResult;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey3.client = new AsyncHTTPClient(previewCAnonStorey3.searchResult.staticPreviewURL);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      previewCAnonStorey3.client.doneCallback = new AsyncHTTPClient.DoneCallback(previewCAnonStorey3.\u003C\u003Em__0);
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey3.client.Begin();
    }

    public static void RefreshFromServer(AssetStoreAssetSelection.AssetsRefreshed callback)
    {
      if (AssetStoreAssetSelection.s_SelectedAssets.Count == 0)
        return;
      List<AssetStoreAsset> assets = new List<AssetStoreAsset>();
      foreach (KeyValuePair<int, AssetStoreAsset> selectedAsset in AssetStoreAssetSelection.s_SelectedAssets)
        assets.Add(selectedAsset.Value);
      AssetStoreClient.AssetsInfo(assets, (AssetStoreResultBase<AssetStoreAssetsInfo>.Callback) (results =>
      {
        AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.ServiceDisabled;
        if (results.error != null && results.error != "")
        {
          Console.WriteLine("Error performing Asset Store Info search: " + results.error);
          AssetStoreAssetInspector.OfflineNoticeEnabled = true;
          if (callback == null)
            return;
          callback();
        }
        else
        {
          AssetStoreAssetInspector.OfflineNoticeEnabled = false;
          if (results.status == AssetStoreAssetsInfo.Status.Ok)
            AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.Ok;
          else if (results.status == AssetStoreAssetsInfo.Status.BasketNotEmpty)
            AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.BasketNotEmpty;
          else if (results.status == AssetStoreAssetsInfo.Status.AnonymousUser)
            AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.AnonymousUser;
          AssetStoreAssetInspector.s_PurchaseMessage = results.message;
          AssetStoreAssetInspector.s_PaymentMethodCard = results.paymentMethodCard;
          AssetStoreAssetInspector.s_PaymentMethodExpire = results.paymentMethodExpire;
          AssetStoreAssetInspector.s_PriceText = results.priceText;
          AssetStoreAssetInspector.Instance.Repaint();
          if (callback == null)
            return;
          callback();
        }
      }));
    }

    private static Texture2D ScaleImage(Texture2D source, int w, int h)
    {
      if (source.width % 4 != 0)
        return (Texture2D) null;
      Texture2D texture2D = new Texture2D(w, h, TextureFormat.RGB24, false, true);
      Color[] pixels = texture2D.GetPixels(0);
      double num1 = 1.0 / (double) w;
      double num2 = 1.0 / (double) h;
      double num3 = 0.0;
      double num4 = 0.0;
      int index1 = 0;
      for (int index2 = 0; index2 < h; ++index2)
      {
        int num5 = 0;
        while (num5 < w)
        {
          pixels[index1] = source.GetPixelBilinear((float) num3, (float) num4);
          num3 += num1;
          ++num5;
          ++index1;
        }
        num3 = 0.0;
        num4 += num2;
      }
      texture2D.SetPixels(pixels, 0);
      texture2D.Apply();
      return texture2D;
    }

    public static bool ContainsAsset(int id)
    {
      return AssetStoreAssetSelection.s_SelectedAssets != null && AssetStoreAssetSelection.s_SelectedAssets.ContainsKey(id);
    }

    public static void Clear()
    {
      if (AssetStoreAssetSelection.s_SelectedAssets == null)
        return;
      foreach (KeyValuePair<int, AssetStoreAsset> selectedAsset in AssetStoreAssetSelection.s_SelectedAssets)
        selectedAsset.Value.Dispose();
      AssetStoreAssetSelection.s_SelectedAssets.Clear();
    }

    public static int Count
    {
      get
      {
        return AssetStoreAssetSelection.s_SelectedAssets != null ? AssetStoreAssetSelection.s_SelectedAssets.Count : 0;
      }
    }

    public static bool Empty
    {
      get
      {
        return AssetStoreAssetSelection.s_SelectedAssets == null || AssetStoreAssetSelection.s_SelectedAssets.Count == 0;
      }
    }

    public static AssetStoreAsset GetFirstAsset()
    {
      if (AssetStoreAssetSelection.s_SelectedAssets == null)
        return (AssetStoreAsset) null;
      Dictionary<int, AssetStoreAsset>.Enumerator enumerator = AssetStoreAssetSelection.s_SelectedAssets.GetEnumerator();
      if (!enumerator.MoveNext())
        return (AssetStoreAsset) null;
      return enumerator.Current.Value;
    }

    public delegate void AssetsRefreshed();
  }
}
