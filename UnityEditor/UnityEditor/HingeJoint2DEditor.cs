// Decompiled with JetBrains decompiler
// Type: UnityEditor.HingeJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (HingeJoint2D))]
  internal class HingeJoint2DEditor : AnchoredJoint2DEditor
  {
    private static readonly Quaternion s_RightHandedHandleOrientationOffset = Quaternion.AngleAxis(180f, Vector3.right) * Quaternion.AngleAxis(90f, Vector3.up);
    private static readonly Quaternion s_LeftHandedHandleOrientationOffset = Quaternion.AngleAxis(180f, Vector3.forward) * Quaternion.AngleAxis(90f, Vector3.up);
    private JointAngularLimitHandle m_AngularLimitHandle = new JointAngularLimitHandle();

    public new void OnEnable()
    {
      base.OnEnable();
      this.m_AngularLimitHandle.xHandleColor = Color.white;
      this.m_AngularLimitHandle.yHandleColor = Color.clear;
      this.m_AngularLimitHandle.zHandleColor = Color.clear;
      this.m_AngularLimitHandle.yMotion = ConfigurableJointMotion.Locked;
      this.m_AngularLimitHandle.zMotion = ConfigurableJointMotion.Locked;
      this.m_AngularLimitHandle.xRange = new Vector2(-1000000f, 1000000f);
    }

    public override void OnInspectorGUI()
    {
      HingeJoint2D target = (HingeJoint2D) this.target;
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.JointAngularLimits, "Edit Joint Angular Limits", HingeJoint2DEditor.Styles.editAngularLimitsButton, (IToolModeOwner) this);
      base.OnInspectorGUI();
    }

    internal override Bounds GetWorldBoundsOfTarget(Object targetObject)
    {
      Bounds worldBoundsOfTarget = base.GetWorldBoundsOfTarget(targetObject);
      HingeJoint2D hingeJoint2D = (HingeJoint2D) targetObject;
      worldBoundsOfTarget.Encapsulate(Joint2DEditor.TransformPoint(hingeJoint2D.transform, (Vector3) hingeJoint2D.anchor));
      return worldBoundsOfTarget;
    }

    private void NonEditableHandleDrawFunction(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
    }

    public new void OnSceneGUI()
    {
      HingeJoint2D target = (HingeJoint2D) this.target;
      if (!target.enabled)
        return;
      this.m_AngularLimitHandle.xMotion = !target.useLimits ? ConfigurableJointMotion.Free : ConfigurableJointMotion.Limited;
      JointAngleLimits2D limits1 = target.limits;
      this.m_AngularLimitHandle.xMin = limits1.min;
      this.m_AngularLimitHandle.xMax = limits1.max;
      this.m_AngularLimitHandle.angleHandleDrawFunction = UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.JointAngularLimits || !UnityEditorInternal.EditMode.IsOwner((Editor) this) ? new Handles.CapFunction(this.NonEditableHandleDrawFunction) : (Handles.CapFunction) null;
      Rigidbody2D rigidbody2D1 = target.attachedRigidbody;
      Vector3 vector3 = Vector3.right;
      Vector2 vector2 = target.anchor;
      Rigidbody2D rigidbody2D2 = target.connectedBody;
      Quaternion orientationOffset = HingeJoint2DEditor.s_RightHandedHandleOrientationOffset;
      if (rigidbody2D1.bodyType != RigidbodyType2D.Dynamic && (Object) target.connectedBody != (Object) null && target.connectedBody.bodyType == RigidbodyType2D.Dynamic)
      {
        rigidbody2D1 = target.connectedBody;
        vector3 = Vector3.left;
        vector2 = target.connectedAnchor;
        rigidbody2D2 = target.attachedRigidbody;
        orientationOffset = HingeJoint2DEditor.s_LeftHandedHandleOrientationOffset;
      }
      Vector3 pos = Joint2DEditor.TransformPoint(rigidbody2D1.transform, (Vector3) vector2);
      Quaternion q = (!((Object) rigidbody2D2 == (Object) null) ? Quaternion.LookRotation(Vector3.forward, rigidbody2D2.transform.rotation * Vector3.up) : Quaternion.identity) * orientationOffset;
      Vector3 point = pos + Quaternion.LookRotation(Vector3.forward, rigidbody2D1.transform.rotation * Vector3.up) * vector3;
      Matrix4x4 matrix = Matrix4x4.TRS(pos, q, Vector3.one);
      EditorGUI.BeginChangeCheck();
      using (new Handles.DrawingScope(HingeJoint2DEditor.Styles.handleColor, matrix))
      {
        float num = HandleUtility.GetHandleSize(Vector3.zero) * HingeJoint2DEditor.Styles.handleRadius;
        this.m_AngularLimitHandle.radius = num;
        Handles.DrawLine(Vector3.zero, matrix.inverse.MultiplyPoint3x4(point).normalized * num);
        this.m_AngularLimitHandle.DrawHandle();
      }
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject((Object) target, HingeJoint2DEditor.Styles.editAngularLimitsUndoMessage);
        JointAngleLimits2D limits2 = target.limits;
        limits2.min = this.m_AngularLimitHandle.xMin;
        limits2.max = this.m_AngularLimitHandle.xMax;
        target.limits = limits2;
        rigidbody2D1.WakeUp();
      }
      base.OnSceneGUI();
    }

    protected static class Styles
    {
      public static readonly GUIContent editAngularLimitsButton = new GUIContent(EditorGUIUtility.IconContent("JointAngularLimits"));
      public static readonly string editAngularLimitsUndoMessage = EditorGUIUtility.TextContent("Change Joint Angular Limits").text;
      public static readonly Color handleColor = new Color(0.0f, 1f, 0.0f, 0.7f);
      public static readonly float handleRadius = 0.8f;

      static Styles()
      {
        HingeJoint2DEditor.Styles.editAngularLimitsButton.tooltip = EditorGUIUtility.TextContent("Edit joint angular limits.").text;
      }
    }
  }
}
