// Decompiled with JetBrains decompiler
// Type: UnityEditor.SubModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SubModuleUI : ModuleUI
  {
    private int m_CheckObjectIndex = -1;
    private SerializedProperty m_SubEmitters;
    private static SubModuleUI.Texts s_Texts;

    public SubModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "SubModule", displayName)
    {
      this.m_ToolTip = "Sub emission of particles. This allows each particle to emit particles in another system.";
      this.Init();
    }

    protected override void Init()
    {
      if (this.m_SubEmitters != null)
        return;
      if (SubModuleUI.s_Texts == null)
        SubModuleUI.s_Texts = new SubModuleUI.Texts();
      this.m_SubEmitters = this.GetProperty("subEmitters");
    }

    private void CreateSubEmitter(SerializedProperty objectRefProp, int index, SubModuleUI.SubEmitterType type)
    {
      GameObject particleSystem = this.m_ParticleSystemUI.m_ParticleEffectUI.CreateParticleSystem(this.m_ParticleSystemUI.m_ParticleSystems[0], type);
      particleSystem.name = "SubEmitter" + (object) index;
      objectRefProp.objectReferenceValue = (Object) particleSystem.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
      if (this.m_CheckObjectIndex < 0 || ObjectSelector.isVisible)
        return;
      SerializedProperty propertyRelative = this.m_SubEmitters.GetArrayElementAtIndex(this.m_CheckObjectIndex).FindPropertyRelative("emitter");
      Object objectReferenceValue = propertyRelative.objectReferenceValue;
      ParticleSystem subEmitter = objectReferenceValue as ParticleSystem;
      if ((Object) subEmitter != (Object) null)
      {
        bool flag = true;
        if (this.ValidateSubemitter(subEmitter))
        {
          string str = ParticleSystemEditorUtils.CheckCircularReferences(subEmitter);
          if (str.Length == 0)
          {
            if (!this.CheckIfChild(objectReferenceValue))
              flag = false;
          }
          else
          {
            EditorUtility.DisplayDialog("Circular References Detected", string.Format("'{0}' could not be assigned as subemitter on '{1}' due to circular referencing!\nBacktrace: {2} \n\nReference will be removed.", (object) subEmitter.gameObject.name, (object) this.m_ParticleSystemUI.m_ParticleSystems[0].gameObject.name, (object) str), "Ok");
            flag = false;
          }
        }
        else
          flag = false;
        if (!flag)
        {
          propertyRelative.objectReferenceValue = (Object) null;
          this.m_ParticleSystemUI.ApplyProperties();
          this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.Repaint();
        }
      }
      this.m_CheckObjectIndex = -1;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
    }

    internal static bool IsChild(ParticleSystem subEmitter, ParticleSystem root)
    {
      if ((Object) subEmitter == (Object) null || (Object) root == (Object) null)
        return false;
      return (Object) ParticleSystemEditorUtils.GetRoot(subEmitter) == (Object) root;
    }

    private bool ValidateSubemitter(ParticleSystem subEmitter)
    {
      if ((Object) subEmitter == (Object) null)
        return false;
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_ParticleSystemUI.m_ParticleSystems[0]);
      if (root.gameObject.activeInHierarchy && !subEmitter.gameObject.activeInHierarchy)
      {
        EditorUtility.DisplayDialog("Invalid Sub Emitter", "The assigned sub emitter is part of a prefab and can therefore not be assigned.", "Ok");
        return false;
      }
      if (root.gameObject.activeInHierarchy || !subEmitter.gameObject.activeInHierarchy)
        return true;
      EditorUtility.DisplayDialog("Invalid Sub Emitter", "The assigned sub emitter is part of a scene object and can therefore not be assigned to a prefab.", "Ok");
      return false;
    }

    private bool CheckIfChild(Object subEmitter)
    {
      ParticleSystem root = ParticleSystemEditorUtils.GetRoot(this.m_ParticleSystemUI.m_ParticleSystems[0]);
      ParticleSystem subEmitter1 = subEmitter as ParticleSystem;
      if (SubModuleUI.IsChild(subEmitter1, root))
        return true;
      if (EditorUtility.DisplayDialog("Reparent GameObjects", string.Format("The assigned sub emitter is not a child of the current root particle system GameObject: '{0}' and is therefore NOT considered a part of the current effect. Do you want to reparent it?", (object) root.gameObject.name), "Yes, Reparent", "No, Remove"))
      {
        if (EditorUtility.IsPersistent(subEmitter))
        {
          GameObject gameObject = Object.Instantiate(subEmitter) as GameObject;
          if ((Object) gameObject != (Object) null)
          {
            gameObject.transform.parent = this.m_ParticleSystemUI.m_ParticleSystems[0].transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
          }
        }
        else if ((Object) subEmitter1 != (Object) null)
          Undo.SetTransformParent(subEmitter1.gameObject.transform.transform, this.m_ParticleSystemUI.m_ParticleSystems[0].transform, "Reparent sub emitter");
        return true;
      }
      if ((Object) subEmitter1 != (Object) null)
        subEmitter1.Clear(true);
      return false;
    }

    private List<Object> GetSubEmitterProperties()
    {
      List<Object> objectList = new List<Object>();
      foreach (SerializedProperty subEmitter in this.m_SubEmitters)
        objectList.Add(subEmitter.FindPropertyRelative("emitter").objectReferenceValue);
      return objectList;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      if (this.m_ParticleSystemUI.multiEdit)
      {
        EditorGUILayout.HelpBox("Sub Emitter editing is only available when editing a single Particle System", MessageType.Info, true);
      }
      else
      {
        List<Object> emitterProperties1 = this.GetSubEmitterProperties();
        GUILayout.BeginHorizontal(GUILayout.Height(16f));
        GUILayout.Label("", ParticleSystemStyles.Get().label, new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(true)
        });
        GUILayout.Label(SubModuleUI.s_Texts.inherit, ParticleSystemStyles.Get().label, new GUILayoutOption[1]
        {
          GUILayout.Width(120f)
        });
        GUILayout.EndHorizontal();
        for (int index = 0; index < this.m_SubEmitters.arraySize; ++index)
          this.ShowSubEmitter(index);
        List<Object> emitterProperties2 = this.GetSubEmitterProperties();
        for (int index = 0; index < Mathf.Min(emitterProperties1.Count, emitterProperties2.Count); ++index)
        {
          if (emitterProperties1[index] != emitterProperties2[index])
          {
            if (this.m_CheckObjectIndex == -1)
              EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
            this.m_CheckObjectIndex = index;
            ParticleSystem particleSystem = emitterProperties1[index] as ParticleSystem;
            if ((bool) ((Object) particleSystem))
              particleSystem.Clear(true);
          }
        }
      }
    }

    private void ShowSubEmitter(int index)
    {
      GUILayout.BeginHorizontal(GUILayout.Height(16f));
      SerializedProperty arrayElementAtIndex = this.m_SubEmitters.GetArrayElementAtIndex(index);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("emitter");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("type");
      SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("properties");
      ModuleUI.GUIPopup(GUIContent.none, propertyRelative2, SubModuleUI.s_Texts.subEmitterTypes, new GUILayoutOption[1]
      {
        GUILayout.MaxWidth(80f)
      });
      GUILayout.Label("", ParticleSystemStyles.Get().label, new GUILayoutOption[1]
      {
        GUILayout.Width(4f)
      });
      ModuleUI.GUIObject(GUIContent.none, propertyRelative1);
      GUIStyle style = new GUIStyle((GUIStyle) "OL Plus");
      if (propertyRelative1.objectReferenceValue == (Object) null)
      {
        GUILayout.Label("", ParticleSystemStyles.Get().label, new GUILayoutOption[1]
        {
          GUILayout.Width(8f)
        });
        GUILayout.BeginVertical(new GUILayoutOption[2]
        {
          GUILayout.Width(16f),
          GUILayout.Height(style.fixedHeight)
        });
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(GUIContent.none, ParticleSystemStyles.Get().plus, new GUILayoutOption[0]))
          this.CreateSubEmitter(propertyRelative1, index, (SubModuleUI.SubEmitterType) propertyRelative2.intValue);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
      }
      else
        GUILayout.Label("", ParticleSystemStyles.Get().label, new GUILayoutOption[1]
        {
          GUILayout.Width(24f)
        });
      ModuleUI.GUIMask(GUIContent.none, propertyRelative3, SubModuleUI.s_Texts.propertyTypes, GUILayout.Width(100f));
      GUILayout.Label("", ParticleSystemStyles.Get().label, new GUILayoutOption[1]
      {
        GUILayout.Width(8f)
      });
      if (index == 0)
      {
        if (GUILayout.Button(GUIContent.none, style, new GUILayoutOption[1]{ GUILayout.Width(16f) }))
        {
          this.m_SubEmitters.InsertArrayElementAtIndex(this.m_SubEmitters.arraySize);
          this.m_SubEmitters.GetArrayElementAtIndex(this.m_SubEmitters.arraySize - 1).FindPropertyRelative("emitter").objectReferenceValue = (Object) null;
        }
      }
      else if (GUILayout.Button(GUIContent.none, new GUIStyle((GUIStyle) "OL Minus"), new GUILayoutOption[1]{ GUILayout.Width(16f) }))
        this.m_SubEmitters.DeleteArrayElementAtIndex(index);
      GUILayout.EndHorizontal();
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text += "\nSub Emitters module is enabled.";
    }

    public enum SubEmitterType
    {
      None = -1,
      Birth = 0,
      Collision = 1,
      Death = 2,
      TypesMax = 3,
    }

    private class Texts
    {
      public GUIContent create = EditorGUIUtility.TextContent("|Create and assign a Particle System as sub emitter");
      public GUIContent inherit = EditorGUIUtility.TextContent("Inherit");
      public GUIContent[] subEmitterTypes = new GUIContent[3]{ EditorGUIUtility.TextContent("Birth"), EditorGUIUtility.TextContent("Collision"), EditorGUIUtility.TextContent("Death") };
      public string[] propertyTypes = new string[4]{ "Color", "Size", "Rotation", "Lifetime" };
    }
  }
}
