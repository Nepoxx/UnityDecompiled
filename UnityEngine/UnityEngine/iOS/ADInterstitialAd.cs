// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.ADInterstitialAd
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.iOS
{
  [Obsolete("iOS.ADInterstitialAd class is obsolete, Apple iAD service discontinued", true)]
  public sealed class ADInterstitialAd
  {
    public ADInterstitialAd(bool autoReload)
    {
    }

    public ADInterstitialAd()
    {
    }

    public static bool isAvailable
    {
      get
      {
        return false;
      }
    }

    public void Show()
    {
    }

    public void ReloadAd()
    {
    }

    public bool loaded
    {
      get
      {
        return false;
      }
    }

    public static event ADInterstitialAd.InterstitialWasLoadedDelegate onInterstitialWasLoaded
    {
      add
      {
      }
      remove
      {
      }
    }

    public static event ADInterstitialAd.InterstitialWasViewedDelegate onInterstitialWasViewed
    {
      add
      {
      }
      remove
      {
      }
    }

    public delegate void InterstitialWasLoadedDelegate();

    public delegate void InterstitialWasViewedDelegate();
  }
}
