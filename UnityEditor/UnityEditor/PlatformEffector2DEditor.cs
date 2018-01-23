// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlatformEffector2DEditor
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
  [CustomEditor(typeof (PlatformEffector2D), true)]
  [CanEditMultipleObjects]
  internal class PlatformEffector2DEditor : Effector2DEditor
  {
    private static readonly AnimBool m_ShowSidesRollout = new AnimBool();
    private readonly AnimBool m_ShowOneWayRollout = new AnimBool();
    private SerializedProperty m_RotationalOffset;
    private SerializedProperty m_UseOneWay;
    private SerializedProperty m_UseOneWayGrouping;
    private SerializedProperty m_SurfaceArc;
    private SerializedProperty m_UseSideFriction;
    private SerializedProperty m_UseSideBounce;
    private SerializedProperty m_SideArc;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_RotationalOffset = this.serializedObject.FindProperty("m_RotationalOffset");
      this.m_ShowOneWayRollout.value = true;
      this.m_ShowOneWayRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_UseOneWay = this.serializedObject.FindProperty("m_UseOneWay");
      this.m_UseOneWayGrouping = this.serializedObject.FindProperty("m_UseOneWayGrouping");
      this.m_SurfaceArc = this.serializedObject.FindProperty("m_SurfaceArc");
      PlatformEffector2DEditor.m_ShowSidesRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_UseSideFriction = this.serializedObject.FindProperty("m_UseSideFriction");
      this.m_UseSideBounce = this.serializedObject.FindProperty("m_UseSideBounce");
      this.m_SideArc = this.serializedObject.FindProperty("m_SideArc");
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowOneWayRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      PlatformEffector2DEditor.m_ShowSidesRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_RotationalOffset);
      this.m_ShowOneWayRollout.target = EditorGUILayout.Foldout(this.m_ShowOneWayRollout.target, "One Way", true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowOneWayRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_UseOneWay);
        EditorGUILayout.PropertyField(this.m_UseOneWayGrouping);
        EditorGUILayout.PropertyField(this.m_SurfaceArc);
        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFadeGroup();
      PlatformEffector2DEditor.m_ShowSidesRollout.target = EditorGUILayout.Foldout(PlatformEffector2DEditor.m_ShowSidesRollout.target, "Sides", true);
      if (EditorGUILayout.BeginFadeGroup(PlatformEffector2DEditor.m_ShowSidesRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_UseSideFriction);
        EditorGUILayout.PropertyField(this.m_UseSideBounce);
        EditorGUILayout.PropertyField(this.m_SideArc);
      }
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
      PlatformEffector2D target = (PlatformEffector2D) this.target;
      if (!target.enabled)
        return;
      if (target.useOneWay)
        PlatformEffector2DEditor.DrawSurfaceArc(target);
      if (target.useSideBounce && target.useSideFriction)
        return;
      PlatformEffector2DEditor.DrawSideArc(target);
    }

    private static void DrawSurfaceArc(PlatformEffector2D effector)
    {
      float f = -1f * (float) Math.PI / 180f * effector.rotationalOffset;
      Vector3 normalized = effector.transform.TransformVector(new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0.0f)).normalized;
      if ((double) normalized.sqrMagnitude < (double) Mathf.Epsilon)
        return;
      float num1 = Mathf.Atan2(normalized.x, normalized.y);
      float angle = Mathf.Clamp(effector.surfaceArc, 0.5f, 360f);
      float num2 = (float) ((double) angle * 0.5 * (Math.PI / 180.0));
      Vector3 from = new Vector3(Mathf.Sin(num1 - num2), Mathf.Cos(num1 - num2), 0.0f);
      Vector3 vector3 = new Vector3(Mathf.Sin(num1 + num2), Mathf.Cos(num1 + num2), 0.0f);
      foreach (Collider2D collider2D in ((IEnumerable<Collider2D>) effector.gameObject.GetComponents<Collider2D>()).Where<Collider2D>((Func<Collider2D, bool>) (collider => collider.enabled && collider.usedByEffector)))
      {
        Vector3 center = collider2D.bounds.center;
        float handleSize = HandleUtility.GetHandleSize(center);
        Handles.color = new Color(0.0f, 1f, 1f, 0.07f);
        Handles.DrawSolidArc(center, Vector3.back, from, angle, handleSize);
        Handles.color = new Color(0.0f, 1f, 1f, 0.7f);
        Handles.DrawWireArc(center, Vector3.back, from, angle, handleSize);
        Handles.DrawDottedLine(center, center + from * handleSize, 5f);
        Handles.DrawDottedLine(center, center + vector3 * handleSize, 5f);
      }
    }

    private static void DrawSideArc(PlatformEffector2D effector)
    {
      float f = (float) (-1.0 * Math.PI / 180.0 * (90.0 + (double) effector.rotationalOffset));
      Vector3 normalized = effector.transform.TransformVector(new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0.0f)).normalized;
      if ((double) normalized.sqrMagnitude < (double) Mathf.Epsilon)
        return;
      float num1 = Mathf.Atan2(normalized.x, normalized.y);
      float num2 = num1 + 3.141593f;
      float angle = Mathf.Clamp(effector.sideArc, 0.5f, 180f);
      float num3 = (float) ((double) angle * 0.5 * (Math.PI / 180.0));
      Vector3 from1 = new Vector3(Mathf.Sin(num1 - num3), Mathf.Cos(num1 - num3), 0.0f);
      Vector3 vector3_1 = new Vector3(Mathf.Sin(num1 + num3), Mathf.Cos(num1 + num3), 0.0f);
      Vector3 from2 = new Vector3(Mathf.Sin(num2 - num3), Mathf.Cos(num2 - num3), 0.0f);
      Vector3 vector3_2 = new Vector3(Mathf.Sin(num2 + num3), Mathf.Cos(num2 + num3), 0.0f);
      foreach (Collider2D collider2D in ((IEnumerable<Collider2D>) effector.gameObject.GetComponents<Collider2D>()).Where<Collider2D>((Func<Collider2D, bool>) (collider => collider.enabled && collider.usedByEffector)))
      {
        Vector3 center = collider2D.bounds.center;
        float radius = HandleUtility.GetHandleSize(center) * 0.8f;
        Handles.color = new Color(0.0f, 1f, 0.7f, 0.07f);
        Handles.DrawSolidArc(center, Vector3.back, from1, angle, radius);
        Handles.DrawSolidArc(center, Vector3.back, from2, angle, radius);
        Handles.color = new Color(0.0f, 1f, 0.7f, 0.7f);
        Handles.DrawWireArc(center, Vector3.back, from1, angle, radius);
        Handles.DrawWireArc(center, Vector3.back, from2, angle, radius);
        Handles.DrawDottedLine(center, center + from1 * radius, 5f);
        Handles.DrawDottedLine(center, center + vector3_1 * radius, 5f);
        Handles.DrawDottedLine(center, center + from2 * radius, 5f);
        Handles.DrawDottedLine(center, center + vector3_2 * radius, 5f);
      }
    }
  }
}
