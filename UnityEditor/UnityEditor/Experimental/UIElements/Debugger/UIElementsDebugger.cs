// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.UIElementsDebugger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class UIElementsDebugger : EditorWindow
  {
    private static readonly PropertyInfo[] k_FieldInfos = typeof (IStyle).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    private static readonly PropertyInfo[] k_SortedFieldInfos = ((IEnumerable<PropertyInfo>) UIElementsDebugger.k_FieldInfos).OrderBy<PropertyInfo, string>((Func<PropertyInfo, string>) (f => f.Name)).ToArray<PropertyInfo>();
    private static MatchedRulesExtractor s_MatchedRulesExtractor = new MatchedRulesExtractor();
    private HashSet<int> m_CurFoldout = new HashSet<int>();
    private string m_DumpId = "dump";
    private string m_DetailFilter = string.Empty;
    private Vector2 m_DetailScroll = Vector2.zero;
    private bool m_Overlay = true;
    [SerializeField]
    private string m_LastWindowTitle;
    private bool m_ScheduleRestoreSelection;
    private bool m_UxmlDumpExpanded;
    private bool m_UxmlDumpStyleFields;
    private bool m_NewLineOnAttributes;
    private bool m_AutoNameElements;
    private UIElementsDebugger.ViewPanel? m_CurPanel;
    private PickingData m_PickingData;
    private bool m_PickingElementInPanel;
    private VisualElement m_SelectedElement;
    private bool m_ShowDefaults;
    private bool m_Sort;
    private SplitterState m_SplitterState;
    private Texture2D m_TempTexture;
    private TreeViewState m_VisualTreeTreeViewState;
    private VisualTreeTreeView m_VisualTreeTreeView;
    private string m_SelectedElementUxml;
    private ReorderableList m_ClassList;
    private string m_NewClass;

    [MenuItem("Window/UI Debuggers/UIElements Debugger", false, 2013, true)]
    public static void Open()
    {
      EditorWindow.GetWindow<UIElementsDebugger>().Show();
    }

    private bool InterceptEvents(Event ev)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UIElementsDebugger.\u003CInterceptEvents\u003Ec__AnonStorey0 eventsCAnonStorey0 = new UIElementsDebugger.\u003CInterceptEvents\u003Ec__AnonStorey0();
      if (!this.m_CurPanel.HasValue || !Event.current.isMouse)
        return false;
      // ISSUE: reference to a compiler-generated field
      eventsCAnonStorey0.e = this.m_CurPanel.Value.Panel.Pick(ev.mousePosition);
      // ISSUE: reference to a compiler-generated field
      if (eventsCAnonStorey0.e != null)
      {
        // ISSUE: reference to a compiler-generated field
        ((PanelDebug) this.m_CurPanel.Value.Panel.panelDebug).highlightedElement = eventsCAnonStorey0.e.controlid;
      }
      if (ev.clickCount > 0 && ev.button == 0)
      {
        this.m_CurPanel.Value.Panel.panelDebug.interceptEvents = (Func<Event, bool>) null;
        this.m_PickingElementInPanel = false;
        this.m_VisualTreeTreeView.ExpandAll();
        // ISSUE: reference to a compiler-generated method
        VisualTreeItem visualTreeItem = this.m_VisualTreeTreeView.GetRows().OfType<VisualTreeItem>().FirstOrDefault<VisualTreeItem>(new Func<VisualTreeItem, bool>(eventsCAnonStorey0.\u003C\u003Em__0));
        if (visualTreeItem != null)
          this.m_VisualTreeTreeView.SetSelection((IList<int>) new List<int>()
          {
            visualTreeItem.id
          }, TreeViewSelectionOptions.RevealAndFrame);
      }
      return true;
    }

    public void OnGUI()
    {
      if (this.m_ScheduleRestoreSelection)
      {
        this.m_ScheduleRestoreSelection = false;
        if (this.m_PickingData.TryRestoreSelectedWindow(this.m_LastWindowTitle))
        {
          this.EndPicking(this.m_PickingData.Selected);
          this.m_VisualTreeTreeView.ExpandAll();
        }
        else
          this.m_LastWindowTitle = string.Empty;
      }
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
      bool flag1 = false;
      EditorGUI.BeginChangeCheck();
      this.m_PickingData.DoSelectDropDown();
      if (EditorGUI.EndChangeCheck())
        flag1 = true;
      if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, new GUILayoutOption[1]{ GUILayout.Width(50f) }))
        this.m_PickingData.Refresh();
      bool flag2 = GUILayout.Toggle(this.m_VisualTreeTreeView.includeShadowHierarchy, UIElementsDebugger.Styles.includeShadowHierarchyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (flag2 != this.m_VisualTreeTreeView.includeShadowHierarchy)
      {
        this.m_VisualTreeTreeView.includeShadowHierarchy = flag2;
        flag1 = true;
      }
      if (this.m_CurPanel.HasValue)
      {
        bool flag3 = GUILayout.Toggle(this.m_PickingElementInPanel, UIElementsDebugger.Styles.pickElementInPanelContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
        if (flag3 != this.m_PickingElementInPanel)
        {
          this.m_PickingElementInPanel = flag3;
          if (this.m_PickingElementInPanel)
            this.m_CurPanel.Value.Panel.panelDebug.interceptEvents = new Func<Event, bool>(this.InterceptEvents);
        }
      }
      this.m_Overlay = GUILayout.Toggle(this.m_Overlay, UIElementsDebugger.Styles.overlayContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      EditorGUILayout.EndHorizontal();
      if (flag1)
        this.EndPicking(this.m_PickingData.Selected);
      if (!this.m_CurPanel.HasValue)
        return;
      if (this.m_CurPanel.Value.Panel.panelDebug.enabled != this.m_Overlay)
      {
        this.m_CurPanel.Value.Panel.panelDebug.enabled = this.m_Overlay;
        this.m_CurPanel.Value.Panel.visualTree.Dirty(ChangeType.Repaint);
      }
      SplitterGUILayout.BeginHorizontalSplit(this.m_SplitterState, new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      SplitterGUILayout.EndHorizontalSplit();
      float num = this.m_SplitterState.realSizes.Length <= 0 ? 150f : (float) this.m_SplitterState.realSizes[0];
      float width = this.position.width - num;
      Rect rect1 = new Rect(0.0f, 17f, num, this.position.height - 17f);
      Rect rect2 = new Rect(num, 17f, width, rect1.height);
      this.m_VisualTreeTreeView.OnGUI(rect1);
      this.DrawSelection(rect2);
      EditorGUI.DrawRect(new Rect(num + rect1.xMin, rect1.y, 1f, rect1.height), UIElementsDebugger.Styles.separatorColor);
    }

    private void EndPicking(UIElementsDebugger.ViewPanel? viewPanel)
    {
      Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
      while (panelsIterator.MoveNext())
        panelsIterator.Current.Value.panelDebug = (BasePanelDebug) null;
      this.m_CurPanel = viewPanel;
      if (!this.m_CurPanel.HasValue)
        return;
      this.m_LastWindowTitle = PickingData.GetName(this.m_CurPanel.Value);
      if (this.m_CurPanel.Value.Panel.panelDebug == null)
        this.m_CurPanel.Value.Panel.panelDebug = (BasePanelDebug) new PanelDebug();
      this.m_CurPanel.Value.Panel.panelDebug.enabled = true;
      this.m_CurPanel.Value.Panel.visualTree.Dirty(ChangeType.Repaint);
      this.m_VisualTreeTreeView.panel = this.m_CurPanel.Value.Panel;
      this.m_VisualTreeTreeView.Reload();
    }

    private void DrawSelection(Rect rect)
    {
      if (Event.current.type == EventType.Layout)
        this.CacheData();
      if (this.m_SelectedElement == null)
        return;
      GUILayout.BeginArea(rect);
      EditorGUILayout.LabelField(this.m_SelectedElement.GetType().Name, UIElementsDebugger.Styles.KInspectorTitle, new GUILayoutOption[0]);
      this.m_DetailScroll = EditorGUILayout.BeginScrollView(this.m_DetailScroll);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, new GUILayoutOption[2]{ GUILayout.ExpandWidth(true), GUILayout.Height(350f) });
      rect1.y += EditorGUIUtility.singleLineHeight;
      this.DrawSize(rect1, this.m_SelectedElement);
      this.DrawUxmlDump(this.m_SelectedElement);
      this.DrawMatchingRules();
      this.DrawProperties();
      EditorGUILayout.EndScrollView();
      GUILayout.EndArea();
    }

    private void DrawUxmlDump(VisualElement selectedElement)
    {
      this.m_UxmlDumpExpanded = EditorGUILayout.Foldout(this.m_UxmlDumpExpanded, UIElementsDebugger.Styles.uxmlContent);
      if (!this.m_UxmlDumpExpanded)
        return;
      EditorGUI.BeginChangeCheck();
      this.m_DumpId = EditorGUILayout.TextField("Template id", this.m_DumpId, new GUILayoutOption[0]);
      this.m_UxmlDumpStyleFields = EditorGUILayout.Toggle("Include style fields", this.m_UxmlDumpStyleFields, new GUILayoutOption[0]);
      this.m_NewLineOnAttributes = EditorGUILayout.Toggle("Line breaks on attributes", this.m_NewLineOnAttributes, new GUILayoutOption[0]);
      this.m_AutoNameElements = EditorGUILayout.Toggle("Auto name elements", this.m_AutoNameElements, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_SelectedElementUxml = (string) null;
      if (this.m_SelectedElementUxml == null)
      {
        UxmlExporter.ExportOptions options = UxmlExporter.ExportOptions.None;
        if (this.m_UxmlDumpStyleFields)
          options = UxmlExporter.ExportOptions.StyleFields;
        if (this.m_NewLineOnAttributes)
          options |= UxmlExporter.ExportOptions.NewLineOnAttributes;
        if (this.m_AutoNameElements)
          options |= UxmlExporter.ExportOptions.AutoNameElements;
        this.m_SelectedElementUxml = UxmlExporter.Dump(selectedElement, this.m_DumpId ?? "template", options);
      }
      EditorGUILayout.TextArea(this.m_SelectedElementUxml);
    }

    private void CacheData()
    {
      if (!this.m_VisualTreeTreeView.HasSelection())
      {
        this.m_SelectedElement = (VisualElement) null;
        this.m_UxmlDumpExpanded = false;
        this.m_ClassList = (ReorderableList) null;
        if (this.m_PickingElementInPanel || !this.m_CurPanel.HasValue || this.m_CurPanel.Value.Panel == null || this.m_CurPanel.Value.Panel.panelDebug == null)
          return;
        ((PanelDebug) this.m_CurPanel.Value.Panel.panelDebug).highlightedElement = 0U;
      }
      else
      {
        VisualTreeItem nodeFor = this.m_VisualTreeTreeView.GetNodeFor(this.m_VisualTreeTreeView.GetSelection().First<int>());
        if (nodeFor == null)
          return;
        VisualElement elt = nodeFor.elt;
        if (elt == null || !this.m_CurPanel.HasValue)
          return;
        if (this.m_SelectedElement != elt)
        {
          this.m_SelectedElement = elt;
          this.m_SelectedElementUxml = (string) null;
          this.m_ClassList = (ReorderableList) null;
        }
        if (!this.m_PickingElementInPanel)
          ((PanelDebug) this.m_CurPanel.Value.Panel.panelDebug).highlightedElement = elt.controlid;
        this.GetElementMatchers();
      }
    }

    private void GetElementMatchers()
    {
      if (this.m_SelectedElement == null || this.m_SelectedElement.elementPanel == null)
        return;
      UIElementsDebugger.s_MatchedRulesExtractor.selectedElementRules.Clear();
      UIElementsDebugger.s_MatchedRulesExtractor.selectedElementStylesheets.Clear();
      UIElementsDebugger.s_MatchedRulesExtractor.target = this.m_SelectedElement;
      UIElementsDebugger.s_MatchedRulesExtractor.Traverse(this.m_SelectedElement.elementPanel.visualTree, 0, UIElementsDebugger.s_MatchedRulesExtractor.ruleMatchers);
      UIElementsDebugger.s_MatchedRulesExtractor.ruleMatchers.Clear();
    }

    private static int GetSpecificity<T>(StyleValue<T> style)
    {
      return style.specificity;
    }

    private void DrawProperties()
    {
      EditorGUILayout.LabelField(UIElementsDebugger.Styles.elementStylesContent, UIElementsDebugger.Styles.KInspectorTitle, new GUILayoutOption[0]);
      this.m_SelectedElement.name = EditorGUILayout.TextField("Name", this.m_SelectedElement.name, new GUILayoutOption[0]);
      this.m_SelectedElement.text = EditorGUILayout.TextField("Text", this.m_SelectedElement.text, new GUILayoutOption[0]);
      this.m_SelectedElement.clippingOptions = (VisualElement.ClippingOptions) EditorGUILayout.EnumPopup("Clipping Option", (Enum) this.m_SelectedElement.clippingOptions, new GUILayoutOption[0]);
      this.m_SelectedElement.visible = EditorGUILayout.Toggle("Visible", this.m_SelectedElement.visible, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Layout", this.m_SelectedElement.layout.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("World Bound", this.m_SelectedElement.worldBound.ToString(), new GUILayoutOption[0]);
      if (this.m_ClassList == null)
        this.InitClassList();
      this.m_ClassList.DoLayoutList();
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.m_DetailFilter = EditorGUILayout.ToolbarSearchField(this.m_DetailFilter);
      this.m_ShowDefaults = GUILayout.Toggle(this.m_ShowDefaults, UIElementsDebugger.Styles.showDefaultsContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.m_Sort = GUILayout.Toggle(this.m_Sort, UIElementsDebugger.Styles.sortContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      VisualElementStylesData effectiveStyle = this.m_SelectedElement.effectiveStyle;
      bool flag = false;
      foreach (PropertyInfo field1 in !this.m_Sort ? UIElementsDebugger.k_FieldInfos : UIElementsDebugger.k_SortedFieldInfos)
      {
        if ((string.IsNullOrEmpty(this.m_DetailFilter) || field1.Name.IndexOf(this.m_DetailFilter, StringComparison.InvariantCultureIgnoreCase) != -1) && (field1.PropertyType.IsGenericType && field1.PropertyType.GetGenericTypeDefinition() == typeof (StyleValue<>)))
        {
          object val = field1.GetValue((object) this.m_SelectedElement, (object[]) null);
          EditorGUILayout.BeginHorizontal();
          EditorGUI.BeginChangeCheck();
          int num;
          if (val is StyleValue<float>)
          {
            StyleValue<float> style = (StyleValue<float>) val;
            num = UIElementsDebugger.GetSpecificity<float>(style);
            if (this.m_ShowDefaults || num > 0)
            {
              style.specificity = int.MaxValue;
              style.value = EditorGUILayout.FloatField(field1.Name, ((StyleValue<float>) val).value, new GUILayoutOption[0]);
              val = (object) style;
            }
          }
          else if (val is StyleValue<int>)
          {
            StyleValue<int> style = (StyleValue<int>) val;
            num = UIElementsDebugger.GetSpecificity<int>(style);
            if (this.m_ShowDefaults || num > 0)
            {
              style.specificity = int.MaxValue;
              style.value = EditorGUILayout.IntField(field1.Name, ((StyleValue<int>) val).value, new GUILayoutOption[0]);
              val = (object) style;
            }
          }
          else if (val is StyleValue<bool>)
          {
            StyleValue<bool> style = (StyleValue<bool>) val;
            num = UIElementsDebugger.GetSpecificity<bool>(style);
            if (this.m_ShowDefaults || num > 0)
            {
              style.specificity = int.MaxValue;
              style.value = EditorGUILayout.Toggle(field1.Name, ((StyleValue<bool>) val).value, new GUILayoutOption[0]);
              val = (object) style;
            }
          }
          else if (val is StyleValue<Color>)
          {
            StyleValue<Color> style = (StyleValue<Color>) val;
            num = UIElementsDebugger.GetSpecificity<Color>(style);
            if (this.m_ShowDefaults || num > 0)
            {
              style.specificity = int.MaxValue;
              style.value = EditorGUILayout.ColorField(field1.Name, ((StyleValue<Color>) val).value, new GUILayoutOption[0]);
              val = (object) style;
            }
          }
          else if (val is StyleValue<Font>)
            num = this.HandleReferenceProperty<Font>(field1, ref val);
          else if (val is StyleValue<Texture2D>)
          {
            num = this.HandleReferenceProperty<Texture2D>(field1, ref val);
          }
          else
          {
            System.Type type = val.GetType();
            if (type.GetGenericArguments()[0].IsEnum)
            {
              num = (int) type.GetField("specificity", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(val);
              if (this.m_ShowDefaults || num > 0)
              {
                System.Reflection.FieldInfo field2 = type.GetField("value");
                Enum selected = field2.GetValue(val) as Enum;
                Enum @enum = EditorGUILayout.EnumPopup(field1.Name, selected, new GUILayoutOption[0]);
                if (!object.Equals((object) selected, (object) @enum))
                  field2.SetValue(val, (object) @enum);
              }
            }
            else
            {
              EditorGUILayout.LabelField(field1.Name, val != null ? val.ToString() : "null", new GUILayoutOption[0]);
              num = -1;
            }
          }
          if (EditorGUI.EndChangeCheck())
          {
            flag = true;
            field1.SetValue((object) this.m_SelectedElement, val, (object[]) null);
          }
          if (num > 0)
            GUILayout.Label(num != int.MaxValue ? num.ToString() : "inline");
          EditorGUILayout.EndHorizontal();
        }
      }
      if (!flag)
        return;
      this.m_CurPanel.Value.Panel.visualTree.Dirty(ChangeType.Transform);
      this.m_CurPanel.Value.Panel.visualTree.Dirty(ChangeType.Styles);
      this.m_CurPanel.Value.Panel.visualTree.Dirty(ChangeType.Layout);
      this.m_CurPanel.Value.Panel.visualTree.Dirty(ChangeType.Repaint);
      this.m_CurPanel.Value.View.RepaintImmediately();
    }

    private void InitClassList()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UIElementsDebugger.\u003CInitClassList\u003Ec__AnonStorey1 listCAnonStorey1 = new UIElementsDebugger.\u003CInitClassList\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey1.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      listCAnonStorey1.refresh = new System.Action(listCAnonStorey1.\u003C\u003Em__0);
      this.m_ClassList = new ReorderableList((IList) this.m_SelectedElement.GetClasses().ToList<string>(), typeof (string), false, true, true, true);
      // ISSUE: reference to a compiler-generated method
      this.m_ClassList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(listCAnonStorey1.\u003C\u003Em__1);
      // ISSUE: reference to a compiler-generated method
      this.m_ClassList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(listCAnonStorey1.\u003C\u003Em__2);
      // ISSUE: reference to a compiler-generated method
      this.m_ClassList.onCanAddCallback = new ReorderableList.CanAddCallbackDelegate(listCAnonStorey1.\u003C\u003Em__3);
      // ISSUE: reference to a compiler-generated method
      this.m_ClassList.onAddCallback = new ReorderableList.AddCallbackDelegate(listCAnonStorey1.\u003C\u003Em__4);
    }

    private int HandleReferenceProperty<T>(PropertyInfo field, ref object val) where T : UnityEngine.Object
    {
      StyleValue<T> style = (StyleValue<T>) val;
      int specificity = UIElementsDebugger.GetSpecificity<T>(style);
      if (this.m_ShowDefaults || specificity > 0)
      {
        style.specificity = int.MaxValue;
        style.value = EditorGUILayout.ObjectField(field.Name, (UnityEngine.Object) ((StyleValue<T>) val).value, typeof (T), false, new GUILayoutOption[0]) as T;
        val = (object) style;
      }
      return specificity;
    }

    private void DrawMatchingRules()
    {
      if (UIElementsDebugger.s_MatchedRulesExtractor.selectedElementStylesheets != null && UIElementsDebugger.s_MatchedRulesExtractor.selectedElementStylesheets.Count > 0)
      {
        EditorGUILayout.LabelField(UIElementsDebugger.Styles.stylesheetsContent, UIElementsDebugger.Styles.KInspectorTitle, new GUILayoutOption[0]);
        foreach (string elementStylesheet in UIElementsDebugger.s_MatchedRulesExtractor.selectedElementStylesheets)
        {
          if (GUILayout.Button(elementStylesheet))
            InternalEditorUtility.OpenFileAtLineExternal(elementStylesheet, 0);
        }
      }
      if (UIElementsDebugger.s_MatchedRulesExtractor.selectedElementRules == null || UIElementsDebugger.s_MatchedRulesExtractor.selectedElementRules.Count <= 0)
        return;
      EditorGUILayout.LabelField(UIElementsDebugger.Styles.selectorsContent, UIElementsDebugger.Styles.KInspectorTitle, new GUILayoutOption[0]);
      int num = 0;
      foreach (MatchedRulesExtractor.MatchedRule selectedElementRule in UIElementsDebugger.s_MatchedRulesExtractor.selectedElementRules)
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index1 = 0; index1 < selectedElementRule.ruleMatcher.complexSelector.selectors.Length; ++index1)
        {
          StyleSelector selector = selectedElementRule.ruleMatcher.complexSelector.selectors[index1];
          switch (selector.previousRelationship)
          {
            case StyleSelectorRelationship.Child:
              stringBuilder.Append(" ");
              break;
            case StyleSelectorRelationship.Descendent:
              stringBuilder.Append(" > ");
              break;
          }
          for (int index2 = 0; index2 < selector.parts.Length; ++index2)
          {
            StyleSelectorPart part = selector.parts[index2];
            switch (part.type)
            {
              case StyleSelectorType.Class:
                stringBuilder.Append(".");
                break;
              case StyleSelectorType.PseudoClass:
              case StyleSelectorType.RecursivePseudoClass:
                stringBuilder.Append(":");
                break;
              case StyleSelectorType.ID:
                stringBuilder.Append("#");
                break;
            }
            stringBuilder.Append(part.value);
          }
        }
        StyleProperty[] properties = selectedElementRule.ruleMatcher.complexSelector.rule.properties;
        bool flag1 = this.m_CurFoldout.Contains(num);
        EditorGUILayout.BeginHorizontal();
        bool flag2 = EditorGUILayout.Foldout(this.m_CurFoldout.Contains(num), new GUIContent(stringBuilder.ToString()), true);
        if (selectedElementRule.displayPath != null)
        {
          if (GUILayout.Button(selectedElementRule.displayPath, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.MaxWidth(150f) }))
            InternalEditorUtility.OpenFileAtLineExternal(selectedElementRule.fullPath, selectedElementRule.lineNumber);
        }
        EditorGUILayout.EndHorizontal();
        if (flag1 && !flag2)
          this.m_CurFoldout.Remove(num);
        else if (!flag1 && flag2)
          this.m_CurFoldout.Add(num);
        if (flag2)
        {
          ++EditorGUI.indentLevel;
          for (int index = 0; index < properties.Length; ++index)
          {
            string text = selectedElementRule.ruleMatcher.sheet.ReadAsString(properties[index].values[0]);
            EditorGUILayout.LabelField(new GUIContent(properties[index].name), new GUIContent(text), new GUILayoutOption[0]);
          }
          --EditorGUI.indentLevel;
        }
        ++num;
      }
    }

    private void DrawSize(Rect rect, VisualElement element)
    {
      Rect rect1 = new Rect(rect);
      rect1.x += 35f;
      rect1.y += 35f;
      rect1.width -= 70f;
      rect1.height -= 70f;
      this.DrawRect(rect1, 3f, UIElementsDebugger.Styles.kSizeMarginPrimaryColor, UIElementsDebugger.Styles.kSizeMarginSecondaryColor);
      UIElementsDebugger.DrawSizeLabels(rect1, UIElementsDebugger.Styles.marginContent, (float) element.style.marginTop, (float) element.style.marginRight, (float) element.style.marginBottom, (float) element.style.marginLeft);
      rect1.x += 35f;
      rect1.y += 35f;
      rect1.width -= 70f;
      rect1.height -= 70f;
      this.DrawRect(rect1, 3f, UIElementsDebugger.Styles.kSizeBorderPrimaryColor, UIElementsDebugger.Styles.kSizeBorderSecondaryColor);
      UIElementsDebugger.DrawSizeLabels(rect1, UIElementsDebugger.Styles.borderContent, (float) element.style.borderTop, (float) element.style.borderRight, (float) element.style.borderBottom, (float) element.style.borderLeft);
      rect1.x += 35f;
      rect1.y += 35f;
      rect1.width -= 70f;
      rect1.height -= 70f;
      this.DrawRect(rect1, 3f, UIElementsDebugger.Styles.kSizePaddingPrimaryColor, UIElementsDebugger.Styles.kSizePaddingSecondaryColor);
      UIElementsDebugger.DrawSizeLabels(rect1, UIElementsDebugger.Styles.paddingContent, (float) element.style.paddingTop, (float) element.style.paddingRight, (float) element.style.paddingBottom, (float) element.style.paddingLeft);
      rect1.x += 35f;
      rect1.y += 35f;
      rect1.width -= 70f;
      rect1.height -= 70f;
      this.DrawRect(rect1, 3f, UIElementsDebugger.Styles.kSizePrimaryColor, UIElementsDebugger.Styles.kSizeSecondaryColor);
      EditorGUI.LabelField(rect1, string.Format("{0:F2} x {1:F2}", (object) element.layout.width, (object) element.layout.height), UIElementsDebugger.Styles.KSizeLabel);
    }

    private static void DrawSizeLabels(Rect cursor, GUIContent label, float top, float right, float bottom, float left)
    {
      Rect position = new Rect(cursor.x + (float) (((double) cursor.width - 50.0) * 0.5), cursor.y + 2f, 50f, EditorGUIUtility.singleLineHeight);
      EditorGUI.LabelField(position, top.ToString("F2"), UIElementsDebugger.Styles.KSizeLabel);
      position.y = (float) ((double) cursor.y + (double) cursor.height + 2.0 - 35.0);
      EditorGUI.LabelField(position, bottom.ToString("F2"), UIElementsDebugger.Styles.KSizeLabel);
      position.x = cursor.x;
      position.y = cursor.y + (float) (((double) cursor.height - (double) EditorGUIUtility.singleLineHeight) * 0.5);
      EditorGUI.LabelField(position, left.ToString("F2"), UIElementsDebugger.Styles.KSizeLabel);
      position.x = (float) ((double) cursor.x + (double) cursor.width - 35.0 - 4.0);
      EditorGUI.LabelField(position, right.ToString("F2"), UIElementsDebugger.Styles.KSizeLabel);
      position.x = cursor.x + 2f;
      position.y = cursor.y + 2f;
      position.width = 50f;
      position.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.LabelField(position, label, UIElementsDebugger.Styles.KSizeLabel);
    }

    public void OnEnable()
    {
      this.m_PickingData = new PickingData();
      this.titleContent = new GUIContent("UIElements Debugger");
      this.m_VisualTreeTreeViewState = new TreeViewState();
      this.m_VisualTreeTreeView = new VisualTreeTreeView(this.m_VisualTreeTreeViewState);
      if (this.m_SplitterState == null)
        this.m_SplitterState = new SplitterState(new float[2]
        {
          1f,
          2f
        });
      this.m_TempTexture = new Texture2D(2, 2);
      if (string.IsNullOrEmpty(this.m_LastWindowTitle))
        return;
      this.m_ScheduleRestoreSelection = true;
    }

    public void OnDisable()
    {
      Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
      while (panelsIterator.MoveNext())
        panelsIterator.Current.Value.panelDebug = (BasePanelDebug) null;
    }

    private void DrawRect(Rect rect, float borderSize, Color borderColor, Color fillingColor)
    {
      this.m_TempTexture.SetPixel(0, 0, fillingColor);
      this.m_TempTexture.SetPixel(1, 0, fillingColor);
      this.m_TempTexture.SetPixel(0, 1, fillingColor);
      this.m_TempTexture.SetPixel(1, 1, fillingColor);
      this.m_TempTexture.Apply();
      GUI.DrawTexture(rect, (Texture) this.m_TempTexture);
      this.m_TempTexture.SetPixel(0, 0, borderColor);
      this.m_TempTexture.SetPixel(1, 0, borderColor);
      this.m_TempTexture.SetPixel(0, 1, borderColor);
      this.m_TempTexture.SetPixel(1, 1, borderColor);
      this.m_TempTexture.Apply();
      Rect position = new Rect(rect.x, rect.y, rect.width, borderSize);
      GUI.DrawTexture(position, (Texture) this.m_TempTexture);
      position.y = rect.y + rect.height - borderSize;
      GUI.DrawTexture(position, (Texture) this.m_TempTexture);
      position.width = borderSize;
      position.height = rect.height;
      position.y = rect.y;
      GUI.DrawTexture(position, (Texture) this.m_TempTexture);
      position.x = rect.x + rect.width - borderSize;
      GUI.DrawTexture(position, (Texture) this.m_TempTexture);
    }

    internal struct ViewPanel
    {
      public GUIView View;
      public Panel Panel;
    }

    internal static class Styles
    {
      public static GUIStyle KSizeLabel = new GUIStyle() { alignment = TextAnchor.MiddleCenter };
      public static GUIStyle KInspectorTitle = new GUIStyle(EditorStyles.whiteLargeLabel) { alignment = TextAnchor.MiddleCenter };
      public static readonly GUIContent elementStylesContent = new GUIContent("Element styles");
      public static readonly GUIContent showDefaultsContent = new GUIContent("Show defaults");
      public static readonly GUIContent sortContent = new GUIContent("Sort");
      public static readonly GUIContent inlineContent = new GUIContent("INLINE");
      public static readonly GUIContent marginContent = new GUIContent("Margin");
      public static readonly GUIContent borderContent = new GUIContent("Border");
      public static readonly GUIContent paddingContent = new GUIContent("Padding");
      public static readonly GUIContent cancelPickingContent = new GUIContent("Cancel picking");
      public static readonly GUIContent pickPanelContent = new GUIContent("Pick Panel");
      public static readonly GUIContent pickElementInPanelContent = new GUIContent("Pick Element in panel");
      public static readonly GUIContent overlayContent = new GUIContent("Overlay");
      public static readonly GUIContent uxmlContent = new GUIContent("UXML Dump");
      public static readonly GUIContent stylesheetsContent = new GUIContent("Stylesheets");
      public static readonly GUIContent selectorsContent = new GUIContent("Matching Selectors");
      public static readonly GUIContent includeShadowHierarchyContent = new GUIContent("Include Shadow Hierarchy");
      private static readonly Color k_SeparatorColorPro = new Color(0.15f, 0.15f, 0.15f);
      private static readonly Color k_SeparatorColorNonPro = new Color(0.6f, 0.6f, 0.6f);
      internal static readonly Color kSizeMarginPrimaryColor = new Color(0.0f, 0.0f, 0.0f);
      internal static readonly Color kSizeMarginSecondaryColor = new Color(0.9764706f, 0.8f, 0.6156863f);
      internal static readonly Color kSizeBorderPrimaryColor = new Color(0.0f, 0.0f, 0.0f);
      internal static readonly Color kSizeBorderSecondaryColor = new Color(0.9921569f, 0.8666667f, 0.6078432f);
      internal static readonly Color kSizePaddingPrimaryColor = new Color(0.0f, 0.0f, 0.0f);
      internal static readonly Color kSizePaddingSecondaryColor = new Color(0.7607843f, 0.9294118f, 0.5411765f);
      internal static readonly Color kSizePrimaryColor = new Color(0.0f, 0.0f, 0.0f);
      internal static readonly Color kSizeSecondaryColor = new Color(0.5450981f, 0.7098039f, 0.7529412f);
      internal const float LabelSizeSize = 50f;
      internal const float SizeRectLineSize = 3f;
      internal const float SizeRectBetweenSize = 35f;
      internal const float SizeRectHeight = 350f;
      internal const float SplitterSize = 2f;
      internal const float LabelWidth = 150f;

      public static Color separatorColor
      {
        get
        {
          return !EditorGUIUtility.isProSkin ? UIElementsDebugger.Styles.k_SeparatorColorNonPro : UIElementsDebugger.Styles.k_SeparatorColorPro;
        }
      }
    }
  }
}
