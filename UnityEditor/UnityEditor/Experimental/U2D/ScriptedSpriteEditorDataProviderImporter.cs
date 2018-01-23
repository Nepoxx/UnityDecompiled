// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.U2D.ScriptedSpriteEditorDataProviderImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
  internal abstract class ScriptedSpriteEditorDataProviderImporter : ScriptedImporter, ISpriteEditorDataProvider
  {
    public abstract SpriteImportMode spriteImportMode { get; }

    public abstract int spriteDataCount { get; set; }

    public abstract SpriteDataBase GetSpriteData(int i);

    public abstract void Apply(SerializedObject so);

    public abstract Object targetObject { get; }

    public abstract void GetTextureActualWidthAndHeight(out int width, out int height);

    public abstract void InitSpriteEditorDataProvider(SerializedObject so);
  }
}
