// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSDeviceRequirement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  /// <summary>
  ///   <para>A device requirement description used for configuration of App Slicing.</para>
  /// </summary>
  public sealed class iOSDeviceRequirement
  {
    private SortedDictionary<string, string> m_Values = new SortedDictionary<string, string>();

    /// <summary>
    ///   <para>The values of the device requirement description.</para>
    /// </summary>
    public IDictionary<string, string> values
    {
      get
      {
        return (IDictionary<string, string>) this.m_Values;
      }
    }
  }
}
