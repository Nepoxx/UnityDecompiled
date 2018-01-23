// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabCannotPublishDialog
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal class CollabCannotPublishDialog : EditorWindow
  {
    private static GUIContent WarningText = EditorGUIUtility.TextContent(string.Format("Files that have been moved or in a changed folder cannot be selectively published, please use the Publish option in the collab window to publish all your changes."));
    private static GUIContent IssuesText = EditorGUIUtility.TextContent("Issues:");
    private static GUIContent AcceptText = EditorGUIUtility.TextContent("Accept");
    public Vector2 scrollPosition;
    public string InfoMessage;

    public static CollabCannotPublishDialog ShowCollabWindow(string infoMessage)
    {
      CollabCannotPublishDialog instance = ScriptableObject.CreateInstance<CollabCannotPublishDialog>();
      instance.InfoMessage = infoMessage;
      Rect rect = new Rect(100f, 100f, 600f, 150f);
      instance.minSize = new Vector2(rect.width, rect.height);
      instance.maxSize = new Vector2(rect.width, rect.height);
      instance.position = rect;
      instance.ShowModal();
      instance.m_Parent.window.m_DontSaveToLayout = true;
      return instance;
    }

    public void OnGUI()
    {
      GUILayout.BeginVertical();
      GUI.skin.label.wordWrap = true;
      GUILayout.BeginVertical();
      GUILayout.Label(CollabCannotPublishDialog.WarningText);
      GUILayout.Label(CollabCannotPublishDialog.IssuesText);
      this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
      GUILayout.Label(string.Format(this.InfoMessage), new GUIStyle()
      {
        normal = {
          textColor = new Color(1f, 0.28f, 0.0f)
        }
      }, new GUILayoutOption[0]);
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(CollabCannotPublishDialog.AcceptText))
        this.Close();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }
  }
}
