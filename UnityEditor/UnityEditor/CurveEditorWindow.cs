// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveEditorWindow : EditorWindow
  {
    private GUIContent m_GUIContent = new GUIContent();
    private const int kPresetsHeight = 46;
    private static CurveEditorWindow s_SharedCurveEditor;
    private CurveEditor m_CurveEditor;
    private AnimationCurve m_Curve;
    private Color m_Color;
    private CurvePresetsContentsForPopupWindow m_CurvePresets;
    [SerializeField]
    private GUIView delegateView;
    private Action<AnimationCurve> m_OnCurveChanged;
    internal static CurveEditorWindow.Styles ms_Styles;

    public static CurveEditorWindow instance
    {
      get
      {
        if (!(bool) ((UnityEngine.Object) CurveEditorWindow.s_SharedCurveEditor))
          CurveEditorWindow.s_SharedCurveEditor = ScriptableObject.CreateInstance<CurveEditorWindow>();
        return CurveEditorWindow.s_SharedCurveEditor;
      }
    }

    public string currentPresetLibrary
    {
      get
      {
        this.InitCurvePresets();
        return this.m_CurvePresets.currentPresetLibrary;
      }
      set
      {
        this.InitCurvePresets();
        this.m_CurvePresets.currentPresetLibrary = value;
      }
    }

    public static AnimationCurve curve
    {
      get
      {
        return !CurveEditorWindow.visible ? (AnimationCurve) null : CurveEditorWindow.instance.m_Curve;
      }
      set
      {
        if (value == null)
        {
          CurveEditorWindow.instance.m_Curve = (AnimationCurve) null;
        }
        else
        {
          CurveEditorWindow.instance.m_Curve = value;
          CurveEditorWindow.instance.RefreshShownCurves();
        }
      }
    }

    public static Color color
    {
      get
      {
        return CurveEditorWindow.instance.m_Color;
      }
      set
      {
        CurveEditorWindow.instance.m_Color = value;
        CurveEditorWindow.instance.RefreshShownCurves();
      }
    }

    public static bool visible
    {
      get
      {
        return (UnityEngine.Object) CurveEditorWindow.s_SharedCurveEditor != (UnityEngine.Object) null;
      }
    }

    private void OnEnable()
    {
      CurveEditorWindow.s_SharedCurveEditor = this;
      this.Init((CurveEditorSettings) null);
    }

    private void Init(CurveEditorSettings settings)
    {
      this.m_CurveEditor = new CurveEditor(this.GetCurveEditorRect(), this.GetCurveWrapperArray(), true);
      this.m_CurveEditor.curvesUpdated = new CurveEditor.CallbackFunction(this.UpdateCurve);
      this.m_CurveEditor.scaleWithWindow = true;
      this.m_CurveEditor.margin = 40f;
      if (settings != null)
        this.m_CurveEditor.settings = settings;
      this.m_CurveEditor.settings.hTickLabelOffset = 10f;
      this.m_CurveEditor.settings.rectangleToolFlags = CurveEditorSettings.RectangleToolFlags.MiniRectangleTool;
      this.m_CurveEditor.settings.undoRedoSelection = true;
      this.m_CurveEditor.settings.showWrapperPopups = true;
      bool horizontally = true;
      bool vertically = true;
      if ((double) this.m_CurveEditor.settings.hRangeMin != double.NegativeInfinity && (double) this.m_CurveEditor.settings.hRangeMax != double.PositiveInfinity)
      {
        this.m_CurveEditor.SetShownHRangeInsideMargins(this.m_CurveEditor.settings.hRangeMin, this.m_CurveEditor.settings.hRangeMax);
        horizontally = false;
      }
      if ((double) this.m_CurveEditor.settings.vRangeMin != double.NegativeInfinity && (double) this.m_CurveEditor.settings.vRangeMax != double.PositiveInfinity)
      {
        this.m_CurveEditor.SetShownVRangeInsideMargins(this.m_CurveEditor.settings.vRangeMin, this.m_CurveEditor.settings.vRangeMax);
        vertically = false;
      }
      this.m_CurveEditor.FrameSelected(horizontally, vertically);
      this.titleContent = new GUIContent("Curve");
      this.minSize = new Vector2(240f, 286f);
      this.maxSize = new Vector2(10000f, 10000f);
    }

    private CurveLibraryType curveLibraryType
    {
      get
      {
        return this.m_CurveEditor.settings.hasUnboundedRanges ? CurveLibraryType.Unbounded : CurveLibraryType.NormalizedZeroToOne;
      }
    }

    private bool GetNormalizationRect(out Rect normalizationRect)
    {
      normalizationRect = new Rect();
      if (this.m_CurveEditor.settings.hasUnboundedRanges)
        return false;
      normalizationRect = new Rect(this.m_CurveEditor.settings.hRangeMin, this.m_CurveEditor.settings.vRangeMin, this.m_CurveEditor.settings.hRangeMax - this.m_CurveEditor.settings.hRangeMin, this.m_CurveEditor.settings.vRangeMax - this.m_CurveEditor.settings.vRangeMin);
      return true;
    }

    private static Keyframe[] CopyAndScaleCurveKeys(Keyframe[] orgKeys, Rect rect, CurveEditorWindow.NormalizationMode normalization)
    {
      Keyframe[] keyframeArray = new Keyframe[orgKeys.Length];
      orgKeys.CopyTo((Array) keyframeArray, 0);
      if (normalization == CurveEditorWindow.NormalizationMode.None)
        return keyframeArray;
      if ((double) rect.width == 0.0 || (double) rect.height == 0.0 || (float.IsInfinity(rect.width) || float.IsInfinity(rect.height)))
      {
        Debug.LogError((object) ("CopyAndScaleCurve: Invalid scale: " + (object) rect));
        return keyframeArray;
      }
      float num = rect.height / rect.width;
      switch (normalization)
      {
        case CurveEditorWindow.NormalizationMode.Normalize:
          for (int index = 0; index < keyframeArray.Length; ++index)
          {
            keyframeArray[index].time = (orgKeys[index].time - rect.xMin) / rect.width;
            keyframeArray[index].value = (orgKeys[index].value - rect.yMin) / rect.height;
            if (!float.IsInfinity(orgKeys[index].inTangent))
              keyframeArray[index].inTangent = orgKeys[index].inTangent / num;
            if (!float.IsInfinity(orgKeys[index].outTangent))
              keyframeArray[index].outTangent = orgKeys[index].outTangent / num;
          }
          break;
        case CurveEditorWindow.NormalizationMode.Denormalize:
          for (int index = 0; index < keyframeArray.Length; ++index)
          {
            keyframeArray[index].time = orgKeys[index].time * rect.width + rect.xMin;
            keyframeArray[index].value = orgKeys[index].value * rect.height + rect.yMin;
            if (!float.IsInfinity(orgKeys[index].inTangent))
              keyframeArray[index].inTangent = orgKeys[index].inTangent * num;
            if (!float.IsInfinity(orgKeys[index].outTangent))
              keyframeArray[index].outTangent = orgKeys[index].outTangent * num;
          }
          break;
      }
      return keyframeArray;
    }

    private void InitCurvePresets()
    {
      if (this.m_CurvePresets != null)
        return;
      this.m_CurvePresets = new CurvePresetsContentsForPopupWindow((AnimationCurve) null, this.curveLibraryType, (Action<AnimationCurve>) (presetCurve =>
      {
        this.ValidateCurveLibraryTypeAndScale();
        this.m_Curve.keys = this.GetDenormalizedKeys(presetCurve.keys);
        this.m_Curve.postWrapMode = presetCurve.postWrapMode;
        this.m_Curve.preWrapMode = presetCurve.preWrapMode;
        this.m_CurveEditor.SelectNone();
        this.RefreshShownCurves();
        this.SendEvent("CurveChanged", true);
      }));
      this.m_CurvePresets.InitIfNeeded();
    }

    private void OnDestroy()
    {
      if (this.m_CurvePresets == null)
        return;
      this.m_CurvePresets.GetPresetLibraryEditor().UnloadUsedLibraries();
    }

    private void OnDisable()
    {
      this.m_CurveEditor.OnDisable();
      if ((UnityEngine.Object) CurveEditorWindow.s_SharedCurveEditor == (UnityEngine.Object) this)
        CurveEditorWindow.s_SharedCurveEditor = (CurveEditorWindow) null;
      else if (!this.Equals((object) CurveEditorWindow.s_SharedCurveEditor))
        throw new ApplicationException("s_SharedCurveEditor does not equal this");
    }

    private void RefreshShownCurves()
    {
      this.m_CurveEditor.animationCurves = this.GetCurveWrapperArray();
    }

    public void Show(GUIView viewToUpdate, CurveEditorSettings settings)
    {
      this.delegateView = viewToUpdate;
      this.m_OnCurveChanged = (Action<AnimationCurve>) null;
      this.Init(settings);
      this.ShowAuxWindow();
    }

    public void Show(Action<AnimationCurve> onCurveChanged, CurveEditorSettings settings)
    {
      this.m_OnCurveChanged = onCurveChanged;
      this.delegateView = (GUIView) null;
      this.Init(settings);
      this.ShowAuxWindow();
    }

    private CurveWrapper[] GetCurveWrapperArray()
    {
      if (this.m_Curve == null)
        return new CurveWrapper[0];
      CurveWrapper curveWrapper = new CurveWrapper();
      curveWrapper.id = "Curve".GetHashCode();
      curveWrapper.groupId = -1;
      curveWrapper.color = this.m_Color;
      curveWrapper.hidden = false;
      curveWrapper.readOnly = false;
      curveWrapper.renderer = (CurveRenderer) new NormalCurveRenderer(this.m_Curve);
      curveWrapper.renderer.SetWrap(this.m_Curve.preWrapMode, this.m_Curve.postWrapMode);
      return new CurveWrapper[1]{ curveWrapper };
    }

    private Rect GetCurveEditorRect()
    {
      return new Rect(0.0f, 0.0f, this.position.width, this.position.height - 46f);
    }

    internal static Keyframe[] GetLinearKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 0.0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetLinearMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 1f, -1f, -1f), new Keyframe(1f, 0.0f, -1f, -1f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 0.0f, 0.0f, 0.0f), new Keyframe(1f, 1f, 2f, 2f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 1f, -2f, -2f), new Keyframe(1f, 0.0f, 0.0f, 0.0f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseOutKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 0.0f, 2f, 2f), new Keyframe(1f, 1f, 0.0f, 0.0f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseOutMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 1f, 0.0f, 0.0f), new Keyframe(1f, 0.0f, -2f, -2f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInOutKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 0.0f, 0.0f, 0.0f), new Keyframe(1f, 1f, 0.0f, 0.0f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetEaseInOutMirrorKeys()
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, 1f, 0.0f, 0.0f), new Keyframe(1f, 0.0f, 0.0f, 0.0f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static Keyframe[] GetConstantKeys(float value)
    {
      Keyframe[] keys = new Keyframe[2]{ new Keyframe(0.0f, value, 0.0f, 0.0f), new Keyframe(1f, value, 0.0f, 0.0f) };
      CurveEditorWindow.SetSmoothEditable(ref keys);
      return keys;
    }

    internal static void SetSmoothEditable(ref Keyframe[] keys)
    {
      for (int index = 0; index < keys.Length; ++index)
      {
        AnimationUtility.SetKeyBroken(ref keys[index], false);
        AnimationUtility.SetKeyLeftTangentMode(ref keys[index], AnimationUtility.TangentMode.Free);
        AnimationUtility.SetKeyRightTangentMode(ref keys[index], AnimationUtility.TangentMode.Free);
      }
    }

    private Keyframe[] NormalizeKeys(Keyframe[] sourceKeys, CurveEditorWindow.NormalizationMode normalization)
    {
      Rect normalizationRect;
      if (!this.GetNormalizationRect(out normalizationRect))
        normalization = CurveEditorWindow.NormalizationMode.None;
      return CurveEditorWindow.CopyAndScaleCurveKeys(sourceKeys, normalizationRect, normalization);
    }

    private Keyframe[] GetDenormalizedKeys(Keyframe[] sourceKeys)
    {
      return this.NormalizeKeys(sourceKeys, CurveEditorWindow.NormalizationMode.Denormalize);
    }

    private Keyframe[] GetNormalizedKeys(Keyframe[] sourceKeys)
    {
      return this.NormalizeKeys(sourceKeys, CurveEditorWindow.NormalizationMode.Normalize);
    }

    private void OnGUI()
    {
      bool flag = Event.current.type == EventType.MouseUp;
      if ((UnityEngine.Object) this.delegateView == (UnityEngine.Object) null && this.m_OnCurveChanged == null)
        this.m_Curve = (AnimationCurve) null;
      if (CurveEditorWindow.ms_Styles == null)
        CurveEditorWindow.ms_Styles = new CurveEditorWindow.Styles();
      this.m_CurveEditor.rect = this.GetCurveEditorRect();
      this.m_CurveEditor.hRangeLocked = Event.current.shift;
      this.m_CurveEditor.vRangeLocked = EditorGUI.actionKey;
      GUI.changed = false;
      GUI.Label(this.m_CurveEditor.drawRect, GUIContent.none, CurveEditorWindow.ms_Styles.curveEditorBackground);
      this.m_CurveEditor.OnGUI();
      GUI.Box(new Rect(0.0f, this.position.height - 46f, this.position.width, 46f), "", CurveEditorWindow.ms_Styles.curveSwatchArea);
      this.m_Color.a *= 0.6f;
      float y = (float) ((double) this.position.height - 46.0 + 10.5);
      this.InitCurvePresets();
      CurvePresetLibrary currentLib = this.m_CurvePresets.GetPresetLibraryEditor().GetCurrentLib();
      if ((UnityEngine.Object) currentLib != (UnityEngine.Object) null)
      {
        for (int index = 0; index < currentLib.Count(); ++index)
        {
          Rect rect = new Rect((float) (45.0 + 45.0 * (double) index), y, 40f, 25f);
          this.m_GUIContent.tooltip = currentLib.GetName(index);
          if (GUI.Button(rect, this.m_GUIContent, CurveEditorWindow.ms_Styles.curveSwatch))
          {
            AnimationCurve preset = currentLib.GetPreset(index) as AnimationCurve;
            this.m_Curve.keys = this.GetDenormalizedKeys(preset.keys);
            this.m_Curve.postWrapMode = preset.postWrapMode;
            this.m_Curve.preWrapMode = preset.preWrapMode;
            this.m_CurveEditor.SelectNone();
            this.SendEvent("CurveChanged", true);
          }
          if (Event.current.type == EventType.Repaint)
            currentLib.Draw(rect, index);
          if ((double) rect.xMax > (double) this.position.width - 90.0)
            break;
        }
      }
      this.PresetDropDown(new Rect(25f, y + 5f, 20f, 20f));
      if (Event.current.type == EventType.Used && flag)
      {
        this.DoUpdateCurve(false);
        this.SendEvent("CurveChangeCompleted", true);
      }
      else
      {
        if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
          return;
        this.DoUpdateCurve(true);
      }
    }

    private void PresetDropDown(Rect rect)
    {
      if (!EditorGUI.DropdownButton(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Passive, EditorStyles.inspectorTitlebarText) || this.m_Curve == null)
        return;
      if (this.m_CurvePresets == null)
      {
        Debug.LogError((object) "Curve presets error");
      }
      else
      {
        this.ValidateCurveLibraryTypeAndScale();
        this.m_CurvePresets.curveToSaveAsPreset = new AnimationCurve(this.GetNormalizedKeys(this.m_Curve.keys))
        {
          postWrapMode = this.m_Curve.postWrapMode,
          preWrapMode = this.m_Curve.preWrapMode
        };
        PopupWindow.Show(rect, (PopupWindowContent) this.m_CurvePresets);
      }
    }

    private void ValidateCurveLibraryTypeAndScale()
    {
      Rect normalizationRect;
      if (this.GetNormalizationRect(out normalizationRect))
      {
        if (this.curveLibraryType == CurveLibraryType.NormalizedZeroToOne)
          return;
        Debug.LogError((object) ("When having a normalize rect we should be using curve library type: NormalizedZeroToOne (normalizationRect: " + (object) normalizationRect + ")"));
      }
      else if (this.curveLibraryType != CurveLibraryType.Unbounded)
        Debug.LogError((object) "When NOT having a normalize rect we should be using library type: Unbounded");
    }

    public void UpdateCurve()
    {
      this.DoUpdateCurve(false);
    }

    private void DoUpdateCurve(bool exitGUI)
    {
      if (this.m_CurveEditor.animationCurves.Length <= 0 || this.m_CurveEditor.animationCurves[0] == null || !this.m_CurveEditor.animationCurves[0].changed)
        return;
      this.m_CurveEditor.animationCurves[0].changed = false;
      this.RefreshShownCurves();
      this.SendEvent("CurveChanged", exitGUI);
    }

    private void SendEvent(string eventName, bool exitGUI)
    {
      if ((bool) ((UnityEngine.Object) this.delegateView))
      {
        Event e = EditorGUIUtility.CommandEvent(eventName);
        this.Repaint();
        this.delegateView.SendEvent(e);
        if (exitGUI)
          GUIUtility.ExitGUI();
      }
      if (this.m_OnCurveChanged != null)
        this.m_OnCurveChanged(CurveEditorWindow.curve);
      GUI.changed = true;
    }

    private enum NormalizationMode
    {
      None,
      Normalize,
      Denormalize,
    }

    internal class Styles
    {
      public GUIStyle curveEditorBackground = (GUIStyle) "PopupCurveEditorBackground";
      public GUIStyle miniToolbarPopup = (GUIStyle) "MiniToolbarPopup";
      public GUIStyle miniToolbarButton = (GUIStyle) "MiniToolbarButtonLeft";
      public GUIStyle curveSwatch = (GUIStyle) "PopupCurveEditorSwatch";
      public GUIStyle curveSwatchArea = (GUIStyle) "PopupCurveSwatchBackground";
      public GUIStyle curveWrapPopup = (GUIStyle) "PopupCurveDropdown";
    }
  }
}
