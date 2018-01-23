// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationPropertyContextualMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationPropertyContextualMenu
  {
    public static AnimationPropertyContextualMenu Instance = new AnimationPropertyContextualMenu();
    private static GUIContent addKeyContent = EditorGUIUtility.TextContent("Add Key");
    private static GUIContent updateKeyContent = EditorGUIUtility.TextContent("Update Key");
    private static GUIContent removeKeyContent = EditorGUIUtility.TextContent("Remove Key");
    private static GUIContent removeCurveContent = EditorGUIUtility.TextContent("Remove All Keys");
    private static GUIContent goToPreviousKeyContent = EditorGUIUtility.TextContent("Go to Previous Key");
    private static GUIContent goToNextKeyContent = EditorGUIUtility.TextContent("Go to Next Key");
    private static GUIContent addCandidatesContent = EditorGUIUtility.TextContent("Key All Modified");
    private static GUIContent addAnimatedContent = EditorGUIUtility.TextContent("Key All Animated");
    private IAnimationContextualResponder m_Responder;

    public AnimationPropertyContextualMenu()
    {
      EditorApplication.contextualPropertyMenu += new EditorApplication.SerializedPropertyCallbackFunction(this.OnPropertyContextMenu);
      MaterialEditor.contextualPropertyMenu += new MaterialEditor.MaterialPropertyCallbackFunction(this.OnPropertyContextMenu);
    }

    public void SetResponder(IAnimationContextualResponder responder)
    {
      this.m_Responder = responder;
    }

    public bool IsResponder(IAnimationContextualResponder responder)
    {
      return responder == this.m_Responder;
    }

    private void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
    {
      if (this.m_Responder == null)
        return;
      PropertyModification[] propertyModifications = AnimationWindowUtility.SerializedPropertyToPropertyModifications(property);
      if (!this.m_Responder.IsAnimatable(propertyModifications))
        return;
      if (this.m_Responder.IsEditable(property.serializedObject.targetObject))
        this.OnPropertyContextMenu(menu, propertyModifications);
      else
        this.OnDisabledPropertyContextMenu(menu);
    }

    private void OnPropertyContextMenu(GenericMenu menu, MaterialProperty property, Renderer[] renderers)
    {
      if (this.m_Responder == null || property.targets == null || (property.targets.Length == 0 || renderers == null) || renderers.Length == 0)
        return;
      List<PropertyModification> propertyModificationList = new List<PropertyModification>();
      foreach (Renderer renderer in renderers)
        propertyModificationList.AddRange((IEnumerable<PropertyModification>) MaterialAnimationUtility.MaterialPropertyToPropertyModifications(property, renderer));
      if (this.m_Responder.IsEditable((Object) renderers[0]))
        this.OnPropertyContextMenu(menu, propertyModificationList.ToArray());
      else
        this.OnDisabledPropertyContextMenu(menu);
    }

    private void OnPropertyContextMenu(GenericMenu menu, PropertyModification[] modifications)
    {
      bool flag1 = this.m_Responder.KeyExists(modifications);
      bool flag2 = this.m_Responder.CandidateExists(modifications);
      bool flag3 = flag1 || this.m_Responder.CurveExists(modifications);
      bool flag4 = this.m_Responder.HasAnyCandidates();
      bool flag5 = this.m_Responder.HasAnyCurves();
      menu.AddItem(!flag1 || !flag2 ? AnimationPropertyContextualMenu.addKeyContent : AnimationPropertyContextualMenu.updateKeyContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.AddKey(modifications)));
      if (flag1)
        menu.AddItem(AnimationPropertyContextualMenu.removeKeyContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.RemoveKey(modifications)));
      else
        menu.AddDisabledItem(AnimationPropertyContextualMenu.removeKeyContent);
      if (flag3)
        menu.AddItem(AnimationPropertyContextualMenu.removeCurveContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.RemoveCurve(modifications)));
      else
        menu.AddDisabledItem(AnimationPropertyContextualMenu.removeCurveContent);
      menu.AddSeparator(string.Empty);
      if (flag4)
        menu.AddItem(AnimationPropertyContextualMenu.addCandidatesContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.AddCandidateKeys()));
      else
        menu.AddDisabledItem(AnimationPropertyContextualMenu.addCandidatesContent);
      if (flag5)
        menu.AddItem(AnimationPropertyContextualMenu.addAnimatedContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.AddAnimatedKeys()));
      else
        menu.AddDisabledItem(AnimationPropertyContextualMenu.addAnimatedContent);
      menu.AddSeparator(string.Empty);
      if (flag3)
      {
        menu.AddItem(AnimationPropertyContextualMenu.goToPreviousKeyContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.GoToPreviousKeyframe(modifications)));
        menu.AddItem(AnimationPropertyContextualMenu.goToNextKeyContent, false, (GenericMenu.MenuFunction) (() => this.m_Responder.GoToNextKeyframe(modifications)));
      }
      else
      {
        menu.AddDisabledItem(AnimationPropertyContextualMenu.goToPreviousKeyContent);
        menu.AddDisabledItem(AnimationPropertyContextualMenu.goToNextKeyContent);
      }
    }

    private void OnDisabledPropertyContextMenu(GenericMenu menu)
    {
      menu.AddDisabledItem(AnimationPropertyContextualMenu.addKeyContent);
      menu.AddDisabledItem(AnimationPropertyContextualMenu.removeKeyContent);
      menu.AddDisabledItem(AnimationPropertyContextualMenu.removeCurveContent);
      menu.AddSeparator(string.Empty);
      menu.AddDisabledItem(AnimationPropertyContextualMenu.addCandidatesContent);
      menu.AddDisabledItem(AnimationPropertyContextualMenu.addAnimatedContent);
      menu.AddSeparator(string.Empty);
      menu.AddDisabledItem(AnimationPropertyContextualMenu.goToPreviousKeyContent);
      menu.AddDisabledItem(AnimationPropertyContextualMenu.goToNextKeyContent);
    }
  }
}
