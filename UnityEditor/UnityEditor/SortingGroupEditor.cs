// Decompiled with JetBrains decompiler
// Type: UnityEditor.SortingGroupEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Rendering;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SortingGroup))]
  internal class SortingGroupEditor : Editor
  {
    private SerializedProperty m_SortingOrder;
    private SerializedProperty m_SortingLayerID;

    public virtual void OnEnable()
    {
      this.m_SortingOrder = this.serializedObject.FindProperty("m_SortingOrder");
      this.m_SortingLayerID = this.serializedObject.FindProperty("m_SortingLayerID");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      SortingLayerEditorUtility.RenderSortingLayerFields(this.m_SortingOrder, this.m_SortingLayerID);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
