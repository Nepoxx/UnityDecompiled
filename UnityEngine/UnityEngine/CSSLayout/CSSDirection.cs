// Decompiled with JetBrains decompiler
// Type: UnityEngine.CSSLayout.CSSDirection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.CSSLayout
{
  internal enum CSSDirection
  {
    Inherit = 0,
    LTR = 1,
    [Obsolete("Use LTR instead")] LeftToRight = 1,
    RTL = 2,
    [Obsolete("Use RTL instead")] RightToLeft = 2,
  }
}
