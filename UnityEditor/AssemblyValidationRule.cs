// Decompiled with JetBrains decompiler
// Type: AssemblyValidationRule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class AssemblyValidationRule : Attribute
{
  public int Priority;
  private readonly RuntimePlatform _platform;

  public AssemblyValidationRule(RuntimePlatform platform)
  {
    this._platform = platform;
    this.Priority = 0;
  }

  public RuntimePlatform Platform
  {
    get
    {
      return this._platform;
    }
  }
}
