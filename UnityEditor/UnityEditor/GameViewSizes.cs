// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSizes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Build;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;

namespace UnityEditor
{
  [FilePath("GameViewSizes.asset", FilePathAttribute.Location.PreferencesFolder)]
  internal class GameViewSizes : ScriptableSingleton<GameViewSizes>
  {
    [SerializeField]
    private GameViewSizeGroup m_Standalone = new GameViewSizeGroup();
    [SerializeField]
    private GameViewSizeGroup m_iOS = new GameViewSizeGroup();
    [SerializeField]
    private GameViewSizeGroup m_Android = new GameViewSizeGroup();
    [SerializeField]
    private GameViewSizeGroup m_WiiU = new GameViewSizeGroup();
    [SerializeField]
    private GameViewSizeGroup m_Tizen = new GameViewSizeGroup();
    [SerializeField]
    private GameViewSizeGroup m_N3DS = new GameViewSizeGroup();
    [SerializeField]
    private GameViewSizeGroup m_HMD = new GameViewSizeGroup();
    [NonSerialized]
    private GameViewSize m_Remote = (GameViewSize) null;
    [NonSerialized]
    private Vector2 m_LastStandaloneScreenSize = new Vector2(-1f, -1f);
    [NonSerialized]
    private Vector2 m_LastRemoteScreenSize = new Vector2(-1f, -1f);
    [NonSerialized]
    private int m_ChangeID = 0;
    [NonSerialized]
    private static GameViewSizeGroupType s_GameViewSizeGroupType;

    public GameViewSizeGroupType currentGroupType
    {
      get
      {
        return GameViewSizes.s_GameViewSizeGroupType;
      }
    }

    public GameViewSizeGroup currentGroup
    {
      get
      {
        return this.GetGroup(GameViewSizes.s_GameViewSizeGroupType);
      }
    }

    private void OnEnable()
    {
      GameViewSizes.RefreshGameViewSizeGroupType(BuildTarget.NoTarget, EditorUserBuildSettings.activeBuildTarget);
    }

    public GameViewSizeGroup GetGroup(GameViewSizeGroupType gameViewSizeGroupType)
    {
      this.InitBuiltinGroups();
      switch (gameViewSizeGroupType)
      {
        case GameViewSizeGroupType.Standalone:
        case GameViewSizeGroupType.WebPlayer:
        case GameViewSizeGroupType.PS3:
        case GameViewSizeGroupType.WP8:
          return this.m_Standalone;
        case GameViewSizeGroupType.iOS:
          return this.m_iOS;
        case GameViewSizeGroupType.Android:
          return this.m_Android;
        case GameViewSizeGroupType.WiiU:
          return this.m_WiiU;
        case GameViewSizeGroupType.Tizen:
          return this.m_Tizen;
        case GameViewSizeGroupType.N3DS:
          return this.m_N3DS;
        case GameViewSizeGroupType.HMD:
          return this.m_HMD;
        default:
          Debug.LogError((object) ("Unhandled group enum! " + (object) gameViewSizeGroupType));
          return this.m_Standalone;
      }
    }

    public void SaveToHDD()
    {
      this.Save(true);
    }

    public bool IsDefaultStandaloneScreenSize(GameViewSizeGroupType gameViewSizeGroupType, int index)
    {
      return gameViewSizeGroupType == GameViewSizeGroupType.Standalone && this.GetDefaultStandaloneIndex() == index;
    }

    public bool IsRemoteScreenSize(GameViewSizeGroupType gameViewSizeGroupType, int index)
    {
      return this.GetGroup(gameViewSizeGroupType).IndexOf(this.m_Remote) == index;
    }

    public int GetDefaultStandaloneIndex()
    {
      return this.m_Standalone.GetBuiltinCount() - 1;
    }

