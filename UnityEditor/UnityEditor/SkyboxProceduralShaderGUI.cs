// Decompiled with JetBrains decompiler
// Type: UnityEditor.SkyboxProceduralShaderGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SkyboxProceduralShaderGUI : ShaderGUI
  {
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
      materialEditor.SetDefaultGUIWidths();
      SkyboxProceduralShaderGUI.SunDiskMode floatValue = (SkyboxProceduralShaderGUI.SunDiskMode) ShaderGUI.FindProperty("_SunDisk", props).floatValue;
      for (int index = 0; index < props.Length; ++index)
      {
        if ((props[index].flags & (MaterialProperty.PropFlags.HideInInspector | MaterialProperty.PropFlags.PerRendererData)) == MaterialProperty.PropFlags.None && (!(props[index].name == "_SunSizeConvergence") || floatValue == SkyboxProceduralShaderGUI.SunDiskMode.HighQuality))
        {
          Rect controlRect = EditorGUILayout.GetControlRect(true, materialEditor.GetPropertyHeight(props[index], props[index].displayName), EditorStyles.layerMaskField, new GUILayoutOption[0]);
          materialEditor.ShaderProperty(controlRect, props[index], props[index].displayName);
        }
      }
    }

    private enum SunDiskMode
    {
      None,
      Simple,
      HighQuality,
    }
  }
}
