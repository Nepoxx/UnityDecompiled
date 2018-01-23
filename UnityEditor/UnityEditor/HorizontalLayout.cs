// Decompiled with JetBrains decompiler
// Type: UnityEditor.HorizontalLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class HorizontalLayout : IDisposable
  {
    private static readonly HorizontalLayout instance = new HorizontalLayout();

    private HorizontalLayout()
    {
    }

    public static IDisposable DoLayout()
    {
      GUILayout.BeginHorizontal();
      return (IDisposable) HorizontalLayout.instance;
    }

    void IDisposable.Dispose()
    {
      GUILayout.EndHorizontal();
    }
  }
}
