// Decompiled with JetBrains decompiler
// Type: UnityEditor.FontInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Font))]
  [CanEditMultipleObjects]
  internal class FontInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      foreach (Object target in this.targets)
      {
        if (target.hideFlags == HideFlags.NotEditable)
          return;
      }
      this.DrawDefaultInspector();
    }
  }
}
