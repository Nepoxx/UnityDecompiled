// Decompiled with JetBrains decompiler
// Type: UnityEditor.HeaderDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (HeaderAttribute))]
  internal sealed class HeaderDrawer : DecoratorDrawer
  {
    public override void OnGUI(Rect position)
    {
      position.y += 8f;
      position = EditorGUI.IndentedRect(position);
      GUI.Label(position, (this.attribute as HeaderAttribute).header, EditorStyles.boldLabel);
    }

    public override float GetHeight()
    {
      return 24f;
    }
  }
}
