// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptionsOculus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal class VRCustomOptionsOculus : VRCustomOptions
  {
    private static GUIContent s_SharedDepthBufferLabel = EditorGUIUtility.TextContent("Shared Depth Buffer|Enable depth buffer submission to allow for overlay depth occlusion, etc.");
    private static GUIContent s_DashSupportLabel = EditorGUIUtility.TextContent("Dash Support|If enabled, pressing the home button brings up Dash, otherwise it brings up the older universal menu.");
    private SerializedProperty m_SharedDepthBuffer;
    private SerializedProperty m_DashSupport;

    public override void Initialize(SerializedObject settings)
    {
      this.Initialize(settings, "oculus");
    }

    public override void Initialize(SerializedObject settings, string propertyName)
    {
      base.Initialize(settings, propertyName);
      this.m_SharedDepthBuffer = this.FindPropertyAssert("sharedDepthBuffer");
      this.m_DashSupport = this.FindPropertyAssert("dashSupport");
    }

    public override Rect Draw(Rect rect)
    {
      rect.y += EditorGUIUtility.standardVerticalSpacing;
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label1 = EditorGUI.BeginProperty(rect, VRCustomOptionsOculus.s_SharedDepthBufferLabel, this.m_SharedDepthBuffer);
      EditorGUI.BeginChangeCheck();
      bool flag1 = EditorGUI.Toggle(rect, label1, this.m_SharedDepthBuffer.boolValue);
      if (EditorGUI.EndChangeCheck())
        this.m_SharedDepthBuffer.boolValue = flag1;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label2 = EditorGUI.BeginProperty(rect, VRCustomOptionsOculus.s_DashSupportLabel, this.m_DashSupport);
      EditorGUI.BeginChangeCheck();
      bool flag2 = EditorGUI.Toggle(rect, label2, this.m_DashSupport.boolValue);
      if (EditorGUI.EndChangeCheck())
        this.m_DashSupport.boolValue = flag2;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      return rect;
    }

    public override float GetHeight()
    {
      return (float) ((double) EditorGUIUtility.singleLineHeight * 2.0 + (double) EditorGUIUtility.standardVerticalSpacing * 3.0);
    }
  }
}
