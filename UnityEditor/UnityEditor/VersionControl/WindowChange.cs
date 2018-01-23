// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.WindowChange
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  internal class WindowChange : EditorWindow
  {
    private ListControl submitList = new ListControl();
    private AssetList assetList = new AssetList();
    private ChangeSet changeSet = new ChangeSet();
    private string description = string.Empty;
    private bool allowSubmit = false;
    private Task taskStatus = (Task) null;
    private Task taskDesc = (Task) null;
    private Task taskStat = (Task) null;
    private Task taskSubmit = (Task) null;
    private Task taskAdd = (Task) null;
    private int submitResultCode = 256;
    private string submitErrorMessage = (string) null;
    private int m_TextAreaControlID = 0;
    private const int kSubmitNotStartedResultCode = 256;
    private const int kSubmitRunningResultCode = 0;
    private const string c_defaultDescription = "";

    public void OnEnable()
    {
      this.position = new Rect(100f, 100f, 700f, 395f);
      this.minSize = new Vector2(700f, 395f);
      this.submitList.ReadOnly = true;
      this.taskStatus = (Task) null;
      this.taskDesc = (Task) null;
      this.taskStat = (Task) null;
      this.taskSubmit = (Task) null;
      this.submitResultCode = 256;
      this.submitErrorMessage = (string) null;
    }

    public void OnDisable()
    {
      this.m_TextAreaControlID = 0;
    }

    public static void Open(AssetList list, bool submit)
    {
      WindowChange.Open((ChangeSet) null, list, submit);
    }

    public static void Open(ChangeSet change, AssetList assets, bool submit)
    {
      WindowChange window = EditorWindow.GetWindow<WindowChange>(true, "Version Control Changeset");
      window.allowSubmit = submit;
      window.DoOpen(change, assets);
    }

    private string SanitizeDescription(string desc)
    {
      if (Provider.GetActivePlugin() != null && Provider.GetActivePlugin().name != "Perforce")
        return desc;
      int num1 = desc.IndexOf('\'');
      if (num1 == -1)
        return desc;
      int startIndex = num1 + 1;
      int num2 = desc.IndexOf('\'', startIndex);
      if (num2 == -1)
        return desc;
      return desc.Substring(startIndex, num2 - startIndex).Trim(' ', '\t');
    }

    private void DoOpen(ChangeSet change, AssetList assets)
    {
      this.taskSubmit = (Task) null;
      this.submitResultCode = 256;
      this.submitErrorMessage = (string) null;
      this.changeSet = change;
      this.description = change != null ? this.SanitizeDescription(change.description) : "";
      this.assetList = (AssetList) null;
      if (change == null)
      {
        this.taskStatus = Provider.Status(assets);
      }
      else
      {
        this.taskDesc = Provider.ChangeSetDescription(change);
        this.taskStat = Provider.ChangeSetStatus(change);
      }
    }

    private void RefreshList()
    {
      this.submitList.Clear();
      foreach (Asset asset in (List<Asset>) this.assetList)
        this.submitList.Add((ListItem) null, asset.prettyPath, asset);
      if (this.assetList.Count == 0)
      {
        ChangeSet change = new ChangeSet("Empty change list");
        this.submitList.Add((ListItem) null, change.description, change).Dummy = true;
      }
      this.submitList.Refresh();
      this.Repaint();
    }

    internal static void OnSubmitted(Task task)
    {
      WindowChange[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (WindowChange)) as WindowChange[];
      if (objectsOfTypeAll.Length == 0)
        return;
      WindowChange windowChange1 = objectsOfTypeAll[0];
      windowChange1.assetList = task.assetList;
      windowChange1.submitResultCode = task.resultCode;
      windowChange1.submitErrorMessage = (string) null;
      if ((task.resultCode & 2) != 0)
      {
        string str = "";
        foreach (Message message in task.messages)
        {
          if (message.severity == Message.Severity.Error)
          {
            WindowChange windowChange2 = windowChange1;
            windowChange2.submitErrorMessage = windowChange2.submitErrorMessage + str + message.message;
          }
        }
      }
      if ((task.resultCode & 3) != 0)
      {
        WindowPending.UpdateAllWindows();
        if (windowChange1.changeSet == null)
        {
          Provider.Status("").Wait();
          WindowPending.ExpandLatestChangeSet();
        }
      }
      if ((task.resultCode & 1) != 0)
        windowChange1.ResetAndClose();
      else
        windowChange1.RefreshList();
    }

    internal static void OnAdded(Task task)
    {
      WindowChange[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (WindowChange)) as WindowChange[];
      if (objectsOfTypeAll.Length == 0)
        return;
      WindowChange windowChange = objectsOfTypeAll[0];
      windowChange.taskSubmit = (Task) null;
      windowChange.submitResultCode = 256;
      windowChange.submitErrorMessage = (string) null;
      windowChange.taskAdd = (Task) null;
      windowChange.taskStatus = Provider.Status(windowChange.assetList, false);
      windowChange.assetList = (AssetList) null;
      WindowPending.UpdateAllWindows();
    }

    private void OnGUI()
    {
      if ((this.submitResultCode & 4) != 0)
        this.OnConflictingFilesGUI();
      else if ((this.submitResultCode & 8) != 0)
        this.OnUnaddedFilesGUI();
      else if ((this.submitResultCode & 2) != 0)
        this.OnErrorGUI();
      else
        this.OnSubmitGUI();
    }

    private void OnSubmitGUI()
    {
      if (this.submitResultCode != 256)
        GUI.enabled = false;
      Event current = Event.current;
      if (current.isKey && current.keyCode == KeyCode.Escape)
        this.Close();
      GUILayout.Label("Description", EditorStyles.boldLabel, new GUILayoutOption[0]);
      if (this.taskStatus != null && this.taskStatus.resultCode != 0)
      {
        this.assetList = this.taskStatus.assetList.Filter(true, Asset.States.CheckedOutLocal, Asset.States.DeletedLocal, Asset.States.AddedLocal);
        this.RefreshList();
        this.taskStatus = (Task) null;
      }
      else if (this.taskDesc != null && this.taskDesc.resultCode != 0)
      {
        this.description = this.taskDesc.text.Length <= 0 ? "" : this.taskDesc.text;
        if (this.description.Trim() == "<enter description here>")
          this.description = string.Empty;
        this.taskDesc = (Task) null;
      }
      else if (this.taskStat != null && this.taskStat.resultCode != 0)
      {
        this.assetList = this.taskStat.assetList;
        this.RefreshList();
        this.taskStat = (Task) null;
      }
      Task task = this.taskStatus == null || this.taskStatus.resultCode != 0 ? (this.taskDesc == null || this.taskDesc.resultCode != 0 ? (this.taskStat == null || this.taskStat.resultCode != 0 ? this.taskSubmit : this.taskStat) : this.taskDesc) : this.taskStatus;
      GUI.enabled = (this.taskDesc == null || this.taskDesc.resultCode != 0) && this.submitResultCode == 256;
      this.description = EditorGUILayout.TextArea(this.description, GUILayout.Height(150f)).Trim();
      if (this.m_TextAreaControlID == 0)
        this.m_TextAreaControlID = EditorGUIUtility.s_LastControlID;
      if (this.m_TextAreaControlID != 0)
      {
        GUIUtility.keyboardControl = this.m_TextAreaControlID;
        EditorGUIUtility.editingTextField = true;
      }
      GUI.enabled = true;
      GUILayout.Label("Files", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      Rect screenRect = new Rect(6f, 206f, this.position.width - 12f, this.position.height - 248f);
      GUILayout.BeginArea(screenRect);
      GUILayout.Box("", new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      GUILayout.EndArea();
      this.submitList.OnGUI(new Rect(screenRect.x + 2f, screenRect.y + 2f, screenRect.width - 4f, screenRect.height - 4f), true);
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      if (this.submitResultCode == 256)
      {
        if (task != null)
        {
          GUIContent content = GUIContent.Temp("Getting info");
          content.image = WindowPending.StatusWheel.image;
          GUILayout.Label(content);
          content.image = (Texture) null;
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel"))
          this.ResetAndClose();
        GUI.enabled = task == null && !string.IsNullOrEmpty(this.description);
        bool flag = current.isKey && current.shift && current.keyCode == KeyCode.Return;
        if (Provider.hasChangelistSupport && (GUILayout.Button("Save") || flag && !this.allowSubmit))
          this.Save(false);
        if (this.allowSubmit)
        {
          bool enabled = GUI.enabled;
          GUI.enabled = this.assetList != null && this.assetList.Count > 0 && !string.IsNullOrEmpty(this.description);
          if (GUILayout.Button("Submit") || flag)
            this.Save(true);
          GUI.enabled = enabled;
        }
      }
      else
      {
        bool flag = (this.submitResultCode & 1) != 0;
        GUI.enabled = flag;
        string text = "";
        if (flag)
          text = "Finished successfully";
        else if (task != null)
        {
          GUILayout.Label(WindowPending.StatusWheel);
          text = task.progressMessage;
          if (text.Length == 0)
            text = "Running...";
        }
        GUILayout.Label(text);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Close"))
          this.ResetAndClose();
      }
      GUI.enabled = true;
      GUILayout.EndHorizontal();
      GUILayout.Space(12f);
      if (task == null)
        return;
      this.Repaint();
    }

    private void OnErrorGUI()
    {
      GUILayout.Label("Submit failed", EditorStyles.boldLabel, new GUILayoutOption[0]);
      string str = "";
      if (!string.IsNullOrEmpty(this.submitErrorMessage))
        str = this.submitErrorMessage + "\n";
      GUILayout.Label(str + "See console for details. You can get more details by increasing log level in EditorSettings.");
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Close"))
      {
        this.ResetAndClose();
        WindowPending.UpdateAllWindows();
      }
      GUILayout.EndHorizontal();
    }

    private void OnConflictingFilesGUI()
    {
      string text = "";
      foreach (Asset asset in (List<Asset>) this.assetList)
      {
        if (asset.IsState(Asset.States.Conflicted))
          text = text + asset.prettyPath + "\n";
      }
      GUILayout.Label("Conflicting files", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.Label("Some files need to be resolved before submitting:");
      GUI.enabled = false;
      GUILayout.TextArea(text, GUILayout.ExpandHeight(true));
      GUI.enabled = true;
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Close"))
        this.ResetAndClose();
      GUILayout.EndHorizontal();
    }

    private void OnUnaddedFilesGUI()
    {
      AssetList assets = new AssetList();
      string text = "";
      foreach (Asset asset in (List<Asset>) this.assetList)
      {
        if (!asset.IsState(Asset.States.OutOfSync) && !asset.IsState(Asset.States.Synced) && !asset.IsState(Asset.States.AddedLocal))
        {
          text = text + asset.prettyPath + "\n";
          assets.Add(asset);
        }
      }
      GUILayout.Label("Files to add", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.Label("Some additional files need to be added:");
      GUI.enabled = false;
      GUILayout.TextArea(text);
      GUI.enabled = true;
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Add files"))
      {
        this.taskAdd = Provider.Add(assets, false);
        this.taskAdd.SetCompletionAction(CompletionAction.OnAddedChangeWindow);
      }
      if (GUILayout.Button("Abort"))
        this.ResetAndClose();
      GUILayout.EndHorizontal();
    }

    private void ResetAndClose()
    {
      this.taskSubmit = (Task) null;
      this.submitResultCode = 256;
      this.submitErrorMessage = (string) null;
      this.Close();
    }

    private void Save(bool submit)
    {
      if (this.description.Trim() == "")
      {
        Debug.LogError((object) "Version control: Please enter a valid change description");
      }
      else
      {
        AssetDatabase.SaveAssets();
        this.taskSubmit = Provider.Submit(this.changeSet, this.assetList, this.description, !submit);
        this.submitResultCode = 0;
        this.submitErrorMessage = (string) null;
        this.taskSubmit.SetCompletionAction(CompletionAction.OnSubmittedChangeWindow);
      }
    }
  }
}
