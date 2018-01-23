// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.ADBannerView
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.iOS
{
  [Obsolete("iOS.ADBannerView class is obsolete, Apple iAD service discontinued", true)]
  public sealed class ADBannerView
  {
    public ADBannerView(ADBannerView.Type type, ADBannerView.Layout layout)
    {
    }

    public static bool IsAvailable(ADBannerView.Type type)
    {
      return false;
    }

    public bool loaded
    {
      get
      {
        return false;
      }
    }

    public bool visible
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public ADBannerView.Layout layout
    {
      get
      {
        return ADBannerView.Layout.Top;
      }
      set
      {
      }
    }

    public Vector2 position
    {
      get
      {
        return new Vector2();
      }
      set
      {
      }
    }

    public Vector2 size
    {
      get
      {
        return new Vector2();
      }
    }

    public static event ADBannerView.BannerWasClickedDelegate onBannerWasClicked
    {
      add
      {
      }
      remove
      {
      }
    }

    public static event ADBannerView.BannerWasLoadedDelegate onBannerWasLoaded
    {
      add
      {
      }
      remove
      {
      }
    }

    public static event ADBannerView.BannerFailedToLoadDelegate onBannerFailedToLoad
    {
      add
      {
      }
      remove
      {
      }
    }

    public enum Layout
    {
      Manual = -1,
      Top = 0,
      TopLeft = 0,
      Bottom = 1,
      BottomLeft = 1,
      CenterLeft = 2,
      TopRight = 4,
      BottomRight = 5,
      CenterRight = 6,
      TopCenter = 8,
      BottomCenter = 9,
      Center = 10, // 0x0000000A
    }

    public enum Type
    {
      Banner,
      MediumRect,
    }

    public delegate void BannerWasClickedDelegate();

    public delegate void BannerWasLoadedDelegate();

    public delegate void BannerFailedToLoadDelegate();
  }
}
