// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerDetailedObjectsView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProfilerDetailedObjectsView : ProfilerDetailedView
  {
    private ProfilerHierarchyGUI m_ProfilerHierarchyGUI;

    public ProfilerDetailedObjectsView(ProfilerHierarchyGUI profilerHierarchyGUI, ProfilerHierarchyGUI mainProfilerHierarchyGUI)
      : base(mainProfilerHierarchyGUI)
    {
      this.m_ProfilerHierarchyGUI = profilerHierarchyGUI;
    }

    public void DoGUI(GUIStyle headerStyle, int frameIndex, ProfilerViewType viewType)
    {
      ProfilerProperty detailedProperty = this.GetDetailedProperty(frameIndex, viewType, this.m_ProfilerHierarchyGUI.sortType);
      if (detailedProperty != null)
        this.m_ProfilerHierarchyGUI.DoGUI(detailedProperty, string.Empty, false);
      else
        this.DrawEmptyPane(headerStyle);
    }

    private ProfilerProperty GetDetailedProperty(int frameIndex, ProfilerViewType viewType, ProfilerColumn sortType)
    {
      if (this.m_CachedProfilerPropertyConfig.EqualsTo(frameIndex, viewType, sortType))
        return this.m_CachedProfilerProperty;
      ProfilerProperty detailedProperty = this.m_MainProfilerHierarchyGUI.GetDetailedProperty();
      if (this.m_CachedProfilerProperty != null)
        this.m_CachedProfilerProperty.Cleanup();
      this.m_CachedProfilerPropertyConfig.Set(frameIndex, viewType, sortType);
      this.m_CachedProfilerProperty = detailedProperty;
      return detailedProperty;
    }
  }
}
