// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptionsWindowsMR
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal class VRCustomOptionsWindowsMR : VRCustomOptions
  {
    private static GUIContent[] s_DepthOptions = new GUIContent[2]{ new GUIContent("16-bit depth"), new GUIContent("24-bit depth") };
    private static GUIContent s_DepthFormatLabel = new GUIContent("Depth Format");
    private static GUIContent s_DepthBufferSharingLabel = new GUIContent("Enable Depth Buffer Sharing");
    private SerializedProperty m_DepthFormat;
    private SerializedProperty m_DepthBufferSharingEnabled;

    public override void Initialize(SerializedObject settings, string propertyName)
    {
      base.Initialize(settings, "hololens");
      this.m_DepthFormat = this.FindPropertyAssert("depthFormat");
      this.m_DepthBufferSharingEnabled = this.FindPropertyAssert("depthBufferSharingEnabled");
    }

    public override Rect Draw(Rect rect)
    {
      rect.y += EditorGUIUtility.standardVerticalSpacing;
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label = EditorGUI.BeginProperty(rect, VRCustomOptionsWindowsMR.s_DepthFormatLabel, this.m_DepthFormat);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.Popup(rect, label, this.m_DepthFormat.intValue, VRCustomOptionsWindowsMR.s_DepthOptions);
      if (EditorGUI.EndChangeCheck())
        this.m_DepthFormat.intValue = num;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.BeginProperty(rect, VRCustomOptionsWindowsMR.s_DepthBufferSharingLabel, this.m_DepthBufferSharingEnabled);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUI.Toggle(rect, VRCustomOptionsWindowsMR.s_DepthBufferSharingLabel, this.m_DepthBufferSharingEnabled.boolValue);
      if (EditorGUI.EndChangeCheck())
        this.m_DepthBufferSharingEnabled.boolValue = flag;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      return rect;
    }

    public override float GetHeight()
    {
      return EditorGUIUtility.singleLineHeight * 2f;
    }
  }
}
