// Decompiled with JetBrains decompiler
// Type: UnityEditor.TileEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
  [CustomEditor(typeof (Tile))]
  [CanEditMultipleObjects]
  internal class TileEditor : Editor
  {
    private const float k_PreviewWidth = 32f;
    private const float k_PreviewHeight = 32f;
    private SerializedProperty m_Color;
    private SerializedProperty m_ColliderType;
    private SerializedProperty m_Sprite;

    private Tile tile
    {
      get
      {
        return this.target as Tile;
      }
    }

    public void OnEnable()
    {
      this.m_Color = this.serializedObject.FindProperty("m_Color");
      this.m_ColliderType = this.serializedObject.FindProperty("m_ColliderType");
      this.m_Sprite = this.serializedObject.FindProperty("m_Sprite");
    }

    public override void OnInspectorGUI()
    {
      TileEditor.DoTilePreview(this.tile.sprite, this.tile.color, Matrix4x4.identity);
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Sprite);
      EditorGUILayout.PropertyField(this.m_Color);
      EditorGUILayout.PropertyField(this.m_ColliderType);
      this.serializedObject.ApplyModifiedProperties();
    }

    public static void DoTilePreview(Sprite sprite, Color color, Matrix4x4 transform)
    {
      if ((Object) sprite == (Object) null)
        return;
      Rect rect = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(false, 32f, new GUILayoutOption[0]), new GUIContent(TileEditor.Styles.previewLabel));
      Rect position1 = new Rect(rect.xMin, rect.yMin, 32f, 32f);
      Rect position2 = new Rect(rect.xMin - 1f, rect.yMin - 1f, 34f, 34f);
      if (Event.current.type == EventType.Repaint)
        EditorStyles.textField.Draw(position2, false, false, false, false);
      Texture2D texture2D = SpriteUtility.RenderStaticPreview(sprite, color, 32, 32, transform);
      EditorGUI.DrawTextureTransparent(position1, (Texture) texture2D, ScaleMode.StretchToFill);
    }

    public static Matrix4x4 TransformMatrixOnGUI(Matrix4x4 matrix)
    {
      Matrix4x4 matrix4x4 = matrix;
      if (matrix.ValidTRS())
      {
        EditorGUI.BeginChangeCheck();
        Vector3 vector3_1 = TileEditor.Round((Vector3) matrix.GetColumn(3), 3);
        Vector3 vector3_2 = TileEditor.Round(matrix.rotation.eulerAngles, 3);
        Vector3 vector3_3 = TileEditor.Round(matrix.lossyScale, 3);
        Vector3 pos = EditorGUILayout.Vector3Field(TileEditor.Styles.positionLabel, vector3_1);
        Vector3 euler = EditorGUILayout.Vector3Field(TileEditor.Styles.rotationLabel, vector3_2);
        Vector3 s = EditorGUILayout.Vector3Field(TileEditor.Styles.scaleLabel, vector3_3);
        if (EditorGUI.EndChangeCheck() && (double) s.x != 0.0 && ((double) s.y != 0.0 && (double) s.z != 0.0))
          matrix4x4 = Matrix4x4.TRS(pos, Quaternion.Euler(euler), s);
      }
      else
      {
        GUILayout.BeginVertical();
        GUILayout.Label(TileEditor.Styles.invalidMatrixLabel);
        if (GUILayout.Button(TileEditor.Styles.resetMatrixLabel))
          matrix4x4 = Matrix4x4.identity;
        GUILayout.EndVertical();
      }
      return matrix4x4;
    }

    private static Vector3 Round(Vector3 value, int digits)
    {
      float num = Mathf.Pow(10f, (float) digits);
      return new Vector3(Mathf.Round(value.x * num) / num, Mathf.Round(value.y * num) / num, Mathf.Round(value.z * num) / num);
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
      return SpriteUtility.RenderStaticPreview(this.tile.sprite, this.tile.color, width, height);
    }

    private static class Styles
    {
      public static readonly GUIContent invalidMatrixLabel = EditorGUIUtility.TextContent("Invalid Matrix|No valid Position / Rotation / Scale components available for this matrix");
      public static readonly GUIContent resetMatrixLabel = EditorGUIUtility.TextContent("Reset Matrix");
      public static readonly GUIContent previewLabel = EditorGUIUtility.TextContent("Preview|Preview of tile with attributes set");
      public static readonly GUIContent positionLabel = EditorGUIUtility.TextContent("Position");
      public static readonly GUIContent rotationLabel = EditorGUIUtility.TextContent("Rotation");
      public static readonly GUIContent scaleLabel = EditorGUIUtility.TextContent("Scale");
    }
  }
}
