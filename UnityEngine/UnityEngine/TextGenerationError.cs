// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextGenerationError
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  internal enum TextGenerationError
  {
    None = 0,
    CustomSizeOnNonDynamicFont = 1,
    CustomStyleOnNonDynamicFont = 2,
    NoFont = 4,
  }
}
