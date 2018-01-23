// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextMeshInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (TextMesh))]
  internal class TextMeshInspector : Editor
  {
    private SerializedProperty m_Font;

    private void OnEnable()
    {
      this.m_Font = this.serializedObject.FindProperty("m_Font");
    }

    public override void OnInspectorGUI()
    {
      Font font1 = !this.m_Font.hasMultipleDifferentValues ? this.m_Font.objectReferenceValue as Font : (Font) null;
      this.DrawDefaultInspector();
      Font font2 = !this.m_Font.hasMultipleDifferentValues ? this.m_Font.objectReferenceValue as Font : (Font) null;
      if (!((Object) font2 != (Object) null) || !((Object) font2 != (Object) font1))
        return;
      foreach (Component target in this.targets)
      {
        MeshRenderer component = target.GetComponent<MeshRenderer>();
        if ((bool) ((Object) component))
          component.sharedMaterial = font2.material;
      }
    }
  }
}
