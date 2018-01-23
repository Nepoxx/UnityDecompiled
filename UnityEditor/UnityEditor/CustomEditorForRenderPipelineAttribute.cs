// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomEditorForRenderPipelineAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Tells an Editor class which run-time type it's an editor for when the given RenderPipeline is activated.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class CustomEditorForRenderPipelineAttribute : CustomEditor
  {
    internal System.Type renderPipelineType;

    /// <summary>
    ///   <para>Defines which object type the custom editor class can edit.</para>
    /// </summary>
    /// <param name="inspectedType">Type that this editor can edit.</param>
    /// <param name="renderPipeline">Type of RenderPipelineAsset that that should be active for this inspector to be used.</param>
    /// <param name="editorForChildClasses">If true, child classes of inspectedType will also show this editor. Defaults to false.</param>
    public CustomEditorForRenderPipelineAttribute(System.Type inspectedType, System.Type renderPipeline)
      : base(inspectedType)
    {
      this.renderPipelineType = renderPipeline;
    }

    /// <summary>
    ///   <para>Defines which object type the custom editor class can edit.</para>
    /// </summary>
    /// <param name="inspectedType">Type that this editor can edit.</param>
    /// <param name="renderPipeline">Type of RenderPipelineAsset that that should be active for this inspector to be used.</param>
    /// <param name="editorForChildClasses">If true, child classes of inspectedType will also show this editor. Defaults to false.</param>
    public CustomEditorForRenderPipelineAttribute(System.Type inspectedType, System.Type renderPipeline, bool editorForChildClasses)
      : base(inspectedType, editorForChildClasses)
    {
      this.renderPipelineType = renderPipeline;
    }
  }
}
