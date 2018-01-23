// Decompiled with JetBrains decompiler
// Type: UnityEngine.ADInterstitialAd
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("ADInterstitialAd class is obsolete, Apple iAD service discontinued", true)]
  public sealed class ADInterstitialAd
  {
    public ADInterstitialAd(bool autoReload)
    {
    }

    public ADInterstitialAd()
    {
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

    public static bool isAvailable
    {
      get
      {
        return false;
      }
    }

    public bool loaded
    {
      get
      {
        return false;
      }
    }

    ~ADInterstitialAd()
    {
    }

    public void Show()
    {
    }

    public void ReloadAd()
    {
    }

    public delegate void InterstitialWasLoadedDelegate();
  }
}
