// Decompiled with JetBrains decompiler
// Type: UnityEngine.UICharInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class that specifies some information about a renderable character.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct UICharInfo
  {
    /// <summary>
    ///   <para>Position of the character cursor in local (text generated) space.</para>
    /// </summary>
    public Vector2 cursorPos;
    /// <summary>
    ///   <para>Character width.</para>
    /// </summary>
    public float charWidth;
  }
}
