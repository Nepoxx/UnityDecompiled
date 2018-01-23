// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExternalVersionControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  public struct ExternalVersionControl
  {
    public static readonly string Disabled = "Hidden Meta Files";
    public static readonly string AutoDetect = "Auto detect";
    public static readonly string Generic = "Visible Meta Files";
    [Obsolete("Asset Server VCS support has been removed.")]
    public static readonly string AssetServer = "Asset Server";
    private string m_Value;

    public ExternalVersionControl(string value)
    {
      this.m_Value = value;
    }

    public static implicit operator string(ExternalVersionControl d)
    {
      return d.ToString();
    }

    public static implicit operator ExternalVersionControl(string d)
    {
      return new ExternalVersionControl(d);
    }

    public override string ToString()
    {
      return this.m_Value;
    }
  }
}
