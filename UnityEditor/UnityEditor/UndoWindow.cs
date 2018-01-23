// Decompiled with JetBrains decompiler
// Type: UnityEditor.UndoWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class UndoWindow : EditorWindow
  {
    private List<string> undos = new List<string>();
    private List<string> redos = new List<string>();
    private List<string> newUndos = new List<string>();
    private List<string> newRedos = new List<string>();
    private Vector2 undosScroll = Vector2.zero;
    private Vector2 redosScroll = Vector2.zero;

    internal static void Init()
    {
      EditorWindow.GetWindow(typeof (UndoWindow)).titleContent = new GUIContent("Undo");
    }

    private void Update()
    {
      Undo.GetRecords(this.newUndos, this.newRedos);
      if (this.undos.SequenceEqual<string>((IEnumerable<string>) this.newUndos) && this.redos.SequenceEqual<string>((IEnumerable<string>) this.newRedos))
        return;
      this.undos = new List<string>((IEnumerable<string>) this.newUndos);
      this.redos = new List<string>((IEnumerable<string>) this.newRedos);
      this.Repaint();
    }

    private void OnGUI()
    {
      GUILayout.Label("(Available only in Developer builds)", EditorStyles.boldLabel, new GUILayoutOption[0]);
      float minHeight = this.position.height - 60f;
      float minWidth = (float) ((double) this.position.width * 0.5 - 5.0);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Label("Undos");
      this.undosScroll = GUILayout.BeginScrollView(this.undosScroll, EditorStyles.helpBox, new GUILayoutOption[2]
      {
        GUILayout.MinHeight(minHeight),
        GUILayout.MinWidth(minWidth)
      });
      int num1 = 0;
      foreach (string undo in this.undos)
        GUILayout.Label(string.Format("[{0}] - {1}", (object) num1++, (object) undo));
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label("Redos");
      this.redosScroll = GUILayout.BeginScrollView(this.redosScroll, EditorStyles.helpBox, new GUILayoutOption[2]
      {
        GUILayout.MinHeight(minHeight),
        GUILayout.MinWidth(minWidth)
      });
      int num2 = 0;
      foreach (string redo in this.redos)
        GUILayout.Label(string.Format("[{0}] - {1}", (object) num2++, (object) redo));
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }
  }
}
