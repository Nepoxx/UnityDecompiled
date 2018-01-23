// Decompiled with JetBrains decompiler
// Type: UnityEditor.CollisionModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class CollisionModuleUI : ModuleUI
  {
    private static CollisionModuleUI.PlaneVizType m_PlaneVisualizationType = CollisionModuleUI.PlaneVizType.Solid;
    private static float m_ScaleGrid = 1f;
    private static bool s_VisualizeBounds = false;
    private SerializedProperty[] m_Planes = new SerializedProperty[6];
    private List<Transform> m_ScenePlanes = new List<Transform>();
    private const int k_MaxNumPlanes = 6;
    private SerializedProperty m_Type;
    private SerializedMinMaxCurve m_Dampen;
    private SerializedMinMaxCurve m_Bounce;
    private SerializedMinMaxCurve m_LifetimeLossOnCollision;
    private SerializedProperty m_MinKillSpeed;
    private SerializedProperty m_MaxKillSpeed;
    private SerializedProperty m_RadiusScale;
    private SerializedProperty m_CollidesWith;
    private SerializedProperty m_CollidesWithDynamic;
    private SerializedProperty m_MaxCollisionShapes;
    private SerializedProperty m_Quality;
    private SerializedProperty m_VoxelSize;
    private SerializedProperty m_CollisionMessages;
    private SerializedProperty m_CollisionMode;
    private SerializedProperty m_ColliderForce;
    private SerializedProperty m_MultiplyColliderForceByCollisionAngle;
    private SerializedProperty m_MultiplyColliderForceByParticleSpeed;
    private SerializedProperty m_MultiplyColliderForceByParticleSize;
    private SerializedProperty[] m_ShownPlanes;
    private static Transform s_SelectedTransform;
    private static CollisionModuleUI.Texts s_Texts;

    public CollisionModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "CollisionModule", displayName)
    {
      this.m_ToolTip = "Allows you to specify multiple collision planes that the particle can collide with.";
    }

    private bool editingPlanes
    {
      get
      {
        return (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesMove || UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesRotate) && UnityEditorInternal.EditMode.IsOwner(this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.customEditor);
      }
      set
      {
        if (!value && this.editingPlanes)
          UnityEditorInternal.EditMode.QuitEditMode();
        SceneView.RepaintAll();
      }
    }

    protected override void Init()
    {
      if (this.m_Type != null)
        return;
      if (CollisionModuleUI.s_Texts == null)
        CollisionModuleUI.s_Texts = new CollisionModuleUI.Texts();
      this.m_Type = this.GetProperty("type");
      List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
      for (int index = 0; index < this.m_Planes.Length; ++index)
      {
        this.m_Planes[index] = this.GetProperty("plane" + (object) index);
        if (index == 0 || this.m_Planes[index].objectReferenceValue != (UnityEngine.Object) null)
          serializedPropertyList.Add(this.m_Planes[index]);
      }
      this.m_ShownPlanes = serializedPropertyList.ToArray();
      this.m_Dampen = new SerializedMinMaxCurve((ModuleUI) this, CollisionModuleUI.s_Texts.dampen, "m_Dampen");
      this.m_Dampen.m_AllowCurves = false;
      this.m_Bounce = new SerializedMinMaxCurve((ModuleUI) this, CollisionModuleUI.s_Texts.bounce, "m_Bounce");
      this.m_Bounce.m_AllowCurves = false;
      this.m_LifetimeLossOnCollision = new SerializedMinMaxCurve((ModuleUI) this, CollisionModuleUI.s_Texts.lifetimeLoss, "m_EnergyLossOnCollision");
      this.m_LifetimeLossOnCollision.m_AllowCurves = false;
      this.m_MinKillSpeed = this.GetProperty("minKillSpeed");
      this.m_MaxKillSpeed = this.GetProperty("maxKillSpeed");
      this.m_RadiusScale = this.GetProperty("radiusScale");
      CollisionModuleUI.m_PlaneVisualizationType = (CollisionModuleUI.PlaneVizType) EditorPrefs.GetInt("PlaneColisionVizType", 1);
      CollisionModuleUI.m_ScaleGrid = EditorPrefs.GetFloat("ScalePlaneColision", 1f);
      CollisionModuleUI.s_VisualizeBounds = EditorPrefs.GetBool("VisualizeBounds", false);
      this.m_CollidesWith = this.GetProperty("collidesWith");
      this.m_CollidesWithDynamic = this.GetProperty("collidesWithDynamic");
      this.m_MaxCollisionShapes = this.GetProperty("maxCollisionShapes");
      this.m_Quality = this.GetProperty("quality");
      this.m_VoxelSize = this.GetProperty("voxelSize");
      this.m_CollisionMessages = this.GetProperty("collisionMessages");
      this.m_CollisionMode = this.GetProperty("collisionMode");
      this.m_ColliderForce = this.GetProperty("colliderForce");
      this.m_MultiplyColliderForceByCollisionAngle = this.GetProperty("multiplyColliderForceByCollisionAngle");
      this.m_MultiplyColliderForceByParticleSpeed = this.GetProperty("multiplyColliderForceByParticleSpeed");
      this.m_MultiplyColliderForceByParticleSize = this.GetProperty("multiplyColliderForceByParticleSize");
      this.SyncVisualization();
    }

    public override void UndoRedoPerformed()
    {
      base.UndoRedoPerformed();
      this.SyncVisualization();
    }

    protected override void SetVisibilityState(ModuleUI.VisibilityState newState)
    {
      base.SetVisibilityState(newState);
      if (newState != ModuleUI.VisibilityState.VisibleAndFoldedOut)
      {
        CollisionModuleUI.s_SelectedTransform = (Transform) null;
        this.editingPlanes = false;
      }
      else
        this.SyncVisualization();
    }

    private Bounds GetBounds()
    {
      Bounds bounds = new Bounds();
      bool flag = false;
      foreach (Component particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
      {
        ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
        if (!flag)
          bounds = component.bounds;
        bounds.Encapsulate(component.bounds);
        flag = true;
      }
      return bounds;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      CollisionModuleUI.CollisionTypes collisionTypes = (CollisionModuleUI.CollisionTypes) ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.collisionType, this.m_Type, CollisionModuleUI.s_Texts.collisionTypes);
      if (EditorGUI.EndChangeCheck())
        this.SyncVisualization();
      if (collisionTypes == CollisionModuleUI.CollisionTypes.Plane)
      {
        this.DoListOfPlanesGUI();
        EditorGUI.BeginChangeCheck();
        CollisionModuleUI.m_PlaneVisualizationType = (CollisionModuleUI.PlaneVizType) ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.visualization, (int) CollisionModuleUI.m_PlaneVisualizationType, CollisionModuleUI.s_Texts.planeVizTypes);
        if (EditorGUI.EndChangeCheck())
          EditorPrefs.SetInt("PlaneColisionVizType", (int) CollisionModuleUI.m_PlaneVisualizationType);
        EditorGUI.BeginChangeCheck();
        CollisionModuleUI.m_ScaleGrid = ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.scalePlane, CollisionModuleUI.m_ScaleGrid, "f2", new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          CollisionModuleUI.m_ScaleGrid = Mathf.Max(0.0f, CollisionModuleUI.m_ScaleGrid);
          EditorPrefs.SetFloat("ScalePlaneColision", CollisionModuleUI.m_ScaleGrid);
        }
        ModuleUI.GUIButtonGroup(CollisionModuleUI.s_Texts.sceneViewEditModes, CollisionModuleUI.s_Texts.toolContents, new Func<Bounds>(this.GetBounds), this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.customEditor);
      }
      else
        ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.collisionMode, this.m_CollisionMode, CollisionModuleUI.s_Texts.collisionModes);
      ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.dampen, this.m_Dampen);
      ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.bounce, this.m_Bounce);
      ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.lifetimeLoss, this.m_LifetimeLossOnCollision);
      double num1 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.minKillSpeed, this.m_MinKillSpeed);
      double num2 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.maxKillSpeed, this.m_MaxKillSpeed);
      double num3 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.radiusScale, this.m_RadiusScale);
      if (collisionTypes == CollisionModuleUI.CollisionTypes.World)
      {
        ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.quality, this.m_Quality, CollisionModuleUI.s_Texts.qualitySettings);
        ++EditorGUI.indentLevel;
        ModuleUI.GUILayerMask(CollisionModuleUI.s_Texts.collidesWith, this.m_CollidesWith);
        ModuleUI.GUIInt(CollisionModuleUI.s_Texts.maxCollisionShapes, this.m_MaxCollisionShapes);
        if (this.m_Quality.intValue == 0)
        {
          ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.collidesWithDynamic, this.m_CollidesWithDynamic);
        }
        else
        {
          double num4 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.voxelSize, this.m_VoxelSize);
        }
        --EditorGUI.indentLevel;
        double num5 = (double) ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.colliderForce, this.m_ColliderForce);
        ++EditorGUI.indentLevel;
        ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.multiplyColliderForceByCollisionAngle, this.m_MultiplyColliderForceByCollisionAngle);
        ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.multiplyColliderForceByParticleSpeed, this.m_MultiplyColliderForceByParticleSpeed);
        ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.multiplyColliderForceByParticleSize, this.m_MultiplyColliderForceByParticleSize);
        --EditorGUI.indentLevel;
      }
      ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.collisionMessages, this.m_CollisionMessages);
      EditorGUI.BeginChangeCheck();
      CollisionModuleUI.s_VisualizeBounds = ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.visualizeBounds, CollisionModuleUI.s_VisualizeBounds);
      if (!EditorGUI.EndChangeCheck())
        return;
      EditorPrefs.SetBool("VisualizeBounds", CollisionModuleUI.s_VisualizeBounds);
    }

    protected override void OnModuleEnable()
    {
      base.OnModuleEnable();
      this.SyncVisualization();
    }

    protected override void OnModuleDisable()
    {
      base.OnModuleDisable();
      this.editingPlanes = false;
    }

    private void SyncVisualization()
    {
      this.m_ScenePlanes.Clear();
      if (this.m_ParticleSystemUI.multiEdit)
      {
        foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
        {
          if (particleSystem.collision.type == ParticleSystemCollisionType.Planes)
          {
            for (int index = 0; index < 6; ++index)
            {
              Transform plane = particleSystem.collision.GetPlane(index);
              if ((UnityEngine.Object) plane != (UnityEngine.Object) null && !this.m_ScenePlanes.Contains(plane))
                this.m_ScenePlanes.Add(plane);
            }
          }
        }
      }
      else if (this.m_Type.intValue != 0)
      {
        this.editingPlanes = false;
      }
      else
      {
        for (int index = 0; index < this.m_ShownPlanes.Length; ++index)
        {
          Transform objectReferenceValue = this.m_ShownPlanes[index].objectReferenceValue as Transform;
          if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null && !this.m_ScenePlanes.Contains(objectReferenceValue))
            this.m_ScenePlanes.Add(objectReferenceValue);
        }
      }
    }

    private static GameObject CreateEmptyGameObject(string name, ParticleSystem parentOfGameObject)
    {
      GameObject gameObject = new GameObject(name);
      if (!(bool) ((UnityEngine.Object) gameObject))
        return (GameObject) null;
      if ((bool) ((UnityEngine.Object) parentOfGameObject))
        gameObject.transform.parent = parentOfGameObject.transform;
      Undo.RegisterCreatedObjectUndo((UnityEngine.Object) gameObject, "Created `" + name + "`");
      return gameObject;
    }

    private bool IsListOfPlanesValid()
    {
      if (this.m_ParticleSystemUI.multiEdit)
      {
        for (int index = 0; index < 6; ++index)
        {
          int num1 = -1;
          foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
          {
            int num2 = !((UnityEngine.Object) particleSystem.collision.GetPlane(index) != (UnityEngine.Object) null) ? 0 : 1;
            if (num1 == -1)
              num1 = num2;
            else if (num2 != num1)
              return false;
          }
        }
      }
      return true;
    }

    private void DoListOfPlanesGUI()
    {
      if (!this.IsListOfPlanesValid())
      {
        EditorGUILayout.HelpBox("Plane list editing is only available when all selected systems contain the same number of planes", MessageType.Info, true);
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        int index = this.GUIListOfFloatObjectToggleFields(CollisionModuleUI.s_Texts.planes, this.m_ShownPlanes, (EditorGUI.ObjectFieldValidator) null, CollisionModuleUI.s_Texts.createPlane, !this.m_ParticleSystemUI.multiEdit);
        bool flag = EditorGUI.EndChangeCheck();
        if (index >= 0 && !this.m_ParticleSystemUI.multiEdit)
        {
          GameObject emptyGameObject = CollisionModuleUI.CreateEmptyGameObject("Plane Transform " + (object) (index + 1), this.m_ParticleSystemUI.m_ParticleSystems[0]);
          emptyGameObject.transform.localPosition = new Vector3(0.0f, 0.0f, (float) (10 + index));
          emptyGameObject.transform.localEulerAngles = new Vector3(-90f, 0.0f, 0.0f);
          this.m_ShownPlanes[index].objectReferenceValue = (UnityEngine.Object) emptyGameObject;
          flag = true;
        }
        Rect rect = GUILayoutUtility.GetRect(0.0f, 16f);
        rect.x = (float) ((double) rect.xMax - 24.0 - 5.0);
        rect.width = 12f;
        if (this.m_ShownPlanes.Length > 1 && ModuleUI.MinusButton(rect))
        {
          this.m_ShownPlanes[this.m_ShownPlanes.Length - 1].objectReferenceValue = (UnityEngine.Object) null;
          List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownPlanes);
          serializedPropertyList.RemoveAt(serializedPropertyList.Count - 1);
          this.m_ShownPlanes = serializedPropertyList.ToArray();
          flag = true;
        }
        if (this.m_ShownPlanes.Length < 6 && !this.m_ParticleSystemUI.multiEdit)
        {
          rect.x += 17f;
          if (ModuleUI.PlusButton(rect))
          {
            List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownPlanes);
            serializedPropertyList.Add(this.m_Planes[serializedPropertyList.Count]);
            this.m_ShownPlanes = serializedPropertyList.ToArray();
          }
        }
        if (!flag)
          return;
        this.SyncVisualization();
      }
    }

    public override void OnSceneViewGUI()
    {
      this.RenderCollisionBounds();
      this.CollisionPlanesSceneGUI();
    }

    private void CollisionPlanesSceneGUI()
    {
      if (this.m_ScenePlanes.Count == 0)
        return;
      Event current = Event.current;
      Color color1 = Handles.color;
      Color color2 = new Color(1f, 1f, 1f, 0.5f);
      for (int index = 0; index < this.m_ScenePlanes.Count; ++index)
      {
        if (!((UnityEngine.Object) this.m_ScenePlanes[index] == (UnityEngine.Object) null))
        {
          Transform scenePlane = this.m_ScenePlanes[index];
          Vector3 position1 = scenePlane.position;
          Quaternion rotation = scenePlane.rotation;
          Vector3 axis1 = rotation * Vector3.right;
          Vector3 normal = rotation * Vector3.up;
          Vector3 axis2 = rotation * Vector3.forward;
          bool disabled = EditorApplication.isPlaying && scenePlane.gameObject.isStatic;
          if (this.editingPlanes)
          {
            if (object.ReferenceEquals((object) CollisionModuleUI.s_SelectedTransform, (object) scenePlane))
            {
              EditorGUI.BeginChangeCheck();
              Vector3 vector3 = scenePlane.position;
              Quaternion quaternion = scenePlane.rotation;
              using (new EditorGUI.DisabledScope(disabled))
              {
                if (disabled)
                  Handles.ShowStaticLabel(position1);
                switch (UnityEditorInternal.EditMode.editMode)
                {
                  case UnityEditorInternal.EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesMove:
                    vector3 = Handles.PositionHandle(position1, rotation);
                    break;
                  case UnityEditorInternal.EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesRotate:
                    quaternion = Handles.RotationHandle(rotation, position1);
                    break;
                }
              }
              if (EditorGUI.EndChangeCheck())
              {
                Undo.RecordObject((UnityEngine.Object) scenePlane, "Modified Collision Plane Transform");
                scenePlane.position = vector3;
                scenePlane.rotation = quaternion;
                ParticleSystemEditorUtils.PerformCompleteResimulation();
              }
            }
            else
            {
              float num1 = HandleUtility.GetHandleSize(position1) * 0.6f;
              EventType eventType = current.type;
              if (current.type == EventType.Ignore && current.rawType == EventType.MouseUp)
                eventType = current.rawType;
              Vector3 position2 = position1;
              Quaternion identity = Quaternion.identity;
              double num2 = (double) num1;
              Vector3 zero = Vector3.zero;
              // ISSUE: reference to a compiler-generated field
              if (CollisionModuleUI.\u003C\u003Ef__mg\u0024cache0 == null)
              {
                // ISSUE: reference to a compiler-generated field
                CollisionModuleUI.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(Handles.RectangleHandleCap);
              }
              // ISSUE: reference to a compiler-generated field
              Handles.CapFunction fMgCache0 = CollisionModuleUI.\u003C\u003Ef__mg\u0024cache0;
              Handles.FreeMoveHandle(position2, identity, (float) num2, zero, fMgCache0);
              if (eventType == EventType.MouseDown && current.type == EventType.Used)
              {
                CollisionModuleUI.s_SelectedTransform = scenePlane;
                GUIUtility.hotControl = 0;
              }
            }
          }
          Handles.color = color2;
          Color color3 = Handles.s_ColliderHandleColor * 0.9f;
          if (disabled)
            color3.a *= 0.2f;
          if (CollisionModuleUI.m_PlaneVisualizationType == CollisionModuleUI.PlaneVizType.Grid)
            CollisionModuleUI.DrawGrid(position1, axis1, axis2, normal, color3);
          else
            CollisionModuleUI.DrawSolidPlane(position1, rotation, color3, Color.yellow);
        }
      }
      Handles.color = color1;
    }

    private void RenderCollisionBounds()
    {
      if (!CollisionModuleUI.s_VisualizeBounds)
        return;
      Color color = Handles.color;
      Handles.color = Color.green;
      Matrix4x4 matrix = Handles.matrix;
      Vector3[] dest1 = new Vector3[20];
      Vector3[] dest2 = new Vector3[20];
      Vector3[] dest3 = new Vector3[20];
      Handles.SetDiscSectionPoints(dest1, Vector3.zero, Vector3.forward, Vector3.right, 360f, 1f);
      Handles.SetDiscSectionPoints(dest2, Vector3.zero, Vector3.up, -Vector3.right, 360f, 1f);
      Handles.SetDiscSectionPoints(dest3, Vector3.zero, Vector3.right, Vector3.up, 360f, 1f);
      Vector3[] vector3Array = new Vector3[dest1.Length + dest2.Length + dest3.Length];
      dest1.CopyTo((Array) vector3Array, 0);
      dest2.CopyTo((Array) vector3Array, 20);
      dest3.CopyTo((Array) vector3Array, 40);
      foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
      {
        if (particleSystem.collision.enabled)
        {
          ParticleSystem.Particle[] particles1 = new ParticleSystem.Particle[particleSystem.particleCount];
          int particles2 = particleSystem.GetParticles(particles1);
          Matrix4x4 matrix4x4 = Matrix4x4.identity;
          if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local)
            matrix4x4 = particleSystem.GetLocalToWorldMatrix();
          for (int index = 0; index < particles2; ++index)
          {
            ParticleSystem.Particle particle = particles1[index];
            Vector3 currentSize3D = particle.GetCurrentSize3D(particleSystem);
            float num = Math.Max(currentSize3D.x, Math.Max(currentSize3D.y, currentSize3D.z)) * 0.5f * particleSystem.collision.radiusScale;
            Handles.matrix = matrix4x4 * Matrix4x4.TRS(particle.position, Quaternion.identity, new Vector3(num, num, num));
            Handles.DrawPolyLine(vector3Array);
          }
        }
      }
      Handles.color = color;
      Handles.matrix = matrix;
    }

    private static void DrawSolidPlane(Vector3 pos, Quaternion rot, Color faceColor, Color edgeColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Matrix4x4 matrix = Handles.matrix;
      float num = 10f * CollisionModuleUI.m_ScaleGrid;
      Handles.matrix = Matrix4x4.TRS(pos, rot, new Vector3(num, num, num)) * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0.0f, 0.0f), Vector3.one);
      Handles.DrawSolidRectangleWithOutline(new Rect(-0.5f, -0.5f, 1f, 1f), faceColor, edgeColor);
      Handles.DrawLine(Vector3.zero, Vector3.back / num);
      Handles.matrix = matrix;
    }

    private static void DrawGrid(Vector3 pos, Vector3 axis1, Vector3 axis2, Vector3 normal, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      if ((double) color.a <= 0.0)
        return;
      GL.Begin(1);
      float num1 = 10f * CollisionModuleUI.m_ScaleGrid;
      int num2 = Mathf.Clamp((int) num1, 10, 40);
      if (num2 % 2 == 0)
        ++num2;
      float num3 = num1 * 0.5f;
      float num4 = num1 / (float) (num2 - 1);
      Vector3 vector3_1 = axis1 * num1;
      Vector3 vector3_2 = axis2 * num1;
      Vector3 vector3_3 = axis1 * num4;
      Vector3 vector3_4 = axis2 * num4;
      Vector3 vector3_5 = pos - axis1 * num3 - axis2 * num3;
      for (int index = 0; index < num2; ++index)
      {
        if (index % 2 == 0)
          GL.Color(color * 0.7f);
        else
          GL.Color(color);
        GL.Vertex(vector3_5 + (float) index * vector3_3);
        GL.Vertex(vector3_5 + (float) index * vector3_3 + vector3_2);
        GL.Vertex(vector3_5 + (float) index * vector3_4);
        GL.Vertex(vector3_5 + (float) index * vector3_4 + vector3_1);
      }
      GL.Color(color);
      GL.Vertex(pos);
      GL.Vertex(pos + normal);
      GL.End();
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text += "\nCollision module is enabled.";
    }

    private enum CollisionTypes
    {
      Plane,
      World,
    }

    private enum CollisionModes
    {
      Mode3D,
      Mode2D,
    }

    private enum ForceModes
    {
      None,
      Constant,
      SizeBased,
    }

    private enum PlaneVizType
    {
      Grid,
      Solid,
    }

    private class Texts
    {
      public GUIContent lifetimeLoss = EditorGUIUtility.TextContent("Lifetime Loss|When particle collides, it will lose this fraction of its Start Lifetime");
      public GUIContent planes = EditorGUIUtility.TextContent("Planes|Planes are defined by assigning a reference to a transform. This transform can be any transform in the scene and can be animated. Multiple planes can be used. Note: the Y-axis is used as the plane normal.");
      public GUIContent createPlane = EditorGUIUtility.TextContent("|Create an empty GameObject and assign it as a plane.");
      public GUIContent minKillSpeed = EditorGUIUtility.TextContent("Min Kill Speed|When particles collide and their speed is lower than this value, they are killed.");
      public GUIContent maxKillSpeed = EditorGUIUtility.TextContent("Max Kill Speed|When particles collide and their speed is higher than this value, they are killed.");
      public GUIContent dampen = EditorGUIUtility.TextContent("Dampen|When particle collides, it will lose this fraction of its speed. Unless this is set to 0.0, particle will become slower after collision.");
      public GUIContent bounce = EditorGUIUtility.TextContent("Bounce|When particle collides, the bounce is scaled with this value. The bounce is the upwards motion in the plane normal direction.");
      public GUIContent radiusScale = EditorGUIUtility.TextContent("Radius Scale|Scale particle bounds by this amount to get more precise collisions.");
      public GUIContent visualization = EditorGUIUtility.TextContent("Visualization|Only used for visualizing the planes: Wireframe or Solid.");
      public GUIContent scalePlane = EditorGUIUtility.TextContent("Scale Plane|Resizes the visualization planes.");
      public GUIContent visualizeBounds = EditorGUIUtility.TextContent("Visualize Bounds|Render the collision bounds of the particles.");
      public GUIContent collidesWith = EditorGUIUtility.TextContent("Collides With|Collides the particles with colliders included in the layermask.");
      public GUIContent collidesWithDynamic = EditorGUIUtility.TextContent("Enable Dynamic Colliders|Should particles collide with dynamic objects?");
      public GUIContent maxCollisionShapes = EditorGUIUtility.TextContent("Max Collision Shapes|How many collision shapes can be considered for particle collisions. Excess shapes will be ignored. Terrains take priority.");
      public GUIContent quality = EditorGUIUtility.TextContent("Collision Quality|Quality of world collisions. Medium and low quality are approximate and may leak particles.");
      public GUIContent voxelSize = EditorGUIUtility.TextContent("Voxel Size|Size of voxels in the collision cache. Smaller values improve accuracy, but require higher memory usage and are less efficient.");
      public GUIContent collisionMessages = EditorGUIUtility.TextContent("Send Collision Messages|Send collision callback messages.");
      public GUIContent collisionType = EditorGUIUtility.TextContent("Type|Collide with a list of Planes, or the Physics World.");
      public GUIContent collisionMode = EditorGUIUtility.TextContent("Mode|Use 3D Physics or 2D Physics.");
      public GUIContent colliderForce = EditorGUIUtility.TextContent("Collider Force|Control the strength of particle forces on colliders.");
      public GUIContent multiplyColliderForceByCollisionAngle = EditorGUIUtility.TextContent("Multiply by Collision Angle|Should the force be proportional to the angle of the particle collision?  A particle collision directly along the collision normal produces all the specified force whilst collisions away from the collision normal produce less force.");
      public GUIContent multiplyColliderForceByParticleSpeed = EditorGUIUtility.TextContent("Multiply by Particle Speed|Should the force be proportional to the particle speed?");
      public GUIContent multiplyColliderForceByParticleSize = EditorGUIUtility.TextContent("Multiply by Particle Size|Should the force be proportional to the particle size?");
      public GUIContent[] collisionTypes = new GUIContent[2]{ EditorGUIUtility.TextContent("Planes"), EditorGUIUtility.TextContent("World") };
      public GUIContent[] collisionModes = new GUIContent[2]{ EditorGUIUtility.TextContent("3D"), EditorGUIUtility.TextContent("2D") };
      public GUIContent[] qualitySettings = new GUIContent[3]{ EditorGUIUtility.TextContent("High"), EditorGUIUtility.TextContent("Medium (Static Colliders)"), EditorGUIUtility.TextContent("Low (Static Colliders)") };
      public GUIContent[] planeVizTypes = new GUIContent[2]{ EditorGUIUtility.TextContent("Grid"), EditorGUIUtility.TextContent("Solid") };
      public GUIContent[] toolContents = new GUIContent[2]{ EditorGUIUtility.IconContent("MoveTool", "|Move plane editing mode."), EditorGUIUtility.IconContent("RotateTool", "|Rotate plane editing mode.") };
      public UnityEditorInternal.EditMode.SceneViewEditMode[] sceneViewEditModes = new UnityEditorInternal.EditMode.SceneViewEditMode[2]{ UnityEditorInternal.EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesMove, UnityEditorInternal.EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesRotate };
    }
  }
}
