// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SpriteEditorMenuSetting
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class SpriteEditorMenuSetting : ScriptableObject
  {
    [SerializeField]
    public Vector2 gridCellCount = new Vector2(1f, 1f);
    [SerializeField]
    public Vector2 gridSpriteSize = new Vector2(64f, 64f);
    [SerializeField]
    public Vector2 gridSpriteOffset = new Vector2(0.0f, 0.0f);
    [SerializeField]
    public Vector2 gridSpritePadding = new Vector2(0.0f, 0.0f);
    [SerializeField]
    public Vector2 pivot = Vector2.zero;
    [SerializeField]
    public int autoSlicingMethod = 0;
    [SerializeField]
    public int spriteAlignment;
    [SerializeField]
    public SpriteEditorMenuSetting.SlicingType slicingType;

    public enum SlicingType
    {
      Automatic,
      GridByCellSize,
      GridByCellCount,
    }
  }
}
