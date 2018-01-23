// Decompiled with JetBrains decompiler
// Type: UnityEditor.PragmaFixingWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEditor.Scripting;
using UnityEngine;

namespace UnityEditor
{
  internal class PragmaFixingWindow : EditorWindow
  {
    private static PragmaFixingWindow.Styles s_Styles = (PragmaFixingWindow.Styles) null;
    private ListViewState m_LV = new ListViewState();
    private string[] m_Paths;

    public PragmaFixingWindow()
    {
      this.titleContent = new GUIContent("Unity - #pragma fixing");
    }

    public static void ShowWindow(string[] paths)
    {
      PragmaFixingWindow window = EditorWindow.GetWindow<PragmaFixingWindow>(true);
      window.SetPaths(paths);
      window.ShowModal();
    }

    public void SetPaths(string[] paths)
    {
      this.m_Paths = paths;
      this.m_LV.totalRows = paths.Length;
    }

    private void OnGUI()
    {
      if (PragmaFixingWindow.s_Styles == null)
      {
        PragmaFixingWindow.s_Styles = new PragmaFixingWindow.Styles();
        this.minSize = new Vector2(450f, 300f);
        this.position = new Rect(this.position.x, this.position.y, this.minSize.x, this.minSize.y);
      }
      GUILayout.Space(10f);
      GUILayout.Label("#pragma implicit and #pragma downcast need to be added to following files\nfor backwards compatibility");
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      IEnumerator enumerator = ListViewGUILayout.ListView(this.m_LV, PragmaFixingWindow.s_Styles.box).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          ListViewElement current = (ListViewElement) enumerator.Current;
          if (current.row == this.m_LV.row && Event.current.type == EventType.Repaint)
            PragmaFixingWindow.s_Styles.selected.Draw(current.position, false, false, false, false);
          GUILayout.Label(this.m_Paths[current.row]);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Fix now", PragmaFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        this.Close();
        PragmaFixing30.FixFiles(this.m_Paths);
        GUIUtility.ExitGUI();
      }
      if (GUILayout.Button("Ignore", PragmaFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        this.Close();
        GUIUtility.ExitGUI();
      }
      if (GUILayout.Button("Quit", PragmaFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        EditorApplication.Exit(0);
        GUIUtility.ExitGUI();
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
    }

    private class Styles
    {
      public GUIStyle selected = (GUIStyle) "OL SelectedRow";
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle button = (GUIStyle) "LargeButton";
    }
  }
}
