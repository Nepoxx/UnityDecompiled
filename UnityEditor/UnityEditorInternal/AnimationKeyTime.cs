// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationKeyTime
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal struct AnimationKeyTime
  {
    [SerializeField]
    private float m_FrameRate;
    [SerializeField]
    private int m_Frame;
    [SerializeField]
    private float m_Time;

    public float time
    {
      get
      {
        return this.m_Time;
      }
    }

    public int frame
    {
      get
      {
        return this.m_Frame;
      }
    }

    public float frameRate
    {
      get
      {
        return this.m_FrameRate;
      }
    }

    public float frameFloor
    {
      get
      {
        return ((float) this.frame - 0.5f) / this.frameRate;
      }
    }

    public float frameCeiling
    {
      get
      {
        return ((float) this.frame + 0.5f) / this.frameRate;
      }
    }

    public static AnimationKeyTime Time(float time, float frameRate)
    {
      AnimationKeyTime animationKeyTime = new AnimationKeyTime();
      animationKeyTime.m_Time = Mathf.Max(time, 0.0f);
      animationKeyTime.m_FrameRate = frameRate;
      animationKeyTime.m_Frame = Mathf.RoundToInt(animationKeyTime.m_Time * frameRate);
      return animationKeyTime;
    }

    public static AnimationKeyTime Frame(int frame, float frameRate)
    {
      AnimationKeyTime animationKeyTime = new AnimationKeyTime();
      animationKeyTime.m_Frame = frame >= 0 ? frame : 0;
      animationKeyTime.m_Time = (float) animationKeyTime.m_Frame / frameRate;
      animationKeyTime.m_FrameRate = frameRate;
      return animationKeyTime;
    }

    public bool ContainsTime(float time)
    {
      return (double) time >= (double) this.frameFloor && (double) time < (double) this.frameCeiling;
    }

    public bool Equals(AnimationKeyTime key)
    {
      return this.m_Frame == key.m_Frame && (double) this.m_FrameRate == (double) key.m_FrameRate && Mathf.Approximately(this.m_Time, key.m_Time);
    }
  }
}
