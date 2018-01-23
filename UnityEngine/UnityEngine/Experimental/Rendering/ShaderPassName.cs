// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.ShaderPassName
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
  /// <summary>
  ///   <para>Shader pass name identifier.</para>
  /// </summary>
  public struct ShaderPassName
  {
    private int m_NameIndex;

    /// <summary>
    ///   <para>Create shader pass name identifier.</para>
    /// </summary>
    /// <param name="name">Pass name.</param>
    public ShaderPassName(string name)
    {
      this.m_NameIndex = ShaderPassName.Init(name);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Init(string name);

    internal int nameIndex
    {
      get
      {
        return this.m_NameIndex;
      }
    }
  }
}
