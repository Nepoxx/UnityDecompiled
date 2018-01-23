// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.ProjectContextMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.VersionControl;

namespace UnityEditorInternal.VersionControl
{
  public class ProjectContextMenu
  {
    private static bool GetLatestTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.GetLatestIsValid(Provider.GetAssetListFromSelection());
    }

    private static void GetLatest(MenuCommand cmd)
    {
      Provider.GetLatest(Provider.GetAssetListFromSelection()).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool SubmitTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.SubmitIsValid((ChangeSet) null, Provider.GetAssetListFromSelection());
    }

    private static void Submit(MenuCommand cmd)
    {
      WindowChange.Open(Provider.GetAssetListFromSelection(), true);
    }

    private static bool CheckOutTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.CheckoutIsValid(Provider.GetAssetListFromSelection(), CheckoutMode.Both);
    }

    private static void CheckOut(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Both);
    }

    private static bool CheckOutAssetTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.CheckoutIsValid(Provider.GetAssetListFromSelection(), CheckoutMode.Asset);
    }

    private static void CheckOutAsset(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Asset);
    }

    private static bool CheckOutMetaTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.CheckoutIsValid(Provider.GetAssetListFromSelection(), CheckoutMode.Meta);
    }

    private static void CheckOutMeta(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Meta);
    }

    private static bool CheckOutBothTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.CheckoutIsValid(Provider.GetAssetListFromSelection(), CheckoutMode.Both);
    }

    private static void CheckOutBoth(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Both);
    }

    private static bool MarkAddTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.AddIsValid(Provider.GetAssetListFromSelection());
    }

    private static void MarkAdd(MenuCommand cmd)
    {
      Provider.Add(Provider.GetAssetListFromSelection(), true).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool RevertTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.RevertIsValid(Provider.GetAssetListFromSelection(), RevertMode.Normal);
    }

    private static void Revert(MenuCommand cmd)
    {
      WindowRevert.Open(Provider.GetAssetListFromSelection());
    }

    private static bool RevertUnchangedTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.RevertIsValid(Provider.GetAssetListFromSelection(), RevertMode.Normal);
    }

    private static void RevertUnchanged(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      Provider.Revert(listFromSelection, RevertMode.Unchanged).SetCompletionAction(CompletionAction.UpdatePendingWindow);
      Provider.Status(listFromSelection);
    }

    private static bool ResolveTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.ResolveIsValid(Provider.GetAssetListFromSelection());
    }

    private static void Resolve(MenuCommand cmd)
    {
      WindowResolve.Open(Provider.GetAssetListFromSelection());
    }

    private static bool LockTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.LockIsValid(Provider.GetAssetListFromSelection());
    }

    private static void Lock(MenuCommand cmd)
    {
      Provider.Lock(Provider.GetAssetListFromSelection(), true).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool UnlockTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.UnlockIsValid(Provider.GetAssetListFromSelection());
    }

    private static void Unlock(MenuCommand cmd)
    {
      Provider.Lock(Provider.GetAssetListFromSelection(), false).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool DiffHeadTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.DiffIsValid(Provider.GetAssetListFromSelection());
    }

    private static void DiffHead(MenuCommand cmd)
    {
      Provider.DiffHead(Provider.GetAssetListFromSelection(), false);
    }

    private static bool DiffHeadWithMetaTest(MenuCommand cmd)
    {
      return Provider.enabled && Provider.DiffIsValid(Provider.GetAssetListFromSelection());
    }

    private static void DiffHeadWithMeta(MenuCommand cmd)
    {
      Provider.DiffHead(Provider.GetAssetListFromSelection(), true);
    }
  }
}
