// Decompiled with JetBrains decompiler
// Type: UnityEditor.MenuUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class MenuUtils
  {
    public static void MenuCallback(object callbackObject)
    {
      MenuUtils.MenuCallbackObject menuCallbackObject = callbackObject as MenuUtils.MenuCallbackObject;
      if (menuCallbackObject.onBeforeExecuteCallback != null)
        menuCallbackObject.onBeforeExecuteCallback(menuCallbackObject.menuItemPath, menuCallbackObject.temporaryContext, menuCallbackObject.userData);
      if (menuCallbackObject.temporaryContext != null)
        EditorApplication.ExecuteMenuItemWithTemporaryContext(menuCallbackObject.menuItemPath, menuCallbackObject.temporaryContext);
      else
        EditorApplication.ExecuteMenuItem(menuCallbackObject.menuItemPath);
      if (menuCallbackObject.onAfterExecuteCallback == null)
        return;
      menuCallbackObject.onAfterExecuteCallback(menuCallbackObject.menuItemPath, menuCallbackObject.temporaryContext, menuCallbackObject.userData);
    }

    public static void ExtractSubMenuWithPath(string path, GenericMenu menu, string replacementPath, UnityEngine.Object[] temporaryContext)
    {
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) Unsupported.GetSubmenus(path));
      foreach (string includingSeparator in Unsupported.GetSubmenusIncludingSeparators(path))
      {
        string replacementMenuString = replacementPath + includingSeparator.Substring(path.Length);
        if (stringSet.Contains(includingSeparator))
          MenuUtils.ExtractMenuItemWithPath(includingSeparator, menu, replacementMenuString, temporaryContext, -1, (Action<string, UnityEngine.Object[], int>) null, (Action<string, UnityEngine.Object[], int>) null);
      }
    }

    public static void ExtractMenuItemWithPath(string menuString, GenericMenu menu, string replacementMenuString, UnityEngine.Object[] temporaryContext, int userData, Action<string, UnityEngine.Object[], int> onBeforeExecuteCallback, Action<string, UnityEngine.Object[], int> onAfterExecuteCallback)
    {
      MenuUtils.MenuCallbackObject menuCallbackObject1 = new MenuUtils.MenuCallbackObject();
      menuCallbackObject1.menuItemPath = menuString;
      menuCallbackObject1.temporaryContext = temporaryContext;
      menuCallbackObject1.onBeforeExecuteCallback = onBeforeExecuteCallback;
      menuCallbackObject1.onAfterExecuteCallback = onAfterExecuteCallback;
      menuCallbackObject1.userData = userData;
      GenericMenu genericMenu = menu;
      GUIContent content = new GUIContent(replacementMenuString);
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      if (MenuUtils.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MenuUtils.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(MenuUtils.MenuCallback);
      }
      // ISSUE: reference to a compiler-generated field
      GenericMenu.MenuFunction2 fMgCache0 = MenuUtils.\u003C\u003Ef__mg\u0024cache0;
      MenuUtils.MenuCallbackObject menuCallbackObject2 = menuCallbackObject1;
      genericMenu.AddItem(content, num != 0, fMgCache0, (object) menuCallbackObject2);
    }

    private class MenuCallbackObject
    {
      public string menuItemPath;
      public UnityEngine.Object[] temporaryContext;
      public Action<string, UnityEngine.Object[], int> onBeforeExecuteCallback;
      public Action<string, UnityEngine.Object[], int> onAfterExecuteCallback;
      public int userData;
    }
  }
}
