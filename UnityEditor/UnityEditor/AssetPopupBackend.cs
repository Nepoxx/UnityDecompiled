// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPopupBackend
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetPopupBackend
  {
    public static void AssetPopup<T>(SerializedProperty serializedProperty, GUIContent label, string fileExtension, string defaultFieldName) where T : UnityEngine.Object, new()
    {
      bool showMixedValue = EditorGUI.showMixedValue;
      EditorGUI.showMixedValue = serializedProperty.hasMultipleDifferentValues;
      string referenceTypeString = serializedProperty.objectReferenceTypeString;
      GUIContent buttonContent = !serializedProperty.hasMultipleDifferentValues ? (!(serializedProperty.objectReferenceValue != (UnityEngine.Object) null) ? GUIContent.Temp(defaultFieldName) : GUIContent.Temp(serializedProperty.objectReferenceStringValue)) : EditorGUI.mixedValueContent;
      Rect buttonRect;
      if (AudioMixerEffectGUI.PopupButton(label, buttonContent, EditorStyles.popup, out buttonRect))
        AssetPopupBackend.ShowAssetsPopupMenu<T>(buttonRect, referenceTypeString, serializedProperty, fileExtension, defaultFieldName);
      EditorGUI.showMixedValue = showMixedValue;
    }

    public static void AssetPopup<T>(SerializedProperty serializedProperty, GUIContent label, string fileExtension) where T : UnityEngine.Object, new()
    {
      AssetPopupBackend.AssetPopup<T>(serializedProperty, label, fileExtension, "Default");
    }

    private static void ShowAssetsPopupMenu<T>(Rect buttonRect, string typeName, SerializedProperty serializedProperty, string fileExtension, string defaultFieldName) where T : UnityEngine.Object, new()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetPopupBackend.\u003CShowAssetsPopupMenu\u003Ec__AnonStorey1<T> menuCAnonStorey1 = new AssetPopupBackend.\u003CShowAssetsPopupMenu\u003Ec__AnonStorey1<T>();
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey1.typeName = typeName;
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey1.fileExtension = fileExtension;
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey1.serializedProperty = serializedProperty;
      GenericMenu genericMenu1 = new GenericMenu();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num1 = !(menuCAnonStorey1.serializedProperty.objectReferenceValue != (UnityEngine.Object) null) ? 0 : menuCAnonStorey1.serializedProperty.objectReferenceValue.GetInstanceID();
      bool flag = false;
      // ISSUE: reference to a compiler-generated field
      UnityType typeByName = UnityType.FindTypeByName(menuCAnonStorey1.typeName);
      int classID = typeByName == null ? 0 : typeByName.persistentTypeID;
      BuiltinResource[] builtinResourceArray = (BuiltinResource[]) null;
      if (classID > 0)
      {
        builtinResourceArray = EditorGUIUtility.GetBuiltinResourceList(classID);
        foreach (BuiltinResource builtinResource in builtinResourceArray)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AssetPopupBackend.\u003CShowAssetsPopupMenu\u003Ec__AnonStorey0<T> menuCAnonStorey0 = new AssetPopupBackend.\u003CShowAssetsPopupMenu\u003Ec__AnonStorey0<T>();
          // ISSUE: reference to a compiler-generated field
          menuCAnonStorey0.resource = builtinResource;
          // ISSUE: reference to a compiler-generated field
          if (menuCAnonStorey0.resource.m_Name == defaultFieldName)
          {
            GenericMenu genericMenu2 = genericMenu1;
            // ISSUE: reference to a compiler-generated field
            GUIContent content = new GUIContent(menuCAnonStorey0.resource.m_Name);
            // ISSUE: reference to a compiler-generated field
            int num2 = menuCAnonStorey0.resource.m_InstanceID == num1 ? 1 : 0;
            // ISSUE: reference to a compiler-generated field
            if (AssetPopupBackend.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              AssetPopupBackend.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
            }
            // ISSUE: reference to a compiler-generated field
            GenericMenu.MenuFunction2 fMgCache0 = AssetPopupBackend.\u003C\u003Ef__mg\u0024cache0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object[] objArray = new object[2]{ (object) menuCAnonStorey0.resource.m_InstanceID, (object) menuCAnonStorey1.serializedProperty };
            genericMenu2.AddItem(content, num2 != 0, fMgCache0, (object) objArray);
            // ISSUE: reference to a compiler-generated method
            builtinResourceArray = ((IEnumerable<BuiltinResource>) builtinResourceArray).Where<BuiltinResource>(new Func<BuiltinResource, bool>(menuCAnonStorey0.\u003C\u003Em__0)).ToArray<BuiltinResource>();
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        GenericMenu genericMenu2 = genericMenu1;
        GUIContent content = new GUIContent(defaultFieldName);
        int num2 = num1 == 0 ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AssetPopupBackend.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AssetPopupBackend.\u003C\u003Ef__mg\u0024cache1 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache1 = AssetPopupBackend.\u003C\u003Ef__mg\u0024cache1;
        // ISSUE: reference to a compiler-generated field
        object[] objArray = new object[2]{ (object) 0, (object) menuCAnonStorey1.serializedProperty };
        genericMenu2.AddItem(content, num2 != 0, fMgCache1, (object) objArray);
      }
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      // ISSUE: reference to a compiler-generated field
      SearchFilter filter = new SearchFilter() { classNames = new string[1]{ menuCAnonStorey1.typeName } };
      hierarchyProperty.SetSearchFilter(filter);
      hierarchyProperty.Reset();
      while (hierarchyProperty.Next((int[]) null))
      {
        GenericMenu genericMenu2 = genericMenu1;
        GUIContent content = new GUIContent(hierarchyProperty.name);
        int num2 = hierarchyProperty.instanceID == num1 ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AssetPopupBackend.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AssetPopupBackend.\u003C\u003Ef__mg\u0024cache2 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache2 = AssetPopupBackend.\u003C\u003Ef__mg\u0024cache2;
        // ISSUE: reference to a compiler-generated field
        object[] objArray = new object[2]{ (object) hierarchyProperty.instanceID, (object) menuCAnonStorey1.serializedProperty };
        genericMenu2.AddItem(content, num2 != 0, fMgCache2, (object) objArray);
      }
      if (classID > 0 && builtinResourceArray != null)
      {
        foreach (BuiltinResource builtinResource in builtinResourceArray)
        {
          GenericMenu genericMenu2 = genericMenu1;
          GUIContent content = new GUIContent(builtinResource.m_Name);
          int num2 = builtinResource.m_InstanceID == num1 ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          if (AssetPopupBackend.\u003C\u003Ef__mg\u0024cache3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            AssetPopupBackend.\u003C\u003Ef__mg\u0024cache3 = new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback);
          }
          // ISSUE: reference to a compiler-generated field
          GenericMenu.MenuFunction2 fMgCache3 = AssetPopupBackend.\u003C\u003Ef__mg\u0024cache3;
          // ISSUE: reference to a compiler-generated field
          object[] objArray = new object[2]{ (object) builtinResource.m_InstanceID, (object) menuCAnonStorey1.serializedProperty };
          genericMenu2.AddItem(content, num2 != 0, fMgCache3, (object) objArray);
        }
      }
      genericMenu1.AddSeparator("");
      // ISSUE: reference to a compiler-generated method
      genericMenu1.AddItem(new GUIContent("Create New..."), false, new GenericMenu.MenuFunction(menuCAnonStorey1.\u003C\u003Em__0));
      genericMenu1.DropDown(buttonRect);
    }

    private static void ShowAssetsPopupMenu<T>(Rect buttonRect, string typeName, SerializedProperty serializedProperty, string fileExtension) where T : UnityEngine.Object, new()
    {
      AssetPopupBackend.ShowAssetsPopupMenu<T>(buttonRect, typeName, serializedProperty, fileExtension, "Default");
    }

    private static void AssetPopupMenuCallback(object userData)
    {
      object[] objArray = userData as object[];
      int instanceID = (int) objArray[0];
      SerializedProperty serializedProperty = (SerializedProperty) objArray[1];
      serializedProperty.objectReferenceValue = EditorUtility.InstanceIDToObject(instanceID);
      serializedProperty.m_SerializedObject.ApplyModifiedProperties();
    }
  }
}
