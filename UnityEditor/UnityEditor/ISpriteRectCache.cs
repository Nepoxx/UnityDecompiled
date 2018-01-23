// Decompiled with JetBrains decompiler
// Type: UnityEditor.ISpriteRectCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.U2D.Interface;

namespace UnityEditor
{
  internal interface ISpriteRectCache : IUndoableObject
  {
    int Count { get; }

    SpriteRect RectAt(int i);

    void AddRect(SpriteRect r);

    void RemoveRect(SpriteRect r);

    void ClearAll();

    int GetIndex(SpriteRect spriteRect);

    bool Contains(SpriteRect spriteRect);
  }
}
