// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedRenderTargetState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SavedRenderTargetState
  {
    private RenderTexture renderTexture;
    private Rect viewport;
    private Rect scissor;

    internal SavedRenderTargetState()
    {
      GL.PushMatrix();
      if (ShaderUtil.hardwareSupportsRectRenderTexture)
        this.renderTexture = RenderTexture.active;
      this.viewport = ShaderUtil.rawViewportRect;
      this.scissor = ShaderUtil.rawScissorRect;
    }

    internal void Restore()
    {
      if (ShaderUtil.hardwareSupportsRectRenderTexture)
        EditorGUIUtility.SetRenderTextureNoViewport(this.renderTexture);
      ShaderUtil.rawViewportRect = this.viewport;
      ShaderUtil.rawScissorRect = this.scissor;
      GL.PopMatrix();
    }
  }
}
