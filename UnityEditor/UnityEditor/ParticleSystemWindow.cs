// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ParticleSystemWindow : EditorWindow, ParticleEffectUIOwner
  {
    private static ParticleSystemWindow s_Instance;
    private ParticleSystem m_Target;
    private ParticleEffectUI m_ParticleEffectUI;
    private bool m_IsVisible;
    private static GUIContent[] s_Icons;
    private static ParticleSystemWindow.Texts s_Texts;

    private ParticleSystemWindow()
    {
    }

    public Editor customEditor { get; set; }

    public static void CreateWindow()
    {
      ParticleSystemWindow.s_Instance = EditorWindow.GetWindow<ParticleSystemWindow>();
      ParticleSystemWindow.s_Instance.titleContent = EditorGUIUtility.TextContent("Particle Effect");
      ParticleSystemWindow.s_Instance.minSize = ParticleEffectUI.GetMinSize();
    }

    internal static ParticleSystemWindow GetInstance()
    {
      return ParticleSystemWindow.s_Instance;
    }

    internal bool IsVisible()
    {
      return this.m_IsVisible;
    }

    private void OnEnable()
    {
      ParticleSystemWindow.s_Instance = this;
      this.m_Target = (ParticleSystem) null;
      ParticleEffectUI.m_VerticalLayout = EditorPrefs.GetBool("ShurikenVerticalLayout", false);
      EditorApplication.hierarchyWindowChanged += new EditorApplication.CallbackFunction(this.OnHierarchyOrProjectWindowWasChanged);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.OnHierarchyOrProjectWindowWasChanged);
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      EditorApplication.pauseStateChanged += new Action<PauseState>(this.OnPauseStateChanged);
      EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.autoRepaintOnSceneChange = false;
    }

    private void OnDisable()
    {
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.OnHierarchyOrProjectWindowWasChanged);
      EditorApplication.hierarchyWindowChanged -= new EditorApplication.CallbackFunction(this.OnHierarchyOrProjectWindowWasChanged);
      EditorApplication.pauseStateChanged -= new Action<PauseState>(this.OnPauseStateChanged);
      EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.Clear();
      if (!((UnityEngine.Object) ParticleSystemWindow.s_Instance == (UnityEngine.Object) this))
        return;
      ParticleSystemWindow.s_Instance = (ParticleSystemWindow) null;
    }

    internal void Clear()
    {
      this.m_Target = (ParticleSystem) null;
      if (this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.Clear();
      this.m_ParticleEffectUI = (ParticleEffectUI) null;
    }

    private void OnPauseStateChanged(PauseState state)
    {
      this.Repaint();
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
      this.Repaint();
    }

    private void UndoRedoPerformed()
    {
      if (this.m_ParticleEffectUI != null)
        this.m_ParticleEffectUI.UndoRedoPerformed();
      this.Repaint();
    }

    private void OnHierarchyOrProjectWindowWasChanged()
    {
      this.InitEffectUI();
    }

    private void OnBecameVisible()
    {
      if (this.m_IsVisible)
        return;
      this.m_IsVisible = true;
      this.InitEffectUI();
      SceneView.RepaintAll();
      InspectorWindow.RepaintAllInspectors();
    }

    private void OnBecameInvisible()
    {
      this.m_IsVisible = false;
      this.Clear();
      SceneView.RepaintAll();
      InspectorWindow.RepaintAllInspectors();
    }

    private void OnSelectionChange()
    {
      this.InitEffectUI();
      this.Repaint();
    }

    private void InitEffectUI()
    {
      if (!this.m_IsVisible)
        return;
      ParticleSystem particleSystem = ParticleSystemEditorUtils.lockedParticleSystem;
      if ((UnityEngine.Object) particleSystem == (UnityEngine.Object) null && (UnityEngine.Object) Selection.activeGameObject != (UnityEngine.Object) null)
        particleSystem = Selection.activeGameObject.GetComponent<ParticleSystem>();
      this.m_Target = particleSystem;
      if ((UnityEngine.Object) this.m_Target != (UnityEngine.Object) null)
      {
        if (this.m_ParticleEffectUI == null)
          this.m_ParticleEffectUI = new ParticleEffectUI((ParticleEffectUIOwner) this);
        if (this.m_ParticleEffectUI.InitializeIfNeeded((IEnumerable<ParticleSystem>) new ParticleSystem[1]{ this.m_Target }))
          this.Repaint();
      }
      if (!((UnityEngine.Object) this.m_Target == (UnityEngine.Object) null) || this.m_ParticleEffectUI == null)
        return;
      this.Clear();
      this.Repaint();
      SceneView.RepaintAll();
      GameView.RepaintAll();
    }

    private void Awake()
    {
    }

    private void DoToolbarGUI()
    {
      GUILayout.BeginHorizontal((GUIStyle) "Toolbar", new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(this.m_ParticleEffectUI == null))
      {
        if (!EditorApplication.isPlaying)
        {
          bool flag = false;
          if (this.m_ParticleEffectUI != null)
            flag = this.m_ParticleEffectUI.IsPlaying();
          if (GUILayout.Button(!flag ? ParticleEffectUI.texts.play : ParticleEffectUI.texts.pause, (GUIStyle) "ToolbarButton", new GUILayoutOption[1]{ GUILayout.Width(65f) }))
          {
            if (this.m_ParticleEffectUI != null)
            {
              if (flag)
                this.m_ParticleEffectUI.Pause();
              else
                this.m_ParticleEffectUI.Play();
            }
            this.Repaint();
          }
          if (GUILayout.Button(ParticleEffectUI.texts.stop, (GUIStyle) "ToolbarButton", new GUILayoutOption[0]) && this.m_ParticleEffectUI != null)
            this.m_ParticleEffectUI.Stop();
        }
        else
        {
          if (GUILayout.Button(ParticleEffectUI.texts.play, (GUIStyle) "ToolbarButton", new GUILayoutOption[1]{ GUILayout.Width(65f) }) && this.m_ParticleEffectUI != null)
          {
            this.m_ParticleEffectUI.Stop();
            this.m_ParticleEffectUI.Play();
          }
          if (GUILayout.Button(ParticleEffectUI.texts.stop, (GUIStyle) "ToolbarButton", new GUILayoutOption[0]) && this.m_ParticleEffectUI != null)
            this.m_ParticleEffectUI.Stop();
        }
        GUILayout.FlexibleSpace();
        bool flag1 = this.m_ParticleEffectUI != null && this.m_ParticleEffectUI.IsShowOnlySelectedMode();
        bool enable = GUILayout.Toggle((flag1 ? 1 : 0) != 0, !flag1 ? "Show: All" : "Show: Selected", ParticleSystemStyles.Get().toolbarButtonLeftAlignText, new GUILayoutOption[1]{ GUILayout.Width(100f) });
        if (enable != flag1 && this.m_ParticleEffectUI != null)
          this.m_ParticleEffectUI.SetShowOnlySelectedMode(enable);
        ParticleSystemEditorUtils.editorResimulation = GUILayout.Toggle(ParticleSystemEditorUtils.editorResimulation, ParticleEffectUI.texts.resimulation, (GUIStyle) "ToolbarButton", new GUILayoutOption[0]);
        ParticleEffectUI.m_ShowBounds = GUILayout.Toggle(ParticleEffectUI.m_ShowBounds, ParticleEffectUI.texts.showBounds, (GUIStyle) "ToolbarButton", new GUILayoutOption[0]);
        if (GUILayout.Button(!ParticleEffectUI.m_VerticalLayout ? ParticleSystemWindow.s_Icons[1] : ParticleSystemWindow.s_Icons[0], (GUIStyle) "ToolbarButton", new GUILayoutOption[0]))
        {
          ParticleEffectUI.m_VerticalLayout = !ParticleEffectUI.m_VerticalLayout;
          EditorPrefs.SetBool("ShurikenVerticalLayout", ParticleEffectUI.m_VerticalLayout);
          this.Clear();
        }
        GUILayout.BeginVertical();
        GUILayout.Space(3f);
        bool flag2 = (UnityEngine.Object) ParticleSystemEditorUtils.lockedParticleSystem != (UnityEngine.Object) null;
        bool flag3 = GUILayout.Toggle(flag2, ParticleSystemWindow.s_Texts.lockParticleSystem, (GUIStyle) "IN LockButton", new GUILayoutOption[0]);
        if (flag2 != flag3 && this.m_ParticleEffectUI != null && (UnityEngine.Object) this.m_Target != (UnityEngine.Object) null)
          ParticleSystemEditorUtils.lockedParticleSystem = !flag3 ? (ParticleSystem) null : this.m_Target;
        GUILayout.EndVertical();
      }
      GUILayout.EndHorizontal();
    }

    private void OnGUI()
    {
      if (ParticleSystemWindow.s_Texts == null)
        ParticleSystemWindow.s_Texts = new ParticleSystemWindow.Texts();
      if (ParticleSystemWindow.s_Icons == null)
        ParticleSystemWindow.s_Icons = new GUIContent[2]
        {
          EditorGUIUtility.IconContent("HorizontalSplit"),
          EditorGUIUtility.IconContent("VerticalSplit")
        };
      if ((UnityEngine.Object) this.m_Target == (UnityEngine.Object) null && ((UnityEngine.Object) Selection.activeGameObject != (UnityEngine.Object) null || (UnityEngine.Object) ParticleSystemEditorUtils.lockedParticleSystem != (UnityEngine.Object) null))
        this.InitEffectUI();
      this.DoToolbarGUI();
      if (!((UnityEngine.Object) this.m_Target != (UnityEngine.Object) null) || this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.OnGUI();
    }

    public void OnSceneViewGUI(SceneView sceneView)
    {
      if (!this.m_IsVisible || this.m_ParticleEffectUI == null)
        return;
      this.m_ParticleEffectUI.OnSceneViewGUI();
    }

    private void OnDidOpenScene()
    {
      this.Repaint();
    }

    private class Texts
    {
      public GUIContent lockParticleSystem = new GUIContent("", "Lock the current selected Particle System");
      public GUIContent previewAll = new GUIContent("Simulate All", "Simulate all particle systems that have Play On Awake set");
    }
  }
}
