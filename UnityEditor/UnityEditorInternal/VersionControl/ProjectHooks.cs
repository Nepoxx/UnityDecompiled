// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.ProjectHooks
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditorInternal.VersionControl
{
  internal class ProjectHooks
  {
    public static void OnProjectWindowItem(string guid, Rect drawRect)
    {
      if (!Provider.isActive)
        return;
      Asset assetByGuid = Provider.GetAssetByGUID(guid);
      if (assetByGuid == null)
        return;
      Asset assetByPath = Provider.GetAssetByPath(assetByGuid.path.Trim('/') + ".meta");
      Overlay.DrawOverlay(assetByGuid, assetByPath, drawRect);
    }

    public static Rect GetOverlayRect(Rect drawRect)
    {
      return Overlay.GetOverlayRect(drawRect);
    }
  }
}
