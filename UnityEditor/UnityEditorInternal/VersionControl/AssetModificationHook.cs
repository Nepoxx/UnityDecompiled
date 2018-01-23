// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.AssetModificationHook
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditorInternal.VersionControl
{
  public class AssetModificationHook
  {
    private static Asset GetStatusCachedIfPossible(string from, AssetModificationHook.CachedStatusMode mode)
    {
      Asset asset = Provider.CacheStatus(from);
      if ((asset == null || asset.IsState(Asset.States.Updating)) && mode == AssetModificationHook.CachedStatusMode.Sync)
      {
        Task task = Provider.Status(from, false);
        task.Wait();
        asset = !task.success ? (Asset) null : Provider.CacheStatus(from);
      }
      return asset;
    }

    private static Asset GetStatusForceUpdate(string from)
    {
      Task task = Provider.Status(from);
      task.Wait();
      return task.assetList.Count <= 0 ? (Asset) null : task.assetList[0];
    }

    public static AssetMoveResult OnWillMoveAsset(string from, string to)
    {
      if (!Provider.enabled)
        return AssetMoveResult.DidNotMove;
      Asset cachedIfPossible = AssetModificationHook.GetStatusCachedIfPossible(from, AssetModificationHook.CachedStatusMode.Sync);
      if (cachedIfPossible == null || !cachedIfPossible.IsUnderVersionControl)
        return AssetMoveResult.DidNotMove;
      if (cachedIfPossible.IsState(Asset.States.OutOfSync))
      {
        Debug.LogError((object) "Cannot move version controlled file that is not up to date. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      if (cachedIfPossible.IsState(Asset.States.DeletedRemote))
      {
        Debug.LogError((object) "Cannot move version controlled file that is deleted on server. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      if (cachedIfPossible.IsState(Asset.States.CheckedOutRemote))
      {
        Debug.LogError((object) "Cannot move version controlled file that is checked out on server. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      if (cachedIfPossible.IsState(Asset.States.LockedRemote))
      {
        Debug.LogError((object) "Cannot move version controlled file that is locked on server. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      Task task = Provider.Move(from, to);
      task.Wait();
      return !task.success ? AssetMoveResult.FailedMove : (AssetMoveResult) task.resultCode;
    }

    public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    {
      if (!Provider.enabled)
        return AssetDeleteResult.DidNotDelete;
      Task task = Provider.Delete(assetPath);
      task.SetCompletionAction(CompletionAction.UpdatePendingWindow);
      task.Wait();
      return !task.success ? AssetDeleteResult.FailedDelete : AssetDeleteResult.DidNotDelete;
    }

    public static bool IsOpenForEdit(string assetPath, out string message, StatusQueryOptions statusOptions)
    {
      message = "";
      if (!Provider.enabled || string.IsNullOrEmpty(assetPath))
        return true;
      Asset asset;
      if (statusOptions == StatusQueryOptions.UseCachedIfPossible || statusOptions == StatusQueryOptions.UseCachedAsync)
      {
        AssetModificationHook.CachedStatusMode mode = statusOptions != StatusQueryOptions.UseCachedAsync ? AssetModificationHook.CachedStatusMode.Sync : AssetModificationHook.CachedStatusMode.Async;
        asset = AssetModificationHook.GetStatusCachedIfPossible(assetPath, mode);
      }
      else
        asset = AssetModificationHook.GetStatusForceUpdate(assetPath);
      if (asset != null)
        return Provider.IsOpenForEdit(asset);
      if (Provider.onlineState == OnlineState.Offline && Provider.offlineReason != string.Empty)
        message = Provider.offlineReason;
      return false;
    }

    private enum CachedStatusMode
    {
      Sync,
      Async,
    }
  }
}
