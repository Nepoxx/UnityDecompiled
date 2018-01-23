// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class ModuleUI : SerializedModule
  {
    protected static readonly bool kUseSignedRange = true;
    protected static readonly Rect kUnsignedRange = new Rect(0.0f, 0.0f, 1f, 1f);
    protected static readonly Rect kSignedRange = new Rect(0.0f, -1f, 1f, 2f);
    public static float k_CompactFixedModuleWidth = 295f;
    public static float k_SpaceBetweenModules = 5f;
    public static readonly GUIStyle s_ControlRectStyle = new GUIStyle() { margin = new RectOffset(0, 0, 2, 2) };
    protected string m_ToolTip = "";
    public List<SerializedProperty> m_ModuleCurves = new List<SerializedProperty>();
    private List<SerializedProperty> m_CurvesRemovedWhenFolded = new List<SerializedProperty>();
    public ParticleSystemUI m_ParticleSystemUI;
    private string m_DisplayName;
    private SerializedProperty m_Enabled;
    private ModuleUI.VisibilityState m_VisibilityState;
    protected const int kSingleLineHeight = 13;
    protected const float k_minMaxToggleWidth = 13f;
    protected const float k_toggleWidth = 9f;
    protected const float kDragSpace = 20f;
    protected const int kPlusAddRemoveButtonWidth = 12;
    protected const int kPlusAddRemoveButtonSpacing = 5;
    protected const int kSpacingSubLabel = 4;
    protected const int kSubLabelWidth = 10;
    protected const string kFormatString = "g7";
    protected const float kReorderableListElementHeight = 16f;

    public ModuleUI(ParticleSystemUI owner, SerializedObject o, string name, string displayName)
      : base(o, name)
    {
      this.Setup(owner, o, displayName, ModuleUI.VisibilityState.NotVisible);
    }

    public ModuleUI(ParticleSystemUI owner, SerializedObject o, string name, string displayName, ModuleUI.VisibilityState initialVisibilityState)
      : base(o, name)
    {
      this.Setup(owner, o, displayName, initialVisibilityState);
    }

    public bool visibleUI
    {
      get
      {
        return this.m_VisibilityState != ModuleUI.VisibilityState.NotVisible;
      }
      set
      {
        this.SetVisibilityState(!value ? ModuleUI.VisibilityState.NotVisible : ModuleUI.VisibilityState.VisibleAndFolded);
      }
    }

    public bool foldout
    {
      get
      {
        return this.m_VisibilityState == ModuleUI.VisibilityState.VisibleAndFoldedOut;
      }
      set
      {
        this.SetVisibilityState(!value ? ModuleUI.VisibilityState.VisibleAndFolded : ModuleUI.VisibilityState.VisibleAndFoldedOut);
      }
    }

    public bool enabled
    {
      get
      {
        return this.m_Enabled.boolValue;
      }
      set
      {
        if (this.m_Enabled.boolValue == value)
          return;
        this.m_Enabled.boolValue = value;
        if (value)
          this.OnModuleEnable();
        else
          this.OnModuleDisable();
      }
    }

    public bool enabledHasMultipleDifferentValues
    {
      get
      {
        return this.m_Enabled.hasMultipleDifferentValues;
      }
    }

    public string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
    }

    public string toolTip
    {
      get
      {
        return this.m_ToolTip;
      }
    }

    public bool isWindowView
    {
      get
      {
        return this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner is ParticleSystemWindow;
      }
    }

    private void Setup(ParticleSystemUI owner, SerializedObject o, string displayName, ModuleUI.VisibilityState defaultVisibilityState)
    {
      this.m_ParticleSystemUI = owner;
      this.m_DisplayName = displayName;
      this.m_Enabled = !(this is RendererModuleUI) ? this.GetProperty("enabled") : this.GetProperty0("m_Enabled");
      this.m_VisibilityState = ModuleUI.VisibilityState.NotVisible;
      foreach (UnityEngine.Object targetObject in o.targetObjects)
        this.m_VisibilityState = (ModuleUI.VisibilityState) Mathf.Max(SessionState.GetInt(this.GetUniqueModuleName(targetObject), (int) defaultVisibilityState), (int) this.m_VisibilityState);
      this.CheckVisibilityState();
      if (!this.foldout)
        return;
      this.Init();
    }

    protected abstract void Init();

    public virtual void Validate()
    {
    }

    public virtual float GetXAxisScalar()
    {
      return 1f;
    }

    public abstract void OnInspectorGUI(InitialModuleUI initial);

    public virtual void OnSceneViewGUI()
    {
    }

    public virtual void UpdateCullingSupportedString(ref string text)
    {
    }

    protected virtual void OnModuleEnable()
    {
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.Init();
    }

    public virtual void UndoRedoPerformed()
    {
      if (this.enabled)
        return;
      this.OnModuleDisable();
    }

    protected virtual void OnModuleDisable()
    {
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      ParticleSystemCurveEditor systemCurveEditor = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
      foreach (SerializedProperty moduleCurve in this.m_ModuleCurves)
      {
        if (systemCurveEditor.IsAdded(moduleCurve))
          systemCurveEditor.RemoveCurve(moduleCurve);
      }
    }

    internal void CheckVisibilityState()
    {
      if (!(this is RendererModuleUI) && !this.m_Enabled.boolValue && !ParticleEffectUI.GetAllModulesVisible())
        this.SetVisibilityState(ModuleUI.VisibilityState.NotVisible);
      if (!this.m_Enabled.boolValue || this.visibleUI)
        return;
      this.SetVisibilityState(ModuleUI.VisibilityState.VisibleAndFolded);
    }

    protected virtual void SetVisibilityState(ModuleUI.VisibilityState newState)
    {
      if (newState == this.m_VisibilityState)
        return;
      switch (newState)
      {
        case ModuleUI.VisibilityState.VisibleAndFolded:
          ParticleSystemCurveEditor systemCurveEditor1 = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
          foreach (SerializedProperty moduleCurve in this.m_ModuleCurves)
          {
            if (systemCurveEditor1.IsAdded(moduleCurve))
            {
              this.m_CurvesRemovedWhenFolded.Add(moduleCurve);
              systemCurveEditor1.SetVisible(moduleCurve, false);
            }
          }
          systemCurveEditor1.Refresh();
          break;
        case ModuleUI.VisibilityState.VisibleAndFoldedOut:
          ParticleSystemCurveEditor systemCurveEditor2 = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
          foreach (SerializedProperty curveProp in this.m_CurvesRemovedWhenFolded)
            systemCurveEditor2.SetVisible(curveProp, true);
          this.m_CurvesRemovedWhenFolded.Clear();
          systemCurveEditor2.Refresh();
          break;
      }
      this.m_VisibilityState = newState;
      foreach (UnityEngine.Object targetObject in this.serializedObject.targetObjects)
        SessionState.SetInt(this.GetUniqueModuleName(targetObject), (int) this.m_VisibilityState);
      if (newState == ModuleUI.VisibilityState.VisibleAndFoldedOut)
        this.Init();
    }

    protected ParticleSystem GetParticleSystem()
    {
      return this.m_Enabled.serializedObject.targetObject as ParticleSystem;
    }

    public ParticleSystemCurveEditor GetParticleSystemCurveEditor()
    {
      return this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
    }

    public void AddToModuleCurves(SerializedProperty curveProp)
    {
      this.m_ModuleCurves.Add(curveProp);
      if (this.foldout)
        return;
      this.m_CurvesRemovedWhenFolded.Add(curveProp);
    }

    private static void Label(Rect rect, GUIContent guiContent)
    {
      GUI.Label(rect, guiContent, ParticleSystemStyles.Get().label);
    }

    protected static Rect GetControlRect(int height, params GUILayoutOption[] layoutOptions)
    {
      return GUILayoutUtility.GetRect(0.0f, (float) height, ModuleUI.s_ControlRectStyle, layoutOptions);
    }

    protected static Rect FieldPosition(Rect totalPosition, out Rect labelPosition)
    {
      labelPosition = new Rect(totalPosition.x + EditorGUI.indent, totalPosition.y, EditorGUIUtility.labelWidth - EditorGUI.indent, 13f);
      return new Rect(totalPosition.x + EditorGUIUtility.labelWidth, totalPosition.y, totalPosition.width - EditorGUIUtility.labelWidth, totalPosition.height);
    }

    protected static Rect PrefixLabel(Rect totalPosition, GUIContent label)
    {
      if (!EditorGUI.LabelHasContent(label))
        return EditorGUI.IndentedRect(totalPosition);
      Rect labelPosition;
      Rect rect = ModuleUI.FieldPosition(totalPosition, out labelPosition);
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, 0, ParticleSystemStyles.Get().label);
      return rect;
    }

    protected static Rect SubtractPopupWidth(Rect position)
    {
      position.width -= 14f;
      return position;
    }

    protected static Rect GetPopupRect(Rect position)
    {
      position.xMin = position.xMax - 13f;
      return position;
    }

    protected static bool PlusButton(Rect position)
    {
      return GUI.Button(new Rect(position.x - 2f, position.y - 2f, 12f, 13f), GUIContent.none, (GUIStyle) "OL Plus");
    }

    protected static bool MinusButton(Rect position)
    {
      return GUI.Button(new Rect(position.x - 2f, position.y - 2f, 12f, 13f), GUIContent.none, (GUIStyle) "OL Minus");
    }

    private static float FloatDraggable(Rect rect, SerializedProperty floatProp, float remap, float dragWidth)
    {
      return ModuleUI.FloatDraggable(rect, floatProp, remap, dragWidth, "g7");
    }

    public static float FloatDraggable(Rect rect, float floatValue, float remap, float dragWidth, string formatString)
    {
      int controlId = GUIUtility.GetControlID(1658656233, FocusType.Keyboard, rect);
      Rect dragHotZone = rect;
      dragHotZone.width = dragWidth;
      Rect position = rect;
      position.xMin += dragWidth;
      return EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, dragHotZone, controlId, floatValue * remap, formatString, ParticleSystemStyles.Get().numberField, true) / remap;
    }

    public static float FloatDraggable(Rect rect, SerializedProperty floatProp, float remap, float dragWidth, string formatString)
    {
      EditorGUI.BeginProperty(rect, GUIContent.none, floatProp);
      EditorGUI.BeginChangeCheck();
      float num = ModuleUI.FloatDraggable(rect, floatProp.floatValue, remap, dragWidth, formatString);
      if (EditorGUI.EndChangeCheck())
        floatProp.floatValue = num;
      EditorGUI.EndProperty();
      return num;
    }

    public static Vector3 GUIVector3Field(GUIContent guiContent, SerializedProperty vecProp, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = vecProp.hasMultipleDifferentValues;
      Rect rect = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), guiContent);
      GUIContent[] guiContentArray = new GUIContent[3]{ new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") };
      float num1 = (float) (((double) rect.width - 8.0) / 3.0);
      rect.width = num1;
      SerializedProperty floatProp = vecProp.Copy();
      floatProp.Next(true);
      Vector3 vector3Value = vecProp.vector3Value;
      for (int index = 0; index < 3; ++index)
      {
        ModuleUI.Label(rect, guiContentArray[index]);
        EditorGUI.BeginChangeCheck();
        float num2 = ModuleUI.FloatDraggable(rect, floatProp, 1f, 25f, "g5");
        if (EditorGUI.EndChangeCheck())
          floatProp.floatValue = num2;
        floatProp.Next(false);
        rect.x += num1 + 4f;
      }
      EditorGUI.showMixedValue = false;
      return vector3Value;
    }

    public static float GUIFloat(string label, SerializedProperty floatProp, params GUILayoutOption[] layoutOptions)
    {
      return ModuleUI.GUIFloat(GUIContent.Temp(label), floatProp, layoutOptions);
    }

    public static float GUIFloat(GUIContent guiContent, SerializedProperty floatProp, params GUILayoutOption[] layoutOptions)
    {
      return ModuleUI.GUIFloat(guiContent, floatProp, "g7", layoutOptions);
    }

    public static float GUIFloat(GUIContent guiContent, SerializedProperty floatProp, string formatString, params GUILayoutOption[] layoutOptions)
    {
      Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
      ModuleUI.PrefixLabel(controlRect, guiContent);
      return ModuleUI.FloatDraggable(controlRect, floatProp, 1f, EditorGUIUtility.labelWidth, formatString);
    }

    public static float GUIFloat(GUIContent guiContent, float floatValue, string formatString, params GUILayoutOption[] layoutOptions)
    {
      Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
      ModuleUI.PrefixLabel(controlRect, guiContent);
      return ModuleUI.FloatDraggable(controlRect, floatValue, 1f, EditorGUIUtility.labelWidth, formatString);
    }

    public static void GUIButtonGroup(UnityEditorInternal.EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, Func<Bounds> getBoundsOfTargets, Editor caller)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(EditorGUIUtility.labelWidth);
      UnityEditorInternal.EditMode.DoInspectorToolbar(modes, guiContents, getBoundsOfTargets, caller);
      GUILayout.EndHorizontal();
    }

    private static bool Toggle(Rect rect, SerializedProperty boolProp)
    {
      EditorGUIInternal.mixedToggleStyle = ParticleSystemStyles.Get().toggleMixed;
      EditorGUI.BeginProperty(rect, GUIContent.none, boolProp);
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUI.Toggle(rect, boolProp.boolValue, ParticleSystemStyles.Get().toggle);
      if (EditorGUI.EndChangeCheck())
        boolProp.boolValue = flag;
      EditorGUI.EndProperty();
      EditorGUIInternal.mixedToggleStyle = EditorStyles.toggleMixed;
      return flag;
    }

    public static bool GUIToggle(string label, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
    {
      return ModuleUI.GUIToggle(GUIContent.Temp(label), boolProp, layoutOptions);
    }

    public static bool GUIToggle(GUIContent guiContent, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
    {
      return ModuleUI.Toggle(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), guiContent), boolProp);
    }

    public static void GUILayerMask(GUIContent guiContent, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = boolProp.hasMultipleDifferentValues;
      EditorGUI.LayerMaskField(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), guiContent), boolProp, (GUIContent) null, ParticleSystemStyles.Get().popup);
      EditorGUI.showMixedValue = false;
    }

    public static bool GUIToggle(GUIContent guiContent, bool boolValue, params GUILayoutOption[] layoutOptions)
    {
      boolValue = EditorGUI.Toggle(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), guiContent), boolValue, ParticleSystemStyles.Get().toggle);
      return boolValue;
    }

    public static void GUIToggleWithFloatField(string name, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
    {
      ModuleUI.GUIToggleWithFloatField(EditorGUIUtility.TempContent(name), boolProp, floatProp, invertToggle, layoutOptions);
    }

    public static void GUIToggleWithFloatField(GUIContent guiContent, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
    {
      Rect rect1 = ModuleUI.PrefixLabel(GUILayoutUtility.GetRect(0.0f, 13f, layoutOptions), guiContent);
      Rect rect2 = rect1;
      rect2.xMax = rect2.x + 9f;
      bool flag = ModuleUI.Toggle(rect2, boolProp);
      if (!(!invertToggle ? flag : !flag))
        return;
      float dragWidth = 25f;
      double num = (double) ModuleUI.FloatDraggable(new Rect((float) ((double) rect1.x + (double) EditorGUIUtility.labelWidth + 9.0), rect1.y, rect1.width - 9f, rect1.height), floatProp, 1f, dragWidth);
    }

    public static void GUIToggleWithIntField(string name, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
    {
      ModuleUI.GUIToggleWithIntField(EditorGUIUtility.TempContent(name), boolProp, floatProp, invertToggle, layoutOptions);
    }

    public static void GUIToggleWithIntField(GUIContent guiContent, SerializedProperty boolProp, SerializedProperty intProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
    {
      Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
      Rect rect1 = ModuleUI.PrefixLabel(controlRect, guiContent);
      rect1.xMax = rect1.x + 9f;
      bool flag = ModuleUI.Toggle(rect1, boolProp);
      if (!(!invertToggle ? flag : !flag))
        return;
      EditorGUI.showMixedValue = intProp.hasMultipleDifferentValues;
      float dragWidth = 25f;
      Rect rect2 = new Rect(rect1.xMax, controlRect.y, (float) ((double) controlRect.width - (double) rect1.xMax + 9.0), controlRect.height);
      EditorGUI.BeginChangeCheck();
      int num = ModuleUI.IntDraggable(rect2, (GUIContent) null, intProp.intValue, dragWidth);
      if (EditorGUI.EndChangeCheck())
        intProp.intValue = num;
      EditorGUI.showMixedValue = false;
    }

    public static void GUIObject(GUIContent label, SerializedProperty objectProp, params GUILayoutOption[] layoutOptions)
    {
      ModuleUI.GUIObject(label, objectProp, (System.Type) null, layoutOptions);
    }

    public static void GUIObject(GUIContent label, SerializedProperty objectProp, System.Type objType, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = objectProp.hasMultipleDifferentValues;
      EditorGUI.ObjectField(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label), objectProp, objType, GUIContent.none, ParticleSystemStyles.Get().objectField);
      EditorGUI.showMixedValue = false;
    }

    public static void GUIObjectFieldAndToggle(GUIContent label, SerializedProperty objectProp, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
    {
      Rect rect = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label);
      EditorGUI.showMixedValue = objectProp.hasMultipleDifferentValues;
      rect.xMax -= 19f;
      EditorGUI.ObjectField(rect, objectProp, GUIContent.none);
      EditorGUI.showMixedValue = false;
      if (boolProp == null)
        return;
      rect.x += rect.width + 10f;
      rect.width = 9f;
      ModuleUI.Toggle(rect, boolProp);
    }

    internal UnityEngine.Object ParticleSystemValidator(UnityEngine.Object[] references, System.Type objType, SerializedProperty property)
    {
      foreach (UnityEngine.Object reference in references)
      {
        if (reference != (UnityEngine.Object) null)
        {
          GameObject gameObject = reference as GameObject;
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            if ((bool) ((UnityEngine.Object) component))
              return (UnityEngine.Object) component;
          }
        }
      }
      return (UnityEngine.Object) null;
    }

    public int GUIListOfFloatObjectToggleFields(GUIContent label, SerializedProperty[] objectProps, EditorGUI.ObjectFieldValidator validator, GUIContent buttonTooltip, bool allowCreation, params GUILayoutOption[] layoutOptions)
    {
      int num1 = -1;
      int length = objectProps.Length;
      Rect rect1 = GUILayoutUtility.GetRect(0.0f, (float) (15 * length), layoutOptions);
      rect1.height = 13f;
      float num2 = 10f;
      float num3 = 35f;
      float num4 = 10f;
      float width = (float) ((double) rect1.width - (double) num2 - (double) num3 - (double) num4 * 2.0 - 9.0);
      ModuleUI.PrefixLabel(rect1, label);
      for (int index = 0; index < length; ++index)
      {
        SerializedProperty objectProp = objectProps[index];
        EditorGUI.showMixedValue = objectProp.hasMultipleDifferentValues;
        Rect rect2 = new Rect(rect1.x + num2 + num3 + num4, rect1.y, width, rect1.height);
        int controlId = GUIUtility.GetControlID(1235498, FocusType.Keyboard, rect2);
        EditorGUI.DoObjectField(rect2, rect2, controlId, (UnityEngine.Object) null, (System.Type) null, objectProp, validator, true, ParticleSystemStyles.Get().objectField);
        EditorGUI.showMixedValue = false;
        if (objectProp.objectReferenceValue == (UnityEngine.Object) null)
        {
          rect2 = new Rect(rect1.xMax - 9f, rect1.y + 3f, 9f, 9f);
          if (!allowCreation || GUI.Button(rect2, buttonTooltip ?? GUIContent.none, ParticleSystemStyles.Get().plus))
            num1 = index;
        }
        rect1.y += 15f;
      }
      return num1;
    }

    public static void GUIIntDraggableX2(GUIContent mainLabel, GUIContent label1, SerializedProperty intProp1, GUIContent label2, SerializedProperty intProp2, params GUILayoutOption[] layoutOptions)
    {
      Rect rect1 = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), mainLabel);
      float width = (float) (((double) rect1.width - 4.0) * 0.5);
      Rect rect2 = new Rect(rect1.x, rect1.y, width, rect1.height);
      ModuleUI.IntDraggable(rect2, label1, intProp1, 10f);
      rect2.x += width + 4f;
      ModuleUI.IntDraggable(rect2, label2, intProp2, 10f);
    }

    public static int IntDraggable(Rect rect, GUIContent label, SerializedProperty intProp, float dragWidth)
    {
      EditorGUI.BeginProperty(rect, GUIContent.none, intProp);
      EditorGUI.BeginChangeCheck();
      int num = ModuleUI.IntDraggable(rect, label, intProp.intValue, dragWidth);
      if (EditorGUI.EndChangeCheck())
        intProp.intValue = num;
      EditorGUI.EndProperty();
      return intProp.intValue;
    }

    public static int GUIInt(GUIContent guiContent, SerializedProperty intProp, params GUILayoutOption[] layoutOptions)
    {
      Rect rect = GUILayoutUtility.GetRect(0.0f, 13f, layoutOptions);
      EditorGUI.BeginProperty(rect, GUIContent.none, intProp);
      ModuleUI.PrefixLabel(rect, guiContent);
      EditorGUI.BeginChangeCheck();
      int num = ModuleUI.IntDraggable(rect, (GUIContent) null, intProp.intValue, EditorGUIUtility.labelWidth);
      if (EditorGUI.EndChangeCheck())
        intProp.intValue = num;
      EditorGUI.EndProperty();
      return intProp.intValue;
    }

    public static int IntDraggable(Rect rect, GUIContent label, int value, float dragWidth)
    {
      float width = rect.width;
      Rect position1 = rect;
      position1.width = width;
      int controlId = GUIUtility.GetControlID(16586232, FocusType.Keyboard, position1);
      Rect rect1 = position1;
      rect1.width = dragWidth;
      if (label != null && !string.IsNullOrEmpty(label.text))
        ModuleUI.Label(rect1, label);
      Rect position2 = position1;
      position2.x += dragWidth;
      position2.width = width - dragWidth;
      float dragSensitivity = Mathf.Max(1f, Mathf.Pow(Mathf.Abs((float) value), 0.5f) * 0.03f);
      return (int) EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position2, rect1, controlId, (float) value, EditorGUI.kIntFieldFormatString, ParticleSystemStyles.Get().numberField, true, dragSensitivity);
    }

    public static void GUIMinMaxRange(GUIContent label, SerializedProperty vec2Prop, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = vec2Prop.hasMultipleDifferentValues;
      Rect rect = ModuleUI.PrefixLabel(ModuleUI.SubtractPopupWidth(ModuleUI.GetControlRect(13, layoutOptions)), label);
      float num = (float) (((double) rect.width - 20.0) * 0.5);
      Vector2 vector2Value = vec2Prop.vector2Value;
      EditorGUI.BeginChangeCheck();
      rect.width = num;
      rect.xMin -= 20f;
      vector2Value.x = ModuleUI.FloatDraggable(rect, vector2Value.x, 1f, 20f, "g7");
      rect.x += num + 20f;
      vector2Value.y = ModuleUI.FloatDraggable(rect, vector2Value.y, 1f, 20f, "g7");
      if (EditorGUI.EndChangeCheck())
        vec2Prop.vector2Value = vector2Value;
      EditorGUI.showMixedValue = false;
    }

    public static void GUISlider(SerializedProperty floatProp, float a, float b, float remap)
    {
      ModuleUI.GUISlider("", floatProp, a, b, remap);
    }

    public static void GUISlider(string name, SerializedProperty floatProp, float a, float b, float remap)
    {
      EditorGUI.showMixedValue = floatProp.hasMultipleDifferentValues;
      EditorGUI.BeginChangeCheck();
      float num = EditorGUILayout.Slider(name, (float) ((double) floatProp.floatValue * (double) remap), a, b, new GUILayoutOption[1]{ GUILayout.MinWidth(300f) }) / remap;
      if (EditorGUI.EndChangeCheck())
        floatProp.floatValue = num;
      EditorGUI.showMixedValue = false;
    }

    public static void GUIMinMaxSlider(GUIContent label, SerializedProperty vec2Prop, float a, float b, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = vec2Prop.hasMultipleDifferentValues;
      Rect controlRect = ModuleUI.GetControlRect(26, layoutOptions);
      controlRect.height = 13f;
      controlRect.y += 3f;
      ModuleUI.PrefixLabel(controlRect, label);
      Vector2 vector2Value = vec2Prop.vector2Value;
      controlRect.y += 13f;
      EditorGUI.BeginChangeCheck();
      EditorGUI.MinMaxSlider(controlRect, ref vector2Value.x, ref vector2Value.y, a, b);
      if (EditorGUI.EndChangeCheck())
        vec2Prop.vector2Value = vector2Value;
      EditorGUI.showMixedValue = false;
    }

    public static bool GUIBoolAsPopup(GUIContent label, SerializedProperty boolProp, string[] options, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = boolProp.hasMultipleDifferentValues;
      Rect position = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.Popup(position, (GUIContent) null, !boolProp.boolValue ? 0 : 1, EditorGUIUtility.TempContent(options), ParticleSystemStyles.Get().popup);
      if (EditorGUI.EndChangeCheck())
        boolProp.boolValue = num > 0;
      EditorGUI.showMixedValue = false;
      return num > 0;
    }

    public static Enum GUIEnumMask(GUIContent label, Enum enumValue, params GUILayoutOption[] layoutOptions)
    {
      return EditorGUI.EnumFlagsField(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label), enumValue, ParticleSystemStyles.Get().popup);
    }

    public static void GUIMask(GUIContent label, SerializedProperty intProp, string[] options, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = intProp.hasMultipleDifferentValues;
      Rect position = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.MaskField(position, label, intProp.intValue, options, ParticleSystemStyles.Get().popup);
      if (EditorGUI.EndChangeCheck())
        intProp.intValue = num;
      EditorGUI.showMixedValue = false;
    }

    public static int GUIPopup(GUIContent label, SerializedProperty intProp, GUIContent[] options, params GUILayoutOption[] layoutOptions)
    {
      EditorGUI.showMixedValue = intProp.hasMultipleDifferentValues;
      Rect position = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.Popup(position, (GUIContent) null, intProp.intValue, options, ParticleSystemStyles.Get().popup);
      if (EditorGUI.EndChangeCheck())
        intProp.intValue = num;
      EditorGUI.showMixedValue = false;
      return intProp.intValue;
    }

    public static int GUIPopup(GUIContent label, int intValue, GUIContent[] options, params GUILayoutOption[] layoutOptions)
    {
      return EditorGUI.Popup(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13, layoutOptions), label), intValue, options, ParticleSystemStyles.Get().popup);
    }

    private static Color GetColor(SerializedMinMaxCurve mmCurve)
    {
      return mmCurve.m_Module.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor().GetCurveColor(mmCurve.maxCurve);
    }

    private static void GUICurveField(Rect position, SerializedProperty maxCurve, SerializedProperty minCurve, Color color, Rect ranges, ModuleUI.CurveFieldMouseDownCallback mouseDownCallback)
    {
      int controlId = GUIUtility.GetControlID(1321321231, FocusType.Keyboard, position);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (!position.Contains(current.mousePosition) || mouseDownCallback == null || !mouseDownCallback(current.button, position, ranges))
            break;
          current.Use();
          break;
        case EventType.Repaint:
          Rect position1 = position;
          if (minCurve == null)
            EditorGUIUtility.DrawCurveSwatch(position1, (AnimationCurve) null, maxCurve, color, EditorGUI.kCurveBGColor, ranges);
          else
            EditorGUIUtility.DrawRegionSwatch(position1, maxCurve, minCurve, color, EditorGUI.kCurveBGColor, ranges);
          EditorStyles.colorPickerBox.Draw(position1, GUIContent.none, controlId, false);
          break;
        case EventType.ValidateCommand:
          if (!(current.commandName == "UndoRedoPerformed"))
            break;
          AnimationCurvePreviewCache.ClearCache();
          break;
      }
    }

    public static void GUIMinMaxCurve(string label, SerializedMinMaxCurve mmCurve, params GUILayoutOption[] layoutOptions)
    {
      ModuleUI.GUIMinMaxCurve(GUIContent.Temp(label), mmCurve, layoutOptions);
    }

    public static void GUIMinMaxCurve(GUIContent label, SerializedMinMaxCurve mmCurve, params GUILayoutOption[] layoutOptions)
    {
      ModuleUI.GUIMinMaxCurve(label, mmCurve, (SerializedProperty) null, layoutOptions);
    }

    public static void GUIMinMaxCurve(SerializedProperty editableLabel, SerializedMinMaxCurve mmCurve, params GUILayoutOption[] layoutOptions)
    {
      ModuleUI.GUIMinMaxCurve((GUIContent) null, mmCurve, editableLabel, layoutOptions);
    }

    internal static void GUIMinMaxCurve(GUIContent label, SerializedMinMaxCurve mmCurve, SerializedProperty editableLabel, params GUILayoutOption[] layoutOptions)
    {
      bool multipleDifferentValues = mmCurve.stateHasMultipleDifferentValues;
      Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
      Rect popupRect = ModuleUI.GetPopupRect(controlRect);
      Rect rect1 = ModuleUI.SubtractPopupWidth(controlRect);
      Rect rect2;
      if (editableLabel != null)
      {
        Rect labelPosition;
        rect2 = ModuleUI.FieldPosition(rect1, out labelPosition);
        labelPosition.width -= 4f;
        float x = ParticleSystemStyles.Get().editableLabel.CalcSize(GUIContent.Temp(editableLabel.stringValue)).x;
        labelPosition.width = Mathf.Min(labelPosition.width, x + 4f);
        EditorGUI.BeginProperty(labelPosition, GUIContent.none, editableLabel);
        EditorGUI.BeginChangeCheck();
        string str = EditorGUI.TextFieldInternal(GUIUtility.GetControlID(FocusType.Passive, labelPosition), labelPosition, editableLabel.stringValue, ParticleSystemStyles.Get().editableLabel);
        if (EditorGUI.EndChangeCheck())
          editableLabel.stringValue = str;
        EditorGUI.EndProperty();
      }
      else
        rect2 = ModuleUI.PrefixLabel(rect1, label);
      if (multipleDifferentValues)
      {
        ModuleUI.Label(rect2, GUIContent.Temp("-"));
      }
      else
      {
        MinMaxCurveState state = mmCurve.state;
        switch (state)
        {
          case MinMaxCurveState.k_Scalar:
            EditorGUI.BeginChangeCheck();
            float a1 = ModuleUI.FloatDraggable(rect1, mmCurve.scalar, mmCurve.m_RemapValue, EditorGUIUtility.labelWidth);
            if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
            {
              mmCurve.scalar.floatValue = Mathf.Max(a1, 0.0f);
              break;
            }
            break;
          case MinMaxCurveState.k_TwoScalars:
            Rect rect3 = rect2;
            rect3.width = (float) (((double) rect2.width - 20.0) * 0.5);
            Rect rect4 = rect3;
            rect4.xMin -= 20f;
            EditorGUI.BeginChangeCheck();
            float a2 = ModuleUI.FloatDraggable(rect4, mmCurve.minScalar, mmCurve.m_RemapValue, 20f, "g5");
            if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
              mmCurve.minScalar.floatValue = Mathf.Max(a2, 0.0f);
            rect4.x += rect3.width + 20f;
            EditorGUI.BeginChangeCheck();
            float a3 = ModuleUI.FloatDraggable(rect4, mmCurve.scalar, mmCurve.m_RemapValue, 20f, "g5");
            if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
            {
              mmCurve.scalar.floatValue = Mathf.Max(a3, 0.0f);
              break;
            }
            break;
          default:
            Rect ranges = !mmCurve.signedRange ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
            SerializedProperty minCurve = state != MinMaxCurveState.k_TwoCurves ? (SerializedProperty) null : mmCurve.minCurve;
            ModuleUI.GUICurveField(rect2, mmCurve.maxCurve, minCurve, ModuleUI.GetColor(mmCurve), ranges, new ModuleUI.CurveFieldMouseDownCallback(mmCurve.OnCurveAreaMouseDown));
            break;
        }
      }
      ModuleUI.GUIMMCurveStateList(popupRect, mmCurve);
    }

    public static Rect GUIMinMaxCurveInline(Rect rect, SerializedMinMaxCurve mmCurve, float dragWidth)
    {
      if (mmCurve.stateHasMultipleDifferentValues)
      {
        ModuleUI.Label(rect, GUIContent.Temp("-"));
      }
      else
      {
        MinMaxCurveState state = mmCurve.state;
        switch (state)
        {
          case MinMaxCurveState.k_Scalar:
            EditorGUI.BeginChangeCheck();
            float a1 = ModuleUI.FloatDraggable(rect, mmCurve.scalar, mmCurve.m_RemapValue, dragWidth, "n0");
            if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
            {
              mmCurve.scalar.floatValue = Mathf.Max(a1, 0.0f);
              break;
            }
            break;
          case MinMaxCurveState.k_TwoScalars:
            Rect rect1 = rect;
            rect1.width = rect.width * 0.5f;
            Rect rect2 = rect1;
            EditorGUI.BeginChangeCheck();
            float a2 = ModuleUI.FloatDraggable(rect2, mmCurve.minScalar, mmCurve.m_RemapValue, dragWidth, "n0");
            if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
              mmCurve.minScalar.floatValue = Mathf.Max(a2, 0.0f);
            rect2.x += rect1.width;
            EditorGUI.BeginChangeCheck();
            float a3 = ModuleUI.FloatDraggable(rect2, mmCurve.scalar, mmCurve.m_RemapValue, dragWidth, "n0");
            if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
            {
              mmCurve.scalar.floatValue = Mathf.Max(a3, 0.0f);
              break;
            }
            break;
          default:
            Rect ranges = !mmCurve.signedRange ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
            SerializedProperty minCurve = state != MinMaxCurveState.k_TwoCurves ? (SerializedProperty) null : mmCurve.minCurve;
            ModuleUI.GUICurveField(rect, mmCurve.maxCurve, minCurve, ModuleUI.GetColor(mmCurve), ranges, new ModuleUI.CurveFieldMouseDownCallback(mmCurve.OnCurveAreaMouseDown));
            break;
        }
      }
      rect.width += 13f;
      ModuleUI.GUIMMCurveStateList(ModuleUI.GetPopupRect(rect), mmCurve);
      return rect;
    }

    public void GUIMinMaxGradient(GUIContent label, SerializedMinMaxGradient minMaxGradient, bool hdr, params GUILayoutOption[] layoutOptions)
    {
      this.GUIMinMaxGradient(label, minMaxGradient, (SerializedProperty) null, hdr, layoutOptions);
    }

    public void GUIMinMaxGradient(SerializedProperty editableLabel, SerializedMinMaxGradient minMaxGradient, bool hdr, params GUILayoutOption[] layoutOptions)
    {
      this.GUIMinMaxGradient((GUIContent) null, minMaxGradient, editableLabel, hdr, layoutOptions);
    }

    internal void GUIMinMaxGradient(GUIContent label, SerializedMinMaxGradient minMaxGradient, SerializedProperty editableLabel, bool hdr, params GUILayoutOption[] layoutOptions)
    {
      bool multipleDifferentValues = minMaxGradient.stateHasMultipleDifferentValues;
      MinMaxGradientState state = minMaxGradient.state;
      Rect rect1 = GUILayoutUtility.GetRect(0.0f, multipleDifferentValues || state != MinMaxGradientState.k_RandomBetweenTwoColors && state != MinMaxGradientState.k_RandomBetweenTwoGradients ? 13f : 26f, layoutOptions);
      Rect popupRect = ModuleUI.GetPopupRect(rect1);
      Rect totalPosition = ModuleUI.SubtractPopupWidth(rect1);
      Rect rect2;
      if (editableLabel != null)
      {
        Rect labelPosition;
        rect2 = ModuleUI.FieldPosition(totalPosition, out labelPosition);
        labelPosition.width -= 4f;
        EditorGUI.BeginProperty(labelPosition, GUIContent.none, editableLabel);
        EditorGUI.BeginChangeCheck();
        string str = EditorGUI.TextFieldInternal(GUIUtility.GetControlID(FocusType.Passive, labelPosition), labelPosition, editableLabel.stringValue, ParticleSystemStyles.Get().editableLabel);
        if (EditorGUI.EndChangeCheck())
          editableLabel.stringValue = str;
        EditorGUI.EndProperty();
      }
      else
        rect2 = ModuleUI.PrefixLabel(totalPosition, label);
      rect2.height = 13f;
      if (multipleDifferentValues)
      {
        ModuleUI.Label(rect2, GUIContent.Temp("-"));
      }
      else
      {
        switch (state)
        {
          case MinMaxGradientState.k_Color:
            EditorGUI.showMixedValue = minMaxGradient.m_MaxColor.hasMultipleDifferentValues;
            ModuleUI.GUIColor(rect2, minMaxGradient.m_MaxColor, hdr);
            EditorGUI.showMixedValue = false;
            break;
          case MinMaxGradientState.k_Gradient:
          case MinMaxGradientState.k_RandomColor:
            EditorGUI.showMixedValue = minMaxGradient.m_MaxGradient.hasMultipleDifferentValues;
            EditorGUI.GradientField(rect2, minMaxGradient.m_MaxGradient, hdr);
            EditorGUI.showMixedValue = false;
            break;
          case MinMaxGradientState.k_RandomBetweenTwoColors:
            EditorGUI.showMixedValue = minMaxGradient.m_MaxColor.hasMultipleDifferentValues;
            ModuleUI.GUIColor(rect2, minMaxGradient.m_MaxColor, hdr);
            EditorGUI.showMixedValue = false;
            rect2.y += rect2.height;
            EditorGUI.showMixedValue = minMaxGradient.m_MinColor.hasMultipleDifferentValues;
            ModuleUI.GUIColor(rect2, minMaxGradient.m_MinColor, hdr);
            EditorGUI.showMixedValue = false;
            break;
          case MinMaxGradientState.k_RandomBetweenTwoGradients:
            EditorGUI.showMixedValue = minMaxGradient.m_MaxGradient.hasMultipleDifferentValues;
            EditorGUI.GradientField(rect2, minMaxGradient.m_MaxGradient, hdr);
            EditorGUI.showMixedValue = false;
            rect2.y += rect2.height;
            EditorGUI.showMixedValue = minMaxGradient.m_MinGradient.hasMultipleDifferentValues;
            EditorGUI.GradientField(rect2, minMaxGradient.m_MinGradient, hdr);
            EditorGUI.showMixedValue = false;
            break;
        }
      }
      ModuleUI.GUIMMGradientPopUp(popupRect, minMaxGradient);
    }

    private static void GUIColor(Rect rect, SerializedProperty colorProp)
    {
      ModuleUI.GUIColor(rect, colorProp, false);
    }

    private static void GUIColor(Rect rect, SerializedProperty colorProp, bool hdr)
    {
      EditorGUI.BeginChangeCheck();
      Color color = EditorGUI.ColorField(rect, GUIContent.none, colorProp.colorValue, false, true, hdr, ColorPicker.defaultHDRConfig);
      if (!EditorGUI.EndChangeCheck())
        return;
      colorProp.colorValue = color;
    }

    public void GUITripleMinMaxCurve(GUIContent label, GUIContent x, SerializedMinMaxCurve xCurve, GUIContent y, SerializedMinMaxCurve yCurve, GUIContent z, SerializedMinMaxCurve zCurve, SerializedProperty randomizePerFrame, params GUILayoutOption[] layoutOptions)
    {
      bool multipleDifferentValues = xCurve.stateHasMultipleDifferentValues;
      MinMaxCurveState state = xCurve.state;
      bool flag = label != GUIContent.none;
      int num1 = !flag ? 1 : 2;
      if (state == MinMaxCurveState.k_TwoScalars)
        ++num1;
      Rect controlRect = ModuleUI.GetControlRect(13 * num1, layoutOptions);
      Rect popupRect = ModuleUI.GetPopupRect(controlRect);
      Rect totalPosition = ModuleUI.SubtractPopupWidth(controlRect);
      Rect rect = totalPosition;
      float num2 = (float) (((double) totalPosition.width - 8.0) / 3.0);
      if (num1 > 1)
        rect.height = 13f;
      if (flag)
      {
        ModuleUI.PrefixLabel(totalPosition, label);
        rect.y += rect.height;
      }
      rect.width = num2;
      GUIContent[] guiContentArray = new GUIContent[3]{ x, y, z };
      SerializedMinMaxCurve[] minMaxCurves = new SerializedMinMaxCurve[3]{ xCurve, yCurve, zCurve };
      if (multipleDifferentValues)
      {
        ModuleUI.Label(rect, GUIContent.Temp("-"));
      }
      else
      {
        switch (state)
        {
          case MinMaxCurveState.k_Scalar:
            for (int index = 0; index < minMaxCurves.Length; ++index)
            {
              ModuleUI.Label(rect, guiContentArray[index]);
              EditorGUI.BeginChangeCheck();
              float a = ModuleUI.FloatDraggable(rect, minMaxCurves[index].scalar, minMaxCurves[index].m_RemapValue, 10f);
              if (EditorGUI.EndChangeCheck() && !minMaxCurves[index].signedRange)
                minMaxCurves[index].scalar.floatValue = Mathf.Max(a, 0.0f);
              rect.x += num2 + 4f;
            }
            break;
          case MinMaxCurveState.k_TwoScalars:
            for (int index = 0; index < minMaxCurves.Length; ++index)
            {
              ModuleUI.Label(rect, guiContentArray[index]);
              float minConstant = minMaxCurves[index].minConstant;
              float maxConstant = minMaxCurves[index].maxConstant;
              EditorGUI.BeginChangeCheck();
              float num3 = ModuleUI.FloatDraggable(rect, maxConstant, minMaxCurves[index].m_RemapValue, 10f, "g5");
              if (EditorGUI.EndChangeCheck())
                minMaxCurves[index].maxConstant = num3;
              rect.y += 13f;
              EditorGUI.BeginChangeCheck();
              float num4 = ModuleUI.FloatDraggable(rect, minConstant, minMaxCurves[index].m_RemapValue, 10f, "g5");
              if (EditorGUI.EndChangeCheck())
                minMaxCurves[index].minConstant = num4;
              rect.x += num2 + 4f;
              rect.y -= 13f;
            }
            break;
          default:
            rect.width = num2;
            Rect ranges = !xCurve.signedRange ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
            for (int index = 0; index < minMaxCurves.Length; ++index)
            {
              ModuleUI.Label(rect, guiContentArray[index]);
              Rect position = rect;
              position.xMin += 10f;
              SerializedProperty minCurve = state != MinMaxCurveState.k_TwoCurves ? (SerializedProperty) null : minMaxCurves[index].minCurve;
              ModuleUI.GUICurveField(position, minMaxCurves[index].maxCurve, minCurve, ModuleUI.GetColor(minMaxCurves[index]), ranges, new ModuleUI.CurveFieldMouseDownCallback(minMaxCurves[index].OnCurveAreaMouseDown));
              rect.x += num2 + 4f;
            }
            break;
        }
      }
      ModuleUI.GUIMMCurveStateList(popupRect, minMaxCurves);
    }

    private static void SelectMinMaxCurveStateCallback(object obj)
    {
      ModuleUI.CurveStateCallbackData stateCallbackData = (ModuleUI.CurveStateCallbackData) obj;
      foreach (SerializedMinMaxCurve minMaxCurve in stateCallbackData.minMaxCurves)
        minMaxCurve.state = stateCallbackData.selectedState;
    }

    public static void GUIMMCurveStateList(Rect rect, SerializedMinMaxCurve minMaxCurves)
    {
      SerializedMinMaxCurve[] minMaxCurves1 = new SerializedMinMaxCurve[1]{ minMaxCurves };
      ModuleUI.GUIMMCurveStateList(rect, minMaxCurves1);
    }

    public static void GUIMMCurveStateList(Rect rect, SerializedMinMaxCurve[] minMaxCurves)
    {
      if (!EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown) || minMaxCurves.Length == 0)
        return;
      GUIContent[] guiContentArray = new GUIContent[4]{ new GUIContent("Constant"), new GUIContent("Curve"), new GUIContent("Random Between Two Constants"), new GUIContent("Random Between Two Curves") };
      MinMaxCurveState[] minMaxCurveStateArray = new MinMaxCurveState[4]{ MinMaxCurveState.k_Scalar, MinMaxCurveState.k_Curve, MinMaxCurveState.k_TwoScalars, MinMaxCurveState.k_TwoCurves };
      bool[] flagArray = new bool[4]{ minMaxCurves[0].m_AllowConstant, minMaxCurves[0].m_AllowCurves, minMaxCurves[0].m_AllowRandom, minMaxCurves[0].m_AllowRandom && minMaxCurves[0].m_AllowCurves };
      bool flag = !minMaxCurves[0].stateHasMultipleDifferentValues;
      GenericMenu genericMenu1 = new GenericMenu();
      for (int index = 0; index < guiContentArray.Length; ++index)
      {
        if (flagArray[index])
        {
          GenericMenu genericMenu2 = genericMenu1;
          GUIContent content = guiContentArray[index];
          int num = !flag ? 0 : (minMaxCurves[0].state == minMaxCurveStateArray[index] ? 1 : 0);
          // ISSUE: reference to a compiler-generated field
          if (ModuleUI.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ModuleUI.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxCurveStateCallback);
          }
          // ISSUE: reference to a compiler-generated field
          GenericMenu.MenuFunction2 fMgCache0 = ModuleUI.\u003C\u003Ef__mg\u0024cache0;
          ModuleUI.CurveStateCallbackData stateCallbackData = new ModuleUI.CurveStateCallbackData(minMaxCurveStateArray[index], minMaxCurves);
          genericMenu2.AddItem(content, num != 0, fMgCache0, (object) stateCallbackData);
        }
      }
      genericMenu1.DropDown(rect);
      Event.current.Use();
    }

    private static void SelectMinMaxGradientStateCallback(object obj)
    {
      ModuleUI.GradientCallbackData gradientCallbackData = (ModuleUI.GradientCallbackData) obj;
      gradientCallbackData.gradientProp.state = gradientCallbackData.selectedState;
    }

    public static void GUIMMGradientPopUp(Rect rect, SerializedMinMaxGradient gradientProp)
    {
      if (!EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
        return;
      GUIContent[] guiContentArray = new GUIContent[5]{ new GUIContent("Color"), new GUIContent("Gradient"), new GUIContent("Random Between Two Colors"), new GUIContent("Random Between Two Gradients"), new GUIContent("Random Color") };
      MinMaxGradientState[] maxGradientStateArray = new MinMaxGradientState[5]{ MinMaxGradientState.k_Color, MinMaxGradientState.k_Gradient, MinMaxGradientState.k_RandomBetweenTwoColors, MinMaxGradientState.k_RandomBetweenTwoGradients, MinMaxGradientState.k_RandomColor };
      bool[] flagArray = new bool[5]{ gradientProp.m_AllowColor, gradientProp.m_AllowGradient, gradientProp.m_AllowRandomBetweenTwoColors, gradientProp.m_AllowRandomBetweenTwoGradients, gradientProp.m_AllowRandomColor };
      bool flag = !gradientProp.stateHasMultipleDifferentValues;
      GenericMenu genericMenu1 = new GenericMenu();
      for (int index = 0; index < guiContentArray.Length; ++index)
      {
        if (flagArray[index])
        {
          GenericMenu genericMenu2 = genericMenu1;
          GUIContent content = guiContentArray[index];
          int num = !flag ? 0 : (gradientProp.state == maxGradientStateArray[index] ? 1 : 0);
          // ISSUE: reference to a compiler-generated field
          if (ModuleUI.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ModuleUI.\u003C\u003Ef__mg\u0024cache1 = new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxGradientStateCallback);
          }
          // ISSUE: reference to a compiler-generated field
          GenericMenu.MenuFunction2 fMgCache1 = ModuleUI.\u003C\u003Ef__mg\u0024cache1;
          ModuleUI.GradientCallbackData gradientCallbackData = new ModuleUI.GradientCallbackData(maxGradientStateArray[index], gradientProp);
          genericMenu2.AddItem(content, num != 0, fMgCache1, (object) gradientCallbackData);
        }
      }
      genericMenu1.ShowAsContext();
      Event.current.Use();
    }

    private static void SelectMinMaxColorStateCallback(object obj)
    {
      ModuleUI.ColorCallbackData colorCallbackData = (ModuleUI.ColorCallbackData) obj;
      colorCallbackData.boolProp.boolValue = colorCallbackData.selectedState;
    }

    public static void GUIMMColorPopUp(Rect rect, SerializedProperty boolProp)
    {
      if (!EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
        return;
      GenericMenu genericMenu1 = new GenericMenu();
      GUIContent[] guiContentArray = new GUIContent[2]{ new GUIContent("Constant Color"), new GUIContent("Random Between Two Colors") };
      bool[] flagArray = new bool[2]{ false, true };
      for (int index = 0; index < guiContentArray.Length; ++index)
      {
        GenericMenu genericMenu2 = genericMenu1;
        GUIContent content = guiContentArray[index];
        int num = boolProp.boolValue == flagArray[index] ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        if (ModuleUI.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ModuleUI.\u003C\u003Ef__mg\u0024cache2 = new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxColorStateCallback);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction2 fMgCache2 = ModuleUI.\u003C\u003Ef__mg\u0024cache2;
        ModuleUI.ColorCallbackData colorCallbackData = new ModuleUI.ColorCallbackData(flagArray[index], boolProp);
        genericMenu2.AddItem(content, num != 0, fMgCache2, (object) colorCallbackData);
      }
      genericMenu1.ShowAsContext();
      Event.current.Use();
    }

    public enum VisibilityState
    {
      NotVisible,
      VisibleAndFolded,
      VisibleAndFoldedOut,
    }

    public delegate bool CurveFieldMouseDownCallback(int button, Rect drawRect, Rect curveRanges);

    private class CurveStateCallbackData
    {
      public SerializedMinMaxCurve[] minMaxCurves;
      public MinMaxCurveState selectedState;

      public CurveStateCallbackData(MinMaxCurveState state, SerializedMinMaxCurve[] curves)
      {
        this.minMaxCurves = curves;
        this.selectedState = state;
      }
    }

    private class GradientCallbackData
    {
      public SerializedMinMaxGradient gradientProp;
      public MinMaxGradientState selectedState;

      public GradientCallbackData(MinMaxGradientState state, SerializedMinMaxGradient p)
      {
        this.gradientProp = p;
        this.selectedState = state;
      }
    }

    private class ColorCallbackData
    {
      public SerializedProperty boolProp;
      public bool selectedState;

      public ColorCallbackData(bool state, SerializedProperty bp)
      {
        this.boolProp = bp;
        this.selectedState = state;
      }
    }
  }
}
