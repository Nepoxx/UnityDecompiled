// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptionsCardboard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal class VRCustomOptionsCardboard : VRCustomOptionsGoogleVR
  {
    private static GUIContent s_EnableTransitionViewLabel = new GUIContent("Enable Transition View");
    private SerializedProperty m_EnableTransitionView;

    public override void Initialize(SerializedObject settings)
    {
      this.Initialize(settings, "cardboard");
    }

    public override void Initialize(SerializedObject settings, string propertyName)
    {
      base.Initialize(settings, propertyName);
      this.m_EnableTransitionView = this.FindPropertyAssert("enableTransitionView");
    }

    public override Rect Draw(Rect rect)
    {
      rect = base.Draw(rect);
      rect.height = EditorGUIUtility.singleLineHeight;
      GUIContent label = EditorGUI.BeginProperty(rect, VRCustomOptionsCardboard.s_EnableTransitionViewLabel, this.m_EnableTransitionView);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUI.Toggle(rect, label, this.m_EnableTransitionView.boolValue);
      if (EditorGUI.EndChangeCheck())
        this.m_EnableTransitionView.boolValue = flag;
      EditorGUI.EndProperty();
      rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      return rect;
    }

    public override float GetHeight()
    {
      return base.GetHeight() + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
  }
}
