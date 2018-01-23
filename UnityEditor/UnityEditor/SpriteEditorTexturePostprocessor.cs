// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteEditorTexturePostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SpriteEditorTexturePostprocessor : AssetPostprocessor
  {
    public override int GetPostprocessOrder()
    {
      return 1;
    }

    public void OnPostprocessTexture(Texture2D tex)
    {
      if (!((Object) SpriteEditorWindow.s_Instance != (Object) null) || !this.assetPath.Equals(SpriteEditorWindow.s_Instance.m_SelectedAssetPath))
        return;
      if (!SpriteEditorWindow.s_Instance.m_IgnoreNextPostprocessEvent)
        SpriteEditorWindow.s_Instance.m_ResetOnNextRepaint = true;
      else
        SpriteEditorWindow.s_Instance.m_IgnoreNextPostprocessEvent = false;
      SpriteEditorWindow.s_Instance.Repaint();
    }
  }
}
