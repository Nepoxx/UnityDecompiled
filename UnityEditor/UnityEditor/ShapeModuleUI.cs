// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShapeModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal class ShapeModuleUI : ModuleUI
  {
    private static readonly Matrix4x4 s_ArcHandleOffsetMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(90f, Vector3.right) * Quaternion.AngleAxis(90f, Vector3.up), Vector3.one);
    private static Color s_ShapeGizmoColor = new Color(0.5803922f, 0.8980392f, 1f, 0.9f);
    private static ShapeModuleUI.MultiModeTexts s_RadiusTexts = new ShapeModuleUI.MultiModeTexts("Radius|New particles are spawned along the radius.", "Mode|Control how particles are spawned along the radius.", "Spread|Spawn particles only at specific positions along the radius (0 to disable).", "Speed|Control the speed that the emission position moves along the radius.");
    private static ShapeModuleUI.MultiModeTexts s_ArcTexts = new ShapeModuleUI.MultiModeTexts("Arc|New particles are spawned around the arc.", "Mode|Control how particles are spawned around the arc.", "Spread|Spawn particles only at specific angles around the arc (0 to disable).", "Speed|Control the speed that the emission position moves around the arc.");
    private ArcHandle m_ArcHandle = new ArcHandle();
    private BoxBoundsHandle m_BoxBoundsHandle = new BoxBoundsHandle();
    private SphereBoundsHandle m_SphereBoundsHandle = new SphereBoundsHandle();
    private readonly ParticleSystemShapeType[] m_GuiTypes = new ParticleSystemShapeType[10]{ ParticleSystemShapeType.Sphere, ParticleSystemShapeType.Hemisphere, ParticleSystemShapeType.Cone, ParticleSystemShapeType.Donut, ParticleSystemShapeType.Box, ParticleSystemShapeType.Mesh, ParticleSystemShapeType.MeshRenderer, ParticleSystemShapeType.SkinnedMeshRenderer, ParticleSystemShapeType.Circle, ParticleSystemShapeType.SingleSidedEdge };
    private readonly int[] m_TypeToGuiTypeIndex = new int[18]{ 0, 0, 1, 1, 2, 4, 5, 2, 2, 2, 8, 8, 9, 6, 7, 4, 4, 3 };
    private readonly ParticleSystemShapeType[] boxShapes = new ParticleSystemShapeType[3]{ ParticleSystemShapeType.Box, ParticleSystemShapeType.BoxShell, ParticleSystemShapeType.BoxEdge };
    private readonly ParticleSystemShapeType[] coneShapes = new ParticleSystemShapeType[2]{ ParticleSystemShapeType.Cone, ParticleSystemShapeType.ConeVolume };
    private SerializedProperty m_Type;
    private SerializedProperty m_RandomDirectionAmount;
    private SerializedProperty m_SphericalDirectionAmount;
    private SerializedProperty m_RandomPositionAmount;
    private SerializedProperty m_Position;
    private SerializedProperty m_Scale;
    private SerializedProperty m_Rotation;
    private ShapeModuleUI.MultiModeParameter m_Radius;
    private SerializedProperty m_RadiusThickness;
    private SerializedProperty m_Angle;
    private SerializedProperty m_Length;
    private SerializedProperty m_BoxThickness;
    private ShapeModuleUI.MultiModeParameter m_Arc;
    private SerializedProperty m_DonutRadius;
    private SerializedProperty m_PlacementMode;
    private SerializedProperty m_Mesh;
    private SerializedProperty m_MeshRenderer;
    private SerializedProperty m_SkinnedMeshRenderer;
    private SerializedProperty m_MeshMaterialIndex;
    private SerializedProperty m_UseMeshMaterialIndex;
    private SerializedProperty m_UseMeshColors;
    private SerializedProperty m_MeshNormalOffset;
    private SerializedProperty m_AlignToDirection;
    private Material m_Material;
    private static ShapeModuleUI.Texts s_Texts;

    public ShapeModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ShapeModule", displayName, ModuleUI.VisibilityState.VisibleAndFolded)
    {
      this.m_ToolTip = "Shape of the emitter volume, which controls where particles are emitted and their initial direction.";
    }

    protected override void Init()
    {
      if (this.m_Type != null)
        return;
      if (ShapeModuleUI.s_Texts == null)
        ShapeModuleUI.s_Texts = new ShapeModuleUI.Texts();
      this.m_Type = this.GetProperty("type");
      this.m_Radius = ShapeModuleUI.MultiModeParameter.GetProperty((ModuleUI) this, "radius", ShapeModuleUI.s_RadiusTexts.speed);
      this.m_RadiusThickness = this.GetProperty("radiusThickness");
      this.m_Angle = this.GetProperty("angle");
      this.m_Length = this.GetProperty("length");
      this.m_BoxThickness = this.GetProperty("boxThickness");
      this.m_Arc = ShapeModuleUI.MultiModeParameter.GetProperty((ModuleUI) this, "arc", ShapeModuleUI.s_ArcTexts.speed);
      this.m_DonutRadius = this.GetProperty("donutRadius");
      this.m_PlacementMode = this.GetProperty("placementMode");
      this.m_Mesh = this.GetProperty("m_Mesh");
      this.m_MeshRenderer = this.GetProperty("m_MeshRenderer");
      this.m_SkinnedMeshRenderer = this.GetProperty("m_SkinnedMeshRenderer");
      this.m_MeshMaterialIndex = this.GetProperty("m_MeshMaterialIndex");
      this.m_UseMeshMaterialIndex = this.GetProperty("m_UseMeshMaterialIndex");
      this.m_UseMeshColors = this.GetProperty("m_UseMeshColors");
      this.m_MeshNormalOffset = this.GetProperty("m_MeshNormalOffset");
      this.m_RandomDirectionAmount = this.GetProperty("randomDirectionAmount");
      this.m_SphericalDirectionAmount = this.GetProperty("sphericalDirectionAmount");
      this.m_RandomPositionAmount = this.GetProperty("randomPositionAmount");
      this.m_AlignToDirection = this.GetProperty("alignToDirection");
      this.m_Position = this.GetProperty("m_Position");
      this.m_Scale = this.GetProperty("m_Scale");
      this.m_Rotation = this.GetProperty("m_Rotation");
      this.m_Material = EditorGUIUtility.GetBuiltinExtraResource(typeof (Material), "Default-Material.mat") as Material;
    }

    public override float GetXAxisScalar()
    {
      return this.m_ParticleSystemUI.GetEmitterDuration();
    }

    private ParticleSystemShapeType ConvertConeEmitFromToConeType(int emitFrom)
    {
      return this.coneShapes[emitFrom];
    }

    private int ConvertConeTypeToConeEmitFrom(ParticleSystemShapeType shapeType)
    {
      return Array.IndexOf<ParticleSystemShapeType>(this.coneShapes, shapeType);
    }

    private ParticleSystemShapeType ConvertBoxEmitFromToBoxType(int emitFrom)
    {
      return this.boxShapes[emitFrom];
    }

    private int ConvertBoxTypeToBoxEmitFrom(ParticleSystemShapeType shapeType)
    {
      return Array.IndexOf<ParticleSystemShapeType>(this.boxShapes, shapeType);
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.showMixedValue = this.m_Type.hasMultipleDifferentValues;
      int index1 = this.m_Type.intValue;
      int intValue = this.m_TypeToGuiTypeIndex[index1];
      EditorGUI.BeginChangeCheck();
      int index2 = ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.shape, intValue, ShapeModuleUI.s_Texts.shapeTypes);
      bool flag = EditorGUI.EndChangeCheck();
      EditorGUI.showMixedValue = false;
      ParticleSystemShapeType guiType = this.m_GuiTypes[index2];
      if (index2 != intValue)
        index1 = (int) guiType;
      if (!this.m_Type.hasMultipleDifferentValues)
      {
        switch (guiType)
        {
          case ParticleSystemShapeType.Sphere:
          case ParticleSystemShapeType.Hemisphere:
            double num1 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value);
            double num2 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness);
            break;
          case ParticleSystemShapeType.Cone:
            double num3 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.coneAngle, this.m_Angle);
            double num4 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value);
            double num5 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness);
            this.m_Arc.OnInspectorGUI(ShapeModuleUI.s_ArcTexts);
            using (new EditorGUI.DisabledScope(index1 != 8))
            {
              double num6 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.coneLength, this.m_Length);
            }
            int coneEmitFrom = this.ConvertConeTypeToConeEmitFrom((ParticleSystemShapeType) index1);
            index1 = (int) this.ConvertConeEmitFromToConeType(ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.emitFrom, coneEmitFrom, ShapeModuleUI.s_Texts.coneTypes));
            break;
          case ParticleSystemShapeType.Box:
            int boxEmitFrom = this.ConvertBoxTypeToBoxEmitFrom((ParticleSystemShapeType) index1);
            index1 = (int) this.ConvertBoxEmitFromToBoxType(ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.emitFrom, boxEmitFrom, ShapeModuleUI.s_Texts.boxTypes));
            switch (index1)
            {
              case 15:
              case 16:
                ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.boxThickness, this.m_BoxThickness);
                break;
            }
          case ParticleSystemShapeType.Mesh:
          case ParticleSystemShapeType.MeshRenderer:
          case ParticleSystemShapeType.SkinnedMeshRenderer:
            ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.meshType, this.m_PlacementMode, ShapeModuleUI.s_Texts.meshTypes);
            Material material = (Material) null;
            Mesh mesh = (Mesh) null;
            if (guiType == ParticleSystemShapeType.Mesh)
            {
              ModuleUI.GUIObject(ShapeModuleUI.s_Texts.mesh, this.m_Mesh);
              mesh = (Mesh) this.m_Mesh.objectReferenceValue;
            }
            else if (guiType == ParticleSystemShapeType.MeshRenderer)
            {
              ModuleUI.GUIObject(ShapeModuleUI.s_Texts.meshRenderer, this.m_MeshRenderer);
              MeshRenderer objectReferenceValue = (MeshRenderer) this.m_MeshRenderer.objectReferenceValue;
              if ((bool) ((UnityEngine.Object) objectReferenceValue))
              {
                material = objectReferenceValue.sharedMaterial;
                if ((bool) ((UnityEngine.Object) objectReferenceValue.GetComponent<MeshFilter>()))
                  mesh = objectReferenceValue.GetComponent<MeshFilter>().sharedMesh;
              }
            }
            else
            {
              ModuleUI.GUIObject(ShapeModuleUI.s_Texts.skinnedMeshRenderer, this.m_SkinnedMeshRenderer);
              SkinnedMeshRenderer objectReferenceValue = (SkinnedMeshRenderer) this.m_SkinnedMeshRenderer.objectReferenceValue;
              if ((bool) ((UnityEngine.Object) objectReferenceValue))
              {
                material = objectReferenceValue.sharedMaterial;
                mesh = objectReferenceValue.sharedMesh;
              }
            }
            ModuleUI.GUIToggleWithIntField(ShapeModuleUI.s_Texts.meshMaterialIndex, this.m_UseMeshMaterialIndex, this.m_MeshMaterialIndex, false);
            if (ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.useMeshColors, this.m_UseMeshColors) && (UnityEngine.Object) material != (UnityEngine.Object) null && (UnityEngine.Object) mesh != (UnityEngine.Object) null)
            {
              int id1 = Shader.PropertyToID("_Color");
              int id2 = Shader.PropertyToID("_TintColor");
              if (!material.HasProperty(id1) && !material.HasProperty(id2) && !mesh.HasChannel(Mesh.InternalShaderChannel.Color))
                EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("To use mesh colors, your source mesh must either provide vertex colors, or its shader must contain a color property named \"_Color\" or \"_TintColor\".").text, MessageType.Warning, true);
            }
            double num7 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.meshNormalOffset, this.m_MeshNormalOffset);
            break;
          case ParticleSystemShapeType.Circle:
            double num8 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value);
            double num9 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness);
            this.m_Arc.OnInspectorGUI(ShapeModuleUI.s_ArcTexts);
            break;
          case ParticleSystemShapeType.SingleSidedEdge:
            this.m_Radius.OnInspectorGUI(ShapeModuleUI.s_RadiusTexts);
            break;
          case ParticleSystemShapeType.Donut:
            double num10 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value);
            double num11 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.donutRadius, this.m_DonutRadius);
            double num12 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness);
            this.m_Arc.OnInspectorGUI(ShapeModuleUI.s_ArcTexts);
            break;
        }
      }
      if (flag || !this.m_Type.hasMultipleDifferentValues)
        this.m_Type.intValue = index1;
      EditorGUILayout.Space();
      ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.position, this.m_Position);
      ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.rotation, this.m_Rotation);
      ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.scale, this.m_Scale);
      EditorGUILayout.Space();
      ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.alignToDirection, this.m_AlignToDirection);
      double num13 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.randomDirectionAmount, this.m_RandomDirectionAmount);
      double num14 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.sphericalDirectionAmount, this.m_SphericalDirectionAmount);
      double num15 = (double) ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.randomPositionAmount, this.m_RandomPositionAmount);
    }

    public override void OnSceneViewGUI()
    {
      Color color = Handles.color;
      Handles.color = ShapeModuleUI.s_ShapeGizmoColor;
      Matrix4x4 matrix1 = Handles.matrix;
      EditorGUI.BeginChangeCheck();
      foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
      {
        ParticleSystem.ShapeModule shape = particleSystem.shape;
        ParticleSystem.MainModule main = particleSystem.main;
        ParticleSystemShapeType shapeType = shape.shapeType;
        Matrix4x4 matrix4x4_1 = new Matrix4x4();
        if (main.scalingMode == ParticleSystemScalingMode.Local)
          matrix4x4_1.SetTRS(particleSystem.transform.position, particleSystem.transform.rotation, particleSystem.transform.localScale);
        else if (main.scalingMode == ParticleSystemScalingMode.Hierarchy)
          matrix4x4_1 = particleSystem.transform.localToWorldMatrix;
        else
          matrix4x4_1.SetTRS(particleSystem.transform.position, particleSystem.transform.rotation, particleSystem.transform.lossyScale);
        Vector3 s = shapeType != ParticleSystemShapeType.Box && shapeType != ParticleSystemShapeType.BoxShell && shapeType != ParticleSystemShapeType.BoxEdge ? shape.scale : Vector3.one;
        Matrix4x4 matrix4x4_2 = Matrix4x4.TRS(shape.position, Quaternion.Euler(shape.rotation), s);
        Matrix4x4 matrix2 = matrix4x4_1 * matrix4x4_2;
        Handles.matrix = matrix2;
        switch (shapeType)
        {
          case ParticleSystemShapeType.Sphere:
            EditorGUI.BeginChangeCheck();
            float num1 = Handles.DoSimpleRadiusHandle(Quaternion.identity, Vector3.zero, shape.radius, false);
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Sphere Handle Change");
              shape.radius = num1;
              break;
            }
            break;
          case ParticleSystemShapeType.Hemisphere:
            EditorGUI.BeginChangeCheck();
            float num2 = Handles.DoSimpleRadiusHandle(Quaternion.identity, Vector3.zero, shape.radius, true);
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Hemisphere Handle Change");
              shape.radius = num2;
              break;
            }
            break;
          case ParticleSystemShapeType.Cone:
            EditorGUI.BeginChangeCheck();
            Vector3 radiusAngleRange1 = new Vector3(shape.radius, shape.angle, main.startSpeedMultiplier);
            radiusAngleRange1 = Handles.ConeFrustrumHandle(Quaternion.identity, Vector3.zero, radiusAngleRange1);
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Cone Handle Change");
              shape.radius = radiusAngleRange1.x;
              shape.angle = radiusAngleRange1.y;
              main.startSpeedMultiplier = radiusAngleRange1.z;
              break;
            }
            break;
          case ParticleSystemShapeType.Box:
          case ParticleSystemShapeType.BoxShell:
          case ParticleSystemShapeType.BoxEdge:
            EditorGUI.BeginChangeCheck();
            this.m_BoxBoundsHandle.center = Vector3.zero;
            this.m_BoxBoundsHandle.size = shape.scale;
            this.m_BoxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Box Handle Change");
              shape.scale = this.m_BoxBoundsHandle.size;
              break;
            }
            break;
          case ParticleSystemShapeType.Mesh:
            Mesh mesh = shape.mesh;
            if ((bool) ((UnityEngine.Object) mesh))
            {
              bool wireframe = GL.wireframe;
              GL.wireframe = true;
              this.m_Material.SetPass(0);
              Graphics.DrawMeshNow(mesh, matrix2);
              GL.wireframe = wireframe;
            }
            break;
          case ParticleSystemShapeType.ConeVolume:
            EditorGUI.BeginChangeCheck();
            Vector3 radiusAngleRange2 = new Vector3(shape.radius, shape.angle, shape.length);
            radiusAngleRange2 = Handles.ConeFrustrumHandle(Quaternion.identity, Vector3.zero, radiusAngleRange2);
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Cone Volume Handle Change");
              shape.radius = radiusAngleRange2.x;
              shape.angle = radiusAngleRange2.y;
              shape.length = radiusAngleRange2.z;
              break;
            }
            break;
          case ParticleSystemShapeType.Circle:
            EditorGUI.BeginChangeCheck();
            this.m_ArcHandle.radius = shape.radius;
            this.m_ArcHandle.angle = shape.arc;
            this.m_ArcHandle.SetColorWithRadiusHandle(Color.white, 0.0f);
            using (new Handles.DrawingScope(Handles.matrix * ShapeModuleUI.s_ArcHandleOffsetMatrix))
              this.m_ArcHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Circle Handle Change");
              shape.radius = this.m_ArcHandle.radius;
              shape.arc = this.m_ArcHandle.angle;
              break;
            }
            break;
          case ParticleSystemShapeType.SingleSidedEdge:
            EditorGUI.BeginChangeCheck();
            float num3 = Handles.DoSimpleEdgeHandle(Quaternion.identity, Vector3.zero, shape.radius);
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Edge Handle Change");
              shape.radius = num3;
              break;
            }
            break;
          case ParticleSystemShapeType.Donut:
            EditorGUI.BeginChangeCheck();
            this.m_ArcHandle.radius = shape.radius;
            this.m_ArcHandle.angle = shape.arc;
            this.m_ArcHandle.SetColorWithRadiusHandle(Color.white, 0.0f);
            this.m_ArcHandle.wireframeColor = Color.clear;
            using (new Handles.DrawingScope(Handles.matrix * ShapeModuleUI.s_ArcHandleOffsetMatrix))
              this.m_ArcHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject((UnityEngine.Object) particleSystem, "Donut Handle Change");
              shape.radius = this.m_ArcHandle.radius;
              shape.arc = this.m_ArcHandle.angle;
            }
            using (new Handles.DrawingScope(Handles.matrix * ShapeModuleUI.s_ArcHandleOffsetMatrix))
            {
              float num4 = shape.arc % 360f;
              float angle = (double) Mathf.Abs(shape.arc) < 360.0 ? num4 : 360f;
              Handles.DrawWireArc(new Vector3(0.0f, shape.donutRadius, 0.0f), Vector3.up, Vector3.forward, angle, shape.radius);
              Handles.DrawWireArc(new Vector3(0.0f, -shape.donutRadius, 0.0f), Vector3.up, Vector3.forward, angle, shape.radius);
              Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, angle, shape.radius + shape.donutRadius);
              Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, angle, shape.radius - shape.donutRadius);
              if ((double) shape.arc != 360.0)
              {
                Quaternion quaternion = Quaternion.AngleAxis(shape.arc, Vector3.up);
                Handles.DrawWireDisc(quaternion * Vector3.forward * shape.radius, quaternion * Vector3.right, shape.donutRadius);
              }
            }
            this.m_SphereBoundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
            this.m_SphereBoundsHandle.radius = shape.donutRadius;
            this.m_SphereBoundsHandle.center = Vector3.zero;
            this.m_SphereBoundsHandle.SetColor(Color.white);
            float num5 = 90f;
            int num6 = Mathf.Max(1, (int) Mathf.Ceil(shape.arc / num5));
            Matrix4x4 matrix4x4_3 = Matrix4x4.TRS(new Vector3(shape.radius, 0.0f, 0.0f), Quaternion.Euler(90f, 0.0f, 0.0f), Vector3.one);
            for (int index = 0; index < num6; ++index)
            {
              EditorGUI.BeginChangeCheck();
              using (new Handles.DrawingScope(Handles.matrix * (Matrix4x4.Rotate(Quaternion.Euler(0.0f, 0.0f, num5 * (float) index)) * matrix4x4_3)))
                this.m_SphereBoundsHandle.DrawHandle();
              if (EditorGUI.EndChangeCheck())
              {
                Undo.RecordObject((UnityEngine.Object) particleSystem, "Donut Radius Handle Change");
                shape.donutRadius = this.m_SphereBoundsHandle.radius;
              }
            }
            break;
        }
      }
      if (EditorGUI.EndChangeCheck())
        this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.Repaint();
      Handles.color = color;
      Handles.matrix = matrix1;
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      if (this.m_Arc.m_Mode.intValue == 0 && this.m_Radius.m_Mode.intValue == 0)
        return;
      text += "\n\tAnimated shape emission is enabled.";
    }

    private struct MultiModeParameter
    {
      public SerializedProperty m_Value;
      public SerializedProperty m_Mode;
      public SerializedProperty m_Spread;
      public SerializedMinMaxCurve m_Speed;

      public static ShapeModuleUI.MultiModeParameter GetProperty(ModuleUI ui, string name, GUIContent speed)
      {
        ShapeModuleUI.MultiModeParameter multiModeParameter = new ShapeModuleUI.MultiModeParameter();
        multiModeParameter.m_Value = ui.GetProperty(name + ".value");
        multiModeParameter.m_Mode = ui.GetProperty(name + ".mode");
        multiModeParameter.m_Spread = ui.GetProperty(name + ".spread");
        multiModeParameter.m_Speed = new SerializedMinMaxCurve(ui, speed, name + ".speed", ModuleUI.kUseSignedRange);
        multiModeParameter.m_Speed.m_AllowRandom = false;
        return multiModeParameter;
      }

      public void OnInspectorGUI(ShapeModuleUI.MultiModeTexts text)
      {
        double num1 = (double) ModuleUI.GUIFloat(text.value, this.m_Value);
        ++EditorGUI.indentLevel;
        ModuleUI.GUIPopup(text.mode, this.m_Mode, ShapeModuleUI.s_Texts.emissionModes);
        double num2 = (double) ModuleUI.GUIFloat(text.spread, this.m_Spread);
        if (!this.m_Mode.hasMultipleDifferentValues)
        {
          switch ((ShapeModuleUI.MultiModeParameter.ValueMode) this.m_Mode.intValue)
          {
            case ShapeModuleUI.MultiModeParameter.ValueMode.Loop:
            case ShapeModuleUI.MultiModeParameter.ValueMode.PingPong:
              ModuleUI.GUIMinMaxCurve(text.speed, this.m_Speed);
              break;
          }
        }
        --EditorGUI.indentLevel;
      }

      public enum ValueMode
      {
        Random,
        Loop,
        PingPong,
        BurstSpread,
      }
    }

    private class Texts
    {
      public GUIContent shape = EditorGUIUtility.TextContent("Shape|Defines the shape of the volume from which particles can be emitted, and the direction of the start velocity.");
      public GUIContent radius = EditorGUIUtility.TextContent("Radius|Radius of the shape.");
      public GUIContent radiusThickness = EditorGUIUtility.TextContent("Radius Thickness|Control the thickness of the spawn volume, from 0 to 1.");
      public GUIContent coneAngle = EditorGUIUtility.TextContent("Angle|Angle of the cone.");
      public GUIContent coneLength = EditorGUIUtility.TextContent("Length|Length of the cone.");
      public GUIContent boxThickness = EditorGUIUtility.TextContent("Box Thickness|When using shell/edge modes, control the thickness of the spawn volume, from 0 to 1.");
      public GUIContent meshType = EditorGUIUtility.TextContent("Type|Generate particles from vertices, edges or triangles.");
      public GUIContent mesh = EditorGUIUtility.TextContent("Mesh|Mesh that the particle system will emit from.");
      public GUIContent meshRenderer = EditorGUIUtility.TextContent("Mesh|MeshRenderer that the particle system will emit from.");
      public GUIContent skinnedMeshRenderer = EditorGUIUtility.TextContent("Mesh|SkinnedMeshRenderer that the particle system will emit from.");
      public GUIContent meshMaterialIndex = EditorGUIUtility.TextContent("Single Material|Only emit from a specific material of the mesh.");
      public GUIContent useMeshColors = EditorGUIUtility.TextContent("Use Mesh Colors|Modulate particle color with mesh vertex colors, or if they don't exist, use the shader color property \"_Color\" or \"_TintColor\" from the material. Does not read texture colors.");
      public GUIContent meshNormalOffset = EditorGUIUtility.TextContent("Normal Offset|Offset particle spawn positions along the mesh normal.");
      public GUIContent alignToDirection = EditorGUIUtility.TextContent("Align To Direction|Automatically align particles based on their initial direction of travel.");
      public GUIContent randomDirectionAmount = EditorGUIUtility.TextContent("Randomize Direction|Randomize the emission direction.");
      public GUIContent sphericalDirectionAmount = EditorGUIUtility.TextContent("Spherize Direction|Spherize the emission direction.");
      public GUIContent randomPositionAmount = EditorGUIUtility.TextContent("Randomize Position|Randomize the starting positions.");
      public GUIContent emitFrom = EditorGUIUtility.TextContent("Emit from:|Specifies from where particles are emitted.");
      public GUIContent donutRadius = EditorGUIUtility.TextContent("Donut Radius|The radius of the donut. Used to control the thickness of the ring.");
      public GUIContent position = EditorGUIUtility.TextContent("Position|Translate the emission shape.");
      public GUIContent rotation = EditorGUIUtility.TextContent("Rotation|Rotate the emission shape.");
      public GUIContent scale = EditorGUIUtility.TextContent("Scale|Scale the emission shape.");
      public GUIContent[] shapeTypes = new GUIContent[10]{ EditorGUIUtility.TextContent("Sphere"), EditorGUIUtility.TextContent("Hemisphere"), EditorGUIUtility.TextContent("Cone"), EditorGUIUtility.TextContent("Donut"), EditorGUIUtility.TextContent("Box"), EditorGUIUtility.TextContent("Mesh"), EditorGUIUtility.TextContent("Mesh Renderer"), EditorGUIUtility.TextContent("Skinned Mesh Renderer"), EditorGUIUtility.TextContent("Circle"), EditorGUIUtility.TextContent("Edge") };
      public GUIContent[] boxTypes = new GUIContent[3]{ EditorGUIUtility.TextContent("Volume"), EditorGUIUtility.TextContent("Shell"), EditorGUIUtility.TextContent("Edge") };
      public GUIContent[] coneTypes = new GUIContent[2]{ EditorGUIUtility.TextContent("Base"), EditorGUIUtility.TextContent("Volume") };
      public GUIContent[] meshTypes = new GUIContent[3]{ EditorGUIUtility.TextContent("Vertex"), EditorGUIUtility.TextContent("Edge"), EditorGUIUtility.TextContent("Triangle") };
      public GUIContent[] emissionModes = new GUIContent[4]{ EditorGUIUtility.TextContent("Random"), EditorGUIUtility.TextContent("Loop"), EditorGUIUtility.TextContent("Ping-Pong"), EditorGUIUtility.TextContent("Burst Spread") };
    }

    private class MultiModeTexts
    {
      public GUIContent value;
      public GUIContent mode;
      public GUIContent spread;
      public GUIContent speed;

      public MultiModeTexts(string _value, string _mode, string _spread, string _speed)
      {
        this.value = EditorGUIUtility.TextContent(_value);
        this.mode = EditorGUIUtility.TextContent(_mode);
        this.spread = EditorGUIUtility.TextContent(_spread);
        this.speed = EditorGUIUtility.TextContent(_speed);
      }
    }
  }
}
