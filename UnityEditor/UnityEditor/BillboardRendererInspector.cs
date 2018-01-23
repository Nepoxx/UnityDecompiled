// Decompiled with JetBrains decompiler
// Type: UnityEditor.BillboardRendererInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (BillboardRenderer))]
  internal class BillboardRendererInspector : RendererEditorBase
  {
    private string[] m_ExcludedProperties;

    public override void OnEnable()
    {
      base.OnEnable();
      this.InitializeProbeFields();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) new string[2]
      {
        "m_Materials",
        "m_LightmapParameters"
      });
      stringList.AddRange((IEnumerable<string>) RendererEditorBase.Probes.GetFieldsStringArray());
      this.m_ExcludedProperties = stringList.ToArray();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Editor.DrawPropertiesExcluding(this.serializedObject, this.m_ExcludedProperties);
      this.m_Probes.OnGUI(this.targets, (Renderer) this.target, false);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
