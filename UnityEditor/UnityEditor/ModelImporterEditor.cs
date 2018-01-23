// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (ModelImporter))]
  [CanEditMultipleObjects]
  internal class ModelImporterEditor : AssetImporterTabbedEditor
  {
    public override void OnEnable()
    {
      if (this.tabs == null)
      {
        this.tabs = new BaseAssetImporterTabUI[4]
        {
          (BaseAssetImporterTabUI) new ModelImporterModelEditor((AssetImporterEditor) this),
          (BaseAssetImporterTabUI) new ModelImporterRigEditor((AssetImporterEditor) this),
          (BaseAssetImporterTabUI) new ModelImporterClipEditor((AssetImporterEditor) this),
          (BaseAssetImporterTabUI) new ModelImporterMaterialEditor((AssetImporterEditor) this)
        };
        this.m_TabNames = new string[4]
        {
          "Model",
          "Rig",
          "Animation",
          "Materials"
        };
      }
      base.OnEnable();
    }

    public override void OnDisable()
    {
      foreach (BaseAssetImporterTabUI tab in this.tabs)
        tab.OnDisable();
      base.OnDisable();
    }

    public override bool HasPreviewGUI()
    {
      return base.HasPreviewGUI() && this.targets.Length < 2;
    }

    public override GUIContent GetPreviewTitle()
    {
      ModelImporterClipEditor activeTab = this.activeTab as ModelImporterClipEditor;
      if (activeTab != null)
        return new GUIContent(activeTab.selectedClipName);
      return base.GetPreviewTitle();
    }

    public override bool showImportedObject
    {
      get
      {
        return this.activeTab is ModelImporterModelEditor;
      }
    }
  }
}
