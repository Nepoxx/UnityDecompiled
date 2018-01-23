// Decompiled with JetBrains decompiler
// Type: UnityEditor.WindowFocusState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class WindowFocusState : ScriptableObject
  {
    internal string m_LastWindowTypeInSameDock = "";
    internal bool m_WasMaximizedBeforePlay = false;
    internal bool m_CurrentlyInPlayMode = false;
    private static WindowFocusState m_Instance;

    internal static WindowFocusState instance
    {
      get
      {
        if ((Object) WindowFocusState.m_Instance == (Object) null)
          WindowFocusState.m_Instance = Object.FindObjectOfType(typeof (WindowFocusState)) as WindowFocusState;
        if ((Object) WindowFocusState.m_Instance == (Object) null)
          WindowFocusState.m_Instance = ScriptableObject.CreateInstance<WindowFocusState>();
        return WindowFocusState.m_Instance;
      }
    }

    private void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
      WindowFocusState.m_Instance = this;
    }
  }
}
