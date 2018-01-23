// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetSelectionPopupMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetSelectionPopupMenu
  {
    public static void Show(Rect buttonRect, string[] classNames, int initialSelectedInstanceID)
    {
      GenericMenu genericMenu1 = new GenericMenu();
      List<UnityEngine.Object> assetsOfType = AssetSelectionPopupMenu.FindAssetsOfType(classNames);
      if (assetsOfType.Any<UnityEngine.Object>())
      {
        assetsOfType.Sort((Comparison<UnityEngine.Object>) ((result1, result2) => EditorUtility.NaturalCompare(result1.name, result2.name)));
        foreach (UnityEngine.Object object1 in assetsOfType)
        {
          GUIContent guiContent = new GUIContent(object1.name);
          bool flag = object1.GetInstanceID() == initialSelectedInstanceID;
          GenericMenu genericMenu2 = genericMenu1;
          GUIContent content = guiContent;
          int num = flag ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          if (AssetSelectionPopupMenu.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            AssetSelectionPopupMenu.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(AssetSelectionPopupMenu.SelectCallback);
          }
          // ISSUE: reference to a compiler-generated field
          GenericMenu.MenuFunction2 fMgCache0 = AssetSelectionPopupMenu.\u003C\u003Ef__mg\u0024cache0;
          UnityEngine.Object object2 = object1;
          genericMenu2.AddItem(content, num != 0, fMgCache0, (object) object2);
        }
      }
      else
        genericMenu1.AddDisabledItem(new GUIContent("No Audio Mixers found in this project"));
      genericMenu1.DropDown(buttonRect);
    }

    private static void SelectCallback(object userData)
    {
      UnityEngine.Object @object = userData as UnityEngine.Object;
      if (!(@object != (UnityEngine.Object) null))
        return;
      Selection.activeInstanceID = @object.GetInstanceID();
    }

    private static List<UnityEngine.Object> FindAssetsOfType(string[] classNames)
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      hierarchyProperty.SetSearchFilter(new SearchFilter()
      {
        classNames = classNames
      });
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      while (hierarchyProperty.Next((int[]) null))
        objectList.Add(hierarchyProperty.pptrValue);
      return objectList;
    }
  }
}
