// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Text file assets.</para>
  /// </summary>
  public class TextAsset : Object
  {
    public TextAsset()
    {
      TextAsset.Internal_CreateTextAsset(this);
    }

    /// <summary>
    ///   <para>The text contents of the .txt file as a string. (Read Only)</para>
    /// </summary>
    public extern string text { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The raw bytes of the text asset. (Read Only)</para>
    /// </summary>
    public extern byte[] bytes { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the contents of the TextAsset.</para>
    /// </summary>
    public override string ToString()
    {
      return this.text;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateTextAsset([Writable] TextAsset mono);
  }
}
