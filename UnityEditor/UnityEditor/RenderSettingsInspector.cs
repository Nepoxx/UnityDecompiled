// Decompiled with JetBrains decompiler
// Type: UnityEditor.RenderSettingsInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderSettings))]
  internal class RenderSettingsInspector : Editor
  {
    private Editor m_LightingEditor;
    private Editor m_FogEditor;
    private Editor m_OtherRenderingEditor;

    private Editor lightingEditor
    {
      get
      {
        return this.m_LightingEditor ?? (this.m_LightingEditor = Editor.CreateEditor(this.target, typeof (LightingEditor)));
      }
    }

    private Editor fogEditor
    {
      get
      {
        return this.m_FogEditor ?? (this.m_FogEditor = Editor.CreateEditor(this.target, typeof (FogEditor)));
      }
    }

    private Editor otherRenderingEditor
    {
      get
      {
        return this.m_OtherRenderingEditor ?? (this.m_OtherRenderingEditor = Editor.CreateEditor(this.target, typeof (OtherRenderingEditor)));
      }
    }

    public virtual void OnEnable()
    {
      this.m_LightingEditor = (Editor) null;
      this.m_FogEditor = (Editor) null;
      this.m_OtherRenderingEditor = (Editor) null;
    }

    public override void OnInspectorGUI()
    {
      this.lightingEditor.OnInspectorGUI();
      this.fogEditor.OnInspectorGUI();
      this.otherRenderingEditor.OnInspectorGUI();
    }
  }
}