    public void RefreshStandaloneAndRemoteDefaultSizes()
    {
      if ((double) InternalEditorUtility.defaultScreenWidth != (double) this.m_LastStandaloneScreenSize.x || (double) InternalEditorUtility.defaultScreenHeight != (double) this.m_LastStandaloneScreenSize.y)
      {
        this.m_LastStandaloneScreenSize = new Vector2(InternalEditorUtility.defaultScreenWidth, InternalEditorUtility.defaultScreenHeight);
        this.RefreshStandaloneDefaultScreenSize((int) this.m_LastStandaloneScreenSize.x, (int) this.m_LastStandaloneScreenSize.y);
      }
      if ((double) InternalEditorUtility.remoteScreenWidth != (double) this.m_LastRemoteScreenSize.x || (double) InternalEditorUtility.remoteScreenHeight != (double) this.m_LastRemoteScreenSize.y)
      {
        this.m_LastRemoteScreenSize = new Vector2(InternalEditorUtility.remoteScreenWidth, InternalEditorUtility.remoteScreenHeight);
        this.RefreshRemoteScreenSize((int) this.m_LastRemoteScreenSize.x, (int) this.m_LastRemoteScreenSize.y);
      }
      if (!XRSettings.isDeviceActive || this.m_Remote.width == XRSettings.eyeTextureWidth || this.m_Remote.height == XRSettings.eyeTextureHeight)
        return;
      this.RefreshRemoteScreenSize(XRSettings.eyeTextureWidth, XRSettings.eyeTextureHeight);
    }

    public void RefreshStandaloneDefaultScreenSize(int width, int height)
    {
      GameViewSize gameViewSize = this.m_Standalone.GetGameViewSize(this.GetDefaultStandaloneIndex());
      gameViewSize.height = height;
      gameViewSize.width = width;
      this.Changed();
    }

    public void RefreshRemoteScreenSize(int width, int height)
    {
      this.m_Remote.width = width;
      this.m_Remote.height = height;
      this.m_Remote.baseText = width <= 0 || height <= 0 ? "Remote (Not Connected)" : "Remote";
      this.Changed();
    }

    public void Changed()
    {
      ++this.m_ChangeID;
    }

    public int GetChangeID()
    {
      return this.m_ChangeID;
    }

