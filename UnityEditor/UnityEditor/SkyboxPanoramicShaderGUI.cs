// Decompiled with JetBrains decompiler
// Type: UnityEditor.SkyboxPanoramicShaderGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditorInternal.VR;
using UnityEngine.Events;

namespace UnityEditor
{
  internal class SkyboxPanoramicShaderGUI : ShaderGUI
  {
    private readonly AnimBool m_ShowLatLongLayout = new AnimBool();
    private readonly AnimBool m_ShowMirrorOnBack = new AnimBool();
    private readonly AnimBool m_Show3DControl = new AnimBool();
    private bool m_Initialized = false;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
      if (!this.m_Initialized)
      {
        this.m_ShowLatLongLayout.valueChanged.AddListener(new UnityAction(((Editor) materialEditor).Repaint));
        this.m_ShowMirrorOnBack.valueChanged.AddListener(new UnityAction(((Editor) materialEditor).Repaint));
        this.m_Show3DControl.valueChanged.AddListener(new UnityAction(((Editor) materialEditor).Repaint));
        this.m_Initialized = true;
      }
      float labelWidth = EditorGUIUtility.labelWidth;
      materialEditor.SetDefaultGUIWidths();
      double num1 = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_Tint", props));
      double num2 = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_Exposure", props));
      double num3 = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_Rotation", props));
      double num4 = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_MainTex", props));
      EditorGUIUtility.labelWidth = labelWidth;
      this.m_ShowLatLongLayout.target = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_Mapping", props)) == 1.0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowLatLongLayout.faded))
      {
        this.m_ShowMirrorOnBack.target = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_ImageType", props)) == 1.0;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowMirrorOnBack.faded))
        {
          ++EditorGUI.indentLevel;
          double num5 = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_MirrorOnBack", props));
          --EditorGUI.indentLevel;
        }
        EditorGUILayout.EndFadeGroup();
        this.m_Show3DControl.value = false;
        foreach (BuildPlatform buildPlatform in BuildPlatforms.instance.buildPlatforms)
        {
          if (VREditor.GetVREnabledOnTargetGroup(buildPlatform.targetGroup))
          {
            this.m_Show3DControl.value = true;
            break;
          }
        }
        if (EditorGUILayout.BeginFadeGroup(this.m_Show3DControl.faded))
        {
          double num6 = (double) this.ShowProp(materialEditor, ShaderGUI.FindProperty("_Layout", props));
        }
        EditorGUILayout.EndFadeGroup();
      }
      EditorGUILayout.EndFadeGroup();
      materialEditor.PropertiesDefaultGUI(new MaterialProperty[0]);
    }

    private float ShowProp(MaterialEditor materialEditor, MaterialProperty prop)
    {
      materialEditor.ShaderProperty(prop, prop.displayName);
      return prop.floatValue;
    }
  }
}
