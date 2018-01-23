// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.SpriteAtlasManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.U2D
{
  /// <summary>
  ///   <para>Manages SpriteAtlas during runtime.</para>
  /// </summary>
  public sealed class SpriteAtlasManager
  {
    public static event SpriteAtlasManager.RequestAtlasCallback atlasRequested = null;

    [RequiredByNativeCode]
    private static bool RequestAtlas(string tag)
    {
      // ISSUE: reference to a compiler-generated field
      if (SpriteAtlasManager.atlasRequested == null)
        return false;
      // ISSUE: reference to a compiler-generated field
      SpriteAtlasManager.RequestAtlasCallback atlasRequested = SpriteAtlasManager.atlasRequested;
      string tag1 = tag;
      // ISSUE: reference to a compiler-generated field
      if (SpriteAtlasManager.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SpriteAtlasManager.\u003C\u003Ef__mg\u0024cache0 = new Action<SpriteAtlas>(SpriteAtlasManager.Register);
      }
      // ISSUE: reference to a compiler-generated field
      Action<SpriteAtlas> fMgCache0 = SpriteAtlasManager.\u003C\u003Ef__mg\u0024cache0;
      atlasRequested(tag1, fMgCache0);
      return true;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Register(SpriteAtlas spriteAtlas);

    /// <summary>
    ///   <para>Delegate type for atlas request callback.</para>
    /// </summary>
    /// <param name="tag">Tag of SpriteAtlas that needs to be provided by user.</param>
    /// <param name="action">An Action that takes user loaded SpriteAtlas.</param>
    public delegate void RequestAtlasCallback(string tag, Action<SpriteAtlas> action);
  }
}
