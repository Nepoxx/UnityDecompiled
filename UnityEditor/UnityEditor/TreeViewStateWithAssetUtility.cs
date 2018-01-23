// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewStateWithAssetUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TreeViewStateWithAssetUtility : TreeViewState
  {
    [SerializeField]
    private CreateAssetUtility m_CreateAssetUtility = new CreateAssetUtility();

    internal CreateAssetUtility createAssetUtility
    {
      get
      {
        return this.m_CreateAssetUtility;
      }
      set
      {
        this.m_CreateAssetUtility = value;
      }
    }

    internal override void OnAwake()
    {
      base.OnAwake();
      this.m_CreateAssetUtility.Clear();
    }
  }
}
