// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevEnvironmentLibrary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class LookDevEnvironmentLibrary : ScriptableObject, ISerializationCallbackReceiver
  {
    [SerializeField]
    private List<CubemapInfo> m_HDRIList = new List<CubemapInfo>();
    [SerializeField]
    private List<CubemapInfo> m_SerialShadowMapHDRIList = new List<CubemapInfo>();
    private LookDevView m_LookDevView = (LookDevView) null;
    private bool m_Dirty = false;

    public bool dirty
    {
      get
      {
        return this.m_Dirty;
      }
      set
      {
        this.m_Dirty = value;
      }
    }

    public List<CubemapInfo> hdriList
    {
      get
      {
        return this.m_HDRIList;
      }
    }

    public int hdriCount
    {
      get
      {
        return this.hdriList.Count;
      }
    }

    public void InsertHDRI(Cubemap cubemap)
    {
      this.InsertHDRI(cubemap, -1);
    }

    public void InsertHDRI(Cubemap cubemap, int insertionIndex)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LookDevEnvironmentLibrary.\u003CInsertHDRI\u003Ec__AnonStorey0 hdriCAnonStorey0 = new LookDevEnvironmentLibrary.\u003CInsertHDRI\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      hdriCAnonStorey0.cubemap = cubemap;
      Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Insert HDRI");
      Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.config, "Insert HDRI");
      // ISSUE: reference to a compiler-generated field
      hdriCAnonStorey0.cubemap0 = (Cubemap) null;
      // ISSUE: reference to a compiler-generated field
      hdriCAnonStorey0.cubemap1 = (Cubemap) null;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) hdriCAnonStorey0.cubemap == (UnityEngine.Object) LookDevResources.m_DefaultHDRI)
      {
        // ISSUE: reference to a compiler-generated field
        hdriCAnonStorey0.cubemap0 = LookDevResources.m_DefaultHDRI;
        // ISSUE: reference to a compiler-generated field
        hdriCAnonStorey0.cubemap1 = LookDevResources.m_DefaultHDRI;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        hdriCAnonStorey0.cubemap0 = this.m_HDRIList[this.m_LookDevView.config.lookDevContexts[0].currentHDRIIndex].cubemap;
        // ISSUE: reference to a compiler-generated field
        hdriCAnonStorey0.cubemap1 = this.m_HDRIList[this.m_LookDevView.config.lookDevContexts[1].currentHDRIIndex].cubemap;
      }
      // ISSUE: reference to a compiler-generated method
      int index1 = this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(hdriCAnonStorey0.\u003C\u003Em__0));
      if (index1 == -1)
      {
        this.m_Dirty = true;
        CubemapInfo cubemapInfo = (CubemapInfo) null;
        for (int index2 = 0; index2 < this.m_HDRIList.Count; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_HDRIList[index2].cubemapShadowInfo.cubemap == (UnityEngine.Object) hdriCAnonStorey0.cubemap)
          {
            cubemapInfo = this.m_HDRIList[index2].cubemapShadowInfo;
            cubemapInfo.SetCubemapShadowInfo(cubemapInfo);
            break;
          }
        }
        if (cubemapInfo == null)
        {
          cubemapInfo = new CubemapInfo();
          // ISSUE: reference to a compiler-generated field
          cubemapInfo.cubemap = hdriCAnonStorey0.cubemap;
          cubemapInfo.ambientProbe.Clear();
          cubemapInfo.alreadyComputed = false;
          cubemapInfo.SetCubemapShadowInfo(cubemapInfo);
        }
        int count = this.m_HDRIList.Count;
        this.m_HDRIList.Insert(insertionIndex != -1 ? insertionIndex : count, cubemapInfo);
        if ((UnityEngine.Object) cubemapInfo.cubemap != (UnityEngine.Object) LookDevResources.m_DefaultHDRI)
          LookDevResources.UpdateShadowInfoWithBrightestSpot(cubemapInfo);
      }
      if (index1 != insertionIndex && index1 != -1 && insertionIndex != -1)
      {
        CubemapInfo hdri = this.m_HDRIList[index1];
        this.m_HDRIList.RemoveAt(index1);
        this.m_HDRIList.Insert(index1 <= insertionIndex ? insertionIndex - 1 : insertionIndex, hdri);
      }
      // ISSUE: reference to a compiler-generated method
      this.m_LookDevView.config.lookDevContexts[0].UpdateProperty(LookDevProperty.HDRI, this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(hdriCAnonStorey0.\u003C\u003Em__1)));
      // ISSUE: reference to a compiler-generated method
      this.m_LookDevView.config.lookDevContexts[1].UpdateProperty(LookDevProperty.HDRI, this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(hdriCAnonStorey0.\u003C\u003Em__2)));
      this.m_LookDevView.Repaint();
    }

    public bool RemoveHDRI(Cubemap cubemap)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LookDevEnvironmentLibrary.\u003CRemoveHDRI\u003Ec__AnonStorey1 hdriCAnonStorey1 = new LookDevEnvironmentLibrary.\u003CRemoveHDRI\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      hdriCAnonStorey1.cubemap = cubemap;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) hdriCAnonStorey1.cubemap != (UnityEngine.Object) null)
      {
        Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Remove HDRI");
        Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.config, "Remove HDRI");
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) hdriCAnonStorey1.cubemap == (UnityEngine.Object) LookDevResources.m_DefaultHDRI)
      {
        Debug.LogWarning((object) "Cannot remove default HDRI from the library");
        return false;
      }
      // ISSUE: reference to a compiler-generated method
      int index = this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(hdriCAnonStorey1.\u003C\u003Em__0));
      if (index == -1)
        return false;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LookDevEnvironmentLibrary.\u003CRemoveHDRI\u003Ec__AnonStorey2 hdriCAnonStorey2 = new LookDevEnvironmentLibrary.\u003CRemoveHDRI\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      hdriCAnonStorey2.cubemap0 = this.m_HDRIList[this.m_LookDevView.config.lookDevContexts[0].currentHDRIIndex].cubemap;
      // ISSUE: reference to a compiler-generated field
      hdriCAnonStorey2.cubemap1 = this.m_HDRIList[this.m_LookDevView.config.lookDevContexts[1].currentHDRIIndex].cubemap;
      this.m_HDRIList.RemoveAt(index);
      int num = this.m_HDRIList.Count != 0 ? 0 : -1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LookDevView.config.lookDevContexts[0].UpdateProperty(LookDevProperty.HDRI, !((UnityEngine.Object) hdriCAnonStorey2.cubemap0 == (UnityEngine.Object) hdriCAnonStorey1.cubemap) ? this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(hdriCAnonStorey2.\u003C\u003Em__0)) : num);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LookDevView.config.lookDevContexts[1].UpdateProperty(LookDevProperty.HDRI, !((UnityEngine.Object) hdriCAnonStorey2.cubemap1 == (UnityEngine.Object) hdriCAnonStorey1.cubemap) ? this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(hdriCAnonStorey2.\u003C\u003Em__1)) : num);
      this.m_LookDevView.Repaint();
      this.m_Dirty = true;
      return true;
    }

    public void CleanupDeletedHDRI()
    {
      do
        ;
      while (this.RemoveHDRI((Cubemap) null));
    }

    private ShadowInfo GetCurrentShadowInfo()
    {
      return this.m_HDRIList[this.m_LookDevView.config.lookDevContexts[(int) this.m_LookDevView.config.currentEditionContext].currentHDRIIndex].shadowInfo;
    }

    public void SetLookDevView(LookDevView lookDevView)
    {
      this.m_LookDevView = lookDevView;
    }

    public void OnBeforeSerialize()
    {
      this.m_SerialShadowMapHDRIList.Clear();
      for (int index = 0; index < this.m_HDRIList.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LookDevEnvironmentLibrary.\u003COnBeforeSerialize\u003Ec__AnonStorey3 serializeCAnonStorey3 = new LookDevEnvironmentLibrary.\u003COnBeforeSerialize\u003Ec__AnonStorey3();
        // ISSUE: reference to a compiler-generated field
        serializeCAnonStorey3.shadowCubemapInfo = this.m_HDRIList[index].cubemapShadowInfo;
        // ISSUE: reference to a compiler-generated method
        this.m_HDRIList[index].serialIndexMain = this.m_HDRIList.FindIndex(new Predicate<CubemapInfo>(serializeCAnonStorey3.\u003C\u003Em__0));
        if (this.m_HDRIList[index].serialIndexMain == -1)
        {
          // ISSUE: reference to a compiler-generated method
          this.m_HDRIList[index].serialIndexShadow = this.m_SerialShadowMapHDRIList.FindIndex(new Predicate<CubemapInfo>(serializeCAnonStorey3.\u003C\u003Em__1));
          if (this.m_HDRIList[index].serialIndexShadow == -1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SerialShadowMapHDRIList.Add(serializeCAnonStorey3.shadowCubemapInfo);
            this.m_HDRIList[index].serialIndexShadow = this.m_SerialShadowMapHDRIList.Count - 1;
          }
        }
      }
    }

    public void OnAfterDeserialize()
    {
      for (int index = 0; index < this.m_HDRIList.Count; ++index)
        this.m_HDRIList[index].cubemapShadowInfo = this.m_HDRIList[index].serialIndexMain == -1 ? this.m_SerialShadowMapHDRIList[this.m_HDRIList[index].serialIndexShadow] : this.m_HDRIList[this.hdriList[index].serialIndexMain];
    }
  }
}
