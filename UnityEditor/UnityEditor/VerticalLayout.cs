// Decompiled with JetBrains decompiler
// Type: UnityEditor.VerticalLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class VerticalLayout : IDisposable
  {
    private static readonly VerticalLayout instance = new VerticalLayout();

    private VerticalLayout()
    {
    }

    public static IDisposable DoLayout()
    {
      GUILayout.BeginVertical();
      return (IDisposable) VerticalLayout.instance;
    }

    void IDisposable.Dispose()
    {
      GUILayout.EndVertical();
    }
  }
}
