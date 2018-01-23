// Decompiled with JetBrains decompiler
// Type: UnityEditor.FrameDebuggerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class FrameDebuggerWindow : EditorWindow
  {
    public static readonly string[] s_FrameEventTypeNames = new string[22]{ "Clear (nothing)", "Clear (color)", "Clear (Z)", "Clear (color+Z)", "Clear (stencil)", "Clear (color+stencil)", "Clear (Z+stencil)", "Clear (color+Z+stencil)", "SetRenderTarget", "Resolve Color", "Resolve Depth", "Grab RenderTexture", "Static Batch", "Dynamic Batch", "Draw Mesh", "Draw Dynamic", "Draw GL", "GPU Skinning", "Draw Procedural", "Compute Shader", "Plugin Event", "Draw Mesh (instanced)" };
    private static List<FrameDebuggerWindow> s_FrameDebuggers = new List<FrameDebuggerWindow>();
    [SerializeField]
    private float m_ListWidth = 300f;
    private int m_RepaintFrames = 4;
    public Vector2 m_PreviewDir = new Vector2(120f, -20f);
    [NonSerialized]
    private float m_RTWhiteLevel = 1f;
    private int m_PrevEventsLimit = 0;
    private int m_PrevEventsCount = 0;
    private uint m_CurEventDataHash = 0;
    private Vector2 m_ScrollViewShaderProps = Vector2.zero;
    private ShowAdditionalInfo m_AdditionalInfo = ShowAdditionalInfo.ShaderProperties;
    private GUIContent[] m_AdditionalInfoGuiContents = ((IEnumerable<string>) Enum.GetNames(typeof (ShowAdditionalInfo))).Select<string, GUIContent>((Func<string, GUIContent>) (m => new GUIContent(m))).ToArray<GUIContent>();
    private AttachProfilerUI m_AttachProfilerUI = new AttachProfilerUI();
    private const float kScrollbarWidth = 16f;
    private const float kResizerWidth = 5f;
    private const float kMinListWidth = 200f;
    private const float kMinDetailsWidth = 200f;
    private const float kMinWindowWidth = 240f;
    private const float kDetailsMargin = 4f;
    private const float kMinPreviewSize = 64f;
    private const string kFloatFormat = "g2";
    private const string kFloatDetailedFormat = "g7";
    private const float kShaderPropertiesIndention = 15f;
    private const float kNameFieldWidth = 200f;
    private const float kValueFieldWidth = 200f;
    private const float kArrayValuePopupBtnWidth = 25f;
    private const int kShaderTypeBits = 6;
    private const int kArraySizeBitMask = 1023;
    private const int kNeedToRepaintFrames = 4;
    private PreviewRenderUtility m_PreviewUtility;
    private Material m_Material;
    private Material m_WireMaterial;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    [NonSerialized]
    private FrameDebuggerTreeView m_Tree;
    [NonSerialized]
    private int m_FrameEventsHash;
    [NonSerialized]
    private int m_RTIndex;
    [NonSerialized]
    private int m_RTChannel;
    [NonSerialized]
    private float m_RTBlackLevel;
    private FrameDebuggerEventData m_CurEventData;
    private FrameDebuggerWindow.EventDataStrings m_CurEventDataStrings;
    private static FrameDebuggerWindow.Styles ms_Styles;

    public FrameDebuggerWindow()
    {
      this.position = new Rect(50f, 50f, 600f, 350f);
      this.minSize = new Vector2(400f, 200f);
    }

    [MenuItem("Window/Frame Debugger", false, 2100)]
    public static FrameDebuggerWindow ShowFrameDebuggerWindow()
    {
      FrameDebuggerWindow window = EditorWindow.GetWindow(typeof (FrameDebuggerWindow)) as FrameDebuggerWindow;
      if ((UnityEngine.Object) window != (UnityEngine.Object) null)
        window.titleContent = EditorGUIUtility.TextContent("Frame Debug");
      return window;
    }

    internal static void RepaintAll()
    {
      foreach (EditorWindow frameDebugger in FrameDebuggerWindow.s_FrameDebuggers)
        frameDebugger.Repaint();
    }

    internal void ChangeFrameEventLimit(int newLimit)
    {
      if (newLimit <= 0 || newLimit > FrameDebuggerUtility.count)
        return;
      if (newLimit != FrameDebuggerUtility.limit && newLimit > 0)
      {
        GameObject frameEventGameObject = FrameDebuggerUtility.GetFrameEventGameObject(newLimit - 1);
        if ((UnityEngine.Object) frameEventGameObject != (UnityEngine.Object) null)
          EditorGUIUtility.PingObject((UnityEngine.Object) frameEventGameObject);
      }
      FrameDebuggerUtility.limit = newLimit;
      if (this.m_Tree == null)
        return;
      this.m_Tree.SelectFrameEventIndex(newLimit);
    }

    private static void DisableFrameDebugger()
    {
      if (FrameDebuggerUtility.IsLocalEnabled())
        EditorApplication.SetSceneRepaintDirty();
      FrameDebuggerUtility.SetEnabled(false, FrameDebuggerUtility.GetRemotePlayerGUID());
    }

    internal void OnDidOpenScene()
    {
      FrameDebuggerWindow.DisableFrameDebugger();
    }

    private void OnPauseStateChanged(PauseState state)
    {
      this.RepaintOnLimitChange();
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
      this.RepaintOnLimitChange();
    }

    internal void OnEnable()
    {
      this.autoRepaintOnSceneChange = true;
      FrameDebuggerWindow.s_FrameDebuggers.Add(this);
      EditorApplication.pauseStateChanged += new Action<PauseState>(this.OnPauseStateChanged);
      EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      this.m_RepaintFrames = 4;
    }

    internal void OnDisable()
    {
      if ((UnityEngine.Object) this.m_WireMaterial != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_WireMaterial, true);
      if (this.m_PreviewUtility != null)
      {
        this.m_PreviewUtility.Cleanup();
        this.m_PreviewUtility = (PreviewRenderUtility) null;
      }
      FrameDebuggerWindow.s_FrameDebuggers.Remove(this);
      EditorApplication.pauseStateChanged -= new Action<PauseState>(this.OnPauseStateChanged);
      EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      FrameDebuggerWindow.DisableFrameDebugger();
    }

    public void EnableIfNeeded()
    {
      if (FrameDebuggerUtility.IsLocalEnabled() || FrameDebuggerUtility.IsRemoteEnabled())
        return;
      this.m_RTChannel = 0;
      this.m_RTIndex = 0;
      this.m_RTBlackLevel = 0.0f;
      this.m_RTWhiteLevel = 1f;
      this.ClickEnableFrameDebugger();
      this.RepaintOnLimitChange();
    }

    private void ClickEnableFrameDebugger()
    {
      bool flag1 = FrameDebuggerUtility.IsLocalEnabled() || FrameDebuggerUtility.IsRemoteEnabled();
      bool flag2 = !flag1 && this.m_AttachProfilerUI.IsEditor();
      if (flag2 && !FrameDebuggerUtility.locallySupported)
        return;
      if (flag2 && EditorApplication.isPlaying && !EditorApplication.isPaused)
        EditorApplication.isPaused = true;
      if (!flag1)
        FrameDebuggerUtility.SetEnabled(true, ProfilerDriver.connectedProfiler);
      else
        FrameDebuggerUtility.SetEnabled(false, FrameDebuggerUtility.GetRemotePlayerGUID());
      if (FrameDebuggerUtility.IsLocalEnabled())
      {
        GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
        if ((bool) ((UnityEngine.Object) editorWindowOfType))
          editorWindowOfType.ShowTab();
      }
      this.m_PrevEventsLimit = FrameDebuggerUtility.limit;
      this.m_PrevEventsCount = FrameDebuggerUtility.count;
    }

    private void BuildCurEventDataStrings()
    {
      this.m_CurEventDataStrings.shader = string.Format("{0}, SubShader #{1}", (object) this.m_CurEventData.shaderName, (object) this.m_CurEventData.subShaderIndex.ToString());
      this.m_CurEventDataStrings.pass = (!string.IsNullOrEmpty(this.m_CurEventData.passName) ? this.m_CurEventData.passName : "#" + this.m_CurEventData.shaderPassIndex.ToString()) + (!string.IsNullOrEmpty(this.m_CurEventData.passLightMode) ? string.Format(" ({0})", (object) this.m_CurEventData.passLightMode) : "");
      if (this.m_CurEventData.stencilState.stencilEnable)
      {
        this.m_CurEventDataStrings.stencilRef = this.m_CurEventData.stencilRef.ToString();
        if ((int) this.m_CurEventData.stencilState.readMask != (int) byte.MaxValue)
          this.m_CurEventDataStrings.stencilReadMask = this.m_CurEventData.stencilState.readMask.ToString();
        if ((int) this.m_CurEventData.stencilState.writeMask != (int) byte.MaxValue)
          this.m_CurEventDataStrings.stencilWriteMask = this.m_CurEventData.stencilState.writeMask.ToString();
        if (this.m_CurEventData.rasterState.cullMode == CullMode.Back)
        {
          this.m_CurEventDataStrings.stencilComp = this.m_CurEventData.stencilState.stencilFuncFront.ToString();
          this.m_CurEventDataStrings.stencilPass = this.m_CurEventData.stencilState.stencilPassOpFront.ToString();
          this.m_CurEventDataStrings.stencilFail = this.m_CurEventData.stencilState.stencilFailOpFront.ToString();
          this.m_CurEventDataStrings.stencilZFail = this.m_CurEventData.stencilState.stencilZFailOpFront.ToString();
        }
        else if (this.m_CurEventData.rasterState.cullMode == CullMode.Front)
        {
          this.m_CurEventDataStrings.stencilComp = this.m_CurEventData.stencilState.stencilFuncBack.ToString();
          this.m_CurEventDataStrings.stencilPass = this.m_CurEventData.stencilState.stencilPassOpBack.ToString();
          this.m_CurEventDataStrings.stencilFail = this.m_CurEventData.stencilState.stencilFailOpBack.ToString();
          this.m_CurEventDataStrings.stencilZFail = this.m_CurEventData.stencilState.stencilZFailOpBack.ToString();
        }
        else
        {
          this.m_CurEventDataStrings.stencilComp = string.Format("{0} {1}", (object) this.m_CurEventData.stencilState.stencilFuncFront.ToString(), (object) this.m_CurEventData.stencilState.stencilFuncBack.ToString());
          this.m_CurEventDataStrings.stencilPass = string.Format("{0} {1}", (object) this.m_CurEventData.stencilState.stencilPassOpFront.ToString(), (object) this.m_CurEventData.stencilState.stencilPassOpBack.ToString());
          this.m_CurEventDataStrings.stencilFail = string.Format("{0} {1}", (object) this.m_CurEventData.stencilState.stencilFailOpFront.ToString(), (object) this.m_CurEventData.stencilState.stencilFailOpBack.ToString());
          this.m_CurEventDataStrings.stencilZFail = string.Format("{0} {1}", (object) this.m_CurEventData.stencilState.stencilZFailOpFront.ToString(), (object) this.m_CurEventData.stencilState.stencilZFailOpBack.ToString());
        }
      }
      ShaderTextureInfo[] textures = this.m_CurEventData.shaderProperties.textures;
      this.m_CurEventDataStrings.texturePropertyTooltips = new string[textures.Length];
      StringBuilder builder = new StringBuilder();
      for (int index = 0; index < textures.Length; ++index)
      {
        Texture texture = textures[index].value;
        if (!((UnityEngine.Object) texture == (UnityEngine.Object) null))
        {
          builder.Clear();
          builder.AppendFormat("Size: {0} x {1}", (object) texture.width.ToString(), (object) texture.height.ToString());
          builder.AppendFormat("\nDimension: {0}", (object) texture.dimension.ToString());
          string format1 = "\nFormat: {0}";
          string format2 = "\nDepth: {0}";
          if (texture is Texture2D)
            builder.AppendFormat(format1, (object) (texture as Texture2D).format.ToString());
          else if (texture is Cubemap)
            builder.AppendFormat(format1, (object) (texture as Cubemap).format.ToString());
          else if (texture is Texture2DArray)
          {
            builder.AppendFormat(format1, (object) (texture as Texture2DArray).format.ToString());
            builder.AppendFormat(format2, (object) (texture as Texture2DArray).depth.ToString());
          }
          else if (texture is Texture3D)
          {
            builder.AppendFormat(format1, (object) (texture as Texture3D).format.ToString());
            builder.AppendFormat(format2, (object) (texture as Texture3D).depth.ToString());
          }
          else if (texture is CubemapArray)
          {
            builder.AppendFormat(format1, (object) (texture as CubemapArray).format.ToString());
            builder.AppendFormat("\nCubemap Count: {0}", (object) (texture as CubemapArray).cubemapCount.ToString());
          }
          else if (texture is RenderTexture)
            builder.AppendFormat("\nRT Format: {0}", (object) (texture as RenderTexture).format.ToString());
          builder.Append("\n\nCtrl + Click to show preview");
          this.m_CurEventDataStrings.texturePropertyTooltips[index] = builder.ToString();
        }
      }
    }

    private bool DrawToolbar(FrameDebuggerEvent[] descs)
    {
      bool flag1 = false;
      bool flag2 = !this.m_AttachProfilerUI.IsEditor() || FrameDebuggerUtility.locallySupported;
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      using (new EditorGUI.DisabledScope(!flag2))
        GUILayout.Toggle((FrameDebuggerUtility.IsLocalEnabled() ? 1 : (FrameDebuggerUtility.IsRemoteEnabled() ? 1 : 0)) != 0, FrameDebuggerWindow.styles.recordButton, EditorStyles.toolbarButton, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(80f)
        });
      if (EditorGUI.EndChangeCheck())
      {
        this.ClickEnableFrameDebugger();
        flag1 = true;
      }
      this.m_AttachProfilerUI.OnGUILayout((EditorWindow) this);
      bool flag3 = FrameDebuggerUtility.IsLocalEnabled() || FrameDebuggerUtility.IsRemoteEnabled();
      if (flag3 && ProfilerDriver.connectedProfiler != FrameDebuggerUtility.GetRemotePlayerGUID())
      {
        FrameDebuggerUtility.SetEnabled(false, FrameDebuggerUtility.GetRemotePlayerGUID());
        FrameDebuggerUtility.SetEnabled(true, ProfilerDriver.connectedProfiler);
      }
      GUI.enabled = flag3;
      EditorGUI.BeginChangeCheck();
      int newLimit;
      using (new EditorGUI.DisabledScope(FrameDebuggerUtility.count <= 1))
        newLimit = EditorGUILayout.IntSlider(FrameDebuggerUtility.limit, 1, FrameDebuggerUtility.count);
      if (EditorGUI.EndChangeCheck())
        this.ChangeFrameEventLimit(newLimit);
      GUILayout.Label(" of " + (object) FrameDebuggerUtility.count, EditorStyles.miniLabel, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(newLimit <= 1))
      {
        if (GUILayout.Button(FrameDebuggerWindow.styles.prevFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          this.ChangeFrameEventLimit(newLimit - 1);
      }
      using (new EditorGUI.DisabledScope(newLimit >= FrameDebuggerUtility.count))
      {
        if (GUILayout.Button(FrameDebuggerWindow.styles.nextFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          this.ChangeFrameEventLimit(newLimit + 1);
        if (this.m_PrevEventsLimit == this.m_PrevEventsCount && FrameDebuggerUtility.count != this.m_PrevEventsCount && FrameDebuggerUtility.limit == this.m_PrevEventsLimit)
          this.ChangeFrameEventLimit(FrameDebuggerUtility.count);
        this.m_PrevEventsLimit = FrameDebuggerUtility.limit;
        this.m_PrevEventsCount = FrameDebuggerUtility.count;
      }
      GUILayout.EndHorizontal();
      return flag1;
    }

    private void DrawMeshPreview(Rect previewRect, Rect meshInfoRect, Mesh mesh, int meshSubset)
    {
      if (this.m_PreviewUtility == null)
      {
        this.m_PreviewUtility = new PreviewRenderUtility();
        this.m_PreviewUtility.camera.fieldOfView = 30f;
      }
      if ((UnityEngine.Object) this.m_Material == (UnityEngine.Object) null)
        this.m_Material = EditorGUIUtility.GetBuiltinExtraResource(typeof (Material), "Default-Material.mat") as Material;
      if ((UnityEngine.Object) this.m_WireMaterial == (UnityEngine.Object) null)
        this.m_WireMaterial = ModelInspector.CreateWireframeMaterial();
      this.m_PreviewUtility.BeginPreview(previewRect, (GUIStyle) "preBackground");
      ModelInspector.RenderMeshPreview(mesh, this.m_PreviewUtility, this.m_Material, this.m_WireMaterial, this.m_PreviewDir, meshSubset);
      this.m_PreviewUtility.EndAndDrawPreview(previewRect);
      string str = mesh.name;
      if (string.IsNullOrEmpty(str))
        str = "<no name>";
      string text = str + " subset " + (object) meshSubset + "\n" + (object) this.m_CurEventData.vertexCount + " verts, " + (object) this.m_CurEventData.indexCount + " indices";
      if (this.m_CurEventData.instanceCount > 1)
        text = text + ", " + (object) this.m_CurEventData.instanceCount + " instances";
      EditorGUI.DropShadowLabel(meshInfoRect, text);
    }

    private bool DrawEventMesh()
    {
      Mesh mesh = this.m_CurEventData.mesh;
      if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
        return false;
      Rect rect = GUILayoutUtility.GetRect(10f, 10f, new GUILayoutOption[1]{ GUILayout.ExpandHeight(true) });
      if ((double) rect.width < 64.0 || (double) rect.height < 64.0)
        return true;
      GameObject frameEventGameObject = FrameDebuggerUtility.GetFrameEventGameObject(this.m_CurEventData.frameEventIndex);
      Rect meshInfoRect = rect;
      meshInfoRect.yMin = meshInfoRect.yMax - EditorGUIUtility.singleLineHeight * 2f;
      Rect position = meshInfoRect;
      meshInfoRect.xMin = meshInfoRect.center.x;
      position.xMax = position.center.x;
      if (Event.current.type == EventType.MouseDown)
      {
        if (meshInfoRect.Contains(Event.current.mousePosition))
        {
          EditorGUIUtility.PingObject((UnityEngine.Object) mesh);
          Event.current.Use();
        }
        if ((UnityEngine.Object) frameEventGameObject != (UnityEngine.Object) null && position.Contains(Event.current.mousePosition))
        {
          EditorGUIUtility.PingObject(frameEventGameObject.GetInstanceID());
          Event.current.Use();
        }
      }
      this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, rect);
      if (Event.current.type == EventType.Repaint)
      {
        int meshSubset = this.m_CurEventData.meshSubset;
        this.DrawMeshPreview(rect, meshInfoRect, mesh, meshSubset);
        if ((UnityEngine.Object) frameEventGameObject != (UnityEngine.Object) null)
          EditorGUI.DropShadowLabel(position, frameEventGameObject.name);
      }
      return true;
    }

    private void DrawRenderTargetControls()
    {
      FrameDebuggerEventData curEventData = this.m_CurEventData;
      if (curEventData.rtWidth <= 0 || curEventData.rtHeight <= 0)
        return;
      bool disabled = curEventData.rtFormat == 1 || curEventData.rtFormat == 3;
      bool flag1 = (int) curEventData.rtHasDepthTexture != 0;
      short rtCount = curEventData.rtCount;
      if (flag1)
        ++rtCount;
      EditorGUILayout.LabelField("RenderTarget", curEventData.rtName, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      bool flag2;
      using (new EditorGUI.DisabledScope((int) rtCount <= 1))
      {
        GUIContent[] displayedOptions = new GUIContent[(int) rtCount];
        for (int index = 0; index < (int) curEventData.rtCount; ++index)
          displayedOptions[index] = FrameDebuggerWindow.Styles.mrtLabels[index];
        if (flag1)
          displayedOptions[(int) curEventData.rtCount] = FrameDebuggerWindow.Styles.depthLabel;
        int num = Mathf.Clamp(this.m_RTIndex, 0, (int) rtCount - 1);
        flag2 = num != this.m_RTIndex;
        this.m_RTIndex = num;
        this.m_RTIndex = EditorGUILayout.Popup(this.m_RTIndex, displayedOptions, EditorStyles.toolbarPopup, new GUILayoutOption[1]
        {
          GUILayout.Width(70f)
        });
      }
      GUILayout.Space(10f);
      using (new EditorGUI.DisabledScope(disabled))
      {
        GUILayout.Label(FrameDebuggerWindow.Styles.channelHeader, EditorStyles.miniLabel, new GUILayoutOption[0]);
        this.m_RTChannel = GUILayout.Toolbar(this.m_RTChannel, FrameDebuggerWindow.Styles.channelLabels, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      }
      GUILayout.Space(10f);
      GUILayout.Label(FrameDebuggerWindow.Styles.levelsHeader, EditorStyles.miniLabel, new GUILayoutOption[0]);
      EditorGUILayout.MinMaxSlider(ref this.m_RTBlackLevel, ref this.m_RTWhiteLevel, 0.0f, 1f, GUILayout.MaxWidth(200f));
      if (EditorGUI.EndChangeCheck() || flag2)
      {
        Vector4 channels = Vector4.zero;
        if (this.m_RTChannel == 1)
          channels.x = 1f;
        else if (this.m_RTChannel == 2)
          channels.y = 1f;
        else if (this.m_RTChannel == 3)
          channels.z = 1f;
        else if (this.m_RTChannel == 4)
          channels.w = 1f;
        else
          channels = Vector4.one;
        int rtIndex = this.m_RTIndex;
        if (rtIndex >= (int) curEventData.rtCount)
          rtIndex = -1;
        FrameDebuggerUtility.SetRenderTargetDisplayOptions(rtIndex, channels, this.m_RTBlackLevel, this.m_RTWhiteLevel);
        this.RepaintAllNeededThings();
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.Label(string.Format("{0}x{1} {2}", (object) curEventData.rtWidth, (object) curEventData.rtHeight, (object) (RenderTextureFormat) curEventData.rtFormat));
      if (curEventData.rtDim != 4)
        return;
      GUILayout.Label("Rendering into cubemap");
    }

    private void DrawEventDrawCallInfo()
    {
      EditorGUILayout.LabelField("Shader", this.m_CurEventDataStrings.shader, new GUILayoutOption[0]);
      if (GUI.Button(GUILayoutUtility.GetLastRect(), FrameDebuggerWindow.Styles.selectShaderTooltip, GUI.skin.label))
      {
        EditorGUIUtility.PingObject(this.m_CurEventData.shaderInstanceID);
        Event.current.Use();
      }
      EditorGUILayout.LabelField("Pass", this.m_CurEventDataStrings.pass, new GUILayoutOption[0]);
      if (!string.IsNullOrEmpty(this.m_CurEventData.shaderKeywords))
      {
        EditorGUILayout.LabelField("Keywords", this.m_CurEventData.shaderKeywords, new GUILayoutOption[0]);
        if (GUI.Button(GUILayoutUtility.GetLastRect(), FrameDebuggerWindow.Styles.copyToClipboardTooltip, GUI.skin.label))
          EditorGUIUtility.systemCopyBuffer = this.m_CurEventDataStrings.shader + Environment.NewLine + this.m_CurEventData.shaderKeywords;
      }
      this.DrawStates();
      if (this.m_CurEventData.batchBreakCause > 1)
      {
        GUILayout.Space(10f);
        GUILayout.Label(FrameDebuggerWindow.Styles.causeOfNewDrawCallLabel, EditorStyles.boldLabel, new GUILayoutOption[0]);
        GUILayout.Label(FrameDebuggerWindow.styles.batchBreakCauses[this.m_CurEventData.batchBreakCause], EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      }
      GUILayout.Space(15f);
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.m_AdditionalInfo = (ShowAdditionalInfo) GUILayout.Toolbar((int) this.m_AdditionalInfo, this.m_AdditionalInfoGuiContents, (GUIStyle) "LargeButton", GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      switch (this.m_AdditionalInfo)
      {
        case ShowAdditionalInfo.Preview:
          if (this.DrawEventMesh())
            break;
          EditorGUILayout.LabelField("Vertices", this.m_CurEventData.vertexCount.ToString(), new GUILayoutOption[0]);
          EditorGUILayout.LabelField("Indices", this.m_CurEventData.indexCount.ToString(), new GUILayoutOption[0]);
          break;
        case ShowAdditionalInfo.ShaderProperties:
          this.DrawShaderProperties(this.m_CurEventData.shaderProperties);
          break;
      }
    }

    private void DrawEventComputeDispatchInfo()
    {
      EditorGUILayout.LabelField("Compute Shader", this.m_CurEventData.csName, new GUILayoutOption[0]);
      if (GUI.Button(GUILayoutUtility.GetLastRect(), GUIContent.none, GUI.skin.label))
      {
        EditorGUIUtility.PingObject(this.m_CurEventData.csInstanceID);
        Event.current.Use();
      }
      EditorGUILayout.LabelField("Kernel", this.m_CurEventData.csKernel, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Thread Groups", this.m_CurEventData.csThreadGroupsX != 0 || this.m_CurEventData.csThreadGroupsY != 0 || this.m_CurEventData.csThreadGroupsZ != 0 ? string.Format("{0}x{1}x{2}", (object) this.m_CurEventData.csThreadGroupsX, (object) this.m_CurEventData.csThreadGroupsY, (object) this.m_CurEventData.csThreadGroupsZ) : "indirect dispatch", new GUILayoutOption[0]);
    }

    private void DrawCurrentEvent(Rect rect, FrameDebuggerEvent[] descs)
    {
      int index = FrameDebuggerUtility.limit - 1;
      if (index < 0 || index >= descs.Length)
        return;
      GUILayout.BeginArea(rect);
      uint eventDataHash = FrameDebuggerUtility.eventDataHash;
      bool flag = index == this.m_CurEventData.frameEventIndex;
      if ((int) eventDataHash != 0 && (int) this.m_CurEventDataHash != (int) eventDataHash)
      {
        flag = FrameDebuggerUtility.GetFrameEventData(index, out this.m_CurEventData);
        this.m_CurEventDataHash = eventDataHash;
        this.BuildCurEventDataStrings();
      }
      if (flag)
        this.DrawRenderTargetControls();
      FrameDebuggerEvent desc = descs[index];
      GUILayout.Label(string.Format("Event #{0}: {1}", (object) (index + 1), (object) FrameDebuggerWindow.s_FrameEventTypeNames[(int) desc.type]), EditorStyles.boldLabel, new GUILayoutOption[0]);
      if (FrameDebuggerUtility.IsRemoteEnabled() && FrameDebuggerUtility.receivingRemoteFrameEventData)
        GUILayout.Label("Receiving frame event data...");
      else if (flag)
      {
        if (this.m_CurEventData.vertexCount > 0 || this.m_CurEventData.indexCount > 0)
          this.DrawEventDrawCallInfo();
        else if (desc.type == FrameEventType.ComputeDispatch)
          this.DrawEventComputeDispatchInfo();
      }
      GUILayout.EndArea();
    }

    private void DrawShaderPropertyFlags(int flags)
    {
      string empty = string.Empty;
      if ((flags & 2) != 0)
        empty += (string) (object) 'v';
      if ((flags & 4) != 0)
        empty += (string) (object) 'f';
      if ((flags & 8) != 0)
        empty += (string) (object) 'g';
      if ((flags & 16) != 0)
        empty += (string) (object) 'h';
      if ((flags & 32) != 0)
        empty += (string) (object) 'd';
      GUILayout.Label(empty, EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(20f)
      });
    }

    private void ShaderPropertyCopyValueMenu(Rect valueRect, object value)
    {
      Event current = Event.current;
      if (current.type != EventType.ContextClick || !valueRect.Contains(current.mousePosition))
        return;
      current.Use();
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Copy value"), false, (GenericMenu.MenuFunction) (() =>
      {
        string empty = string.Empty;
        EditorGUIUtility.systemCopyBuffer = !(value is Vector4) ? (!(value is Matrix4x4) ? (!(value is float) ? value.ToString() : ((float) value).ToString("g7")) : ((Matrix4x4) value).ToString("g7")) : ((Vector4) value).ToString("g7");
      }));
      genericMenu.ShowAsContext();
    }

    private void OnGUIShaderPropFloats(ShaderFloatInfo[] floats, int startIndex, int numValues)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(15f);
      ShaderFloatInfo shaderFloatInfo = floats[startIndex];
      if (numValues == 1)
      {
        GUILayout.Label(shaderFloatInfo.name, EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.DrawShaderPropertyFlags(shaderFloatInfo.flags);
        GUILayout.Label(shaderFloatInfo.value.ToString("g2"), EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.ShaderPropertyCopyValueMenu(GUILayoutUtility.GetLastRect(), (object) shaderFloatInfo.value);
      }
      else
      {
        GUILayout.Label(string.Format("{0} [{1}]", (object) shaderFloatInfo.name, (object) numValues), EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.DrawShaderPropertyFlags(shaderFloatInfo.flags);
        Rect rect = GUILayoutUtility.GetRect(FrameDebuggerWindow.Styles.arrayValuePopupButton, GUI.skin.button, new GUILayoutOption[1]{ GUILayout.MinWidth(200f) });
        rect.width = 25f;
        if (GUI.Button(rect, FrameDebuggerWindow.Styles.arrayValuePopupButton))
        {
          FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate getValueString = (FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate) ((index, highPrecision) => floats[index].value.ToString(!highPrecision ? "g2" : "g7"));
          PopupWindowWithoutFocus.Show(rect, (PopupWindowContent) new FrameDebuggerWindow.ArrayValuePopup(startIndex, numValues, 100f, getValueString), new PopupLocationHelper.PopupLocation[3]
          {
            PopupLocationHelper.PopupLocation.Left,
            PopupLocationHelper.PopupLocation.Below,
            PopupLocationHelper.PopupLocation.Right
          });
        }
      }
      GUILayout.EndHorizontal();
    }

    private void OnGUIShaderPropVectors(ShaderVectorInfo[] vectors, int startIndex, int numValues)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(15f);
      ShaderVectorInfo vector = vectors[startIndex];
      if (numValues == 1)
      {
        GUILayout.Label(vector.name, EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.DrawShaderPropertyFlags(vector.flags);
        GUILayout.Label(vector.value.ToString("g2"), EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.ShaderPropertyCopyValueMenu(GUILayoutUtility.GetLastRect(), (object) vector.value);
      }
      else
      {
        GUILayout.Label(string.Format("{0} [{1}]", (object) vector.name, (object) numValues), EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.DrawShaderPropertyFlags(vector.flags);
        Rect rect = GUILayoutUtility.GetRect(FrameDebuggerWindow.Styles.arrayValuePopupButton, GUI.skin.button, new GUILayoutOption[1]{ GUILayout.MinWidth(200f) });
        rect.width = 25f;
        if (GUI.Button(rect, FrameDebuggerWindow.Styles.arrayValuePopupButton))
        {
          FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate getValueString = (FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate) ((index, highPrecision) => vectors[index].value.ToString(!highPrecision ? "g2" : "g7"));
          PopupWindowWithoutFocus.Show(rect, (PopupWindowContent) new FrameDebuggerWindow.ArrayValuePopup(startIndex, numValues, 200f, getValueString), new PopupLocationHelper.PopupLocation[3]
          {
            PopupLocationHelper.PopupLocation.Left,
            PopupLocationHelper.PopupLocation.Below,
            PopupLocationHelper.PopupLocation.Right
          });
        }
      }
      GUILayout.EndHorizontal();
    }

    private void OnGUIShaderPropMatrices(ShaderMatrixInfo[] matrices, int startIndex, int numValues)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(15f);
      ShaderMatrixInfo matrix = matrices[startIndex];
      if (numValues == 1)
      {
        GUILayout.Label(matrix.name, EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.DrawShaderPropertyFlags(matrix.flags);
        GUILayout.Label(matrix.value.ToString("g2"), EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.ShaderPropertyCopyValueMenu(GUILayoutUtility.GetLastRect(), (object) matrix.value);
      }
      else
      {
        GUILayout.Label(string.Format("{0} [{1}]", (object) matrix.name, (object) numValues), EditorStyles.miniLabel, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(200f)
        });
        this.DrawShaderPropertyFlags(matrix.flags);
        Rect rect = GUILayoutUtility.GetRect(FrameDebuggerWindow.Styles.arrayValuePopupButton, GUI.skin.button, new GUILayoutOption[1]{ GUILayout.MinWidth(200f) });
        rect.width = 25f;
        if (GUI.Button(rect, FrameDebuggerWindow.Styles.arrayValuePopupButton))
        {
          FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate getValueString = (FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate) ((index, highPrecision) => 10.ToString() + matrices[index].value.ToString(!highPrecision ? "g2" : "g7"));
          PopupWindowWithoutFocus.Show(rect, (PopupWindowContent) new FrameDebuggerWindow.ArrayValuePopup(startIndex, numValues, 200f, getValueString), new PopupLocationHelper.PopupLocation[3]
          {
            PopupLocationHelper.PopupLocation.Left,
            PopupLocationHelper.PopupLocation.Below,
            PopupLocationHelper.PopupLocation.Right
          });
        }
      }
      GUILayout.EndHorizontal();
    }

    private void OnGUIShaderPropBuffer(ShaderBufferInfo t)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(15f);
      GUILayout.Label(t.name, EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(200f)
      });
      this.DrawShaderPropertyFlags(t.flags);
      GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(200f)
      });
      GUILayout.EndHorizontal();
    }

    private void OnGUIShaderPropTexture(int idx, ShaderTextureInfo t)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(15f);
      GUILayout.Label(t.name, EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(200f)
      });
      this.DrawShaderPropertyFlags(t.flags);
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.label, new GUILayoutOption[1]{ GUILayout.MinWidth(200f) });
      Event current = Event.current;
      Rect position1 = rect;
      position1.width = position1.height;
      if ((UnityEngine.Object) t.value != (UnityEngine.Object) null && position1.Contains(current.mousePosition))
        GUI.Label(position1, GUIContent.Temp(string.Empty, this.m_CurEventDataStrings.texturePropertyTooltips[idx]));
      if (current.type == EventType.Repaint)
      {
        Rect position2 = rect;
        position2.xMin += position1.width;
        if ((UnityEngine.Object) t.value != (UnityEngine.Object) null)
        {
          Texture miniThumbnail = t.value;
          if (miniThumbnail.dimension != TextureDimension.Tex2D)
            miniThumbnail = (Texture) AssetPreview.GetMiniThumbnail((UnityEngine.Object) miniThumbnail);
          EditorGUI.DrawPreviewTexture(position1, miniThumbnail);
        }
        GUI.Label(position2, !((UnityEngine.Object) t.value != (UnityEngine.Object) null) ? t.textureName : t.value.name);
      }
      else if (current.type == EventType.MouseDown && rect.Contains(current.mousePosition))
      {
        EditorGUI.PingObjectOrShowPreviewOnClick((UnityEngine.Object) t.value, rect);
        current.Use();
      }
      GUILayout.EndHorizontal();
    }

    private void DrawShaderProperties(ShaderProperties props)
    {
      this.m_ScrollViewShaderProps = GUILayout.BeginScrollView(this.m_ScrollViewShaderProps);
      if (((IEnumerable<ShaderTextureInfo>) props.textures).Count<ShaderTextureInfo>() > 0)
      {
        GUILayout.Label("Textures", EditorStyles.boldLabel, new GUILayoutOption[0]);
        for (int idx = 0; idx < props.textures.Length; ++idx)
          this.OnGUIShaderPropTexture(idx, props.textures[idx]);
      }
      if (((IEnumerable<ShaderFloatInfo>) props.floats).Count<ShaderFloatInfo>() > 0)
      {
        GUILayout.Label("Floats", EditorStyles.boldLabel, new GUILayoutOption[0]);
        int startIndex = 0;
        while (startIndex < props.floats.Length)
        {
          int numValues = props.floats[startIndex].flags >> 6 & 1023;
          this.OnGUIShaderPropFloats(props.floats, startIndex, numValues);
          startIndex += numValues;
        }
      }
      if (((IEnumerable<ShaderVectorInfo>) props.vectors).Count<ShaderVectorInfo>() > 0)
      {
        GUILayout.Label("Vectors", EditorStyles.boldLabel, new GUILayoutOption[0]);
        int startIndex = 0;
        while (startIndex < props.vectors.Length)
        {
          int numValues = props.vectors[startIndex].flags >> 6 & 1023;
          this.OnGUIShaderPropVectors(props.vectors, startIndex, numValues);
          startIndex += numValues;
        }
      }
      if (((IEnumerable<ShaderMatrixInfo>) props.matrices).Count<ShaderMatrixInfo>() > 0)
      {
        GUILayout.Label("Matrices", EditorStyles.boldLabel, new GUILayoutOption[0]);
        int startIndex = 0;
        while (startIndex < props.matrices.Length)
        {
          int numValues = props.matrices[startIndex].flags >> 6 & 1023;
          this.OnGUIShaderPropMatrices(props.matrices, startIndex, numValues);
          startIndex += numValues;
        }
      }
      if (((IEnumerable<ShaderBufferInfo>) props.buffers).Count<ShaderBufferInfo>() > 0)
      {
        GUILayout.Label("Buffers", EditorStyles.boldLabel, new GUILayoutOption[0]);
        foreach (ShaderBufferInfo buffer in props.buffers)
          this.OnGUIShaderPropBuffer(buffer);
      }
      GUILayout.EndScrollView();
    }

    private void DrawStates()
    {
      FrameDebuggerBlendState blendState = this.m_CurEventData.blendState;
      FrameDebuggerRasterState rasterState = this.m_CurEventData.rasterState;
      FrameDebuggerDepthState depthState = this.m_CurEventData.depthState;
      string label2_1 = string.Format("{0} {1}", (object) blendState.srcBlend, (object) blendState.dstBlend);
      if (blendState.srcBlendAlpha != blendState.srcBlend || blendState.dstBlendAlpha != blendState.dstBlend)
        label2_1 += string.Format(", {0} {1}", (object) blendState.srcBlendAlpha, (object) blendState.dstBlendAlpha);
      EditorGUILayout.LabelField("Blend", label2_1, new GUILayoutOption[0]);
      if (blendState.blendOp != BlendOp.Add || blendState.blendOpAlpha != BlendOp.Add)
        EditorGUILayout.LabelField("BlendOp", blendState.blendOp != blendState.blendOpAlpha ? string.Format("{0}, {1}", (object) blendState.blendOp, (object) blendState.blendOpAlpha) : blendState.blendOp.ToString(), new GUILayoutOption[0]);
      if ((int) blendState.writeMask != 15)
      {
        string label2_2 = "";
        if ((int) blendState.writeMask == 0)
        {
          label2_2 += (string) (object) '0';
        }
        else
        {
          if (((int) blendState.writeMask & 2) != 0)
            label2_2 += (string) (object) 'R';
          if (((int) blendState.writeMask & 4) != 0)
            label2_2 += (string) (object) 'G';
          if (((int) blendState.writeMask & 8) != 0)
            label2_2 += (string) (object) 'B';
          if (((int) blendState.writeMask & 1) != 0)
            label2_2 += (string) (object) 'A';
        }
        EditorGUILayout.LabelField("ColorMask", label2_2, new GUILayoutOption[0]);
      }
      EditorGUILayout.LabelField("ZClip", rasterState.depthClip.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("ZTest", depthState.depthFunc.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("ZWrite", depthState.depthWrite != 0 ? "On" : "Off", new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Cull", rasterState.cullMode.ToString(), new GUILayoutOption[0]);
      if ((double) rasterState.slopeScaledDepthBias != 0.0 || rasterState.depthBias != 0)
        EditorGUILayout.LabelField("Offset", string.Format("{0}, {1}", (object) rasterState.slopeScaledDepthBias, (object) rasterState.depthBias), new GUILayoutOption[0]);
      if (!this.m_CurEventData.stencilState.stencilEnable)
        return;
      EditorGUILayout.LabelField("Stencil Ref", this.m_CurEventDataStrings.stencilRef, new GUILayoutOption[0]);
      if ((int) this.m_CurEventData.stencilState.readMask != (int) byte.MaxValue)
        EditorGUILayout.LabelField("Stencil ReadMask", this.m_CurEventDataStrings.stencilReadMask, new GUILayoutOption[0]);
      if ((int) this.m_CurEventData.stencilState.writeMask != (int) byte.MaxValue)
        EditorGUILayout.LabelField("Stencil WriteMask", this.m_CurEventDataStrings.stencilWriteMask, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Stencil Comp", this.m_CurEventDataStrings.stencilComp, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Stencil Pass", this.m_CurEventDataStrings.stencilPass, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Stencil Fail", this.m_CurEventDataStrings.stencilFail, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Stencil ZFail", this.m_CurEventDataStrings.stencilZFail, new GUILayoutOption[0]);
    }

    internal void OnGUI()
    {
      FrameDebuggerEvent[] frameEvents = FrameDebuggerUtility.GetFrameEvents();
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      if (this.m_Tree == null)
      {
        this.m_Tree = new FrameDebuggerTreeView(frameEvents, this.m_TreeViewState, this, new Rect());
        this.m_FrameEventsHash = FrameDebuggerUtility.eventsHash;
        this.m_Tree.m_DataSource.SetExpandedWithChildren(this.m_Tree.m_DataSource.root, true);
      }
      if (FrameDebuggerUtility.eventsHash != this.m_FrameEventsHash)
      {
        this.m_Tree.m_DataSource.SetEvents(frameEvents);
        this.m_FrameEventsHash = FrameDebuggerUtility.eventsHash;
      }
      int limit = FrameDebuggerUtility.limit;
      bool flag = this.DrawToolbar(frameEvents);
      if (!FrameDebuggerUtility.IsLocalEnabled() && !FrameDebuggerUtility.IsRemoteEnabled() && this.m_AttachProfilerUI.IsEditor())
      {
        GUI.enabled = true;
        if (!FrameDebuggerUtility.locallySupported)
          EditorGUILayout.HelpBox("Frame Debugger requires multi-threaded renderer. Usually Unity uses that; if it does not, try starting with -force-gfx-mt command line argument.", MessageType.Warning, true);
        EditorGUILayout.HelpBox("Frame Debugger lets you step through draw calls and see how exactly frame is rendered. Click Enable!", MessageType.Info, true);
      }
      else
      {
        float fixedHeight = EditorStyles.toolbar.fixedHeight;
        Rect dragRect = EditorGUIUtility.HandleHorizontalSplitter(new Rect(this.m_ListWidth, fixedHeight, 5f, this.position.height - fixedHeight), this.position.width, 200f, 200f);
        this.m_ListWidth = dragRect.x;
        Rect rect1 = new Rect(0.0f, fixedHeight, this.m_ListWidth, this.position.height - fixedHeight);
        Rect rect2 = new Rect(this.m_ListWidth + 4f, fixedHeight + 4f, (float) ((double) this.position.width - (double) this.m_ListWidth - 8.0), (float) ((double) this.position.height - (double) fixedHeight - 8.0));
        this.DrawEventsTree(rect1);
        EditorGUIUtility.DrawHorizontalSplitter(dragRect);
        this.DrawCurrentEvent(rect2, frameEvents);
      }
      if (flag || limit != FrameDebuggerUtility.limit)
        this.RepaintOnLimitChange();
      if (this.m_RepaintFrames <= 0)
        return;
      this.m_Tree.SelectFrameEventIndex(FrameDebuggerUtility.limit);
      this.RepaintAllNeededThings();
      --this.m_RepaintFrames;
    }

    private void RepaintOnLimitChange()
    {
      this.m_RepaintFrames = 4;
      this.RepaintAllNeededThings();
    }

    private void RepaintAllNeededThings()
    {
      EditorApplication.SetSceneRepaintDirty();
      this.Repaint();
    }

    private void DrawEventsTree(Rect rect)
    {
      this.m_Tree.OnGUI(rect);
    }

    public static FrameDebuggerWindow.Styles styles
    {
      get
      {
        return FrameDebuggerWindow.ms_Styles ?? (FrameDebuggerWindow.ms_Styles = new FrameDebuggerWindow.Styles());
      }
    }

    private struct EventDataStrings
    {
      public string shader;
      public string pass;
      public string stencilRef;
      public string stencilReadMask;
      public string stencilWriteMask;
      public string stencilComp;
      public string stencilPass;
      public string stencilFail;
      public string stencilZFail;
      public string[] texturePropertyTooltips;
    }

    internal class Styles
    {
      public static readonly string[] s_ColumnNames = new string[4]{ "#", "Type", "Vertices", "Indices" };
      public static readonly GUIContent[] mrtLabels = new GUIContent[8]{ EditorGUIUtility.TextContent("RT 0|Show render target #0"), EditorGUIUtility.TextContent("RT 1|Show render target #1"), EditorGUIUtility.TextContent("RT 2|Show render target #2"), EditorGUIUtility.TextContent("RT 3|Show render target #3"), EditorGUIUtility.TextContent("RT 4|Show render target #4"), EditorGUIUtility.TextContent("RT 5|Show render target #5"), EditorGUIUtility.TextContent("RT 6|Show render target #6"), EditorGUIUtility.TextContent("RT 7|Show render target #7") };
      public static readonly GUIContent depthLabel = EditorGUIUtility.TextContent("Depth|Show depth buffer");
      public static readonly GUIContent[] channelLabels = new GUIContent[5]{ EditorGUIUtility.TextContent("All|Show all (RGB) color channels"), EditorGUIUtility.TextContent("R|Show red channel only"), EditorGUIUtility.TextContent("G|Show green channel only"), EditorGUIUtility.TextContent("B|Show blue channel only"), EditorGUIUtility.TextContent("A|Show alpha channel only") };
      public static readonly GUIContent channelHeader = EditorGUIUtility.TextContent("Channels|Which render target color channels to show");
      public static readonly GUIContent levelsHeader = EditorGUIUtility.TextContent("Levels|Render target display black/white intensity levels");
      public static readonly GUIContent causeOfNewDrawCallLabel = EditorGUIUtility.TextContent("Why this draw call can't be batched with the previous one");
      public static readonly GUIContent selectShaderTooltip = EditorGUIUtility.TextContent("|Click to select shader");
      public static readonly GUIContent copyToClipboardTooltip = EditorGUIUtility.TextContent("|Click to copy shader and keywords text to clipboard.");
      public static readonly GUIContent arrayValuePopupButton = new GUIContent("...");
      public GUIStyle header = (GUIStyle) "OL title";
      public GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public GUIStyle rowText = (GUIStyle) "OL Label";
      public GUIStyle rowTextRight = new GUIStyle((GUIStyle) "OL Label");
      public GUIContent recordButton = new GUIContent(EditorGUIUtility.TextContent("Record|Record profiling information"));
      public GUIContent prevFrame = new GUIContent(EditorGUIUtility.IconContent("Profiler.PrevFrame", "|Go back one frame"));
      public GUIContent nextFrame = new GUIContent(EditorGUIUtility.IconContent("Profiler.NextFrame", "|Go one frame forwards"));
      public GUIContent[] headerContent;
      public readonly string[] batchBreakCauses;

      public Styles()
      {
        this.rowTextRight.alignment = TextAnchor.MiddleRight;
        this.recordButton.text = "Enable";
        this.recordButton.tooltip = "Enable Frame Debugging";
        this.prevFrame.tooltip = "Previous event";
        this.nextFrame.tooltip = "Next event";
        this.headerContent = new GUIContent[FrameDebuggerWindow.Styles.s_ColumnNames.Length];
        for (int index = 0; index < this.headerContent.Length; ++index)
          this.headerContent[index] = EditorGUIUtility.TextContent(FrameDebuggerWindow.Styles.s_ColumnNames[index]);
        this.batchBreakCauses = FrameDebuggerUtility.GetBatchBreakCauseStrings();
      }
    }

    private class ArrayValuePopup : PopupWindowContent
    {
      private static readonly GUIStyle m_Style = EditorStyles.miniLabel;
      private Vector2 m_ScrollPos = Vector2.zero;
      private FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate GetValueString;
      private int m_StartIndex;
      private int m_NumValues;
      private float m_WindowWidth;

      public ArrayValuePopup(int startIndex, int numValues, float windowWidth, FrameDebuggerWindow.ArrayValuePopup.GetValueStringDelegate getValueString)
      {
        this.m_StartIndex = startIndex;
        this.m_NumValues = numValues;
        this.m_WindowWidth = windowWidth;
        this.GetValueString = getValueString;
      }

      public override Vector2 GetWindowSize()
      {
        return new Vector2(this.m_WindowWidth, Math.Min((FrameDebuggerWindow.ArrayValuePopup.m_Style.lineHeight + (float) FrameDebuggerWindow.ArrayValuePopup.m_Style.padding.vertical + (float) FrameDebuggerWindow.ArrayValuePopup.m_Style.margin.top) * (float) this.m_NumValues, 250f));
      }

      public override void OnGUI(Rect rect)
      {
        this.m_ScrollPos = EditorGUILayout.BeginScrollView(this.m_ScrollPos);
        for (int index = 0; index < this.m_NumValues; ++index)
          GUILayout.Label(string.Format("[{0}]\t{1}", (object) index, (object) this.GetValueString(this.m_StartIndex + index, false)), FrameDebuggerWindow.ArrayValuePopup.m_Style, new GUILayoutOption[0]);
        EditorGUILayout.EndScrollView();
        Event current = Event.current;
        if (current.type != EventType.ContextClick || !rect.Contains(current.mousePosition))
          return;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        FrameDebuggerWindow.ArrayValuePopup.\u003COnGUI\u003Ec__AnonStorey0 onGuiCAnonStorey0 = new FrameDebuggerWindow.ArrayValuePopup.\u003COnGUI\u003Ec__AnonStorey0();
        current.Use();
        // ISSUE: reference to a compiler-generated field
        onGuiCAnonStorey0.allText = string.Empty;
        for (int index = 0; index < this.m_NumValues; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          onGuiCAnonStorey0.allText += string.Format("[{0}]\t{1}\n", (object) index, (object) this.GetValueString(this.m_StartIndex + index, true));
        }
        GenericMenu genericMenu = new GenericMenu();
        // ISSUE: reference to a compiler-generated method
        genericMenu.AddItem(new GUIContent("Copy value"), false, new GenericMenu.MenuFunction(onGuiCAnonStorey0.\u003C\u003Em__0));
        genericMenu.ShowAsContext();
      }

      public delegate string GetValueStringDelegate(int index, bool highPrecision);
    }
  }
}
