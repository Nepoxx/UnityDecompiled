// Decompiled with JetBrains decompiler
// Type: UnityEditor.JointEditor`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class JointEditor<T> : Editor where T : Joint
  {
    private JointAngularLimitHandle m_AngularLimitHandle = new JointAngularLimitHandle();

    protected static float GetAngularLimitHandleSize(Vector3 position)
    {
      return HandleUtility.GetHandleSize(position);
    }

    protected JointAngularLimitHandle angularLimitHandle
    {
      get
      {
        return this.m_AngularLimitHandle;
      }
    }

    protected bool editingAngularLimits
    {
      get
      {
        return UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.JointAngularLimits && UnityEditorInternal.EditMode.IsOwner((Editor) this);
      }
    }

    public override void OnInspectorGUI()
    {
      this.DoInspectorEditButtons();
      base.OnInspectorGUI();
    }

    protected void DoInspectorEditButtons()
    {
      T target = (T) this.target;
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.JointAngularLimits, "Edit Joint Angular Limits", JointEditor<T>.Styles.editAngularLimitsButton, (IToolModeOwner) this);
    }

    internal override Bounds GetWorldBoundsOfTarget(Object targetObject)
    {
      Bounds worldBoundsOfTarget = base.GetWorldBoundsOfTarget(targetObject);
      worldBoundsOfTarget.Encapsulate(this.GetAngularLimitHandleMatrix((T) targetObject).MultiplyPoint3x4(Vector3.zero));
      return worldBoundsOfTarget;
    }

    protected virtual void OnSceneGUI()
    {
      if (!this.editingAngularLimits)
        return;
      T target = (T) this.target;
      EditorGUI.BeginChangeCheck();
      using (new Handles.DrawingScope(this.GetAngularLimitHandleMatrix(target)))
        this.DoAngularLimitHandles(target);
      if (EditorGUI.EndChangeCheck())
      {
        Rigidbody component = target.GetComponent<Rigidbody>();
        if (component.isKinematic && (Object) target.connectedBody != (Object) null)
          target.connectedBody.WakeUp();
        else
          component.WakeUp();
      }
    }

    protected virtual void GetActors(T joint, out Rigidbody dynamicActor, out Rigidbody connectedActor, out int jointFrameActorIndex, out bool rightHandedLimit)
    {
      jointFrameActorIndex = 1;
      rightHandedLimit = false;
      dynamicActor = joint.GetComponent<Rigidbody>();
      connectedActor = joint.connectedBody;
      if (!dynamicActor.isKinematic || !((Object) connectedActor != (Object) null) || connectedActor.isKinematic)
        return;
      Rigidbody rigidbody = connectedActor;
      connectedActor = dynamicActor;
      dynamicActor = rigidbody;
    }

    private Matrix4x4 GetAngularLimitHandleMatrix(T joint)
    {
      Rigidbody dynamicActor;
      Rigidbody connectedActor;
      int jointFrameActorIndex;
      bool rightHandedLimit;
      this.GetActors(joint, out dynamicActor, out connectedActor, out jointFrameActorIndex, out rightHandedLimit);
      Quaternion quaternion1 = !((Object) connectedActor == (Object) null) ? connectedActor.transform.rotation : Quaternion.identity;
      Matrix4x4 actorLocalPose = joint.GetActorLocalPose(jointFrameActorIndex);
      Quaternion quaternion2 = Quaternion.LookRotation(actorLocalPose.MultiplyVector(Vector3.forward), actorLocalPose.MultiplyVector(!rightHandedLimit ? Vector3.up : Vector3.down));
      Vector3 vector3 = joint.anchor;
      if ((Object) dynamicActor != (Object) null)
        vector3 = dynamicActor.transform.TransformPoint(vector3);
      return Matrix4x4.TRS(vector3, quaternion1 * quaternion2, Vector3.one);
    }

    protected virtual void DoAngularLimitHandles(T joint)
    {
    }

    protected static class Styles
    {
      public static readonly GUIContent editAngularLimitsButton = new GUIContent(EditorGUIUtility.IconContent("JointAngularLimits"));
      public static readonly string editAngularLimitsUndoMessage = EditorGUIUtility.TextContent("Change Joint Angular Limits").text;

      static Styles()
      {
        JointEditor<T>.Styles.editAngularLimitsButton.tooltip = EditorGUIUtility.TextContent("Edit joint angular limits.").text;
      }
    }
  }
}
