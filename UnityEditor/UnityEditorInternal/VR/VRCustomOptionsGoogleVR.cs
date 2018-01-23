// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptionsGoogleVR
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal class VRCustomOptionsGoogleVR : VRCustomOptions
  {
    private static GUIContent[] s_DepthOptions = new GUIContent[3]{ new GUIContent("16-bit depth"), new GUIContent("24-bit depth"), new GUIContent("24-bit depth | 8-bit stencil") };
    private static GUIContent s_DepthFormatLabel = new GUIContent("Depth Format");
    private SerializedProperty m_DepthFormat;

    public override void Initialize(SerializedObject settings, string propertyName)
    {
      base.Initialize(settings, propertyName);
      this.m_DepthFormat = this.FindPropertyAssert("depthFormat");
    }

    public override Rect Draw(Rect rect)
    {
      rect.y += EditorGUIUtility.standardVerticalSpacing;
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label = EditorGUI.BeginProperty(rect, VRCustomOptionsGoogleVR.s_DepthFormatLabel, this.m_DepthFormat);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.Popup(rect, label, this.m_DepthFormat.intValue, VRCustomOptionsGoogleVR.s_DepthOptions);
      if (EditorGUI.EndChangeCheck())
        this.m_DepthFormat.intValue = num;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      return rect;
    }

    public override float GetHeight()
    {
      return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
    }
  }
}
