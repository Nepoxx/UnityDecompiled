// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialToggleDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class MaterialToggleDrawer : MaterialPropertyDrawer
  {
    protected readonly string keyword;

    public MaterialToggleDrawer()
    {
    }

    public MaterialToggleDrawer(string keyword)
    {
      this.keyword = keyword;
    }

    private static bool IsPropertyTypeSuitable(MaterialProperty prop)
    {
      return prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range;
    }

    protected virtual void SetKeyword(MaterialProperty prop, bool on)
    {
      this.SetKeywordInternal(prop, on, "_ON");
    }

    protected void SetKeywordInternal(MaterialProperty prop, bool on, string defaultKeywordSuffix)
    {
      string keyword = !string.IsNullOrEmpty(this.keyword) ? this.keyword : prop.name.ToUpperInvariant() + defaultKeywordSuffix;
      foreach (Material target in prop.targets)
      {
        if (on)
          target.EnableKeyword(keyword);
        else
          target.DisableKeyword(keyword);
      }
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      if (!MaterialToggleDrawer.IsPropertyTypeSuitable(prop))
        return 40f;
      return base.GetPropertyHeight(prop, label, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {
      if (!MaterialToggleDrawer.IsPropertyTypeSuitable(prop))
      {
        GUIContent label1 = EditorGUIUtility.TempContent("Toggle used on a non-float property: " + prop.name, (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        EditorGUI.LabelField(position, label1, EditorStyles.helpBox);
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        bool flag = (double) Math.Abs(prop.floatValue) > 1.0 / 1000.0;
        EditorGUI.showMixedValue = prop.hasMixedValue;
        bool on = EditorGUI.Toggle(position, label, flag);
        EditorGUI.showMixedValue = false;
        if (!EditorGUI.EndChangeCheck())
          return;
        prop.floatValue = !on ? 0.0f : 1f;
        this.SetKeyword(prop, on);
      }
    }

    public override void Apply(MaterialProperty prop)
    {
      base.Apply(prop);
      if (!MaterialToggleDrawer.IsPropertyTypeSuitable(prop) || prop.hasMixedValue)
        return;
      this.SetKeyword(prop, (double) Math.Abs(prop.floatValue) > 1.0 / 1000.0);
    }
  }
}
