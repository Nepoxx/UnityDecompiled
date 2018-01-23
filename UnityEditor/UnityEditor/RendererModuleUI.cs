// Decompiled with JetBrains decompiler
// Type: UnityEditor.RendererModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class RendererModuleUI : ModuleUI
  {
    private static bool s_VisualizePivot = false;
    private SerializedProperty[] m_Meshes = new SerializedProperty[4];
    private const int k_MaxNumMeshes = 4;
    private SerializedProperty m_CastShadows;
    private SerializedProperty m_ReceiveShadows;
    private SerializedProperty m_MotionVectors;
    private SerializedProperty m_Material;
    private SerializedProperty m_TrailMaterial;
    private SerializedProperty m_SortingOrder;
    private SerializedProperty m_SortingLayerID;
    private SerializedProperty m_RenderMode;
    private SerializedProperty[] m_ShownMeshes;
    private SerializedProperty m_MinParticleSize;
    private SerializedProperty m_MaxParticleSize;
    private SerializedProperty m_CameraVelocityScale;
    private SerializedProperty m_VelocityScale;
    private SerializedProperty m_LengthScale;
    private SerializedProperty m_SortMode;
    private SerializedProperty m_SortingFudge;
    private SerializedProperty m_NormalDirection;
    private RendererEditorBase.Probes m_Probes;
    private SerializedProperty m_RenderAlignment;
    private SerializedProperty m_Pivot;
    private SerializedProperty m_UseCustomVertexStreams;
    private SerializedProperty m_VertexStreams;
    private SerializedProperty m_MaskInteraction;
    private ReorderableList m_VertexStreamsList;
    private int m_NumTexCoords;
    private int m_TexCoordChannelIndex;
    private bool m_HasTangent;
    private bool m_HasColor;
    private static RendererModuleUI.Texts s_Texts;

    public RendererModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ParticleSystemRenderer", displayName, ModuleUI.VisibilityState.VisibleAndFolded)
    {
      this.m_ToolTip = "Specifies how the particles are rendered.";
    }

    protected override void Init()
    {
      if (this.m_CastShadows != null)
        return;
      if (RendererModuleUI.s_Texts == null)
        RendererModuleUI.s_Texts = new RendererModuleUI.Texts();
      this.m_CastShadows = this.GetProperty0("m_CastShadows");
      this.m_ReceiveShadows = this.GetProperty0("m_ReceiveShadows");
      this.m_MotionVectors = this.GetProperty0("m_MotionVectors");
      this.m_Material = this.GetProperty0("m_Materials.Array.data[0]");
      this.m_TrailMaterial = this.GetProperty0("m_Materials.Array.data[1]");
      this.m_SortingOrder = this.GetProperty0("m_SortingOrder");
      this.m_SortingLayerID = this.GetProperty0("m_SortingLayerID");
      this.m_RenderMode = this.GetProperty0("m_RenderMode");
      this.m_MinParticleSize = this.GetProperty0("m_MinParticleSize");
      this.m_MaxParticleSize = this.GetProperty0("m_MaxParticleSize");
      this.m_CameraVelocityScale = this.GetProperty0("m_CameraVelocityScale");
      this.m_VelocityScale = this.GetProperty0("m_VelocityScale");
      this.m_LengthScale = this.GetProperty0("m_LengthScale");
      this.m_SortingFudge = this.GetProperty0("m_SortingFudge");
      this.m_SortMode = this.GetProperty0("m_SortMode");
      this.m_NormalDirection = this.GetProperty0("m_NormalDirection");
      this.m_Probes = new RendererEditorBase.Probes();
      this.m_Probes.Initialize(this.serializedObject);
      this.m_RenderAlignment = this.GetProperty0("m_RenderAlignment");
      this.m_Pivot = this.GetProperty0("m_Pivot");
      this.m_Meshes[0] = this.GetProperty0("m_Mesh");
      this.m_Meshes[1] = this.GetProperty0("m_Mesh1");
      this.m_Meshes[2] = this.GetProperty0("m_Mesh2");
      this.m_Meshes[3] = this.GetProperty0("m_Mesh3");
      List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
      for (int index = 0; index < this.m_Meshes.Length; ++index)
      {
        if (index == 0 || this.m_Meshes[index].objectReferenceValue != (UnityEngine.Object) null)
          serializedPropertyList.Add(this.m_Meshes[index]);
      }
      this.m_ShownMeshes = serializedPropertyList.ToArray();
      this.m_MaskInteraction = this.GetProperty0("m_MaskInteraction");
      this.m_UseCustomVertexStreams = this.GetProperty0("m_UseCustomVertexStreams");
      this.m_VertexStreams = this.GetProperty0("m_VertexStreams");
      this.m_VertexStreamsList = new ReorderableList(this.serializedObject, this.m_VertexStreams, true, true, true, true);
      this.m_VertexStreamsList.elementHeight = 16f;
      this.m_VertexStreamsList.headerHeight = 0.0f;
      this.m_VertexStreamsList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.OnVertexStreamListAddDropdownCallback);
      this.m_VertexStreamsList.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.OnVertexStreamListCanRemoveCallback);
      this.m_VertexStreamsList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawVertexStreamListElementCallback);
      RendererModuleUI.s_VisualizePivot = EditorPrefs.GetBool("VisualizePivot", false);
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      RendererModuleUI.RenderMode renderMode = (RendererModuleUI.RenderMode) ModuleUI.GUIPopup(RendererModuleUI.s_Texts.renderMode, this.m_RenderMode, RendererModuleUI.s_Texts.particleTypes);
      bool flag = EditorGUI.EndChangeCheck();
      if (!this.m_RenderMode.hasMultipleDifferentValues)
      {
        if (renderMode == RendererModuleUI.RenderMode.Mesh)
        {
          ++EditorGUI.indentLevel;
          this.DoListOfMeshesGUI();
          --EditorGUI.indentLevel;
          if (flag && this.m_Meshes[0].objectReferenceInstanceIDValue == 0 && !this.m_Meshes[0].hasMultipleDifferentValues)
            this.m_Meshes[0].objectReferenceValue = UnityEngine.Resources.GetBuiltinResource(typeof (Mesh), "Cube.fbx");
        }
        else if (renderMode == RendererModuleUI.RenderMode.Stretch3D)
        {
          ++EditorGUI.indentLevel;
          double num1 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.cameraSpeedScale, this.m_CameraVelocityScale);
          double num2 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.speedScale, this.m_VelocityScale);
          double num3 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.lengthScale, this.m_LengthScale);
          --EditorGUI.indentLevel;
        }
        if (renderMode != RendererModuleUI.RenderMode.None)
        {
          if (renderMode != RendererModuleUI.RenderMode.Mesh)
          {
            double num = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.normalDirection, this.m_NormalDirection);
          }
          if (this.m_Material != null)
            ModuleUI.GUIObject(RendererModuleUI.s_Texts.material, this.m_Material);
        }
      }
      if (this.m_TrailMaterial != null)
        ModuleUI.GUIObject(RendererModuleUI.s_Texts.trailMaterial, this.m_TrailMaterial);
      if (renderMode != RendererModuleUI.RenderMode.None)
      {
        if (!this.m_RenderMode.hasMultipleDifferentValues)
        {
          ModuleUI.GUIPopup(RendererModuleUI.s_Texts.sortMode, this.m_SortMode, RendererModuleUI.s_Texts.sortTypes);
          double num1 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.sortingFudge, this.m_SortingFudge);
          if (renderMode != RendererModuleUI.RenderMode.Mesh)
          {
            double num2 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.minParticleSize, this.m_MinParticleSize);
            double num3 = (double) ModuleUI.GUIFloat(RendererModuleUI.s_Texts.maxParticleSize, this.m_MaxParticleSize);
          }
          if (renderMode == RendererModuleUI.RenderMode.Billboard || renderMode == RendererModuleUI.RenderMode.Mesh)
          {
            if ((UnityEngine.Object) ((IEnumerable<ParticleSystem>) this.m_ParticleSystemUI.m_ParticleSystems).FirstOrDefault<ParticleSystem>((Func<ParticleSystem, bool>) (o => o.shape.alignToDirection)) != (UnityEngine.Object) null)
            {
              using (new EditorGUI.DisabledScope(true))
                ModuleUI.GUIPopup(RendererModuleUI.s_Texts.space, 0, RendererModuleUI.s_Texts.localSpace);
              EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Using Align to Direction in the Shape Module forces the system to be rendered using Local Render Alignment.").text, MessageType.Info, true);
            }
            else
              ModuleUI.GUIPopup(RendererModuleUI.s_Texts.space, this.m_RenderAlignment, RendererModuleUI.s_Texts.spaces);
          }
          ModuleUI.GUIVector3Field(RendererModuleUI.s_Texts.pivot, this.m_Pivot);
          EditorGUI.BeginChangeCheck();
          RendererModuleUI.s_VisualizePivot = ModuleUI.GUIToggle(RendererModuleUI.s_Texts.visualizePivot, RendererModuleUI.s_VisualizePivot);
          if (EditorGUI.EndChangeCheck())
            EditorPrefs.SetBool("VisualizePivot", RendererModuleUI.s_VisualizePivot);
        }
        ModuleUI.GUIPopup(RendererModuleUI.s_Texts.maskingMode, this.m_MaskInteraction, RendererModuleUI.s_Texts.maskInteractions);
        if (!this.m_RenderMode.hasMultipleDifferentValues)
        {
          if (ModuleUI.GUIToggle(RendererModuleUI.s_Texts.useCustomVertexStreams, this.m_UseCustomVertexStreams))
            this.DoVertexStreamsGUI(renderMode);
          EditorGUILayout.Space();
          ModuleUI.GUIPopup(RendererModuleUI.s_Texts.castShadows, this.m_CastShadows, EditorGUIUtility.TempContent(this.m_CastShadows.enumDisplayNames));
          using (new EditorGUI.DisabledScope(SceneView.IsUsingDeferredRenderingPath()))
            ModuleUI.GUIToggle(RendererModuleUI.s_Texts.receiveShadows, this.m_ReceiveShadows);
          ModuleUI.GUIPopup(RendererModuleUI.s_Texts.motionVectors, this.m_MotionVectors, RendererModuleUI.s_Texts.motionVectorOptions);
          EditorGUILayout.SortingLayerField(RendererModuleUI.s_Texts.sortingLayer, this.m_SortingLayerID, ParticleSystemStyles.Get().popup, ParticleSystemStyles.Get().label);
          ModuleUI.GUIInt(RendererModuleUI.s_Texts.sortingOrder, this.m_SortingOrder);
        }
      }
      List<ParticleSystemRenderer> source = new List<ParticleSystemRenderer>();
      foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
        source.Add(particleSystem.GetComponent<ParticleSystemRenderer>());
      this.m_Probes.OnGUI((UnityEngine.Object[]) source.ToArray(), (Renderer) source.FirstOrDefault<ParticleSystemRenderer>(), true);
    }

    private void DoListOfMeshesGUI()
    {
      this.GUIListOfFloatObjectToggleFields(RendererModuleUI.s_Texts.mesh, this.m_ShownMeshes, (EditorGUI.ObjectFieldValidator) null, (GUIContent) null, false);
      Rect rect = GUILayoutUtility.GetRect(0.0f, 13f);
      rect.x = (float) ((double) rect.xMax - 24.0 - 5.0);
      rect.width = 12f;
      if (this.m_ShownMeshes.Length > 1 && ModuleUI.MinusButton(rect))
      {
        this.m_ShownMeshes[this.m_ShownMeshes.Length - 1].objectReferenceValue = (UnityEngine.Object) null;
        List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownMeshes);
        serializedPropertyList.RemoveAt(serializedPropertyList.Count - 1);
        this.m_ShownMeshes = serializedPropertyList.ToArray();
      }
      if (this.m_ShownMeshes.Length >= 4 || this.m_ParticleSystemUI.multiEdit)
        return;
      rect.x += 17f;
      if (ModuleUI.PlusButton(rect))
      {
        List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownMeshes);
        serializedPropertyList.Add(this.m_Meshes[serializedPropertyList.Count]);
        this.m_ShownMeshes = serializedPropertyList.ToArray();
      }
    }

    private void SelectVertexStreamCallback(object obj)
    {
      RendererModuleUI.StreamCallbackData streamCallbackData = (RendererModuleUI.StreamCallbackData) obj;
      ReorderableList.defaultBehaviours.DoAddButton(streamCallbackData.list);
      streamCallbackData.streamProp.GetArrayElementAtIndex(streamCallbackData.list.index).intValue = streamCallbackData.stream;
      this.m_ParticleSystemUI.m_RendererSerializedObject.ApplyModifiedProperties();
    }

    private void DoVertexStreamsGUI(RendererModuleUI.RenderMode renderMode)
    {
      this.m_NumTexCoords = 0;
      this.m_TexCoordChannelIndex = 0;
      this.m_HasTangent = false;
      this.m_HasColor = false;
      this.m_VertexStreamsList.DoLayoutList();
      if (this.m_ParticleSystemUI.multiEdit)
        return;
      string textAndTooltip = "";
      if (this.m_Material != null)
      {
        Material objectReferenceValue = this.m_Material.objectReferenceValue as Material;
        int texCoordChannelCount = this.m_NumTexCoords * 4 + this.m_TexCoordChannelIndex;
        bool tangentError = false;
        bool colorError = false;
        bool uvError = false;
        if (this.m_ParticleSystemUI.m_ParticleSystems[0].CheckVertexStreamsMatchShader(this.m_HasTangent, this.m_HasColor, texCoordChannelCount, objectReferenceValue, ref tangentError, ref colorError, ref uvError))
        {
          textAndTooltip += "Vertex streams do not match the shader inputs. Particle systems may not render correctly. Ensure your streams match and are used by the shader.";
          if (tangentError)
            textAndTooltip += "\n- TANGENT stream does not match.";
          if (colorError)
            textAndTooltip += "\n- COLOR stream does not match.";
          if (uvError)
            textAndTooltip += "\n- TEXCOORD streams do not match.";
        }
      }
      int maxTexCoordStreams = this.m_ParticleSystemUI.m_ParticleSystems[0].GetMaxTexCoordStreams();
      if (this.m_NumTexCoords > maxTexCoordStreams || this.m_NumTexCoords == maxTexCoordStreams && this.m_TexCoordChannelIndex > 0)
      {
        if (textAndTooltip != "")
          textAndTooltip += "\n\n";
        textAndTooltip = textAndTooltip + "Only " + (object) maxTexCoordStreams + " TEXCOORD streams are supported.";
      }
      if (renderMode == RendererModuleUI.RenderMode.Mesh)
      {
        ParticleSystemRenderer component = this.m_ParticleSystemUI.m_ParticleSystems[0].GetComponent<ParticleSystemRenderer>();
        Mesh[] meshes1 = new Mesh[4];
        int meshes2 = component.GetMeshes(meshes1);
        for (int index = 0; index < meshes2; ++index)
        {
          if (meshes1[index].HasChannel(Mesh.InternalShaderChannel.TexCoord2))
          {
            if (textAndTooltip != "")
              textAndTooltip += "\n\n";
            textAndTooltip += "Meshes may only use a maximum of 2 input UV streams.";
          }
        }
      }
      if (textAndTooltip != "")
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent(textAndTooltip).text, MessageType.Error, true);
    }

    private void OnVertexStreamListAddDropdownCallback(Rect rect, ReorderableList list)
    {
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < RendererModuleUI.s_Texts.vertexStreamsPacked.Length; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < this.m_VertexStreams.arraySize; ++index2)
        {
          if (this.m_VertexStreams.GetArrayElementAtIndex(index2).intValue == index1)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          intList.Add(index1);
      }
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < intList.Count; ++index)
        genericMenu.AddItem(RendererModuleUI.s_Texts.vertexStreamsMenuContent[intList[index]], false, new GenericMenu.MenuFunction2(this.SelectVertexStreamCallback), (object) new RendererModuleUI.StreamCallbackData(list, this.m_VertexStreams, intList[index]));
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private bool OnVertexStreamListCanRemoveCallback(ReorderableList list)
    {
      SerializedProperty arrayElementAtIndex = this.m_VertexStreams.GetArrayElementAtIndex(list.index);
      return RendererModuleUI.s_Texts.vertexStreamsPacked[arrayElementAtIndex.intValue] != "Position";
    }

    private void DrawVertexStreamListElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
      int intValue = this.m_VertexStreams.GetArrayElementAtIndex(index).intValue;
      string str1 = !this.isWindowView ? "TEXCOORD" : "TEX";
      int streamTexCoordChannel = RendererModuleUI.s_Texts.vertexStreamTexCoordChannels[intValue];
      if (streamTexCoordChannel != 0)
      {
        int length = this.m_TexCoordChannelIndex + streamTexCoordChannel <= 4 ? streamTexCoordChannel : streamTexCoordChannel + 1;
        string str2 = RendererModuleUI.s_Texts.channels.Substring(this.m_TexCoordChannelIndex, length);
        GUI.Label(rect, RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] + " (" + str1 + (object) this.m_NumTexCoords + "." + str2 + ")", ParticleSystemStyles.Get().label);
        this.m_TexCoordChannelIndex += streamTexCoordChannel;
        if (this.m_TexCoordChannelIndex < 4)
          return;
        this.m_TexCoordChannelIndex -= 4;
        ++this.m_NumTexCoords;
      }
      else
      {
        GUI.Label(rect, RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] + " (" + RendererModuleUI.s_Texts.vertexStreamPackedTypes[intValue] + ")", ParticleSystemStyles.Get().label);
        if (RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] == "Tangent")
          this.m_HasTangent = true;
        if (RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] == "Color")
          this.m_HasColor = true;
      }
    }

    public override void OnSceneViewGUI()
    {
      if (!RendererModuleUI.s_VisualizePivot)
        return;
      Color color = Handles.color;
      Handles.color = Color.green;
      Matrix4x4 matrix = Handles.matrix;
      Vector3[] lineSegments = new Vector3[6];
      foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
      {
        ParticleSystem.Particle[] particles1 = new ParticleSystem.Particle[particleSystem.particleCount];
        int particles2 = particleSystem.GetParticles(particles1);
        Matrix4x4 matrix4x4 = Matrix4x4.identity;
        if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local)
          matrix4x4 = particleSystem.GetLocalToWorldMatrix();
        Handles.matrix = matrix4x4;
        for (int index = 0; index < particles2; ++index)
        {
          ParticleSystem.Particle particle = particles1[index];
          Vector3 vector3 = particle.GetCurrentSize3D(particleSystem) * 0.05f;
          lineSegments[0] = particle.position - Vector3.right * vector3.x;
          lineSegments[1] = particle.position + Vector3.right * vector3.x;
          lineSegments[2] = particle.position - Vector3.up * vector3.y;
          lineSegments[3] = particle.position + Vector3.up * vector3.y;
          lineSegments[4] = particle.position - Vector3.forward * vector3.z;
          lineSegments[5] = particle.position + Vector3.forward * vector3.z;
          Handles.DrawLines(lineSegments);
        }
      }
      Handles.color = color;
      Handles.matrix = matrix;
    }

    private enum RenderMode
    {
      Billboard,
      Stretch3D,
      BillboardFixedHorizontal,
      BillboardFixedVertical,
      Mesh,
      None,
    }

    private class Texts
    {
      public GUIContent renderMode = EditorGUIUtility.TextContent("Render Mode|Defines the render mode of the particle renderer.");
      public GUIContent material = EditorGUIUtility.TextContent("Material|Defines the material used to render particles.");
      public GUIContent trailMaterial = EditorGUIUtility.TextContent("Trail Material|Defines the material used to render particle trails.");
      public GUIContent mesh = EditorGUIUtility.TextContent("Mesh|Defines the mesh that will be rendered as particle.");
      public GUIContent minParticleSize = EditorGUIUtility.TextContent("Min Particle Size|How small is a particle allowed to be on screen at least? 1 is entire viewport. 0.5 is half viewport.");
      public GUIContent maxParticleSize = EditorGUIUtility.TextContent("Max Particle Size|How large is a particle allowed to be on screen at most? 1 is entire viewport. 0.5 is half viewport.");
      public GUIContent cameraSpeedScale = EditorGUIUtility.TextContent("Camera Scale|How much the camera speed is factored in when determining particle stretching.");
      public GUIContent speedScale = EditorGUIUtility.TextContent("Speed Scale|Defines the length of the particle compared to its speed.");
      public GUIContent lengthScale = EditorGUIUtility.TextContent("Length Scale|Defines the length of the particle compared to its width.");
      public GUIContent sortingFudge = EditorGUIUtility.TextContent("Sorting Fudge|Lower the number and most likely these particles will appear in front of other transparent objects, including other particles.");
      public GUIContent sortMode = EditorGUIUtility.TextContent("Sort Mode|The draw order of particles can be sorted by distance, oldest in front, or youngest in front.");
      public GUIContent rotation = EditorGUIUtility.TextContent("Rotation|Set whether the rotation of the particles is defined in Screen or World space.");
      public GUIContent castShadows = EditorGUIUtility.TextContent("Cast Shadows|Only opaque materials cast shadows");
      public GUIContent receiveShadows = EditorGUIUtility.TextContent("Receive Shadows|Only opaque materials receive shadows");
      public GUIContent motionVectors = EditorGUIUtility.TextContent("Motion Vectors|Specifies whether the Particle System renders 'Per Object Motion', 'Camera Motion', or 'No Motion' vectors to the Camera Motion Vector Texture. Note that there is no built-in support for Per-Particle Motion.");
      public GUIContent normalDirection = EditorGUIUtility.TextContent("Normal Direction|Value between 0.0 and 1.0. If 1.0 is used, normals will point towards camera. If 0.0 is used, normals will point out in the corner direction of the particle.");
      public GUIContent sortingLayer = EditorGUIUtility.TextContent("Sorting Layer|Name of the Renderer's sorting layer.");
      public GUIContent sortingOrder = EditorGUIUtility.TextContent("Order in Layer|Renderer's order within a sorting layer");
      public GUIContent space = EditorGUIUtility.TextContent("Render Alignment|Specifies if the particles will face the camera, align to world axes, or stay local to the system's transform.");
      public GUIContent pivot = EditorGUIUtility.TextContent("Pivot|Applies an offset to the pivot of particles, as a multiplier of its size.");
      public GUIContent visualizePivot = EditorGUIUtility.TextContent("Visualize Pivot|Render the pivot positions of the particles.");
      public GUIContent useCustomVertexStreams = EditorGUIUtility.TextContent("Custom Vertex Streams|Choose whether to send custom particle data to the shader.");
      public GUIContent[] particleTypes = new GUIContent[6]{ EditorGUIUtility.TextContent("Billboard"), EditorGUIUtility.TextContent("Stretched Billboard"), EditorGUIUtility.TextContent("Horizontal Billboard"), EditorGUIUtility.TextContent("Vertical Billboard"), EditorGUIUtility.TextContent("Mesh"), EditorGUIUtility.TextContent("None") };
      public GUIContent[] sortTypes = new GUIContent[4]{ EditorGUIUtility.TextContent("None"), EditorGUIUtility.TextContent("By Distance"), EditorGUIUtility.TextContent("Oldest in Front"), EditorGUIUtility.TextContent("Youngest in Front") };
      public GUIContent[] spaces = new GUIContent[5]{ EditorGUIUtility.TextContent("View"), EditorGUIUtility.TextContent("World"), EditorGUIUtility.TextContent("Local"), EditorGUIUtility.TextContent("Facing"), EditorGUIUtility.TextContent("Velocity") };
      public GUIContent[] localSpace = new GUIContent[1]{ EditorGUIUtility.TextContent("Local") };
      public GUIContent[] motionVectorOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Camera Motion Only"), EditorGUIUtility.TextContent("Per Object Motion"), EditorGUIUtility.TextContent("Force No Motion") };
      public GUIContent maskingMode = EditorGUIUtility.TextContent("Masking|Defines the masking behavior of the particles. See Sprite Masking documentation for more details.");
      public GUIContent[] maskInteractions = new GUIContent[3]{ EditorGUIUtility.TextContent("No Masking"), EditorGUIUtility.TextContent("Visible Inside Mask"), EditorGUIUtility.TextContent("Visible Outside Mask") };
      private string[] vertexStreamsMenu = new string[45]{ "Position", "Normal", "Tangent", "Color", "UV/UV1", "UV/UV2", "UV/UV3", "UV/UV4", "UV/AnimBlend", "UV/AnimFrame", "Center", "VertexID", "Size/Size.x", "Size/Size.xy", "Size/Size.xyz", "Rotation/Rotation", "Rotation/Rotation3D", "Rotation/RotationSpeed", "Rotation/RotationSpeed3D", "Velocity", "Speed", "Lifetime/AgePercent", "Lifetime/InverseStartLifetime", "Random/Stable.x", "Random/Stable.xy", "Random/Stable.xyz", "Random/Stable.xyzw", "Random/Varying.x", "Random/Varying.xy", "Random/Varying.xyz", "Random/Varying.xyzw", "Custom/Custom1.x", "Custom/Custom1.xy", "Custom/Custom1.xyz", "Custom/Custom1.xyzw", "Custom/Custom2.x", "Custom/Custom2.xy", "Custom/Custom2.xyz", "Custom/Custom2.xyzw", "Noise/Sum.x", "Noise/Sum.xy", "Noise/Sum.xyz", "Noise/Impulse.x", "Noise/Impulse.xy", "Noise/Impulse.xyz" };
      public string[] vertexStreamsPacked = new string[45]{ "Position", "Normal", "Tangent", "Color", "UV", "UV2", "UV3", "UV4", "AnimBlend", "AnimFrame", "Center", "VertexID", "Size", "Size.xy", "Size.xyz", "Rotation", "Rotation3D", "RotationSpeed", "RotationSpeed3D", "Velocity", "Speed", "AgePercent", "InverseStartLifetime", "StableRandom.x", "StableRandom.xy", "StableRandom.xyz", "StableRandom.xyzw", "VariableRandom.x", "VariableRandom.xy", "VariableRandom.xyz", "VariableRandom.xyzw", "Custom1.x", "Custom1.xy", "Custom1.xyz", "Custom1.xyzw", "Custom2.x", "Custom2.xy", "Custom2.xyz", "Custom2.xyzw", "NoiseSum.x", "NoiseSum.xy", "NoiseSum.xyz", "NoiseImpulse.x", "NoiseImpulse.xy", "NoiseImpulse.xyz" };
      public string[] vertexStreamPackedTypes = new string[4]{ "POSITION.xyz", "NORMAL.xyz", "TANGENT.xyzw", "COLOR.xyzw" };
      public int[] vertexStreamTexCoordChannels = new int[45]{ 0, 0, 0, 0, 2, 2, 2, 2, 1, 1, 3, 1, 1, 2, 3, 1, 3, 1, 3, 3, 1, 1, 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 1, 2, 3 };
      public string channels = "xyzw|xyz";
      public GUIContent[] vertexStreamsMenuContent;

      public Texts()
      {
        this.vertexStreamsMenuContent = ((IEnumerable<string>) this.vertexStreamsMenu).Select<string, GUIContent>((Func<string, GUIContent>) (x => new GUIContent(x))).ToArray<GUIContent>();
      }
    }

    private class StreamCallbackData
    {
      public ReorderableList list;
      public SerializedProperty streamProp;
      public int stream;

      public StreamCallbackData(ReorderableList l, SerializedProperty prop, int s)
      {
        this.list = l;
        this.streamProp = prop;
        this.stream = s;
      }
    }
  }
}
