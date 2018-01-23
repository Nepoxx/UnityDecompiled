// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorWindowTitleAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class EditorWindowTitleAttribute : Attribute
  {
    public string title { get; set; }

    public string icon { get; set; }

    public bool useTypeNameAsIconName { get; set; }
  }
}
