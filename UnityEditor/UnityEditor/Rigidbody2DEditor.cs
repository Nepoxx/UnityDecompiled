// Decompiled with JetBrains decompiler
// Type: UnityEditor.Rigidbody2DEditor
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
  [CustomEditor(typeof (Rigidbody2D))]
  [CanEditMultipleObjects]
  internal class Rigidbody2DEditor : Editor
  {
    private static readonly GUIContent m_FreezePositionLabel = new GUIContent("Freeze Position");
    private static readonly GUIContent m_FreezeRotationLabel = new GUIContent("Freeze Rotation");
    private static ContactPoint2D[] m_Contacts = new ContactPoint2D[100];
    private readonly AnimBool m_ShowIsStatic = new AnimBool();
    private readonly AnimBool m_ShowIsKinematic = new AnimBool();
    private readonly AnimBool m_ShowInfo = new AnimBool();
    private readonly AnimBool m_ShowContacts = new AnimBool();
    private SerializedProperty m_Simulated;
    private SerializedProperty m_BodyType;
    private SerializedProperty m_Material;
    private SerializedProperty m_UseFullKinematicContacts;
    private SerializedProperty m_UseAutoMass;
    private SerializedProperty m_Mass;
    private SerializedProperty m_LinearDrag;
    private SerializedProperty m_AngularDrag;
    private SerializedProperty m_GravityScale;
    private SerializedProperty m_Interpolate;
    private SerializedProperty m_SleepingMode;
    private SerializedProperty m_CollisionDetection;
    private SerializedProperty m_Constraints;
    private Vector2 m_ContactScrollPosition;
    private const int k_ToggleOffset = 30;

    public void OnEnable()
    {
      Rigidbody2D target = this.target as Rigidbody2D;
      this.m_Simulated = this.serializedObject.FindProperty("m_Simulated");
      this.m_BodyType = this.serializedObject.FindProperty("m_BodyType");
      this.m_Material = this.serializedObject.FindProperty("m_Material");
      this.m_UseFullKinematicContacts = this.serializedObject.FindProperty("m_UseFullKinematicContacts");
      this.m_UseAutoMass = this.serializedObject.FindProperty("m_UseAutoMass");
      this.m_Mass = this.serializedObject.FindProperty("m_Mass");
      this.m_LinearDrag = this.serializedObject.FindProperty("m_LinearDrag");
      this.m_AngularDrag = this.serializedObject.FindProperty("m_AngularDrag");
      this.m_GravityScale = this.serializedObject.FindProperty("m_GravityScale");
      this.m_Interpolate = this.serializedObject.FindProperty("m_Interpolate");
      this.m_SleepingMode = this.serializedObject.FindProperty("m_SleepingMode");
      this.m_CollisionDetection = this.serializedObject.FindProperty("m_CollisionDetection");
      this.m_Constraints = this.serializedObject.FindProperty("m_Constraints");
      this.m_ShowIsStatic.value = target.bodyType != RigidbodyType2D.Static;
      this.m_ShowIsStatic.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowIsKinematic.value = target.bodyType != RigidbodyType2D.Kinematic;
      this.m_ShowIsKinematic.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowInfo.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowContacts.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ContactScrollPosition = Vector2.zero;
    }

    public void OnDisable()
    {
      this.m_ShowIsStatic.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowIsKinematic.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowInfo.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowContacts.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      Rigidbody2D target = this.target as Rigidbody2D;
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_BodyType);
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Simulated);
      if (!this.m_Simulated.boolValue && !this.m_Simulated.hasMultipleDifferentValues)
        EditorGUILayout.HelpBox("The body has now been taken out of the simulation along with any attached colliders, joints or effectors.", MessageType.Info);
      if (this.m_BodyType.hasMultipleDifferentValues)
      {
        EditorGUILayout.HelpBox("Cannot edit properties that are body type specific when the selection contains different body types.", MessageType.Info);
      }
      else
      {
        this.m_ShowIsStatic.target = target.bodyType != RigidbodyType2D.Static;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowIsStatic.faded))
        {
          this.m_ShowIsKinematic.target = target.bodyType != RigidbodyType2D.Kinematic;
          if (EditorGUILayout.BeginFadeGroup(this.m_ShowIsKinematic.faded))
          {
            EditorGUILayout.PropertyField(this.m_UseAutoMass);
            if (!this.m_UseAutoMass.hasMultipleDifferentValues)
            {
              if (this.m_UseAutoMass.boolValue && ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => PrefabUtility.GetPrefabType(x) == PrefabType.Prefab || !(x as Rigidbody2D).gameObject.activeInHierarchy)))
              {
                EditorGUILayout.HelpBox("The auto mass value cannot be displayed for a prefab or if the object is not active.  The value will be calculated for a prefab instance and when the object is active.", MessageType.Info);
              }
              else
              {
                EditorGUI.BeginDisabledGroup(target.useAutoMass);
                EditorGUILayout.PropertyField(this.m_Mass);
                EditorGUI.EndDisabledGroup();
              }
            }
            EditorGUILayout.PropertyField(this.m_LinearDrag);
            EditorGUILayout.PropertyField(this.m_AngularDrag);
            EditorGUILayout.PropertyField(this.m_GravityScale);
          }
          Rigidbody2DEditor.FixedEndFadeGroup(this.m_ShowIsKinematic.faded);
          if (!this.m_ShowIsKinematic.target)
            EditorGUILayout.PropertyField(this.m_UseFullKinematicContacts);
          EditorGUILayout.PropertyField(this.m_CollisionDetection);
          EditorGUILayout.PropertyField(this.m_SleepingMode);
          EditorGUILayout.PropertyField(this.m_Interpolate);
          GUILayout.BeginHorizontal();
          this.m_Constraints.isExpanded = EditorGUILayout.Foldout(this.m_Constraints.isExpanded, "Constraints", true);
          GUILayout.EndHorizontal();
          RigidbodyConstraints2D intValue = (RigidbodyConstraints2D) this.m_Constraints.intValue;
          if (this.m_Constraints.isExpanded)
          {
            ++EditorGUI.indentLevel;
            this.ToggleFreezePosition(intValue, Rigidbody2DEditor.m_FreezePositionLabel, 0, 1);
            this.ToggleFreezeRotation(intValue, Rigidbody2DEditor.m_FreezeRotationLabel, 2);
            --EditorGUI.indentLevel;
          }
          if (intValue == RigidbodyConstraints2D.FreezeAll)
            EditorGUILayout.HelpBox("Rather than turning on all constraints, you may want to consider removing the Rigidbody2D component which makes any colliders static.  This gives far better performance overall.", MessageType.Info);
        }
        Rigidbody2DEditor.FixedEndFadeGroup(this.m_ShowIsStatic.faded);
      }
      this.serializedObject.ApplyModifiedProperties();
      this.ShowBodyInfoProperties();
    }

    private void ShowBodyInfoProperties()
    {
      this.m_ShowInfo.target = EditorGUILayout.Foldout(this.m_ShowInfo.target, "Info", true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowInfo.faded))
      {
        if (this.targets.Length == 1)
        {
          Rigidbody2D target = this.targets[0] as Rigidbody2D;
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.Vector2Field("Position", target.position);
          double num1 = (double) EditorGUILayout.FloatField("Rotation", target.rotation, new GUILayoutOption[0]);
          EditorGUILayout.Vector2Field("Velocity", target.velocity);
          double num2 = (double) EditorGUILayout.FloatField("Angular Velocity", target.angularVelocity, new GUILayoutOption[0]);
          double num3 = (double) EditorGUILayout.FloatField("Inertia", target.inertia, new GUILayoutOption[0]);
          EditorGUILayout.Vector2Field("Local Center of Mass", target.centerOfMass);
          EditorGUILayout.Vector2Field("World Center of Mass", target.worldCenterOfMass);
          EditorGUILayout.LabelField("Sleep State", !target.IsSleeping() ? "Awake" : "Asleep", new GUILayoutOption[0]);
          EditorGUI.EndDisabledGroup();
          this.ShowContacts(target);
          this.Repaint();
        }
        else
          EditorGUILayout.HelpBox("Cannot show Info properties when multiple bodies are selected.", MessageType.Info);
      }
      Rigidbody2DEditor.FixedEndFadeGroup(this.m_ShowInfo.faded);
    }

    private void ShowContacts(Rigidbody2D body)
    {
      ++EditorGUI.indentLevel;
      this.m_ShowContacts.target = EditorGUILayout.Foldout(this.m_ShowContacts.target, "Contacts");
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowContacts.faded))
      {
        int contacts = body.GetContacts(Rigidbody2DEditor.m_Contacts);
        if (contacts > 0)
        {
          this.m_ContactScrollPosition = EditorGUILayout.BeginScrollView(this.m_ContactScrollPosition, GUILayout.Height(180f));
          EditorGUI.BeginDisabledGroup(true);
          for (int index = 0; index < contacts; ++index)
          {
            ContactPoint2D contact = Rigidbody2DEditor.m_Contacts[index];
            EditorGUILayout.HelpBox(string.Format("Contact#{0}", (object) index), MessageType.None);
            ++EditorGUI.indentLevel;
            EditorGUILayout.Vector2Field("Point", contact.point);
            EditorGUILayout.Vector2Field("Normal", contact.normal);
            EditorGUILayout.Vector2Field("Relative Velocity", contact.relativeVelocity);
            double num1 = (double) EditorGUILayout.FloatField("Normal Impulse", contact.normalImpulse, new GUILayoutOption[0]);
            double num2 = (double) EditorGUILayout.FloatField("Tangent Impulse", contact.tangentImpulse, new GUILayoutOption[0]);
            EditorGUILayout.ObjectField("Collider", (UnityEngine.Object) contact.collider, typeof (Collider2D), true, new GUILayoutOption[0]);
            EditorGUILayout.ObjectField("Rigidbody", (UnityEngine.Object) contact.rigidbody, typeof (Rigidbody2D), false, new GUILayoutOption[0]);
            EditorGUILayout.ObjectField("OtherCollider", (UnityEngine.Object) contact.otherCollider, typeof (Collider2D), false, new GUILayoutOption[0]);
            --EditorGUI.indentLevel;
            EditorGUILayout.Space();
          }
          EditorGUI.EndDisabledGroup();
          EditorGUILayout.EndScrollView();
        }
        else
          EditorGUILayout.HelpBox("No Contacts", MessageType.Info);
      }
      Rigidbody2DEditor.FixedEndFadeGroup(this.m_ShowContacts.faded);
      --EditorGUI.indentLevel;
    }

    private static void FixedEndFadeGroup(float value)
    {
      if ((double) value == 0.0 || (double) value == 1.0)
        return;
      EditorGUILayout.EndFadeGroup();
    }

    private void ConstraintToggle(Rect r, string label, RigidbodyConstraints2D value, int bit)
    {
      bool flag1 = (value & (RigidbodyConstraints2D) (1 << bit)) != RigidbodyConstraints2D.None;
      EditorGUI.showMixedValue = (this.m_Constraints.hasMultipleDifferentValuesBitwise & 1 << bit) != 0;
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      bool flag2 = EditorGUI.ToggleLeft(r, label, flag1);
      EditorGUI.indentLevel = indentLevel;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Edit Constraints2D");
        this.m_Constraints.SetBitAtIndexForAllTargetsImmediate(bit, flag2);
      }
      EditorGUI.showMixedValue = false;
    }

    private void ToggleFreezePosition(RigidbodyConstraints2D constraints, GUIContent label, int x, int y)
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(7231, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, label);
      r.width = 30f;
      this.ConstraintToggle(r, "X", constraints, x);
      r.x += 30f;
      this.ConstraintToggle(r, "Y", constraints, y);
      GUILayout.EndHorizontal();
    }

    private void ToggleFreezeRotation(RigidbodyConstraints2D constraints, GUIContent label, int z)
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(7231, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, label);
      r.width = 30f;
      this.ConstraintToggle(r, "Z", constraints, z);
      GUILayout.EndHorizontal();
    }
  }
}
