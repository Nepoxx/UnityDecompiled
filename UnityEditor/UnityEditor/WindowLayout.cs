// Decompiled with JetBrains decompiler
// Type: UnityEditor.WindowLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEditor.Connect;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class WindowLayout
  {
    internal static PrefKey s_MaximizeKey = new PrefKey("Window/Maximize View", "# ");
    private const string kMaximizeRestoreFile = "CurrentMaximizeLayout.dwlt";

    private static void ShowWindowImmediate(EditorWindow win)
    {
      win.Show(true);
    }

    internal static EditorWindow FindEditorWindowOfType(System.Type type)
    {
      UnityEngine.Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(type);
      if (objectsOfTypeAll.Length > 0)
        return objectsOfTypeAll[0] as EditorWindow;
      return (EditorWindow) null;
    }

    [DebuggerHidden]
    private static IEnumerable<T> FindEditorWindowsOfType<T>() where T : class
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WindowLayout.\u003CFindEditorWindowsOfType\u003Ec__Iterator0<T> ofTypeCIterator0_1 = new WindowLayout.\u003CFindEditorWindowsOfType\u003Ec__Iterator0<T>();
      // ISSUE: variable of a compiler-generated type
      WindowLayout.\u003CFindEditorWindowsOfType\u003Ec__Iterator0<T> ofTypeCIterator0_2 = ofTypeCIterator0_1;
      // ISSUE: reference to a compiler-generated field
      ofTypeCIterator0_2.\u0024PC = -2;
      return (IEnumerable<T>) ofTypeCIterator0_2;
    }

    internal static void CheckWindowConsistency()
    {
      foreach (EditorWindow editorWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (EditorWindow)))
      {
        if ((UnityEngine.Object) editorWindow.m_Parent == (UnityEngine.Object) null)
          UnityEngine.Debug.LogError((object) ("Invalid editor window " + (object) editorWindow.GetType()));
      }
    }

    internal static EditorWindow TryGetLastFocusedWindowInSameDock()
    {
      System.Type type = (System.Type) null;
      string windowTypeInSameDock = WindowFocusState.instance.m_LastWindowTypeInSameDock;
      if (windowTypeInSameDock != "")
        type = System.Type.GetType(windowTypeInSameDock);
      GameView editorWindowOfType = WindowLayout.FindEditorWindowOfType(typeof (GameView)) as GameView;
      if (type != null && (bool) ((UnityEngine.Object) editorWindowOfType) && ((UnityEngine.Object) editorWindowOfType.m_Parent != (UnityEngine.Object) null && editorWindowOfType.m_Parent is DockArea))
      {
        object[] objectsOfTypeAll = (object[]) UnityEngine.Resources.FindObjectsOfTypeAll(type);
        DockArea parent = editorWindowOfType.m_Parent as DockArea;
        for (int index = 0; index < objectsOfTypeAll.Length; ++index)
        {
          EditorWindow editorWindow = objectsOfTypeAll[index] as EditorWindow;
          if ((bool) ((UnityEngine.Object) editorWindow) && (UnityEngine.Object) editorWindow.m_Parent == (UnityEngine.Object) parent)
            return editorWindow;
        }
      }
      return (EditorWindow) null;
    }

    internal static void SaveCurrentFocusedWindowInSameDock(EditorWindow windowToBeFocused)
    {
      if (!((UnityEngine.Object) windowToBeFocused.m_Parent != (UnityEngine.Object) null) || !(windowToBeFocused.m_Parent is DockArea))
        return;
      EditorWindow actualView = (windowToBeFocused.m_Parent as DockArea).actualView;
      if ((bool) ((UnityEngine.Object) actualView))
        WindowFocusState.instance.m_LastWindowTypeInSameDock = actualView.GetType().ToString();
    }

    internal static void FindFirstGameViewAndSetToMaximizeOnPlay()
    {
      GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
      if (!(bool) ((UnityEngine.Object) editorWindowOfType))
        return;
      editorWindowOfType.maximizeOnPlay = true;
    }

    internal static EditorWindow TryFocusAppropriateWindow(bool enteringPlaymode)
    {
      if (enteringPlaymode)
      {
        GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
        if ((bool) ((UnityEngine.Object) editorWindowOfType))
        {
          WindowLayout.SaveCurrentFocusedWindowInSameDock((EditorWindow) editorWindowOfType);
          editorWindowOfType.Focus();
        }
        return (EditorWindow) editorWindowOfType;
      }
      EditorWindow windowInSameDock = WindowLayout.TryGetLastFocusedWindowInSameDock();
      if ((bool) ((UnityEngine.Object) windowInSameDock))
        windowInSameDock.ShowTab();
      return windowInSameDock;
    }

    internal static EditorWindow GetMaximizedWindow()
    {
      UnityEngine.Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (MaximizedHostView));
      if (objectsOfTypeAll.Length != 0)
      {
        MaximizedHostView maximizedHostView = objectsOfTypeAll[0] as MaximizedHostView;
        if ((bool) ((UnityEngine.Object) maximizedHostView.actualView))
          return maximizedHostView.actualView;
      }
      return (EditorWindow) null;
    }

    internal static EditorWindow ShowAppropriateViewOnEnterExitPlaymode(bool entering)
    {
      if (WindowFocusState.instance.m_CurrentlyInPlayMode == entering)
        return (EditorWindow) null;
      WindowFocusState.instance.m_CurrentlyInPlayMode = entering;
      EditorWindow maximizedWindow = WindowLayout.GetMaximizedWindow();
      if (entering)
      {
        WindowFocusState.instance.m_WasMaximizedBeforePlay = (UnityEngine.Object) maximizedWindow != (UnityEngine.Object) null;
        if ((UnityEngine.Object) maximizedWindow != (UnityEngine.Object) null)
          return maximizedWindow;
      }
      else if (WindowFocusState.instance.m_WasMaximizedBeforePlay)
        return maximizedWindow;
      if ((bool) ((UnityEngine.Object) maximizedWindow))
        WindowLayout.Unmaximize(maximizedWindow);
      EditorWindow editorWindow = WindowLayout.TryFocusAppropriateWindow(entering);
      if ((bool) ((UnityEngine.Object) editorWindow) || !entering)
        return editorWindow;
      EditorWindow editorWindowOfType = WindowLayout.FindEditorWindowOfType(typeof (SceneView));
      if ((bool) ((UnityEngine.Object) editorWindowOfType) && editorWindowOfType.m_Parent is DockArea)
      {
        DockArea parent = editorWindowOfType.m_Parent as DockArea;
        if ((bool) ((UnityEngine.Object) parent))
        {
          WindowFocusState.instance.m_LastWindowTypeInSameDock = editorWindowOfType.GetType().ToString();
          GameView instance = ScriptableObject.CreateInstance<GameView>();
          parent.AddTab((EditorWindow) instance);
          return (EditorWindow) instance;
        }
      }
      GameView instance1 = ScriptableObject.CreateInstance<GameView>();
      instance1.Show(true);
      instance1.Focus();
      return (EditorWindow) instance1;
    }

    internal static bool IsMaximized(EditorWindow window)
    {
      return window.m_Parent is MaximizedHostView;
    }

    internal static void MaximizeKeyHandler()
    {
      if (!WindowLayout.s_MaximizeKey.activated && Event.current.type != EditorGUIUtility.magnifyGestureEventType || GUIUtility.hotControl != 0)
        return;
      EventType type = Event.current.type;
      Event.current.Use();
      EditorWindow mouseOverWindow = EditorWindow.mouseOverWindow;
      if ((bool) ((UnityEngine.Object) mouseOverWindow) && !(mouseOverWindow is PreviewWindow))
      {
        if (type == EditorGUIUtility.magnifyGestureEventType)
        {
          if ((double) Event.current.delta.x < -0.05)
          {
            if (WindowLayout.IsMaximized(mouseOverWindow))
              WindowLayout.Unmaximize(mouseOverWindow);
          }
          else if ((double) Event.current.delta.x > 0.05 && !WindowLayout.IsMaximized(mouseOverWindow))
            WindowLayout.Maximize(mouseOverWindow);
        }
        else if (WindowLayout.IsMaximized(mouseOverWindow))
          WindowLayout.Unmaximize(mouseOverWindow);
        else
          WindowLayout.Maximize(mouseOverWindow);
      }
    }

    public static void Unmaximize(EditorWindow win)
    {
      HostView parent1 = win.m_Parent;
      if ((UnityEngine.Object) parent1 == (UnityEngine.Object) null)
      {
        UnityEngine.Debug.LogError((object) "Host view was not found");
        WindowLayout.RevertFactorySettings();
      }
      else
      {
        UnityEngine.Object[] objectArray = InternalEditorUtility.LoadSerializedFileAndForget(Path.Combine(WindowLayout.layoutsProjectPath, "CurrentMaximizeLayout.dwlt"));
        if (objectArray.Length < 2)
        {
          UnityEngine.Debug.Log((object) "Maximized serialized file backup not found");
          WindowLayout.RevertFactorySettings();
        }
        else
        {
          SplitView splitView = objectArray[0] as SplitView;
          EditorWindow pane = objectArray[1] as EditorWindow;
          if ((UnityEngine.Object) splitView == (UnityEngine.Object) null)
          {
            UnityEngine.Debug.Log((object) "Maximization failed because the root split view was not found");
            WindowLayout.RevertFactorySettings();
          }
          else
          {
            ContainerWindow window = win.m_Parent.window;
            if ((UnityEngine.Object) window == (UnityEngine.Object) null)
            {
              UnityEngine.Debug.Log((object) "Maximization failed because the root split view has no container window");
              WindowLayout.RevertFactorySettings();
            }
            else
            {
              try
              {
                ContainerWindow.SetFreezeDisplay(true);
                if (!(bool) ((UnityEngine.Object) parent1.parent))
                  throw new Exception();
                int idx1 = parent1.parent.IndexOfChild((View) parent1);
                Rect position = parent1.position;
                View parent2 = parent1.parent;
                parent2.RemoveChild(idx1);
                parent2.AddChild((View) splitView, idx1);
                splitView.position = position;
                DockArea parent3 = pane.m_Parent as DockArea;
                int idx2 = parent3.m_Panes.IndexOf(pane);
                parent1.actualView = (EditorWindow) null;
                win.m_Parent = (HostView) null;
                parent3.AddTab(idx2, win);
                parent3.RemoveTab(pane);
                UnityEngine.Object.DestroyImmediate((UnityEngine.Object) pane);
                foreach (UnityEngine.Object @object in objectArray)
                {
                  EditorWindow editorWindow = @object as EditorWindow;
                  if ((UnityEngine.Object) editorWindow != (UnityEngine.Object) null)
                    editorWindow.MakeParentsSettingsMatchMe();
                }
                parent2.Initialize(parent2.window);
                parent2.position = parent2.position;
                splitView.Reflow();
                UnityEngine.Object.DestroyImmediate((UnityEngine.Object) parent1);
                win.Focus();
                window.DisplayAllViews();
                win.m_Parent.MakeVistaDWMHappyDance();
              }
              catch (Exception ex)
              {
                UnityEngine.Debug.Log((object) ("Maximization failed: " + (object) ex));
                WindowLayout.RevertFactorySettings();
              }
              try
              {
                if (Application.platform != RuntimePlatform.OSXEditor || !SystemInfo.operatingSystem.Contains("10.7") || !SystemInfo.graphicsDeviceVendor.Contains("ATI"))
                  return;
                foreach (GUIView guiView in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (GUIView)))
                  guiView.Repaint();
              }
              finally
              {
                ContainerWindow.SetFreezeDisplay(false);
              }
            }
          }
        }
      }
    }

    public static void AddSplitViewAndChildrenRecurse(View splitview, ArrayList list)
    {
      list.Add((object) splitview);
      DockArea dockArea = splitview as DockArea;
      if ((UnityEngine.Object) dockArea != (UnityEngine.Object) null)
        list.AddRange((ICollection) dockArea.m_Panes);
      if ((UnityEngine.Object) (splitview as DockArea) != (UnityEngine.Object) null)
        list.Add((object) dockArea.actualView);
      foreach (View child in splitview.children)
        WindowLayout.AddSplitViewAndChildrenRecurse(child, list);
    }

    public static void SaveSplitViewAndChildren(View splitview, EditorWindow win, string path)
    {
      ArrayList list = new ArrayList();
      WindowLayout.AddSplitViewAndChildrenRecurse(splitview, list);
      list.Remove((object) splitview);
      list.Remove((object) win);
      list.Insert(0, (object) splitview);
      list.Insert(1, (object) win);
      InternalEditorUtility.SaveToSerializedFileAndForget(list.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[], path, true);
    }

    public static void Maximize(EditorWindow win)
    {
      if (!WindowLayout.MaximizePrepare(win))
        return;
      WindowLayout.MaximizePresent(win);
    }

    public static bool MaximizePrepare(EditorWindow win)
    {
      View parent1 = win.m_Parent.parent;
      View view = parent1;
      for (; (UnityEngine.Object) parent1 != (UnityEngine.Object) null && parent1 is SplitView; parent1 = parent1.parent)
        view = parent1;
      DockArea parent2 = win.m_Parent as DockArea;
      if ((UnityEngine.Object) parent2 == (UnityEngine.Object) null || (UnityEngine.Object) parent1 == (UnityEngine.Object) null || ((UnityEngine.Object) (view.parent as MainView) == (UnityEngine.Object) null || (UnityEngine.Object) win.m_Parent.window == (UnityEngine.Object) null))
        return false;
      int index = parent2.m_Panes.IndexOf(win);
      if (index == -1)
        return false;
      parent2.selected = index;
      WindowLayout.SaveSplitViewAndChildren(view, win, Path.Combine(WindowLayout.layoutsProjectPath, "CurrentMaximizeLayout.dwlt"));
      parent2.actualView = (EditorWindow) null;
      parent2.m_Panes[index] = (EditorWindow) null;
      MaximizedHostView instance = ScriptableObject.CreateInstance<MaximizedHostView>();
      int idx = parent1.IndexOfChild(view);
      Rect position = view.position;
      parent1.RemoveChild(view);
      parent1.AddChild((View) instance, idx);
      instance.actualView = win;
      instance.position = position;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) view, true);
      return true;
    }

    public static void MaximizePresent(EditorWindow win)
    {
      ContainerWindow.SetFreezeDisplay(true);
      win.Focus();
      WindowLayout.CheckWindowConsistency();
      win.m_Parent.window.DisplayAllViews();
      win.m_Parent.MakeVistaDWMHappyDance();
      ContainerWindow.SetFreezeDisplay(false);
    }

    public static bool LoadWindowLayout(string path, bool newProjectLayoutWasCreated)
    {
      Rect rect = new Rect();
      foreach (ContainerWindow containerWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (ContainerWindow)))
      {
        if (containerWindow.showMode == ShowMode.MainWindow)
          rect = containerWindow.position;
      }
      bool flag = false;
      try
      {
        ContainerWindow.SetFreezeDisplay(true);
        WindowLayout.CloseWindows();
        UnityEngine.Object[] objectArray = InternalEditorUtility.LoadSerializedFileAndForget(path);
        List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
        for (int index = 0; index < objectArray.Length; ++index)
        {
          UnityEngine.Object @object = objectArray[index];
          EditorWindow editorWindow = @object as EditorWindow;
          if ((UnityEngine.Object) editorWindow != (UnityEngine.Object) null)
          {
            if ((UnityEngine.Object) editorWindow.m_Parent == (UnityEngine.Object) null)
            {
              UnityEngine.Object.DestroyImmediate((UnityEngine.Object) editorWindow, true);
              Console.WriteLine("LoadWindowLayout: Removed unparented EditorWindow while reading window layout: window #" + (object) index + ", type=" + @object.GetType().ToString() + ", instanceID=" + (object) @object.GetInstanceID());
              flag = true;
              continue;
            }
          }
          else
          {
            DockArea dockArea = @object as DockArea;
            if ((UnityEngine.Object) dockArea != (UnityEngine.Object) null && dockArea.m_Panes.Count == 0)
            {
              dockArea.Close((object) null);
              Console.WriteLine("LoadWindowLayout: Removed empty DockArea while reading window layout: window #" + (object) index + ", instanceID=" + (object) @object.GetInstanceID());
              flag = true;
              continue;
            }
          }
          objectList.Add(@object);
        }
        ContainerWindow containerWindow1 = (ContainerWindow) null;
        ContainerWindow containerWindow2 = (ContainerWindow) null;
        for (int index = 0; index < objectList.Count; ++index)
        {
          ContainerWindow containerWindow3 = objectList[index] as ContainerWindow;
          if ((UnityEngine.Object) containerWindow3 != (UnityEngine.Object) null && containerWindow3.showMode == ShowMode.MainWindow)
          {
            containerWindow2 = containerWindow3;
            if ((double) rect.width != 0.0)
            {
              containerWindow1 = containerWindow3;
              containerWindow1.position = rect;
            }
          }
        }
        for (int index = 0; index < objectList.Count; ++index)
        {
          UnityEngine.Object @object = objectList[index];
          if (@object == (UnityEngine.Object) null)
          {
            Console.WriteLine("LoadWindowLayout: Error while reading window layout: window #" + (object) index + " is null");
            flag = true;
          }
          else if (@object.GetType() == null)
          {
            Console.WriteLine("LoadWindowLayout: Error while reading window layout: window #" + (object) index + " type is null, instanceID=" + (object) @object.GetInstanceID());
            flag = true;
          }
          else if (newProjectLayoutWasCreated)
          {
            MethodInfo method = @object.GetType().GetMethod("OnNewProjectLayoutWasCreated", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
              method.Invoke((object) @object, (object[]) null);
          }
        }
        if ((bool) ((UnityEngine.Object) containerWindow1))
        {
          containerWindow1.position = rect;
          containerWindow1.OnResize();
        }
        if ((UnityEngine.Object) containerWindow2 == (UnityEngine.Object) null)
        {
          UnityEngine.Debug.LogError((object) "Error while reading window layout: no main window found");
          throw new Exception();
        }
        containerWindow2.Show(containerWindow2.showMode, true, true);
        for (int index = 0; index < objectList.Count; ++index)
        {
          EditorWindow editorWindow = objectList[index] as EditorWindow;
          if ((bool) ((UnityEngine.Object) editorWindow))
            editorWindow.minSize = editorWindow.minSize;
          ContainerWindow containerWindow3 = objectList[index] as ContainerWindow;
          if ((bool) ((UnityEngine.Object) containerWindow3) && (UnityEngine.Object) containerWindow3 != (UnityEngine.Object) containerWindow2)
            containerWindow3.Show(containerWindow3.showMode, true, true);
        }
        GameView maximizedWindow = WindowLayout.GetMaximizedWindow() as GameView;
        if ((UnityEngine.Object) maximizedWindow != (UnityEngine.Object) null && maximizedWindow.maximizeOnPlay)
          WindowLayout.Unmaximize((EditorWindow) maximizedWindow);
        if (newProjectLayoutWasCreated)
        {
          if (UnityConnect.instance.online && UnityConnect.instance.loggedIn && UnityConnect.instance.shouldShowServicesWindow)
            UnityConnectServiceCollection.instance.ShowService("Hub", true, "new_project_created");
          else
            UnityConnectServiceCollection.instance.CloseServices();
        }
      }
      catch (Exception ex)
      {
        UnityEngine.Debug.LogError((object) ("Failed to load window layout: " + (object) ex));
        int num = 0;
        if (!Application.isTestRun)
          num = EditorUtility.DisplayDialogComplex("Failed to load window layout", "This can happen if layout contains custom windows and there are compile errors in the project.", "Load Default Layout", "Quit", "Revert Factory Settings");
        switch (num)
        {
          case 0:
            WindowLayout.LoadDefaultLayout();
            break;
          case 1:
            EditorApplication.Exit(0);
            break;
          case 2:
            WindowLayout.RevertFactorySettings();
            break;
        }
        return false;
      }
      finally
      {
        ContainerWindow.SetFreezeDisplay(false);
        Toolbar.lastLoadedLayoutName = !(Path.GetExtension(path) == ".wlt") ? (string) null : Path.GetFileNameWithoutExtension(path);
      }
      if (flag)
        UnityEngine.Debug.Log((object) "The editor layout could not be fully loaded, this can happen when the layout contains EditorWindows not available in this project");
      return true;
    }

    private static void LoadDefaultLayout()
    {
      InternalEditorUtility.LoadDefaultLayout();
    }

    public static void CloseWindows()
    {
      try
      {
        TooltipView.Close();
      }
      catch (Exception ex)
      {
      }
      foreach (ContainerWindow containerWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (ContainerWindow)))
      {
        try
        {
          containerWindow.Close();
        }
        catch (Exception ex)
        {
        }
      }
      UnityEngine.Object[] objectsOfTypeAll1 = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (EditorWindow));
      if (objectsOfTypeAll1.Length != 0)
      {
        string str = "";
        foreach (EditorWindow editorWindow in objectsOfTypeAll1)
        {
          str = str + "\n" + editorWindow.GetType().Name;
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) editorWindow, true);
        }
        UnityEngine.Debug.LogError((object) ("Failed to destroy editor windows: #" + (object) objectsOfTypeAll1.Length + str));
      }
      UnityEngine.Object[] objectsOfTypeAll2 = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (View));
      if (objectsOfTypeAll2.Length == 0)
        return;
      string str1 = "";
      foreach (View view in objectsOfTypeAll2)
      {
        str1 = str1 + "\n" + view.GetType().Name;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) view, true);
      }
      UnityEngine.Debug.LogError((object) ("Failed to destroy views: #" + (object) objectsOfTypeAll2.Length + str1));
    }

    public static void SaveWindowLayout(string path)
    {
      TooltipView.Close();
      ArrayList arrayList = new ArrayList();
      UnityEngine.Object[] objectsOfTypeAll1 = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (EditorWindow));
      UnityEngine.Object[] objectsOfTypeAll2 = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (ContainerWindow));
      UnityEngine.Object[] objectsOfTypeAll3 = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (View));
      foreach (ContainerWindow containerWindow in objectsOfTypeAll2)
      {
        if (!containerWindow.m_DontSaveToLayout)
          arrayList.Add((object) containerWindow);
      }
      foreach (View view in objectsOfTypeAll3)
      {
        if (!((UnityEngine.Object) view.window != (UnityEngine.Object) null) || !view.window.m_DontSaveToLayout)
          arrayList.Add((object) view);
      }
      foreach (EditorWindow editorWindow in objectsOfTypeAll1)
      {
        if (!((UnityEngine.Object) editorWindow.m_Parent != (UnityEngine.Object) null) || !((UnityEngine.Object) editorWindow.m_Parent.window != (UnityEngine.Object) null) || !editorWindow.m_Parent.window.m_DontSaveToLayout)
          arrayList.Add((object) editorWindow);
      }
      InternalEditorUtility.SaveToSerializedFileAndForget(arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[], path, true);
    }

    public static void EnsureMainWindowHasBeenLoaded()
    {
      if (UnityEngine.Resources.FindObjectsOfTypeAll<MainView>().Length != 0)
        return;
      MainView.MakeMain();
    }

    internal static MainView FindMainView()
    {
      MainView[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll<MainView>();
      if (objectsOfTypeAll.Length != 0)
        return objectsOfTypeAll[0];
      UnityEngine.Debug.LogError((object) "No Main View found!");
      return (MainView) null;
    }

    public static void SaveGUI()
    {
      Rect screenPosition = WindowLayout.FindMainView().screenPosition;
      EditorWindow.GetWindowWithRect<SaveWindowLayout>(new Rect(screenPosition.xMax - 180f, screenPosition.y + 20f, 200f, 48f), true, "Save Window Layout").m_Parent.window.m_DontSaveToLayout = true;
    }

    private static void RevertFactorySettings()
    {
      InternalEditorUtility.RevertFactoryLayoutSettings(true);
    }

    internal static string layoutsPreferencesPath
    {
      get
      {
        return InternalEditorUtility.unityPreferencesFolder + "/Layouts";
      }
    }

    internal static string layoutsProjectPath
    {
      get
      {
        return Directory.GetCurrentDirectory() + "/Library";
      }
    }

    public static void DeleteGUI()
    {
      Rect screenPosition = WindowLayout.FindMainView().screenPosition;
      EditorWindow.GetWindowWithRect<DeleteWindowLayout>(new Rect(screenPosition.xMax - 180f, screenPosition.y + 20f, 200f, 150f), true, "Delete Window Layout").m_Parent.window.m_DontSaveToLayout = true;
    }
  }
}
