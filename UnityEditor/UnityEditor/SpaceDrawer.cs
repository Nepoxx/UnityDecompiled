// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpaceDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (SpaceAttribute))]
  internal sealed class SpaceDrawer : DecoratorDrawer
  {
    public override float GetHeight()
    {
      return (this.attribute as SpaceAttribute).height;
    }
  }
}
