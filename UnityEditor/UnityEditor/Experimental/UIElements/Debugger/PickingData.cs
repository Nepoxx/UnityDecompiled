// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.PickingData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class PickingData
  {
    private readonly List<UIElementsDebugger.ViewPanel> m_Panels;
    private GUIContent[] m_Labels;
    public Rect screenRect;
    private int m_Selected;

    public PickingData()
    {
      this.m_Panels = new List<UIElementsDebugger.ViewPanel>();
      this.Refresh();
    }

    internal bool Draw(ref UIElementsDebugger.ViewPanel? selectedPanel, Rect dataScreenRect)
    {
      foreach (UIElementsDebugger.ViewPanel panel in this.m_Panels)
      {
        Rect screenPosition = panel.View.screenPosition;
        screenPosition.x -= dataScreenRect.xMin;
        screenPosition.y -= dataScreenRect.yMin;
        if (GUI.Button(screenPosition, string.Format("{0}({1})", (object) panel.Panel.visualTree.name, (object) panel.View.GetInstanceID()), EditorStyles.miniButton))
        {
          selectedPanel = new UIElementsDebugger.ViewPanel?(panel);
          return true;
        }
        PickingData.DrawRect(screenPosition, Color.white);
      }
      return false;
    }

    public static void DrawRect(Rect sp, Color c)
    {
      ++sp.xMin;
      --sp.xMax;
      ++sp.yMin;
      --sp.yMax;
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.Begin(1);
      GL.Color(c);
      GL.Vertex3(sp.xMin, sp.yMin, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMax, sp.yMin, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMax, sp.yMin, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMax, sp.yMax, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMax, sp.yMax, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMin, sp.yMax, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMin, sp.yMax, 0.0f);
      GL.Color(c);
      GL.Vertex3(sp.xMin, sp.yMin, 0.0f);
      GL.End();
      GL.PopMatrix();
    }

    public void Refresh()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PickingData.\u003CRefresh\u003Ec__AnonStorey0 refreshCAnonStorey0 = new PickingData.\u003CRefresh\u003Ec__AnonStorey0();
      this.m_Panels.Clear();
      // ISSUE: reference to a compiler-generated field
      refreshCAnonStorey0.it = UIElementsUtility.GetPanelsIterator();
      List<GUIView> guiViewList = new List<GUIView>();
      GUIViewDebuggerHelper.GetViews(guiViewList);
      bool flag = false;
      Rect rect = new Rect(float.MaxValue, float.MaxValue, 0.0f, 0.0f);
      // ISSUE: reference to a compiler-generated field
      while (refreshCAnonStorey0.it.MoveNext())
      {
        // ISSUE: reference to a compiler-generated method
        GUIView guiView = guiViewList.FirstOrDefault<GUIView>(new Func<GUIView, bool>(refreshCAnonStorey0.\u003C\u003Em__0));
        if (!((UnityEngine.Object) guiView == (UnityEngine.Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Panels.Add(new UIElementsDebugger.ViewPanel()
          {
            Panel = refreshCAnonStorey0.it.Current.Value,
            View = guiView
          });
          if ((double) rect.xMin > (double) guiView.screenPosition.xMin)
            rect.xMin = guiView.screenPosition.xMin;
          if ((double) rect.yMin > (double) guiView.screenPosition.yMin)
            rect.yMin = guiView.screenPosition.yMin;
          if ((double) rect.xMax < (double) guiView.screenPosition.xMax || !flag)
            rect.xMax = guiView.screenPosition.xMax;
          if ((double) rect.yMax < (double) guiView.screenPosition.yMax || !flag)
            rect.yMax = guiView.screenPosition.yMax;
          flag = true;
        }
      }
      this.m_Labels = new GUIContent[this.m_Panels.Count + 1];
      this.m_Labels[0] = new GUIContent("Select a panel");
      for (int index = 0; index < this.m_Panels.Count; ++index)
        this.m_Labels[index + 1] = new GUIContent(PickingData.GetName(this.m_Panels[index]));
      this.screenRect = rect;
    }

    internal static string GetName(UIElementsDebugger.ViewPanel viewPanel)
    {
      HostView view = viewPanel.View as HostView;
      if ((UnityEngine.Object) view != (UnityEngine.Object) null)
      {
        EditorWindow actualView = view.actualView;
        if ((UnityEngine.Object) actualView != (UnityEngine.Object) null)
        {
          if (!string.IsNullOrEmpty(actualView.name))
            return actualView.name;
          return actualView.GetType().Name;
        }
        if (!string.IsNullOrEmpty(view.name))
          return view.name;
      }
      return viewPanel.Panel.visualTree.name;
    }

    public void DoSelectDropDown()
    {
      this.m_Selected = EditorGUILayout.Popup(this.m_Selected, this.m_Labels, EditorStyles.popup, new GUILayoutOption[0]);
    }

    internal UIElementsDebugger.ViewPanel? Selected
    {
      get
      {
        if (this.m_Selected == 0 || this.m_Selected > this.m_Panels.Count)
          return new UIElementsDebugger.ViewPanel?();
        return new UIElementsDebugger.ViewPanel?(this.m_Panels[this.m_Selected - 1]);
      }
    }

    public bool TryRestoreSelectedWindow(string lastWindowTitle)
    {
      for (int index = 0; index < this.m_Labels.Length; ++index)
      {
        if (this.m_Labels[index].text == lastWindowTitle)
        {
          this.m_Selected = index;
          return true;
        }
      }
      return false;
    }
  }
}
