// Decompiled with JetBrains decompiler
// Type: BaseExposedPropertyDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

internal abstract class BaseExposedPropertyDrawer : PropertyDrawer
{
  private static float kDriveWidgetWidth = 18f;
  private static GUIStyle kDropDownStyle = (GUIStyle) null;
  private static Color kMissingOverrideColor = new Color(1f, 0.11f, 0.11f, 1f);
  protected readonly GUIContent ExposePropertyContent = EditorGUIUtility.TextContent("Expose Property");
  protected readonly GUIContent UnexposePropertyContent = EditorGUIUtility.TextContent("Unexpose Property");
  protected readonly GUIContent NotFoundOn = EditorGUIUtility.TextContent("not found on");
  protected readonly GUIContent OverridenByContent = EditorGUIUtility.TextContent("Overriden by ");
  private GUIContent m_ModifiedLabel = new GUIContent();

  public BaseExposedPropertyDrawer()
  {
    if (BaseExposedPropertyDrawer.kDropDownStyle != null)
      return;
    BaseExposedPropertyDrawer.kDropDownStyle = new GUIStyle((GUIStyle) "ShurikenDropdown");
  }

  private static BaseExposedPropertyDrawer.ExposedPropertyMode GetExposedPropertyMode(string propertyName)
  {
    if (string.IsNullOrEmpty(propertyName))
      return BaseExposedPropertyDrawer.ExposedPropertyMode.DefaultValue;
    GUID result;
    return GUID.TryParse(propertyName, out result) ? BaseExposedPropertyDrawer.ExposedPropertyMode.NamedGUID : BaseExposedPropertyDrawer.ExposedPropertyMode.Named;
  }

  protected IExposedPropertyTable GetExposedPropertyTable(SerializedProperty property)
  {
    return property.serializedObject.context as IExposedPropertyTable;
  }

  protected abstract void OnRenderProperty(Rect position, PropertyName exposedPropertyNameString, Object currentReferenceValue, SerializedProperty exposedPropertyDefault, SerializedProperty exposedPropertyName, BaseExposedPropertyDrawer.ExposedPropertyMode mode, IExposedPropertyTable exposedProperties);

  public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
  {
    SerializedProperty propertyRelative1 = prop.FindPropertyRelative("defaultValue");
    SerializedProperty propertyRelative2 = prop.FindPropertyRelative("exposedName");
    string stringValue = propertyRelative2.stringValue;
    BaseExposedPropertyDrawer.ExposedPropertyMode exposedPropertyMode = BaseExposedPropertyDrawer.GetExposedPropertyMode(stringValue);
    Rect rect = position;
    rect.xMax -= BaseExposedPropertyDrawer.kDriveWidgetWidth;
    Rect position1 = position;
    position1.x = rect.xMax;
    position1.width = BaseExposedPropertyDrawer.kDriveWidgetWidth;
    IExposedPropertyTable exposedPropertyTable = this.GetExposedPropertyTable(prop);
    bool showContextMenu = exposedPropertyTable != null;
    PropertyName propertyName = new PropertyName(stringValue);
    BaseExposedPropertyDrawer.OverrideState currentOverrideState = BaseExposedPropertyDrawer.OverrideState.DefaultValue;
    Object currentReferenceValue = this.Resolve(propertyName, exposedPropertyTable, propertyRelative1.objectReferenceValue, out currentOverrideState);
    Color color = GUI.color;
    bool boldDefaultFont = EditorGUIUtility.GetBoldDefaultFont();
    Rect position2 = this.DrawLabel(showContextMenu, currentOverrideState, label, position, exposedPropertyTable, stringValue, propertyRelative2, propertyRelative1);
    EditorGUI.BeginChangeCheck();
    if (exposedPropertyMode == BaseExposedPropertyDrawer.ExposedPropertyMode.DefaultValue || exposedPropertyMode == BaseExposedPropertyDrawer.ExposedPropertyMode.NamedGUID)
    {
      this.OnRenderProperty(position2, propertyName, currentReferenceValue, propertyRelative1, propertyRelative2, exposedPropertyMode, exposedPropertyTable);
    }
    else
    {
      position2.width /= 2f;
      EditorGUI.BeginChangeCheck();
      string name = EditorGUI.TextField(position2, stringValue);
      if (EditorGUI.EndChangeCheck())
        propertyRelative2.stringValue = name;
      position2.x += position2.width;
      this.OnRenderProperty(position2, new PropertyName(name), currentReferenceValue, propertyRelative1, propertyRelative2, exposedPropertyMode, exposedPropertyTable);
    }
    EditorGUI.EndDisabledGroup();
    GUI.color = color;
    EditorGUIUtility.SetBoldDefaultFont(boldDefaultFont);
    if (!showContextMenu || !GUI.Button(position1, GUIContent.none, BaseExposedPropertyDrawer.kDropDownStyle))
      return;
    GenericMenu menu = new GenericMenu();
    this.PopulateContextMenu(menu, currentOverrideState, exposedPropertyTable, propertyRelative2, propertyRelative1);
    menu.ShowAsContext();
    Event.current.Use();
  }

