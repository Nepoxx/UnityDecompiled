// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleEffectUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class ParticleEffectUI
  {
    public static bool m_ShowBounds = false;
    private static readonly Vector2 k_MinEmitterAreaSize = new Vector2(125f, 100f);
    private static readonly Vector2 k_MinCurveAreaSize = new Vector2(100f, 100f);
    private static readonly Color k_DarkSkinDisabledColor = new Color(0.66f, 0.66f, 0.66f, 0.95f);
    private static readonly Color k_LightSkinDisabledColor = new Color(0.84f, 0.84f, 0.84f, 0.95f);
    private static PrefKey kPlay = new PrefKey("ParticleSystem/Play", ",");
    private static PrefKey kStop = new PrefKey("ParticleSystem/Stop", ".");
    private static PrefKey kForward = new PrefKey("ParticleSystem/Forward", "m");
    private static PrefKey kReverse = new PrefKey("ParticleSystem/Reverse", "n");
    private TimeHelper m_TimeHelper = new TimeHelper();
    private float m_EmitterAreaWidth = 230f;
    private float m_CurveEditorAreaHeight = 330f;
    private Vector2 m_EmitterAreaScrollPos = Vector2.zero;
    private int m_IsDraggingTimeHotControlID = -1;
    public ParticleEffectUIOwner m_Owner;
    public ParticleSystemUI[] m_Emitters;
    private bool m_EmittersActiveInHierarchy;
    private ParticleSystemCurveEditor m_ParticleSystemCurveEditor;
    private List<ParticleSystem> m_SelectedParticleSystems;
    private bool m_ShowOnlySelectedMode;
    public static ParticleSystem m_MainPlaybackSystem;
    public static bool m_VerticalLayout;
    private const string k_SimulationStateId = "SimulationState";
    private const string k_ShowSelectedId = "ShowSelected";
    private static ParticleEffectUI.Texts s_Texts;

    public ParticleEffectUI(ParticleEffectUIOwner owner)
    {
      this.m_Owner = owner;
    }

    internal static ParticleEffectUI.Texts texts
    {
      get
      {
        if (ParticleEffectUI.s_Texts == null)
          ParticleEffectUI.s_Texts = new ParticleEffectUI.Texts();
        return ParticleEffectUI.s_Texts;
      }
    }

    public bool multiEdit
    {
      get
      {
        return this.m_SelectedParticleSystems != null && this.m_SelectedParticleSystems.Count > 1;
      }
    }

    private bool ShouldManagePlaybackState(ParticleSystem root)
    {
      bool flag = false;
      if ((UnityEngine.Object) root != (UnityEngine.Object) null)
        flag = root.gameObject.activeInHierarchy;
      return !EditorApplication.isPlaying && flag;
    }

    private static Color GetDisabledColor()
    {
      return EditorGUIUtility.isProSkin ? ParticleEffectUI.k_DarkSkinDisabledColor : ParticleEffectUI.k_LightSkinDisabledColor;
    }

    internal static ParticleSystem[] GetParticleSystems(ParticleSystem root)
    {
      List<ParticleSystem> particleSystems = new List<ParticleSystem>();
      particleSystems.Add(root);
      ParticleEffectUI.GetDirectParticleSystemChildrenRecursive(root.transform, particleSystems);
      return particleSystems.ToArray();
    }

    private static void GetDirectParticleSystemChildrenRecursive(Transform transform, List<ParticleSystem> particleSystems)
    {
      IEnumerator enumerator = transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          ParticleSystem component = current.gameObject.GetComponent<ParticleSystem>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            particleSystems.Add(component);
            ParticleEffectUI.GetDirectParticleSystemChildrenRecursive(current, particleSystems);
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    public bool InitializeIfNeeded(IEnumerable<ParticleSystem> systems)
    {
      bool flag1 = false;
      ParticleSystem[] array = systems.ToArray<ParticleSystem>();
      bool flag2 = ((IEnumerable<ParticleSystem>) array).Count<ParticleSystem>() > 1;
      bool flag3 = false;
      ParticleSystem root1 = (ParticleSystem) null;
      foreach (ParticleSystem ps in array)
      {
        ParticleSystem[] particleSystemArray;
        if (!flag2)
        {
          ParticleSystem root2 = ParticleSystemEditorUtils.GetRoot(ps);
          if (!((UnityEngine.Object) root2 == (UnityEngine.Object) null))
          {
            particleSystemArray = ParticleEffectUI.GetParticleSystems(root2);
            root1 = root2;
            if (this.m_SelectedParticleSystems != null && this.m_SelectedParticleSystems.Count > 0 && ((UnityEngine.Object) root2 == (UnityEngine.Object) ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystems[0]) && this.m_ParticleSystemCurveEditor != null) && (this.m_Emitters != null && particleSystemArray.Length == this.m_Emitters.Length && ps.gameObject.activeInHierarchy == this.m_EmittersActiveInHierarchy))
            {
              this.m_SelectedParticleSystems = new List<ParticleSystem>();
              this.m_SelectedParticleSystems.Add(ps);
              if (this.IsShowOnlySelectedMode())
              {
                this.RefreshShowOnlySelected();
                continue;
              }
              continue;
            }
          }
          else
            continue;
        }
        else
        {
          particleSystemArray = new ParticleSystem[1]{ ps };
          root1 = ps;
        }
        if (this.m_ParticleSystemCurveEditor != null)
          this.Clear();
        flag3 = true;
        if (!flag1)
        {
          this.m_SelectedParticleSystems = new List<ParticleSystem>();
          flag1 = true;
        }
        this.m_SelectedParticleSystems.Add(ps);
        if (!flag2)
        {
          this.m_ParticleSystemCurveEditor = new ParticleSystemCurveEditor();
          this.m_ParticleSystemCurveEditor.Init();
          int length = particleSystemArray.Length;
          if (length > 0)
          {
            this.m_Emitters = new ParticleSystemUI[length];
            for (int index = 0; index < length; ++index)
            {
              this.m_Emitters[index] = new ParticleSystemUI();
              this.m_Emitters[index].Init(this, new ParticleSystem[1]
              {
                particleSystemArray[index]
              });
            }
            this.m_EmittersActiveInHierarchy = ps.gameObject.activeInHierarchy;
          }
        }
      }
      if (flag3)
      {
        if (flag2)
        {
          this.m_ParticleSystemCurveEditor = new ParticleSystemCurveEditor();
          this.m_ParticleSystemCurveEditor.Init();
          if (this.m_SelectedParticleSystems.Count > 0)
          {
            this.m_Emitters = new ParticleSystemUI[1];
            this.m_Emitters[0] = new ParticleSystemUI();
            this.m_Emitters[0].Init(this, this.m_SelectedParticleSystems.ToArray());
            this.m_EmittersActiveInHierarchy = this.m_SelectedParticleSystems[0].gameObject.activeInHierarchy;
          }
        }
        foreach (ParticleSystemUI emitter in this.m_Emitters)
        {
          foreach (ModuleUI module in emitter.m_Modules)
          {
            if (module != null)
              module.Validate();
          }
        }
        if (ParticleEffectUI.GetAllModulesVisible())
          this.SetAllModulesVisible(true);
        this.m_EmitterAreaWidth = EditorPrefs.GetFloat("ParticleSystemEmitterAreaWidth", ParticleEffectUI.k_MinEmitterAreaSize.x);
        this.m_CurveEditorAreaHeight = EditorPrefs.GetFloat("ParticleSystemCurveEditorAreaHeight", ParticleEffectUI.k_MinCurveAreaSize.y);
        this.SetShowOnlySelectedMode(this.m_Owner is ParticleSystemWindow && SessionState.GetBool("ShowSelected" + (object) root1.GetInstanceID(), false));
        this.m_EmitterAreaScrollPos.x = SessionState.GetFloat("CurrentEmitterAreaScroll", 0.0f);
        if (this.ShouldManagePlaybackState(root1))
        {
          Vector3 vector3 = SessionState.GetVector3("SimulationState" + (object) root1.GetInstanceID(), Vector3.zero);
          if (root1.GetInstanceID() == (int) vector3.x)
          {
            float z = vector3.z;
            if ((double) z > 0.0)
              ParticleSystemEditorUtils.editorPlaybackTime = z;
          }
          if ((UnityEngine.Object) ParticleEffectUI.m_MainPlaybackSystem != (UnityEngine.Object) root1)
          {
            ParticleSystemEditorUtils.PerformCompleteResimulation();
            this.Play();
          }
        }
      }
      ParticleEffectUI.m_MainPlaybackSystem = root1;
      return flag3;
    }

    internal void UndoRedoPerformed()
    {
      this.Refresh();
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        foreach (ModuleUI module in emitter.m_Modules)
        {
          if (module != null)
          {
            module.CheckVisibilityState();
            if (module.foldout)
              module.UndoRedoPerformed();
          }
        }
      }
      this.m_Owner.Repaint();
    }

    public void Clear()
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystems[0]);
      if (this.ShouldManagePlaybackState(root) && (UnityEngine.Object) root != (UnityEngine.Object) null)
      {
        ParticleEffectUI.PlayState playState = !this.IsPlaying() ? (!this.IsPaused() ? ParticleEffectUI.PlayState.Stopped : ParticleEffectUI.PlayState.Paused) : ParticleEffectUI.PlayState.Playing;
        int instanceId = root.GetInstanceID();
        SessionState.SetVector3("SimulationState" + (object) instanceId, new Vector3((float) instanceId, (float) playState, ParticleSystemEditorUtils.editorPlaybackTime));
      }
      this.m_ParticleSystemCurveEditor.OnDisable();
      Tools.s_Hidden = false;
      if ((UnityEngine.Object) root != (UnityEngine.Object) null)
        SessionState.SetBool("ShowSelected" + (object) root.GetInstanceID(), this.m_ShowOnlySelectedMode);
      this.SetShowOnlySelectedMode(false);
      GameView.RepaintAll();
      SceneView.RepaintAll();
    }

    public static Vector2 GetMinSize()
    {
      return ParticleEffectUI.k_MinEmitterAreaSize + ParticleEffectUI.k_MinCurveAreaSize;
    }

    public void Refresh()
    {
      this.UpdateProperties();
      this.m_ParticleSystemCurveEditor.Refresh();
    }

    public string GetNextParticleSystemName()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ParticleEffectUI.\u003CGetNextParticleSystemName\u003Ec__AnonStorey0 nameCAnonStorey0 = new ParticleEffectUI.\u003CGetNextParticleSystemName\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      nameCAnonStorey0.nextName = "";
      for (int index = 2; index < 50; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        nameCAnonStorey0.nextName = "Particle System " + (object) index;
        bool flag = false;
        foreach (ParticleSystemUI emitter in this.m_Emitters)
        {
          // ISSUE: reference to a compiler-generated method
          if ((UnityEngine.Object) ((IEnumerable<ParticleSystem>) emitter.m_ParticleSystems).FirstOrDefault<ParticleSystem>(new Func<ParticleSystem, bool>(nameCAnonStorey0.\u003C\u003Em__0)) != (UnityEngine.Object) null)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated field
          return nameCAnonStorey0.nextName;
        }
      }
      return "Particle System";
    }

    public bool IsParticleSystemUIVisible(ParticleSystemUI psUI)
    {
      if ((!(this.m_Owner is ParticleSystemInspector) ? 1 : 0) == 1)
        return true;
      foreach (ParticleSystem particleSystem in psUI.m_ParticleSystems)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        if ((UnityEngine.Object) this.m_SelectedParticleSystems.FirstOrDefault<ParticleSystem>(new Func<ParticleSystem, bool>(new ParticleEffectUI.\u003CIsParticleSystemUIVisible\u003Ec__AnonStorey1() { ps = particleSystem }.\u003C\u003Em__0)) != (UnityEngine.Object) null)
          return true;
      }
      return false;
    }

    public void PlayOnAwakeChanged(bool newPlayOnAwake)
    {
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        (emitter.m_Modules[0] as InitialModuleUI).m_PlayOnAwake.boolValue = newPlayOnAwake;
        emitter.ApplyProperties();
      }
    }

    public GameObject CreateParticleSystem(ParticleSystem parentOfNewParticleSystem, SubModuleUI.SubEmitterType defaultType)
    {
      GameObject gameObject = new GameObject(this.GetNextParticleSystemName(), new System.Type[1]{ typeof (ParticleSystem) });
      if (!(bool) ((UnityEngine.Object) gameObject))
        return (GameObject) null;
      if ((bool) ((UnityEngine.Object) parentOfNewParticleSystem))
        gameObject.transform.parent = parentOfNewParticleSystem.transform;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localRotation = Quaternion.identity;
      ParticleSystem component1 = gameObject.GetComponent<ParticleSystem>();
      if (defaultType != SubModuleUI.SubEmitterType.None)
        component1.SetupDefaultType((int) defaultType);
      SessionState.SetFloat("CurrentEmitterAreaScroll", this.m_EmitterAreaScrollPos.x);
      ParticleSystemRenderer component2 = gameObject.GetComponent<ParticleSystemRenderer>();
      Material material = (Material) null;
      if ((UnityEngine.Object) GraphicsSettings.renderPipelineAsset != (UnityEngine.Object) null)
        material = GraphicsSettings.renderPipelineAsset.GetDefaultParticleMaterial();
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
      component2.materials = new Material[2]
      {
        material,
        null
      };
      Undo.RegisterCreatedObjectUndo((UnityEngine.Object) gameObject, "Create ParticleSystem");
      return gameObject;
    }

    public ParticleSystemCurveEditor GetParticleSystemCurveEditor()
    {
      return this.m_ParticleSystemCurveEditor;
    }

    private void SceneViewGUICallback(UnityEngine.Object target, SceneView sceneView)
    {
      this.PlayStopGUI();
    }

    public void OnSceneViewGUI()
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystems[0]);
      if ((bool) ((UnityEngine.Object) root) && root.gameObject.activeInHierarchy)
        SceneViewOverlay.Window(ParticleSystemInspector.playBackTitle, new SceneViewOverlay.WindowFunction(this.SceneViewGUICallback), 600, SceneViewOverlay.WindowDisplayOption.OneWindowPerTitle);
      foreach (ParticleSystemUI emitter in this.m_Emitters)
        emitter.OnSceneViewGUI();
    }

    internal void PlayBackInfoGUI(bool isPlayMode)
    {
      EventType type = Event.current.type;
      int hotControl = GUIUtility.hotControl;
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUIUtility.labelWidth = 110f;
      if (!isPlayMode)
      {
        EditorGUI.kFloatFieldFormatString = ParticleEffectUI.s_Texts.secondsFloatFieldFormatString;
        if ((double) Time.timeScale == 0.0)
        {
          using (new EditorGUI.DisabledScope(true))
          {
            double num = (double) EditorGUILayout.FloatField(ParticleEffectUI.s_Texts.previewSpeedDisabled, 0.0f, new GUILayoutOption[0]);
          }
        }
        else
          ParticleSystemEditorUtils.editorSimulationSpeed = Mathf.Clamp(EditorGUILayout.FloatField(ParticleEffectUI.s_Texts.previewSpeed, ParticleSystemEditorUtils.editorSimulationSpeed, new GUILayoutOption[0]), 0.0f, 10f);
        EditorGUI.kFloatFieldFormatString = fieldFormatString;
        EditorGUI.BeginChangeCheck();
        EditorGUI.kFloatFieldFormatString = ParticleEffectUI.s_Texts.secondsFloatFieldFormatString;
        float a = EditorGUILayout.FloatField(ParticleEffectUI.s_Texts.previewTime, ParticleSystemEditorUtils.editorPlaybackTime, new GUILayoutOption[0]);
        EditorGUI.kFloatFieldFormatString = fieldFormatString;
        if (EditorGUI.EndChangeCheck())
        {
          if (type == EventType.MouseDrag)
          {
            ParticleSystemEditorUtils.editorIsScrubbing = true;
            float editorSimulationSpeed = ParticleSystemEditorUtils.editorSimulationSpeed;
            float editorPlaybackTime = ParticleSystemEditorUtils.editorPlaybackTime;
            float num = a - editorPlaybackTime;
            a = editorPlaybackTime + num * (0.05f * editorSimulationSpeed);
          }
          ParticleSystemEditorUtils.editorPlaybackTime = Mathf.Max(a, 0.0f);
          foreach (ParticleSystem selectedParticleSystem in this.m_SelectedParticleSystems)
          {
            ParticleSystem root = ParticleSystemEditorUtils.GetRoot(selectedParticleSystem);
            if (root.isStopped)
            {
              root.Play();
              root.Pause();
            }
          }
          ParticleSystemEditorUtils.PerformCompleteResimulation();
        }
        if (type == EventType.MouseDown && GUIUtility.hotControl != hotControl)
        {
          this.m_IsDraggingTimeHotControlID = GUIUtility.hotControl;
          ParticleSystemEditorUtils.editorIsScrubbing = true;
        }
        if (this.m_IsDraggingTimeHotControlID != -1 && GUIUtility.hotControl != this.m_IsDraggingTimeHotControlID)
        {
          this.m_IsDraggingTimeHotControlID = -1;
          ParticleSystemEditorUtils.editorIsScrubbing = false;
        }
      }
      int particleCount1 = 0;
      float fastestParticle = 0.0f;
      float slowestParticle = float.PositiveInfinity;
      foreach (ParticleSystem selectedParticleSystem in this.m_SelectedParticleSystems)
        selectedParticleSystem.CalculateEffectUIData(ref particleCount1, ref fastestParticle, ref slowestParticle);
      EditorGUILayout.LabelField(ParticleEffectUI.s_Texts.particleCount, GUIContent.Temp(particleCount1.ToString()), new GUILayoutOption[0]);
      bool flag = false;
      int num1 = 0;
      foreach (ParticleSystem selectedParticleSystem in this.m_SelectedParticleSystems)
      {
        int particleCount2 = 0;
        if (selectedParticleSystem.CalculateEffectUISubEmitterData(ref particleCount2, ref fastestParticle, ref slowestParticle))
        {
          flag = true;
          num1 += particleCount2;
        }
      }
      if (flag)
        EditorGUILayout.LabelField(ParticleEffectUI.s_Texts.subEmitterParticleCount, GUIContent.Temp(num1.ToString()), new GUILayoutOption[0]);
      if ((double) fastestParticle >= (double) slowestParticle)
        EditorGUILayout.LabelField(ParticleEffectUI.s_Texts.particleSpeeds, GUIContent.Temp(slowestParticle.ToString(ParticleEffectUI.s_Texts.speedFloatFieldFormatString) + " - " + fastestParticle.ToString(ParticleEffectUI.s_Texts.speedFloatFieldFormatString)), new GUILayoutOption[0]);
      else
        EditorGUILayout.LabelField(ParticleEffectUI.s_Texts.particleSpeeds, GUIContent.Temp("0.0 - 0.0"), new GUILayoutOption[0]);
      if (!EditorApplication.isPlaying)
      {
        int editorPreviewLayers = (int) ParticleSystemEditorUtils.editorPreviewLayers;
        GUIContent previewLayers = ParticleEffectUI.s_Texts.previewLayers;
        // ISSUE: reference to a compiler-generated field
        if (ParticleEffectUI.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ParticleEffectUI.\u003C\u003Ef__mg\u0024cache0 = new EditorUtility.SelectMenuItemFunction(ParticleEffectUI.SetPreviewLayersDelegate);
        }
        // ISSUE: reference to a compiler-generated field
        EditorUtility.SelectMenuItemFunction fMgCache0 = ParticleEffectUI.\u003C\u003Ef__mg\u0024cache0;
        GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[0];
        EditorGUILayout.LayerMaskField((uint) editorPreviewLayers, previewLayers, fMgCache0, guiLayoutOptionArray);
        ParticleSystemEditorUtils.editorResimulation = GUILayout.Toggle(ParticleSystemEditorUtils.editorResimulation, ParticleEffectUI.s_Texts.resimulation, EditorStyles.toggle, new GUILayoutOption[0]);
      }
      ParticleEffectUI.m_ShowBounds = GUILayout.Toggle(ParticleEffectUI.m_ShowBounds, ParticleEffectUI.texts.showBounds, EditorStyles.toggle, new GUILayoutOption[0]);
      EditorGUIUtility.labelWidth = 0.0f;
    }

    internal static void SetPreviewLayersDelegate(object userData, string[] options, int selected)
    {
      ParticleSystemEditorUtils.editorPreviewLayers = SerializedProperty.ToggleLayerMask(((Tuple<SerializedProperty, uint>) userData).Item2, selected);
    }

    private void HandleKeyboardShortcuts()
    {
      Event current = Event.current;
      if (current.type == EventType.KeyDown)
      {
        int num = 0;
        if (current.keyCode == (Event) ParticleEffectUI.kPlay.keyCode)
        {
          if (EditorApplication.isPlaying)
          {
            this.Stop();
            this.Play();
          }
          else if (!ParticleSystemEditorUtils.editorIsPlaying)
            this.Play();
          else
            this.Pause();
          current.Use();
        }
        else if (current.keyCode == (Event) ParticleEffectUI.kStop.keyCode)
        {
          this.Stop();
          current.Use();
        }
        else if (current.keyCode == (Event) ParticleEffectUI.kReverse.keyCode)
          num = -1;
        else if (current.keyCode == (Event) ParticleEffectUI.kForward.keyCode)
          num = 1;
        if (num != 0)
        {
          ParticleSystemEditorUtils.editorIsScrubbing = true;
          float editorSimulationSpeed = ParticleSystemEditorUtils.editorSimulationSpeed;
          ParticleSystemEditorUtils.editorPlaybackTime = Mathf.Max(0.0f, ParticleSystemEditorUtils.editorPlaybackTime + (float) ((!current.shift ? 1.0 : 3.0) * (double) this.m_TimeHelper.deltaTime * (num <= 0 ? -3.0 : 3.0)) * editorSimulationSpeed);
          foreach (ParticleSystem selectedParticleSystem in this.m_SelectedParticleSystems)
          {
            ParticleSystem root = ParticleSystemEditorUtils.GetRoot(selectedParticleSystem);
            if (root.isStopped)
            {
              root.Play();
              root.Pause();
            }
          }
          ParticleSystemEditorUtils.PerformCompleteResimulation();
          current.Use();
        }
      }
      if (current.type != EventType.KeyUp || current.keyCode != (Event) ParticleEffectUI.kReverse.keyCode && current.keyCode != (Event) ParticleEffectUI.kForward.keyCode)
        return;
      ParticleSystemEditorUtils.editorIsScrubbing = false;
    }

    internal static bool IsStopped(ParticleSystem root)
    {
      return !ParticleSystemEditorUtils.editorIsPlaying && !ParticleSystemEditorUtils.editorIsPaused && !ParticleSystemEditorUtils.editorIsScrubbing;
    }

    internal bool IsPaused()
    {
      return !this.IsPlaying() && !ParticleEffectUI.IsStopped(ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystems[0]));
    }

    internal bool IsPlaying()
    {
      return ParticleSystemEditorUtils.editorIsPlaying;
    }

    internal void Play()
    {
      bool flag = false;
      foreach (ParticleSystem selectedParticleSystem in this.m_SelectedParticleSystems)
      {
        ParticleSystem root = ParticleSystemEditorUtils.GetRoot(selectedParticleSystem);
        if ((bool) ((UnityEngine.Object) root))
        {
          root.Play();
          flag = true;
        }
      }
      if (!flag)
        return;
      ParticleSystemEditorUtils.editorIsScrubbing = false;
      this.m_Owner.Repaint();
    }

    internal void Pause()
    {
      bool flag = false;
      foreach (ParticleSystem selectedParticleSystem in this.m_SelectedParticleSystems)
      {
        ParticleSystem root = ParticleSystemEditorUtils.GetRoot(selectedParticleSystem);
        if ((bool) ((UnityEngine.Object) root))
        {
          root.Pause();
          flag = true;
        }
      }
      if (!flag)
        return;
      ParticleSystemEditorUtils.editorIsScrubbing = true;
      this.m_Owner.Repaint();
    }

    internal void Stop()
    {
      ParticleSystemEditorUtils.editorIsScrubbing = false;
      ParticleSystemEditorUtils.editorPlaybackTime = 0.0f;
      ParticleSystemEditorUtils.StopEffect();
      this.m_Owner.Repaint();
    }

    internal void PlayStopGUI()
    {
      if (ParticleEffectUI.s_Texts == null)
        ParticleEffectUI.s_Texts = new ParticleEffectUI.Texts();
      if (Event.current.type == EventType.Layout)
      {
        double num = (double) this.m_TimeHelper.Update();
      }
      bool disabled = (double) Time.timeScale == 0.0;
      GUIContent content = !disabled ? ParticleEffectUI.s_Texts.play : ParticleEffectUI.s_Texts.playDisabled;
      if (!EditorApplication.isPlaying)
      {
        GUILayout.BeginHorizontal(GUILayout.Width(210f));
        using (new EditorGUI.DisabledScope(disabled))
        {
          bool flag = ParticleSystemEditorUtils.editorIsPlaying && !ParticleSystemEditorUtils.editorIsPaused && !disabled;
          if (GUILayout.Button(!flag ? content : ParticleEffectUI.s_Texts.pause, (GUIStyle) "ButtonLeft", new GUILayoutOption[0]))
          {
            if (flag)
              this.Pause();
            else
              this.Play();
          }
        }
        if (GUILayout.Button(ParticleEffectUI.s_Texts.restart, (GUIStyle) "ButtonMid", new GUILayoutOption[0]))
        {
          this.Stop();
          this.Play();
        }
        if (GUILayout.Button(ParticleEffectUI.s_Texts.stop, (GUIStyle) "ButtonRight", new GUILayoutOption[0]))
          this.Stop();
        GUILayout.EndHorizontal();
      }
      else
      {
        GUILayout.BeginHorizontal();
        using (new EditorGUI.DisabledScope(disabled))
        {
          if (GUILayout.Button(content))
          {
            this.Stop();
            this.Play();
          }
        }
        if (GUILayout.Button(ParticleEffectUI.s_Texts.stop))
          this.Stop();
        GUILayout.EndHorizontal();
      }
      this.PlayBackInfoGUI(EditorApplication.isPlaying);
      this.HandleKeyboardShortcuts();
    }

    private void InspectorParticleSystemGUI()
    {
      GUILayout.BeginVertical(ParticleSystemStyles.Get().effectBgStyle, new GUILayoutOption[0]);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ParticleEffectUI.\u003CInspectorParticleSystemGUI\u003Ec__AnonStorey2 systemGuiCAnonStorey2 = new ParticleEffectUI.\u003CInspectorParticleSystemGUI\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      systemGuiCAnonStorey2.selectedSystem = this.m_SelectedParticleSystems.Count <= 0 ? (ParticleSystem) null : this.m_SelectedParticleSystems[0];
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) systemGuiCAnonStorey2.selectedSystem != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        ParticleSystemUI particleSystemUi = ((IEnumerable<ParticleSystemUI>) this.m_Emitters).FirstOrDefault<ParticleSystemUI>(new Func<ParticleSystemUI, bool>(systemGuiCAnonStorey2.\u003C\u003Em__0));
        if (particleSystemUi != null)
        {
          float width = GUIClip.visibleRect.width - 18f;
          particleSystemUi.OnGUI(width, false);
        }
      }
      GUILayout.EndVertical();
      GUILayout.FlexibleSpace();
      this.HandleKeyboardShortcuts();
    }

    private void DrawSelectionMarker(Rect rect)
    {
      ++rect.x;
      ++rect.y;
      rect.width -= 2f;
      rect.height -= 2f;
      ParticleSystemStyles.Get().selectionMarker.Draw(rect, GUIContent.none, false, true, true, false);
    }

    private List<ParticleSystemUI> GetSelectedParticleSystemUIs()
    {
      List<ParticleSystemUI> particleSystemUiList = new List<ParticleSystemUI>();
      int[] instanceIds = Selection.instanceIDs;
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        if (((IEnumerable<int>) instanceIds).Contains<int>(emitter.m_ParticleSystems[0].gameObject.GetInstanceID()))
          particleSystemUiList.Add(emitter);
      }
      return particleSystemUiList;
    }

    private void MultiParticleSystemGUI(bool verticalLayout)
    {
      GUILayout.BeginVertical(ParticleSystemStyles.Get().effectBgStyle, new GUILayoutOption[0]);
      this.m_EmitterAreaScrollPos = EditorGUILayout.BeginScrollView(this.m_EmitterAreaScrollPos);
      Rect position = EditorGUILayout.BeginVertical();
      this.m_EmitterAreaScrollPos -= EditorGUI.MouseDeltaReader(position, Event.current.alt);
      GUILayout.Space(3f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(3f);
      Color color = GUI.color;
      bool flag1 = Event.current.type == EventType.Repaint;
      bool flag2 = this.IsShowOnlySelectedMode();
      List<ParticleSystemUI> particleSystemUis = this.GetSelectedParticleSystemUIs();
      for (int index = 0; index < this.m_Emitters.Length; ++index)
      {
        if (index != 0)
          GUILayout.Space(ModuleUI.k_SpaceBetweenModules);
        bool flag3 = particleSystemUis.Contains(this.m_Emitters[index]);
        ModuleUI rendererModuleUi = this.m_Emitters[index].GetParticleSystemRendererModuleUI();
        if (flag1 && rendererModuleUi != null && !rendererModuleUi.enabled)
          GUI.color = ParticleEffectUI.GetDisabledColor();
        if (flag1 && flag2 && !flag3)
          GUI.color = ParticleEffectUI.GetDisabledColor();
        Rect rect = EditorGUILayout.BeginVertical();
        if (flag1 && flag3 && this.m_Emitters.Length > 1)
          this.DrawSelectionMarker(rect);
        this.m_Emitters[index].OnGUI(ModuleUI.k_CompactFixedModuleWidth, true);
        EditorGUILayout.EndVertical();
        GUI.color = color;
      }
      GUILayout.Space(5f);
      if (GUILayout.Button(ParticleEffectUI.s_Texts.addParticleSystem, (GUIStyle) "OL Plus", new GUILayoutOption[1]{ GUILayout.Width(20f) }))
        this.CreateParticleSystem(ParticleSystemEditorUtils.GetRoot(this.m_SelectedParticleSystems[0]), SubModuleUI.SubEmitterType.None);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.Space(4f);
      this.m_EmitterAreaScrollPos -= EditorGUI.MouseDeltaReader(position, true);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();
      GUILayout.EndVertical();
      this.HandleKeyboardShortcuts();
    }

    private void WindowCurveEditorGUI(bool verticalLayout)
    {
      Rect rect;
      if (verticalLayout)
      {
        rect = GUILayoutUtility.GetRect(13f, this.m_CurveEditorAreaHeight, new GUILayoutOption[1]
        {
          GUILayout.MinHeight(this.m_CurveEditorAreaHeight)
        });
      }
      else
      {
        EditorWindow owner = (EditorWindow) this.m_Owner;
        rect = GUILayoutUtility.GetRect(owner.position.width - this.m_EmitterAreaWidth, owner.position.height - 17f);
      }
      this.ResizeHandling(verticalLayout);
      this.m_ParticleSystemCurveEditor.OnGUI(rect);
    }

    private void ResizeHandling(bool verticalLayout)
    {
      if (verticalLayout)
      {
        Rect lastRect = GUILayoutUtility.GetLastRect();
        lastRect.y += -5f;
        lastRect.height = 5f;
        float y = EditorGUI.MouseDeltaReader(lastRect, true).y;
        if ((double) y != 0.0)
        {
          this.m_CurveEditorAreaHeight -= y;
          this.ClampWindowContentSizes();
          EditorPrefs.SetFloat("ParticleSystemCurveEditorAreaHeight", this.m_CurveEditorAreaHeight);
        }
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.SplitResizeUpDown);
      }
      else
      {
        Rect position = new Rect(this.m_EmitterAreaWidth - 5f, 0.0f, 5f, GUIClip.visibleRect.height);
        float x = EditorGUI.MouseDeltaReader(position, true).x;
        if ((double) x != 0.0)
        {
          this.m_EmitterAreaWidth += x;
          this.ClampWindowContentSizes();
          EditorPrefs.SetFloat("ParticleSystemEmitterAreaWidth", this.m_EmitterAreaWidth);
        }
        if (Event.current.type == EventType.Repaint)
          EditorGUIUtility.AddCursorRect(position, MouseCursor.SplitResizeLeftRight);
      }
    }

    private void ClampWindowContentSizes()
    {
      if (Event.current.type == EventType.Layout)
        return;
      float width = GUIClip.visibleRect.width;
      float height = GUIClip.visibleRect.height;
      if (ParticleEffectUI.m_VerticalLayout)
        this.m_CurveEditorAreaHeight = Mathf.Clamp(this.m_CurveEditorAreaHeight, ParticleEffectUI.k_MinCurveAreaSize.y, height - ParticleEffectUI.k_MinEmitterAreaSize.y);
      else
        this.m_EmitterAreaWidth = Mathf.Clamp(this.m_EmitterAreaWidth, ParticleEffectUI.k_MinEmitterAreaSize.x, width - ParticleEffectUI.k_MinCurveAreaSize.x);
    }

    public void OnGUI()
    {
      if (ParticleEffectUI.s_Texts == null)
        ParticleEffectUI.s_Texts = new ParticleEffectUI.Texts();
      if (this.m_Emitters == null)
        return;
      this.UpdateProperties();
      switch (!(this.m_Owner is ParticleSystemInspector) ? ParticleEffectUI.OwnerType.ParticleSystemWindow : ParticleEffectUI.OwnerType.Inspector)
      {
        case ParticleEffectUI.OwnerType.Inspector:
          this.InspectorParticleSystemGUI();
          break;
        case ParticleEffectUI.OwnerType.ParticleSystemWindow:
          this.ClampWindowContentSizes();
          bool verticalLayout = ParticleEffectUI.m_VerticalLayout;
          if (verticalLayout)
          {
            this.MultiParticleSystemGUI(verticalLayout);
            this.WindowCurveEditorGUI(verticalLayout);
            break;
          }
          GUILayout.BeginHorizontal();
          this.MultiParticleSystemGUI(verticalLayout);
          this.WindowCurveEditorGUI(verticalLayout);
          GUILayout.EndHorizontal();
          break;
        default:
          Debug.LogError((object) "Unhandled enum");
          break;
      }
      this.ApplyModifiedProperties();
    }

    private void ApplyModifiedProperties()
    {
      for (int index = 0; index < this.m_Emitters.Length; ++index)
        this.m_Emitters[index].ApplyProperties();
    }

    internal void UpdateProperties()
    {
      for (int index = 0; index < this.m_Emitters.Length; ++index)
        this.m_Emitters[index].UpdateProperties();
    }

    internal static bool GetAllModulesVisible()
    {
      return EditorPrefs.GetBool("ParticleSystemShowAllModules", true);
    }

    internal void SetAllModulesVisible(bool showAll)
    {
      EditorPrefs.SetBool("ParticleSystemShowAllModules", showAll);
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        for (int index = 0; index < emitter.m_Modules.Length; ++index)
        {
          ModuleUI module = emitter.m_Modules[index];
          if (module != null)
          {
            if (showAll)
            {
              if (!module.visibleUI)
                module.visibleUI = true;
            }
            else
            {
              bool flag = true;
              if (module is RendererModuleUI && (UnityEngine.Object) ((IEnumerable<ParticleSystem>) emitter.m_ParticleSystems).FirstOrDefault<ParticleSystem>((Func<ParticleSystem, bool>) (o => (UnityEngine.Object) o.GetComponent<ParticleSystemRenderer>() == (UnityEngine.Object) null)) == (UnityEngine.Object) null)
                flag = false;
              if (flag && !module.enabled)
                module.visibleUI = false;
            }
          }
        }
      }
    }

    internal bool IsShowOnlySelectedMode()
    {
      return this.m_ShowOnlySelectedMode;
    }

    internal void SetShowOnlySelectedMode(bool enable)
    {
      this.m_ShowOnlySelectedMode = enable;
      this.RefreshShowOnlySelected();
    }

    internal void RefreshShowOnlySelected()
    {
      int[] instanceIds = Selection.instanceIDs;
      foreach (ParticleSystemUI emitter in this.m_Emitters)
      {
        if ((UnityEngine.Object) emitter.m_ParticleSystems[0] != (UnityEngine.Object) null)
        {
          ParticleSystemRenderer component = emitter.m_ParticleSystems[0].GetComponent<ParticleSystemRenderer>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.editorEnabled = !this.IsShowOnlySelectedMode() || ((IEnumerable<int>) instanceIds).Contains<int>(component.gameObject.GetInstanceID());
        }
      }
    }

    private enum PlayState
    {
      Stopped,
      Playing,
      Paused,
    }

    private enum OwnerType
    {
      Inspector,
      ParticleSystemWindow,
    }

    internal class Texts
    {
      public GUIContent previewSpeed = EditorGUIUtility.TextContent("Playback Speed|Playback Speed is also affected by the Time Scale setting in the Time Manager.");
      public GUIContent previewSpeedDisabled = EditorGUIUtility.TextContent("Playback Speed|Playback Speed is locked to 0.0, because the Time Scale in the Time Manager is set to 0.0.");
      public GUIContent previewTime = EditorGUIUtility.TextContent("Playback Time");
      public GUIContent particleCount = EditorGUIUtility.TextContent("Particles");
      public GUIContent subEmitterParticleCount = EditorGUIUtility.TextContent("Sub Emitter Particles");
      public GUIContent particleSpeeds = EditorGUIUtility.TextContent("Speed Range");
      public GUIContent play = EditorGUIUtility.TextContent("Play");
      public GUIContent playDisabled = EditorGUIUtility.TextContent("Play|Play is disabled, because the Time Scale in the Time Manager is set to 0.0.");
      public GUIContent stop = EditorGUIUtility.TextContent("Stop");
      public GUIContent pause = EditorGUIUtility.TextContent("Pause");
      public GUIContent restart = EditorGUIUtility.TextContent("Restart");
      public GUIContent addParticleSystem = EditorGUIUtility.TextContent("|Create Particle System");
      public GUIContent showBounds = EditorGUIUtility.TextContent("Show Bounds|Show world space bounding boxes.");
      public GUIContent resimulation = EditorGUIUtility.TextContent("Resimulate|If resimulate is enabled, the Particle System will show changes made to the system immediately (including changes made to the Particle System Transform).");
      public GUIContent previewLayers = EditorGUIUtility.TextContent("Simulate Layers|Automatically preview all looping Particle Systems on the chosen layers, in addition to the selected Game Objects.");
      public string secondsFloatFieldFormatString = "f2";
      public string speedFloatFieldFormatString = "f1";
    }
  }
}
