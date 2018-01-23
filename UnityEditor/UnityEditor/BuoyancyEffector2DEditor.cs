// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuoyancyEffector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (BuoyancyEffector2D), true)]
  [CanEditMultipleObjects]
  internal class BuoyancyEffector2DEditor : Effector2DEditor
  {
    private static readonly AnimBool m_ShowDampingRollout = new AnimBool();
    private static readonly AnimBool m_ShowFlowRollout = new AnimBool();
    private SerializedProperty m_Density;
    private SerializedProperty m_SurfaceLevel;
    private SerializedProperty m_LinearDrag;
    private SerializedProperty m_AngularDrag;
    private SerializedProperty m_FlowAngle;
    private SerializedProperty m_FlowMagnitude;
    private SerializedProperty m_FlowVariation;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Density = this.serializedObject.FindProperty("m_Density");
      this.m_SurfaceLevel = this.serializedObject.FindProperty("m_SurfaceLevel");
      BuoyancyEffector2DEditor.m_ShowDampingRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_LinearDrag = this.serializedObject.FindProperty("m_LinearDrag");
      this.m_AngularDrag = this.serializedObject.FindProperty("m_AngularDrag");
      BuoyancyEffector2DEditor.m_ShowFlowRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_FlowAngle = this.serializedObject.FindProperty("m_FlowAngle");
      this.m_FlowMagnitude = this.serializedObject.FindProperty("m_FlowMagnitude");
      this.m_FlowVariation = this.serializedObject.FindProperty("m_FlowVariation");
    }

    public override void OnDisable()
    {
      base.OnDisable();
      BuoyancyEffector2DEditor.m_ShowDampingRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      BuoyancyEffector2DEditor.m_ShowFlowRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Density);
      EditorGUILayout.PropertyField(this.m_SurfaceLevel);
      BuoyancyEffector2DEditor.m_ShowDampingRollout.target = EditorGUILayout.Foldout(BuoyancyEffector2DEditor.m_ShowDampingRollout.target, "Damping", true);
      if (EditorGUILayout.BeginFadeGroup(BuoyancyEffector2DEditor.m_ShowDampingRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_LinearDrag);
        EditorGUILayout.PropertyField(this.m_AngularDrag);
        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFadeGroup();
      BuoyancyEffector2DEditor.m_ShowFlowRollout.target = EditorGUILayout.Foldout(BuoyancyEffector2DEditor.m_ShowFlowRollout.target, "Flow", true);
      if (EditorGUILayout.BeginFadeGroup(BuoyancyEffector2DEditor.m_ShowFlowRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_FlowAngle);
        EditorGUILayout.PropertyField(this.m_FlowMagnitude);
        EditorGUILayout.PropertyField(this.m_FlowVariation);
      }
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
      BuoyancyEffector2D target = (BuoyancyEffector2D) this.target;
      if (!target.enabled)
        return;
      float y = target.transform.position.y + target.transform.lossyScale.y * target.surfaceLevel;
      List<Vector3> vector3List = new List<Vector3>();
      float num = float.NegativeInfinity;
      float x1 = num;
      foreach (Collider2D collider2D in ((IEnumerable<Collider2D>) target.gameObject.GetComponents<Collider2D>()).Where<Collider2D>((Func<Collider2D, bool>) (c => c.enabled && c.usedByEffector)))
      {
        Bounds bounds = collider2D.bounds;
        float x2 = bounds.min.x;
        float x3 = bounds.max.x;
        if (float.IsNegativeInfinity(num))
        {
          num = x2;
          x1 = x3;
        }
        else
        {
          if ((double) x2 < (double) num)
            num = x2;
          if ((double) x3 > (double) x1)
            x1 = x3;
        }
        Vector3 vector3_1 = new Vector3(x2, y, 0.0f);
        Vector3 vector3_2 = new Vector3(x3, y, 0.0f);
        vector3List.Add(vector3_1);
        vector3List.Add(vector3_2);
      }
      Handles.color = Color.red;
      Handles.DrawAAPolyLine(new Vector3[2]
      {
        new Vector3(num, y, 0.0f),
        new Vector3(x1, y, 0.0f)
      });
      Handles.color = Color.cyan;
      int index = 0;
      while (index < vector3List.Count - 1)
      {
        Handles.DrawAAPolyLine(new Vector3[2]
        {
          vector3List[index],
          vector3List[index + 1]
        });
        index += 2;
      }
    }
  }
}
