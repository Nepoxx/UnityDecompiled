// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collider2DEditorBase
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
  [CanEditMultipleObjects]
  internal abstract class Collider2DEditorBase : ColliderEditorBase
  {
    private static ContactPoint2D[] m_Contacts = new ContactPoint2D[100];
    private readonly AnimBool m_ShowDensity = new AnimBool();
    private readonly AnimBool m_ShowInfo = new AnimBool();
    private readonly AnimBool m_ShowContacts = new AnimBool();
    private readonly AnimBool m_ShowCompositeRedundants = new AnimBool();
    private SerializedProperty m_Density;
    private Vector2 m_ContactScrollPosition;
    private SerializedProperty m_Material;
    private SerializedProperty m_IsTrigger;
    private SerializedProperty m_UsedByEffector;
    private SerializedProperty m_UsedByComposite;
    private SerializedProperty m_Offset;
    protected SerializedProperty m_AutoTiling;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Density = this.serializedObject.FindProperty("m_Density");
      this.m_ShowDensity.value = this.ShouldShowDensity();
      this.m_ShowDensity.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowInfo.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowContacts.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ContactScrollPosition = Vector2.zero;
      this.m_Material = this.serializedObject.FindProperty("m_Material");
      this.m_IsTrigger = this.serializedObject.FindProperty("m_IsTrigger");
      this.m_UsedByEffector = this.serializedObject.FindProperty("m_UsedByEffector");
      this.m_UsedByComposite = this.serializedObject.FindProperty("m_UsedByComposite");
      this.m_Offset = this.serializedObject.FindProperty("m_Offset");
      this.m_AutoTiling = this.serializedObject.FindProperty("m_AutoTiling");
      this.m_ShowCompositeRedundants.value = !this.m_UsedByComposite.boolValue;
      this.m_ShowCompositeRedundants.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnDisable()
    {
      this.m_ShowDensity.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowInfo.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowContacts.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCompositeRedundants.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      base.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      this.m_ShowCompositeRedundants.target = !this.m_UsedByComposite.boolValue;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCompositeRedundants.faded))
      {
        this.m_ShowDensity.target = this.ShouldShowDensity();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowDensity.faded))
          EditorGUILayout.PropertyField(this.m_Density);
        Collider2DEditorBase.FixedEndFadeGroup(this.m_ShowDensity.faded);
        EditorGUILayout.PropertyField(this.m_Material);
        EditorGUILayout.PropertyField(this.m_IsTrigger);
        EditorGUILayout.PropertyField(this.m_UsedByEffector);
      }
      Collider2DEditorBase.FixedEndFadeGroup(this.m_ShowCompositeRedundants.faded);
      if (((IEnumerable<UnityEngine.Object>) this.targets).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => !(x as Collider2D).compositeCapable)).Count<UnityEngine.Object>() == 0)
        EditorGUILayout.PropertyField(this.m_UsedByComposite);
      if (this.m_AutoTiling != null)
        EditorGUILayout.PropertyField(this.m_AutoTiling, Collider2DEditorBase.Styles.s_AutoTilingLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Offset);
    }

    public void FinalizeInspectorGUI()
    {
      this.ShowColliderInfoProperties();
      this.CheckColliderErrorState();
      if (this.targets.Length == 1)
      {
        Collider2D target = this.target as Collider2D;
        if (target.isActiveAndEnabled && (UnityEngine.Object) target.composite == (UnityEngine.Object) null && this.m_UsedByComposite.boolValue)
          EditorGUILayout.HelpBox("This collider will not function with a composite until there is a CompositeCollider2D on the GameObject that the attached Rigidbody2D is on.", MessageType.Warning);
      }
      Effector2DEditor.CheckEffectorWarnings(this.target as Collider2D);
    }

    private void ShowColliderInfoProperties()
    {
      this.m_ShowInfo.target = EditorGUILayout.Foldout(this.m_ShowInfo.target, "Info", true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowInfo.faded))
      {
        if (this.targets.Length == 1)
        {
          Collider2D target = this.targets[0] as Collider2D;
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.ObjectField("Attached Body", (UnityEngine.Object) target.attachedRigidbody, typeof (Rigidbody2D), false, new GUILayoutOption[0]);
          double num1 = (double) EditorGUILayout.FloatField("Friction", target.friction, new GUILayoutOption[0]);
          double num2 = (double) EditorGUILayout.FloatField("Bounciness", target.bounciness, new GUILayoutOption[0]);
          double num3 = (double) EditorGUILayout.FloatField("Shape Count", (float) target.shapeCount, new GUILayoutOption[0]);
          if (target.isActiveAndEnabled)
            EditorGUILayout.BoundsField("Bounds", target.bounds, new GUILayoutOption[0]);
          EditorGUI.EndDisabledGroup();
          this.ShowContacts(target);
          this.Repaint();
        }
        else
          EditorGUILayout.HelpBox("Cannot show Info properties when multiple colliders are selected.", MessageType.Info);
      }
      EditorGUILayout.EndFadeGroup();
    }

    private bool ShouldShowDensity()
    {
      if (((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, Rigidbody2D>((Func<UnityEngine.Object, Rigidbody2D>) (x => (x as Collider2D).attachedRigidbody)).Distinct<Rigidbody2D>().Count<Rigidbody2D>() > 1)
        return false;
      Rigidbody2D attachedRigidbody = (this.target as Collider2D).attachedRigidbody;
      return (bool) ((UnityEngine.Object) attachedRigidbody) && attachedRigidbody.useAutoMass && attachedRigidbody.bodyType == RigidbodyType2D.Dynamic;
    }

    private void ShowContacts(Collider2D collider)
    {
      ++EditorGUI.indentLevel;
      this.m_ShowContacts.target = EditorGUILayout.Foldout(this.m_ShowContacts.target, "Contacts");
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowContacts.faded))
      {
        int contacts = collider.GetContacts(Collider2DEditorBase.m_Contacts);
        if (contacts > 0)
        {
          this.m_ContactScrollPosition = EditorGUILayout.BeginScrollView(this.m_ContactScrollPosition, GUILayout.Height(180f));
          EditorGUI.BeginDisabledGroup(true);
          for (int index = 0; index < contacts; ++index)
          {
            ContactPoint2D contact = Collider2DEditorBase.m_Contacts[index];
            EditorGUILayout.HelpBox(string.Format("Contact#{0}", (object) index), MessageType.None);
            ++EditorGUI.indentLevel;
            EditorGUILayout.Vector2Field("Point", contact.point);
            EditorGUILayout.Vector2Field("Normal", contact.normal);
            EditorGUILayout.Vector2Field("Relative Velocity", contact.relativeVelocity);
            double num1 = (double) EditorGUILayout.FloatField("Normal Impulse", contact.normalImpulse, new GUILayoutOption[0]);
            double num2 = (double) EditorGUILayout.FloatField("Tangent Impulse", contact.tangentImpulse, new GUILayoutOption[0]);
            EditorGUILayout.ObjectField("Collider", (UnityEngine.Object) contact.collider, typeof (Collider2D), false, new GUILayoutOption[0]);
            EditorGUILayout.ObjectField("Rigidbody", (UnityEngine.Object) contact.rigidbody, typeof (Rigidbody2D), false, new GUILayoutOption[0]);
            EditorGUILayout.ObjectField("OtherRigidbody", (UnityEngine.Object) contact.otherRigidbody, typeof (Rigidbody2D), false, new GUILayoutOption[0]);
            --EditorGUI.indentLevel;
            EditorGUILayout.Space();
          }
          EditorGUI.EndDisabledGroup();
          EditorGUILayout.EndScrollView();
        }
        else
          EditorGUILayout.HelpBox("No Contacts", MessageType.Info);
      }
      Collider2DEditorBase.FixedEndFadeGroup(this.m_ShowContacts.faded);
      --EditorGUI.indentLevel;
    }

    private static void FixedEndFadeGroup(float value)
    {
      if ((double) value == 0.0 || (double) value == 1.0)
        return;
      EditorGUILayout.EndFadeGroup();
    }

    internal override void OnForceReloadInspector()
    {
      base.OnForceReloadInspector();
      if (!this.editingCollider)
        return;
      UnityEditorInternal.EditMode.QuitEditMode();
    }

    protected void CheckColliderErrorState()
    {
      switch ((this.target as Collider2D).errorState)
      {
        case ColliderErrorState2D.NoShapes:
          EditorGUILayout.HelpBox("The collider did not create any collision shapes as they all failed verification.  This could be because they were deemed too small or the vertices were too close.  Vertices can also become close under certain rotations or very small scaling.", MessageType.Warning);
          break;
        case ColliderErrorState2D.RemovedShapes:
          EditorGUILayout.HelpBox("The collider created collision shape(s) but some were removed as they failed verification.  This could be because they were deemed too small or the vertices were too close.  Vertices can also become close under certain rotations or very small scaling.", MessageType.Warning);
          break;
      }
    }

    protected void BeginColliderInspector()
    {
      this.serializedObject.Update();
      using (new EditorGUI.DisabledScope(this.targets.Length > 1))
        this.InspectorEditButtonGUI();
    }

    protected void EndColliderInspector()
    {
      this.serializedObject.ApplyModifiedProperties();
    }

    protected bool CanEditCollider()
    {
      return !(bool) ((IEnumerable<UnityEngine.Object>) this.targets).FirstOrDefault<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x =>
      {
        SpriteRenderer component = (x as Component).GetComponent<SpriteRenderer>();
        return (UnityEngine.Object) component != (UnityEngine.Object) null && component.drawMode != SpriteDrawMode.Simple && this.m_AutoTiling.boolValue;
      }));
    }

    protected class Styles
    {
      public static readonly GUIContent s_ColliderEditDisableHelp = EditorGUIUtility.TextContent("Collider cannot be edited because it is driven by SpriteRenderer's tiling properties.");
      public static readonly GUIContent s_AutoTilingLabel = EditorGUIUtility.TextContent("Auto Tiling | When enabled, the collider's shape will update automaticaly based on the SpriteRenderer's tiling properties");
    }
  }
}
