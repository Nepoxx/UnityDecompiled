// Decompiled with JetBrains decompiler
// Type: UnityEditor.PolygonCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (PolygonCollider2D))]
  [CanEditMultipleObjects]
  internal class PolygonCollider2DEditor : Collider2DEditorBase
  {
    private readonly PolygonEditorUtility m_PolyUtility = new PolygonEditorUtility();
    private SerializedProperty m_Points;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Points = this.serializedObject.FindProperty("m_Points");
      this.m_AutoTiling = this.serializedObject.FindProperty("m_AutoTiling");
      this.m_Points.isExpanded = false;
    }

    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();
      if (!this.CanEditCollider())
      {
        EditorGUILayout.HelpBox(Collider2DEditorBase.Styles.s_ColliderEditDisableHelp.text, MessageType.Info);
        if (this.editingCollider)
          UnityEditorInternal.EditMode.QuitEditMode();
      }
      else
        this.BeginColliderInspector();
      base.OnInspectorGUI();
      if (this.targets.Length == 1)
      {
        EditorGUI.BeginDisabledGroup(this.editingCollider);
        EditorGUILayout.PropertyField(this.m_Points, true, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
      }
      this.EndColliderInspector();
      this.FinalizeInspectorGUI();
      EditorGUILayout.EndVertical();
      this.HandleDragAndDrop(GUILayoutUtility.GetLastRect());
    }

    private void HandleDragAndDrop(Rect targetRect)
    {
      if (Event.current.type != EventType.DragPerform && Event.current.type != EventType.DragUpdated || !targetRect.Contains(Event.current.mousePosition))
        return;
      using (IEnumerator<UnityEngine.Object> enumerator = ((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (obj => obj is Sprite || obj is Texture2D)).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          UnityEngine.Object current = enumerator.Current;
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          if (Event.current.type != EventType.DragPerform)
            return;
          Sprite sprite = !(current is Sprite) ? SpriteUtility.TextureToSprite(current as Texture2D) : current as Sprite;
          foreach (PolygonCollider2D polygonCollider2D in ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, PolygonCollider2D>((Func<UnityEngine.Object, PolygonCollider2D>) (target => target as PolygonCollider2D)))
          {
            Vector2[][] paths;
            UnityEditor.Sprites.SpriteUtility.GenerateOutlineFromSprite(sprite, 0.25f, (byte) 200, true, out paths);
            polygonCollider2D.pathCount = paths.Length;
            for (int index = 0; index < paths.Length; ++index)
              polygonCollider2D.SetPath(index, paths[index]);
            this.m_PolyUtility.StopEditing();
            DragAndDrop.AcceptDrag();
          }
          return;
        }
      }
      DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    }

    protected override void OnEditStart()
    {
      if (this.target == (UnityEngine.Object) null)
        return;
      this.m_PolyUtility.StartEditing(this.target as Collider2D);
    }

    protected override void OnEditEnd()
    {
      this.m_PolyUtility.StopEditing();
    }

    public void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      this.m_PolyUtility.OnSceneGUI();
    }
  }
}
