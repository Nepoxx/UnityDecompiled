// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.U2D.SpriteUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
  internal static class SpriteUtility
  {
    public static void GenerateOutline(Texture2D texture, Rect rect, float detail, byte alphaTolerance, bool holeDetection, out Vector2[][] paths)
    {
      UnityEditor.Sprites.SpriteUtility.GenerateOutline(texture, rect, detail, alphaTolerance, holeDetection, out paths);
    }
  }
}
