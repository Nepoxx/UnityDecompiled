// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.BlendTreePreviewUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  public sealed class BlendTreePreviewUtility
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetRootBlendTreeChildWeights(Animator animator, int layerIndex, int stateHash, float[] weightArray);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CalculateRootBlendTreeChildWeights(Animator animator, int layerIndex, int stateHash, float[] weightArray, float blendX, float blendY);

    public static void CalculateBlendTexture(Animator animator, int layerIndex, int stateHash, Texture2D blendTexture, Texture2D[] weightTextures, Rect rect)
    {
      BlendTreePreviewUtility.INTERNAL_CALL_CalculateBlendTexture(animator, layerIndex, stateHash, blendTexture, weightTextures, ref rect);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateBlendTexture(Animator animator, int layerIndex, int stateHash, Texture2D blendTexture, Texture2D[] weightTextures, ref Rect rect);
  }
}
