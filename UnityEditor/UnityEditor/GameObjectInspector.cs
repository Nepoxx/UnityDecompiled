// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (GameObject))]
  [CanEditMultipleObjects]
  internal class GameObjectInspector : Editor
  {
    private Dictionary<int, GameObjectInspector.PreviewData> m_PreviewInstances = new Dictionary<int, GameObjectInspector.PreviewData>();
    private bool m_HasInstance = false;
    private bool m_AllOfSamePrefabType = true;
    private SerializedProperty m_Name;
    private SerializedProperty m_IsActive;
    private SerializedProperty m_Layer;
    private SerializedProperty m_Tag;
    private SerializedProperty m_StaticEditorFlags;
    private SerializedProperty m_Icon;
    private static GameObjectInspector.Styles s_Styles;
    private const float kIconSize = 24f;
    private Vector2 previewDir;
    public static GameObject dragObject;

    public void OnEnable()
    {
      this.previewDir = EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D ? new Vector2(120f, -20f) : new Vector2(0.0f, 0.0f);
      this.m_Name = this.serializedObject.FindProperty("m_Name");
      this.m_IsActive = this.serializedObject.FindProperty("m_IsActive");
      this.m_Layer = this.serializedObject.FindProperty("m_Layer");
      this.m_Tag = this.serializedObject.FindProperty("m_TagString");
      this.m_StaticEditorFlags = this.serializedObject.FindProperty("m_StaticEditorFlags");
      this.m_Icon = this.serializedObject.FindProperty("m_Icon");
      this.CalculatePrefabStatus();
    }

    private void CalculatePrefabStatus()
    {
      this.m_HasInstance = false;
      this.m_AllOfSamePrefabType = true;
      PrefabType prefabType1 = PrefabUtility.GetPrefabType((UnityEngine.Object) (this.targets[0] as GameObject));
      foreach (UnityEngine.Object target in this.targets)
      {
        PrefabType prefabType2 = PrefabUtility.GetPrefabType(target);
        if (prefabType2 != prefabType1)
          this.m_AllOfSamePrefabType = false;
        if (prefabType2 != PrefabType.None && prefabType2 != PrefabType.Prefab && prefabType2 != PrefabType.ModelPrefab)
          this.m_HasInstance = true;
      }
    }

    private void OnDisable()
    {
    }

    private static bool ShowMixedStaticEditorFlags(StaticEditorFlags mask)
    {
      uint num1 = 0;
      uint num2 = 0;
      IEnumerator enumerator = Enum.GetValues(typeof (StaticEditorFlags)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          object current = enumerator.Current;
          ++num2;
          if ((mask & (StaticEditorFlags) current) > (StaticEditorFlags) 0)
            ++num1;
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return num1 > 0U && (int) num1 != (int) num2;
    }

    protected override void OnHeaderGUI()
    {
      if (GameObjectInspector.s_Styles == null)
        GameObjectInspector.s_Styles = new GameObjectInspector.Styles();
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      EditorGUILayout.BeginVertical(GameObjectInspector.s_Styles.header, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      this.DrawInspector();
      EditorGUILayout.EndVertical();
    }

    public override void OnInspectorGUI()
    {
    }

    internal bool DrawInspector()
    {
      this.serializedObject.Update();
      GameObject target = this.target as GameObject;
      GUIContent guiContent = (GUIContent) null;
      PrefabType prefabType = PrefabType.None;
      if (this.m_AllOfSamePrefabType)
      {
        prefabType = PrefabUtility.GetPrefabType((UnityEngine.Object) target);
        switch (prefabType)
        {
          case PrefabType.None:
            guiContent = GameObjectInspector.s_Styles.goIcon;
            break;
          case PrefabType.Prefab:
          case PrefabType.PrefabInstance:
          case PrefabType.MissingPrefabInstance:
          case PrefabType.DisconnectedPrefabInstance:
            guiContent = GameObjectInspector.s_Styles.prefabIcon;
            break;
          case PrefabType.ModelPrefab:
          case PrefabType.ModelPrefabInstance:
          case PrefabType.DisconnectedModelPrefabInstance:
            guiContent = GameObjectInspector.s_Styles.modelIcon;
            break;
        }
      }
      else
        guiContent = GameObjectInspector.s_Styles.typelessIcon;
      EditorGUILayout.BeginHorizontal();
      EditorGUI.ObjectIconDropDown(GUILayoutUtility.GetRect(24f, 24f, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }), this.targets, true, guiContent.image as Texture2D, this.m_Icon);
      this.DrawPostIconContent();
      using (new EditorGUI.DisabledScope(prefabType == PrefabType.ModelPrefab))
      {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginHorizontal(GUILayout.Width(GameObjectInspector.s_Styles.tagFieldWidth));
        GUILayout.FlexibleSpace();
        EditorGUI.PropertyField(GUILayoutUtility.GetRect((float) EditorStyles.toggle.padding.left, EditorGUIUtility.singleLineHeight, EditorStyles.toggle, new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        }), this.m_IsActive, GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.DelayedTextField(this.m_Name, GUIContent.none, new GUILayoutOption[0]);
        this.DoStaticToggleField(target);
        this.DoStaticFlagsDropDown(target);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        this.DoTagsField(target);
        this.DoLayerField(target);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.EndHorizontal();
      GUILayout.Space(2f);
      using (new EditorGUI.DisabledScope(prefabType == PrefabType.ModelPrefab))
        this.DoPrefabButtons(prefabType, target);
      this.serializedObject.ApplyModifiedProperties();
      return true;
    }

    private void DoPrefabButtons(PrefabType prefabType, GameObject go)
    {
      if (!this.m_HasInstance)
        return;
      using (new EditorGUI.DisabledScope(EditorApplication.isPlayingOrWillChangePlaymode))
      {
        EditorGUILayout.BeginHorizontal();
        GUIContent content = this.targets.Length <= 1 ? GameObjectInspector.s_Styles.goTypeLabel[(int) prefabType] : GameObjectInspector.s_Styles.goTypeLabelMultiple;
        if (content != null)
        {
          EditorGUILayout.BeginHorizontal(GUILayout.Width(24f + GameObjectInspector.s_Styles.tagFieldWidth));
          GUILayout.FlexibleSpace();
          if (prefabType == PrefabType.DisconnectedModelPrefabInstance || prefabType == PrefabType.MissingPrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
          {
            GUI.contentColor = GUI.skin.GetStyle("CN StatusWarn").normal.textColor;
            GUILayout.Label(content, EditorStyles.whiteLabel, new GUILayoutOption[1]
            {
              GUILayout.ExpandWidth(false)
            });
            GUI.contentColor = Color.white;
          }
          else
            GUILayout.Label(content, new GUILayoutOption[1]
            {
              GUILayout.ExpandWidth(false)
            });
          EditorGUILayout.EndHorizontal();
        }
        if (this.targets.Length > 1)
        {
          GUILayout.Label("Instance Management Disabled", GameObjectInspector.s_Styles.instanceManagementInfo, new GUILayoutOption[0]);
        }
        else
        {
          if (prefabType != PrefabType.MissingPrefabInstance && GUILayout.Button("Select", (GUIStyle) "MiniButtonLeft", new GUILayoutOption[0]))
          {
            Selection.activeObject = PrefabUtility.GetPrefabParent(this.target);
            EditorGUIUtility.PingObject(Selection.activeObject);
          }
          using (new EditorGUI.DisabledScope(AnimationMode.InAnimationMode()))
          {
            if (prefabType != PrefabType.MissingPrefabInstance)
            {
              if (GUILayout.Button("Revert", (GUIStyle) "MiniButtonMid", new GUILayoutOption[0]))
              {
                PrefabUtility.RevertPrefabInstanceWithUndo(go);
                if ((UnityEngine.Object) go != (UnityEngine.Object) null)
                  this.CalculatePrefabStatus();
                GUIUtility.ExitGUI();
              }
              if (prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
              {
                GameObject prefabInstanceRoot = PrefabUtility.FindValidUploadPrefabInstanceRoot(go);
                GUI.enabled = (UnityEngine.Object) prefabInstanceRoot != (UnityEngine.Object) null && !AnimationMode.InAnimationMode();
                if (GUILayout.Button("Apply", (GUIStyle) "MiniButtonRight", new GUILayoutOption[0]))
                {
                  if (Provider.PromptAndCheckoutIfNeeded(new string[1]{ AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent((UnityEngine.Object) prefabInstanceRoot)) }, "The version control requires you to check out the prefab before applying changes."))
                  {
                    PrefabUtility.ReplacePrefabWithUndo(prefabInstanceRoot);
                    this.CalculatePrefabStatus();
                    GUIUtility.ExitGUI();
                  }
                }
              }
            }
          }
          if ((prefabType == PrefabType.DisconnectedModelPrefabInstance || prefabType == PrefabType.ModelPrefabInstance) && GUILayout.Button("Open", (GUIStyle) "MiniButtonRight", new GUILayoutOption[0]))
          {
            AssetDatabase.OpenAsset(PrefabUtility.GetPrefabParent(this.target));
            GUIUtility.ExitGUI();
          }
        }
        EditorGUILayout.EndHorizontal();
      }
    }

    private void DoLayerField(GameObject go)
    {
      EditorGUIUtility.labelWidth = GameObjectInspector.s_Styles.layerFieldWidth;
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GameObjectInspector.s_Styles.layerPopup);
      EditorGUI.BeginProperty(rect, GUIContent.none, this.m_Layer);
      EditorGUI.BeginChangeCheck();
      int layer = EditorGUI.LayerField(rect, GameObjectInspector.s_Styles.layerContent, go.layer, GameObjectInspector.s_Styles.layerPopup);
      if (EditorGUI.EndChangeCheck())
      {
        GameObjectUtility.ShouldIncludeChildren shouldIncludeChildren = GameObjectUtility.DisplayUpdateChildrenDialogIfNeeded(this.targets.OfType<GameObject>(), "Change Layer", string.Format("Do you want to set layer to {0} for all child objects as well?", (object) InternalEditorUtility.GetLayerName(layer)));
        if (shouldIncludeChildren != GameObjectUtility.ShouldIncludeChildren.Cancel)
        {
          this.m_Layer.intValue = layer;
          this.SetLayer(layer, shouldIncludeChildren == GameObjectUtility.ShouldIncludeChildren.IncludeChildren);
        }
        GUIUtility.ExitGUI();
      }
      EditorGUI.EndProperty();
    }

    private void DoTagsField(GameObject go)
    {
      string tag;
      try
      {
        tag = go.tag;
      }
      catch (Exception ex)
      {
        tag = "Undefined";
      }
      EditorGUIUtility.labelWidth = GameObjectInspector.s_Styles.tagFieldWidth;
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.popup);
      EditorGUI.BeginProperty(rect, GUIContent.none, this.m_Tag);
      EditorGUI.BeginChangeCheck();
      string str = EditorGUI.TagField(rect, GameObjectInspector.s_Styles.tagContent, tag);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_Tag.stringValue = str;
        Undo.RecordObjects(this.targets, "Change Tag of " + this.targetTitle);
        foreach (UnityEngine.Object target in this.targets)
          (target as GameObject).tag = str;
      }
      EditorGUI.EndProperty();
    }

    private void DoStaticFlagsDropDown(GameObject go)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_StaticEditorFlags.hasMultipleDifferentValues;
      int changedFlags;
      bool changedToValue;
      EditorGUI.EnumFlagsField(GUILayoutUtility.GetRect(GUIContent.none, GameObjectInspector.s_Styles.staticDropdown, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }), GUIContent.none, (Enum) GameObjectUtility.GetStaticEditorFlags(go), out changedFlags, out changedToValue, GameObjectInspector.s_Styles.staticDropdown);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      SceneModeUtility.SetStaticFlags(this.targets, changedFlags, changedToValue);
      this.serializedObject.SetIsDifferentCacheDirty();
      GUIUtility.ExitGUI();
    }

    private void DoStaticToggleField(GameObject go)
    {
      Rect rect = GUILayoutUtility.GetRect(GameObjectInspector.s_Styles.staticContent, EditorStyles.toggle, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) });
      EditorGUI.BeginProperty(rect, GUIContent.none, this.m_StaticEditorFlags);
      EditorGUI.BeginChangeCheck();
      Rect position = rect;
      EditorGUI.showMixedValue |= GameObjectInspector.ShowMixedStaticEditorFlags((StaticEditorFlags) this.m_StaticEditorFlags.intValue);
      Event current = Event.current;
      EventType type = current.type;
      bool flag = current.type == EventType.MouseDown && current.button != 0;
      if (flag)
        current.type = EventType.Ignore;
      bool flagValue = EditorGUI.ToggleLeft(position, GameObjectInspector.s_Styles.staticContent, go.isStatic);
      if (flag)
        current.type = type;
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        SceneModeUtility.SetStaticFlags(this.targets, -1, flagValue);
        this.serializedObject.SetIsDifferentCacheDirty();
        GUIUtility.ExitGUI();
      }
      EditorGUI.EndProperty();
    }

    private UnityEngine.Object[] GetObjects(bool includeChildren)
    {
      return (UnityEngine.Object[]) SceneModeUtility.GetObjects(this.targets, includeChildren);
    }

    private void SetLayer(int layer, bool includeChildren)
    {
      UnityEngine.Object[] objects = this.GetObjects(includeChildren);
      Undo.RecordObjects(objects, "Change Layer of " + this.targetTitle);
      foreach (GameObject gameObject in objects)
        gameObject.layer = layer;
    }

    public override void ReloadPreviewInstances()
    {
      foreach (KeyValuePair<int, GameObjectInspector.PreviewData> previewInstance in this.m_PreviewInstances)
      {
        int key = previewInstance.Key;
        if (key <= this.targets.Length)
          previewInstance.Value.UpdateGameObject(this.targets[key]);
      }
    }

    private GameObjectInspector.PreviewData GetPreviewData()
    {
      GameObjectInspector.PreviewData previewData;
      if (!this.m_PreviewInstances.TryGetValue(this.referenceTargetIndex, out previewData))
      {
        previewData = new GameObjectInspector.PreviewData(this.target);
        this.m_PreviewInstances.Add(this.referenceTargetIndex, previewData);
      }
      return previewData;
    }

    public void OnDestroy()
    {
      foreach (GameObjectInspector.PreviewData previewData in this.m_PreviewInstances.Values)
        previewData.Dispose();
      this.m_PreviewInstances.Clear();
    }

    public static bool HasRenderableParts(GameObject go)
    {
      foreach (Component componentsInChild in go.GetComponentsInChildren<MeshRenderer>())
      {
        MeshFilter component = componentsInChild.gameObject.GetComponent<MeshFilter>();
        if ((bool) ((UnityEngine.Object) component) && (bool) ((UnityEngine.Object) component.sharedMesh))
          return true;
      }
      foreach (SkinnedMeshRenderer componentsInChild in go.GetComponentsInChildren<SkinnedMeshRenderer>())
      {
        if ((bool) ((UnityEngine.Object) componentsInChild.sharedMesh))
          return true;
      }
      foreach (SpriteRenderer componentsInChild in go.GetComponentsInChildren<SpriteRenderer>())
      {
        if ((bool) ((UnityEngine.Object) componentsInChild.sprite))
          return true;
      }
      return false;
    }

    public static void GetRenderableBoundsRecurse(ref Bounds bounds, GameObject go)
    {
      MeshRenderer component1 = go.GetComponent(typeof (MeshRenderer)) as MeshRenderer;
      MeshFilter component2 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
      if ((bool) ((UnityEngine.Object) component1) && (bool) ((UnityEngine.Object) component2) && (bool) ((UnityEngine.Object) component2.sharedMesh))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component1.bounds;
        else
          bounds.Encapsulate(component1.bounds);
      }
      SkinnedMeshRenderer component3 = go.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if ((bool) ((UnityEngine.Object) component3) && (bool) ((UnityEngine.Object) component3.sharedMesh))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component3.bounds;
        else
          bounds.Encapsulate(component3.bounds);
      }
      SpriteRenderer component4 = go.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
      if ((bool) ((UnityEngine.Object) component4) && (bool) ((UnityEngine.Object) component4.sprite))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component4.bounds;
        else
          bounds.Encapsulate(component4.bounds);
      }
      IEnumerator enumerator = go.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          GameObjectInspector.GetRenderableBoundsRecurse(ref bounds, current.gameObject);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    private static float GetRenderableCenterRecurse(ref Vector3 center, GameObject go, int depth, int minDepth, int maxDepth)
    {
      if (depth > maxDepth)
        return 0.0f;
      float num = 0.0f;
      if (depth > minDepth)
      {
        MeshRenderer component1 = go.GetComponent(typeof (MeshRenderer)) as MeshRenderer;
        MeshFilter component2 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
        SkinnedMeshRenderer component3 = go.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        SpriteRenderer component4 = go.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null && ((UnityEngine.Object) component3 == (UnityEngine.Object) null && (UnityEngine.Object) component4 == (UnityEngine.Object) null))
        {
          num = 1f;
          center += go.transform.position;
        }
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(component1.bounds.center, go.transform.position) < 0.00999999977648258)
          {
            num = 1f;
            center += go.transform.position;
          }
        }
        else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(component3.bounds.center, go.transform.position) < 0.00999999977648258)
          {
            num = 1f;
            center += go.transform.position;
          }
        }
        else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && (double) Vector3.Distance(component4.bounds.center, go.transform.position) < 0.00999999977648258)
        {
          num = 1f;
          center += go.transform.position;
        }
      }
      ++depth;
      IEnumerator enumerator = go.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          num += GameObjectInspector.GetRenderableCenterRecurse(ref center, current.gameObject, depth, minDepth, maxDepth);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      return num;
    }

    public static Vector3 GetRenderableCenterRecurse(GameObject go, int minDepth, int maxDepth)
    {
      Vector3 zero = Vector3.zero;
      float renderableCenterRecurse = GameObjectInspector.GetRenderableCenterRecurse(ref zero, go, 0, minDepth, maxDepth);
      return (double) renderableCenterRecurse <= 0.0 ? go.transform.position : zero / renderableCenterRecurse;
    }

    public override bool HasPreviewGUI()
    {
      if (!EditorUtility.IsPersistent(this.target))
        return false;
      return this.HasStaticPreview();
    }

    private bool HasStaticPreview()
    {
      if (this.targets.Length > 1)
        return true;
      if (this.target == (UnityEngine.Object) null)
        return false;
      GameObject target = this.target as GameObject;
      if ((bool) ((UnityEngine.Object) (target.GetComponent(typeof (Camera)) as Camera)))
        return true;
      return GameObjectInspector.HasRenderableParts(target);
    }

    public override void OnPreviewSettings()
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return;
      GUI.enabled = true;
    }

    private void DoRenderPreview()
    {
      GameObjectInspector.PreviewData previewData = this.GetPreviewData();
      Bounds bounds = new Bounds(previewData.gameObject.transform.position, Vector3.zero);
      GameObjectInspector.GetRenderableBoundsRecurse(ref bounds, previewData.gameObject);
      float num1 = Mathf.Max(bounds.extents.magnitude, 0.0001f);
      float num2 = num1 * 3.8f;
      Quaternion quaternion = Quaternion.Euler(-this.previewDir.y, -this.previewDir.x, 0.0f);
      Vector3 vector3 = bounds.center - quaternion * (Vector3.forward * num2);
      previewData.renderUtility.camera.transform.position = vector3;
      previewData.renderUtility.camera.transform.rotation = quaternion;
      previewData.renderUtility.camera.nearClipPlane = num2 - num1 * 1.1f;
      previewData.renderUtility.camera.farClipPlane = num2 + num1 * 1.1f;
      previewData.renderUtility.lights[0].intensity = 0.7f;
      previewData.renderUtility.lights[0].transform.rotation = quaternion * Quaternion.Euler(40f, 40f, 0.0f);
      previewData.renderUtility.lights[1].intensity = 0.7f;
      previewData.renderUtility.lights[1].transform.rotation = quaternion * Quaternion.Euler(340f, 218f, 177f);
      previewData.renderUtility.ambientColor = new Color(0.1f, 0.1f, 0.1f, 0.0f);
      previewData.renderUtility.Render(true, true);
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!this.HasStaticPreview() || !ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      PreviewRenderUtility renderUtility = this.GetPreviewData().renderUtility;
      renderUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview();
      return renderUtility.EndStaticPreview();
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "Preview requires\nrender texture support");
      }
      else
      {
        this.previewDir = PreviewGUI.Drag2D(this.previewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        PreviewRenderUtility renderUtility = this.GetPreviewData().renderUtility;
        renderUtility.BeginPreview(r, background);
        this.DoRenderPreview();
        renderUtility.EndAndDrawPreview(r);
      }
    }

    public void OnSceneDrag(SceneView sceneView)
    {
      GameObject target = this.target as GameObject;
      switch (PrefabUtility.GetPrefabType((UnityEngine.Object) target))
      {
        case PrefabType.Prefab:
        case PrefabType.ModelPrefab:
          Event current = Event.current;
          switch (current.type)
          {
            case EventType.DragUpdated:
              if ((UnityEngine.Object) GameObjectInspector.dragObject == (UnityEngine.Object) null)
              {
                GameObjectInspector.dragObject = (GameObject) PrefabUtility.InstantiatePrefab((UnityEngine.Object) PrefabUtility.FindPrefabRoot(target));
                GameObjectInspector.dragObject.hideFlags = HideFlags.HideInHierarchy;
                GameObjectInspector.dragObject.name = target.name;
              }
              if (HandleUtility.ignoreRaySnapObjects == null)
                HandleUtility.ignoreRaySnapObjects = GameObjectInspector.dragObject.GetComponentsInChildren<Transform>();
              DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
              object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(current.mousePosition));
              if (obj != null)
              {
                RaycastHit raycastHit = (RaycastHit) obj;
                float num1 = 0.0f;
                if (Tools.pivotMode == PivotMode.Center)
                {
                  float num2 = HandleUtility.CalcRayPlaceOffset(HandleUtility.ignoreRaySnapObjects, raycastHit.normal);
                  if ((double) num2 != double.PositiveInfinity)
                    num1 = Vector3.Dot(GameObjectInspector.dragObject.transform.position, raycastHit.normal) - num2;
                }
                GameObjectInspector.dragObject.transform.position = Matrix4x4.identity.MultiplyPoint(raycastHit.point + raycastHit.normal * num1);
              }
              else
                GameObjectInspector.dragObject.transform.position = HandleUtility.GUIPointToWorldRay(current.mousePosition).GetPoint(10f);
              if (sceneView.in2DMode)
              {
                Vector3 position = GameObjectInspector.dragObject.transform.position;
                position.z = PrefabUtility.FindPrefabRoot(target).transform.position.z;
                GameObjectInspector.dragObject.transform.position = position;
              }
              current.Use();
              return;
            case EventType.DragPerform:
              string uniqueNameForSibling = GameObjectUtility.GetUniqueNameForSibling((Transform) null, GameObjectInspector.dragObject.name);
              GameObjectInspector.dragObject.hideFlags = HideFlags.None;
              Undo.RegisterCreatedObjectUndo((UnityEngine.Object) GameObjectInspector.dragObject, "Place " + GameObjectInspector.dragObject.name);
              EditorUtility.SetDirty((UnityEngine.Object) GameObjectInspector.dragObject);
              DragAndDrop.AcceptDrag();
              Selection.activeObject = (UnityEngine.Object) GameObjectInspector.dragObject;
              HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
              if ((UnityEngine.Object) EditorWindow.mouseOverWindow != (UnityEngine.Object) null)
                EditorWindow.mouseOverWindow.Focus();
              GameObjectInspector.dragObject.name = uniqueNameForSibling;
              GameObjectInspector.dragObject = (GameObject) null;
              current.Use();
              return;
            case EventType.DragExited:
              if (!(bool) ((UnityEngine.Object) GameObjectInspector.dragObject))
                return;
              UnityEngine.Object.DestroyImmediate((UnityEngine.Object) GameObjectInspector.dragObject, false);
              HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
              GameObjectInspector.dragObject = (GameObject) null;
              current.Use();
              return;
            default:
              return;
          }
      }
    }

    private class Styles
    {
      public GUIContent goIcon = EditorGUIUtility.IconContent("GameObject Icon");
      public GUIContent typelessIcon = EditorGUIUtility.IconContent("Prefab Icon");
      public GUIContent prefabIcon = EditorGUIUtility.IconContent("PrefabNormal Icon");
      public GUIContent modelIcon = EditorGUIUtility.IconContent("PrefabModel Icon");
      public GUIContent staticContent = EditorGUIUtility.TextContent("Static|Enable the checkbox to mark this GameObject as static for all systems.\n\nDisable the checkbox to mark this GameObject as not static for all systems.\n\nUse the drop-down menu to mark as this GameObject as static or not static for individual systems.");
      public GUIContent layerContent = EditorGUIUtility.TextContent("Layer|The layer that this GameObject is in.\n\nChoose Add Layer... to edit the list of available layers.");
      public GUIContent tagContent = EditorGUIUtility.TextContent("Tag|The tag that this GameObject has.\n\nChoose Untagged to remove the current tag.\n\nChoose Add Tag... to edit the list of available tags.");
      public float tagFieldWidth = EditorStyles.boldLabel.CalcSize(EditorGUIUtility.TempContent("Tag")).x;
      public float layerFieldWidth = EditorStyles.boldLabel.CalcSize(EditorGUIUtility.TempContent("Layer")).x;
      public GUIStyle staticDropdown = (GUIStyle) "StaticDropdown";
      public GUIStyle header = new GUIStyle((GUIStyle) "IN GameObjectHeader");
      public GUIStyle layerPopup = new GUIStyle(EditorStyles.popup);
      public GUIStyle instanceManagementInfo = new GUIStyle(EditorStyles.helpBox);
      public GUIContent goTypeLabelMultiple = new GUIContent("Multiple");
      public GUIContent[] goTypeLabel = new GUIContent[8]{ null, EditorGUIUtility.TextContent("Prefab"), EditorGUIUtility.TextContent("Model"), EditorGUIUtility.TextContent("Prefab"), EditorGUIUtility.TextContent("Model"), EditorGUIUtility.TextContent("Missing|The source Prefab or Model has been deleted."), EditorGUIUtility.TextContent("Prefab|You have broken the prefab connection. Changes to the prefab will not be applied to this object before you Apply or Revert."), EditorGUIUtility.TextContent("Model|You have broken the prefab connection. Changes to the model will not be applied to this object before you Revert.") };

      public Styles()
      {
        GUIStyle guiStyle = (GUIStyle) "MiniButtonMid";
        this.instanceManagementInfo.padding = guiStyle.padding;
        this.instanceManagementInfo.alignment = guiStyle.alignment;
        this.layerPopup.margin.right = 0;
        this.header.padding.bottom -= 3;
      }
    }

    private class PreviewData : IDisposable
    {
      private bool m_Disposed;
      private GameObject m_GameObject;
      public readonly PreviewRenderUtility renderUtility;

      public PreviewData(UnityEngine.Object targetObject)
      {
        this.renderUtility = new PreviewRenderUtility();
        this.renderUtility.camera.fieldOfView = 30f;
        this.UpdateGameObject(targetObject);
      }

      public GameObject gameObject
      {
        get
        {
          return this.m_GameObject;
        }
      }

      public void UpdateGameObject(UnityEngine.Object targetObject)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.gameObject);
        this.m_GameObject = EditorUtility.InstantiateForAnimatorPreview(targetObject);
        this.renderUtility.AddManagedGO(this.gameObject);
      }

      public void Dispose()
      {
        if (this.m_Disposed)
          return;
        this.renderUtility.Cleanup();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.gameObject);
        this.m_Disposed = true;
      }
    }
  }
}
