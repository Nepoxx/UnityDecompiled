// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.WindowRevert
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor.VersionControl
{
  internal class WindowRevert : EditorWindow
  {
    private ListControl revertList = new ListControl();
    private AssetList assetList = new AssetList();

    public void OnEnable()
    {
      this.position = new Rect(100f, 100f, 700f, 230f);
      this.minSize = new Vector2(700f, 230f);
      this.revertList.ReadOnly = true;
    }

    public static void Open(ChangeSet change)
    {
      Task task = Provider.ChangeSetStatus(change);
      task.Wait();
      WindowRevert.GetWindow().DoOpen(task.assetList);
    }

    public static void Open(AssetList assets)
    {
      Task task = Provider.Status(assets);
      task.Wait();
      WindowRevert.GetWindow().DoOpen(task.assetList.Filter(true, Asset.States.CheckedOutLocal, Asset.States.DeletedLocal, Asset.States.AddedLocal, Asset.States.Missing));
    }

    private static WindowRevert GetWindow()
    {
      return EditorWindow.GetWindow<WindowRevert>(true, "Version Control Revert");
    }

    private void DoOpen(AssetList revert)
    {
      this.assetList = revert;
      this.RefreshList();
    }

    private void RefreshList()
    {
      this.revertList.Clear();
      foreach (Asset asset in (List<Asset>) this.assetList)
        this.revertList.Add((ListItem) null, asset.prettyPath, asset);
      if (this.assetList.Count == 0)
      {
        ChangeSet change = new ChangeSet("no files to revert");
        this.revertList.Add((ListItem) null, change.description, change).Dummy = true;
      }
      this.revertList.Refresh();
      this.Repaint();
    }

    private void OnGUI()
    {
      GUILayout.Label("Revert Files", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      Rect screenRect = new Rect(6f, 40f, this.position.width - 12f, this.position.height - 82f);
      GUILayout.BeginArea(screenRect);
      GUILayout.Box("", new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      GUILayout.EndArea();
      this.revertList.OnGUI(new Rect(screenRect.x + 2f, screenRect.y + 2f, screenRect.width - 4f, screenRect.height - 4f), true);
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Cancel"))
        this.Close();
      if (this.assetList.Count > 0 && GUILayout.Button("Revert"))
      {
        string str = "";
        foreach (Asset asset in (List<Asset>) this.assetList)
        {
          Scene sceneByPath = SceneManager.GetSceneByPath(asset.path);
          if (sceneByPath.IsValid() && sceneByPath.isLoaded)
            str = str + sceneByPath.path + "\n";
        }
        if (str.Length > 0 && !EditorUtility.DisplayDialog("Revert open scene(s)?", "You are about to revert your currently open scene(s):\n\n" + str + "\nContinuing will remove all unsaved changes.", "Continue", "Cancel"))
        {
          this.Close();
          return;
        }
        Provider.Revert(this.assetList, RevertMode.Normal).Wait();
        WindowPending.UpdateAllWindows();
        AssetDatabase.Refresh();
        this.Close();
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(12f);
    }
  }
}
