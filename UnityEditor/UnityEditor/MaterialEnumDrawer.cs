// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialEnumDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class MaterialEnumDrawer : MaterialPropertyDrawer
  {
    private readonly GUIContent[] names;
    private readonly int[] values;

    public MaterialEnumDrawer(string enumName)
    {
      System.Type[] array = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) AssemblyHelper.GetTypesFromAssembly(x))).ToArray<System.Type>();
      try
      {
        System.Type enumType = ((IEnumerable<System.Type>) array).FirstOrDefault<System.Type>((Func<System.Type, bool>) (x => x.IsSubclassOf(typeof (Enum)) && (x.Name == enumName || x.FullName == enumName)));
        string[] names = Enum.GetNames(enumType);
        this.names = new GUIContent[names.Length];
        for (int index = 0; index < names.Length; ++index)
          this.names[index] = new GUIContent(names[index]);
        Array values = Enum.GetValues(enumType);
        this.values = new int[values.Length];
        for (int index = 0; index < values.Length; ++index)
          this.values[index] = (int) values.GetValue(index);
      }
      catch (Exception ex)
      {
        Debug.LogWarningFormat("Failed to create MaterialEnum, enum {0} not found", (object) enumName);
        throw;
      }
    }

    public MaterialEnumDrawer(string n1, float v1)
      : this(new string[1]{ n1 }, new float[1]{ v1 })
    {
    }

    public MaterialEnumDrawer(string n1, float v1, string n2, float v2)
      : this(new string[2]{ n1, n2 }, new float[2]{ v1, v2 })
    {
    }

    public MaterialEnumDrawer(string n1, float v1, string n2, float v2, string n3, float v3)
      : this(new string[3]{ n1, n2, n3 }, new float[3]{ v1, v2, v3 })
    {
    }

    public MaterialEnumDrawer(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4)
      : this(new string[4]{ n1, n2, n3, n4 }, new float[4]{ v1, v2, v3, v4 })
    {
    }

    public MaterialEnumDrawer(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5)
      : this(new string[5]{ n1, n2, n3, n4, n5 }, new float[5]{ v1, v2, v3, v4, v5 })
    {
    }

    public MaterialEnumDrawer(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6)
      : this(new string[6]{ n1, n2, n3, n4, n5, n6 }, new float[6]{ v1, v2, v3, v4, v5, v6 })
    {
    }

    public MaterialEnumDrawer(string n1, float v1, string n2, float v2, string n3, float v3, string n4, float v4, string n5, float v5, string n6, float v6, string n7, float v7)
      : this(new string[7]{ n1, n2, n3, n4, n5, n6, n7 }, new float[7]{ v1, v2, v3, v4, v5, v6, v7 })
    {
    }

    public MaterialEnumDrawer(string[] enumNames, float[] vals)
    {
      this.names = new GUIContent[enumNames.Length];
      for (int index = 0; index < enumNames.Length; ++index)
        this.names[index] = new GUIContent(enumNames[index]);
      this.values = new int[vals.Length];
      for (int index = 0; index < vals.Length; ++index)
        this.values[index] = (int) vals[index];
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      if (prop.type != MaterialProperty.PropType.Float && prop.type != MaterialProperty.PropType.Range)
        return 40f;
      return base.GetPropertyHeight(prop, label, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
    {
      if (prop.type != MaterialProperty.PropType.Float && prop.type != MaterialProperty.PropType.Range)
      {
        GUIContent label1 = EditorGUIUtility.TempContent("Enum used on a non-float property: " + prop.name, (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        EditorGUI.LabelField(position, label1, EditorStyles.helpBox);
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;
        int floatValue = (int) prop.floatValue;
        int num = EditorGUI.IntPopup(position, label, floatValue, this.names, this.values);
        EditorGUI.showMixedValue = false;
        if (!EditorGUI.EndChangeCheck())
          return;
        prop.floatValue = (float) num;
      }
    }
  }
}
