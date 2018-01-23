// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.U2D.ISpriteEditorDataProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
  internal interface ISpriteEditorDataProvider
  {
    SpriteImportMode spriteImportMode { get; }

    int spriteDataCount { get; set; }

    SpriteDataBase GetSpriteData(int i);

    void Apply(SerializedObject so);

    Object targetObject { get; }

    void GetTextureActualWidthAndHeight(out int width, out int height);

    void InitSpriteEditorDataProvider(SerializedObject so);
  }
}
