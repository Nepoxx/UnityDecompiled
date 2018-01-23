// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationClipSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  public sealed class AnimationClipSettings
  {
    public AnimationClip additiveReferencePoseClip;
    public float additiveReferencePoseTime;
    public float startTime;
    public float stopTime;
    public float orientationOffsetY;
    public float level;
    public float cycleOffset;
    public bool hasAdditiveReferencePose;
    public bool loopTime;
    public bool loopBlend;
    public bool loopBlendOrientation;
    public bool loopBlendPositionY;
    public bool loopBlendPositionXZ;
    public bool keepOriginalOrientation;
    public bool keepOriginalPositionY;
    public bool keepOriginalPositionXZ;
    public bool heightFromFeet;
    public bool mirror;
  }
}
