// Decompiled with JetBrains decompiler
// Type: UnityEngine.Internal_DrawWithTextSelectionArguments
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal struct Internal_DrawWithTextSelectionArguments
  {
    public IntPtr target;
    public Rect position;
    public int firstPos;
    public int lastPos;
    public Color cursorColor;
    public Color selectionColor;
    public int isHover;
    public int isActive;
    public int on;
    public int hasKeyboardFocus;
    public int drawSelectionAsComposition;
  }
}
