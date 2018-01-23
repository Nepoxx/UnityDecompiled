// Decompiled with JetBrains decompiler
// Type: UnityEditor.TilemapEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Tilemap))]
  internal class TilemapEditor : Editor
  {
    private SerializedProperty m_AnimationFrameRate;
    private SerializedProperty m_TilemapColor;
    private SerializedProperty m_TileAnchor;
    private SerializedProperty m_Orientation;
    private SerializedProperty m_OrientationMatrix;

    private Tilemap tilemap
    {
      get
      {
        return this.target as Tilemap;
      }
    }

    private void OnEnable()
    {
      this.m_AnimationFrameRate = this.serializedObject.FindProperty("m_AnimationFrameRate");
      this.m_TilemapColor = this.serializedObject.FindProperty("m_Color");
      this.m_TileAnchor = this.serializedObject.FindProperty("m_TileAnchor");
      this.m_Orientation = this.serializedObject.FindProperty("m_TileOrientation");
      this.m_OrientationMatrix = this.serializedObject.FindProperty("m_TileOrientationMatrix");
    }

    private void OnDisable()
    {
      if (!((UnityEngine.Object) this.tilemap != (UnityEngine.Object) null))
        return;
      this.tilemap.ClearAllEditorPreviewTiles();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.UpdateIfRequiredOrScript();
      EditorGUILayout.PropertyField(this.m_AnimationFrameRate, TilemapEditor.Styles.animationFrameRateLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_TilemapColor, TilemapEditor.Styles.tilemapColorLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_TileAnchor, TilemapEditor.Styles.tileAnchorLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Orientation, TilemapEditor.Styles.orientationLabel, new GUILayoutOption[0]);
      GUI.enabled = !this.m_Orientation.hasMultipleDifferentValues && Tilemap.Orientation.Custom == this.tilemap.orientation;
      if (this.targets.Length > 1)
      {
        EditorGUILayout.PropertyField(this.m_OrientationMatrix, true, new GUILayoutOption[0]);
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        Matrix4x4 matrix4x4 = TileEditor.TransformMatrixOnGUI(this.tilemap.orientationMatrix);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject((UnityEngine.Object) this.tilemap, "Tilemap property change");
          this.tilemap.orientationMatrix = matrix4x4;
        }
      }
      GUI.enabled = true;
      this.serializedObject.ApplyModifiedProperties();
    }

    [MenuItem("GameObject/2D Object/Tilemap")]
    internal static void CreateRectangularTilemap()
    {
      GameObject orCreateRootGrid = TilemapEditor.FindOrCreateRootGrid();
      GameObject gameObject = new GameObject(GameObjectUtility.GetUniqueNameForSibling(orCreateRootGrid.transform, "Tilemap"), new System.Type[2]{ typeof (Tilemap), typeof (TilemapRenderer) });
      gameObject.transform.SetParent(orCreateRootGrid.transform);
      gameObject.transform.position = Vector3.zero;
      Undo.RegisterCreatedObjectUndo((UnityEngine.Object) gameObject, "Create Tilemap");
    }

    private static GameObject FindOrCreateRootGrid()
    {
      GameObject gameObject = (GameObject) null;
      if (Selection.activeObject is GameObject)
      {
        Grid componentInParent = ((GameObject) Selection.activeObject).GetComponentInParent<Grid>();
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
          gameObject = componentInParent.gameObject;
      }
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        gameObject = new GameObject("Grid", new System.Type[1]
        {
          typeof (Grid)
        });
        gameObject.transform.position = Vector3.zero;
        gameObject.GetComponent<Grid>().cellSize = new Vector3(1f, 1f, 0.0f);
        Undo.RegisterCreatedObjectUndo((UnityEngine.Object) gameObject, "Create Grid");
      }
      return gameObject;
    }

    [MenuItem("CONTEXT/Tilemap/Refresh All Tiles")]
    internal static void RefreshAllTiles(MenuCommand item)
    {
      ((Tilemap) item.context).RefreshAllTiles();
      InternalEditorUtility.RepaintAllViews();
    }

    [MenuItem("CONTEXT/Tilemap/Compress Tilemap Bounds")]
    internal static void CompressBounds(MenuCommand item)
    {
      ((Tilemap) item.context).CompressBounds();
    }

    private static class Styles
    {
      public static readonly GUIContent animationFrameRateLabel = EditorGUIUtility.TextContent("Animation Frame Rate|Frame rate for playing animated tiles in the tilemap");
      public static readonly GUIContent tilemapColorLabel = EditorGUIUtility.TextContent("Color|Color tinting all Sprites from tiles in the tilemap");
      public static readonly GUIContent tileAnchorLabel = EditorGUIUtility.TextContent("Tile Anchor|Anchoring position for Sprites from tiles in the tilemap");
      public static readonly GUIContent orientationLabel = EditorGUIUtility.TextContent("Orientation|Orientation for tiles in the tilemap");
    }
  }
}
