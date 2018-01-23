// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.Collab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor.Connect;
using UnityEditor.SceneManagement;
using UnityEditor.Web;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
  [InitializeOnLoad]
  internal sealed class Collab : AssetPostprocessor
  {
    [SerializeField]
    public CollabFilters collabFilters = new CollabFilters();
    private static bool s_IsFirstStateChange = true;
    public static string[] clientType = new string[2]{ "Cloud Server", "Mock Server" };
    internal static string editorPrefCollabClientType = "CollabConfig_Client";
    private static Collab s_Instance = new Collab();
    public string[] currentProjectBrowserSelection;

    static Collab()
    {
      Collab.s_Instance.projectBrowserSingleSelectionPath = string.Empty;
      Collab.s_Instance.projectBrowserSingleMetaSelectionPath = string.Empty;
      JSProxyMgr.GetInstance().AddGlobalObject("unity/collab", (object) Collab.s_Instance);
      // ISSUE: reference to a compiler-generated field
      if (Collab.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Collab.\u003C\u003Ef__mg\u0024cache0 = new ObjectListArea.OnAssetIconDrawDelegate(CollabProjectHook.OnProjectWindowIconOverlay);
      }
      // ISSUE: reference to a compiler-generated field
      ObjectListArea.postAssetIconDrawCallback += Collab.\u003C\u003Ef__mg\u0024cache0;
      // ISSUE: reference to a compiler-generated field
      if (Collab.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Collab.\u003C\u003Ef__mg\u0024cache1 = new AssetsTreeViewGUI.OnAssetIconDrawDelegate(CollabProjectHook.OnProjectBrowserNavPanelIconOverlay);
      }
      // ISSUE: reference to a compiler-generated field
      AssetsTreeViewGUI.postAssetIconDrawCallback += Collab.\u003C\u003Ef__mg\u0024cache1;
      Collab.InitializeSoftlocksViewController();
      Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
      Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> dictionary = statusNotifier = CollabSettingsManager.statusNotifier;
      int num = 0;
      CollabSettingsManager.SettingStatusChanged settingStatusChanged1 = dictionary[CollabSettingType.InProgressEnabled];
      // ISSUE: reference to a compiler-generated field
      if (Collab.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Collab.\u003C\u003Ef__mg\u0024cache2 = new CollabSettingsManager.SettingStatusChanged(Collab.OnSettingStatusChanged);
      }
      // ISSUE: reference to a compiler-generated field
      CollabSettingsManager.SettingStatusChanged fMgCache2 = Collab.\u003C\u003Ef__mg\u0024cache2;
      CollabSettingsManager.SettingStatusChanged settingStatusChanged2 = settingStatusChanged1 + fMgCache2;
      statusNotifier[(CollabSettingType) num] = settingStatusChanged2;
      CollabSettingsManager.statusNotifier[CollabSettingType.InProgressEnabled] += new CollabSettingsManager.SettingStatusChanged(SoftlockViewController.Instance.softLockFilters.OnSettingStatusChanged);
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetProjectPath();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetProjectGUID();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsConnected();

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool AnyJobRunning();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool JobRunning(int a_jobID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Disconnect();

    public ProgressInfo GetJobProgress(int jobId)
    {
      return this.GetJobProgressByType(jobId);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ProgressInfo GetJobProgressByType(int jobType);

    [ExcludeFromDocs]
    public void CancelJob(int jobId)
    {
      bool forceCancel = false;
      this.CancelJob(jobId, forceCancel);
    }

    public void CancelJob(int jobId, [DefaultValue("false")] bool forceCancel)
    {
      this.CancelJobByType(jobId, forceCancel);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CancelJobByType(int jobType, [DefaultValue("false")] bool forceCancel);

    [ExcludeFromDocs]
    public void CancelJobByType(int jobType)
    {
      bool forceCancel = false;
      this.CancelJobByType(jobType, forceCancel);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern long GetAssetStateInternal(string guid);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern long GetSelectedAssetStateInternal();

    public extern CollabInfo collabInfo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DoInitialCommit();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetSeat(bool value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ShouldDoInitialCommit();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Publish(string comment, [DefaultValue("false")] bool useSelectedAssets, [DefaultValue("false")] bool confirmMatchesPrevious);

    [ExcludeFromDocs]
    public void Publish(string comment, bool useSelectedAssets)
    {
      bool confirmMatchesPrevious = false;
      this.Publish(comment, useSelectedAssets, confirmMatchesPrevious);
    }

    [ExcludeFromDocs]
    public void Publish(string comment)
    {
      bool confirmMatchesPrevious = false;
      bool useSelectedAssets = false;
      this.Publish(comment, useSelectedAssets, confirmMatchesPrevious);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ValidateSelectiveCommit();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Update(string revisionID, bool updateToRevision);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RevertFile(string path, bool forceOverwrite);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Change[] GetCollabConflicts();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SetConflictResolvedMine(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SetConflictsResolvedMine(string[] paths);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SetConflictResolvedTheirs(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SetConflictsResolvedTheirs(string[] paths);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ClearConflictResolved(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool ClearConflictsResolved(string[] paths);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void LaunchConflictExternalMerge(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CheckConflictsResolvedExternal();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ShowConflictDifferences(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ShowDifferences(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Collab.CollabStateID GetCollabState();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Change[] GetChangesToPublishInternal();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ResyncSnapshot();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void GoBackToRevision(string revisionID, bool updateToRevision);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SendNotification();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ResyncToRevision(string revisionID);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetError(int filter, out int code, out int priority, out int behaviour, out string errorMsg, out string errorShortMsg);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetError(int errorCode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearError(int errorCode);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearErrors();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetCollabEnabledForCurrentProject(bool enabled);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool IsCollabEnabledForCurrentProject();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern SoftLock[] GetSoftLocks(string assetGuid);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Revision[] GetRevisions();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool AreTestsRunning();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetTestsRunning(bool running);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearAllFailures();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DeleteTempFile(string path, CollabTempFolder folderMask);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void FailNextOperation(int operation, int code);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void TimeOutNextOperation(int operation, int timeOutSec);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearNextOperationFailure();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void FailNextOperationForFile(string path, int operation, int code);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void TimeOutNextOperationForFile(string path, int operation, int timeOutSec);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearNextOperationFailureForFile(string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetGUIDForTests();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void NewGUIDForTests();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void TestPostSoftLockAsCollaborator(string projectGuid, string projectPath, string machineGuid, string assetGuid);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void TestClearSoftLockAsCollaborator(string projectGuid, string projectPath, string machineGuid, string softLockHash);

    public event StateChangedDelegate StateChanged;

    public string projectBrowserSingleSelectionPath { get; set; }

    public string projectBrowserSingleMetaSelectionPath { get; set; }

    public static string GetProjectClientType()
    {
      string configValue = EditorUserSettings.GetConfigValue(Collab.editorPrefCollabClientType);
      return !string.IsNullOrEmpty(configValue) ? configValue : Collab.clientType[0];
    }

    [MenuItem("Window/Collab/Get Revisions", false, 1000, true)]
    public static void TestGetRevisions()
    {
      Revision[] revisions = Collab.instance.GetRevisions();
      if (revisions.Length == 0)
      {
        Debug.Log((object) "No revisions");
      }
      else
      {
        int length = revisions.Length;
        foreach (Revision revision in revisions)
        {
          Debug.Log((object) ("Revision #" + (object) length + ": " + revision.revisionID));
          --length;
        }
      }
    }

    public static Collab instance
    {
      get
      {
        return Collab.s_Instance;
      }
    }

    public static void OnSettingStatusChanged(CollabSettingType type, CollabSettingStatus status)
    {
      Collab.InitializeSoftlocksViewController();
    }

    public static bool InitializeSoftlocksViewController()
    {
      if (!CollabSettingsManager.IsAvailable(CollabSettingType.InProgressEnabled))
        return false;
      if (CollabSettingsManager.inProgressEnabled)
        SoftlockViewController.Instance.TurnOn();
      else
        SoftlockViewController.Instance.TurnOff();
      return true;
    }

    public void CancelJobWithoutException(int jobType)
    {
      try
      {
        this.CancelJobByType(jobType);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Cannot cancel job, reason:" + ex.Message));
      }
    }

    public Collab.CollabStates GetAssetState(string guid)
    {
      return (Collab.CollabStates) this.GetAssetStateInternal(guid);
    }

    public Collab.CollabStates GetSelectedAssetState()
    {
      return (Collab.CollabStates) this.GetSelectedAssetStateInternal();
    }

    public void UpdateEditorSelectionCache()
    {
      List<string> stringList = new List<string>();
      foreach (string guid in Selection.assetGUIDsDeepSelection)
      {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        stringList.Add(assetPath);
        string path = assetPath + ".meta";
        if (File.Exists(path))
          stringList.Add(path);
      }
      this.currentProjectBrowserSelection = stringList.ToArray();
    }

    public CollabInfo GetCollabInfo()
    {
      return this.collabInfo;
    }

    public static bool IsDiffToolsAvailable()
    {
      return InternalEditorUtility.GetAvailableDiffTools().Length > 0;
    }

    public void SaveAssets()
    {
      AssetDatabase.SaveAssets();
    }

    public static void SwitchToDefaultMode()
    {
      bool flag = EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D;
      SceneView lastActiveSceneView = SceneView.lastActiveSceneView;
      if (!((UnityEngine.Object) lastActiveSceneView != (UnityEngine.Object) null) || lastActiveSceneView.in2DMode == flag)
        return;
      lastActiveSceneView.in2DMode = flag;
    }

    public void ShowInProjectBrowser(string filterString)
    {
      this.collabFilters.ShowInProjectBrowser(filterString);
    }

    public PublishInfo GetChangesToPublish()
    {
      Change[] toPublishInternal = this.GetChangesToPublishInternal();
      return new PublishInfo() { changes = toPublishInternal, filter = false };
    }

    private static void OnStateChanged()
    {
      if (Collab.s_IsFirstStateChange)
      {
        Collab.s_IsFirstStateChange = false;
        UnityConnect instance = UnityConnect.instance;
        // ISSUE: reference to a compiler-generated field
        if (Collab.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Collab.\u003C\u003Ef__mg\u0024cache3 = new UnityEditor.Connect.StateChangedDelegate(Collab.OnUnityConnectStateChanged);
        }
        // ISSUE: reference to a compiler-generated field
        UnityEditor.Connect.StateChangedDelegate fMgCache3 = Collab.\u003C\u003Ef__mg\u0024cache3;
        instance.StateChanged += fMgCache3;
      }
      // ISSUE: reference to a compiler-generated field
      StateChangedDelegate stateChanged = Collab.instance.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged(Collab.instance.collabInfo);
    }

    private static void PublishDialog(string changelist)
    {
      if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        return;
      CollabPublishDialog collabPublishDialog = CollabPublishDialog.ShowCollabWindow(changelist);
      if (!collabPublishDialog.Options.DoPublish)
        return;
      Collab.instance.Publish(collabPublishDialog.Options.Comments, true);
    }

    private static void CannotPublishDialog(string infoMessage)
    {
      CollabCannotPublishDialog.ShowCollabWindow(infoMessage);
    }

    private static void OnUnityConnectStateChanged(ConnectInfo state)
    {
      Collab.instance.SendNotification();
    }

    public static void OnProgressEnabledSettingStatusChanged(CollabSettingType type, CollabSettingStatus status)
    {
      if (type != CollabSettingType.InProgressEnabled || status != CollabSettingStatus.Available)
        return;
      if (CollabSettingsManager.inProgressEnabled)
        SoftlockViewController.Instance.softLockFilters.ShowInFavoriteSearchFilters();
      Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
      Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> dictionary = statusNotifier = CollabSettingsManager.statusNotifier;
      int num = 0;
      CollabSettingsManager.SettingStatusChanged settingStatusChanged1 = dictionary[CollabSettingType.InProgressEnabled];
      // ISSUE: reference to a compiler-generated field
      if (Collab.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Collab.\u003C\u003Ef__mg\u0024cache4 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
      }
      // ISSUE: reference to a compiler-generated field
      CollabSettingsManager.SettingStatusChanged fMgCache4 = Collab.\u003C\u003Ef__mg\u0024cache4;
      CollabSettingsManager.SettingStatusChanged settingStatusChanged2 = settingStatusChanged1 - fMgCache4;
      statusNotifier[(CollabSettingType) num] = settingStatusChanged2;
    }

    [RequiredByNativeCode]
    private static void OnCollabEnabledForCurrentProject(bool enabled)
    {
      if (enabled)
      {
        Collab.instance.StateChanged += new StateChangedDelegate(Collab.instance.collabFilters.OnCollabStateChanged);
        Collab.instance.collabFilters.ShowInFavoriteSearchFilters();
        if (CollabSettingsManager.IsAvailable(CollabSettingType.InProgressEnabled))
        {
          if (!CollabSettingsManager.inProgressEnabled)
            return;
          SoftlockViewController.Instance.softLockFilters.ShowInFavoriteSearchFilters();
        }
        else
        {
          Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier1;
          Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> dictionary1 = statusNotifier1 = CollabSettingsManager.statusNotifier;
          int num1 = 0;
          CollabSettingsManager.SettingStatusChanged settingStatusChanged1 = dictionary1[CollabSettingType.InProgressEnabled];
          // ISSUE: reference to a compiler-generated field
          if (Collab.\u003C\u003Ef__mg\u0024cache5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Collab.\u003C\u003Ef__mg\u0024cache5 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
          }
          // ISSUE: reference to a compiler-generated field
          CollabSettingsManager.SettingStatusChanged fMgCache5 = Collab.\u003C\u003Ef__mg\u0024cache5;
          CollabSettingsManager.SettingStatusChanged settingStatusChanged2 = settingStatusChanged1 - fMgCache5;
          statusNotifier1[(CollabSettingType) num1] = settingStatusChanged2;
          Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier2;
          Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> dictionary2 = statusNotifier2 = CollabSettingsManager.statusNotifier;
          int num2 = 0;
          CollabSettingsManager.SettingStatusChanged settingStatusChanged3 = dictionary2[CollabSettingType.InProgressEnabled];
          // ISSUE: reference to a compiler-generated field
          if (Collab.\u003C\u003Ef__mg\u0024cache6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Collab.\u003C\u003Ef__mg\u0024cache6 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
          }
          // ISSUE: reference to a compiler-generated field
          CollabSettingsManager.SettingStatusChanged fMgCache6 = Collab.\u003C\u003Ef__mg\u0024cache6;
          CollabSettingsManager.SettingStatusChanged settingStatusChanged4 = settingStatusChanged3 + fMgCache6;
          statusNotifier2[(CollabSettingType) num2] = settingStatusChanged4;
        }
      }
      else
      {
        Collab.instance.StateChanged -= new StateChangedDelegate(Collab.instance.collabFilters.OnCollabStateChanged);
        Collab.instance.collabFilters.HideFromFavoriteSearchFilters();
        SoftlockViewController.Instance.softLockFilters.HideFromFavoriteSearchFilters();
        Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
        Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> dictionary = statusNotifier = CollabSettingsManager.statusNotifier;
        int num = 0;
        CollabSettingsManager.SettingStatusChanged settingStatusChanged1 = dictionary[CollabSettingType.InProgressEnabled];
        // ISSUE: reference to a compiler-generated field
        if (Collab.\u003C\u003Ef__mg\u0024cache7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Collab.\u003C\u003Ef__mg\u0024cache7 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
        }
        // ISSUE: reference to a compiler-generated field
        CollabSettingsManager.SettingStatusChanged fMgCache7 = Collab.\u003C\u003Ef__mg\u0024cache7;
        CollabSettingsManager.SettingStatusChanged settingStatusChanged2 = settingStatusChanged1 - fMgCache7;
        statusNotifier[(CollabSettingType) num] = settingStatusChanged2;
        if ((UnityEngine.Object) ProjectBrowser.s_LastInteractedProjectBrowser != (UnityEngine.Object) null)
        {
          if (ProjectBrowser.s_LastInteractedProjectBrowser.Initialized() && ProjectBrowser.s_LastInteractedProjectBrowser.IsTwoColumns())
          {
            int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID("assets");
            ProjectBrowser.s_LastInteractedProjectBrowser.SetFolderSelection(new int[1]
            {
              mainAssetInstanceId
            }, true);
          }
          ProjectBrowser.s_LastInteractedProjectBrowser.SetSearch("");
          ProjectBrowser.s_LastInteractedProjectBrowser.Repaint();
        }
      }
    }

    [System.Flags]
    public enum CollabStates : ulong
    {
      kCollabNone = 0,
      kCollabLocal = 1,
      kCollabSynced = 2,
      kCollabOutOfSync = 4,
      kCollabIgnored = 8,
      kCollabCheckedOutLocal = 16, // 0x0000000000000010
      kCollabCheckedOutRemote = 32, // 0x0000000000000020
      kCollabDeletedLocal = 64, // 0x0000000000000040
      kCollabDeletedRemote = 128, // 0x0000000000000080
      kCollabAddedLocal = 256, // 0x0000000000000100
      kCollabAddedRemote = 512, // 0x0000000000000200
      kCollabConflicted = 1024, // 0x0000000000000400
      kCollabMovedLocal = 2048, // 0x0000000000000800
      kCollabMovedRemote = 4096, // 0x0000000000001000
      kCollabUpdating = 8192, // 0x0000000000002000
      kCollabReadOnly = 16384, // 0x0000000000004000
      kCollabMetaFile = 32768, // 0x0000000000008000
      kCollabUseMine = 65536, // 0x0000000000010000
      kCollabUseTheir = 131072, // 0x0000000000020000
      kCollabMerged = 262144, // 0x0000000000040000
      kCollabPendingMerge = 524288, // 0x0000000000080000
      kCollabFolderMetaFile = 1048576, // 0x0000000000100000
      KCollabContentChanged = 2097152, // 0x0000000000200000
      KCollabContentConflicted = 4194304, // 0x0000000000400000
      KCollabContentDeleted = 8388608, // 0x0000000000800000
      kCollabInvalidState = 1073741824, // 0x0000000040000000
      kAnyLocalChanged = kCollabMovedLocal | kCollabAddedLocal | kCollabDeletedLocal | kCollabCheckedOutLocal, // 0x0000000000000950
      kAnyLocalEdited = kCollabMovedLocal | kCollabAddedLocal | kCollabCheckedOutLocal, // 0x0000000000000910
    }

    internal enum CollabStateID
    {
      None,
      Uninitialized,
      Initialized,
    }
  }
}
