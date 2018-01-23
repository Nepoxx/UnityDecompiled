// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialIntRangeDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MaterialIntRangeDrawer : MaterialPropertyDrawer
  {
    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {
      if (prop.type != MaterialProperty.PropType.Range)
      {
        GUIContent label1 = EditorGUIUtility.TempContent("IntRange used on a non-range property: " + prop.name, (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        EditorGUI.LabelField(position, label1, EditorStyles.helpBox);
      }
      else
        MaterialEditor.DoIntRangeProperty(position, prop, label);
    }
  }
}
