// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorPluginImporterExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Modules;
using UnityEngine;

namespace UnityEditor
{
  internal class EditorPluginImporterExtension : DefaultPluginImporterExtension
  {
    public EditorPluginImporterExtension()
      : base(EditorPluginImporterExtension.GetProperties())
    {
    }

    private static DefaultPluginImporterExtension.Property[] GetProperties()
    {
      return (DefaultPluginImporterExtension.Property[]) new EditorPluginImporterExtension.EditorProperty[2]{ new EditorPluginImporterExtension.EditorProperty(EditorGUIUtility.TextContent("CPU|Is plugin compatible with 32bit or 64bit Editor?"), "CPU", (object) EditorPluginImporterExtension.EditorPluginCPUArchitecture.AnyCPU), new EditorPluginImporterExtension.EditorProperty(EditorGUIUtility.TextContent("OS|Is plugin compatible with Windows, OS X or Linux Editor?"), "OS", (object) EditorPluginImporterExtension.EditorPluginOSArchitecture.AnyOS) };
    }

    internal enum EditorPluginCPUArchitecture
    {
      AnyCPU,
      x86,
      x86_64,
    }

    internal enum EditorPluginOSArchitecture
    {
      AnyOS,
      OSX,
      Windows,
      Linux,
    }

    internal class EditorProperty : DefaultPluginImporterExtension.Property
    {
      public EditorProperty(GUIContent name, string key, object defaultValue)
        : base(name, key, defaultValue, BuildPipeline.GetEditorTargetName())
      {
      }

      internal override void Reset(PluginImporterInspector inspector)
      {
        string editorData = inspector.importer.GetEditorData(this.key);
        this.ParseStringValue(inspector, editorData, false);
      }

      internal override void Apply(PluginImporterInspector inspector)
      {
        inspector.importer.SetEditorData(this.key, this.value.ToString());
      }
    }
  }
}
