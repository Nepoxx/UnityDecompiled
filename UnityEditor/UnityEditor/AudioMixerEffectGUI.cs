// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerEffectGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal static class AudioMixerEffectGUI
  {
    private const string kAudioSliderFloatFormat = "F2";
    private const string kExposedParameterUnicodeChar = " ➔";

    public static void EffectHeader(string text)
    {
      GUILayout.Label(text, AudioMixerEffectGUI.styles.headerStyle, new GUILayoutOption[0]);
    }

    public static bool Slider(GUIContent label, ref float value, float displayScale, float displayExponent, string unit, float leftValue, float rightValue, AudioMixerController controller, AudioParameterPath path, params GUILayoutOption[] options)
    {
      EditorGUI.BeginChangeCheck();
      float fieldWidth = EditorGUIUtility.fieldWidth;
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      bool flag = controller.ContainsExposedParameter(path.parameter);
      EditorGUIUtility.fieldWidth = 70f;
      EditorGUI.kFloatFieldFormatString = "F2";
      EditorGUI.s_UnitString = unit;
      GUIContent label1 = label;
      if (flag)
        label1 = GUIContent.Temp(label.text + " ➔", label.tooltip);
      float num1 = value * displayScale;
      float num2 = EditorGUILayout.PowerSlider(label1, num1, leftValue * displayScale, rightValue * displayScale, displayExponent, options);
      EditorGUI.s_UnitString = (string) null;
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
      EditorGUIUtility.fieldWidth = fieldWidth;
      if (Event.current.type == EventType.ContextClick && GUILayoutUtility.topLevel.GetLast().Contains(Event.current.mousePosition))
      {
        Event.current.Use();
        GenericMenu genericMenu1 = new GenericMenu();
        if (!flag)
        {
          GenericMenu genericMenu2 = genericMenu1;
          GUIContent content = new GUIContent("Expose '" + path.ResolveStringPath(false) + "' to script");
          int num3 = 0;
          // ISSUE: reference to a compiler-generated field
          if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ExposePopupCallback);
          }
          // ISSUE: reference to a compiler-generated field
          GenericMenu.MenuFunction2 fMgCache0 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache0;
          AudioMixerEffectGUI.ExposedParamContext exposedParamContext = new AudioMixerEffectGUI.ExposedParamContext(controller, path);
          genericMenu2.AddItem(content, num3 != 0, fMgCache0, (object) exposedParamContext);
        }
        else
        {
          GenericMenu genericMenu2 = genericMenu1;
          GUIContent content = new GUIContent("Unexpose");
          int num3 = 0;
          // ISSUE: reference to a compiler-generated field
          if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache1 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.UnexposePopupCallback);
          }
          // ISSUE: reference to a compiler-generated field
          GenericMenu.MenuFunction2 fMgCache1 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache1;
          AudioMixerEffectGUI.ExposedParamContext exposedParamContext = new AudioMixerEffectGUI.ExposedParamContext(controller, path);
          genericMenu2.AddItem(content, num3 != 0, fMgCache1, (object) exposedParamContext);
        }
        ParameterTransitionType type;
        controller.TargetSnapshot.GetTransitionTypeOverride(path.parameter, out type);
        genericMenu1.AddSeparator(string.Empty);
        GenericMenu genericMenu3 = genericMenu1;
        GUIContent content1 = new GUIContent("Linear Snapshot Transition");
        int num4 = type == ParameterTransitionType.Lerp ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache2 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache2 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache2;
        AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext1 = new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Lerp);
        genericMenu3.AddItem(content1, num4 != 0, fMgCache2, (object) transitionOverrideContext1);
        GenericMenu genericMenu4 = genericMenu1;
        GUIContent content2 = new GUIContent("Smoothstep Snapshot Transition");
        int num5 = type == ParameterTransitionType.Smoothstep ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache3 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache3 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache3;
        AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext2 = new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Smoothstep);
        genericMenu4.AddItem(content2, num5 != 0, fMgCache3, (object) transitionOverrideContext2);
        GenericMenu genericMenu5 = genericMenu1;
        GUIContent content3 = new GUIContent("Squared Snapshot Transition");
        int num6 = type == ParameterTransitionType.Squared ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache4 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache4 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache4;
        AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext3 = new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Squared);
        genericMenu5.AddItem(content3, num6 != 0, fMgCache4, (object) transitionOverrideContext3);
        GenericMenu genericMenu6 = genericMenu1;
        GUIContent content4 = new GUIContent("SquareRoot Snapshot Transition");
        int num7 = type == ParameterTransitionType.SquareRoot ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache5 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache5 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache5;
        AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext4 = new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.SquareRoot);
        genericMenu6.AddItem(content4, num7 != 0, fMgCache5, (object) transitionOverrideContext4);
        GenericMenu genericMenu7 = genericMenu1;
        GUIContent content5 = new GUIContent("BrickwallStart Snapshot Transition");
        int num8 = type == ParameterTransitionType.BrickwallStart ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache6 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache6 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache6;
        AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext5 = new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallStart);
        genericMenu7.AddItem(content5, num8 != 0, fMgCache6, (object) transitionOverrideContext5);
        GenericMenu genericMenu8 = genericMenu1;
        GUIContent content6 = new GUIContent("BrickwallEnd Snapshot Transition");
        int num9 = type == ParameterTransitionType.BrickwallEnd ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache7 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache7 = AudioMixerEffectGUI.\u003C\u003Ef__mg\u0024cache7;
        AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext6 = new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallEnd);
        genericMenu8.AddItem(content6, num9 != 0, fMgCache7, (object) transitionOverrideContext6);
        genericMenu1.AddSeparator(string.Empty);
        genericMenu1.ShowAsContext();
      }
      if (!EditorGUI.EndChangeCheck())
        return false;
      value = num2 / displayScale;
      return true;
    }

    public static void ExposePopupCallback(object obj)
    {
      AudioMixerEffectGUI.ExposedParamContext exposedParamContext = (AudioMixerEffectGUI.ExposedParamContext) obj;
      Undo.RecordObject((Object) exposedParamContext.controller, "Expose Mixer Parameter");
      exposedParamContext.controller.AddExposedParameter(exposedParamContext.path);
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public static void UnexposePopupCallback(object obj)
    {
      AudioMixerEffectGUI.ExposedParamContext exposedParamContext = (AudioMixerEffectGUI.ExposedParamContext) obj;
      Undo.RecordObject((Object) exposedParamContext.controller, "Unexpose Mixer Parameter");
      exposedParamContext.controller.RemoveExposedParameter(exposedParamContext.path.parameter);
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public static void ParameterTransitionOverrideCallback(object obj)
    {
      AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext = (AudioMixerEffectGUI.ParameterTransitionOverrideContext) obj;
      Undo.RecordObject((Object) transitionOverrideContext.controller, "Change Parameter Transition Type");
      if (transitionOverrideContext.type == ParameterTransitionType.Lerp)
        transitionOverrideContext.controller.TargetSnapshot.ClearTransitionTypeOverride(transitionOverrideContext.parameter);
      else
        transitionOverrideContext.controller.TargetSnapshot.SetTransitionTypeOverride(transitionOverrideContext.parameter, transitionOverrideContext.type);
    }

    public static bool PopupButton(GUIContent label, GUIContent buttonContent, GUIStyle style, out Rect buttonRect, params GUILayoutOption[] options)
    {
      if (label != null)
      {
        Rect rect = EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options);
        int controlId = GUIUtility.GetControlID("EditorPopup".GetHashCode(), FocusType.Keyboard, rect);
        buttonRect = EditorGUI.PrefixLabel(rect, controlId, label);
      }
      else
      {
        Rect rect = GUILayoutUtility.GetRect(buttonContent, style, options);
        buttonRect = rect;
      }
      return EditorGUI.DropdownButton(buttonRect, buttonContent, FocusType.Passive, style);
    }

    private static AudioMixerDrawUtils.Styles styles
    {
      get
      {
        return AudioMixerDrawUtils.styles;
      }
    }

    private class ExposedParamContext
    {
      public AudioMixerController controller;
      public AudioParameterPath path;

      public ExposedParamContext(AudioMixerController controller, AudioParameterPath path)
      {
        this.controller = controller;
        this.path = path;
      }
    }

    private class ParameterTransitionOverrideContext
    {
      public AudioMixerController controller;
      public GUID parameter;
      public ParameterTransitionType type;

      public ParameterTransitionOverrideContext(AudioMixerController controller, GUID parameter, ParameterTransitionType type)
      {
        this.controller = controller;
        this.parameter = parameter;
        this.type = type;
      }
    }

    private class ParameterTransitionOverrideRemoveContext
    {
      public AudioMixerController controller;
      public GUID parameter;

      public ParameterTransitionOverrideRemoveContext(AudioMixerController controller, GUID parameter)
      {
        this.controller = controller;
        this.parameter = parameter;
      }
    }
  }
}
