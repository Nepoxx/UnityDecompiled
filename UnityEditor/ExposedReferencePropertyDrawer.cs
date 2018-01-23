// Decompiled with JetBrains decompiler
// Type: ExposedReferencePropertyDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof (ExposedReference<>))]
internal class ExposedReferencePropertyDrawer : BaseExposedPropertyDrawer
{
  protected override void OnRenderProperty(Rect position, PropertyName exposedPropertyNameString, UnityEngine.Object currentReferenceValue, SerializedProperty exposedPropertyDefault, SerializedProperty exposedPropertyName, BaseExposedPropertyDrawer.ExposedPropertyMode mode, IExposedPropertyTable exposedPropertyTable)
  {
    System.Type genericArgument = this.fieldInfo.FieldType.GetGenericArguments()[0];
    EditorGUI.BeginChangeCheck();
    UnityEngine.Object target = EditorGUI.ObjectField(position, currentReferenceValue, genericArgument, exposedPropertyTable != null);
    if (!EditorGUI.EndChangeCheck())
      return;
    if (mode == BaseExposedPropertyDrawer.ExposedPropertyMode.DefaultValue)
    {
      if (!EditorUtility.IsPersistent(exposedPropertyDefault.serializedObject.targetObject) || target == (UnityEngine.Object) null || EditorUtility.IsPersistent(target))
      {
        if (!EditorGUI.CheckForCrossSceneReferencing(exposedPropertyDefault.serializedObject.targetObject, target))
          exposedPropertyDefault.objectReferenceValue = target;
      }
      else
      {
        string name = GUID.Generate().ToString();
        exposedPropertyNameString = new PropertyName(name);
        exposedPropertyName.stringValue = name;
        Undo.RecordObject(exposedPropertyTable as UnityEngine.Object, "Set Exposed Property");
        exposedPropertyTable.SetReferenceValue(exposedPropertyNameString, target);
      }
    }
    else
    {
      Undo.RecordObject(exposedPropertyTable as UnityEngine.Object, "Set Exposed Property");
      exposedPropertyTable.SetReferenceValue(exposedPropertyNameString, target);
    }
  }

  protected override void PopulateContextMenu(GenericMenu menu, BaseExposedPropertyDrawer.OverrideState overrideState, IExposedPropertyTable exposedPropertyTable, SerializedProperty exposedName, SerializedProperty defaultValue)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ExposedReferencePropertyDrawer.\u003CPopulateContextMenu\u003Ec__AnonStorey0 menuCAnonStorey0 = new ExposedReferencePropertyDrawer.\u003CPopulateContextMenu\u003Ec__AnonStorey0();
    // ISSUE: reference to a compiler-generated field
    menuCAnonStorey0.exposedName = exposedName;
    // ISSUE: reference to a compiler-generated field
    menuCAnonStorey0.exposedPropertyTable = exposedPropertyTable;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    menuCAnonStorey0.propertyName = new PropertyName(menuCAnonStorey0.exposedName.stringValue);
    BaseExposedPropertyDrawer.OverrideState currentOverrideState;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    menuCAnonStorey0.currentValue = this.Resolve(new PropertyName(menuCAnonStorey0.exposedName.stringValue), menuCAnonStorey0.exposedPropertyTable, defaultValue.objectReferenceValue, out currentOverrideState);
    if (overrideState == BaseExposedPropertyDrawer.OverrideState.DefaultValue)
    {
      // ISSUE: reference to a compiler-generated method
      menu.AddItem(new GUIContent(this.ExposePropertyContent.text), false, new GenericMenu.MenuFunction2(menuCAnonStorey0.\u003C\u003Em__0), (object) null);
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      menu.AddItem(this.UnexposePropertyContent, false, new GenericMenu.MenuFunction2(menuCAnonStorey0.\u003C\u003Em__1), (object) null);
    }
  }
}
