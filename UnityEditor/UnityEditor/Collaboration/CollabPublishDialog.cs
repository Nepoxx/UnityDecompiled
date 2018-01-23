// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabPublishDialog
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal class CollabPublishDialog : EditorWindow
  {
    private static GUIContent DescribeChangesText = EditorGUIUtility.TextContent("Describe your changes here");
    private static GUIContent ChangeAssetsText = EditorGUIUtility.TextContent("Changed assets:");
    private static GUIContent PublishText = EditorGUIUtility.TextContent("Publish");
    private static GUIContent CancelText = EditorGUIUtility.TextContent("Cancel");
    public Vector2 scrollView;
    public string Changelist;
    public PublishDialogOptions Options;

    public CollabPublishDialog()
    {
      this.Options.Comments = "";
    }

    public static CollabPublishDialog ShowCollabWindow(string changelist)
    {
      CollabPublishDialog instance = ScriptableObject.CreateInstance<CollabPublishDialog>();
      instance.Changelist = changelist;
      Rect rect = new Rect(100f, 100f, 600f, 225f);
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
      GUILayout.Label(CollabPublishDialog.DescribeChangesText);
      this.Options.Comments = GUILayout.TextArea(this.Options.Comments, 1000, new GUILayoutOption[1]
      {
        GUILayout.MinHeight(80f)
      });
      GUILayout.Label(CollabPublishDialog.ChangeAssetsText);
      this.scrollView = EditorGUILayout.BeginScrollView(this.scrollView, false, false, new GUILayoutOption[0]);
      EditorGUILayout.SelectableLabel(this.Changelist, EditorStyles.textField, GUILayout.ExpandHeight(true), GUILayout.MinHeight(new GUIStyle().CalcSize(new GUIContent(this.Changelist)).y));
      EditorGUILayout.EndScrollView();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(CollabPublishDialog.CancelText))
      {
        this.Options.DoPublish = false;
        this.Close();
      }
      if (GUILayout.Button(CollabPublishDialog.PublishText))
      {
        this.Options.DoPublish = true;
        this.Close();
      }
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }
  }
}
