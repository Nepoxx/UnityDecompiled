// Decompiled with JetBrains decompiler
// Type: UnityEditor.LODGroupEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (LODGroup))]
  internal class LODGroupEditor : Editor
  {
    private static readonly GUIContent[] kSLightIcons = new GUIContent[2];
    private int m_SelectedLODSlider = -1;
    private int m_SelectedLOD = -1;
    private AnimBool m_ShowAnimateCrossFading = new AnimBool();
    private AnimBool m_ShowFadeTransitionWidth = new AnimBool();
    private Vector3 m_LastCameraPos = Vector3.zero;
    private readonly int m_LODSliderId = "LODSliderIDHash".GetHashCode();
    private readonly int m_CameraSliderId = "LODCameraIDHash".GetHashCode();
    private Vector2 m_PreviewDir = new Vector2(0.0f, -20f);
    private int m_NumberOfLODs;
    private LODGroup m_LODGroup;
    private bool m_IsPrefab;
    private SerializedProperty m_FadeMode;
    private SerializedProperty m_AnimateCrossFading;
    private SerializedProperty m_LODs;
    private const string kLODDataPath = "m_LODs.Array.data[{0}]";
    private const string kPixelHeightDataPath = "m_LODs.Array.data[{0}].screenRelativeHeight";
    private const string kRenderRootPath = "m_LODs.Array.data[{0}].renderers";
    private const string kFadeTransitionWidthDataPath = "m_LODs.Array.data[{0}].fadeTransitionWidth";
    private PreviewRenderUtility m_PreviewUtility;

    private void OnEnable()
    {
      this.m_FadeMode = this.serializedObject.FindProperty("m_FadeMode");
      this.m_AnimateCrossFading = this.serializedObject.FindProperty("m_AnimateCrossFading");
      this.m_LODs = this.serializedObject.FindProperty("m_LODs");
      this.m_ShowAnimateCrossFading.value = this.m_FadeMode.intValue != 0;
      this.m_ShowAnimateCrossFading.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowFadeTransitionWidth.value = false;
      this.m_ShowFadeTransitionWidth.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      this.m_LODGroup = (LODGroup) this.target;
      PrefabType prefabType = PrefabUtility.GetPrefabType((UnityEngine.Object) this.m_LODGroup.gameObject);
      this.m_IsPrefab = prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab;
      this.Repaint();
    }

    private void OnDisable()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      this.m_ShowAnimateCrossFading.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowFadeTransitionWidth.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
    }

    private static Rect CalculateScreenRect(IEnumerable<Vector3> points)
    {
      List<Vector2> list = points.Select<Vector3, Vector2>((Func<Vector3, Vector2>) (p => HandleUtility.WorldToGUIPoint(p))).ToList<Vector2>();
      Vector2 vector2_1 = new Vector2(float.MaxValue, float.MaxValue);
      Vector2 vector2_2 = new Vector2(float.MinValue, float.MinValue);
      foreach (Vector2 vector2_3 in list)
      {
        vector2_1.x = (double) vector2_3.x >= (double) vector2_1.x ? vector2_1.x : vector2_3.x;
        vector2_2.x = (double) vector2_3.x <= (double) vector2_2.x ? vector2_2.x : vector2_3.x;
        vector2_1.y = (double) vector2_3.y >= (double) vector2_1.y ? vector2_1.y : vector2_3.y;
        vector2_2.y = (double) vector2_3.y <= (double) vector2_2.y ? vector2_2.y : vector2_3.y;
      }
      return new Rect(vector2_1.x, vector2_1.y, vector2_2.x - vector2_1.x, vector2_2.y - vector2_1.y);
    }

    public void OnSceneGUI()
    {
      if (Event.current.type != EventType.Repaint || (UnityEngine.Object) Camera.current == (UnityEngine.Object) null || (UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) SceneView.currentDrawingSceneView)
        return;
      Camera camera = SceneView.lastActiveSceneView.camera;
      Vector3 worldReferencePoint = LODUtility.CalculateWorldReferencePoint(this.m_LODGroup);
      if ((double) Vector3.Dot(camera.transform.forward, (camera.transform.position - worldReferencePoint).normalized) > 0.0)
        return;
      LODVisualizationInformation visualizationData = LODUtility.CalculateVisualizationData(camera, this.m_LODGroup, -1);
      float worldSpaceSize = visualizationData.worldSpaceSize;
      Handles.color = visualizationData.activeLODLevel == -1 ? LODGroupGUI.kCulledLODColor : LODGroupGUI.kLODColors[visualizationData.activeLODLevel];
      Handles.SelectionFrame(0, worldReferencePoint, camera.transform.rotation, worldSpaceSize / 2f);
      Vector3 vector3_1 = camera.transform.right * worldSpaceSize / 2f;
      Vector3 vector3_2 = camera.transform.up * worldSpaceSize / 2f;
      Rect position = LODGroupEditor.CalculateScreenRect((IEnumerable<Vector3>) new Vector3[4]{ worldReferencePoint - vector3_1 + vector3_2, worldReferencePoint - vector3_1 - vector3_2, worldReferencePoint + vector3_1 + vector3_2, worldReferencePoint + vector3_1 - vector3_2 });
      position = new Rect(position.x + position.width / 2f - 100f, position.yMax, 200f, 45f);
      if ((double) position.yMax > (double) (Screen.height - 45))
        position.y = (float) (Screen.height - 45 - 40);
      Handles.BeginGUI();
      GUI.Label(position, GUIContent.none, EditorStyles.notificationBackground);
      EditorGUI.DoDropShadowLabel(position, GUIContent.Temp(visualizationData.activeLODLevel < 0 ? "Culled" : "LOD " + (object) visualizationData.activeLODLevel), LODGroupGUI.Styles.m_LODLevelNotifyText, 0.3f);
      Handles.EndGUI();
    }

    public void Update()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null || (UnityEngine.Object) SceneView.lastActiveSceneView.camera == (UnityEngine.Object) null || !(SceneView.lastActiveSceneView.camera.transform.position != this.m_LastCameraPos))
        return;
      this.m_LastCameraPos = SceneView.lastActiveSceneView.camera.transform.position;
      this.Repaint();
    }

    private int activeLOD
    {
      get
      {
        return this.m_SelectedLOD;
      }
    }

    private ModelImporter GetImporter()
    {
      return AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(this.target))) as ModelImporter;
    }

    private bool IsLODUsingCrossFadeWidth(int lod)
    {
      if (this.m_FadeMode.intValue == 0 || this.m_AnimateCrossFading.boolValue)
        return false;
      if (this.m_FadeMode.intValue == 1 || this.m_NumberOfLODs > 0 && this.m_SelectedLOD == this.m_NumberOfLODs - 1)
        return true;
      if (this.m_NumberOfLODs > 1 && this.m_SelectedLOD == this.m_NumberOfLODs - 2)
      {
        SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) (this.m_NumberOfLODs - 1)));
        if (property.arraySize == 1 && property.GetArrayElementAtIndex(0).FindPropertyRelative("renderer").objectReferenceValue is BillboardRenderer)
          return true;
      }
      return false;
    }

    public override void OnInspectorGUI()
    {
      bool enabled1 = GUI.enabled;
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_FadeMode);
      this.m_ShowAnimateCrossFading.target = this.m_FadeMode.intValue != 0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAnimateCrossFading.faded))
        EditorGUILayout.PropertyField(this.m_AnimateCrossFading);
      EditorGUILayout.EndFadeGroup();
      this.m_NumberOfLODs = this.m_LODs.arraySize;
      if (this.m_SelectedLOD >= this.m_NumberOfLODs)
        this.m_SelectedLOD = this.m_NumberOfLODs - 1;
      if (this.m_NumberOfLODs > 0 && this.activeLOD >= 0)
      {
        SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) this.activeLOD));
        for (int index = property.arraySize - 1; index >= 0; --index)
        {
          if ((UnityEngine.Object) (property.GetArrayElementAtIndex(index).FindPropertyRelative("renderer").objectReferenceValue as Renderer) == (UnityEngine.Object) null)
            property.DeleteArrayElementAtIndex(index);
        }
      }
      GUILayout.Space(18f);
      Rect rect = GUILayoutUtility.GetRect(0.0f, 30f, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
      List<LODGroupGUI.LODInfo> lodInfos = LODGroupGUI.CreateLODInfos(this.m_NumberOfLODs, rect, (Func<int, string>) (i => string.Format("LOD {0}", (object) i)), (Func<int, float>) (i => this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].screenRelativeHeight", (object) i)).floatValue));
      this.DrawLODLevelSlider(rect, lodInfos);
      GUILayout.Space(16f);
      if ((double) QualitySettings.lodBias != 1.0)
        EditorGUILayout.HelpBox(string.Format("Active LOD bias is {0:0.0#}. Distances are adjusted accordingly.", (object) QualitySettings.lodBias), MessageType.Warning);
      if (this.m_NumberOfLODs > 0 && this.activeLOD >= 0 && this.activeLOD < this.m_NumberOfLODs)
      {
        this.m_ShowFadeTransitionWidth.target = this.IsLODUsingCrossFadeWidth(this.activeLOD);
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowFadeTransitionWidth.faded))
          EditorGUILayout.PropertyField(this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].fadeTransitionWidth", (object) this.activeLOD)));
        EditorGUILayout.EndFadeGroup();
        this.DrawRenderersInfo(EditorGUIUtility.currentViewWidth);
      }
      GUILayout.Space(8f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool flag1 = LODUtility.NeedUpdateLODGroupBoundingBox(this.m_LODGroup);
      using (new EditorGUI.DisabledScope(!flag1))
      {
        if (GUILayout.Button(!flag1 ? LODGroupGUI.Styles.m_RecalculateBoundsDisabled : LODGroupGUI.Styles.m_RecalculateBounds, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        {
          Undo.RecordObject((UnityEngine.Object) this.m_LODGroup, "Recalculate LODGroup Bounds");
          this.m_LODGroup.RecalculateBounds();
        }
      }
      if (GUILayout.Button(LODGroupGUI.Styles.m_LightmapScale, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        this.SendPercentagesToLightmapScale();
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      ModelImporter modelImporter = PrefabUtility.GetPrefabType(this.target) != PrefabType.ModelPrefabInstance ? (ModelImporter) null : this.GetImporter();
      if ((UnityEngine.Object) modelImporter != (UnityEngine.Object) null)
      {
        SerializedObject serializedObject = new SerializedObject((UnityEngine.Object) modelImporter);
        SerializedProperty property = serializedObject.FindProperty("m_LODScreenPercentages");
        bool flag2 = property.isArray && property.arraySize == lodInfos.Count;
        bool enabled2 = GUI.enabled;
        if (!flag2)
          GUI.enabled = false;
        if (GUILayout.Button(!flag2 ? LODGroupGUI.Styles.m_UploadToImporterDisabled : LODGroupGUI.Styles.m_UploadToImporter))
        {
          for (int index = 0; index < property.arraySize; ++index)
            property.GetArrayElementAtIndex(index).floatValue = lodInfos[index].RawScreenPercent;
          serializedObject.ApplyModifiedProperties();
          AssetDatabase.ImportAsset(modelImporter.assetPath);
        }
        GUI.enabled = enabled2;
      }
      this.serializedObject.ApplyModifiedProperties();
      GUI.enabled = enabled1;
    }

    private void DrawRenderersInfo(float availableWidth)
    {
      int num1 = Mathf.FloorToInt(availableWidth / 60f);
      Rect rect1 = GUILayoutUtility.GetRect(LODGroupGUI.Styles.m_RendersTitle, LODGroupGUI.Styles.m_LODSliderTextSelected);
      if (Event.current.type == EventType.Repaint)
        EditorStyles.label.Draw(rect1, LODGroupGUI.Styles.m_RendersTitle, false, false, false, false);
      SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) this.activeLOD));
      int num2 = property.arraySize + 1;
      int num3 = Mathf.CeilToInt((float) num2 / (float) num1);
      Rect rect2 = GUILayoutUtility.GetRect(0.0f, (float) (num3 * 60), new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
      Rect rect3 = rect2;
      GUI.Box(rect2, GUIContent.none);
      rect3.width -= 6f;
      rect3.x += 3f;
      float num4 = rect3.width / (float) num1;
      List<Rect> rectList = new List<Rect>();
      for (int index1 = 0; index1 < num3; ++index1)
      {
        for (int index2 = 0; index2 < num1 && index1 * num1 + index2 < property.arraySize; ++index2)
        {
          Rect position = new Rect((float) (2.0 + (double) rect3.x + (double) index2 * (double) num4), 2f + rect3.y + (float) (index1 * 60), num4 - 4f, 56f);
          rectList.Add(position);
          this.DrawRendererButton(position, index1 * num1 + index2);
        }
      }
      if (this.m_IsPrefab)
        return;
      int num5 = (num2 - 1) % num1;
      int num6 = num3 - 1;
      this.HandleAddRenderer(new Rect((float) (2.0 + (double) rect3.x + (double) num5 * (double) num4), 2f + rect3.y + (float) (num6 * 60), num4 - 4f, 56f), (IEnumerable<Rect>) rectList, rect2);
    }

    private void HandleAddRenderer(Rect position, IEnumerable<Rect> alreadyDrawn, Rect drawArea)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LODGroupEditor.\u003CHandleAddRenderer\u003Ec__AnonStorey0 rendererCAnonStorey0 = new LODGroupEditor.\u003CHandleAddRenderer\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      rendererCAnonStorey0.evt = Event.current;
      // ISSUE: reference to a compiler-generated field
      EventType type = rendererCAnonStorey0.evt.type;
      switch (type)
      {
        case EventType.Repaint:
          LODGroupGUI.Styles.m_LODStandardButton.Draw(position, GUIContent.none, false, false, false, false);
          LODGroupGUI.Styles.m_LODRendererAddButton.Draw(new Rect(position.x - 2f, position.y, position.width, position.height), "Add", false, false, false, false);
          break;
        case EventType.DragUpdated:
        case EventType.DragPerform:
          bool flag = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (drawArea.Contains(rendererCAnonStorey0.evt.mousePosition) && alreadyDrawn.All<Rect>(new Func<Rect, bool>(rendererCAnonStorey0.\u003C\u003Em__0)))
            flag = true;
          if (!flag)
            break;
          if (((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Count<UnityEngine.Object>() > 0)
          {
            DragAndDrop.visualMode = !this.m_IsPrefab ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.None;
            // ISSUE: reference to a compiler-generated field
            if (rendererCAnonStorey0.evt.type == EventType.DragPerform)
            {
              this.AddGameObjectRenderers(this.GetRenderers(((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (go => (UnityEngine.Object) (go as GameObject) != (UnityEngine.Object) null)).Select<UnityEngine.Object, GameObject>((Func<UnityEngine.Object, GameObject>) (go => go as GameObject)), true), true);
              DragAndDrop.AcceptDrag();
              // ISSUE: reference to a compiler-generated field
              rendererCAnonStorey0.evt.Use();
              break;
            }
          }
          // ISSUE: reference to a compiler-generated field
          rendererCAnonStorey0.evt.Use();
          break;
        case EventType.ExecuteCommand:
          // ISSUE: reference to a compiler-generated field
          if (!(rendererCAnonStorey0.evt.commandName == "ObjectSelectorClosed") || ObjectSelector.get.objectSelectorID != "LODGroupSelector".GetHashCode())
            break;
          GameObject currentObject = ObjectSelector.GetCurrentObject() as GameObject;
          if ((UnityEngine.Object) currentObject != (UnityEngine.Object) null)
            this.AddGameObjectRenderers(this.GetRenderers((IEnumerable<GameObject>) new List<GameObject>()
            {
              currentObject
            }, true), true);
          // ISSUE: reference to a compiler-generated field
          rendererCAnonStorey0.evt.Use();
          GUIUtility.ExitGUI();
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          if (type != EventType.MouseDown || !position.Contains(rendererCAnonStorey0.evt.mousePosition))
            break;
          // ISSUE: reference to a compiler-generated field
          rendererCAnonStorey0.evt.Use();
          int hashCode = "LODGroupSelector".GetHashCode();
          ObjectSelector.get.Show((UnityEngine.Object) null, typeof (Renderer), (SerializedProperty) null, true);
          ObjectSelector.get.objectSelectorID = hashCode;
          GUIUtility.ExitGUI();
          break;
      }
    }

    private void DrawRendererButton(Rect position, int rendererIndex)
    {
      SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) this.activeLOD));
      Renderer objectReferenceValue = property.GetArrayElementAtIndex(rendererIndex).FindPropertyRelative("renderer").objectReferenceValue as Renderer;
      Rect position1 = new Rect(position.xMax - 20f, position.yMax - 20f, 20f, 20f);
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.MouseDown:
          if (!this.m_IsPrefab && position1.Contains(current.mousePosition))
          {
            property.DeleteArrayElementAtIndex(rendererIndex);
            current.Use();
            this.serializedObject.ApplyModifiedProperties();
            this.m_LODGroup.RecalculateBounds();
            break;
          }
          if (!position.Contains(current.mousePosition))
            break;
          EditorGUIUtility.PingObject((UnityEngine.Object) objectReferenceValue);
          current.Use();
          break;
        case EventType.Repaint:
          if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
          {
            MeshFilter component = objectReferenceValue.GetComponent<MeshFilter>();
            GUIContent content = !((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.sharedMesh != (UnityEngine.Object) null) ? (!(objectReferenceValue is SkinnedMeshRenderer) ? new GUIContent(ObjectNames.NicifyVariableName(objectReferenceValue.GetType().Name), objectReferenceValue.gameObject.name) : new GUIContent((Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) (objectReferenceValue as SkinnedMeshRenderer).sharedMesh), objectReferenceValue.gameObject.name)) : new GUIContent((Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) component.sharedMesh), objectReferenceValue.gameObject.name);
            LODGroupGUI.Styles.m_LODBlackBox.Draw(position, GUIContent.none, false, false, false, false);
            LODGroupGUI.Styles.m_LODRendererButton.Draw(new Rect(position.x + 2f, position.y + 2f, position.width - 4f, position.height - 4f), content, false, false, false, false);
          }
          else
          {
            LODGroupGUI.Styles.m_LODBlackBox.Draw(position, GUIContent.none, false, false, false, false);
            LODGroupGUI.Styles.m_LODRendererButton.Draw(position, "<Empty>", false, false, false, false);
          }
          if (this.m_IsPrefab)
            break;
          LODGroupGUI.Styles.m_LODBlackBox.Draw(position1, GUIContent.none, false, false, false, false);
          LODGroupGUI.Styles.m_LODRendererRemove.Draw(position1, LODGroupGUI.Styles.m_IconRendererMinus, false, false, false, false);
          break;
      }
    }

    private IEnumerable<Renderer> GetRenderers(IEnumerable<GameObject> selectedGameObjects, bool searchChildren)
    {
      if (EditorUtility.IsPersistent((UnityEngine.Object) this.m_LODGroup))
        return (IEnumerable<Renderer>) new List<Renderer>();
      IEnumerable<GameObject> first = selectedGameObjects.Where<GameObject>((Func<GameObject, bool>) (go => go.transform.IsChildOf(this.m_LODGroup.transform)));
      IEnumerable<GameObject> source = selectedGameObjects.Where<GameObject>((Func<GameObject, bool>) (go => !go.transform.IsChildOf(this.m_LODGroup.transform)));
      List<GameObject> gameObjectList = new List<GameObject>();
      if (source.Count<GameObject>() > 0 && EditorUtility.DisplayDialog("Reparent GameObjects", "Some objects are not children of the LODGroup GameObject. Do you want to reparent them and add them to the LODGroup?", "Yes, Reparent", "No, Use Only Existing Children"))
      {
        foreach (GameObject original in source)
        {
          if (EditorUtility.IsPersistent((UnityEngine.Object) original))
          {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
            if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            {
              gameObject.transform.parent = this.m_LODGroup.transform;
              gameObject.transform.localPosition = Vector3.zero;
              gameObject.transform.localRotation = Quaternion.identity;
              gameObjectList.Add(gameObject);
            }
          }
          else
          {
            original.transform.parent = this.m_LODGroup.transform;
            gameObjectList.Add(original);
          }
        }
        first = first.Union<GameObject>((IEnumerable<GameObject>) gameObjectList);
      }
      List<Renderer> rendererList = new List<Renderer>();
      foreach (GameObject gameObject in first)
      {
        if (searchChildren)
          rendererList.AddRange((IEnumerable<Renderer>) gameObject.GetComponentsInChildren<Renderer>());
        else
          rendererList.Add(gameObject.GetComponent<Renderer>());
      }
      IEnumerable<Renderer> collection = ((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (go => (UnityEngine.Object) (go as Renderer) != (UnityEngine.Object) null)).Select<UnityEngine.Object, Renderer>((Func<UnityEngine.Object, Renderer>) (go => go as Renderer));
      rendererList.AddRange(collection);
      return (IEnumerable<Renderer>) rendererList;
    }

    private void AddGameObjectRenderers(IEnumerable<Renderer> toAdd, bool add)
    {
      SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) this.activeLOD));
      if (!add)
        property.ClearArray();
      List<Renderer> rendererList = new List<Renderer>();
      for (int index = 0; index < property.arraySize; ++index)
      {
        Renderer objectReferenceValue = property.GetArrayElementAtIndex(index).FindPropertyRelative("renderer").objectReferenceValue as Renderer;
        if (!((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null))
          rendererList.Add(objectReferenceValue);
      }
      foreach (Renderer renderer in toAdd)
      {
        if (!rendererList.Contains(renderer))
        {
          ++property.arraySize;
          property.GetArrayElementAtIndex(property.arraySize - 1).FindPropertyRelative("renderer").objectReferenceValue = (UnityEngine.Object) renderer;
          rendererList.Add(renderer);
        }
      }
      this.serializedObject.ApplyModifiedProperties();
      this.m_LODGroup.RecalculateBounds();
    }

    private void DeletedLOD()
    {
      --this.m_SelectedLOD;
    }

    private static void UpdateCamera(float desiredPercentage, LODGroup group)
    {
      Vector3 worldReferencePoint = LODUtility.CalculateWorldReferencePoint(group);
      float distance = LODUtility.CalculateDistance(SceneView.lastActiveSceneView.camera, Mathf.Max(desiredPercentage / QualitySettings.lodBias, 1E-06f), group);
      if (SceneView.lastActiveSceneView.camera.orthographic)
        distance *= Mathf.Sqrt(2f * SceneView.lastActiveSceneView.camera.aspect);
      SceneView.lastActiveSceneView.LookAtDirect(worldReferencePoint, SceneView.lastActiveSceneView.camera.transform.rotation, distance);
    }

    private void UpdateSelectedLODFromCamera(IEnumerable<LODGroupGUI.LODInfo> lods, float cameraPercent)
    {
      foreach (LODGroupGUI.LODInfo lod in lods)
      {
        if ((double) cameraPercent > (double) lod.RawScreenPercent)
        {
          this.m_SelectedLOD = lod.LODLevel;
          break;
        }
      }
    }

    private void DrawLODLevelSlider(Rect sliderPosition, List<LODGroupGUI.LODInfo> lods)
    {
      int controlId1 = GUIUtility.GetControlID(this.m_LODSliderId, FocusType.Passive);
      int controlId2 = GUIUtility.GetControlID(this.m_CameraSliderId, FocusType.Passive);
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(controlId1);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.button == 1 && sliderPosition.Contains(current.mousePosition))
          {
            float cameraPercent = LODGroupGUI.GetCameraPercent(current.mousePosition, sliderPosition);
            GenericMenu genericMenu = new GenericMenu();
            if (lods.Count >= 8)
              genericMenu.AddDisabledItem(EditorGUIUtility.TextContent("Insert Before"));
            else
              genericMenu.AddItem(EditorGUIUtility.TextContent("Insert Before"), false, new GenericMenu.MenuFunction(new LODGroupEditor.LODAction(lods, cameraPercent, current.mousePosition, this.m_LODs, (LODGroupEditor.LODAction.Callback) null).InsertLOD));
            bool flag1 = true;
            if (lods.Count > 0 && (double) lods[lods.Count - 1].RawScreenPercent < (double) cameraPercent)
              flag1 = false;
            if (flag1)
              genericMenu.AddDisabledItem(EditorGUIUtility.TextContent("Delete"));
            else
              genericMenu.AddItem(EditorGUIUtility.TextContent("Delete"), false, new GenericMenu.MenuFunction(new LODGroupEditor.LODAction(lods, cameraPercent, current.mousePosition, this.m_LODs, new LODGroupEditor.LODAction.Callback(this.DeletedLOD)).DeleteLOD));
            genericMenu.ShowAsContext();
            bool flag2 = false;
            foreach (LODGroupGUI.LODInfo lod in lods)
            {
              if (lod.m_RangePosition.Contains(current.mousePosition))
              {
                this.m_SelectedLOD = lod.LODLevel;
                flag2 = true;
                break;
              }
            }
            if (!flag2)
              this.m_SelectedLOD = -1;
            current.Use();
            break;
          }
          Rect rect1 = sliderPosition;
          rect1.x -= 5f;
          rect1.width += 10f;
          if (rect1.Contains(current.mousePosition))
          {
            current.Use();
            GUIUtility.hotControl = controlId1;
            bool flag = false;
            IOrderedEnumerable<LODGroupGUI.LODInfo> orderedEnumerable1 = lods.Where<LODGroupGUI.LODInfo>((Func<LODGroupGUI.LODInfo, bool>) (lod => (double) lod.ScreenPercent > 0.5)).OrderByDescending<LODGroupGUI.LODInfo, int>((Func<LODGroupGUI.LODInfo, int>) (x => x.LODLevel));
            IOrderedEnumerable<LODGroupGUI.LODInfo> orderedEnumerable2 = lods.Where<LODGroupGUI.LODInfo>((Func<LODGroupGUI.LODInfo, bool>) (lod => (double) lod.ScreenPercent <= 0.5)).OrderBy<LODGroupGUI.LODInfo, int>((Func<LODGroupGUI.LODInfo, int>) (x => x.LODLevel));
            List<LODGroupGUI.LODInfo> lodInfoList = new List<LODGroupGUI.LODInfo>();
            lodInfoList.AddRange((IEnumerable<LODGroupGUI.LODInfo>) orderedEnumerable1);
            lodInfoList.AddRange((IEnumerable<LODGroupGUI.LODInfo>) orderedEnumerable2);
            foreach (LODGroupGUI.LODInfo lodInfo in lodInfoList)
            {
              if (lodInfo.m_ButtonPosition.Contains(current.mousePosition))
              {
                this.m_SelectedLODSlider = lodInfo.LODLevel;
                flag = true;
                this.BeginLODDrag(lodInfo.RawScreenPercent + 1f / 1000f, this.m_LODGroup);
                break;
              }
            }
            if (!flag)
            {
              foreach (LODGroupGUI.LODInfo lodInfo in lodInfoList)
              {
                if (lodInfo.m_RangePosition.Contains(current.mousePosition))
                {
                  this.m_SelectedLODSlider = -1;
                  this.m_SelectedLOD = lodInfo.LODLevel;
                  break;
                }
              }
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId1)
          {
            GUIUtility.hotControl = 0;
            this.m_SelectedLODSlider = -1;
            this.EndLODDrag();
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId1 && this.m_SelectedLODSlider >= 0 && lods[this.m_SelectedLODSlider] != null)
          {
            current.Use();
            float cameraPercent = LODGroupGUI.GetCameraPercent(current.mousePosition, sliderPosition);
            LODGroupGUI.SetSelectedLODLevelPercentage(cameraPercent - 1f / 1000f, this.m_SelectedLODSlider, lods);
            this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].screenRelativeHeight", (object) lods[this.m_SelectedLODSlider].LODLevel)).floatValue = lods[this.m_SelectedLODSlider].RawScreenPercent;
            this.UpdateLODDrag(cameraPercent, this.m_LODGroup);
            break;
          }
          break;
        case EventType.Repaint:
          LODGroupGUI.DrawLODSlider(sliderPosition, (IList<LODGroupGUI.LODInfo>) lods, this.activeLOD);
          break;
        case EventType.DragUpdated:
        case EventType.DragPerform:
          int num1 = -2;
          foreach (LODGroupGUI.LODInfo lod in lods)
          {
            if (lod.m_RangePosition.Contains(current.mousePosition))
            {
              num1 = lod.LODLevel;
              break;
            }
          }
          if (num1 == -2 && LODGroupGUI.GetCulledBox(sliderPosition, lods.Count <= 0 ? 1f : lods[lods.Count - 1].ScreenPercent).Contains(current.mousePosition))
            num1 = -1;
          if (num1 >= -1)
          {
            this.m_SelectedLOD = num1;
            if (((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Count<UnityEngine.Object>() > 0)
            {
              DragAndDrop.visualMode = !this.m_IsPrefab ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.None;
              if (current.type == EventType.DragPerform)
              {
                IEnumerable<Renderer> renderers = this.GetRenderers(((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (go => (UnityEngine.Object) (go as GameObject) != (UnityEngine.Object) null)).Select<UnityEngine.Object, GameObject>((Func<UnityEngine.Object, GameObject>) (go => go as GameObject)), true);
                if (num1 == -1)
                {
                  ++this.m_LODs.arraySize;
                  SerializedProperty property1 = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].screenRelativeHeight", (object) lods.Count));
                  if (lods.Count == 0)
                  {
                    property1.floatValue = 0.5f;
                  }
                  else
                  {
                    SerializedProperty property2 = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].screenRelativeHeight", (object) (lods.Count - 1)));
                    property1.floatValue = property2.floatValue / 2f;
                  }
                  this.m_SelectedLOD = lods.Count;
                  this.AddGameObjectRenderers(renderers, false);
                }
                else
                  this.AddGameObjectRenderers(renderers, true);
                DragAndDrop.AcceptDrag();
              }
            }
            current.Use();
            break;
          }
          break;
        default:
          if (typeForControl == EventType.DragExited)
          {
            current.Use();
            break;
          }
          break;
      }
      if (!((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null) || !((UnityEngine.Object) SceneView.lastActiveSceneView.camera != (UnityEngine.Object) null) || this.m_IsPrefab)
        return;
      Camera camera = SceneView.lastActiveSceneView.camera;
      float relativeScreenSize = LODUtility.CalculateVisualizationData(camera, this.m_LODGroup, -1).activeRelativeScreenSize;
      float num2 = LODGroupGUI.DelinearizeScreenPercentage(relativeScreenSize);
      Vector3 normalized = (SceneView.lastActiveSceneView.camera.transform.position - LODUtility.CalculateWorldReferencePoint(this.m_LODGroup)).normalized;
      if ((double) Vector3.Dot(camera.transform.forward, normalized) > 0.0)
        num2 = 1f;
      Rect rect2 = LODGroupGUI.CalcLODButton(sliderPosition, Mathf.Clamp01(num2));
      Rect position1 = new Rect(rect2.center.x - 15f, rect2.y - 25f, 32f, 32f);
      Rect position2 = new Rect(rect2.center.x - 1f, rect2.y, 2f, rect2.height);
      Rect position3 = new Rect(position1.center.x - 5f, position2.yMax, 35f, 20f);
      switch (current.GetTypeForControl(controlId2))
      {
        case EventType.MouseDown:
          if (position1.Contains(current.mousePosition))
          {
            current.Use();
            float cameraPercent = LODGroupGUI.GetCameraPercent(current.mousePosition, sliderPosition);
            this.UpdateSelectedLODFromCamera((IEnumerable<LODGroupGUI.LODInfo>) lods, cameraPercent);
            GUIUtility.hotControl = controlId2;
            this.BeginLODDrag(cameraPercent, this.m_LODGroup);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId2)
          {
            this.EndLODDrag();
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId2)
          {
            current.Use();
            float cameraPercent = LODGroupGUI.GetCameraPercent(current.mousePosition, sliderPosition);
            this.UpdateSelectedLODFromCamera((IEnumerable<LODGroupGUI.LODInfo>) lods, cameraPercent);
            this.UpdateLODDrag(cameraPercent, this.m_LODGroup);
            break;
          }
          break;
        case EventType.Repaint:
          Color backgroundColor = GUI.backgroundColor;
          GUI.backgroundColor = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.8f);
          LODGroupGUI.Styles.m_LODCameraLine.Draw(position2, false, false, false, false);
          GUI.backgroundColor = backgroundColor;
          GUI.Label(position1, LODGroupGUI.Styles.m_CameraIcon, GUIStyle.none);
          LODGroupGUI.Styles.m_LODSliderText.Draw(position3, string.Format("{0:0}%", (object) (float) ((double) Mathf.Clamp01(relativeScreenSize) * 100.0)), false, false, false, false);
          break;
      }
    }

    private void BeginLODDrag(float desiredPercentage, LODGroup group)
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null || (UnityEngine.Object) SceneView.lastActiveSceneView.camera == (UnityEngine.Object) null || this.m_IsPrefab)
        return;
      LODGroupEditor.UpdateCamera(desiredPercentage, group);
      SceneView.lastActiveSceneView.ClearSearchFilter();
      SceneView.lastActiveSceneView.SetSceneViewFiltering(true);
      HierarchyProperty.FilterSingleSceneObject(group.gameObject.GetInstanceID(), false);
      SceneView.RepaintAll();
    }

    private void UpdateLODDrag(float desiredPercentage, LODGroup group)
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null || (UnityEngine.Object) SceneView.lastActiveSceneView.camera == (UnityEngine.Object) null || this.m_IsPrefab)
        return;
      LODGroupEditor.UpdateCamera(desiredPercentage, group);
      SceneView.RepaintAll();
    }

    private void EndLODDrag()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null || (UnityEngine.Object) SceneView.lastActiveSceneView.camera == (UnityEngine.Object) null || this.m_IsPrefab)
        return;
      SceneView.lastActiveSceneView.SetSceneViewFiltering(false);
      SceneView.lastActiveSceneView.ClearSearchFilter();
      HierarchyProperty.ClearSceneObjectsFilter();
    }

    private void SendPercentagesToLightmapScale()
    {
      List<LODGroupEditor.LODLightmapScale> lodLightmapScaleList = new List<LODGroupEditor.LODLightmapScale>();
      for (int index1 = 0; index1 < this.m_NumberOfLODs; ++index1)
      {
        SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) index1));
        List<SerializedProperty> renderers = new List<SerializedProperty>();
        for (int index2 = 0; index2 < property.arraySize; ++index2)
        {
          SerializedProperty propertyRelative = property.GetArrayElementAtIndex(index2).FindPropertyRelative("renderer");
          if (propertyRelative != null)
            renderers.Add(propertyRelative);
        }
        float scale = index1 != 0 ? this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].screenRelativeHeight", (object) (index1 - 1))).floatValue : 1f;
        lodLightmapScaleList.Add(new LODGroupEditor.LODLightmapScale(scale, renderers));
      }
      for (int index = 0; index < this.m_NumberOfLODs; ++index)
        LODGroupEditor.SetLODLightmapScale(lodLightmapScaleList[index]);
    }

    private static void SetLODLightmapScale(LODGroupEditor.LODLightmapScale lodRenderer)
    {
      foreach (SerializedProperty renderer in lodRenderer.m_Renderers)
      {
        if (!(renderer.objectReferenceValue == (UnityEngine.Object) null))
        {
          SerializedObject serializedObject = new SerializedObject(renderer.objectReferenceValue);
          serializedObject.FindProperty("m_ScaleInLightmap").floatValue = Mathf.Max(0.0f, lodRenderer.m_Scale * (1f / LightmapVisualization.GetLightmapLODLevelScale((Renderer) renderer.objectReferenceValue)));
          serializedObject.ApplyModifiedProperties();
        }
      }
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (UnityEngine.Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "LOD preview \nnot available");
      }
      else
      {
        this.InitPreview();
        this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, r);
        this.m_PreviewDir.y = Mathf.Clamp(this.m_PreviewDir.y, -89f, 89f);
        if (Event.current.type != EventType.Repaint)
          return;
        this.m_PreviewUtility.BeginPreview(r, background);
        this.DoRenderPreview();
        this.m_PreviewUtility.EndAndDrawPreview(r);
      }
    }

    private void InitPreview()
    {
      if (this.m_PreviewUtility == null)
        this.m_PreviewUtility = new PreviewRenderUtility();
      if (LODGroupEditor.kSLightIcons[0] != null)
        return;
      LODGroupEditor.kSLightIcons[0] = EditorGUIUtility.IconContent("PreMatLight0");
      LODGroupEditor.kSLightIcons[1] = EditorGUIUtility.IconContent("PreMatLight1");
    }

    protected void DoRenderPreview()
    {
      if (this.m_PreviewUtility.renderTexture.width <= 0 || this.m_PreviewUtility.renderTexture.height <= 0 || (this.m_NumberOfLODs <= 0 || this.activeLOD < 0))
        return;
      Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
      bool flag = false;
      List<MeshFilter> meshFilterList = new List<MeshFilter>();
      SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) this.activeLOD));
      for (int index = 0; index < property.arraySize; ++index)
      {
        Renderer objectReferenceValue = property.GetArrayElementAtIndex(index).FindPropertyRelative("renderer").objectReferenceValue as Renderer;
        if (!((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null))
        {
          MeshFilter component = objectReferenceValue.GetComponent<MeshFilter>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.sharedMesh != (UnityEngine.Object) null && component.sharedMesh.subMeshCount > 0)
            meshFilterList.Add(component);
          if (!flag)
          {
            bounds = objectReferenceValue.bounds;
            flag = true;
          }
          else
            bounds.Encapsulate(objectReferenceValue.bounds);
        }
      }
      if (!flag)
        return;
      float num = bounds.extents.magnitude * 10f;
      Vector2 vector2 = -(this.m_PreviewDir / 100f);
      this.m_PreviewUtility.camera.transform.position = bounds.center + new Vector3(Mathf.Sin(vector2.x) * Mathf.Cos(vector2.y), Mathf.Sin(vector2.y), Mathf.Cos(vector2.x) * Mathf.Cos(vector2.y)) * num;
      this.m_PreviewUtility.camera.transform.LookAt(bounds.center);
      this.m_PreviewUtility.camera.nearClipPlane = 0.05f;
      this.m_PreviewUtility.camera.farClipPlane = 1000f;
      this.m_PreviewUtility.lights[0].intensity = 1f;
      this.m_PreviewUtility.lights[0].transform.rotation = Quaternion.Euler(50f, 50f, 0.0f);
      this.m_PreviewUtility.lights[1].intensity = 1f;
      this.m_PreviewUtility.ambientColor = new Color(0.2f, 0.2f, 0.2f, 0.0f);
      foreach (MeshFilter meshFilter in meshFilterList)
      {
        for (int subMeshIndex = 0; subMeshIndex < meshFilter.sharedMesh.subMeshCount; ++subMeshIndex)
        {
          if (subMeshIndex < meshFilter.GetComponent<Renderer>().sharedMaterials.Length)
          {
            Matrix4x4 matrix = Matrix4x4.TRS(meshFilter.transform.position, meshFilter.transform.rotation, meshFilter.transform.localScale);
            this.m_PreviewUtility.DrawMesh(meshFilter.sharedMesh, matrix, meshFilter.GetComponent<Renderer>().sharedMaterials[subMeshIndex], subMeshIndex);
          }
        }
      }
      this.m_PreviewUtility.Render(false, true);
    }

    public override string GetInfoString()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null || (UnityEngine.Object) SceneView.lastActiveSceneView.camera == (UnityEngine.Object) null || (this.m_NumberOfLODs <= 0 || this.activeLOD < 0))
        return "";
      List<Material> source = new List<Material>();
      SerializedProperty property = this.serializedObject.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) this.activeLOD));
      for (int index = 0; index < property.arraySize; ++index)
      {
        Renderer objectReferenceValue = property.GetArrayElementAtIndex(index).FindPropertyRelative("renderer").objectReferenceValue as Renderer;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
          source.AddRange((IEnumerable<Material>) objectReferenceValue.sharedMaterials);
      }
      LODVisualizationInformation visualizationData = LODUtility.CalculateVisualizationData(SceneView.lastActiveSceneView.camera, this.m_LODGroup, this.activeLOD);
      return this.activeLOD == -1 ? "LOD: culled" : string.Format("{0} Renderer(s)\n{1} Triangle(s)\n{2} Material(s)", (object) property.arraySize, (object) visualizationData.triangleCount, (object) source.Distinct<Material>().Count<Material>());
    }

    private class LODAction
    {
      private readonly float m_Percentage;
      private readonly List<LODGroupGUI.LODInfo> m_LODs;
      private readonly Vector2 m_ClickedPosition;
      private readonly SerializedObject m_ObjectRef;
      private readonly SerializedProperty m_LODsProperty;
      private readonly LODGroupEditor.LODAction.Callback m_Callback;

      public LODAction(List<LODGroupGUI.LODInfo> lods, float percentage, Vector2 clickedPosition, SerializedProperty propLODs, LODGroupEditor.LODAction.Callback callback)
      {
        this.m_LODs = lods;
        this.m_Percentage = percentage;
        this.m_ClickedPosition = clickedPosition;
        this.m_LODsProperty = propLODs;
        this.m_ObjectRef = propLODs.serializedObject;
        this.m_Callback = callback;
      }

      public void InsertLOD()
      {
        if (!this.m_LODsProperty.isArray)
          return;
        int index = -1;
        foreach (LODGroupGUI.LODInfo loD in this.m_LODs)
        {
          if ((double) this.m_Percentage > (double) loD.RawScreenPercent)
          {
            index = loD.LODLevel;
            break;
          }
        }
        if (index < 0)
        {
          this.m_LODsProperty.InsertArrayElementAtIndex(this.m_LODs.Count);
          index = this.m_LODs.Count;
        }
        else
          this.m_LODsProperty.InsertArrayElementAtIndex(index);
        this.m_ObjectRef.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) index)).arraySize = 0;
        this.m_LODsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("screenRelativeHeight").floatValue = this.m_Percentage;
        if (this.m_Callback != null)
          this.m_Callback();
        this.m_ObjectRef.ApplyModifiedProperties();
      }

      public void DeleteLOD()
      {
        if (this.m_LODs.Count <= 0)
          return;
        foreach (LODGroupGUI.LODInfo loD in this.m_LODs)
        {
          int arraySize = this.m_ObjectRef.FindProperty(string.Format("m_LODs.Array.data[{0}].renderers", (object) loD.LODLevel)).arraySize;
          if (loD.m_RangePosition.Contains(this.m_ClickedPosition) && (arraySize == 0 || EditorUtility.DisplayDialog("Delete LOD", "Are you sure you wish to delete this LOD?", "Yes", "No")))
          {
            this.m_ObjectRef.FindProperty(string.Format("m_LODs.Array.data[{0}]", (object) loD.LODLevel)).DeleteCommand();
            this.m_ObjectRef.ApplyModifiedProperties();
            if (this.m_Callback == null)
              break;
            this.m_Callback();
            break;
          }
        }
      }

      public delegate void Callback();
    }

    private class LODLightmapScale
    {
      public readonly float m_Scale;
      public readonly List<SerializedProperty> m_Renderers;

      public LODLightmapScale(float scale, List<SerializedProperty> renderers)
      {
        this.m_Scale = scale;
        this.m_Renderers = renderers;
      }
    }
  }
}
