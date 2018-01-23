// Decompiled with JetBrains decompiler
// Type: UnityEditor.ISpriteEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Experimental.U2D;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal interface ISpriteEditor
  {
    ISpriteRectCache spriteRects { get; }

    SpriteRect selectedSpriteRect { get; set; }

    bool enableMouseMoveEvent { set; }

    bool editingDisabled { get; }

    Rect windowDimension { get; }

    ITexture2D selectedTexture { get; }

    ITexture2D previewTexture { get; }

    ISpriteEditorDataProvider spriteEditorDataProvider { get; }

    void HandleSpriteSelection();

    void RequestRepaint();

    void SetDataModified();

    void DisplayProgressBar(string title, string content, float progress);

    void ClearProgressBar();

    ITexture2D GetReadableTexture2D();

    void ApplyOrRevertModification(bool apply);
  }
}
