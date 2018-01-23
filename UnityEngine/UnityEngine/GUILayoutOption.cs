// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayoutOption
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class internally used to pass layout options into GUILayout functions. You don't use these directly, but construct them with the layouting functions in the GUILayout class.</para>
  /// </summary>
  public sealed class GUILayoutOption
  {
    internal GUILayoutOption.Type type;
    internal object value;

    internal GUILayoutOption(GUILayoutOption.Type type, object value)
    {
      this.type = type;
      this.value = value;
    }

    internal enum Type
    {
      fixedWidth,
      fixedHeight,
      minWidth,
      maxWidth,
      minHeight,
      maxHeight,
      stretchWidth,
      stretchHeight,
      alignStart,
      alignMiddle,
      alignEnd,
      alignJustify,
      equalSize,
      spacing,
    }
  }
}
