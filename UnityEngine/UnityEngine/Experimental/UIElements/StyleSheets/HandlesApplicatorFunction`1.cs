// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.StyleSheets.HandlesApplicatorFunction`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
  internal delegate void HandlesApplicatorFunction<T>(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<T> property);
}
