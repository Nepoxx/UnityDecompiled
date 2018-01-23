// Decompiled with JetBrains decompiler
// Type: UnityEditor.ButtonWithAnimatedIconRotation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class ButtonWithAnimatedIconRotation
  {
    private readonly CallbackController m_CallbackController;
    private readonly Func<float> m_AngleCallback;
    private readonly bool m_MouseDownButton;

    public ButtonWithAnimatedIconRotation(Func<float> angleCallback, Action repaintCallback, float repaintsPerSecond, bool mouseDownButton)
    {
      this.m_CallbackController = new CallbackController(repaintCallback, repaintsPerSecond);
      this.m_AngleCallback = angleCallback;
      this.m_MouseDownButton = mouseDownButton;
    }

    public bool OnGUI(Rect rect, GUIContent guiContent, bool animate, GUIStyle style)
    {
      if (animate && !this.m_CallbackController.active)
        this.m_CallbackController.Start();
      if (!animate && this.m_CallbackController.active)
        this.m_CallbackController.Stop();
      float iconAngle = !animate ? 0.0f : this.m_AngleCallback();
      return EditorGUI.ButtonWithRotatedIcon(rect, guiContent, iconAngle, this.m_MouseDownButton, style);
    }

    public void Clear()
    {
      this.m_CallbackController.Stop();
    }
  }
}
