// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (ParticleRenderer))]
  internal class ParticleRendererEditor : RendererEditorBase
  {
    public override void OnEnable()
    {
      base.OnEnable();
      this.InitializeProbeFields();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Editor.DrawPropertiesExcluding(this.serializedObject, RendererEditorBase.Probes.GetFieldsStringArray());
      this.m_Probes.OnGUI(this.targets, (Renderer) this.target, false);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
