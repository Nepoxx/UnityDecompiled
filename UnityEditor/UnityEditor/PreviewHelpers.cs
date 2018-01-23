// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreviewHelpers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PreviewHelpers
  {
    internal static void AdjustWidthAndHeightForStaticPreview(int textureWidth, int textureHeight, ref int width, ref int height)
    {
      int max1 = width;
      int max2 = height;
      if (textureWidth <= width && textureHeight <= height)
      {
        width = textureWidth;
        height = textureHeight;
      }
      else
      {
        float b = (float) height / (float) textureWidth;
        float num = Mathf.Min((float) width / (float) textureHeight, b);
        width = Mathf.RoundToInt((float) textureWidth * num);
        height = Mathf.RoundToInt((float) textureHeight * num);
      }
      width = Mathf.Clamp(width, 2, max1);
      height = Mathf.Clamp(height, 2, max2);
    }
  }
}
