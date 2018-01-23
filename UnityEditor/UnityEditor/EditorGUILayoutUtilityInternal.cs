// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorGUILayoutUtilityInternal
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal sealed class EditorGUILayoutUtilityInternal : GUILayoutUtility
  {
    internal new static GUILayoutGroup BeginLayoutArea(GUIStyle style, System.Type LayoutType)
    {
      return GUILayoutUtility.DoBeginLayoutArea(style, LayoutType);
    }

    internal new static GUILayoutGroup topLevel
    {
      get
      {
        return GUILayoutUtility.topLevel;
      }
    }
  }
}
