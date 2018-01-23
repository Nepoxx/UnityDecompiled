// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorApplicationLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class EditorApplicationLayout
  {
    private static GameView m_GameView = (GameView) null;
    private static bool m_MaximizePending = false;

    internal static bool IsInitializingPlaymodeLayout()
    {
      return (Object) EditorApplicationLayout.m_GameView != (Object) null;
    }

    internal static void SetPlaymodeLayout()
    {
      EditorApplicationLayout.InitPlaymodeLayout();
      EditorApplicationLayout.FinalizePlaymodeLayout();
    }

    internal static void SetStopmodeLayout()
    {
      WindowLayout.ShowAppropriateViewOnEnterExitPlaymode(false);
      Toolbar.RepaintToolbar();
    }

    internal static void SetPausemodeLayout()
    {
      EditorApplicationLayout.SetStopmodeLayout();
    }

    internal static void InitPlaymodeLayout()
    {
      EditorApplicationLayout.m_GameView = WindowLayout.ShowAppropriateViewOnEnterExitPlaymode(true) as GameView;
      if ((Object) EditorApplicationLayout.m_GameView == (Object) null)
        return;
      if (EditorApplicationLayout.m_GameView.maximizeOnPlay)
      {
        DockArea parent = EditorApplicationLayout.m_GameView.m_Parent as DockArea;
        if ((Object) parent != (Object) null)
          EditorApplicationLayout.m_MaximizePending = WindowLayout.MaximizePrepare(parent.actualView);
      }
      EditorApplicationLayout.m_GameView.m_Parent.SetAsStartView();
      Toolbar.RepaintToolbar();
    }

    internal static void FinalizePlaymodeLayout()
    {
      if ((Object) EditorApplicationLayout.m_GameView != (Object) null)
      {
        if (EditorApplicationLayout.m_MaximizePending)
          WindowLayout.MaximizePresent((EditorWindow) EditorApplicationLayout.m_GameView);
        EditorApplicationLayout.m_GameView.m_Parent.ClearStartView();
      }
      EditorApplicationLayout.Clear();
    }

    private static void Clear()
    {
      EditorApplicationLayout.m_MaximizePending = false;
      EditorApplicationLayout.m_GameView = (GameView) null;
    }
  }
}
