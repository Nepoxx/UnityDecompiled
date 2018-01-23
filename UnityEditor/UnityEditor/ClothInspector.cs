// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClothInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Cloth))]
  internal class ClothInspector : Editor
  {
    private static Texture2D s_ColorTexture = (Texture2D) null;
    private static bool s_BrushCreated = false;
    public static PrefColor s_BrushColor = new PrefColor("Cloth/Brush Color 2", 0.0f, 0.0f, 0.0f, 0.2f);
    public static PrefColor s_SelfAndInterCollisionParticleColor = new PrefColor("Cloth/Self or Inter Collision Particle Color 2", 0.5686275f, 0.9568627f, 0.5450981f, 0.5f);
    public static PrefColor s_UnselectedSelfAndInterCollisionParticleColor = new PrefColor("Cloth/Unselected Self or Inter Collision Particle Color 2", 0.1f, 0.1f, 0.1f, 0.5f);
    public static PrefColor s_SelectedParticleColor = new PrefColor("Cloth/Selected Self or Inter Collision Particle Color 2", 0.2509804f, 0.627451f, 1f, 0.5f);
    public static ClothInspector.ToolMode[] s_ToolMode = new ClothInspector.ToolMode[2]{ ClothInspector.ToolMode.Paint, ClothInspector.ToolMode.Select };
    private int m_BrushFace = -1;
    private int m_MouseOver = -1;
    private bool m_RectSelecting = false;
    private bool m_DidSelect = false;
    private float[] m_MaxVisualizedValue = new float[3];
    private float[] m_MinVisualizedValue = new float[3];
    private ClothInspector.RectSelectionMode m_RectSelectionMode = ClothInspector.RectSelectionMode.Add;
    private int m_NumVerts = 0;
    private int m_NumSelection = 0;
    private bool[] m_ParticleSelection;
    private bool[] m_ParticleRectSelection;
    private bool[] m_SelfAndInterCollisionSelection;
    private Vector3[] m_ClothParticlesInWorldSpace;
    private Vector3 m_BrushPos;
    private Vector3 m_BrushNorm;
    private Vector3[] m_LastVertices;
    private Vector2 m_SelectStartPoint;
    private Vector2 m_SelectMousePoint;
    private const float kDisabledValue = 3.402823E+38f;
    private SerializedProperty m_SelfCollisionDistance;
    private SerializedProperty m_SelfCollisionStiffness;
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;

    private ClothInspectorState state
    {
      get
      {
        return ScriptableSingleton<ClothInspectorState>.instance;
      }
    }

    private ClothInspector.DrawMode drawMode
    {
      get
      {
        return this.state.DrawMode;
      }
      set
      {
        if (this.state.DrawMode == value)
          return;
        this.state.DrawMode = value;
        this.Repaint();
      }
    }

    private Cloth cloth
    {
      get
      {
        return (Cloth) this.target;
      }
    }

    public bool editingConstraints
    {
      get
      {
        return UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ClothConstraints && UnityEditorInternal.EditMode.IsOwner((Editor) this);
      }
    }

    public bool editingSelfAndInterCollisionParticles
    {
      get
      {
        return UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ClothSelfAndInterCollisionParticles && UnityEditorInternal.EditMode.IsOwner((Editor) this);
      }
    }

    private GUIContent GetDrawModeString(ClothInspector.DrawMode mode)
    {
      return ClothInspector.Styles.drawModeStrings[(int) mode];
    }

    private GUIContent GetCollVisModeString(ClothInspector.CollisionVisualizationMode mode)
    {
      return ClothInspector.Styles.collVisModeStrings[(int) mode];
    }

    private bool IsMeshValid()
    {
      if (this.cloth.vertices.Length != this.m_NumVerts)
      {
        this.InitInspector();
        return true;
      }
      return this.m_NumVerts != 0;
    }

    private Texture2D GenerateColorTexture(int width)
    {
      Texture2D texture2D = new Texture2D(width, 1, TextureFormat.RGBA32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      texture2D.hideFlags = HideFlags.DontSave;
      Color[] colors = new Color[width];
      for (int index = 0; index < width; ++index)
        colors[index] = this.GetGradientColor((float) index / (float) (width - 1));
      texture2D.SetPixels(colors);
      texture2D.Apply();
      return texture2D;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (this.targets.Length <= 1)
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        UnityEditorInternal.EditMode.DoInspectorToolbar(ClothInspector.Styles.sceneViewEditModes, ClothInspector.Styles.toolContents, (IToolModeOwner) this);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
      }
      if (this.editingSelfAndInterCollisionParticles)
      {
        if (this.state.SetSelfAndInterCollision || this.state.CollToolMode == ClothInspector.CollToolMode.Paint || this.state.CollToolMode == ClothInspector.CollToolMode.Erase)
        {
          if ((double) this.cloth.selfCollisionDistance > 0.0)
          {
            this.state.SelfCollisionDistance = this.cloth.selfCollisionDistance;
            this.m_SelfCollisionDistance.floatValue = this.cloth.selfCollisionDistance;
          }
          else
          {
            this.cloth.selfCollisionDistance = this.state.SelfCollisionDistance;
            this.m_SelfCollisionDistance.floatValue = this.state.SelfCollisionDistance;
          }
          if ((double) this.cloth.selfCollisionStiffness > 0.0)
          {
            this.state.SelfCollisionStiffness = this.cloth.selfCollisionStiffness;
            this.m_SelfCollisionStiffness.floatValue = this.cloth.selfCollisionStiffness;
          }
          else
          {
            this.cloth.selfCollisionStiffness = this.state.SelfCollisionStiffness;
            this.m_SelfCollisionStiffness.floatValue = this.cloth.selfCollisionStiffness;
          }
          EditorGUI.LabelField(GUILayoutUtility.GetRect(new GUIContent(), GUIStyle.none, new GUILayoutOption[2]
          {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(17f)
          }), ClothInspector.Styles.selfCollisionString, EditorStyles.boldLabel);
          EditorGUI.PropertyField(GUILayoutUtility.GetRect(new GUIContent(), GUIStyle.none, new GUILayoutOption[2]
          {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(17f)
          }), this.m_SelfCollisionDistance, ClothInspector.Styles.selfCollisionDistanceGUIContent);
          EditorGUI.PropertyField(GUILayoutUtility.GetRect(new GUIContent(), GUIStyle.none, new GUILayoutOption[2]
          {
            GUILayout.ExpandWidth(true),
            GUILayout.Height(17f)
          }), this.m_SelfCollisionStiffness, ClothInspector.Styles.selfCollisionStiffnessGUIContent);
          GUILayout.Space(10f);
        }
        if ((double) Physics.interCollisionDistance > 0.0)
          this.state.InterCollisionDistance = Physics.interCollisionDistance;
        else
          Physics.interCollisionDistance = this.state.InterCollisionDistance;
        if ((double) Physics.interCollisionStiffness > 0.0)
          this.state.InterCollisionStiffness = Physics.interCollisionStiffness;
        else
          Physics.interCollisionStiffness = this.state.InterCollisionStiffness;
      }
      Editor.DrawPropertiesExcluding(this.serializedObject, "m_SelfAndInterCollisionIndices", "m_VirtualParticleIndices", "m_SelfCollisionDistance", "m_SelfCollisionStiffness");
      this.serializedObject.ApplyModifiedProperties();
      if (!((UnityEngine.Object) this.cloth.GetComponent<MeshRenderer>() != (UnityEngine.Object) null))
        return;
      Debug.LogWarning((object) "MeshRenderer will not work with a cloth component! Use only SkinnedMeshRenderer. Any MeshRenderer's attached to a cloth component will be deleted at runtime.");
    }

    public bool Raycast(out Vector3 pos, out Vector3 norm, out int face)
    {
      RaycastHit hitInfo;
      if (this.cloth.gameObject.GetComponent<MeshCollider>().Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo, float.PositiveInfinity))
      {
        norm = hitInfo.normal;
        pos = hitInfo.point;
        face = hitInfo.triangleIndex;
        return true;
      }
      norm = (Vector3) Vector2.zero;
      pos = Vector3.zero;
      face = -1;
      return false;
    }

    private void UpdatePreviewBrush()
    {
      this.Raycast(out this.m_BrushPos, out this.m_BrushNorm, out this.m_BrushFace);
    }

    private void DrawBrush()
    {
      if (this.m_BrushFace < 0)
        return;
      Handles.color = (Color) ClothInspector.s_BrushColor;
      Handles.DrawSolidDisc(this.m_BrushPos, this.m_BrushNorm, this.state.BrushRadius);
    }

    internal override Bounds GetWorldBoundsOfTarget(UnityEngine.Object targetObject)
    {
      SkinnedMeshRenderer component = ((Component) targetObject).GetComponent<SkinnedMeshRenderer>();
      return !((UnityEngine.Object) component == (UnityEngine.Object) null) ? component.bounds : base.GetWorldBoundsOfTarget(targetObject);
    }

    private bool SelectionMeshDirty()
    {
      if (this.m_LastVertices != null)
      {
        Vector3[] vertices = this.cloth.vertices;
        Transform actualRootBone = this.m_SkinnedMeshRenderer.actualRootBone;
        if (this.m_LastVertices.Length != vertices.Length)
          return true;
        for (int index = 0; index < this.m_LastVertices.Length; ++index)
        {
          Vector3 vector3 = actualRootBone.rotation * vertices[index] + actualRootBone.position;
          if (!(this.m_LastVertices[index] == vector3))
            return true;
        }
      }
      return false;
    }

    private void GenerateSelectionMesh()
    {
      if (!this.IsMeshValid())
        return;
      Vector3[] vertices = this.cloth.vertices;
      int length = vertices.Length;
      this.m_ParticleSelection = new bool[length];
      this.m_ParticleRectSelection = new bool[length];
      this.m_LastVertices = new Vector3[length];
      Transform actualRootBone = this.m_SkinnedMeshRenderer.actualRootBone;
      for (int index = 0; index < length; ++index)
        this.m_LastVertices[index] = actualRootBone.rotation * vertices[index] + actualRootBone.position;
    }

    private void InitSelfAndInterCollisionSelection()
    {
      int length = this.cloth.vertices.Length;
      this.m_SelfAndInterCollisionSelection = new bool[length];
      for (int index = 0; index < length; ++index)
        this.m_SelfAndInterCollisionSelection[index] = false;
      List<uint> indices = new List<uint>(length);
      indices.Clear();
      this.cloth.GetSelfAndInterCollisionIndices(indices);
      int count = indices.Count;
      for (int index = 0; index < count; ++index)
        this.m_SelfAndInterCollisionSelection[(IntPtr) indices[index]] = true;
    }

    private void InitClothParticlesInWorldSpace()
    {
      Vector3[] vertices = this.cloth.vertices;
      int length = vertices.Length;
      this.m_ClothParticlesInWorldSpace = new Vector3[length];
      Transform actualRootBone = this.m_SkinnedMeshRenderer.actualRootBone;
      Quaternion rotation = actualRootBone.rotation;
      Vector3 position = actualRootBone.position;
      for (int index = 0; index < length; ++index)
        this.m_ClothParticlesInWorldSpace[index] = rotation * vertices[index] + position;
    }

    private void DrawSelfAndInterCollisionParticles()
    {
      Transform actualRootBone = this.m_SkinnedMeshRenderer.actualRootBone;
      Vector3[] vertices = this.cloth.vertices;
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      float collisionDistance = this.state.SelfCollisionDistance;
      if (this.state.VisualizeSelfOrInterCollision == ClothInspector.CollisionVisualizationMode.SelfCollision)
        collisionDistance = this.state.SelfCollisionDistance;
      else if (this.state.VisualizeSelfOrInterCollision == ClothInspector.CollisionVisualizationMode.InterCollision)
        collisionDistance = this.state.InterCollisionDistance;
      int length = this.m_SelfAndInterCollisionSelection.Length;
      for (int index = 0; index < length; ++index)
      {
        Vector3 vector3 = this.m_ClothParticlesInWorldSpace[index] - this.m_BrushPos;
        bool flag = (double) Vector3.Dot(actualRootBone.rotation * this.cloth.normals[index], Camera.current.transform.forward) <= 0.0;
        if (flag || this.state.ManipulateBackfaces)
        {
          if (this.m_SelfAndInterCollisionSelection[index] && !this.m_ParticleSelection[index])
            Handles.color = (Color) ClothInspector.s_SelfAndInterCollisionParticleColor;
          else if (!this.m_SelfAndInterCollisionSelection[index] && !this.m_ParticleSelection[index])
            Handles.color = (Color) ClothInspector.s_UnselectedSelfAndInterCollisionParticleColor;
          if (this.m_ParticleSelection[index] && this.m_NumSelection > 0 && this.state.CollToolMode == ClothInspector.CollToolMode.Select)
            Handles.color = (Color) ClothInspector.s_SelectedParticleColor;
          if ((double) vector3.magnitude < (double) this.state.BrushRadius && flag && (this.state.CollToolMode == ClothInspector.CollToolMode.Paint || this.state.CollToolMode == ClothInspector.CollToolMode.Erase))
            Handles.color = (Color) ClothInspector.s_SelectedParticleColor;
          Handles.SphereHandleCap(controlId, this.m_ClothParticlesInWorldSpace[index], actualRootBone.rotation, collisionDistance, EventType.Repaint);
        }
      }
    }

    private void InitInspector()
    {
      this.InitBrushCollider();
      this.InitSelfAndInterCollisionSelection();
      this.InitClothParticlesInWorldSpace();
      this.m_NumVerts = this.cloth.vertices.Length;
    }

    private void OnEnable()
    {
      if ((UnityEngine.Object) ClothInspector.s_ColorTexture == (UnityEngine.Object) null)
        ClothInspector.s_ColorTexture = this.GenerateColorTexture(100);
      this.m_SkinnedMeshRenderer = this.cloth.GetComponent<SkinnedMeshRenderer>();
      this.InitInspector();
      this.GenerateSelectionMesh();
      this.m_SelfCollisionDistance = this.serializedObject.FindProperty("m_SelfCollisionDistance");
      this.m_SelfCollisionStiffness = this.serializedObject.FindProperty("m_SelfCollisionStiffness");
      SceneView.onPreSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
    }

    private void InitBrushCollider()
    {
      if (!((UnityEngine.Object) this.cloth != (UnityEngine.Object) null))
        return;
      GameObject gameObject = this.cloth.gameObject;
      MeshCollider component = gameObject.GetComponent<MeshCollider>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.hideFlags == (HideFlags.HideInHierarchy | HideFlags.HideInInspector))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component);
      MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
      meshCollider.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
      meshCollider.sharedMesh = this.m_SkinnedMeshRenderer.sharedMesh;
      ClothInspector.s_BrushCreated = true;
    }

    public void OnDestroy()
    {
      SceneView.onPreSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
      if (!ClothInspector.s_BrushCreated || !((UnityEngine.Object) this.cloth != (UnityEngine.Object) null))
        return;
      MeshCollider component = this.cloth.gameObject.GetComponent<MeshCollider>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.hideFlags == (HideFlags.HideInHierarchy | HideFlags.HideInInspector))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component);
      ClothInspector.s_BrushCreated = false;
    }

    private float GetCoefficient(ClothSkinningCoefficient coefficient)
    {
      switch (this.drawMode)
      {
        case ClothInspector.DrawMode.MaxDistance:
          return coefficient.maxDistance;
        case ClothInspector.DrawMode.CollisionSphereDistance:
          return coefficient.collisionSphereDistance;
        default:
          return 0.0f;
      }
    }

    private Color GetGradientColor(float val)
    {
      if ((double) val < 0.300000011920929)
        return Color.Lerp(Color.red, Color.magenta, val / 0.2f);
      if ((double) val < 0.699999988079071)
        return Color.Lerp(Color.magenta, Color.yellow, (float) (((double) val - 0.200000002980232) / 0.5));
      return Color.Lerp(Color.yellow, Color.green, (float) (((double) val - 0.699999988079071) / 0.300000011920929));
    }

    private void OnDisable()
    {
      SceneView.onPreSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnPreSceneGUICallback);
    }

    private float CoefficientField(float value, float useValue, bool enabled, ClothInspector.DrawMode mode)
    {
      GUIContent drawModeString = this.GetDrawModeString(mode);
      using (new EditorGUI.DisabledScope(!enabled))
      {
        GUILayout.BeginHorizontal();
        EditorGUI.showMixedValue = (double) useValue < 0.0;
        EditorGUI.BeginChangeCheck();
        useValue = !EditorGUILayout.Toggle(GUIContent.none, (double) useValue != 0.0, new GUILayoutOption[0]) ? 0.0f : 1f;
        if (EditorGUI.EndChangeCheck())
        {
          value = (double) useValue <= 0.0 ? float.MaxValue : 0.0f;
          this.drawMode = mode;
        }
        GUILayout.Space(-152f);
        EditorGUI.showMixedValue = false;
        using (new EditorGUI.DisabledScope((double) useValue != 1.0))
        {
          float num1 = value;
          EditorGUI.showMixedValue = (double) value < 0.0;
          EditorGUI.BeginChangeCheck();
          int keyboardControl = GUIUtility.keyboardControl;
          if ((double) useValue > 0.0)
          {
            num1 = EditorGUILayout.FloatField(drawModeString, value, new GUILayoutOption[0]);
          }
          else
          {
            double num2 = (double) EditorGUILayout.FloatField(drawModeString, 0.0f, new GUILayoutOption[0]);
          }
          bool flag = EditorGUI.EndChangeCheck();
          if (flag)
          {
            value = num1;
            if ((double) value < 0.0)
              value = 0.0f;
          }
          if (!flag)
          {
            if (keyboardControl == GUIUtility.keyboardControl)
              goto label_16;
          }
          this.drawMode = mode;
        }
      }
label_16:
      if ((double) useValue > 0.0)
      {
        float num1 = this.m_MinVisualizedValue[(int) mode];
        float num2 = this.m_MaxVisualizedValue[(int) mode];
        if ((double) num2 - (double) num1 > 0.0)
          this.DrawColorBox((Texture) null, this.GetGradientColor((float) (((double) value - (double) num1) / ((double) num2 - (double) num1))));
        else
          this.DrawColorBox((Texture) null, this.GetGradientColor((double) value > (double) num1 ? 1f : 0.0f));
      }
      else
        this.DrawColorBox((Texture) null, Color.black);
      EditorGUI.showMixedValue = false;
      GUILayout.EndHorizontal();
      return value;
    }

    private float PaintField(float value, ref bool enabled, ClothInspector.DrawMode mode)
    {
      GUIContent drawModeString = this.GetDrawModeString(mode);
      GUILayout.BeginHorizontal();
      enabled = (GUILayout.Toggle((enabled ? 1 : 0) != 0, ClothInspector.Styles.paintIcon, (GUIStyle) "MiniButton", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }) ? 1 : 0) != 0;
      bool flag;
      float num1;
      using (new EditorGUI.DisabledScope(!enabled))
      {
        EditorGUI.BeginChangeCheck();
        flag = EditorGUILayout.Toggle(GUIContent.none, (double) value < 3.40282346638529E+38, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          value = !flag ? float.MaxValue : 0.0f;
          this.drawMode = mode;
        }
        GUILayout.Space(-162f);
        using (new EditorGUI.DisabledScope(!flag))
        {
          num1 = value;
          int keyboardControl = GUIUtility.keyboardControl;
          EditorGUI.BeginChangeCheck();
          if (flag)
          {
            num1 = EditorGUILayout.FloatField(drawModeString, value, new GUILayoutOption[0]);
          }
          else
          {
            double num2 = (double) EditorGUILayout.FloatField(drawModeString, 0.0f, new GUILayoutOption[0]);
          }
          if ((double) num1 < 0.0)
            num1 = 0.0f;
          if (!EditorGUI.EndChangeCheck())
          {
            if (keyboardControl == GUIUtility.keyboardControl)
              goto label_14;
          }
          this.drawMode = mode;
        }
      }
