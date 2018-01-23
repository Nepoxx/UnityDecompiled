// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tilemaps.TileAnimationData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
  [RequiredByNativeCode]
  [NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
  public struct TileAnimationData
  {
    private Sprite[] m_AnimatedSprites;
    private float m_AnimationSpeed;
    private float m_AnimationStartTime;

    public Sprite[] animatedSprites
    {
      get
      {
        return this.m_AnimatedSprites;
      }
      set
      {
        this.m_AnimatedSprites = value;
      }
    }

    public float animationSpeed
    {
      get
      {
        return this.m_AnimationSpeed;
      }
      set
      {
        this.m_AnimationSpeed = value;
      }
    }

    public float animationStartTime
    {
      get
      {
        return this.m_AnimationStartTime;
      }
      set
      {
        this.m_AnimationStartTime = value;
      }
    }
  }
}
