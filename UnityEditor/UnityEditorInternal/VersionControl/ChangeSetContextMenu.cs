// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.ChangeSetContextMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.VersionControl;

namespace UnityEditorInternal.VersionControl
{
  public class ChangeSetContextMenu
  {
    private static ChangeSet GetChangeSet(ChangeSets changes)
    {
      if (changes.Count == 0)
        return (ChangeSet) null;
      return changes[0];
    }

    private static bool SubmitTest(int userData)
    {
      ChangeSets selectedChangeSets = ListControl.FromID(userData).SelectedChangeSets;
      return selectedChangeSets.Count > 0 && Provider.SubmitIsValid(selectedChangeSets[0], (AssetList) null);
    }

    private static void Submit(int userData)
    {
      ChangeSet changeSet = ChangeSetContextMenu.GetChangeSet(ListControl.FromID(userData).SelectedChangeSets);
      if (changeSet == null)
        return;
      WindowChange.Open(changeSet, new AssetList(), true);
    }

    private static bool RevertTest(int userData)
    {
      return ListControl.FromID(userData).SelectedChangeSets.Count > 0;
    }

    private static void Revert(int userData)
    {
      ChangeSet changeSet = ChangeSetContextMenu.GetChangeSet(ListControl.FromID(userData).SelectedChangeSets);
      if (changeSet == null)
        return;
      WindowRevert.Open(changeSet);
    }

    private static bool RevertUnchangedTest(int userData)
    {
      return ListControl.FromID(userData).SelectedChangeSets.Count > 0;
    }

    private static void RevertUnchanged(int userData)
    {
      Provider.RevertChangeSets(ListControl.FromID(userData).SelectedChangeSets, RevertMode.Unchanged).SetCompletionAction(CompletionAction.UpdatePendingWindow);
      Provider.InvalidateCache();
    }

    private static bool ResolveTest(int userData)
    {
      return ListControl.FromID(userData).SelectedChangeSets.Count > 0;
    }

    private static void Resolve(int userData)
    {
      ChangeSet changeSet = ChangeSetContextMenu.GetChangeSet(ListControl.FromID(userData).SelectedChangeSets);
      if (changeSet == null)
        return;
      WindowResolve.Open(changeSet);
    }

    private static bool NewChangeSetTest(int userDatad)
    {
      return Provider.isActive;
    }

    private static void NewChangeSet(int userData)
    {
      WindowChange.Open(new AssetList(), false);
    }

    private static bool EditChangeSetTest(int userData)
    {
      ChangeSets selectedChangeSets = ListControl.FromID(userData).SelectedChangeSets;
      if (selectedChangeSets.Count == 0)
        return false;
      return ChangeSetContextMenu.GetChangeSet(selectedChangeSets).id != "-1" && Provider.SubmitIsValid(selectedChangeSets[0], (AssetList) null);
    }

    private static void EditChangeSet(int userData)
    {
      ChangeSet changeSet = ChangeSetContextMenu.GetChangeSet(ListControl.FromID(userData).SelectedChangeSets);
      if (changeSet == null)
        return;
      WindowChange.Open(changeSet, new AssetList(), false);
    }

    private static bool DeleteChangeSetTest(int userData)
    {
      ListControl listControl = ListControl.FromID(userData);
      ChangeSets selectedChangeSets = listControl.SelectedChangeSets;
      if (selectedChangeSets.Count == 0)
        return false;
      ChangeSet changeSet = ChangeSetContextMenu.GetChangeSet(selectedChangeSets);
      if (changeSet.id == "-1")
        return false;
      ListItem changeSetItem = listControl.GetChangeSetItem(changeSet);
      bool flag = changeSetItem != null && changeSetItem.HasChildren && changeSetItem.FirstChild.Asset != null && changeSetItem.FirstChild.Name != "Empty change list";
      if (!flag)
      {
        Task task = Provider.ChangeSetStatus(changeSet);
        task.Wait();
        flag = task.assetList.Count != 0;
      }
      return !flag && Provider.DeleteChangeSetsIsValid(selectedChangeSets);
    }

    private static void DeleteChangeSet(int userData)
    {
      Provider.DeleteChangeSets(ListControl.FromID(userData).SelectedChangeSets).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }
  }
}
