// Decompiled with JetBrains decompiler
// Type: UnityEditor.TriggerModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class TriggerModuleUI : ModuleUI
  {
    private static bool s_VisualizeBounds = false;
    private SerializedProperty[] m_CollisionShapes = new SerializedProperty[6];
    private const int k_MaxNumCollisionShapes = 6;
    private SerializedProperty m_Inside;
    private SerializedProperty m_Outside;
    private SerializedProperty m_Enter;
    private SerializedProperty m_Exit;
    private SerializedProperty m_RadiusScale;
    private SerializedProperty[] m_ShownCollisionShapes;
    private static TriggerModuleUI.Texts s_Texts;

    public TriggerModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "TriggerModule", displayName)
    {
      this.m_ToolTip = "Allows you to execute script code based on whether particles are inside or outside the collision shapes.";
    }

    protected override void Init()
    {
      if (this.m_Inside != null)
        return;
      if (TriggerModuleUI.s_Texts == null)
        TriggerModuleUI.s_Texts = new TriggerModuleUI.Texts();
      List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>();
      for (int index = 0; index < this.m_CollisionShapes.Length; ++index)
      {
        this.m_CollisionShapes[index] = this.GetProperty("collisionShape" + (object) index);
        if (index == 0 || this.m_CollisionShapes[index].objectReferenceValue != (UnityEngine.Object) null)
          serializedPropertyList.Add(this.m_CollisionShapes[index]);
      }
      this.m_ShownCollisionShapes = serializedPropertyList.ToArray();
      this.m_Inside = this.GetProperty("inside");
      this.m_Outside = this.GetProperty("outside");
      this.m_Enter = this.GetProperty("enter");
      this.m_Exit = this.GetProperty("exit");
      this.m_RadiusScale = this.GetProperty("radiusScale");
      TriggerModuleUI.s_VisualizeBounds = EditorPrefs.GetBool("VisualizeTriggerBounds", false);
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      this.DoListOfCollisionShapesGUI();
      ModuleUI.GUIPopup(TriggerModuleUI.s_Texts.inside, this.m_Inside, TriggerModuleUI.s_Texts.overlapOptions);
      ModuleUI.GUIPopup(TriggerModuleUI.s_Texts.outside, this.m_Outside, TriggerModuleUI.s_Texts.overlapOptions);
      ModuleUI.GUIPopup(TriggerModuleUI.s_Texts.enter, this.m_Enter, TriggerModuleUI.s_Texts.overlapOptions);
      ModuleUI.GUIPopup(TriggerModuleUI.s_Texts.exit, this.m_Exit, TriggerModuleUI.s_Texts.overlapOptions);
      double num = (double) ModuleUI.GUIFloat(TriggerModuleUI.s_Texts.radiusScale, this.m_RadiusScale);
      EditorGUI.BeginChangeCheck();
      TriggerModuleUI.s_VisualizeBounds = ModuleUI.GUIToggle(TriggerModuleUI.s_Texts.visualizeBounds, TriggerModuleUI.s_VisualizeBounds);
      if (!EditorGUI.EndChangeCheck())
        return;
      EditorPrefs.SetBool("VisualizeTriggerBounds", TriggerModuleUI.s_VisualizeBounds);
    }

    private static GameObject CreateDefaultCollider(string name, ParticleSystem parentOfGameObject)
    {
      GameObject gameObject = new GameObject(name);
      if (!(bool) ((UnityEngine.Object) gameObject))
        return (GameObject) null;
      if ((bool) ((UnityEngine.Object) parentOfGameObject))
        gameObject.transform.parent = parentOfGameObject.transform;
      gameObject.AddComponent<SphereCollider>();
      return gameObject;
    }

    private void DoListOfCollisionShapesGUI()
    {
      if (this.m_ParticleSystemUI.multiEdit)
      {
        for (int index = 0; index < 6; ++index)
        {
          int num1 = -1;
          foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
          {
            int num2 = !((UnityEngine.Object) particleSystem.trigger.GetCollider(index) != (UnityEngine.Object) null) ? 0 : 1;
            if (num1 == -1)
              num1 = num2;
            else if (num2 != num1)
            {
              EditorGUILayout.HelpBox("Collider list editing is only available when all selected systems contain the same number of colliders", MessageType.Info, true);
              return;
            }
          }
        }
      }
      int index1 = this.GUIListOfFloatObjectToggleFields(TriggerModuleUI.s_Texts.collisionShapes, this.m_ShownCollisionShapes, (EditorGUI.ObjectFieldValidator) null, TriggerModuleUI.s_Texts.createCollisionShape, !this.m_ParticleSystemUI.multiEdit);
      if (index1 >= 0 && !this.m_ParticleSystemUI.multiEdit)
      {
        GameObject defaultCollider = TriggerModuleUI.CreateDefaultCollider("Collider " + (object) (index1 + 1), this.m_ParticleSystemUI.m_ParticleSystems[0]);
        defaultCollider.transform.localPosition = new Vector3(0.0f, 0.0f, (float) (10 + index1));
        this.m_ShownCollisionShapes[index1].objectReferenceValue = (UnityEngine.Object) defaultCollider;
      }
      Rect rect = GUILayoutUtility.GetRect(0.0f, 16f);
      rect.x = (float) ((double) rect.xMax - 24.0 - 5.0);
      rect.width = 12f;
      if (this.m_ShownCollisionShapes.Length > 1 && ModuleUI.MinusButton(rect))
      {
        this.m_ShownCollisionShapes[this.m_ShownCollisionShapes.Length - 1].objectReferenceValue = (UnityEngine.Object) null;
        List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownCollisionShapes);
        serializedPropertyList.RemoveAt(serializedPropertyList.Count - 1);
        this.m_ShownCollisionShapes = serializedPropertyList.ToArray();
      }
      if (this.m_ShownCollisionShapes.Length >= 6 || this.m_ParticleSystemUI.multiEdit)
        return;
      rect.x += 17f;
      if (ModuleUI.PlusButton(rect))
      {
        List<SerializedProperty> serializedPropertyList = new List<SerializedProperty>((IEnumerable<SerializedProperty>) this.m_ShownCollisionShapes);
        serializedPropertyList.Add(this.m_CollisionShapes[serializedPropertyList.Count]);
        this.m_ShownCollisionShapes = serializedPropertyList.ToArray();
      }
    }

    public override void OnSceneViewGUI()
    {
      if (!TriggerModuleUI.s_VisualizeBounds)
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
        if (particleSystem.trigger.enabled)
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
            float num = Math.Max(currentSize3D.x, Math.Max(currentSize3D.y, currentSize3D.z)) * 0.5f * particleSystem.trigger.radiusScale;
            Handles.matrix = matrix4x4 * Matrix4x4.TRS(particle.position, Quaternion.identity, new Vector3(num, num, num));
            Handles.DrawPolyLine(vector3Array);
          }
        }
      }
      Handles.color = color;
      Handles.matrix = matrix;
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text += "\nTriggers module is enabled.";
    }

    private enum OverlapOptions
    {
      Ignore,
      Kill,
      Callback,
    }

    private class Texts
    {
      public GUIContent collisionShapes = EditorGUIUtility.TextContent("Colliders|The list of collision shapes to use for the trigger.");
      public GUIContent createCollisionShape = EditorGUIUtility.TextContent("|Create a GameObject containing a sphere collider and assigns it to the list.");
      public GUIContent inside = EditorGUIUtility.TextContent("Inside|What to do for particles that are inside the collision volume.");
      public GUIContent outside = EditorGUIUtility.TextContent("Outside|What to do for particles that are outside the collision volume.");
      public GUIContent enter = EditorGUIUtility.TextContent("Enter|Triggered once when particles enter the collison volume.");
      public GUIContent exit = EditorGUIUtility.TextContent("Exit|Triggered once when particles leave the collison volume.");
      public GUIContent radiusScale = EditorGUIUtility.TextContent("Radius Scale|Scale particle bounds by this amount to get more precise collisions.");
      public GUIContent visualizeBounds = EditorGUIUtility.TextContent("Visualize Bounds|Render the collision bounds of the particles.");
      public GUIContent[] overlapOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Ignore"), EditorGUIUtility.TextContent("Kill"), EditorGUIUtility.TextContent("Callback") };
    }
  }
}
