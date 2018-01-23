// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteMaskEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (SpriteMask))]
  [CanEditMultipleObjects]
  internal class SpriteMaskEditor : RendererEditorBase
  {
    private SerializedProperty m_Sprite;
    private SerializedProperty m_AlphaCutoff;
    private SerializedProperty m_IsCustomRangeActive;
    private SerializedProperty m_FrontSortingOrder;
    private SerializedProperty m_FrontSortingLayerID;
    private SerializedProperty m_BackSortingOrder;
    private SerializedProperty m_BackSortingLayerID;
    private AnimBool m_ShowCustomRangeValues;

    [MenuItem("GameObject/2D Object/Sprite Mask")]
    private static void CreateSpriteMaskGameObject()
    {
      GameObject child = new GameObject("", new System.Type[1]{ typeof (SpriteMask) });
      if (Selection.activeObject is Sprite)
        child.GetComponent<SpriteMask>().sprite = (Sprite) Selection.activeObject;
      else if (Selection.activeObject is Texture2D)
      {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(assetPath))
        {
          Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
          if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
            child.GetComponent<SpriteMask>().sprite = sprite;
        }
      }
      else if (Selection.activeObject is GameObject)
      {
        GameObject activeObject = (GameObject) Selection.activeObject;
        switch (PrefabUtility.GetPrefabType((UnityEngine.Object) activeObject))
        {
          case PrefabType.Prefab:
          case PrefabType.ModelPrefab:
            break;
          default:
            GameObjectUtility.SetParentAndAlign(child, activeObject);
            goto case PrefabType.Prefab;
        }
      }
      child.name = GameObjectUtility.GetUniqueNameForSibling(child.transform.parent, SpriteMaskEditor.Contents.newSpriteMaskName.text);
      Undo.RegisterCreatedObjectUndo((UnityEngine.Object) child, SpriteMaskEditor.Contents.createSpriteMaskUndoString.text);
      Selection.activeGameObject = child;
    }

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Sprite = this.serializedObject.FindProperty("m_Sprite");
      this.m_AlphaCutoff = this.serializedObject.FindProperty("m_MaskAlphaCutoff");
      this.m_IsCustomRangeActive = this.serializedObject.FindProperty("m_IsCustomRangeActive");
      this.m_FrontSortingOrder = this.serializedObject.FindProperty("m_FrontSortingOrder");
      this.m_FrontSortingLayerID = this.serializedObject.FindProperty("m_FrontSortingLayerID");
      this.m_BackSortingOrder = this.serializedObject.FindProperty("m_BackSortingOrder");
      this.m_BackSortingLayerID = this.serializedObject.FindProperty("m_BackSortingLayerID");
      this.m_ShowCustomRangeValues = new AnimBool(this.ShouldShowCustomRangeValues());
      this.m_ShowCustomRangeValues.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Sprite, SpriteMaskEditor.Contents.spriteLabel, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_AlphaCutoff, 0.0f, 1f, SpriteMaskEditor.Contents.alphaCutoffLabel, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_IsCustomRangeActive, SpriteMaskEditor.Contents.isCustomRangeActive, new GUILayoutOption[0]);
      this.m_ShowCustomRangeValues.target = this.ShouldShowCustomRangeValues();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCustomRangeValues.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(SpriteMaskEditor.Contents.frontLabel);
        SortingLayerEditorUtility.RenderSortingLayerFields(this.m_FrontSortingOrder, this.m_FrontSortingLayerID);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(SpriteMaskEditor.Contents.backLabel);
        SortingLayerEditorUtility.RenderSortingLayerFields(this.m_BackSortingOrder, this.m_BackSortingLayerID);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
    }

    private bool ShouldShowCustomRangeValues()
    {
      return this.m_IsCustomRangeActive.boolValue && !this.m_IsCustomRangeActive.hasMultipleDifferentValues;
    }

    private static class Contents
    {
      public static readonly GUIContent spriteLabel = EditorGUIUtility.TextContent("Sprite|The Sprite defining the mask");
      public static readonly GUIContent alphaCutoffLabel = EditorGUIUtility.TextContent("Alpha Cutoff|The minimum alpha value used by the mask to select the area of influence defined over the mask's sprite.");
      public static readonly GUIContent isCustomRangeActive = EditorGUIUtility.TextContent("Custom Range|Mask sprites from front to back sorting values only.");
      public static readonly GUIContent createSpriteMaskUndoString = EditorGUIUtility.TextContent("Create Sprite Mask");
      public static readonly GUIContent newSpriteMaskName = EditorGUIUtility.TextContent("New Sprite Mask");
      public static readonly GUIContent frontLabel = EditorGUIUtility.TextContent("Front");
      public static readonly GUIContent backLabel = EditorGUIUtility.TextContent("Back");
    }
  }
}
