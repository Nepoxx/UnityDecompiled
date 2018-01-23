// Decompiled with JetBrains decompiler
// Type: UnityEditor.LegacyIlluminShaderGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LegacyIlluminShaderGUI : ShaderGUI
  {
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
      base.OnGUI(materialEditor, props);
      materialEditor.LightmapEmissionProperty(0);
      foreach (Material target in materialEditor.targets)
        target.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }
  }
}
