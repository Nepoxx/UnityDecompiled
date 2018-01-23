// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tilemaps.TilemapRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps
{
  [RequireComponent(typeof (Tilemap))]
  [NativeType(Header = "Modules/Tilemap/Public/TilemapRenderer.h")]
  public sealed class TilemapRenderer : Renderer
  {
    public Vector3Int chunkSize
    {
      get
      {
        Vector3Int ret;
        this.get_chunkSize_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_chunkSize_Injected(ref value);
      }
    }

    public extern int maxChunkCount { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern int maxFrameAge { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern TilemapRenderer.SortOrder sortOrder { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern SpriteMaskInteraction maskInteraction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_chunkSize_Injected(out Vector3Int ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_chunkSize_Injected(ref Vector3Int value);

    public enum SortOrder
    {
      BottomLeft,
      BottomRight,
      TopLeft,
      TopRight,
    }
  }
}
