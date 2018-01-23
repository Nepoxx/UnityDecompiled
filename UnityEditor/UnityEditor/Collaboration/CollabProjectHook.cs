// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.CollabProjectHook
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Collaboration
{
  internal class CollabProjectHook
  {
    public static void OnProjectWindowIconOverlay(Rect iconRect, string guid, bool isListMode)
    {
      CollabProjectHook.DrawProjectBrowserIconOverlay(iconRect, guid, isListMode);
    }

    public static void OnProjectBrowserNavPanelIconOverlay(Rect iconRect, string guid)
    {
      CollabProjectHook.DrawProjectBrowserIconOverlay(iconRect, guid, true);
    }

    private static void DrawProjectBrowserIconOverlay(Rect iconRect, string guid, bool isListMode)
    {
      if (!Collab.instance.IsCollabEnabledForCurrentProject())
        return;
      Overlay.DrawOverlays(CollabProjectHook.GetAssetState(guid), iconRect, isListMode);
    }

    public static Collab.CollabStates GetAssetState(string assetGuid)
    {
      if (!Collab.instance.IsCollabEnabledForCurrentProject())
        return Collab.CollabStates.kCollabNone;
      return Collab.instance.GetAssetState(assetGuid);
    }
  }
}
