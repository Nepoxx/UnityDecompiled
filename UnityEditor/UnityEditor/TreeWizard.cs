// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeWizard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TreeWizard : TerrainWizard
  {
    private int m_PrototypeIndex = -1;
    private bool m_IsValidTree = false;
    public GameObject m_Tree;
    public float m_BendFactor;

    public void OnEnable()
    {
      this.minSize = new Vector2(400f, 150f);
    }

    private static bool IsValidTree(GameObject tree, int prototypeIndex, Terrain terrain)
    {
      if ((Object) tree == (Object) null)
        return false;
      TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
      for (int index = 0; index < treePrototypes.Length; ++index)
      {
        if (index != prototypeIndex && (Object) treePrototypes[index].m_Prefab == (Object) tree)
          return false;
      }
      return true;
    }

    internal void InitializeDefaults(Terrain terrain, int index)
    {
      this.m_Terrain = terrain;
      this.m_PrototypeIndex = index;
      if (this.m_PrototypeIndex == -1)
      {
        this.m_Tree = (GameObject) null;
        this.m_BendFactor = 0.0f;
      }
      else
      {
        this.m_Tree = this.m_Terrain.terrainData.treePrototypes[this.m_PrototypeIndex].prefab;
        this.m_BendFactor = this.m_Terrain.terrainData.treePrototypes[this.m_PrototypeIndex].bendFactor;
      }
      this.m_IsValidTree = TreeWizard.IsValidTree(this.m_Tree, this.m_PrototypeIndex, terrain);
      this.OnWizardUpdate();
    }

    private void DoApply()
    {
      if ((Object) this.terrainData == (Object) null)
        return;
      TreePrototype[] treePrototypes = this.m_Terrain.terrainData.treePrototypes;
      if (this.m_PrototypeIndex == -1)
      {
        TreePrototype[] treePrototypeArray = new TreePrototype[treePrototypes.Length + 1];
        for (int index = 0; index < treePrototypes.Length; ++index)
          treePrototypeArray[index] = treePrototypes[index];
        treePrototypeArray[treePrototypes.Length] = new TreePrototype();
        treePrototypeArray[treePrototypes.Length].prefab = this.m_Tree;
        treePrototypeArray[treePrototypes.Length].bendFactor = this.m_BendFactor;
        this.m_PrototypeIndex = treePrototypes.Length;
        this.m_Terrain.terrainData.treePrototypes = treePrototypeArray;
        TreePainter.selectedTree = this.m_PrototypeIndex;
      }
      else
      {
        treePrototypes[this.m_PrototypeIndex].prefab = this.m_Tree;
        treePrototypes[this.m_PrototypeIndex].bendFactor = this.m_BendFactor;
        this.m_Terrain.terrainData.treePrototypes = treePrototypes;
      }
      this.m_Terrain.Flush();
      EditorUtility.SetDirty((Object) this.m_Terrain);
    }

    private void OnWizardCreate()
    {
      this.DoApply();
    }

    private void OnWizardOtherButton()
    {
      this.DoApply();
    }

    protected override bool DrawWizardGUI()
    {
      EditorGUI.BeginChangeCheck();
      this.m_Tree = (GameObject) EditorGUILayout.ObjectField("Tree Prefab", (Object) this.m_Tree, typeof (GameObject), !EditorUtility.IsPersistent((Object) this.m_Terrain.terrainData), new GUILayoutOption[0]);
      if (!TerrainEditorUtility.IsLODTreePrototype(this.m_Tree))
        this.m_BendFactor = EditorGUILayout.FloatField("Bend Factor", this.m_BendFactor, new GUILayoutOption[0]);
      bool flag = EditorGUI.EndChangeCheck();
      if (flag)
        this.m_IsValidTree = TreeWizard.IsValidTree(this.m_Tree, this.m_PrototypeIndex, this.m_Terrain);
      return flag;
    }

    internal override void OnWizardUpdate()
    {
      base.OnWizardUpdate();
      if ((Object) this.m_Tree == (Object) null)
      {
        this.errorString = "Please assign a tree";
        this.isValid = false;
      }
      else if (!this.m_IsValidTree)
      {
        this.errorString = "Tree has already been selected as a prototype";
        this.isValid = false;
      }
      else
      {
        if (this.m_PrototypeIndex == -1)
          return;
        this.DoApply();
      }
    }
  }
}
