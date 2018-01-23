// Decompiled with JetBrains decompiler
// Type: UnityEditor.AlphabeticalSorting
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AlphabeticalSorting : HierarchySorting
  {
    private readonly GUIContent m_Content = new GUIContent((Texture) EditorGUIUtility.FindTexture(nameof (AlphabeticalSorting)), "Alphabetical Order");

    public override GUIContent content
    {
      get
      {
        return this.m_Content;
      }
    }
  }
}
