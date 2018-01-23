// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationCurveContextMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AnimationCurveContextMenu
  {
    private readonly SerializedProperty m_Prop1;
    private readonly SerializedProperty m_Prop2;
    private readonly SerializedProperty m_Scalar;
    private readonly ParticleSystemCurveEditor m_ParticleSystemCurveEditor;
    private readonly Rect m_CurveRanges;

    private AnimationCurveContextMenu(SerializedProperty prop1, SerializedProperty prop2, SerializedProperty scalar, Rect curveRanges, ParticleSystemCurveEditor owner)
    {
      this.m_Prop1 = prop1;
      this.m_Prop2 = prop2;
      this.m_Scalar = scalar;
      this.m_ParticleSystemCurveEditor = owner;
      this.m_CurveRanges = curveRanges;
    }

    internal static void Show(Rect position, SerializedProperty property, SerializedProperty property2, SerializedProperty scalar, Rect curveRanges, ParticleSystemCurveEditor curveEditor)
    {
      GUIContent content1 = new GUIContent("Copy");
      GUIContent content2 = new GUIContent("Paste");
      GenericMenu genericMenu = new GenericMenu();
      bool flag1 = property != null && property2 != null;
      bool flag2 = flag1 && ParticleSystemClipboard.HasDoubleAnimationCurve() || !flag1 && ParticleSystemClipboard.HasSingleAnimationCurve();
      AnimationCurveContextMenu curveContextMenu = new AnimationCurveContextMenu(property, property2, scalar, curveRanges, curveEditor);
      genericMenu.AddItem(content1, false, new GenericMenu.MenuFunction(curveContextMenu.Copy));
      if (flag2)
        genericMenu.AddItem(content2, false, new GenericMenu.MenuFunction(curveContextMenu.Paste));
      else
        genericMenu.AddDisabledItem(content2);
      genericMenu.DropDown(position);
    }

    private void Copy()
    {
      ParticleSystemClipboard.CopyAnimationCurves(this.m_Prop1 == null ? (AnimationCurve) null : this.m_Prop1.animationCurveValue, this.m_Prop2 == null ? (AnimationCurve) null : this.m_Prop2.animationCurveValue, this.m_Scalar == null ? 1f : this.m_Scalar.floatValue);
    }

    private void Paste()
    {
      ParticleSystemClipboard.PasteAnimationCurves(this.m_Prop1, this.m_Prop2, this.m_Scalar, this.m_CurveRanges, this.m_ParticleSystemCurveEditor);
      if (this.m_Prop1 != null)
        this.m_Prop1.serializedObject.ApplyModifiedProperties();
      if (this.m_Prop2 != null)
        this.m_Prop2.serializedObject.ApplyModifiedProperties();
      if (this.m_Scalar == null)
        return;
      this.m_Scalar.serializedObject.ApplyModifiedProperties();
    }
  }
}