  private Rect DrawLabel(bool showContextMenu, BaseExposedPropertyDrawer.OverrideState currentOverrideState, GUIContent label, Rect position, IExposedPropertyTable exposedPropertyTable, string exposedNameStr, SerializedProperty exposedName, SerializedProperty defaultValue)
  {
    if (showContextMenu)
      position.xMax -= BaseExposedPropertyDrawer.kDriveWidgetWidth;
    EditorGUIUtility.SetBoldDefaultFont(currentOverrideState != BaseExposedPropertyDrawer.OverrideState.DefaultValue);
    this.m_ModifiedLabel.text = label.text;
    this.m_ModifiedLabel.tooltip = label.tooltip;
    this.m_ModifiedLabel.image = label.image;
    if (!string.IsNullOrEmpty(this.m_ModifiedLabel.tooltip))
      this.m_ModifiedLabel.tooltip += "\n";
    switch (currentOverrideState)
    {
      case BaseExposedPropertyDrawer.OverrideState.MissingOverride:
        GUI.color = BaseExposedPropertyDrawer.kMissingOverrideColor;
        GUIContent modifiedLabel1 = this.m_ModifiedLabel;
        modifiedLabel1.tooltip = modifiedLabel1.tooltip + label.text + " " + this.NotFoundOn.text + " " + exposedPropertyTable.ToString() + ".";
        break;
      case BaseExposedPropertyDrawer.OverrideState.Overridden:
        if (exposedPropertyTable != null)
        {
          GUIContent modifiedLabel2 = this.m_ModifiedLabel;
          modifiedLabel2.tooltip = modifiedLabel2.tooltip + this.OverridenByContent.text + exposedPropertyTable.ToString() + ".";
          break;
        }
        break;
    }
    Rect rect = EditorGUI.PrefixLabel(position, this.m_ModifiedLabel);
    if (exposedPropertyTable != null && Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition))
    {
      GenericMenu menu = new GenericMenu();
      this.PopulateContextMenu(menu, !string.IsNullOrEmpty(exposedNameStr) ? BaseExposedPropertyDrawer.OverrideState.Overridden : BaseExposedPropertyDrawer.OverrideState.DefaultValue, exposedPropertyTable, exposedName, defaultValue);
      menu.ShowAsContext();
    }
    return rect;
  }

  protected Object Resolve(PropertyName exposedPropertyName, IExposedPropertyTable exposedPropertyTable, Object defaultValue, out BaseExposedPropertyDrawer.OverrideState currentOverrideState)
  {
    Object @object = (Object) null;
    bool idValid = false;
    bool flag = !PropertyName.IsNullOrEmpty(exposedPropertyName);
    currentOverrideState = BaseExposedPropertyDrawer.OverrideState.DefaultValue;
    if (exposedPropertyTable != null)
    {
      @object = exposedPropertyTable.GetReferenceValue(exposedPropertyName, out idValid);
      if (idValid)
        currentOverrideState = BaseExposedPropertyDrawer.OverrideState.Overridden;
      else if (flag)
        currentOverrideState = BaseExposedPropertyDrawer.OverrideState.MissingOverride;
    }
    return currentOverrideState != BaseExposedPropertyDrawer.OverrideState.Overridden ? defaultValue : @object;
  }

  protected abstract void PopulateContextMenu(GenericMenu menu, BaseExposedPropertyDrawer.OverrideState overrideState, IExposedPropertyTable exposedPropertyTable, SerializedProperty exposedName, SerializedProperty defaultValue);

  protected enum ExposedPropertyMode
  {
    DefaultValue,
    Named,
    NamedGUID,
  }

  protected enum OverrideState
  {
    DefaultValue,
    MissingOverride,
    Overridden,
  }
}
