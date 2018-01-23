// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProfilerIPWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ProfilerIPWindow : EditorWindow
  {
    internal bool didFocus = false;
    private const string kTextFieldId = "IPWindow";
    private const string kLastIP = "ProfilerLastIP";
    internal string m_IPString;

    public static void Show(Rect buttonScreenRect)
    {
      Rect rect = new Rect(buttonScreenRect.x, buttonScreenRect.yMax, 300f, 50f);
      ProfilerIPWindow windowWithRect = EditorWindow.GetWindowWithRect<ProfilerIPWindow>(rect, true, "Enter Player IP");
      windowWithRect.position = rect;
      windowWithRect.m_Parent.window.m_DontSaveToLayout = true;
    }

    private void OnEnable()
    {
      this.m_IPString = ProfilerIPWindow.GetLastIPString();
    }

    public static string GetLastIPString()
    {
      return EditorPrefs.GetString("ProfilerLastIP", "");
    }

    private void OnGUI()
    {
      Event current = Event.current;
      bool flag = current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter);
      GUI.SetNextControlName("IPWindow");
      EditorGUILayout.BeginVertical();
      GUILayout.Space(5f);
      this.m_IPString = EditorGUILayout.TextField(this.m_IPString);
      if (!this.didFocus)
      {
        this.didFocus = true;
        EditorGUI.FocusTextInControl("IPWindow");
      }
      GUI.enabled = this.m_IPString.Length != 0;
      if (GUILayout.Button("Connect") || flag)
      {
        this.Close();
        EditorPrefs.SetString("ProfilerLastIP", this.m_IPString);
        AttachProfilerUI.DirectIPConnect(this.m_IPString);
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.EndVertical();
    }
  }
}
