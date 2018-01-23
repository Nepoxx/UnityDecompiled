// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShapeEditorFactory
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal class ShapeEditorFactory : IShapeEditorFactory
  {
    public ShapeEditor CreateShapeEditor()
    {
      return new ShapeEditor((IGUIUtility) new GUIUtilitySystem(), (IEventSystem) new EventSystem());
    }
  }
}
