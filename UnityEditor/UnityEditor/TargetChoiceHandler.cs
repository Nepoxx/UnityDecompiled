// Decompiled with JetBrains decompiler
// Type: UnityEditor.TargetChoiceHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class TargetChoiceHandler
  {
    internal static void DuplicateArrayElement(object userData)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.DuplicateCommand();
      serializedProperty.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    internal static void DeleteArrayElement(object userData)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.DeleteCommand();
      serializedProperty.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    internal static void SetPrefabOverride(object userData)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.prefabOverride = false;
      serializedProperty.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    internal static void SetToValueOfTarget(SerializedProperty property, Object target)
    {
      property.SetToValueOfTarget(target);
      property.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    private static void TargetChoiceForwardFunction(object userData)
    {
      PropertyAndTargetHandler andTargetHandler = (PropertyAndTargetHandler) userData;
      andTargetHandler.function(andTargetHandler.property, andTargetHandler.target);
    }

    internal static void AddSetToValueOfTargetMenuItems(GenericMenu menu, SerializedProperty property, TargetChoiceHandler.TargetChoiceMenuFunction func)
    {
      SerializedProperty property1 = property.serializedObject.FindProperty(property.propertyPath);
      Object[] targetObjects = property.serializedObject.targetObjects;
      List<string> stringList = new List<string>();
      foreach (Object target in targetObjects)
      {
        string textAndTooltip = string.Format("Set to Value of {0}", (object) target.name);
        if (stringList.Contains(textAndTooltip))
        {
          int num = 1;
          while (true)
          {
            textAndTooltip = string.Format("Set to Value of {0}({1})", (object) target.name, (object) num);
            if (stringList.Contains(textAndTooltip))
              ++num;
            else
              break;
          }
        }
        stringList.Add(textAndTooltip);
        GenericMenu genericMenu = menu;
        GUIContent content = EditorGUIUtility.TextContent(textAndTooltip);
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        if (TargetChoiceHandler.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TargetChoiceHandler.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(TargetChoiceHandler.TargetChoiceForwardFunction);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache0 = TargetChoiceHandler.\u003C\u003Ef__mg\u0024cache0;
        PropertyAndTargetHandler andTargetHandler = new PropertyAndTargetHandler(property1, target, func);
        genericMenu.AddItem(content, num1 != 0, fMgCache0, (object) andTargetHandler);
      }
    }

    internal delegate void TargetChoiceMenuFunction(SerializedProperty property, Object target);
  }
}
