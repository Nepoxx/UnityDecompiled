// Decompiled with JetBrains decompiler
// Type: UnityEditor.Joint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Joint2D))]
  internal class Joint2DEditor : Editor
  {
    private SerializedProperty m_BreakForce;
    private SerializedProperty m_BreakTorque;
    protected static Joint2DEditor.Styles s_Styles;

    public void OnEnable()
    {
      this.m_BreakForce = this.serializedObject.FindProperty("m_BreakForce");
      this.m_BreakTorque = this.serializedObject.FindProperty("m_BreakTorque");
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(this.m_BreakForce);
      System.Type type = this.target.GetType();
      if (type != typeof (DistanceJoint2D) && type != typeof (TargetJoint2D) && type != typeof (SpringJoint2D))
        EditorGUILayout.PropertyField(this.m_BreakTorque);
      this.serializedObject.ApplyModifiedProperties();
    }

    protected bool HandleAnchor(ref Vector3 position, bool isConnectedAnchor)
    {
      if (Joint2DEditor.s_Styles == null)
        Joint2DEditor.s_Styles = new Joint2DEditor.Styles();
      Handles.CapFunction capFunction1;
      if (isConnectedAnchor)
      {
        // ISSUE: reference to a compiler-generated field
        if (Joint2DEditor.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Joint2DEditor.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(Joint2DEditor.ConnectedAnchorHandleCap);
        }
        // ISSUE: reference to a compiler-generated field
        capFunction1 = Joint2DEditor.\u003C\u003Ef__mg\u0024cache0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (Joint2DEditor.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Joint2DEditor.\u003C\u003Ef__mg\u0024cache1 = new Handles.CapFunction(Joint2DEditor.AnchorHandleCap);
        }
        // ISSUE: reference to a compiler-generated field
        capFunction1 = Joint2DEditor.\u003C\u003Ef__mg\u0024cache1;
      }
      Handles.CapFunction capFunction2 = capFunction1;
      int id = this.target.GetInstanceID() + (!isConnectedAnchor ? 0 : 1);
      EditorGUI.BeginChangeCheck();
      position = Handles.Slider2D(id, position, Vector3.back, Vector3.right, Vector3.up, 0.0f, capFunction2, Vector2.zero);
      return EditorGUI.EndChangeCheck();
    }

    public static void AnchorHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (controlID == GUIUtility.keyboardControl)
        Joint2DEditor.HandleCap(controlID, position, Joint2DEditor.s_Styles.anchorActive, eventType);
      else
        Joint2DEditor.HandleCap(controlID, position, Joint2DEditor.s_Styles.anchor, eventType);
    }

    public static void ConnectedAnchorHandleCap(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
      if (controlID == GUIUtility.keyboardControl)
        Joint2DEditor.HandleCap(controlID, position, Joint2DEditor.s_Styles.connectedAnchorActive, eventType);
      else
        Joint2DEditor.HandleCap(controlID, position, Joint2DEditor.s_Styles.connectedAnchor, eventType);
    }

    private static void HandleCap(int controlID, Vector3 position, GUIStyle guiStyle, EventType eventType)
    {
      if (eventType != EventType.Layout)
      {
        if (eventType != EventType.Repaint)
          return;
        Handles.BeginGUI();
        position = (Vector3) HandleUtility.WorldToGUIPoint(position);
        float fixedWidth = guiStyle.fixedWidth;
        float fixedHeight = guiStyle.fixedHeight;
        Rect position1 = new Rect(position.x - fixedWidth / 2f, position.y - fixedHeight / 2f, fixedWidth, fixedHeight);
        guiStyle.Draw(position1, GUIContent.none, controlID);
        Handles.EndGUI();
      }
      else
        HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangleInternal(position, Quaternion.identity, Vector2.zero));
    }

    public static void DrawAALine(Vector3 start, Vector3 end)
    {
      Handles.DrawAAPolyLine(new Vector3[2]
      {
        start,
        end
      });
    }

    public static void DrawDistanceGizmo(Vector3 anchor, Vector3 connectedAnchor, float distance)
    {
      Vector3 normalized = (anchor - connectedAnchor).normalized;
      Vector3 end = connectedAnchor + normalized * distance;
      Vector3 vector3 = Vector3.Cross(normalized, Vector3.forward) * (HandleUtility.GetHandleSize(connectedAnchor) * 0.16f);
      Handles.color = Color.green;
      Joint2DEditor.DrawAALine(anchor, end);
      Joint2DEditor.DrawAALine(connectedAnchor + vector3, connectedAnchor - vector3);
      Joint2DEditor.DrawAALine(end + vector3, end - vector3);
    }

    private static Matrix4x4 GetAnchorSpaceMatrix(Transform transform)
    {
      return Matrix4x4.TRS(transform.position, Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z), transform.lossyScale);
    }

    protected static Vector3 TransformPoint(Transform transform, Vector3 position)
    {
      return Joint2DEditor.GetAnchorSpaceMatrix(transform).MultiplyPoint(position);
    }

    protected static Vector3 InverseTransformPoint(Transform transform, Vector3 position)
    {
      return Joint2DEditor.GetAnchorSpaceMatrix(transform).inverse.MultiplyPoint(position);
    }

    protected static Vector3 SnapToSprite(SpriteRenderer spriteRenderer, Vector3 position, float snapDistance)
    {
      if ((UnityEngine.Object) spriteRenderer == (UnityEngine.Object) null)
        return position;
      snapDistance = HandleUtility.GetHandleSize(position) * snapDistance;
      float x = spriteRenderer.sprite.bounds.size.x / 2f;
      float y = spriteRenderer.sprite.bounds.size.y / 2f;
      Vector2[] vector2Array = new Vector2[9]{ new Vector2(-x, -y), new Vector2(0.0f, -y), new Vector2(x, -y), new Vector2(-x, 0.0f), new Vector2(0.0f, 0.0f), new Vector2(x, 0.0f), new Vector2(-x, y), new Vector2(0.0f, y), new Vector2(x, y) };
      foreach (Vector2 vector2 in vector2Array)
      {
        Vector3 vector3 = spriteRenderer.transform.TransformPoint((Vector3) vector2);
        if ((double) Vector2.Distance((Vector2) position, (Vector2) vector3) <= (double) snapDistance)
          return vector3;
      }
      return position;
    }

    protected static Vector3 SnapToPoint(Vector3 position, Vector3 snapPosition, float snapDistance)
    {
      snapDistance = HandleUtility.GetHandleSize(position) * snapDistance;
      return (double) Vector3.Distance(position, snapPosition) > (double) snapDistance ? position : snapPosition;
    }

    protected static Vector2 RotateVector2(Vector2 direction, float angle)
    {
      float f = (float) (Math.PI / 180.0 * -(double) angle);
      float num1 = Mathf.Cos(f);
      float num2 = Mathf.Sin(f);
      return new Vector2((float) ((double) direction.x * (double) num1 - (double) direction.y * (double) num2), (float) ((double) direction.x * (double) num2 + (double) direction.y * (double) num1));
    }

    public class Styles
    {
      public readonly GUIStyle anchor = (GUIStyle) "U2D.pivotDot";
      public readonly GUIStyle anchorActive = (GUIStyle) "U2D.pivotDotActive";
      public readonly GUIStyle connectedAnchor = (GUIStyle) "U2D.dragDot";
      public readonly GUIStyle connectedAnchorActive = (GUIStyle) "U2D.dragDotActive";
    }
  }
}