    private void InitBuiltinGroups()
    {
      if (this.m_Standalone.GetBuiltinCount() > 0)
        return;
      this.m_Remote = new GameViewSize(GameViewSizeType.FixedResolution, 0, 0, "Remote (Not Connected)");
      GameViewSize gameViewSize1 = new GameViewSize(GameViewSizeType.AspectRatio, 0, 0, "Free Aspect");
      GameViewSize gameViewSize2 = new GameViewSize(GameViewSizeType.AspectRatio, 5, 4, "");
      GameViewSize gameViewSize3 = new GameViewSize(GameViewSizeType.AspectRatio, 4, 3, "");
      GameViewSize gameViewSize4 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 2, "");
      GameViewSize gameViewSize5 = new GameViewSize(GameViewSizeType.AspectRatio, 16, 10, "");
      GameViewSize gameViewSize6 = new GameViewSize(GameViewSizeType.AspectRatio, 16, 9, "");
      GameViewSize gameViewSize7 = new GameViewSize(GameViewSizeType.FixedResolution, 0, 0, "Standalone");
      GameViewSize gameViewSize8 = new GameViewSize(GameViewSizeType.FixedResolution, 320, 480, "iPhone Tall");
      GameViewSize gameViewSize9 = new GameViewSize(GameViewSizeType.FixedResolution, 480, 320, "iPhone Wide");
      GameViewSize gameViewSize10 = new GameViewSize(GameViewSizeType.FixedResolution, 640, 960, "iPhone 4 Tall");
      GameViewSize gameViewSize11 = new GameViewSize(GameViewSizeType.FixedResolution, 960, 640, "iPhone 4 Wide");
      GameViewSize gameViewSize12 = new GameViewSize(GameViewSizeType.FixedResolution, 768, 1024, "iPad Tall");
      GameViewSize gameViewSize13 = new GameViewSize(GameViewSizeType.FixedResolution, 1024, 768, "iPad Wide");
      GameViewSize gameViewSize14 = new GameViewSize(GameViewSizeType.AspectRatio, 9, 16, "iPhone 5 Tall");
      GameViewSize gameViewSize15 = new GameViewSize(GameViewSizeType.AspectRatio, 16, 9, "iPhone 5 Wide");
      GameViewSize gameViewSize16 = new GameViewSize(GameViewSizeType.AspectRatio, 2, 3, "iPhone Tall");
      GameViewSize gameViewSize17 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 2, "iPhone Wide");
      GameViewSize gameViewSize18 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 4, "iPad Tall");
      GameViewSize gameViewSize19 = new GameViewSize(GameViewSizeType.AspectRatio, 4, 3, "iPad Wide");
      GameViewSize gameViewSize20 = new GameViewSize(GameViewSizeType.FixedResolution, 320, 480, "HVGA Portrait");
      GameViewSize gameViewSize21 = new GameViewSize(GameViewSizeType.FixedResolution, 480, 320, "HVGA Landscape");
      GameViewSize gameViewSize22 = new GameViewSize(GameViewSizeType.FixedResolution, 480, 800, "WVGA Portrait");
      GameViewSize gameViewSize23 = new GameViewSize(GameViewSizeType.FixedResolution, 800, 480, "WVGA Landscape");
      GameViewSize gameViewSize24 = new GameViewSize(GameViewSizeType.FixedResolution, 480, 854, "FWVGA Portrait");
      GameViewSize gameViewSize25 = new GameViewSize(GameViewSizeType.FixedResolution, 854, 480, "FWVGA Landscape");
      GameViewSize gameViewSize26 = new GameViewSize(GameViewSizeType.FixedResolution, 600, 1024, "WSVGA Portrait");
      GameViewSize gameViewSize27 = new GameViewSize(GameViewSizeType.FixedResolution, 1024, 600, "WSVGA Landscape");
      GameViewSize gameViewSize28 = new GameViewSize(GameViewSizeType.FixedResolution, 800, 1280, "WXGA Portrait");
      GameViewSize gameViewSize29 = new GameViewSize(GameViewSizeType.FixedResolution, 1280, 800, "WXGA Landscape");
      GameViewSize gameViewSize30 = new GameViewSize(GameViewSizeType.AspectRatio, 2, 3, "3:2 Portrait");
      GameViewSize gameViewSize31 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 2, "3:2 Landscape");
      GameViewSize gameViewSize32 = new GameViewSize(GameViewSizeType.AspectRatio, 10, 16, "16:10 Portrait");
      GameViewSize gameViewSize33 = new GameViewSize(GameViewSizeType.AspectRatio, 16, 10, "16:10 Landscape");
      GameViewSize gameViewSize34 = new GameViewSize(GameViewSizeType.FixedResolution, 1920, 1080, "1080p (16:9)");
      GameViewSize gameViewSize35 = new GameViewSize(GameViewSizeType.FixedResolution, 1280, 720, "720p (16:9)");
      GameViewSize gameViewSize36 = new GameViewSize(GameViewSizeType.FixedResolution, 854, 480, "GamePad 480p (16:9)");
      GameViewSize gameViewSize37 = new GameViewSize(GameViewSizeType.FixedResolution, 1280, 720, "16:9 Landscape");
      GameViewSize gameViewSize38 = new GameViewSize(GameViewSizeType.FixedResolution, 720, 1280, "9:16 Portrait");
      GameViewSize gameViewSize39 = new GameViewSize(GameViewSizeType.FixedResolution, 400, 240, "Top Screen");
      GameViewSize gameViewSize40 = new GameViewSize(GameViewSizeType.FixedResolution, 320, 240, "Bottom Screen");
      this.m_Standalone.AddBuiltinSizes(gameViewSize1, gameViewSize2, gameViewSize3, gameViewSize4, gameViewSize5, gameViewSize6, gameViewSize7);
      this.m_WiiU.AddBuiltinSizes(gameViewSize1, gameViewSize3, gameViewSize6, gameViewSize34, gameViewSize35, gameViewSize36);
      this.m_iOS.AddBuiltinSizes(gameViewSize1, gameViewSize8, gameViewSize9, gameViewSize10, gameViewSize11, gameViewSize12, gameViewSize13, gameViewSize14, gameViewSize15, gameViewSize16, gameViewSize17, gameViewSize18, gameViewSize19);
      this.m_Android.AddBuiltinSizes(gameViewSize1, this.m_Remote, gameViewSize20, gameViewSize21, gameViewSize22, gameViewSize23, gameViewSize24, gameViewSize25, gameViewSize26, gameViewSize27, gameViewSize28, gameViewSize29, gameViewSize30, gameViewSize31, gameViewSize32, gameViewSize33);
      this.m_Tizen.AddBuiltinSizes(gameViewSize1, gameViewSize37, gameViewSize38);
      this.m_N3DS.AddBuiltinSizes(gameViewSize1, gameViewSize39, gameViewSize40);
      this.m_HMD.AddBuiltinSizes(gameViewSize1, this.m_Remote);
    }

    internal static bool DefaultLowResolutionSettingForStandalone()
    {
      if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX)
        return !PlayerSettings.macRetinaSupport;
      return true;
    }

    internal static bool DefaultLowResolutionSettingForSizeGroupType(GameViewSizeGroupType sizeGroupType)
    {
      switch (sizeGroupType)
      {
        case GameViewSizeGroupType.Standalone:
          return GameViewSizes.DefaultLowResolutionSettingForStandalone();
        case GameViewSizeGroupType.WiiU:
        case GameViewSizeGroupType.N3DS:
          return true;
        default:
          return false;
      }
    }

    private static void RefreshDerivedGameViewSize(GameViewSizeGroupType groupType, int gameViewSizeIndex, GameViewSize gameViewSize)
    {
      if (ScriptableSingleton<GameViewSizes>.instance.IsDefaultStandaloneScreenSize(groupType, gameViewSizeIndex))
      {
        gameViewSize.width = (int) InternalEditorUtility.defaultScreenWidth;
        gameViewSize.height = (int) InternalEditorUtility.defaultScreenHeight;
      }
      else
      {
        if (!ScriptableSingleton<GameViewSizes>.instance.IsRemoteScreenSize(groupType, gameViewSizeIndex))
          return;
        int num1;
        int num2;
        if (XRSettings.isDeviceActive)
        {
          num1 = XRSettings.eyeTextureWidth;
          num2 = XRSettings.eyeTextureHeight;
        }
        else
        {
          num1 = (int) InternalEditorUtility.remoteScreenWidth;
          num2 = (int) InternalEditorUtility.remoteScreenHeight;
        }
        if (num1 > 0 && num2 > 0)
        {
          gameViewSize.sizeType = GameViewSizeType.FixedResolution;
          gameViewSize.width = num1;
          gameViewSize.height = num2;
        }
        else
        {
          gameViewSize.sizeType = GameViewSizeType.AspectRatio;
          GameViewSize gameViewSize1 = gameViewSize;
          int num3 = 0;
          gameViewSize.height = num3;
          int num4 = num3;
          gameViewSize1.width = num4;
        }
      }
    }

    public static Rect GetConstrainedRect(Rect startRect, GameViewSizeGroupType groupType, int gameViewSizeIndex, out bool fitsInsideRect)
    {
      fitsInsideRect = true;
      Rect rect = startRect;
      GameViewSize gameViewSize = ScriptableSingleton<GameViewSizes>.instance.GetGroup(groupType).GetGameViewSize(gameViewSizeIndex);
      GameViewSizes.RefreshDerivedGameViewSize(groupType, gameViewSizeIndex, gameViewSize);
      if (gameViewSize.isFreeAspectRatio)
        return startRect;
      float num = 0.0f;
      bool flag;
      switch (gameViewSize.sizeType)
      {
        case GameViewSizeType.AspectRatio:
          num = gameViewSize.aspectRatio;
          flag = true;
          break;
        case GameViewSizeType.FixedResolution:
          if ((double) gameViewSize.height > (double) startRect.height || (double) gameViewSize.width > (double) startRect.width)
          {
            num = gameViewSize.aspectRatio;
            flag = true;
            fitsInsideRect = false;
            break;
          }
          rect.height = (float) gameViewSize.height;
          rect.width = (float) gameViewSize.width;
          flag = false;
          break;
        default:
          throw new ArgumentException("Unrecognized size type");
      }
      if (flag)
      {
        rect.height = (double) rect.width / (double) num <= (double) startRect.height ? rect.width / num : startRect.height;
        rect.width = rect.height * num;
      }
      rect.height = Mathf.Clamp(rect.height, 0.0f, startRect.height);
      rect.width = Mathf.Clamp(rect.width, 0.0f, startRect.width);
      rect.y = (float) ((double) startRect.height * 0.5 - (double) rect.height * 0.5) + startRect.y;
      rect.x = (float) ((double) startRect.width * 0.5 - (double) rect.width * 0.5) + startRect.x;
      rect.width = Mathf.Floor(rect.width + 0.5f);
      rect.height = Mathf.Floor(rect.height + 0.5f);
      rect.x = Mathf.Floor(rect.x + 0.5f);
      rect.y = Mathf.Floor(rect.y + 0.5f);
      return rect;
    }

    public static Vector2 GetRenderTargetSize(Rect startRect, GameViewSizeGroupType groupType, int gameViewSizeIndex, out bool clamped)
    {
      GameViewSize gameViewSize = ScriptableSingleton<GameViewSizes>.instance.GetGroup(groupType).GetGameViewSize(gameViewSizeIndex);
      GameViewSizes.RefreshDerivedGameViewSize(groupType, gameViewSizeIndex, gameViewSize);
      clamped = false;
      Vector2 vector2;
      if (gameViewSize.isFreeAspectRatio)
      {
        vector2 = startRect.size;
      }
      else
      {
        switch (gameViewSize.sizeType)
        {
          case GameViewSizeType.AspectRatio:
            vector2 = (double) startRect.height == 0.0 || (double) gameViewSize.aspectRatio == 0.0 ? Vector2.zero : ((double) (startRect.width / startRect.height) >= (double) gameViewSize.aspectRatio ? new Vector2(startRect.height * gameViewSize.aspectRatio, startRect.height) : new Vector2(startRect.width, startRect.width / gameViewSize.aspectRatio));
            break;
          case GameViewSizeType.FixedResolution:
            vector2 = new Vector2((float) gameViewSize.width, (float) gameViewSize.height);
            break;
          default:
            throw new ArgumentException("Unrecognized size type");
        }
      }
      float num1 = (float) ((double) SystemInfo.graphicsMemorySize * 0.200000002980232 / 12.0 * 1024.0 * 1024.0);
      if ((double) (vector2.x * vector2.y) > (double) num1)
      {
        float num2 = vector2.y / vector2.x;
        vector2.x = Mathf.Sqrt(num1 * num2);
        vector2.y = num2 * vector2.x;
        clamped = true;
      }
      float num3 = Mathf.Min((float) SystemInfo.maxRenderTextureSize, 8192f);
      if ((double) vector2.x > (double) num3 || (double) vector2.y > (double) num3)
      {
        if ((double) vector2.x > (double) vector2.y)
          vector2 *= num3 / vector2.x;
        else
          vector2 *= num3 / vector2.y;
        clamped = true;
      }
      return vector2;
    }

    private static void RefreshGameViewSizeGroupType(BuildTarget oldTarget, BuildTarget newTarget)
    {
      GameViewSizes.s_GameViewSizeGroupType = GameViewSizes.BuildTargetGroupToGameViewSizeGroup(BuildPipeline.GetBuildTargetGroup(newTarget));
    }

    public static GameViewSizeGroupType BuildTargetGroupToGameViewSizeGroup(BuildTargetGroup buildTargetGroup)
    {
      if (XRSettings.enabled && XRSettings.showDeviceView)
        return GameViewSizeGroupType.HMD;
      switch (buildTargetGroup)
      {
        case BuildTargetGroup.Standalone:
          return GameViewSizeGroupType.Standalone;
        case BuildTargetGroup.iPhone:
          return GameViewSizeGroupType.iOS;
        default:
          if (buildTargetGroup == BuildTargetGroup.N3DS)
            return GameViewSizeGroupType.N3DS;
          if (buildTargetGroup == BuildTargetGroup.WiiU)
            return GameViewSizeGroupType.WiiU;
          if (buildTargetGroup == BuildTargetGroup.Android)
            return GameViewSizeGroupType.Android;
          return buildTargetGroup == BuildTargetGroup.Tizen ? GameViewSizeGroupType.Tizen : GameViewSizeGroupType.Standalone;
      }
    }

    private class BuildTargetChangedHandler : IActiveBuildTargetChanged, IOrderedCallback
    {
      public int callbackOrder
      {
        get
        {
          return 0;
        }
      }

      public void OnActiveBuildTargetChanged(BuildTarget oldTarget, BuildTarget newTarget)
      {
        GameViewSizes.RefreshGameViewSizeGroupType(oldTarget, newTarget);
      }
    }
  }
}
