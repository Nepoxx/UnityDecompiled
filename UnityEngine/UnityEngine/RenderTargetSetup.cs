// Decompiled with JetBrains decompiler
// Type: UnityEngine.RenderTargetSetup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Rendering;

namespace UnityEngine
{
  public struct RenderTargetSetup
  {
    public RenderBuffer[] color;
    public RenderBuffer depth;
    public int mipLevel;
    public CubemapFace cubemapFace;
    public int depthSlice;
    public RenderBufferLoadAction[] colorLoad;
    public RenderBufferStoreAction[] colorStore;
    public RenderBufferLoadAction depthLoad;
    public RenderBufferStoreAction depthStore;

    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, RenderBufferLoadAction[] colorLoad, RenderBufferStoreAction[] colorStore, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore)
    {
      this.color = color;
      this.depth = depth;
      this.mipLevel = mip;
      this.cubemapFace = face;
      this.depthSlice = 0;
      this.colorLoad = colorLoad;
      this.colorStore = colorStore;
      this.depthLoad = depthLoad;
      this.depthStore = depthStore;
    }

    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]
      {
        color
      }, depth);
    }

    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]
      {
        color
      }, depth, mipLevel);
    }

    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]
      {
        color
      }, depth, mipLevel, face);
    }

    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face, int depthSlice)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]
      {
        color
      }, depth, mipLevel, face);
      this.depthSlice = depthSlice;
    }

    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth)
    {
      this = new RenderTargetSetup(color, depth, 0, CubemapFace.Unknown);
    }

    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mipLevel)
    {
      this = new RenderTargetSetup(color, depth, mipLevel, CubemapFace.Unknown);
    }

    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face)
    {
      this = new RenderTargetSetup(color, depth, mip, face, RenderTargetSetup.LoadActions(color), RenderTargetSetup.StoreActions(color), depth.loadAction, depth.storeAction);
    }

    internal static RenderBufferLoadAction[] LoadActions(RenderBuffer[] buf)
    {
      RenderBufferLoadAction[] bufferLoadActionArray = new RenderBufferLoadAction[buf.Length];
      for (int index = 0; index < buf.Length; ++index)
      {
        bufferLoadActionArray[index] = buf[index].loadAction;
        buf[index].loadAction = RenderBufferLoadAction.Load;
      }
      return bufferLoadActionArray;
    }

    internal static RenderBufferStoreAction[] StoreActions(RenderBuffer[] buf)
    {
      RenderBufferStoreAction[] bufferStoreActionArray = new RenderBufferStoreAction[buf.Length];
      for (int index = 0; index < buf.Length; ++index)
      {
        bufferStoreActionArray[index] = buf[index].storeAction;
        buf[index].storeAction = RenderBufferStoreAction.Store;
      }
      return bufferStoreActionArray;
    }
  }
}
