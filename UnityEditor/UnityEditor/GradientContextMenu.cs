// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientContextMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GradientContextMenu
  {
    private readonly SerializedProperty m_Prop1;

    private GradientContextMenu(SerializedProperty prop1)
    {
      this.m_Prop1 = prop1;
    }

    internal static void Show(SerializedProperty prop)
    {
      GUIContent content1 = new GUIContent("Copy");
      GUIContent content2 = new GUIContent("Paste");
      GenericMenu genericMenu = new GenericMenu();
      GradientContextMenu gradientContextMenu = new GradientContextMenu(prop);
      genericMenu.AddItem(content1, false, new GenericMenu.MenuFunction(gradientContextMenu.Copy));
      if (ParticleSystemClipboard.HasSingleGradient())
        genericMenu.AddItem(content2, false, new GenericMenu.MenuFunction(gradientContextMenu.Paste));
      else
        genericMenu.AddDisabledItem(content2);
      genericMenu.ShowAsContext();
    }

    private void Copy()
    {
      ParticleSystemClipboard.CopyGradient(this.m_Prop1 == null ? (Gradient) null : this.m_Prop1.gradientValue, (Gradient) null);
    }

    private void Paste()
    {
      ParticleSystemClipboard.PasteGradient(this.m_Prop1, (SerializedProperty) null);
      if (this.m_Prop1 != null)
        this.m_Prop1.serializedObject.ApplyModifiedProperties();
      GradientPreviewCache.ClearCache();
    }
  }
}
