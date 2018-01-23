// Decompiled with JetBrains decompiler
// Type: UnityEngine.Internal_DrawTextureArguments
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal struct Internal_DrawTextureArguments
  {
    public Rect screenRect;
    public Rect sourceRect;
    public int leftBorder;
    public int rightBorder;
    public int topBorder;
    public int bottomBorder;
    public Color32 color;
    public Vector4 borderWidths;
    public Vector4 cornerRadiuses;
    public int pass;
    public Texture texture;
    public Material mat;
  }
}
