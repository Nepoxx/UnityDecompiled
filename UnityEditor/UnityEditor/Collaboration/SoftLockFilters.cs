// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collaboration.SoftLockFilters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.Collaboration
{
  internal class SoftLockFilters : AbstractFilters
  {
    public SoftLockFilters()
    {
      this.InitializeFilters();
    }

    public override void InitializeFilters()
    {
      this.filters = new List<string[]>()
      {
        new string[2]{ "All In Progress", "s:inprogress" }
      };
    }

    public void OnSettingStatusChanged(CollabSettingType type, CollabSettingStatus status)
    {
      if (type != CollabSettingType.InProgressEnabled || status != CollabSettingStatus.Available)
        return;
      if (Collab.instance.IsCollabEnabledForCurrentProject() && CollabSettingsManager.inProgressEnabled)
        this.ShowInFavoriteSearchFilters();
      else
        this.HideFromFavoriteSearchFilters();
    }
  }
}
