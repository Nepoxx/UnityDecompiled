// Decompiled with JetBrains decompiler
// Type: UnityEditor.TilemapCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Tilemaps;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (TilemapCollider2D))]
  internal class TilemapCollider2DEditor : Collider2DEditorBase
  {
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      base.OnInspectorGUI();
      this.serializedObject.ApplyModifiedProperties();
      this.FinalizeInspectorGUI();
    }
  }
}