label_14:
      if (flag)
      {
        float num2 = this.m_MinVisualizedValue[(int) mode];
        float num3 = this.m_MaxVisualizedValue[(int) mode];
        if ((double) num3 - (double) num2 > 0.0)
          this.DrawColorBox((Texture) null, this.GetGradientColor((float) (((double) value - (double) num2) / ((double) num3 - (double) num2))));
        else
          this.DrawColorBox((Texture) null, this.GetGradientColor((double) value > (double) num2 ? 1f : 0.0f));
      }
      else
        this.DrawColorBox((Texture) null, Color.black);
      GUILayout.EndHorizontal();
      return num1;
    }

    private void SelectionGUI()
    {
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      float num1 = 0.0f;
      float useValue1 = 0.0f;
      float num2 = 0.0f;
      float useValue2 = 0.0f;
      int num3 = 0;
      bool flag = true;
      for (int index = 0; index < this.m_ParticleSelection.Length; ++index)
      {
        if (this.m_ParticleSelection[index])
        {
          if (flag)
          {
            num1 = coefficients[index].maxDistance;
            useValue1 = (double) num1 >= 3.40282346638529E+38 ? 0.0f : 1f;
            num2 = coefficients[index].collisionSphereDistance;
            useValue2 = (double) num2 >= 3.40282346638529E+38 ? 0.0f : 1f;
            flag = false;
          }
          if ((double) coefficients[index].maxDistance != (double) num1)
            num1 = -1f;
          if ((double) coefficients[index].collisionSphereDistance != (double) num2)
            num2 = -1f;
          if ((double) useValue1 != ((double) coefficients[index].maxDistance >= 3.40282346638529E+38 ? 0.0 : 1.0))
            useValue1 = -1f;
          if ((double) useValue2 != ((double) coefficients[index].collisionSphereDistance >= 3.40282346638529E+38 ? 0.0 : 1.0))
            useValue2 = -1f;
          ++num3;
        }
      }
      float num4 = this.CoefficientField(num1, useValue1, num3 > 0, ClothInspector.DrawMode.MaxDistance);
      if ((double) num4 != (double) num1)
      {
        for (int index = 0; index < coefficients.Length; ++index)
        {
          if (this.m_ParticleSelection[index])
            coefficients[index].maxDistance = num4;
        }
        this.cloth.coefficients = coefficients;
        Undo.RegisterCompleteObjectUndo(this.target, "Change Cloth Coefficients");
      }
      float num5 = this.CoefficientField(num2, useValue2, num3 > 0, ClothInspector.DrawMode.CollisionSphereDistance);
      if ((double) num5 != (double) num2)
      {
        for (int index = 0; index < coefficients.Length; ++index)
        {
          if (this.m_ParticleSelection[index])
            coefficients[index].collisionSphereDistance = num5;
        }
        this.cloth.coefficients = coefficients;
        Undo.RegisterCompleteObjectUndo(this.target, "Change Cloth Coefficients");
      }
      using (new EditorGUI.DisabledScope(true))
      {
        GUILayout.BeginHorizontal();
        if (num3 > 0)
        {
          GUILayout.FlexibleSpace();
          GUILayout.Label(num3.ToString() + " selected");
        }
        else
        {
          GUILayout.Label("Select cloth vertices to edit their constraints.");
          GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
      }
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Backspace)
        return;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        if (this.m_ParticleSelection[index])
        {
          switch (this.drawMode)
          {
            case ClothInspector.DrawMode.MaxDistance:
              coefficients[index].maxDistance = float.MaxValue;
              break;
            case ClothInspector.DrawMode.CollisionSphereDistance:
              coefficients[index].collisionSphereDistance = float.MaxValue;
              break;
          }
        }
      }
      this.cloth.coefficients = coefficients;
    }

    private void CollSelectionGUI()
    {
      if (!this.IsMeshValid())
        return;
      bool flag1 = false;
      bool flag2 = false;
      int num = 0;
      int length = this.m_ParticleRectSelection.Length;
      for (int index = 0; index < length; ++index)
      {
        if (this.m_ParticleRectSelection[index])
        {
          if (!flag1)
          {
            this.state.SetSelfAndInterCollision = this.m_SelfAndInterCollisionSelection[index];
            flag1 = true;
          }
          else if (this.state.SetSelfAndInterCollision != this.m_SelfAndInterCollisionSelection[index])
          {
            flag2 = true;
            this.state.SetSelfAndInterCollision = false;
          }
          ++num;
        }
      }
      this.m_NumSelection = num;
      if (this.m_NumSelection == 0)
        this.state.SetSelfAndInterCollision = false;
      using (new EditorGUI.DisabledScope(this.m_NumSelection == 0))
      {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.showMixedValue = flag2;
        EditorGUI.BeginChangeCheck();
        bool flag3 = EditorGUILayout.Toggle(GUIContent.none, this.state.SetSelfAndInterCollision, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          this.state.SetSelfAndInterCollision = flag3;
          for (int index = 0; index < length; ++index)
          {
            if (this.m_ParticleRectSelection[index])
              this.m_SelfAndInterCollisionSelection[index] = this.state.SetSelfAndInterCollision;
          }
          Undo.RegisterCompleteObjectUndo(this.target, "Change Cloth Particles Selected for self or inter collision");
        }
        EditorGUILayout.LabelField(ClothInspector.Styles.setSelfAndInterCollisionString);
        EditorGUI.showMixedValue = false;
        EditorGUILayout.EndHorizontal();
      }
    }

    private void EditBrushSize()
    {
      EditorGUI.BeginChangeCheck();
      float num = EditorGUILayout.FloatField(ClothInspector.Styles.brushRadiusString, this.state.BrushRadius, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.state.BrushRadius = num;
      if ((double) this.state.BrushRadius < 0.0)
        this.state.BrushRadius = 0.0f;
    }

    private void PaintGUI()
    {
      this.state.PaintMaxDistance = this.PaintField(this.state.PaintMaxDistance, ref this.state.PaintMaxDistanceEnabled, ClothInspector.DrawMode.MaxDistance);
      this.state.PaintCollisionSphereDistance = this.PaintField(this.state.PaintCollisionSphereDistance, ref this.state.PaintCollisionSphereDistanceEnabled, ClothInspector.DrawMode.CollisionSphereDistance);
      if (this.state.PaintMaxDistanceEnabled && !this.state.PaintCollisionSphereDistanceEnabled)
        this.drawMode = ClothInspector.DrawMode.MaxDistance;
      else if (!this.state.PaintMaxDistanceEnabled && this.state.PaintCollisionSphereDistanceEnabled)
        this.drawMode = ClothInspector.DrawMode.CollisionSphereDistance;
      using (new EditorGUI.DisabledScope(true))
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Set constraints to paint onto cloth vertices.");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
      }
      this.EditBrushSize();
    }

    private int GetMouseVertex(Event e)
    {
      if (Tools.current != Tool.None || this.m_LastVertices == null)
        return -1;
      Vector3[] normals = this.cloth.normals;
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
      float num1 = 1000f;
      int num2 = -1;
      Quaternion rotation = this.m_SkinnedMeshRenderer.actualRootBone.rotation;
      for (int index = 0; index < coefficients.Length; ++index)
      {
        float sqrMagnitude = Vector3.Cross(this.m_LastVertices[index] - worldRay.origin, worldRay.direction).sqrMagnitude;
        if (((double) Vector3.Dot(rotation * normals[index], Camera.current.transform.forward) <= 0.0 || this.state.ManipulateBackfaces) && ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < 0.00250000017695129))
        {
          num1 = sqrMagnitude;
          num2 = index;
        }
      }
      return num2;
    }

    private void DrawConstraints()
    {
      if (this.SelectionMeshDirty())
        this.GenerateSelectionMesh();
      Transform actualRootBone = this.m_SkinnedMeshRenderer.actualRootBone;
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      int length = coefficients.Length;
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index = 0; index < length; ++index)
      {
        float coefficient = this.GetCoefficient(coefficients[index]);
        if ((double) coefficient < 3.40282346638529E+38)
        {
          if ((double) coefficient < (double) num1)
            num1 = coefficient;
          if ((double) coefficient > (double) num2)
            num2 = coefficient;
        }
      }
      this.m_MaxVisualizedValue[(int) this.drawMode] = num2;
      this.m_MinVisualizedValue[(int) this.drawMode] = num1;
      Vector3[] normals = this.cloth.normals;
      for (int index = 0; index < length; ++index)
      {
        bool flag = (double) Vector3.Dot(actualRootBone.rotation * normals[index], Camera.current.transform.forward) <= 0.0;
        if (flag || this.state.ManipulateBackfaces)
        {
          float coefficient = this.GetCoefficient(coefficients[index]);
          Handles.color = (double) coefficient < 3.40282346638529E+38 ? this.GetGradientColor((double) num2 - (double) num1 == 0.0 ? 0.0f : (float) (((double) coefficient - (double) num1) / ((double) num2 - (double) num1))) : Color.black;
          Vector3 vector3 = this.m_ClothParticlesInWorldSpace[index] - this.m_BrushPos;
          if (this.m_ParticleSelection[index] && this.state.CollToolMode == ClothInspector.CollToolMode.Select)
            Handles.color = (Color) ClothInspector.s_SelectedParticleColor;
          if ((double) vector3.magnitude < (double) this.state.BrushRadius && flag && this.state.ToolMode == ClothInspector.ToolMode.Paint)
            Handles.color = (Color) ClothInspector.s_SelectedParticleColor;
          Handles.SphereHandleCap(controlId, this.m_ClothParticlesInWorldSpace[index], actualRootBone.rotation, this.state.ConstraintSize, EventType.Repaint);
        }
      }
    }

    private bool UpdateRectParticleSelection()
    {
      if (!this.IsMeshValid())
        return false;
      bool flag1 = false;
      Vector3[] normals = this.cloth.normals;
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      float x1 = Mathf.Min(this.m_SelectStartPoint.x, this.m_SelectMousePoint.x);
      float x2 = Mathf.Max(this.m_SelectStartPoint.x, this.m_SelectMousePoint.x);
      float y1 = Mathf.Min(this.m_SelectStartPoint.y, this.m_SelectMousePoint.y);
      float y2 = Mathf.Max(this.m_SelectStartPoint.y, this.m_SelectMousePoint.y);
      Ray worldRay1 = HandleUtility.GUIPointToWorldRay(new Vector2(x1, y1));
      Ray worldRay2 = HandleUtility.GUIPointToWorldRay(new Vector2(x2, y1));
      Ray worldRay3 = HandleUtility.GUIPointToWorldRay(new Vector2(x1, y2));
      Ray worldRay4 = HandleUtility.GUIPointToWorldRay(new Vector2(x2, y2));
      Plane plane1 = new Plane(worldRay2.origin + worldRay2.direction, worldRay1.origin + worldRay1.direction, worldRay1.origin);
      Plane plane2 = new Plane(worldRay3.origin + worldRay3.direction, worldRay4.origin + worldRay4.direction, worldRay4.origin);
      Plane plane3 = new Plane(worldRay1.origin + worldRay1.direction, worldRay3.origin + worldRay3.direction, worldRay3.origin);
      Plane plane4 = new Plane(worldRay4.origin + worldRay4.direction, worldRay2.origin + worldRay2.direction, worldRay2.origin);
      Quaternion rotation = this.m_SkinnedMeshRenderer.actualRootBone.rotation;
      int length = coefficients.Length;
      for (int index = 0; index < length; ++index)
      {
        Vector3 lastVertex = this.m_LastVertices[index];
        bool flag2 = (double) Vector3.Dot(rotation * normals[index], Camera.current.transform.forward) <= 0.0;
        bool flag3 = plane1.GetSide(lastVertex) && plane2.GetSide(lastVertex) && plane3.GetSide(lastVertex) && plane4.GetSide(lastVertex) && (this.state.ManipulateBackfaces || flag2);
        if (this.m_ParticleRectSelection[index] != flag3)
        {
          this.m_ParticleRectSelection[index] = flag3;
          flag1 = true;
        }
      }
      return flag1;
    }

    private void ApplyRectSelection()
    {
      if (!this.IsMeshValid())
        return;
      int length = this.cloth.coefficients.Length;
      for (int index = 0; index < length; ++index)
      {
        switch (this.m_RectSelectionMode)
        {
          case ClothInspector.RectSelectionMode.Replace:
            this.m_ParticleSelection[index] = this.m_ParticleRectSelection[index];
            break;
          case ClothInspector.RectSelectionMode.Add:
            this.m_ParticleSelection[index] |= this.m_ParticleRectSelection[index];
            break;
          case ClothInspector.RectSelectionMode.Substract:
            this.m_ParticleSelection[index] = this.m_ParticleSelection[index] && !this.m_ParticleRectSelection[index];
            break;
        }
      }
    }

    private bool RectSelectionModeFromEvent()
    {
      Event current = Event.current;
      ClothInspector.RectSelectionMode rectSelectionMode = ClothInspector.RectSelectionMode.Replace;
      if (current.shift)
        rectSelectionMode = ClothInspector.RectSelectionMode.Add;
      if (current.alt)
        rectSelectionMode = ClothInspector.RectSelectionMode.Substract;
      if (this.m_RectSelectionMode == rectSelectionMode)
        return false;
      this.m_RectSelectionMode = rectSelectionMode;
      return true;
    }

    internal void SendCommandsOnModifierKeys()
    {
      SceneView.lastActiveSceneView.SendEvent(EditorGUIUtility.CommandEvent("ModifierKeysChanged"));
    }

    private void SelectionPreSceneGUI(int id)
    {
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.alt || current.control || (current.command || current.button != 0))
            break;
          GUIUtility.hotControl = id;
          int mouseVertex = this.GetMouseVertex(current);
          if (mouseVertex != -1)
          {
            if (current.shift)
            {
              this.m_ParticleSelection[mouseVertex] = !this.m_ParticleSelection[mouseVertex];
            }
            else
            {
              for (int index = 0; index < this.m_ParticleSelection.Length; ++index)
                this.m_ParticleSelection[index] = false;
              this.m_ParticleSelection[mouseVertex] = true;
            }
            this.m_DidSelect = true;
            this.Repaint();
          }
          else
            this.m_DidSelect = false;
          this.m_SelectStartPoint = current.mousePosition;
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != id || current.button != 0)
            break;
          GUIUtility.hotControl = 0;
          if (this.m_RectSelecting)
          {
            EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(this.SendCommandsOnModifierKeys);
            this.m_RectSelecting = false;
            this.RectSelectionModeFromEvent();
            this.ApplyRectSelection();
          }
          else if (!this.m_DidSelect && !current.alt && (!current.control && !current.command))
          {
            ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
            for (int index = 0; index < coefficients.Length; ++index)
              this.m_ParticleSelection[index] = false;
          }
          GUIUtility.keyboardControl = 0;
          SceneView.RepaintAll();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != id)
            break;
          if (!this.m_RectSelecting && ((double) (current.mousePosition - this.m_SelectStartPoint).magnitude > 2.0 && !current.alt && (!current.control && !current.command)))
          {
            EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(this.SendCommandsOnModifierKeys);
            this.m_RectSelecting = true;
            this.RectSelectionModeFromEvent();
          }
          if (this.m_RectSelecting)
          {
            this.m_SelectMousePoint = new Vector2(Mathf.Max(current.mousePosition.x, 0.0f), Mathf.Max(current.mousePosition.y, 0.0f));
            this.RectSelectionModeFromEvent();
            this.UpdateRectParticleSelection();
            current.Use();
          }
          break;
        default:
          if (typeForControl != EventType.ExecuteCommand || !this.m_RectSelecting || !(current.commandName == "ModifierKeysChanged"))
            break;
          this.RectSelectionModeFromEvent();
          this.UpdateRectParticleSelection();
          break;
      }
    }

    private void GetBrushedConstraints(Event e)
    {
      if (!this.IsMeshValid())
        return;
      Vector3[] vertices = this.cloth.vertices;
      Vector3[] normals = this.cloth.normals;
      ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
      Quaternion rotation = this.m_SkinnedMeshRenderer.actualRootBone.rotation;
      int length = vertices.Length;
      for (int index = 0; index < length; ++index)
      {
        if ((double) (this.m_ClothParticlesInWorldSpace[index] - this.m_BrushPos).magnitude < (double) this.state.BrushRadius && ((double) Vector3.Dot(rotation * normals[index], Camera.current.transform.forward) <= 0.0 || this.state.ManipulateBackfaces))
        {
          bool flag = false;
          if (this.state.PaintMaxDistanceEnabled && (double) coefficients[index].maxDistance != (double) this.state.PaintMaxDistance)
          {
            coefficients[index].maxDistance = this.state.PaintMaxDistance;
            flag = true;
          }
          if (this.state.PaintCollisionSphereDistanceEnabled && (double) coefficients[index].collisionSphereDistance != (double) this.state.PaintCollisionSphereDistance)
          {
            coefficients[index].collisionSphereDistance = this.state.PaintCollisionSphereDistance;
            flag = true;
          }
          if (flag)
          {
            Undo.RegisterCompleteObjectUndo(this.target, "Paint Cloth Constraints");
            this.cloth.coefficients = coefficients;
            this.Repaint();
          }
        }
      }
    }

    private void GetBrushedParticles(Event e)
    {
      if (!this.IsMeshValid())
        return;
      Vector3[] vertices = this.cloth.vertices;
      Vector3[] normals = this.cloth.normals;
      Quaternion rotation = this.m_SkinnedMeshRenderer.actualRootBone.rotation;
      int length = vertices.Length;
      for (int index = 0; index < length; ++index)
      {
        if ((double) (this.m_ClothParticlesInWorldSpace[index] - this.m_BrushPos).magnitude < (double) this.state.BrushRadius && ((double) Vector3.Dot(rotation * normals[index], Camera.current.transform.forward) <= 0.0 || this.state.ManipulateBackfaces))
        {
          if (e.button == 0)
          {
            if (this.state.CollToolMode == ClothInspector.CollToolMode.Paint)
              this.m_SelfAndInterCollisionSelection[index] = true;
            else if (this.state.CollToolMode == ClothInspector.CollToolMode.Erase)
              this.m_SelfAndInterCollisionSelection[index] = false;
          }
          int controlId = GUIUtility.GetControlID(FocusType.Passive);
          float collisionDistance = this.cloth.selfCollisionDistance;
          if (this.state.VisualizeSelfOrInterCollision == ClothInspector.CollisionVisualizationMode.SelfCollision)
            collisionDistance = this.cloth.selfCollisionDistance;
          else if (this.state.VisualizeSelfOrInterCollision == ClothInspector.CollisionVisualizationMode.InterCollision)
            collisionDistance = Physics.interCollisionDistance;
          Handles.color = (Color) ClothInspector.s_SelectedParticleColor;
          Handles.SphereHandleCap(controlId, this.m_ClothParticlesInWorldSpace[index], rotation, collisionDistance, EventType.Repaint);
          this.Repaint();
        }
      }
      Undo.RegisterCompleteObjectUndo(this.target, "Paint Collision");
    }

    private void PaintPreSceneGUI(int id)
    {
      if (!this.IsMeshValid())
        return;
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.MouseDown:
        case EventType.MouseDrag:
          ClothSkinningCoefficient[] coefficients = this.cloth.coefficients;
          if (GUIUtility.hotControl != id && (current.alt || current.control || (current.command || current.button != 0)))
            break;
          if (typeForControl == EventType.MouseDown)
            GUIUtility.hotControl = id;
          if (this.editingSelfAndInterCollisionParticles)
            this.GetBrushedParticles(current);
          if (this.editingConstraints)
            this.GetBrushedConstraints(current);
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && current.button == 0)
          {
            GUIUtility.hotControl = 0;
            current.Use();
          }
          break;
      }
    }

    private void OnPreSceneGUICallback(SceneView sceneView)
    {
      if (this.targets.Length > 1 || !this.editingConstraints && !this.editingSelfAndInterCollisionParticles)
        return;
      this.OnPreSceneGUI();
    }

    private void OnPreSceneGUI()
    {
      if (!this.IsMeshValid())
        return;
      Tools.current = Tool.None;
      if (this.state.ToolMode == ~ClothInspector.ToolMode.Select)
        this.state.ToolMode = ClothInspector.ToolMode.Select;
      if (this.m_ParticleSelection == null)
        this.GenerateSelectionMesh();
      else if (this.m_ParticleSelection.Length != this.cloth.coefficients.Length)
        this.InitInspector();
      Handles.BeginGUI();
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseMove:
        case EventType.MouseDrag:
          int mouseOver = this.m_MouseOver;
          this.m_MouseOver = this.GetMouseVertex(current);
          if (this.m_MouseOver != mouseOver)
          {
            SceneView.RepaintAll();
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
      if (this.editingConstraints)
      {
        switch (this.state.ToolMode)
        {
          case ClothInspector.ToolMode.Select:
            this.SelectionPreSceneGUI(controlId);
            break;
          case ClothInspector.ToolMode.Paint:
            this.PaintPreSceneGUI(controlId);
            break;
        }
      }
      if (this.editingSelfAndInterCollisionParticles)
      {
        switch (this.state.CollToolMode)
        {
          case ClothInspector.CollToolMode.Select:
            this.SelectionPreSceneGUI(controlId);
            break;
          case ClothInspector.CollToolMode.Paint:
          case ClothInspector.CollToolMode.Erase:
            this.PaintPreSceneGUI(controlId);
            break;
        }
      }
      Handles.EndGUI();
    }

    public void OnSceneGUI()
    {
      if (this.editingConstraints)
      {
        this.OnSceneEditConstraintsGUI();
      }
      else
      {
        if (!this.editingSelfAndInterCollisionParticles)
          return;
        this.OnSceneEditSelfAndInterCollisionParticlesGUI();
      }
    }

    private void OnSceneEditConstraintsGUI()
    {
      if (Event.current.type == EventType.Repaint && this.state.ToolMode == ClothInspector.ToolMode.Paint)
      {
        this.UpdatePreviewBrush();
        this.DrawBrush();
      }
      if (Selection.gameObjects.Length > 1)
        return;
      if (Event.current.type == EventType.Repaint)
        this.DrawConstraints();
      Event current = Event.current;
      if (current.commandName == "SelectAll")
      {
        if (current.type == EventType.ValidateCommand)
          current.Use();
        if (current.type == EventType.ExecuteCommand)
        {
          int length = this.cloth.vertices.Length;
          for (int index = 0; index < length; ++index)
            this.m_ParticleSelection[index] = true;
          SceneView.RepaintAll();
          this.state.ToolMode = ClothInspector.ToolMode.Select;
          current.Use();
        }
      }
      Handles.BeginGUI();
      if (this.m_RectSelecting && this.state.ToolMode == ClothInspector.ToolMode.Select && Event.current.type == EventType.Repaint)
        EditorStyles.selectionRect.Draw(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), GUIContent.none, false, false, false, false);
      Handles.EndGUI();
      SceneViewOverlay.Window(new GUIContent("Cloth Constraints"), new SceneViewOverlay.WindowFunction(this.ConstraintEditing), 0, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
    }

    private void OnSceneEditSelfAndInterCollisionParticlesGUI()
    {
      if (Selection.gameObjects.Length > 1)
        return;
      this.DrawSelfAndInterCollisionParticles();
      if (Event.current.type == EventType.Repaint && (this.state.CollToolMode == ClothInspector.CollToolMode.Paint || this.state.CollToolMode == ClothInspector.CollToolMode.Erase))
      {
        this.UpdatePreviewBrush();
        this.DrawBrush();
      }
      Handles.BeginGUI();
      if (this.m_RectSelecting && this.state.CollToolMode == ClothInspector.CollToolMode.Select && Event.current.type == EventType.Repaint)
        EditorStyles.selectionRect.Draw(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), GUIContent.none, false, false, false, false);
      Handles.EndGUI();
      SceneViewOverlay.Window(ClothInspector.Styles.clothSelfCollisionAndInterCollision, new SceneViewOverlay.WindowFunction(this.SelfAndInterCollisionEditing), 100, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
    }

    public void VisualizationMenuSetMaxDistanceMode()
    {
      this.drawMode = ClothInspector.DrawMode.MaxDistance;
      if (this.state.PaintMaxDistanceEnabled)
        return;
      this.state.PaintCollisionSphereDistanceEnabled = false;
      this.state.PaintMaxDistanceEnabled = true;
    }

    public void VisualizationMenuSetCollisionSphereMode()
    {
      this.drawMode = ClothInspector.DrawMode.CollisionSphereDistance;
      if (this.state.PaintCollisionSphereDistanceEnabled)
        return;
      this.state.PaintCollisionSphereDistanceEnabled = true;
      this.state.PaintMaxDistanceEnabled = false;
    }

    public void VisualizationMenuToggleManipulateBackfaces()
    {
      this.state.ManipulateBackfaces = !this.state.ManipulateBackfaces;
    }

    public void VisualizationMenuSelfCollision()
    {
      this.state.VisualizeSelfOrInterCollision = ClothInspector.CollisionVisualizationMode.SelfCollision;
    }

    public void VisualizationMenuInterCollision()
    {
      this.state.VisualizeSelfOrInterCollision = ClothInspector.CollisionVisualizationMode.InterCollision;
    }

    public void DrawColorBox(Texture gradientTex, Color col)
    {
      if (!GUI.enabled)
      {
        col = new Color(0.3f, 0.3f, 0.3f, 1f);
        EditorGUI.showMixedValue = false;
      }
      GUILayout.BeginVertical();
      GUILayout.Space(5f);
      Rect position = GUILayoutUtility.GetRect(new GUIContent(), GUIStyle.none, new GUILayoutOption[2]{ GUILayout.ExpandWidth(true), GUILayout.Height(10f) });
      GUI.Box(position, GUIContent.none);
      position = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
      if ((bool) ((UnityEngine.Object) gradientTex))
        GUI.DrawTexture(position, gradientTex);
      else
        EditorGUIUtility.DrawColorSwatch(position, col, false);
      GUILayout.EndVertical();
    }

    private bool IsConstrained()
    {
      foreach (ClothSkinningCoefficient coefficient in this.cloth.coefficients)
      {
        if ((double) coefficient.maxDistance < 3.40282346638529E+38 || (double) coefficient.collisionSphereDistance < 3.40282346638529E+38)
          return true;
      }
      return false;
    }

    private void ConstraintEditing(UnityEngine.Object unused, SceneView sceneView)
    {
      GUILayout.BeginVertical(GUILayout.Width((float) ClothInspector.Styles.clothEditorWindowWidth));
      GUILayout.BeginHorizontal();
      GUILayout.Label("Visualization ", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.BeginVertical();
      if (EditorGUILayout.DropdownButton(this.GetDrawModeString(this.drawMode), FocusType.Passive, EditorStyles.toolbarDropDown, new GUILayoutOption[0]))
      {
        Rect last = GUILayoutUtility.topLevel.GetLast();
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(this.GetDrawModeString(ClothInspector.DrawMode.MaxDistance), this.drawMode == ClothInspector.DrawMode.MaxDistance, new GenericMenu.MenuFunction(this.VisualizationMenuSetMaxDistanceMode));
        genericMenu.AddItem(this.GetDrawModeString(ClothInspector.DrawMode.CollisionSphereDistance), this.drawMode == ClothInspector.DrawMode.CollisionSphereDistance, new GenericMenu.MenuFunction(this.VisualizationMenuSetCollisionSphereMode));
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Manipulate Backfaces"), this.state.ManipulateBackfaces, new GenericMenu.MenuFunction(this.VisualizationMenuToggleManipulateBackfaces));
        genericMenu.DropDown(last);
      }
      GUILayout.BeginHorizontal();
      GUILayout.Label(this.m_MinVisualizedValue[(int) this.drawMode].ToString(), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      this.DrawColorBox((Texture) ClothInspector.s_ColorTexture, Color.clear);
      GUILayout.Label(this.m_MaxVisualizedValue[(int) this.drawMode].ToString(), new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.Label("Unconstrained ");
      GUILayout.Space(-24f);
      GUILayout.BeginHorizontal(GUILayout.Width(20f));
      this.DrawColorBox((Texture) null, Color.black);
      GUILayout.EndHorizontal();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      if (Tools.current != Tool.None)
        this.state.ToolMode = ~ClothInspector.ToolMode.Select;
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.state.ToolMode = (ClothInspector.ToolMode) GUILayout.Toolbar((int) this.state.ToolMode, ClothInspector.Styles.toolIcons);
      using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
      {
        if (changeCheckScope.changed)
        {
          GUIUtility.keyboardControl = 0;
          SceneView.RepaintAll();
        }
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      switch (this.state.ToolMode)
      {
        case ClothInspector.ToolMode.Select:
          Tools.current = Tool.None;
          this.SelectionGUI();
          break;
        case ClothInspector.ToolMode.Paint:
          Tools.current = Tool.None;
          this.PaintGUI();
          break;
      }
      if (!this.IsConstrained())
        EditorGUILayout.HelpBox("No constraints have been set up, so the cloth will move freely. Set up vertex constraints here to restrict it.", MessageType.Info);
      GUILayout.EndVertical();
    }

    private void SelectManipulateBackFaces()
    {
      GUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(GUIContent.none, this.state.ManipulateBackfaces, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.state.ManipulateBackfaces = flag;
      EditorGUILayout.LabelField(ClothInspector.Styles.manipulateBackFaceString);
      GUILayout.EndHorizontal();
    }

    private void ResetParticleSelection()
    {
      int length = this.m_ParticleRectSelection.Length;
      for (int index = 0; index < length; ++index)
      {
        this.m_ParticleRectSelection[index] = false;
        this.m_ParticleSelection[index] = false;
      }
    }

    private void SelfAndInterCollisionEditing(UnityEngine.Object unused, SceneView sceneView)
    {
      GUILayout.BeginVertical(GUILayout.Width((float) ClothInspector.Styles.clothEditorWindowWidth));
      GUILayout.BeginHorizontal();
      GUILayout.Label("Visualization ", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.BeginVertical();
      if (EditorGUILayout.DropdownButton(this.GetCollVisModeString(this.state.VisualizeSelfOrInterCollision), FocusType.Passive, EditorStyles.toolbarDropDown, new GUILayoutOption[0]))
      {
        Rect last = GUILayoutUtility.topLevel.GetLast();
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(this.GetCollVisModeString(ClothInspector.CollisionVisualizationMode.SelfCollision), this.state.VisualizeSelfOrInterCollision == ClothInspector.CollisionVisualizationMode.SelfCollision, new GenericMenu.MenuFunction(this.VisualizationMenuSelfCollision));
        genericMenu.AddItem(this.GetCollVisModeString(ClothInspector.CollisionVisualizationMode.InterCollision), this.state.VisualizeSelfOrInterCollision == ClothInspector.CollisionVisualizationMode.InterCollision, new GenericMenu.MenuFunction(this.VisualizationMenuInterCollision));
        genericMenu.DropDown(last);
      }
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      if (Tools.current != Tool.None)
        this.state.ToolMode = ~ClothInspector.ToolMode.Select;
      ClothInspector.CollToolMode collToolMode = this.state.CollToolMode;
      this.state.CollToolMode = (ClothInspector.CollToolMode) GUILayout.Toolbar((int) this.state.CollToolMode, ClothInspector.Styles.collToolModeIcons);
      if (this.state.CollToolMode != collToolMode)
      {
        GUIUtility.keyboardControl = 0;
        SceneView.RepaintAll();
      }
      switch (this.state.CollToolMode)
      {
        case ClothInspector.CollToolMode.Select:
          Tools.current = Tool.None;
          this.CollSelectionGUI();
          break;
        case ClothInspector.CollToolMode.Paint:
        case ClothInspector.CollToolMode.Erase:
          Tools.current = Tool.None;
          this.ResetParticleSelection();
          this.EditBrushSize();
          break;
      }
      this.SelectManipulateBackFaces();
      int capacity = 0;
      int length = this.m_SelfAndInterCollisionSelection.Length;
      for (int index = 0; index < length; ++index)
      {
        if (this.m_SelfAndInterCollisionSelection[index])
          ++capacity;
      }
      if (capacity > 0)
      {
        List<uint> indices = new List<uint>(capacity);
        indices.Clear();
        for (uint index = 0; (long) index < (long) length; ++index)
        {
          if (this.m_SelfAndInterCollisionSelection[(IntPtr) index])
            indices.Add(index);
        }
        this.cloth.SetSelfAndInterCollisionIndices(indices);
      }
      GUILayout.EndVertical();
    }

    public enum DrawMode
    {
      MaxDistance = 1,
      CollisionSphereDistance = 2,
    }

    public enum ToolMode
    {
      Select,
      Paint,
    }

    public enum CollToolMode
    {
      Select,
      Paint,
      Erase,
    }

    private enum RectSelectionMode
    {
      Replace,
      Add,
      Substract,
    }

    public enum CollisionVisualizationMode
    {
      SelfCollision,
      InterCollision,
    }

    private static class Styles
    {
      public static readonly GUIContent editConstraintsLabel = EditorGUIUtility.TextContent("Edit Constraints");
      public static readonly GUIContent editSelfInterCollisionLabel = EditorGUIUtility.TextContent("Edit Collision Particles");
      public static readonly GUIContent selfInterCollisionParticleColor = EditorGUIUtility.TextContent("Visualization Color");
      public static readonly GUIContent selfInterCollisionBrushColor = EditorGUIUtility.TextContent("Brush Color");
      public static readonly GUIContent clothSelfCollisionAndInterCollision = EditorGUIUtility.TextContent("Cloth Self-Collision And Inter-Collision");
      public static readonly GUIContent paintCollisionParticles = EditorGUIUtility.TextContent("Paint Collision Particles");
      public static readonly GUIContent selectCollisionParticles = EditorGUIUtility.TextContent("Select Collision Particles");
      public static readonly GUIContent brushRadiusString = EditorGUIUtility.TextContent("Brush Radius");
      public static readonly GUIContent selfAndInterCollisionMode = EditorGUIUtility.TextContent("Paint or Select Particles");
      public static readonly GUIContent backFaceManipulationMode = EditorGUIUtility.TextContent("Back Face Manipulation");
      public static readonly GUIContent manipulateBackFaceString = EditorGUIUtility.TextContent("Manipulate Backfaces");
      public static readonly GUIContent selfCollisionString = EditorGUIUtility.TextContent("Self Collision");
      public static readonly GUIContent setSelfAndInterCollisionString = EditorGUIUtility.TextContent("Self-Collision and Inter-Collision");
      public static readonly int clothEditorWindowWidth = 300;
      public static GUIContent[] toolContents = new GUIContent[2]{ EditorGUIUtility.IconContent("EditCollider"), EditorGUIUtility.IconContent("EditCollider") };
      public static GUIContent[] toolIcons = new GUIContent[2]{ EditorGUIUtility.TextContent("Select"), EditorGUIUtility.TextContent("Paint") };
      public static GUIContent[] drawModeStrings = new GUIContent[3]{ EditorGUIUtility.TextContent("Fixed"), EditorGUIUtility.TextContent("Max Distance"), EditorGUIUtility.TextContent("Surface Penetration") };
      public static GUIContent[] toolModeStrings = new GUIContent[3]{ EditorGUIUtility.TextContent("Select"), EditorGUIUtility.TextContent("Paint"), EditorGUIUtility.TextContent("Erase") };
      public static GUIContent[] collToolModeIcons = new GUIContent[3]{ EditorGUIUtility.TextContent("Select"), EditorGUIUtility.TextContent("Paint"), EditorGUIUtility.TextContent("Erase") };
      public static GUIContent[] collVisModeStrings = new GUIContent[2]{ EditorGUIUtility.TextContent("Self Collision"), EditorGUIUtility.TextContent("Inter Collision") };
      public static GUIContent paintIcon = EditorGUIUtility.IconContent("ClothInspector.PaintValue", "Change this vertex coefficient value by painting in the scene view.");
      public static UnityEditorInternal.EditMode.SceneViewEditMode[] sceneViewEditModes = new UnityEditorInternal.EditMode.SceneViewEditMode[2]{ UnityEditorInternal.EditMode.SceneViewEditMode.ClothConstraints, UnityEditorInternal.EditMode.SceneViewEditMode.ClothSelfAndInterCollisionParticles };
      public static GUIContent selfCollisionDistanceGUIContent = EditorGUIUtility.TextContent("Self Collision Distance");
      public static GUIContent selfCollisionStiffnessGUIContent = EditorGUIUtility.TextContent("Self Collision Stiffness");

      static Styles()
      {
        ClothInspector.Styles.toolContents[0].tooltip = EditorGUIUtility.TextContent("Edit cloth constraints").text;
        ClothInspector.Styles.toolContents[1].tooltip = EditorGUIUtility.TextContent("Edit cloth self or inter collision").text;
        ClothInspector.Styles.toolIcons[0].tooltip = EditorGUIUtility.TextContent("Select cloth particles for use in self or inter collision").text;
        ClothInspector.Styles.toolIcons[1].tooltip = EditorGUIUtility.TextContent("Paint cloth particles for use in self or inter collision").text;
        ClothInspector.Styles.collToolModeIcons[0].tooltip = EditorGUIUtility.TextContent("Select cloth particles.").text;
        ClothInspector.Styles.collToolModeIcons[1].tooltip = EditorGUIUtility.TextContent("Paint cloth particles.").text;
        ClothInspector.Styles.collToolModeIcons[2].tooltip = EditorGUIUtility.TextContent("Erase cloth particles.").text;
      }
    }
  }
}
