// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class ParticleSystemUI
  {
    public ParticleEffectUI m_ParticleEffectUI;
    public ModuleUI[] m_Modules;
    public ParticleSystem[] m_ParticleSystems;
    public SerializedObject m_ParticleSystemSerializedObject;
    public SerializedObject m_RendererSerializedObject;
    private static string[] s_ModuleNames;
    private string m_SupportsCullingText;
    private string m_SupportsCullingTextLabel;
    private static ParticleSystemUI.Texts s_Texts;

    public bool multiEdit
    {
      get
      {
        return this.m_ParticleSystems != null && this.m_ParticleSystems.Length > 1;
      }
    }

    public void Init(ParticleEffectUI owner, ParticleSystem[] systems)
    {
      if (ParticleSystemUI.s_ModuleNames == null)
        ParticleSystemUI.s_ModuleNames = ParticleSystemUI.GetUIModuleNames();
      if (ParticleSystemUI.s_Texts == null)
        ParticleSystemUI.s_Texts = new ParticleSystemUI.Texts();
      this.m_ParticleEffectUI = owner;
      this.m_ParticleSystems = systems;
      this.m_ParticleSystemSerializedObject = new SerializedObject((UnityEngine.Object[]) this.m_ParticleSystems);
      this.m_RendererSerializedObject = (SerializedObject) null;
      this.m_SupportsCullingText = (string) null;
      this.m_Modules = ParticleSystemUI.CreateUIModules(this, this.m_ParticleSystemSerializedObject);
      if (!((UnityEngine.Object) ((IEnumerable<ParticleSystem>) this.m_ParticleSystems).FirstOrDefault<ParticleSystem>((Func<ParticleSystem, bool>) (o => (UnityEngine.Object) o.GetComponent<ParticleSystemRenderer>() == (UnityEngine.Object) null)) != (UnityEngine.Object) null))
        this.InitRendererUI();
      this.UpdateParticleSystemInfoString();
    }

    internal ModuleUI GetParticleSystemRendererModuleUI()
    {
      return this.m_Modules[this.m_Modules.Length - 1];
    }

    private void InitRendererUI()
    {
      List<ParticleSystemRenderer> particleSystemRendererList = new List<ParticleSystemRenderer>();
      foreach (ParticleSystem particleSystem in this.m_ParticleSystems)
      {
        if ((UnityEngine.Object) particleSystem.GetComponent<ParticleSystemRenderer>() == (UnityEngine.Object) null)
          particleSystem.gameObject.AddComponent<ParticleSystemRenderer>();
        particleSystemRendererList.Add(particleSystem.GetComponent<ParticleSystemRenderer>());
      }
      if (particleSystemRendererList.Count <= 0)
        return;
      this.m_RendererSerializedObject = new SerializedObject((UnityEngine.Object[]) particleSystemRendererList.ToArray());
      this.m_Modules[this.m_Modules.Length - 1] = (ModuleUI) new RendererModuleUI(this, this.m_RendererSerializedObject, ParticleSystemUI.s_ModuleNames[ParticleSystemUI.s_ModuleNames.Length - 1]);
    }

    private void ClearRenderers()
    {
      this.m_RendererSerializedObject = (SerializedObject) null;
      foreach (Component particleSystem in this.m_ParticleSystems)
      {
        ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          Undo.DestroyObjectImmediate((UnityEngine.Object) component);
      }
      this.m_Modules[this.m_Modules.Length - 1] = (ModuleUI) null;
    }

    public float GetEmitterDuration()
    {
      InitialModuleUI module = this.m_Modules[0] as InitialModuleUI;
      if (module != null)
        return module.m_LengthInSec.floatValue;
      return -1f;
    }

    public void OnGUI(float width, bool fixedWidth)
    {
      bool flag1 = Event.current.type == EventType.Repaint;
      string str = (string) null;
      if (this.m_ParticleSystems.Length > 1)
        str = "Multiple Particle Systems";
      else if (this.m_ParticleSystems.Length > 0)
        str = this.m_ParticleSystems[0].gameObject.name;
      if (fixedWidth)
      {
        EditorGUIUtility.labelWidth = width * 0.4f;
        EditorGUILayout.BeginVertical(GUILayout.Width(width));
      }
      else
      {
        EditorGUIUtility.labelWidth = 0.0f;
        EditorGUIUtility.labelWidth -= 4f;
        EditorGUILayout.BeginVertical();
      }
      InitialModuleUI module1 = (InitialModuleUI) this.m_Modules[0];
      for (int moduleIndex = 0; moduleIndex < this.m_Modules.Length; ++moduleIndex)
      {
        ModuleUI module2 = this.m_Modules[moduleIndex];
        if (module2 != null)
        {
          bool flag2 = module2 == this.m_Modules[0];
          if (module2.visibleUI || flag2)
          {
            GUIContent content1 = new GUIContent();
            Rect rect;
            GUIStyle style1;
            if (flag2)
            {
              rect = GUILayoutUtility.GetRect(width, 25f);
              style1 = ParticleSystemStyles.Get().emitterHeaderStyle;
            }
            else
            {
              rect = GUILayoutUtility.GetRect(width, 15f);
              style1 = ParticleSystemStyles.Get().moduleHeaderStyle;
            }
            if (module2.foldout)
            {
              using (new EditorGUI.DisabledScope(!module2.enabled))
              {
                Rect position = EditorGUILayout.BeginVertical(ParticleSystemStyles.Get().modulePadding, new GUILayoutOption[0]);
                position.y -= 4f;
                position.height += 4f;
                GUI.Label(position, GUIContent.none, ParticleSystemStyles.Get().moduleBgStyle);
                module2.OnInspectorGUI(module1);
                EditorGUILayout.EndVertical();
              }
            }
            if (flag2)
            {
              ParticleSystemRenderer component = this.m_ParticleSystems[0].GetComponent<ParticleSystemRenderer>();
              float num = 21f;
              Rect position = new Rect(rect.x + 4f, rect.y + 2f, num, num);
              if (flag1 && (UnityEngine.Object) component != (UnityEngine.Object) null)
              {
                bool flag3 = false;
                int instanceID = 0;
                if (!this.multiEdit)
                {
                  if (component.renderMode == ParticleSystemRenderMode.Mesh)
                  {
                    if ((UnityEngine.Object) component.mesh != (UnityEngine.Object) null)
                      instanceID = component.mesh.GetInstanceID();
                  }
                  else if ((UnityEngine.Object) component.sharedMaterial != (UnityEngine.Object) null)
                    instanceID = component.sharedMaterial.GetInstanceID();
                  if (EditorUtility.IsDirty(instanceID))
                    AssetPreview.ClearTemporaryAssetPreviews();
                  if (instanceID != 0)
                  {
                    Texture2D assetPreview = AssetPreview.GetAssetPreview(instanceID);
                    if ((UnityEngine.Object) assetPreview != (UnityEngine.Object) null)
                    {
                      GUI.DrawTexture(position, (Texture) assetPreview, ScaleMode.StretchToFill, true);
                      flag3 = true;
                    }
                  }
                }
                if (!flag3)
                  GUI.Label(position, GUIContent.none, ParticleSystemStyles.Get().moduleBgStyle);
              }
              if (!this.multiEdit && EditorGUI.DropdownButton(position, GUIContent.none, FocusType.Passive, GUIStyle.none))
              {
                if (EditorGUI.actionKey)
                {
                  List<int> intList = new List<int>();
                  int instanceId = this.m_ParticleSystems[0].gameObject.GetInstanceID();
                  intList.AddRange((IEnumerable<int>) Selection.instanceIDs);
                  if (!intList.Contains(instanceId) || intList.Count != 1)
                  {
                    if (intList.Contains(instanceId))
                      intList.Remove(instanceId);
                    else
                      intList.Add(instanceId);
                  }
                  Selection.instanceIDs = intList.ToArray();
                }
                else
                {
                  Selection.instanceIDs = new int[0];
                  Selection.activeInstanceID = this.m_ParticleSystems[0].gameObject.GetInstanceID();
                }
              }
            }
            Rect position1 = new Rect(rect.x + 2f, rect.y + 1f, 13f, 13f);
            if (!flag2 && GUI.Button(position1, GUIContent.none, GUIStyle.none))
              module2.enabled = !module2.enabled;
            Rect position2 = new Rect((float) ((double) rect.x + (double) rect.width - 10.0), (float) ((double) rect.y + (double) rect.height - 10.0), 10f, 10f);
            Rect position3 = new Rect(position2.x - 4f, position2.y - 4f, position2.width + 4f, position2.height + 4f);
            Rect position4 = new Rect(position2.x - 23f, position2.y - 8f, 20f, 20f);
            if (flag2 && EditorGUI.DropdownButton(position3, ParticleSystemUI.s_Texts.addModules, FocusType.Passive, GUIStyle.none))
              this.ShowAddModuleMenu();
            content1.text = string.IsNullOrEmpty(str) ? module2.displayName : (!flag2 ? module2.displayName : str);
            content1.tooltip = module2.toolTip;
            if (GUI.Toggle(rect, module2.foldout, content1, style1) != module2.foldout)
            {
              switch (Event.current.button)
              {
                case 0:
                  bool flag4 = !module2.foldout;
                  if (Event.current.control)
                  {
                    foreach (ModuleUI module3 in this.m_Modules)
                    {
                      if (module3 != null && module3.visibleUI)
                        module3.foldout = flag4;
                    }
                    break;
                  }
                  module2.foldout = flag4;
                  break;
                case 1:
                  if (flag2)
                  {
                    this.ShowEmitterMenu();
                    break;
                  }
                  this.ShowModuleMenu(moduleIndex);
                  break;
              }
            }
            if (!flag2)
            {
              EditorGUI.showMixedValue = module2.enabledHasMultipleDifferentValues;
              GUIStyle style2 = !EditorGUI.showMixedValue ? ParticleSystemStyles.Get().checkmark : ParticleSystemStyles.Get().checkmarkMixed;
              GUI.Toggle(position1, module2.enabled, GUIContent.none, style2);
              EditorGUI.showMixedValue = false;
            }
            if (flag1 && flag2)
              GUI.Label(position2, GUIContent.none, ParticleSystemStyles.Get().plus);
            if (flag2 && !string.IsNullOrEmpty(this.m_SupportsCullingTextLabel))
            {
              GUIContent content2 = new GUIContent("", (Texture) ParticleSystemStyles.Get().warningIcon, this.m_SupportsCullingTextLabel);
              GUI.Label(position4, content2);
            }
            GUILayout.Space(1f);
          }
        }
      }
      GUILayout.Space(-1f);
      EditorGUILayout.EndVertical();
      this.ApplyProperties();
    }

    public void OnSceneViewGUI()
    {
      if (this.m_Modules == null)
        return;
      if (ParticleEffectUI.m_ShowBounds)
      {
        foreach (ParticleSystem particleSystem in this.m_ParticleSystems)
        {
          if (this.multiEdit)
            this.ShowBounds(ParticleSystemEditorUtils.GetRoot(particleSystem));
          else
            this.ShowBounds(particleSystem);
        }
      }
      this.UpdateProperties();
      foreach (ModuleUI module in this.m_Modules)
      {
        if (module != null && module.visibleUI && (module.enabled && module.foldout))
          module.OnSceneViewGUI();
      }
      this.ApplyProperties();
    }

    private void ShowBounds(ParticleSystem ps)
    {
      if (ps.particleCount > 0)
      {
        ParticleSystemRenderer component = ps.GetComponent<ParticleSystemRenderer>();
        Color color = Handles.color;
        Handles.color = Color.yellow;
        Bounds bounds = component.bounds;
        Handles.DrawWireCube(bounds.center, bounds.size);
        Handles.color = color;
      }
      if (!this.multiEdit)
        return;
      foreach (ParticleSystem componentsInChild in ps.transform.GetComponentsInChildren<ParticleSystem>())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ParticleSystemUI.\u003CShowBounds\u003Ec__AnonStorey0 boundsCAnonStorey0 = new ParticleSystemUI.\u003CShowBounds\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        boundsCAnonStorey0.child = componentsInChild;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((UnityEngine.Object) boundsCAnonStorey0.child != (UnityEngine.Object) ps && !((UnityEngine.Object) ((IEnumerable<ParticleSystem>) this.m_ParticleSystems).FirstOrDefault<ParticleSystem>(new Func<ParticleSystem, bool>(boundsCAnonStorey0.\u003C\u003Em__0)) != (UnityEngine.Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          this.ShowBounds(boundsCAnonStorey0.child);
        }
      }
    }

    public void ApplyProperties()
    {
      bool modifiedProperties = this.m_ParticleSystemSerializedObject.hasModifiedProperties;
      if (this.m_ParticleSystemSerializedObject.targetObject != (UnityEngine.Object) null)
        this.m_ParticleSystemSerializedObject.ApplyModifiedProperties();
      if (modifiedProperties)
      {
        foreach (ParticleSystem particleSystem in this.m_ParticleSystems)
        {
          if (!ParticleEffectUI.IsStopped(ParticleSystemEditorUtils.GetRoot(particleSystem)) && ParticleSystemEditorUtils.editorResimulation)
            ParticleSystemEditorUtils.PerformCompleteResimulation();
        }
        this.UpdateParticleSystemInfoString();
      }
      if (this.m_RendererSerializedObject == null || !(this.m_RendererSerializedObject.targetObject != (UnityEngine.Object) null))
        return;
      this.m_RendererSerializedObject.ApplyModifiedProperties();
    }

    private void UpdateParticleSystemInfoString()
    {
      string text = "";
      foreach (ModuleUI module in this.m_Modules)
      {
        if (module != null && module.visibleUI && module.enabled)
          module.UpdateCullingSupportedString(ref text);
      }
      if (text != string.Empty)
      {
        if (!(text != this.m_SupportsCullingText) && this.m_SupportsCullingTextLabel != null)
          return;
        this.m_SupportsCullingText = text;
        this.m_SupportsCullingTextLabel = "Automatic culling is disabled because: " + text.Replace("\n", "\n" + ParticleSystemUI.s_Texts.bulletPoint);
      }
      else
      {
        this.m_SupportsCullingText = (string) null;
        this.m_SupportsCullingTextLabel = (string) null;
      }
    }

    public void UpdateProperties()
    {
      if (this.m_ParticleSystemSerializedObject.targetObject != (UnityEngine.Object) null)
        this.m_ParticleSystemSerializedObject.UpdateIfRequiredOrScript();
      if (this.m_RendererSerializedObject == null || !(this.m_RendererSerializedObject.targetObject != (UnityEngine.Object) null))
        return;
      this.m_RendererSerializedObject.UpdateIfRequiredOrScript();
    }

    private void ResetModules()
    {
      foreach (ModuleUI module in this.m_Modules)
      {
        if (module != null)
        {
          module.enabled = false;
          if (!ParticleEffectUI.GetAllModulesVisible())
            module.visibleUI = false;
        }
      }
      if (this.m_Modules[this.m_Modules.Length - 1] == null)
        this.InitRendererUI();
      int[] numArray = new int[3]{ 1, 2, this.m_Modules.Length - 1 };
      foreach (int index in numArray)
      {
        if (this.m_Modules[index] != null)
        {
          this.m_Modules[index].enabled = true;
          this.m_Modules[index].visibleUI = true;
        }
      }
    }

    private void ShowAddModuleMenu()
    {
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < ParticleSystemUI.s_ModuleNames.Length; ++index)
      {
        if (this.m_Modules[index] == null || !this.m_Modules[index].visibleUI)
          genericMenu.AddItem(new GUIContent(ParticleSystemUI.s_ModuleNames[index]), false, new GenericMenu.MenuFunction2(this.AddModuleCallback), (object) index);
        else
          genericMenu.AddDisabledItem(new GUIContent(ParticleSystemUI.s_ModuleNames[index]));
      }
      genericMenu.AddSeparator("");
      genericMenu.AddItem(new GUIContent("Show All Modules"), ParticleEffectUI.GetAllModulesVisible(), new GenericMenu.MenuFunction2(this.AddModuleCallback), (object) 10000);
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private void AddModuleCallback(object obj)
    {
      int index = (int) obj;
      if (index >= 0 && index < this.m_Modules.Length)
      {
        if (index == this.m_Modules.Length - 1)
        {
          this.InitRendererUI();
        }
        else
        {
          this.m_Modules[index].enabled = true;
          this.m_Modules[index].foldout = true;
        }
      }
      else
        this.m_ParticleEffectUI.SetAllModulesVisible(!ParticleEffectUI.GetAllModulesVisible());
      this.ApplyProperties();
    }

    private void ModuleMenuCallback(object obj)
    {
      int index = (int) obj;
      if (index == this.m_Modules.Length - 1)
      {
        this.ClearRenderers();
      }
      else
      {
        if (!ParticleEffectUI.GetAllModulesVisible())
          this.m_Modules[index].visibleUI = false;
        this.m_Modules[index].enabled = false;
      }
    }

    private void ShowModuleMenu(int moduleIndex)
    {
      GenericMenu genericMenu = new GenericMenu();
      if (!ParticleEffectUI.GetAllModulesVisible())
        genericMenu.AddItem(new GUIContent("Remove"), false, new GenericMenu.MenuFunction2(this.ModuleMenuCallback), (object) moduleIndex);
      else
        genericMenu.AddDisabledItem(new GUIContent("Remove"));
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private void EmitterMenuCallback(object obj)
    {
      switch ((int) obj)
      {
        case 0:
          this.m_ParticleEffectUI.CreateParticleSystem(this.m_ParticleSystems[0], SubModuleUI.SubEmitterType.None);
          break;
        case 1:
          this.ResetModules();
          break;
        case 2:
          EditorGUIUtility.PingObject((UnityEngine.Object) this.m_ParticleSystems[0]);
          break;
      }
    }

    private void ShowEmitterMenu()
    {
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Show Location"), false, new GenericMenu.MenuFunction2(this.EmitterMenuCallback), (object) 2);
      genericMenu.AddSeparator("");
      if (this.m_ParticleSystems[0].gameObject.activeInHierarchy)
        genericMenu.AddItem(new GUIContent("Create Particle System"), false, new GenericMenu.MenuFunction2(this.EmitterMenuCallback), (object) 0);
      else
        genericMenu.AddDisabledItem(new GUIContent("Create new Particle System"));
      genericMenu.AddItem(new GUIContent("Reset"), false, new GenericMenu.MenuFunction2(this.EmitterMenuCallback), (object) 1);
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private static ModuleUI[] CreateUIModules(ParticleSystemUI e, SerializedObject so)
    {
      int num1 = 0;
      ModuleUI[] moduleUiArray = new ModuleUI[23];
      int index1 = 0;
      ParticleSystemUI owner1 = e;
      SerializedObject o1 = so;
      string[] moduleNames1 = ParticleSystemUI.s_ModuleNames;
      int index2 = num1;
      int num2 = index2 + 1;
      string displayName1 = moduleNames1[index2];
      InitialModuleUI initialModuleUi = new InitialModuleUI(owner1, o1, displayName1);
      moduleUiArray[index1] = (ModuleUI) initialModuleUi;
      int index3 = 1;
      ParticleSystemUI owner2 = e;
      SerializedObject o2 = so;
      string[] moduleNames2 = ParticleSystemUI.s_ModuleNames;
      int index4 = num2;
      int num3 = index4 + 1;
      string displayName2 = moduleNames2[index4];
      EmissionModuleUI emissionModuleUi = new EmissionModuleUI(owner2, o2, displayName2);
      moduleUiArray[index3] = (ModuleUI) emissionModuleUi;
      int index5 = 2;
      ParticleSystemUI owner3 = e;
      SerializedObject o3 = so;
      string[] moduleNames3 = ParticleSystemUI.s_ModuleNames;
      int index6 = num3;
      int num4 = index6 + 1;
      string displayName3 = moduleNames3[index6];
      ShapeModuleUI shapeModuleUi = new ShapeModuleUI(owner3, o3, displayName3);
      moduleUiArray[index5] = (ModuleUI) shapeModuleUi;
      int index7 = 3;
      ParticleSystemUI owner4 = e;
      SerializedObject o4 = so;
      string[] moduleNames4 = ParticleSystemUI.s_ModuleNames;
      int index8 = num4;
      int num5 = index8 + 1;
      string displayName4 = moduleNames4[index8];
      VelocityModuleUI velocityModuleUi1 = new VelocityModuleUI(owner4, o4, displayName4);
      moduleUiArray[index7] = (ModuleUI) velocityModuleUi1;
      int index9 = 4;
      ParticleSystemUI owner5 = e;
      SerializedObject o5 = so;
      string[] moduleNames5 = ParticleSystemUI.s_ModuleNames;
      int index10 = num5;
      int num6 = index10 + 1;
      string displayName5 = moduleNames5[index10];
      ClampVelocityModuleUI velocityModuleUi2 = new ClampVelocityModuleUI(owner5, o5, displayName5);
      moduleUiArray[index9] = (ModuleUI) velocityModuleUi2;
      int index11 = 5;
      ParticleSystemUI owner6 = e;
      SerializedObject o6 = so;
      string[] moduleNames6 = ParticleSystemUI.s_ModuleNames;
      int index12 = num6;
      int num7 = index12 + 1;
      string displayName6 = moduleNames6[index12];
      InheritVelocityModuleUI velocityModuleUi3 = new InheritVelocityModuleUI(owner6, o6, displayName6);
      moduleUiArray[index11] = (ModuleUI) velocityModuleUi3;
      int index13 = 6;
      ParticleSystemUI owner7 = e;
      SerializedObject o7 = so;
      string[] moduleNames7 = ParticleSystemUI.s_ModuleNames;
      int index14 = num7;
      int num8 = index14 + 1;
      string displayName7 = moduleNames7[index14];
      ForceModuleUI forceModuleUi = new ForceModuleUI(owner7, o7, displayName7);
      moduleUiArray[index13] = (ModuleUI) forceModuleUi;
      int index15 = 7;
      ParticleSystemUI owner8 = e;
      SerializedObject o8 = so;
      string[] moduleNames8 = ParticleSystemUI.s_ModuleNames;
      int index16 = num8;
      int num9 = index16 + 1;
      string displayName8 = moduleNames8[index16];
      ColorModuleUI colorModuleUi = new ColorModuleUI(owner8, o8, displayName8);
      moduleUiArray[index15] = (ModuleUI) colorModuleUi;
      int index17 = 8;
      ParticleSystemUI owner9 = e;
      SerializedObject o9 = so;
      string[] moduleNames9 = ParticleSystemUI.s_ModuleNames;
      int index18 = num9;
      int num10 = index18 + 1;
      string displayName9 = moduleNames9[index18];
      ColorByVelocityModuleUI velocityModuleUi4 = new ColorByVelocityModuleUI(owner9, o9, displayName9);
      moduleUiArray[index17] = (ModuleUI) velocityModuleUi4;
      int index19 = 9;
      ParticleSystemUI owner10 = e;
      SerializedObject o10 = so;
      string[] moduleNames10 = ParticleSystemUI.s_ModuleNames;
      int index20 = num10;
      int num11 = index20 + 1;
      string displayName10 = moduleNames10[index20];
      SizeModuleUI sizeModuleUi = new SizeModuleUI(owner10, o10, displayName10);
      moduleUiArray[index19] = (ModuleUI) sizeModuleUi;
      int index21 = 10;
      ParticleSystemUI owner11 = e;
      SerializedObject o11 = so;
      string[] moduleNames11 = ParticleSystemUI.s_ModuleNames;
      int index22 = num11;
      int num12 = index22 + 1;
      string displayName11 = moduleNames11[index22];
      SizeByVelocityModuleUI velocityModuleUi5 = new SizeByVelocityModuleUI(owner11, o11, displayName11);
      moduleUiArray[index21] = (ModuleUI) velocityModuleUi5;
      int index23 = 11;
      ParticleSystemUI owner12 = e;
      SerializedObject o12 = so;
      string[] moduleNames12 = ParticleSystemUI.s_ModuleNames;
      int index24 = num12;
      int num13 = index24 + 1;
      string displayName12 = moduleNames12[index24];
      RotationModuleUI rotationModuleUi = new RotationModuleUI(owner12, o12, displayName12);
      moduleUiArray[index23] = (ModuleUI) rotationModuleUi;
      int index25 = 12;
      ParticleSystemUI owner13 = e;
      SerializedObject o13 = so;
      string[] moduleNames13 = ParticleSystemUI.s_ModuleNames;
      int index26 = num13;
      int num14 = index26 + 1;
      string displayName13 = moduleNames13[index26];
      RotationByVelocityModuleUI velocityModuleUi6 = new RotationByVelocityModuleUI(owner13, o13, displayName13);
      moduleUiArray[index25] = (ModuleUI) velocityModuleUi6;
      int index27 = 13;
      ParticleSystemUI owner14 = e;
      SerializedObject o14 = so;
      string[] moduleNames14 = ParticleSystemUI.s_ModuleNames;
      int index28 = num14;
      int num15 = index28 + 1;
      string displayName14 = moduleNames14[index28];
      ExternalForcesModuleUI externalForcesModuleUi = new ExternalForcesModuleUI(owner14, o14, displayName14);
      moduleUiArray[index27] = (ModuleUI) externalForcesModuleUi;
      int index29 = 14;
      ParticleSystemUI owner15 = e;
      SerializedObject o15 = so;
      string[] moduleNames15 = ParticleSystemUI.s_ModuleNames;
      int index30 = num15;
      int num16 = index30 + 1;
      string displayName15 = moduleNames15[index30];
      NoiseModuleUI noiseModuleUi = new NoiseModuleUI(owner15, o15, displayName15);
      moduleUiArray[index29] = (ModuleUI) noiseModuleUi;
      int index31 = 15;
      ParticleSystemUI owner16 = e;
      SerializedObject o16 = so;
      string[] moduleNames16 = ParticleSystemUI.s_ModuleNames;
      int index32 = num16;
      int num17 = index32 + 1;
      string displayName16 = moduleNames16[index32];
      CollisionModuleUI collisionModuleUi = new CollisionModuleUI(owner16, o16, displayName16);
      moduleUiArray[index31] = (ModuleUI) collisionModuleUi;
      int index33 = 16;
      ParticleSystemUI owner17 = e;
      SerializedObject o17 = so;
      string[] moduleNames17 = ParticleSystemUI.s_ModuleNames;
      int index34 = num17;
      int num18 = index34 + 1;
      string displayName17 = moduleNames17[index34];
      TriggerModuleUI triggerModuleUi = new TriggerModuleUI(owner17, o17, displayName17);
      moduleUiArray[index33] = (ModuleUI) triggerModuleUi;
      int index35 = 17;
      ParticleSystemUI owner18 = e;
      SerializedObject o18 = so;
      string[] moduleNames18 = ParticleSystemUI.s_ModuleNames;
      int index36 = num18;
      int num19 = index36 + 1;
      string displayName18 = moduleNames18[index36];
      SubModuleUI subModuleUi = new SubModuleUI(owner18, o18, displayName18);
      moduleUiArray[index35] = (ModuleUI) subModuleUi;
      int index37 = 18;
      ParticleSystemUI owner19 = e;
      SerializedObject o19 = so;
      string[] moduleNames19 = ParticleSystemUI.s_ModuleNames;
      int index38 = num19;
      int num20 = index38 + 1;
      string displayName19 = moduleNames19[index38];
      UVModuleUI uvModuleUi = new UVModuleUI(owner19, o19, displayName19);
      moduleUiArray[index37] = (ModuleUI) uvModuleUi;
      int index39 = 19;
      ParticleSystemUI owner20 = e;
      SerializedObject o20 = so;
      string[] moduleNames20 = ParticleSystemUI.s_ModuleNames;
      int index40 = num20;
      int num21 = index40 + 1;
      string displayName20 = moduleNames20[index40];
      LightsModuleUI lightsModuleUi = new LightsModuleUI(owner20, o20, displayName20);
      moduleUiArray[index39] = (ModuleUI) lightsModuleUi;
      int index41 = 20;
      ParticleSystemUI owner21 = e;
      SerializedObject o21 = so;
      string[] moduleNames21 = ParticleSystemUI.s_ModuleNames;
      int index42 = num21;
      int num22 = index42 + 1;
      string displayName21 = moduleNames21[index42];
      TrailModuleUI trailModuleUi = new TrailModuleUI(owner21, o21, displayName21);
      moduleUiArray[index41] = (ModuleUI) trailModuleUi;
      int index43 = 21;
      ParticleSystemUI owner22 = e;
      SerializedObject o22 = so;
      string[] moduleNames22 = ParticleSystemUI.s_ModuleNames;
      int index44 = num22;
      int num23 = index44 + 1;
      string displayName22 = moduleNames22[index44];
      CustomDataModuleUI customDataModuleUi = new CustomDataModuleUI(owner22, o22, displayName22);
      moduleUiArray[index43] = (ModuleUI) customDataModuleUi;
      return moduleUiArray;
    }

    public static string[] GetUIModuleNames()
    {
      return new string[23]{ "", "Emission", "Shape", "Velocity over Lifetime", "Limit Velocity over Lifetime", "Inherit Velocity", "Force over Lifetime", "Color over Lifetime", "Color by Speed", "Size over Lifetime", "Size by Speed", "Rotation over Lifetime", "Rotation by Speed", "External Forces", "Noise", "Collision", "Triggers", "Sub Emitters", "Texture Sheet Animation", "Lights", "Trails", "Custom Data", "Renderer" };
    }

    public enum DefaultTypes
    {
      Root,
      SubBirth,
      SubCollision,
      SubDeath,
    }

    protected class Texts
    {
      public GUIContent addModules = new GUIContent("", "Show/Hide Modules");
      public string bulletPoint = "• ";
    }
  }
}
