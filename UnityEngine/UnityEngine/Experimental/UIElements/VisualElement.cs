// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualElement
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.CSSLayout;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Base class for objects that are part of the UIElements visual tree.</para>
  /// </summary>
  public class VisualElement : Focusable, ITransform, IUIElementDataWatch, IEnumerable<VisualElement>, IVisualElementScheduler, IStyle, IEnumerable
  {
    private static readonly VisualElement[] s_EmptyList = new VisualElement[0];
    private Vector3 m_Position = Vector3.zero;
    private Quaternion m_Rotation = Quaternion.identity;
    private Vector3 m_Scale = Vector3.one;
    internal VisualElementStylesData m_SharedStyle = VisualElementStylesData.none;
    internal VisualElementStylesData m_Style = VisualElementStylesData.none;
    private static uint s_NextId;
    private string m_Name;
    private HashSet<string> m_ClassList;
    private string m_TypeName;
    private string m_FullTypeName;
    private string m_PersistenceKey;
    private RenderData m_RenderData;
    private Rect m_Layout;
    private PseudoStates m_PseudoStates;
    internal readonly uint controlid;
    private ChangeType changesNeeded;
    [SerializeField]
    private string m_Text;
    private bool m_Enabled;
    internal const Align DefaultAlignContent = Align.FlexStart;
    internal const Align DefaultAlignItems = Align.Stretch;
    private VisualElement.ClippingOptions m_ClippingOptions;
    private VisualElement m_PhysicalParent;
    private VisualElement m_LogicalParent;
    private List<VisualElement> m_Children;
    private List<StyleSheet> m_StyleSheets;
    private List<string> m_StyleSheetPaths;

    public VisualElement()
    {
      this.controlid = ++VisualElement.s_NextId;
      this.shadow = new VisualElement.Hierarchy(this);
      this.m_ClassList = new HashSet<string>();
      this.m_FullTypeName = string.Empty;
      this.m_TypeName = string.Empty;
      this.SetEnabled(true);
      this.visible = true;
      this.focusIndex = -1;
      this.name = string.Empty;
      this.cssNode = new CSSNode();
      this.cssNode.SetMeasureFunction(new MeasureFunction(this.Measure));
      this.changesNeeded = ChangeType.All;
      this.clippingOptions = VisualElement.ClippingOptions.ClipContents;
    }

    /// <summary>
    ///   <para>Used for view data persistence (ie. tree expanded states, scroll position, zoom level).</para>
    /// </summary>
    public string persistenceKey
    {
      get
      {
        return this.m_PersistenceKey;
      }
      set
      {
        if (!(this.m_PersistenceKey != value))
          return;
        this.m_PersistenceKey = value;
        if (!string.IsNullOrEmpty(value))
          this.Dirty(ChangeType.PersistentData);
      }
    }

    internal bool enablePersistence { get; private set; }

    /// <summary>
    ///   <para>This property can be used to associate application-specific user data with this VisualElement.</para>
    /// </summary>
    public object userData { get; set; }

    public override bool canGrabFocus
    {
      get
      {
        return this.enabledInHierarchy && base.canGrabFocus;
      }
    }

    public override FocusController focusController
    {
      get
      {
        return this.panel != null ? this.panel.focusController : (FocusController) null;
      }
    }

    internal RenderData renderData
    {
      get
      {
        return this.m_RenderData ?? (this.m_RenderData = new RenderData());
      }
    }

    public ITransform transform
    {
      get
      {
        return (ITransform) this;
      }
    }

    Vector3 ITransform.position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        if (this.m_Position == value)
          return;
        this.m_Position = value;
        this.Dirty(ChangeType.Transform);
      }
    }

    Quaternion ITransform.rotation
    {
      get
      {
        return this.m_Rotation;
      }
      set
      {
        if (this.m_Rotation == value)
          return;
        this.m_Rotation = value;
        this.Dirty(ChangeType.Transform);
      }
    }

    Vector3 ITransform.scale
    {
      get
      {
        return this.m_Scale;
      }
      set
      {
        if (this.m_Scale == value)
          return;
        this.m_Scale = value;
        this.Dirty(ChangeType.Transform);
      }
    }

    Matrix4x4 ITransform.matrix
    {
      get
      {
        return Matrix4x4.TRS(this.m_Position, this.m_Rotation, this.m_Scale);
      }
    }

    public Rect layout
    {
      get
      {
        Rect layout = this.m_Layout;
        if (this.cssNode != null && this.style.positionType.value != PositionType.Manual)
        {
          layout.x = this.cssNode.LayoutX;
          layout.y = this.cssNode.LayoutY;
          layout.width = this.cssNode.LayoutWidth;
          layout.height = this.cssNode.LayoutHeight;
        }
        return layout;
      }
      set
      {
        if (this.cssNode == null)
          this.cssNode = new CSSNode();
        if (this.style.positionType.value == PositionType.Manual && this.m_Layout == value)
          return;
        this.m_Layout = value;
        IStyle style = (IStyle) this;
        style.positionType = (StyleValue<PositionType>) PositionType.Manual;
        style.marginLeft = (StyleValue<float>) 0.0f;
        style.marginRight = (StyleValue<float>) 0.0f;
        style.marginBottom = (StyleValue<float>) 0.0f;
        style.marginTop = (StyleValue<float>) 0.0f;
        style.positionLeft = (StyleValue<float>) value.x;
        style.positionTop = (StyleValue<float>) value.y;
        style.positionRight = (StyleValue<float>) float.NaN;
        style.positionBottom = (StyleValue<float>) float.NaN;
        style.width = (StyleValue<float>) value.width;
        style.height = (StyleValue<float>) value.height;
        this.Dirty(ChangeType.Layout);
      }
    }

    public Rect contentRect
    {
      get
      {
        return this.paddingRect - new Spacing((float) this.m_Style.paddingLeft, (float) this.m_Style.paddingTop, (float) this.m_Style.paddingRight, (float) this.m_Style.paddingBottom);
      }
    }

    protected Rect paddingRect
    {
      get
      {
        return this.layout - new Spacing((float) this.style.borderLeftWidth, (float) this.style.borderTopWidth, (float) this.style.borderRightWidth, (float) this.style.borderBottomWidth);
      }
    }

    public Rect worldBound
    {
      get
      {
        Matrix4x4 worldTransform = this.worldTransform;
        Vector3 vector3_1 = worldTransform.MultiplyPoint3x4((Vector3) this.layout.min);
        Vector3 vector3_2 = worldTransform.MultiplyPoint3x4((Vector3) this.layout.max);
        return Rect.MinMaxRect(Math.Min(vector3_1.x, vector3_2.x), Math.Min(vector3_1.y, vector3_2.y), Math.Max(vector3_1.x, vector3_2.x), Math.Max(vector3_1.y, vector3_2.y));
      }
    }

    public Rect localBound
    {
      get
      {
        Matrix4x4 matrix = this.transform.matrix;
        Vector3 vector3_1 = matrix.MultiplyPoint3x4((Vector3) this.layout.min);
        Vector3 vector3_2 = matrix.MultiplyPoint3x4((Vector3) this.layout.max);
        return Rect.MinMaxRect(Math.Min(vector3_1.x, vector3_2.x), Math.Min(vector3_1.y, vector3_2.y), Math.Max(vector3_1.x, vector3_2.x), Math.Max(vector3_1.y, vector3_2.y));
      }
    }

    public Matrix4x4 worldTransform
    {
      get
      {
        if (this.IsDirty(ChangeType.Transform))
        {
          this.renderData.worldTransForm = this.shadow.parent == null ? this.transform.matrix : this.shadow.parent.worldTransform * Matrix4x4.Translate(new Vector3(this.shadow.parent.layout.x, this.shadow.parent.layout.y, 0.0f)) * this.transform.matrix;
          this.ClearDirty(ChangeType.Transform);
        }
        return this.renderData.worldTransForm;
      }
    }

    internal PseudoStates pseudoStates
    {
      get
      {
        return this.m_PseudoStates;
      }
      set
      {
        if (this.m_PseudoStates == value)
          return;
        this.m_PseudoStates = value;
        this.Dirty(ChangeType.Styles);
      }
    }

    public PickingMode pickingMode { get; set; }

    public string name
    {
      get
      {
        return this.m_Name;
      }
      set
      {
        if (this.m_Name == value)
          return;
        this.m_Name = value;
        this.Dirty(ChangeType.Styles);
      }
    }

    internal string fullTypeName
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_FullTypeName))
          this.m_FullTypeName = this.GetType().FullName;
        return this.m_FullTypeName;
      }
    }

    internal string typeName
    {
      get
      {
        if (string.IsNullOrEmpty(this.m_TypeName))
          this.m_TypeName = this.GetType().Name;
        return this.m_TypeName;
      }
    }

    internal CSSNode cssNode { get; private set; }

    /// <summary>
    ///   <para>Callback when the styles of an object have changed.</para>
    /// </summary>
    /// <param name="style"></param>
    public virtual void OnStyleResolved(ICustomStyle style)
    {
      this.FinalizeLayout();
    }

    internal VisualElementStylesData sharedStyle
    {
      get
      {
        return this.m_SharedStyle;
      }
    }

    internal VisualElementStylesData effectiveStyle
    {
      get
      {
        return this.m_Style;
      }
    }

    internal bool hasInlineStyle
    {
      get
      {
        return this.m_Style != this.m_SharedStyle;
      }
    }

    private VisualElementStylesData inlineStyle
    {
      get
      {
        if (!this.hasInlineStyle)
        {
          VisualElementStylesData elementStylesData = new VisualElementStylesData(false);
          elementStylesData.Apply(this.m_SharedStyle, StylePropertyApplyMode.Copy);
          this.m_Style = elementStylesData;
        }
        return this.m_Style;
      }
    }

    internal float opacity
    {
      get
      {
        return this.style.opacity.value;
      }
      set
      {
        this.style.opacity = (StyleValue<float>) value;
      }
    }

    protected internal override void ExecuteDefaultAction(EventBase evt)
    {
      base.ExecuteDefaultAction(evt);
      if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId())
        this.pseudoStates |= PseudoStates.Hover;
      else if (evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
        this.pseudoStates &= ~PseudoStates.Hover;
      else if (evt.GetEventTypeId() == EventBase<BlurEvent>.TypeId())
      {
        this.pseudoStates &= ~PseudoStates.Focus;
      }
      else
      {
        if (evt.GetEventTypeId() != EventBase<FocusEvent>.TypeId())
          return;
        this.pseudoStates |= PseudoStates.Focus;
      }
    }

    public override sealed void Focus()
    {
      if (!this.canGrabFocus && this.shadow.parent != null)
        this.shadow.parent.Focus();
      else
        base.Focus();
    }

    internal virtual void ChangePanel(BaseVisualElementPanel p)
    {
      if (this.panel == p)
        return;
      if (this.panel != null)
      {
        DetachFromPanelEvent pooled = EventBase<DetachFromPanelEvent>.GetPooled();
        pooled.target = (IEventHandler) this;
        UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, this.panel);
        EventBase<DetachFromPanelEvent>.ReleasePooled(pooled);
      }
      this.elementPanel = p;
      if (this.panel != null)
      {
        AttachToPanelEvent pooled = EventBase<AttachToPanelEvent>.GetPooled();
        pooled.target = (IEventHandler) this;
        UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, this.panel);
        EventBase<AttachToPanelEvent>.ReleasePooled(pooled);
      }
      this.Dirty(ChangeType.Styles);
      if (this.m_Children == null)
        return;
      foreach (VisualElement child in this.m_Children)
        child.ChangePanel(p);
    }

    private void PropagateToChildren(ChangeType type)
    {
      if ((type & this.changesNeeded) == type)
        return;
      this.changesNeeded |= type;
      type &= ChangeType.Styles | ChangeType.Transform;
      if (type == (ChangeType) 0 || this.m_Children == null)
        return;
      foreach (VisualElement child in this.m_Children)
        child.PropagateToChildren(type);
    }

    private void PropagateChangesToParents()
    {
      ChangeType changeType = (ChangeType) 0;
      if (this.changesNeeded != (ChangeType) 0)
      {
        changeType |= ChangeType.Repaint;
        if ((this.changesNeeded & ChangeType.Styles) > (ChangeType) 0)
          changeType |= ChangeType.StylesPath;
        if ((this.changesNeeded & (ChangeType.PersistentData | ChangeType.PersistentDataPath)) > (ChangeType) 0)
          changeType |= ChangeType.PersistentDataPath;
      }
      for (VisualElement parent = this.shadow.parent; parent != null && (parent.changesNeeded & changeType) != changeType; parent = parent.shadow.parent)
        parent.changesNeeded |= changeType;
    }

    public void Dirty(ChangeType type)
    {
      if ((type & this.changesNeeded) == type)
        return;
      if ((type & ChangeType.Layout) > (ChangeType) 0)
      {
        if (this.cssNode != null && this.cssNode.IsMeasureDefined)
          this.cssNode.MarkDirty();
        type |= ChangeType.Repaint;
      }
      this.PropagateToChildren(type);
      this.PropagateChangesToParents();
    }

    internal bool AnyDirty()
    {
      return this.changesNeeded != (ChangeType) 0;
    }

    public bool IsDirty(ChangeType type)
    {
      return (this.changesNeeded & type) == type;
    }

    /// <summary>
    ///   <para>Checks if any of the ChangeTypes have been marked dirty.</para>
    /// </summary>
    /// <param name="type">The ChangeType(s) to check.</param>
    /// <returns>
    ///   <para>True if at least one of the checked ChangeTypes have been marked dirty.</para>
    /// </returns>
    public bool AnyDirty(ChangeType type)
    {
      return (this.changesNeeded & type) > (ChangeType) 0;
    }

    public void ClearDirty(ChangeType type)
    {
      this.changesNeeded &= ~type;
    }

    public string text
    {
      get
      {
        return this.m_Text ?? string.Empty;
      }
      set
      {
        if (this.m_Text == value)
          return;
        this.m_Text = value;
        this.Dirty(ChangeType.Layout);
        if (string.IsNullOrEmpty(this.persistenceKey))
          return;
        this.SavePersistentData();
      }
    }

    [Obsolete("enabled is deprecated. Use SetEnabled as setter, and enabledSelf/enabledInHierarchy as getters.", true)]
    public virtual bool enabled
    {
      get
      {
        return this.enabledInHierarchy;
      }
      set
      {
        this.SetEnabled(value);
      }
    }

    protected internal bool SetEnabledFromHierarchy(bool state)
    {
      if (state == ((this.pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled))
        return false;
      if (state && this.m_Enabled && (this.parent == null || this.parent.enabledInHierarchy))
        this.pseudoStates &= ~PseudoStates.Disabled;
      else
        this.pseudoStates |= PseudoStates.Disabled;
      return true;
    }

    /// <summary>
    ///   <para>Returns true if the VisualElement is enabled in its own hierarchy.</para>
    /// </summary>
    public bool enabledInHierarchy
    {
      get
      {
        return (this.pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled;
      }
    }

    /// <summary>
    ///   <para>Returns true if the VisualElement is enabled locally.</para>
    /// </summary>
    public bool enabledSelf
    {
      get
      {
        return this.m_Enabled;
      }
    }

    /// <summary>
    ///   <para>Changes the VisualElement enabled state. A disabled VisualElement does not receive most events.</para>
    /// </summary>
    /// <param name="value">New enabled state</param>
    public virtual void SetEnabled(bool value)
    {
      if (this.m_Enabled == value)
        return;
      this.m_Enabled = value;
      this.PropagateEnabledToChildren(value);
    }

    private void PropagateEnabledToChildren(bool value)
    {
      if (!this.SetEnabledFromHierarchy(value))
        return;
      for (int index = 0; index < this.shadow.childCount; ++index)
        this.shadow[index].PropagateEnabledToChildren(value);
    }

    public bool visible
    {
      get
      {
        return (this.pseudoStates & PseudoStates.Invisible) != PseudoStates.Invisible;
      }
      set
      {
        if (value)
          this.pseudoStates &= ~PseudoStates.Invisible;
        else
          this.pseudoStates |= PseudoStates.Invisible;
      }
    }

    public virtual void DoRepaint()
    {
      IStylePainter stylePainter = this.elementPanel.stylePainter;
      stylePainter.DrawBackground(this);
      stylePainter.DrawBorder(this);
      stylePainter.DrawText(this);
    }

    internal virtual void DoRepaint(IStylePainter painter)
    {
      if ((this.pseudoStates & PseudoStates.Invisible) == PseudoStates.Invisible)
        return;
      this.DoRepaint();
    }

    private void GetFullHierarchicalPersistenceKey(StringBuilder key)
    {
      if (this.parent != null)
        this.parent.GetFullHierarchicalPersistenceKey(key);
      if (string.IsNullOrEmpty(this.persistenceKey))
        return;
      key.Append("__");
      key.Append(this.persistenceKey);
    }

    /// <summary>
    ///   <para>Combine this VisualElement's VisualElement.persistenceKey with those of its parents to create a more unique key for use with VisualElement.GetOrCreatePersistentData.</para>
    /// </summary>
    /// <returns>
    ///   <para>Full hierarchical persistence key.</para>
    /// </returns>
    public string GetFullHierarchicalPersistenceKey()
    {
      StringBuilder key = new StringBuilder();
      this.GetFullHierarchicalPersistenceKey(key);
      return key.ToString();
    }

    public T GetOrCreatePersistentData<T>(object existing, string key) where T : class, new()
    {
      Debug.Assert(this.elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
      ISerializableJsonDictionary serializableJsonDictionary = this.elementPanel == null || this.elementPanel.getViewDataDictionary == null ? (ISerializableJsonDictionary) null : this.elementPanel.getViewDataDictionary();
      if (serializableJsonDictionary == null || string.IsNullOrEmpty(this.persistenceKey) || !this.enablePersistence)
      {
        if (existing != null)
          return existing as T;
        return Activator.CreateInstance<T>();
      }
      string key1 = key + "__" + typeof (T).ToString();
      if (!serializableJsonDictionary.ContainsKey(key1))
        serializableJsonDictionary.Set<T>(key1, Activator.CreateInstance<T>());
      return serializableJsonDictionary.Get<T>(key1);
    }

    public T GetOrCreatePersistentData<T>(ScriptableObject existing, string key) where T : ScriptableObject
    {
      Debug.Assert(this.elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
      ISerializableJsonDictionary serializableJsonDictionary = this.elementPanel == null || this.elementPanel.getViewDataDictionary == null ? (ISerializableJsonDictionary) null : this.elementPanel.getViewDataDictionary();
      if (serializableJsonDictionary == null || string.IsNullOrEmpty(this.persistenceKey) || !this.enablePersistence)
      {
        if ((UnityEngine.Object) existing != (UnityEngine.Object) null)
          return existing as T;
        return ScriptableObject.CreateInstance<T>();
      }
      string key1 = key + "__" + typeof (T).ToString();
      if (!serializableJsonDictionary.ContainsKey(key1))
        serializableJsonDictionary.Set<T>(key1, ScriptableObject.CreateInstance<T>());
      return serializableJsonDictionary.GetScriptable<T>(key1);
    }

    /// <summary>
    ///   <para>Overwrite object from the persistent data store.</para>
    /// </summary>
    /// <param name="key">The key for the current VisualElement to be used with the persistence store on the EditorWindow.</param>
    /// <param name="obj">Object to overwrite.</param>
    public void OverwriteFromPersistedData(object obj, string key)
    {
      Debug.Assert(this.elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
      ISerializableJsonDictionary serializableJsonDictionary = this.elementPanel == null || this.elementPanel.getViewDataDictionary == null ? (ISerializableJsonDictionary) null : this.elementPanel.getViewDataDictionary();
      if (serializableJsonDictionary == null || string.IsNullOrEmpty(this.persistenceKey) || !this.enablePersistence)
        return;
      string key1 = key + "__" + (object) obj.GetType();
      if (!serializableJsonDictionary.ContainsKey(key1))
        serializableJsonDictionary.Set<object>(key1, obj);
      else
        serializableJsonDictionary.Overwrite(obj, key1);
    }

    /// <summary>
    ///   <para>Write persistence data to file.</para>
    /// </summary>
    public void SavePersistentData()
    {
      if (this.elementPanel == null || this.elementPanel.savePersistentViewData == null || string.IsNullOrEmpty(this.persistenceKey))
        return;
      this.elementPanel.savePersistentViewData();
    }

    internal bool IsPersitenceSupportedOnChildren()
    {
      return this.GetType() == typeof (VisualElement) || !string.IsNullOrEmpty(this.persistenceKey);
    }

    internal void OnPersistentDataReady(bool enablePersistence)
    {
      this.enablePersistence = enablePersistence;
      this.OnPersistentDataReady();
    }

    /// <summary>
    ///   <para>Called when the persistent data is accessible and/or when the data or persistence key have changed (VisualElement is properly parented).</para>
    /// </summary>
    public virtual void OnPersistentDataReady()
    {
    }

    public virtual bool ContainsPoint(Vector2 localPoint)
    {
      return this.layout.Contains(localPoint);
    }

    public virtual bool Overlaps(Rect rectangle)
    {
      return this.layout.Overlaps(rectangle, true);
    }

    protected internal virtual Vector2 DoMeasure(float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
    {
      IStylePainter stylePainter = this.elementPanel.stylePainter;
      float x = float.NaN;
      float y = float.NaN;
      Font font = (Font) this.style.font;
      if (this.m_Text == null || (UnityEngine.Object) font == (UnityEngine.Object) null)
        return new Vector2(x, y);
      float num1;
      if (widthMode == VisualElement.MeasureMode.Exactly)
      {
        num1 = width;
      }
      else
      {
        TextStylePainterParameters defaultTextParameters = stylePainter.GetDefaultTextParameters(this);
        defaultTextParameters.text = this.text;
        defaultTextParameters.font = font;
        defaultTextParameters.wordWrapWidth = 0.0f;
        defaultTextParameters.wordWrap = false;
        defaultTextParameters.richText = true;
        num1 = stylePainter.ComputeTextWidth(defaultTextParameters);
        if (widthMode == VisualElement.MeasureMode.AtMost)
          num1 = Mathf.Min(num1, width);
      }
      float num2;
      if (heightMode == VisualElement.MeasureMode.Exactly)
      {
        num2 = height;
      }
      else
      {
        TextStylePainterParameters defaultTextParameters = stylePainter.GetDefaultTextParameters(this);
        defaultTextParameters.text = this.text;
        defaultTextParameters.font = font;
        defaultTextParameters.wordWrapWidth = num1;
        defaultTextParameters.richText = true;
        num2 = stylePainter.ComputeTextHeight(defaultTextParameters);
        if (heightMode == VisualElement.MeasureMode.AtMost)
          num2 = Mathf.Min(num2, height);
      }
      return new Vector2(num1, num2);
    }

    internal long Measure(CSSNode node, float width, CSSMeasureMode widthMode, float height, CSSMeasureMode heightMode)
    {
      Debug.Assert(node == this.cssNode, "CSSNode instance mismatch");
      Vector2 vector2 = this.DoMeasure(width, (VisualElement.MeasureMode) widthMode, height, (VisualElement.MeasureMode) heightMode);
      return MeasureOutput.Make(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }

    public void SetSize(Vector2 size)
    {
      Rect layout = this.layout;
      layout.width = size.x;
      layout.height = size.y;
      this.layout = layout;
    }

    private void FinalizeLayout()
    {
      this.cssNode.Flex = this.style.flex.GetSpecifiedValueOrDefault(float.NaN);
      this.cssNode.SetPosition(CSSEdge.Left, this.style.positionLeft.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPosition(CSSEdge.Top, this.style.positionTop.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPosition(CSSEdge.Right, this.style.positionRight.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPosition(CSSEdge.Bottom, this.style.positionBottom.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetMargin(CSSEdge.Left, this.style.marginLeft.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetMargin(CSSEdge.Top, this.style.marginTop.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetMargin(CSSEdge.Right, this.style.marginRight.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetMargin(CSSEdge.Bottom, this.style.marginBottom.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPadding(CSSEdge.Left, this.style.paddingLeft.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPadding(CSSEdge.Top, this.style.paddingTop.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPadding(CSSEdge.Right, this.style.paddingRight.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetPadding(CSSEdge.Bottom, this.style.paddingBottom.GetSpecifiedValueOrDefault(float.NaN));
      this.cssNode.SetBorder(CSSEdge.Left, this.style.borderLeft.GetSpecifiedValueOrDefault(this.style.borderLeftWidth.GetSpecifiedValueOrDefault(float.NaN)));
      this.cssNode.SetBorder(CSSEdge.Top, this.style.borderTop.GetSpecifiedValueOrDefault(this.style.borderTopWidth.GetSpecifiedValueOrDefault(float.NaN)));
      this.cssNode.SetBorder(CSSEdge.Right, this.style.borderRight.GetSpecifiedValueOrDefault(this.style.borderRightWidth.GetSpecifiedValueOrDefault(float.NaN)));
      this.cssNode.SetBorder(CSSEdge.Bottom, this.style.borderBottom.GetSpecifiedValueOrDefault(this.style.borderBottomWidth.GetSpecifiedValueOrDefault(float.NaN)));
      this.cssNode.Width = this.style.width.GetSpecifiedValueOrDefault(float.NaN);
      this.cssNode.Height = this.style.height.GetSpecifiedValueOrDefault(float.NaN);
      switch ((PositionType) this.style.positionType)
      {
        case PositionType.Relative:
          this.cssNode.PositionType = CSSPositionType.Relative;
          break;
        case PositionType.Absolute:
        case PositionType.Manual:
          this.cssNode.PositionType = CSSPositionType.Absolute;
          break;
      }
      this.cssNode.Overflow = (CSSOverflow) this.style.overflow.value;
      this.cssNode.AlignSelf = (CSSAlign) this.style.alignSelf.value;
      this.cssNode.MaxWidth = this.style.maxWidth.GetSpecifiedValueOrDefault(float.NaN);
      this.cssNode.MaxHeight = this.style.maxHeight.GetSpecifiedValueOrDefault(float.NaN);
      this.cssNode.MinWidth = this.style.minWidth.GetSpecifiedValueOrDefault(float.NaN);
      this.cssNode.MinHeight = this.style.minHeight.GetSpecifiedValueOrDefault(float.NaN);
      this.cssNode.FlexDirection = (CSSFlexDirection) this.style.flexDirection.value;
      this.cssNode.AlignContent = (CSSAlign) this.style.alignContent.GetSpecifiedValueOrDefault(Align.FlexStart);
      this.cssNode.AlignItems = (CSSAlign) this.style.alignItems.GetSpecifiedValueOrDefault(Align.Stretch);
      this.cssNode.JustifyContent = (CSSJustify) this.style.justifyContent.value;
      this.cssNode.Wrap = (CSSWrap) this.style.flexWrap.value;
      this.Dirty(ChangeType.Layout);
    }

    internal event OnStylesResolved onStylesResolved;

    internal void SetInlineStyles(VisualElementStylesData inlineStyle)
    {
      Debug.Assert(!inlineStyle.isShared);
      inlineStyle.Apply(this.m_Style, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
      this.m_Style = inlineStyle;
    }

    internal void SetSharedStyles(VisualElementStylesData sharedStyle)
    {
      Debug.Assert(sharedStyle.isShared);
      this.ClearDirty(ChangeType.Styles | ChangeType.StylesPath);
      if (sharedStyle == this.m_SharedStyle)
        return;
      if (this.hasInlineStyle)
        this.m_Style.Apply(sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
      else
        this.m_Style = sharedStyle;
      this.m_SharedStyle = sharedStyle;
      // ISSUE: reference to a compiler-generated field
      if (this.onStylesResolved != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.onStylesResolved((ICustomStyle) this.m_Style);
      }
      this.OnStyleResolved((ICustomStyle) this.m_Style);
      this.Dirty(ChangeType.Repaint);
    }

    public void ResetPositionProperties()
    {
      if (!this.hasInlineStyle)
        return;
      VisualElementStylesData inlineStyle = this.inlineStyle;
      inlineStyle.positionType = StyleValue<int>.nil;
      inlineStyle.marginLeft = StyleValue<float>.nil;
      inlineStyle.marginRight = StyleValue<float>.nil;
      inlineStyle.marginBottom = StyleValue<float>.nil;
      inlineStyle.marginTop = StyleValue<float>.nil;
      inlineStyle.positionLeft = StyleValue<float>.nil;
      inlineStyle.positionTop = StyleValue<float>.nil;
      inlineStyle.positionRight = StyleValue<float>.nil;
      inlineStyle.positionBottom = StyleValue<float>.nil;
      inlineStyle.width = StyleValue<float>.nil;
      inlineStyle.height = StyleValue<float>.nil;
      this.m_Style.Apply(this.sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
      this.FinalizeLayout();
      this.Dirty(ChangeType.Layout);
    }

    public override string ToString()
    {
      return this.name + " " + (object) this.layout + " world rect: " + (object) this.worldBound;
    }

    internal IEnumerable<string> GetClasses()
    {
      return (IEnumerable<string>) this.m_ClassList;
    }

    public void ClearClassList()
    {
      if (this.m_ClassList == null || this.m_ClassList.Count <= 0)
        return;
      this.m_ClassList.Clear();
      this.Dirty(ChangeType.Styles);
    }

    public void AddToClassList(string className)
    {
      if (this.m_ClassList == null)
        this.m_ClassList = new HashSet<string>();
      if (!this.m_ClassList.Add(className))
        return;
      this.Dirty(ChangeType.Styles);
    }

    public void RemoveFromClassList(string className)
    {
      if (this.m_ClassList == null || !this.m_ClassList.Remove(className))
        return;
      this.Dirty(ChangeType.Styles);
    }

    public bool ClassListContains(string cls)
    {
      return this.m_ClassList != null && this.m_ClassList.Contains(cls);
    }

    /// <summary>
    ///   <para>Searchs up the hierachy of this VisualElement and retrieves stored userData, if any is found.</para>
    /// </summary>
    public object FindAncestorUserData()
    {
      for (VisualElement parent = this.parent; parent != null; parent = parent.parent)
      {
        if (parent.userData != null)
          return parent.userData;
      }
      return (object) null;
    }

    /// <summary>
    ///   <para>Access to this element data watch interface.</para>
    /// </summary>
    public IUIElementDataWatch dataWatch
    {
      get
      {
        return (IUIElementDataWatch) this;
      }
    }

    IUIElementDataWatchRequest IUIElementDataWatch.RegisterWatch(UnityEngine.Object toWatch, Action<UnityEngine.Object> watchNotification)
    {
      VisualElement.DataWatchRequest dataWatchRequest = new VisualElement.DataWatchRequest(this) { notification = watchNotification, watchedObject = toWatch };
      dataWatchRequest.Start();
      return (IUIElementDataWatchRequest) dataWatchRequest;
    }

    void IUIElementDataWatch.UnregisterWatch(IUIElementDataWatchRequest requested)
    {
      VisualElement.DataWatchRequest dataWatchRequest = requested as VisualElement.DataWatchRequest;
      if (dataWatchRequest == null)
        return;
      dataWatchRequest.Stop();
    }

    /// <summary>
    ///   <para> Access to this element physical hierarchy
    ///           </para>
    /// </summary>
    public VisualElement.Hierarchy shadow { get; private set; }

    /// <summary>
    ///   <para>Should this element clip painting to its boundaries.</para>
    /// </summary>
    public VisualElement.ClippingOptions clippingOptions
    {
      get
      {
        return this.m_ClippingOptions;
      }
      set
      {
        if (this.m_ClippingOptions == value)
          return;
        this.m_ClippingOptions = value;
        this.Dirty(ChangeType.Repaint);
      }
    }

    public VisualElement parent
    {
      get
      {
        return this.m_LogicalParent;
      }
    }

    internal BaseVisualElementPanel elementPanel { get; private set; }

    public IPanel panel
    {
      get
      {
        return (IPanel) this.elementPanel;
      }
    }

    /// <summary>
    ///   <para> child elements are added to this element, usually this
    ///           </para>
    /// </summary>
    public virtual VisualElement contentContainer
    {
      get
      {
        return this;
      }
    }

    /// <summary>
    ///   <para>Add an element to this element's contentContainer</para>
    /// </summary>
    /// <param name="child"></param>
    public void Add(VisualElement child)
    {
      if (this.contentContainer == this)
        this.shadow.Add(child);
      else
        this.contentContainer.Add(child);
      child.m_LogicalParent = this;
    }

    public void Insert(int index, VisualElement element)
    {
      if (this.contentContainer == this)
        this.shadow.Insert(index, element);
      else
        this.contentContainer.Insert(index, element);
      element.m_LogicalParent = this;
    }

    /// <summary>
    ///   <para>Removes this child from the hierarchy</para>
    /// </summary>
    /// <param name="element"></param>
    public void Remove(VisualElement element)
    {
      if (this.contentContainer == this)
        this.shadow.Remove(element);
      else
        this.contentContainer.Remove(element);
    }

    /// <summary>
    ///   <para>Remove the child element located at this position from this element's contentContainer</para>
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt(int index)
    {
      if (this.contentContainer == this)
        this.shadow.RemoveAt(index);
      else
        this.contentContainer.RemoveAt(index);
    }

    /// <summary>
    ///   <para>Remove all child elements from this element's contentContainer</para>
    /// </summary>
    public void Clear()
    {
      if (this.contentContainer == this)
        this.shadow.Clear();
      else
        this.contentContainer.Clear();
    }

    /// <summary>
    ///   <para>Retrieves the child element at position</para>
    /// </summary>
    /// <param name="index"></param>
    public VisualElement ElementAt(int index)
    {
      if (this.contentContainer == this)
        return this.shadow.ElementAt(index);
      return this.contentContainer.ElementAt(index);
    }

    public VisualElement this[int key]
    {
      get
      {
        return this.ElementAt(key);
      }
    }

    /// <summary>
    ///   <para> Number of child elements in this object's contentContainer
    ///           </para>
    /// </summary>
    public int childCount
    {
      get
      {
        if (this.contentContainer == this)
          return this.shadow.childCount;
        return this.contentContainer.childCount;
      }
    }

    /// <summary>
    ///   <para>Returns the elements from its contentContainer</para>
    /// </summary>
    public IEnumerable<VisualElement> Children()
    {
      if (this.contentContainer == this)
        return this.shadow.Children();
      return this.contentContainer.Children();
    }

    public void Sort(Comparison<VisualElement> comp)
    {
      if (this.contentContainer == this)
        this.shadow.Sort(comp);
      else
        this.contentContainer.Sort(comp);
    }

    /// <summary>
    ///   <para>Removes this element from its parent hierarchy</para>
    /// </summary>
    public void RemoveFromHierarchy()
    {
      if (this.shadow.parent == null)
        return;
      this.shadow.parent.shadow.Remove(this);
    }

    public T GetFirstOfType<T>() where T : class
    {
      T obj = (object) this as T;
      if ((object) obj != null)
        return obj;
      return this.GetFirstAncestorOfType<T>();
    }

    public T GetFirstAncestorOfType<T>() where T : class
    {
      for (VisualElement parent = this.shadow.parent; parent != null; parent = parent.shadow.parent)
      {
        T obj = (object) parent as T;
        if ((object) obj != null)
          return obj;
      }
      return (T) null;
    }

    /// <summary>
    ///   <para>Returns true if the element is a direct child of this VisualElement</para>
    /// </summary>
    /// <param name="child"></param>
    public bool Contains(VisualElement child)
    {
      for (; child != null; child = child.shadow.parent)
      {
        if (child.shadow.parent == this)
          return true;
      }
      return false;
    }

    /// <summary>
    ///   <para>Allows to iterate into this elements children</para>
    /// </summary>
    public IEnumerator<VisualElement> GetEnumerator()
    {
      if (this.contentContainer == this)
        return this.shadow.Children().GetEnumerator();
      return this.contentContainer.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      if (this.contentContainer == this)
        return this.shadow.Children().GetEnumerator();
      return ((IEnumerable) this.contentContainer).GetEnumerator();
    }

    /// <summary>
    ///   <para>Retrieves this VisualElement's IVisualElementScheduler</para>
    /// </summary>
    public IVisualElementScheduler schedule
    {
      get
      {
        return (IVisualElementScheduler) this;
      }
    }

    IVisualElementScheduledItem IVisualElementScheduler.Execute(Action<TimerState> timerUpdateEvent)
    {
      VisualElement.TimerStateScheduledItem stateScheduledItem1 = new VisualElement.TimerStateScheduledItem(this, timerUpdateEvent);
      stateScheduledItem1.timerUpdateStopCondition = ScheduledItem.OnceCondition;
      VisualElement.TimerStateScheduledItem stateScheduledItem2 = stateScheduledItem1;
      stateScheduledItem2.Resume();
      return (IVisualElementScheduledItem) stateScheduledItem2;
    }

    IVisualElementScheduledItem IVisualElementScheduler.Execute(Action updateEvent)
    {
      VisualElement.SimpleScheduledItem simpleScheduledItem1 = new VisualElement.SimpleScheduledItem(this, updateEvent);
      simpleScheduledItem1.timerUpdateStopCondition = ScheduledItem.OnceCondition;
      VisualElement.SimpleScheduledItem simpleScheduledItem2 = simpleScheduledItem1;
      simpleScheduledItem2.Resume();
      return (IVisualElementScheduledItem) simpleScheduledItem2;
    }

    /// <summary>
    ///   <para>Reference to the style object of this element.</para>
    /// </summary>
    public IStyle style
    {
      get
      {
        return (IStyle) this;
      }
    }

    private static bool ApplyAndCompare(ref StyleValue<float> current, StyleValue<float> other)
    {
      float num = current.value;
      if (current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity))
        return (double) num != (double) other.value;
      return false;
    }

    private static bool ApplyAndCompare(ref StyleValue<int> current, StyleValue<int> other)
    {
      int num = current.value;
      if (current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity))
        return num != other.value;
      return false;
    }

    private static bool ApplyAndCompare(ref StyleValue<bool> current, StyleValue<bool> other)
    {
      bool flag = current.value;
      if (current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity))
        return flag != other.value;
      return false;
    }

    private static bool ApplyAndCompare(ref StyleValue<Color> current, StyleValue<Color> other)
    {
      Color color = current.value;
      if (current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity))
        return color != other.value;
      return false;
    }

    private static bool ApplyAndCompare<T>(ref StyleValue<T> current, StyleValue<T> other) where T : UnityEngine.Object
    {
      T obj = current.value;
      if (current.Apply(other, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity))
        return (UnityEngine.Object) obj != (UnityEngine.Object) other.value;
      return false;
    }

    StyleValue<float> IStyle.width
    {
      get
      {
        return this.effectiveStyle.width;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.width, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.Width = value.value;
      }
    }

    StyleValue<float> IStyle.height
    {
      get
      {
        return this.effectiveStyle.height;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.height, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.Height = value.value;
      }
    }

    StyleValue<float> IStyle.maxWidth
    {
      get
      {
        return this.effectiveStyle.maxWidth;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.maxWidth, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.MaxWidth = value.value;
      }
    }

    StyleValue<float> IStyle.maxHeight
    {
      get
      {
        return this.effectiveStyle.maxHeight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.maxHeight, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.MaxHeight = value.value;
      }
    }

    StyleValue<float> IStyle.minWidth
    {
      get
      {
        return this.effectiveStyle.minWidth;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.minWidth, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.MinWidth = value.value;
      }
    }

    StyleValue<float> IStyle.minHeight
    {
      get
      {
        return this.effectiveStyle.minHeight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.minHeight, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.MinHeight = value.value;
      }
    }

    StyleValue<float> IStyle.flex
    {
      get
      {
        return this.effectiveStyle.flex;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.flex, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.Flex = value.value;
      }
    }

    StyleValue<Overflow> IStyle.overflow
    {
      get
      {
        return new StyleValue<Overflow>((Overflow) this.effectiveStyle.overflow.value, this.effectiveStyle.overflow.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.overflow, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.Overflow = (CSSOverflow) value.value;
      }
    }

    StyleValue<float> IStyle.positionLeft
    {
      get
      {
        return this.effectiveStyle.positionLeft;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.positionLeft, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPosition(CSSEdge.Left, value.value);
      }
    }

    StyleValue<float> IStyle.positionTop
    {
      get
      {
        return this.effectiveStyle.positionTop;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.positionTop, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPosition(CSSEdge.Top, value.value);
      }
    }

    StyleValue<float> IStyle.positionRight
    {
      get
      {
        return this.effectiveStyle.positionRight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.positionRight, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPosition(CSSEdge.Right, value.value);
      }
    }

    StyleValue<float> IStyle.positionBottom
    {
      get
      {
        return this.effectiveStyle.positionBottom;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.positionBottom, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPosition(CSSEdge.Bottom, value.value);
      }
    }

    StyleValue<float> IStyle.marginLeft
    {
      get
      {
        return this.effectiveStyle.marginLeft;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.marginLeft, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetMargin(CSSEdge.Left, value.value);
      }
    }

    StyleValue<float> IStyle.marginTop
    {
      get
      {
        return this.effectiveStyle.marginTop;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.marginTop, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetMargin(CSSEdge.Top, value.value);
      }
    }

    StyleValue<float> IStyle.marginRight
    {
      get
      {
        return this.effectiveStyle.marginRight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.marginRight, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetMargin(CSSEdge.Right, value.value);
      }
    }

    StyleValue<float> IStyle.marginBottom
    {
      get
      {
        return this.effectiveStyle.marginBottom;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.marginBottom, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetMargin(CSSEdge.Bottom, value.value);
      }
    }

    StyleValue<float> IStyle.borderLeft
    {
      get
      {
        return this.effectiveStyle.borderLeft;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderLeft, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Left, value.value);
      }
    }

    StyleValue<float> IStyle.borderTop
    {
      get
      {
        return this.effectiveStyle.borderTop;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTop, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Top, value.value);
      }
    }

    StyleValue<float> IStyle.borderRight
    {
      get
      {
        return this.effectiveStyle.borderRight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderRight, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Right, value.value);
      }
    }

    StyleValue<float> IStyle.borderBottom
    {
      get
      {
        return this.effectiveStyle.borderBottom;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottom, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Bottom, value.value);
      }
    }

    StyleValue<float> IStyle.borderLeftWidth
    {
      get
      {
        return this.effectiveStyle.borderLeftWidth;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderLeftWidth, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Left, value.value);
      }
    }

    StyleValue<float> IStyle.borderTopWidth
    {
      get
      {
        return this.effectiveStyle.borderTopWidth;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTopWidth, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Top, value.value);
      }
    }

    StyleValue<float> IStyle.borderRightWidth
    {
      get
      {
        return this.effectiveStyle.borderRightWidth;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderRightWidth, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Right, value.value);
      }
    }

    StyleValue<float> IStyle.borderBottomWidth
    {
      get
      {
        return this.effectiveStyle.borderBottomWidth;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottomWidth, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetBorder(CSSEdge.Bottom, value.value);
      }
    }

    StyleValue<float> IStyle.borderRadius
    {
      get
      {
        return this.style.borderTopLeftRadius;
      }
      set
      {
        this.style.borderTopLeftRadius = value;
        this.style.borderTopRightRadius = value;
        this.style.borderBottomLeftRadius = value;
        this.style.borderBottomRightRadius = value;
      }
    }

    StyleValue<float> IStyle.borderTopLeftRadius
    {
      get
      {
        return this.effectiveStyle.borderTopLeftRadius;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTopLeftRadius, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<float> IStyle.borderTopRightRadius
    {
      get
      {
        return this.effectiveStyle.borderTopRightRadius;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderTopRightRadius, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<float> IStyle.borderBottomRightRadius
    {
      get
      {
        return this.effectiveStyle.borderBottomRightRadius;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottomRightRadius, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<float> IStyle.borderBottomLeftRadius
    {
      get
      {
        return this.effectiveStyle.borderBottomLeftRadius;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderBottomLeftRadius, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<float> IStyle.paddingLeft
    {
      get
      {
        return this.effectiveStyle.paddingLeft;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingLeft, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPadding(CSSEdge.Left, value.value);
      }
    }

    StyleValue<float> IStyle.paddingTop
    {
      get
      {
        return this.effectiveStyle.paddingTop;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingTop, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPadding(CSSEdge.Top, value.value);
      }
    }

    StyleValue<float> IStyle.paddingRight
    {
      get
      {
        return this.effectiveStyle.paddingRight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingRight, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPadding(CSSEdge.Right, value.value);
      }
    }

    StyleValue<float> IStyle.paddingBottom
    {
      get
      {
        return this.effectiveStyle.paddingBottom;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.paddingBottom, value))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.SetPadding(CSSEdge.Bottom, value.value);
      }
    }

    StyleValue<PositionType> IStyle.positionType
    {
      get
      {
        return new StyleValue<PositionType>((PositionType) this.effectiveStyle.positionType.value, this.effectiveStyle.positionType.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.positionType, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        switch (value.value)
        {
          case PositionType.Relative:
            this.cssNode.PositionType = CSSPositionType.Relative;
            break;
          case PositionType.Absolute:
          case PositionType.Manual:
            this.cssNode.PositionType = CSSPositionType.Absolute;
            break;
        }
      }
    }

    StyleValue<Align> IStyle.alignSelf
    {
      get
      {
        return new StyleValue<Align>((Align) this.effectiveStyle.alignSelf.value, this.effectiveStyle.alignSelf.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.alignSelf, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.AlignSelf = (CSSAlign) value.value;
      }
    }

    StyleValue<TextAnchor> IStyle.textAlignment
    {
      get
      {
        return new StyleValue<TextAnchor>((TextAnchor) this.effectiveStyle.textAlignment.value, this.effectiveStyle.textAlignment.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.textAlignment, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<FontStyle> IStyle.fontStyle
    {
      get
      {
        return new StyleValue<FontStyle>((FontStyle) this.effectiveStyle.fontStyle.value, this.effectiveStyle.fontStyle.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.fontStyle, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
      }
    }

    StyleValue<TextClipping> IStyle.textClipping
    {
      get
      {
        return new StyleValue<TextClipping>((TextClipping) this.effectiveStyle.textClipping.value, this.effectiveStyle.textClipping.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.textClipping, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<Font> IStyle.font
    {
      get
      {
        return this.effectiveStyle.font;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare<Font>(ref this.inlineStyle.font, value))
          return;
        this.Dirty(ChangeType.Layout);
      }
    }

    StyleValue<int> IStyle.fontSize
    {
      get
      {
        return this.effectiveStyle.fontSize;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.fontSize, value))
          return;
        this.Dirty(ChangeType.Layout);
      }
    }

    StyleValue<bool> IStyle.wordWrap
    {
      get
      {
        return this.effectiveStyle.wordWrap;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.wordWrap, value))
          return;
        this.Dirty(ChangeType.Layout);
      }
    }

    StyleValue<Color> IStyle.textColor
    {
      get
      {
        return this.effectiveStyle.textColor;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.textColor, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<FlexDirection> IStyle.flexDirection
    {
      get
      {
        return new StyleValue<FlexDirection>((FlexDirection) this.effectiveStyle.flexDirection.value, this.effectiveStyle.flexDirection.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.flexDirection, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Repaint);
        this.cssNode.FlexDirection = (CSSFlexDirection) value.value;
      }
    }

    StyleValue<Color> IStyle.backgroundColor
    {
      get
      {
        return this.effectiveStyle.backgroundColor;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.backgroundColor, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<Color> IStyle.borderColor
    {
      get
      {
        return this.effectiveStyle.borderColor;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.borderColor, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<Texture2D> IStyle.backgroundImage
    {
      get
      {
        return this.effectiveStyle.backgroundImage;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare<Texture2D>(ref this.inlineStyle.backgroundImage, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<ScaleMode> IStyle.backgroundSize
    {
      get
      {
        return new StyleValue<ScaleMode>((ScaleMode) this.effectiveStyle.backgroundSize.value, this.effectiveStyle.backgroundSize.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.backgroundSize, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<Align> IStyle.alignItems
    {
      get
      {
        return new StyleValue<Align>((Align) this.effectiveStyle.alignItems.value, this.effectiveStyle.alignItems.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.alignItems, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.AlignItems = (CSSAlign) value.value;
      }
    }

    StyleValue<Align> IStyle.alignContent
    {
      get
      {
        return new StyleValue<Align>((Align) this.effectiveStyle.alignContent.value, this.effectiveStyle.alignContent.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.alignContent, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.AlignContent = (CSSAlign) value.value;
      }
    }

    StyleValue<Justify> IStyle.justifyContent
    {
      get
      {
        return new StyleValue<Justify>((Justify) this.effectiveStyle.justifyContent.value, this.effectiveStyle.justifyContent.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.justifyContent, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.JustifyContent = (CSSJustify) value.value;
      }
    }

    StyleValue<Wrap> IStyle.flexWrap
    {
      get
      {
        return new StyleValue<Wrap>((Wrap) this.effectiveStyle.flexWrap.value, this.effectiveStyle.flexWrap.specificity);
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.flexWrap, new StyleValue<int>((int) value.value, value.specificity)))
          return;
        this.Dirty(ChangeType.Layout);
        this.cssNode.Wrap = (CSSWrap) value.value;
      }
    }

    StyleValue<int> IStyle.sliceLeft
    {
      get
      {
        return this.effectiveStyle.sliceLeft;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceLeft, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<int> IStyle.sliceTop
    {
      get
      {
        return this.effectiveStyle.sliceTop;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceTop, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<int> IStyle.sliceRight
    {
      get
      {
        return this.effectiveStyle.sliceRight;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceRight, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<int> IStyle.sliceBottom
    {
      get
      {
        return this.effectiveStyle.sliceBottom;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.sliceBottom, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    StyleValue<float> IStyle.opacity
    {
      get
      {
        return this.effectiveStyle.opacity;
      }
      set
      {
        if (!VisualElement.ApplyAndCompare(ref this.inlineStyle.opacity, value))
          return;
        this.Dirty(ChangeType.Repaint);
      }
    }

    internal IEnumerable<StyleSheet> styleSheets
    {
      get
      {
        if (this.m_StyleSheets == null && this.m_StyleSheetPaths != null)
          this.LoadStyleSheetsFromPaths();
        return (IEnumerable<StyleSheet>) this.m_StyleSheets;
      }
    }

    /// <summary>
    ///   <para>Adds this stylesheet file to this element list of applied styles</para>
    /// </summary>
    /// <param name="sheetPath"></param>
    public void AddStyleSheetPath(string sheetPath)
    {
      if (this.m_StyleSheetPaths == null)
        this.m_StyleSheetPaths = new List<string>();
      this.m_StyleSheetPaths.Add(sheetPath);
      this.m_StyleSheets = (List<StyleSheet>) null;
      this.Dirty(ChangeType.Styles);
    }

    /// <summary>
    ///   <para>Removes this stylesheet file from this element list of applied styles</para>
    /// </summary>
    /// <param name="sheetPath"></param>
    public void RemoveStyleSheetPath(string sheetPath)
    {
      if (this.m_StyleSheetPaths == null)
      {
        Debug.LogWarning((object) "Attempting to remove from null style sheet path list");
      }
      else
      {
        this.m_StyleSheetPaths.Remove(sheetPath);
        this.m_StyleSheets = (List<StyleSheet>) null;
        this.Dirty(ChangeType.Styles);
      }
    }

    public bool HasStyleSheetPath(string sheetPath)
    {
      return this.m_StyleSheetPaths != null && this.m_StyleSheetPaths.Contains(sheetPath);
    }

    internal void LoadStyleSheetsFromPaths()
    {
      if (this.m_StyleSheetPaths == null || this.elementPanel == null)
        return;
      this.m_StyleSheets = new List<StyleSheet>();
      foreach (string styleSheetPath in this.m_StyleSheetPaths)
      {
        StyleSheet styleSheet = Panel.loadResourceFunc(styleSheetPath, typeof (StyleSheet)) as StyleSheet;
        if ((UnityEngine.Object) styleSheet != (UnityEngine.Object) null)
        {
          int index = 0;
          for (int length = styleSheet.complexSelectors.Length; index < length; ++index)
            styleSheet.complexSelectors[index].CachePseudoStateMasks();
          this.m_StyleSheets.Add(styleSheet);
        }
        else
          Debug.LogWarning((object) string.Format("Style sheet not found for path \"{0}\"", (object) styleSheetPath));
      }
    }

    /// <summary>
    ///   <para>The modes available to measure VisualElement sizes.</para>
    /// </summary>
    public enum MeasureMode
    {
      Undefined,
      Exactly,
      AtMost,
    }

    private class DataWatchRequest : IUIElementDataWatchRequest, IVisualElementPanelActivatable, IDisposable
    {
      private VisualElementPanelActivator m_Activator;

      public DataWatchRequest(VisualElement handler)
      {
        this.element = handler;
        this.m_Activator = new VisualElementPanelActivator((IVisualElementPanelActivatable) this);
      }

      public Action<UnityEngine.Object> notification { get; set; }

      public UnityEngine.Object watchedObject { get; set; }

      public IDataWatchHandle requestedHandle { get; set; }

      public VisualElement element { get; set; }

      public void Start()
      {
        this.m_Activator.SetActive(true);
      }

      public void Stop()
      {
        this.m_Activator.SetActive(false);
      }

      public bool CanBeActivated()
      {
        return this.element != null && this.element.elementPanel != null && this.element.elementPanel.dataWatch != null;
      }

      public void OnPanelActivate()
      {
        if (this.requestedHandle != null)
          return;
        this.requestedHandle = this.element.elementPanel.dataWatch.AddWatch(this.watchedObject, this.notification);
      }

      public void OnPanelDeactivate()
      {
        if (this.requestedHandle == null)
          return;
        this.element.elementPanel.dataWatch.RemoveWatch(this.requestedHandle);
        this.requestedHandle = (IDataWatchHandle) null;
      }

      public void Dispose()
      {
        this.Stop();
      }
    }

    /// <summary>
    ///   <para>Options to select clipping strategy.</para>
    /// </summary>
    public enum ClippingOptions
    {
      ClipContents,
      NoClipping,
      ClipAndCacheContents,
    }

    /// <summary>
    ///   <para>Hierarchy is a sctuct allowing access to the shadow hierarchy of visual elements</para>
    /// </summary>
    public struct Hierarchy
    {
      private readonly VisualElement m_Owner;

      internal Hierarchy(VisualElement element)
      {
        this.m_Owner = element;
      }

      /// <summary>
      ///   <para> Access the physical parent of this element in the hierarchy
      ///           </para>
      /// </summary>
      public VisualElement parent
      {
        get
        {
          return this.m_Owner.m_PhysicalParent;
        }
      }

      /// <summary>
      ///   <para>Add an element to this element's contentContainer</para>
      /// </summary>
      /// <param name="child"></param>
      public void Add(VisualElement child)
      {
        if (child == null)
          throw new ArgumentException("Cannot add null child");
        this.Insert(this.childCount, child);
      }

      public void Insert(int index, VisualElement child)
      {
        if (child == null)
          throw new ArgumentException("Cannot insert null child");
        if (index > this.childCount)
          throw new IndexOutOfRangeException("Index out of range: " + (object) index);
        if (child == this.m_Owner)
          throw new ArgumentException("Cannot insert element as its own child");
        child.RemoveFromHierarchy();
        child.shadow.SetParent(this.m_Owner);
        if (this.m_Owner.m_Children == null)
          this.m_Owner.m_Children = new List<VisualElement>();
        if (this.m_Owner.cssNode.IsMeasureDefined)
          this.m_Owner.cssNode.SetMeasureFunction((MeasureFunction) null);
        if (index >= this.m_Owner.m_Children.Count)
        {
          this.m_Owner.m_Children.Add(child);
          this.m_Owner.cssNode.Insert(this.m_Owner.cssNode.Count, child.cssNode);
        }
        else
        {
          this.m_Owner.m_Children.Insert(index, child);
          this.m_Owner.cssNode.Insert(index, child.cssNode);
        }
        child.SetEnabledFromHierarchy(this.m_Owner.enabledInHierarchy);
        child.Dirty(ChangeType.Styles);
        this.m_Owner.Dirty(ChangeType.Layout);
        if (string.IsNullOrEmpty(child.persistenceKey))
          return;
        child.Dirty(ChangeType.PersistentData);
      }

      /// <summary>
      ///   <para>Removes this child from the hierarchy</para>
      /// </summary>
      /// <param name="child"></param>
      public void Remove(VisualElement child)
      {
        if (child == null)
          throw new ArgumentException("Cannot remove null child");
        if (child.shadow.parent != this.m_Owner)
          throw new ArgumentException("This visualElement is not my child");
        if (this.m_Owner.m_Children == null)
          return;
        this.RemoveAt(this.m_Owner.m_Children.IndexOf(child));
      }

      /// <summary>
      ///   <para>Remove the child element located at this position from this element's contentContainer</para>
      /// </summary>
      /// <param name="index"></param>
      public void RemoveAt(int index)
      {
        if (index < 0 || index >= this.childCount)
          throw new IndexOutOfRangeException("Index out of range: " + (object) index);
        this.m_Owner.m_Children[index].shadow.SetParent((VisualElement) null);
        this.m_Owner.m_Children.RemoveAt(index);
        this.m_Owner.cssNode.RemoveAt(index);
        if (this.childCount == 0)
          this.m_Owner.cssNode.SetMeasureFunction(new MeasureFunction(this.m_Owner.Measure));
        this.m_Owner.Dirty(ChangeType.Layout);
      }

      /// <summary>
      ///   <para>Remove all child elements from this element's contentContainer</para>
      /// </summary>
      public void Clear()
      {
        if (this.childCount <= 0)
          return;
        foreach (VisualElement child in this.m_Owner.m_Children)
        {
          child.shadow.SetParent((VisualElement) null);
          child.m_LogicalParent = (VisualElement) null;
        }
        this.m_Owner.m_Children.Clear();
        this.m_Owner.cssNode.Clear();
        this.m_Owner.Dirty(ChangeType.Layout);
      }

      /// <summary>
      ///   <para> Number of child elements in this object's contentContainer
      ///           </para>
      /// </summary>
      public int childCount
      {
        get
        {
          return this.m_Owner.m_Children == null ? 0 : this.m_Owner.m_Children.Count;
        }
      }

      public VisualElement this[int key]
      {
        get
        {
          return this.ElementAt(key);
        }
      }

      /// <summary>
      ///   <para>Retrieves the child element at position</para>
      /// </summary>
      /// <param name="index"></param>
      public VisualElement ElementAt(int index)
      {
        if (this.m_Owner.m_Children != null)
          return this.m_Owner.m_Children[index];
        throw new IndexOutOfRangeException("Index out of range: " + (object) index);
      }

      /// <summary>
      ///   <para>Returns the elements from its contentContainer</para>
      /// </summary>
      public IEnumerable<VisualElement> Children()
      {
        if (this.m_Owner.m_Children != null)
          return (IEnumerable<VisualElement>) this.m_Owner.m_Children;
        return (IEnumerable<VisualElement>) VisualElement.s_EmptyList;
      }

      private void SetParent(VisualElement value)
      {
        this.m_Owner.m_PhysicalParent = value;
        this.m_Owner.m_LogicalParent = value;
        if (value != null)
        {
          this.m_Owner.ChangePanel(this.m_Owner.m_PhysicalParent.elementPanel);
          this.m_Owner.PropagateChangesToParents();
        }
        else
          this.m_Owner.ChangePanel((BaseVisualElementPanel) null);
      }

      public void Sort(Comparison<VisualElement> comp)
      {
        this.m_Owner.m_Children.Sort(comp);
        this.m_Owner.cssNode.Clear();
        for (int index = 0; index < this.m_Owner.m_Children.Count; ++index)
          this.m_Owner.cssNode.Insert(index, this.m_Owner.m_Children[index].cssNode);
        this.m_Owner.Dirty(ChangeType.Layout);
      }
    }

    private abstract class BaseVisualElementScheduledItem : ScheduledItem, IVisualElementScheduledItem, IVisualElementPanelActivatable
    {
      public bool isScheduled = false;
      private VisualElementPanelActivator m_Activator;

      protected BaseVisualElementScheduledItem(VisualElement handler)
      {
        this.element = handler;
        this.m_Activator = new VisualElementPanelActivator((IVisualElementPanelActivatable) this);
      }

      public VisualElement element { get; private set; }

      public bool isActive
      {
        get
        {
          return this.m_Activator.isActive;
        }
      }

      public IVisualElementScheduledItem StartingIn(long delayMs)
      {
        this.delayMs = delayMs;
        return (IVisualElementScheduledItem) this;
      }

      public IVisualElementScheduledItem Until(Func<bool> stopCondition)
      {
        if (stopCondition == null)
          stopCondition = ScheduledItem.ForeverCondition;
        this.timerUpdateStopCondition = stopCondition;
        return (IVisualElementScheduledItem) this;
      }

      public IVisualElementScheduledItem ForDuration(long durationMs)
      {
        this.SetDuration(durationMs);
        return (IVisualElementScheduledItem) this;
      }

      public IVisualElementScheduledItem Every(long intervalMs)
      {
        this.intervalMs = intervalMs;
        if (this.timerUpdateStopCondition == ScheduledItem.OnceCondition)
          this.timerUpdateStopCondition = ScheduledItem.ForeverCondition;
        return (IVisualElementScheduledItem) this;
      }

      internal override void OnItemUnscheduled()
      {
        base.OnItemUnscheduled();
        this.isScheduled = false;
        if (this.m_Activator.isDetaching)
          return;
        this.m_Activator.SetActive(false);
      }

      public void Resume()
      {
        this.isScheduled = true;
        this.m_Activator.SetActive(true);
      }

      public void Pause()
      {
        this.m_Activator.SetActive(false);
      }

      public void ExecuteLater(long delayMs)
      {
        if (!this.isScheduled)
          this.Resume();
        this.ResetStartTime();
        this.StartingIn(delayMs);
      }

      public void OnPanelActivate()
      {
        this.isScheduled = true;
        this.ResetStartTime();
        this.element.elementPanel.scheduler.Schedule((IScheduledItem) this);
      }

      public void OnPanelDeactivate()
      {
        if (!this.isScheduled)
          return;
        this.isScheduled = false;
        this.element.elementPanel.scheduler.Unschedule((IScheduledItem) this);
      }

      public bool CanBeActivated()
      {
        return this.element != null && this.element.elementPanel != null && this.element.elementPanel.scheduler != null;
      }
    }

    private abstract class VisualElementScheduledItem<ActionType> : VisualElement.BaseVisualElementScheduledItem
    {
      public ActionType updateEvent;

      public VisualElementScheduledItem(VisualElement handler, ActionType upEvent)
        : base(handler)
      {
        this.updateEvent = upEvent;
      }

      public static bool Matches(ScheduledItem item, ActionType updateEvent)
      {
        VisualElement.VisualElementScheduledItem<ActionType> elementScheduledItem = item as VisualElement.VisualElementScheduledItem<ActionType>;
        if (elementScheduledItem != null)
          return EqualityComparer<ActionType>.Default.Equals(elementScheduledItem.updateEvent, updateEvent);
        return false;
      }
    }

    private class TimerStateScheduledItem : VisualElement.VisualElementScheduledItem<Action<TimerState>>
    {
      public TimerStateScheduledItem(VisualElement handler, Action<TimerState> updateEvent)
        : base(handler, updateEvent)
      {
      }

      public override void PerformTimerUpdate(TimerState state)
      {
        if (!this.isScheduled)
          return;
        this.updateEvent(state);
      }
    }

    private class SimpleScheduledItem : VisualElement.VisualElementScheduledItem<Action>
    {
      public SimpleScheduledItem(VisualElement handler, Action updateEvent)
        : base(handler, updateEvent)
      {
      }

      public override void PerformTimerUpdate(TimerState state)
      {
        if (!this.isScheduled)
          return;
        this.updateEvent();
      }
    }
  }
}
