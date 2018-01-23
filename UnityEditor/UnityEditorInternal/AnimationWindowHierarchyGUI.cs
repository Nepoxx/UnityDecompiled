// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyGUI : TreeViewGUI
  {
    private static readonly Color k_KeyColorInDopesheetMode = new Color(0.7f, 0.7f, 0.7f, 1f);
    private static readonly Color k_KeyColorForNonCurves = new Color(0.7f, 0.7f, 0.7f, 0.5f);
    private static readonly Color k_LeftoverCurveColor = Color.yellow;
    internal static int s_WasInsideValueRectFrame = -1;
    private readonly GUIContent k_AnimatePropertyLabel = new GUIContent("Add Property");
    private Color m_LightSkinPropertyTextColor = new Color(0.35f, 0.35f, 0.35f);
    private Color m_PhantomCurveColor = new Color(0.0f, 0.6f, 0.6f);
    private GUIStyle m_AnimationRowEvenStyle;
    private GUIStyle m_AnimationRowOddStyle;
    private GUIStyle m_AnimationSelectionTextField;
    private GUIStyle m_AnimationLineStyle;
    private GUIStyle m_AnimationCurveDropdown;
    private AnimationWindowHierarchyNode m_RenamedNode;
    private int[] m_HierarchyItemFoldControlIDs;
    private int[] m_HierarchyItemValueControlIDs;
    private int[] m_HierarchyItemButtonControlIDs;
    private const float k_RowRightOffset = 10f;
    private const float k_ValueFieldDragWidth = 15f;
    private const float k_ValueFieldWidth = 50f;
    private const float k_ValueFieldOffsetFromRightSide = 75f;
    private const float k_ColorIndicatorTopMargin = 3f;
    public const float k_DopeSheetRowHeight = 16f;
    public const float k_DopeSheetRowHeightTall = 32f;
    public const float k_AddCurveButtonNodeHeight = 40f;
    public const float k_RowBackgroundColorBrightness = 0.28f;
    private const float k_SelectedPhantomCurveColorMultiplier = 1.4f;

    public AnimationWindowHierarchyGUI(TreeViewController treeView, AnimationWindowState state)
      : base(treeView)
    {
      this.state = state;
      this.InitStyles();
    }

    public AnimationWindowState state { get; set; }

    protected void InitStyles()
    {
      if (this.m_AnimationRowEvenStyle == null)
        this.m_AnimationRowEvenStyle = new GUIStyle((GUIStyle) "AnimationRowEven");
      if (this.m_AnimationRowOddStyle == null)
        this.m_AnimationRowOddStyle = new GUIStyle((GUIStyle) "AnimationRowOdd");
      if (this.m_AnimationSelectionTextField == null)
        this.m_AnimationSelectionTextField = new GUIStyle((GUIStyle) "AnimationSelectionTextField");
      if (this.m_AnimationLineStyle == null)
      {
        this.m_AnimationLineStyle = new GUIStyle(TreeViewGUI.Styles.lineStyle);
        this.m_AnimationLineStyle.padding.left = 0;
      }
      if (this.m_AnimationCurveDropdown != null)
        return;
      this.m_AnimationCurveDropdown = new GUIStyle((GUIStyle) "AnimPropDropdown");
    }

    protected void DoNodeGUI(Rect rect, AnimationWindowHierarchyNode node, bool selected, bool focused, int row)
    {
      this.InitStyles();
      if (node is AnimationWindowHierarchyMasterNode)
        return;
      float indent = this.k_BaseIndent + (float) (node.depth + node.indent) * this.k_IndentWidth;
      if (node is AnimationWindowHierarchyAddButtonNode)
      {
        if (Event.current.type == EventType.MouseMove && AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame >= 0)
        {
          if (AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame >= Time.frameCount - 1)
            Event.current.Use();
          else
            AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame = -1;
        }
        using (new EditorGUI.DisabledScope(!this.state.selection.canAddCurves))
          this.DoAddCurveButton(rect, node, row);
      }
      else
      {
        this.DoRowBackground(rect, row);
        this.DoIconAndName(rect, node, selected, focused, indent);
        this.DoFoldout(node, rect, indent, row);
        bool enabled = false;
        if (node.curves != null)
          enabled = !Array.Exists<AnimationWindowCurve>(node.curves, (Predicate<AnimationWindowCurve>) (curve => !curve.animationIsEditable));
        using (new EditorGUI.DisabledScope(!enabled))
          this.DoValueField(rect, node, row);
        this.DoCurveDropdown(rect, node, row, enabled);
        this.HandleContextMenu(rect, node, enabled);
        this.DoCurveColorIndicator(rect, node);
      }
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    public override void BeginRowGUI()
    {
      base.BeginRowGUI();
      this.HandleDelete();
      int rowCount = this.m_TreeView.data.rowCount;
      this.m_HierarchyItemFoldControlIDs = new int[rowCount];
      this.m_HierarchyItemValueControlIDs = new int[rowCount];
      this.m_HierarchyItemButtonControlIDs = new int[rowCount];
      for (int row = 0; row < rowCount; ++row)
      {
        AnimationWindowHierarchyPropertyNode hierarchyPropertyNode = this.m_TreeView.data.GetItem(row) as AnimationWindowHierarchyPropertyNode;
        this.m_HierarchyItemValueControlIDs[row] = hierarchyPropertyNode == null || hierarchyPropertyNode.isPptrNode ? 0 : GUIUtility.GetControlID(FocusType.Keyboard);
        this.m_HierarchyItemFoldControlIDs[row] = GUIUtility.GetControlID(FocusType.Passive);
        this.m_HierarchyItemButtonControlIDs[row] = GUIUtility.GetControlID(FocusType.Passive);
      }
    }

    private void DoAddCurveButton(Rect rect, AnimationWindowHierarchyNode node, int row)
    {
      float num1 = (float) (((double) rect.width - 230.0) / 2.0);
      float num2 = 10f;
      Rect rect1 = new Rect(rect.xMin + num1, rect.yMin + num2, rect.width - num1 * 2f, rect.height - num2 * 2f);
      if (!this.DoTreeViewButton(this.m_HierarchyItemButtonControlIDs[row], rect1, this.k_AnimatePropertyLabel, GUI.skin.button))
        return;
      AddCurvesPopup.selection = this.state.selection;
      AddCurvesPopupHierarchyDataSource.showEntireHierarchy = true;
      if (AddCurvesPopup.ShowAtPosition(rect1, this.state, new AddCurvesPopup.OnNewCurveAdded(this.OnNewCurveAdded)))
        GUIUtility.ExitGUI();
    }

    private void OnNewCurveAdded(AddCurvesPopupPropertyNode node)
    {
    }

    private void DoRowBackground(Rect rect, int row)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (row % 2 == 0)
        this.m_AnimationRowEvenStyle.Draw(rect, false, false, false, false);
      else
        this.m_AnimationRowOddStyle.Draw(rect, false, false, false, false);
    }

    private void DoFoldout(AnimationWindowHierarchyNode node, Rect rect, float indent, int row)
    {
      if (this.m_TreeView.data.IsExpandable((TreeViewItem) node))
      {
        Rect position = rect;
        position.x = indent;
        position.width = TreeViewGUI.Styles.foldoutWidth;
        EditorGUI.BeginChangeCheck();
        bool expand = GUI.Toggle(position, this.m_HierarchyItemFoldControlIDs[row], this.m_TreeView.data.IsExpanded((TreeViewItem) node), GUIContent.none, TreeViewGUI.Styles.foldout);
        if (!EditorGUI.EndChangeCheck())
          return;
        if (Event.current.alt)
          this.m_TreeView.data.SetExpandedWithChildren((TreeViewItem) node, expand);
        else
          this.m_TreeView.data.SetExpanded((TreeViewItem) node, expand);
      }
      else
      {
        AnimationWindowHierarchyPropertyNode hierarchyPropertyNode = node as AnimationWindowHierarchyPropertyNode;
        AnimationWindowHierarchyState state = this.m_TreeView.state as AnimationWindowHierarchyState;
        if (hierarchyPropertyNode != null && hierarchyPropertyNode.isPptrNode)
        {
          Rect position = rect;
          position.x = indent;
          position.width = TreeViewGUI.Styles.foldoutWidth;
          EditorGUI.BeginChangeCheck();
          bool tallMode1 = state.GetTallMode((AnimationWindowHierarchyNode) hierarchyPropertyNode);
          bool tallMode2 = GUI.Toggle(position, this.m_HierarchyItemFoldControlIDs[row], tallMode1, GUIContent.none, TreeViewGUI.Styles.foldout);
          if (EditorGUI.EndChangeCheck())
            state.SetTallMode((AnimationWindowHierarchyNode) hierarchyPropertyNode, tallMode2);
        }
      }
    }

    private void DoIconAndName(Rect rect, AnimationWindowHierarchyNode node, bool selected, bool focused, float indent)
    {
      EditorGUIUtility.SetIconSize(new Vector2(13f, 13f));
      if (Event.current.type == EventType.Repaint)
      {
        if (selected)
          TreeViewGUI.Styles.selectionStyle.Draw(rect, false, false, true, focused);
        if (node is AnimationWindowHierarchyPropertyNode)
          rect.width -= 77f;
        bool flag1 = AnimationWindowUtility.IsNodeLeftOverCurve(node);
        bool flag2 = AnimationWindowUtility.IsNodeAmbiguous(node);
        bool flag3 = AnimationWindowUtility.IsNodePhantom(node);
        string str1 = "";
        string tooltip = "";
        if (flag3)
        {
          str1 = " (Default Value)";
          tooltip = "Transform position, rotation and scale can't be partially animated. This value will be animated to the default value";
        }
        if (flag1)
        {
          str1 = " (Missing!)";
          tooltip = "The GameObject or Component is missing (" + node.path + ")";
        }
        if (flag2)
        {
          str1 = " (Duplicate GameObject name!)";
          tooltip = "Target for curve is ambiguous since there are multiple GameObjects with same name (" + node.path + ")";
        }
        Color textColor = this.m_AnimationLineStyle.normal.textColor;
        Color color1;
        if (node.depth == 0)
        {
          string str2 = "";
          if (node.curves.Length > 0)
          {
            AnimationWindowSelectionItem selectionBinding = node.curves[0].selectionBinding;
            if ((UnityEngine.Object) selectionBinding != (UnityEngine.Object) null && (UnityEngine.Object) selectionBinding.rootGameObject != (UnityEngine.Object) null && (UnityEngine.Object) selectionBinding.rootGameObject.transform.Find(node.path) == (UnityEngine.Object) null)
              flag1 = true;
            string gameObjectName = this.GetGameObjectName(!((UnityEngine.Object) selectionBinding != (UnityEngine.Object) null) ? (GameObject) null : selectionBinding.rootGameObject, node.path);
            str2 = !string.IsNullOrEmpty(gameObjectName) ? gameObjectName + " : " : "";
          }
          TreeViewGUI.Styles.content = new GUIContent(str2 + node.displayName + str1, this.GetIconForItem((TreeViewItem) node), tooltip);
          color1 = !EditorGUIUtility.isProSkin ? Color.black : Color.gray * 1.35f;
        }
        else
        {
          TreeViewGUI.Styles.content = new GUIContent(node.displayName + str1, this.GetIconForItem((TreeViewItem) node), tooltip);
          Color color2 = !EditorGUIUtility.isProSkin ? this.m_LightSkinPropertyTextColor : Color.gray;
          Color color3 = !selected ? this.m_PhantomCurveColor : this.m_PhantomCurveColor * 1.4f;
          color1 = !flag3 ? color2 : color3;
        }
        this.SetStyleTextColor(this.m_AnimationLineStyle, flag1 || flag2 ? AnimationWindowHierarchyGUI.k_LeftoverCurveColor : color1);
        rect.xMin += (float) (int) ((double) indent + (double) TreeViewGUI.Styles.foldoutWidth + (double) this.m_AnimationLineStyle.margin.left);
        GUI.Label(rect, TreeViewGUI.Styles.content, this.m_AnimationLineStyle);
        this.SetStyleTextColor(this.m_AnimationLineStyle, textColor);
      }
      if (!this.IsRenaming(node.id) || Event.current.type == EventType.Layout)
        return;
      this.GetRenameOverlay().editFieldRect = new Rect(rect.x + this.k_IndentWidth, rect.y, (float) ((double) rect.width - (double) this.k_IndentWidth - 1.0), rect.height);
    }

    private string GetGameObjectName(GameObject rootGameObject, string path)
    {
      if (string.IsNullOrEmpty(path))
        return !((UnityEngine.Object) rootGameObject != (UnityEngine.Object) null) ? "" : rootGameObject.name;
      string[] strArray = path.Split('/');
      return strArray[strArray.Length - 1];
    }

    private string GetPathWithoutChildmostGameObject(string path)
    {
      if (string.IsNullOrEmpty(path))
        return "";
      int num = path.LastIndexOf('/');
      return path.Substring(0, num + 1);
    }

    private void DoValueField(Rect rect, AnimationWindowHierarchyNode node, int row)
    {
      bool flag1 = false;
      if (node is AnimationWindowHierarchyPropertyNode)
      {
        AnimationWindowCurve[] curves = node.curves;
        if (curves == null || curves.Length == 0)
          return;
        AnimationWindowCurve curve = curves[0];
        object currentValue = CurveBindingUtility.GetCurrentValue(this.state, curve);
        if (currentValue is float)
        {
          float num1 = (float) currentValue;
          Rect dragHotZone = new Rect((float) ((double) rect.xMax - 75.0 - 15.0), rect.y, 15f, rect.height);
          Rect position = new Rect(rect.xMax - 75f, rect.y, 50f, rect.height);
          if (Event.current.type == EventType.MouseMove && position.Contains(Event.current.mousePosition))
            AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame = Time.frameCount;
          EditorGUI.BeginChangeCheck();
          float f;
          if (curve.valueType == typeof (bool))
          {
            f = !GUI.Toggle(position, this.m_HierarchyItemValueControlIDs[row], (double) num1 != 0.0, GUIContent.none, EditorStyles.toggle) ? 0.0f : 1f;
          }
          else
          {
            int itemValueControlId = this.m_HierarchyItemValueControlIDs[row];
            bool flag2 = GUIUtility.keyboardControl == itemValueControlId && EditorGUIUtility.editingTextField && Event.current.type == EventType.KeyDown && ((int) Event.current.character == 10 || (int) Event.current.character == 3);
            if (EditorGUI.s_RecycledEditor.controlID == itemValueControlId && Event.current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition))
              GUIUtility.keyboardControl = itemValueControlId;
            f = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, dragHotZone, itemValueControlId, num1, EditorGUI.kFloatFieldFormatString, this.m_AnimationSelectionTextField, true);
            if (flag2)
            {
              GUI.changed = true;
              Event.current.Use();
            }
          }
          if (float.IsInfinity(f) || float.IsNaN(f))
            f = 0.0f;
          if (EditorGUI.EndChangeCheck())
          {
            string undoLabel = "Edit Key";
            float num2 = this.state.currentTime - curve.timeOffset;
            AnimationKeyTime time = AnimationKeyTime.Time(num2, curve.clip.frameRate);
            AnimationWindowKeyframe animationWindowKeyframe = (AnimationWindowKeyframe) null;
            foreach (AnimationWindowKeyframe keyframe in curve.m_Keyframes)
            {
              if (Mathf.Approximately(keyframe.time, num2))
                animationWindowKeyframe = keyframe;
            }
            if (animationWindowKeyframe == null)
              AnimationWindowUtility.AddKeyframeToCurve(curve, (object) f, curve.valueType, time);
            else
              animationWindowKeyframe.value = (object) f;
            this.state.SaveCurve(curve, undoLabel);
            flag1 = true;
          }
        }
      }
      if (!flag1)
        return;
      this.state.ResampleAnimation();
    }

    private bool DoTreeViewButton(int id, Rect position, GUIContent content, GUIStyle style)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition) && current.button == 0)
          {
            GUIUtility.hotControl = id;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            if (position.Contains(current.mousePosition))
              return true;
            break;
          }
          break;
        case EventType.Repaint:
          style.Draw(position, content, id, false);
          break;
      }
      return false;
    }

    private void DoCurveDropdown(Rect rect, AnimationWindowHierarchyNode node, int row, bool enabled)
    {
      rect = new Rect((float) ((double) rect.xMax - 10.0 - 12.0), rect.yMin + 2f, 22f, 12f);
      if (!this.DoTreeViewButton(this.m_HierarchyItemButtonControlIDs[row], rect, GUIContent.none, this.m_AnimationCurveDropdown))
        return;
      this.state.SelectHierarchyItem(node.id, false, false);
      this.GenerateMenu(((IEnumerable<AnimationWindowHierarchyNode>) new AnimationWindowHierarchyNode[1]
      {
        node
      }).ToList<AnimationWindowHierarchyNode>(), enabled).DropDown(rect);
      Event.current.Use();
    }

    private void DoCurveColorIndicator(Rect rect, AnimationWindowHierarchyNode node)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      GUI.color = this.state.showCurveEditor ? (node.curves.Length != 1 || node.curves[0].isPPtrCurve ? AnimationWindowHierarchyGUI.k_KeyColorForNonCurves : CurveUtility.GetPropertyColor(node.curves[0].binding.propertyName)) : AnimationWindowHierarchyGUI.k_KeyColorInDopesheetMode;
      bool flag = false;
      if (this.state.previewing)
      {
        foreach (AnimationWindowCurve curve in node.curves)
        {
          if (curve.m_Keyframes.Any<AnimationWindowKeyframe>((Func<AnimationWindowKeyframe, bool>) (key => this.state.time.ContainsTime(key.time))))
            flag = true;
        }
      }
      Texture image = !flag ? (Texture) CurveUtility.GetIconCurve() : (Texture) CurveUtility.GetIconKey();
      rect = new Rect((float) ((double) rect.xMax - 10.0 - (double) (image.width / 2) - 5.0), rect.yMin + 3f, (float) image.width, (float) image.height);
      GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit, true, 1f);
      GUI.color = color;
    }

    private void HandleDelete()
    {
      if (!this.m_TreeView.HasFocus())
        return;
      switch (Event.current.type)
      {
        case EventType.KeyDown:
          if (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete)
          {
            this.RemoveCurvesFromSelectedNodes();
            Event.current.Use();
            break;
          }
          break;
        case EventType.ExecuteCommand:
          if (Event.current.commandName == "SoftDelete" || Event.current.commandName == "Delete")
          {
            if (Event.current.type == EventType.ExecuteCommand)
              this.RemoveCurvesFromSelectedNodes();
            Event.current.Use();
            break;
          }
          break;
      }
    }

    private void HandleContextMenu(Rect rect, AnimationWindowHierarchyNode node, bool enabled)
    {
      if (Event.current.type != EventType.ContextClick || !rect.Contains(Event.current.mousePosition))
        return;
      this.state.SelectHierarchyItem(node.id, true, true);
      this.GenerateMenu(this.state.selectedHierarchyNodes, enabled).ShowAsContext();
      Event.current.Use();
    }

    private GenericMenu GenerateMenu(List<AnimationWindowHierarchyNode> interactedNodes, bool enabled)
    {
      List<AnimationWindowCurve> curvesAffectedByNodes1 = this.GetCurvesAffectedByNodes(interactedNodes, false);
      List<AnimationWindowCurve> curvesAffectedByNodes2 = this.GetCurvesAffectedByNodes(interactedNodes, true);
      bool flag1 = curvesAffectedByNodes1.Count == 1 && AnimationWindowUtility.ForceGrouping(curvesAffectedByNodes1[0].binding);
      GenericMenu genericMenu = new GenericMenu();
      GUIContent content = new GUIContent(curvesAffectedByNodes1.Count > 1 || flag1 ? "Remove Properties" : "Remove Property");
      if (!enabled)
        genericMenu.AddDisabledItem(content);
      else
        genericMenu.AddItem(content, false, new GenericMenu.MenuFunction(this.RemoveCurvesFromSelectedNodes));
      bool flag2 = true;
      EditorCurveBinding[] curves = new EditorCurveBinding[curvesAffectedByNodes2.Count];
      for (int index = 0; index < curvesAffectedByNodes2.Count; ++index)
        curves[index] = curvesAffectedByNodes2[index].binding;
      RotationCurveInterpolation.Mode interpolationMode = this.GetRotationInterpolationMode(curves);
      if (interpolationMode == RotationCurveInterpolation.Mode.Undefined)
      {
        flag2 = false;
      }
      else
      {
        foreach (AnimationWindowHierarchyNode interactedNode in interactedNodes)
        {
          if (!(interactedNode is AnimationWindowHierarchyPropertyGroupNode))
            flag2 = false;
        }
      }
      if (flag2)
      {
        string str = !this.state.activeAnimationClip.legacy ? "" : " (Not fully supported in Legacy)";
        GenericMenu.MenuFunction2 menuFunction2 = (GenericMenu.MenuFunction2) null;
        genericMenu.AddItem(new GUIContent("Interpolation/Euler Angles" + str), interpolationMode == RotationCurveInterpolation.Mode.RawEuler, !enabled ? menuFunction2 : new GenericMenu.MenuFunction2(this.ChangeRotationInterpolation), (object) RotationCurveInterpolation.Mode.RawEuler);
        genericMenu.AddItem(new GUIContent("Interpolation/Euler Angles (Quaternion)"), interpolationMode == RotationCurveInterpolation.Mode.Baked, !enabled ? menuFunction2 : new GenericMenu.MenuFunction2(this.ChangeRotationInterpolation), (object) RotationCurveInterpolation.Mode.Baked);
        genericMenu.AddItem(new GUIContent("Interpolation/Quaternion"), interpolationMode == RotationCurveInterpolation.Mode.NonBaked, !enabled ? menuFunction2 : new GenericMenu.MenuFunction2(this.ChangeRotationInterpolation), (object) RotationCurveInterpolation.Mode.NonBaked);
      }
      if (this.state.previewing)
      {
        genericMenu.AddSeparator("");
        bool flag3 = true;
        bool flag4 = true;
        foreach (AnimationWindowCurve animationWindowCurve in curvesAffectedByNodes1)
        {
          if (!animationWindowCurve.HasKeyframe(this.state.time))
            flag3 = false;
          else
            flag4 = false;
        }
        string text1 = "Add Key";
        if (flag3 || !enabled)
          genericMenu.AddDisabledItem(new GUIContent(text1));
        else
          genericMenu.AddItem(new GUIContent(text1), false, new GenericMenu.MenuFunction2(this.AddKeysAtCurrentTime), (object) curvesAffectedByNodes1);
        string text2 = "Delete Key";
        if (flag4 || !enabled)
          genericMenu.AddDisabledItem(new GUIContent(text2));
        else
          genericMenu.AddItem(new GUIContent(text2), false, new GenericMenu.MenuFunction2(this.DeleteKeysAtCurrentTime), (object) curvesAffectedByNodes1);
      }
      return genericMenu;
    }

    private void AddKeysAtCurrentTime(object obj)
    {
      this.AddKeysAtCurrentTime((List<AnimationWindowCurve>) obj);
    }

    private void AddKeysAtCurrentTime(List<AnimationWindowCurve> curves)
    {
      AnimationWindowUtility.AddKeyframes(this.state, curves.ToArray(), this.state.time);
    }

    private void DeleteKeysAtCurrentTime(object obj)
    {
      this.DeleteKeysAtCurrentTime((List<AnimationWindowCurve>) obj);
    }

    private void DeleteKeysAtCurrentTime(List<AnimationWindowCurve> curves)
    {
      AnimationWindowUtility.RemoveKeyframes(this.state, curves.ToArray(), this.state.time);
    }

    private void ChangeRotationInterpolation(object interpolationMode)
    {
      RotationCurveInterpolation.Mode mode = (RotationCurveInterpolation.Mode) interpolationMode;
      AnimationWindowCurve[] array = this.state.activeCurves.ToArray();
      EditorCurveBinding[] curveBindings = new EditorCurveBinding[array.Length];
      for (int index = 0; index < array.Length; ++index)
        curveBindings[index] = array[index].binding;
      RotationCurveInterpolation.SetInterpolation(this.state.activeAnimationClip, curveBindings, mode);
      this.MaintainTreeviewStateAfterRotationInterpolation(mode);
      this.state.hierarchyData.ReloadData();
    }

    private void RemoveCurvesFromSelectedNodes()
    {
      this.RemoveCurvesFromNodes(this.state.selectedHierarchyNodes);
    }

    private void RemoveCurvesFromNodes(List<AnimationWindowHierarchyNode> nodes)
    {
      string undoLabel = "Remove Curve";
      this.state.SaveKeySelection(undoLabel);
      foreach (AnimationWindowHierarchyNode node in nodes)
      {
        AnimationWindowHierarchyNode windowHierarchyNode = node;
        if (windowHierarchyNode.parent is AnimationWindowHierarchyPropertyGroupNode && windowHierarchyNode.binding.HasValue && AnimationWindowUtility.ForceGrouping(windowHierarchyNode.binding.Value))
          windowHierarchyNode = (AnimationWindowHierarchyNode) windowHierarchyNode.parent;
        if (windowHierarchyNode.curves != null)
        {
          foreach (AnimationWindowCurve curve in windowHierarchyNode is AnimationWindowHierarchyPropertyGroupNode || windowHierarchyNode is AnimationWindowHierarchyPropertyNode ? AnimationWindowUtility.FilterCurves(((IEnumerable<AnimationWindowCurve>) windowHierarchyNode.curves).ToArray<AnimationWindowCurve>(), windowHierarchyNode.path, windowHierarchyNode.animatableObjectType, windowHierarchyNode.propertyName) : AnimationWindowUtility.FilterCurves(((IEnumerable<AnimationWindowCurve>) windowHierarchyNode.curves).ToArray<AnimationWindowCurve>(), windowHierarchyNode.path, windowHierarchyNode.animatableObjectType))
            this.state.RemoveCurve(curve, undoLabel);
        }
      }
      this.m_TreeView.ReloadData();
      this.state.controlInterface.ResampleAnimation();
    }

    private List<AnimationWindowCurve> GetCurvesAffectedByNodes(List<AnimationWindowHierarchyNode> nodes, bool includeLinkedCurves)
    {
      List<AnimationWindowCurve> source = new List<AnimationWindowCurve>();
      foreach (AnimationWindowHierarchyNode node in nodes)
      {
        AnimationWindowHierarchyNode windowHierarchyNode = node;
        if (windowHierarchyNode.parent is AnimationWindowHierarchyPropertyGroupNode && includeLinkedCurves)
          windowHierarchyNode = (AnimationWindowHierarchyNode) windowHierarchyNode.parent;
        if (windowHierarchyNode.curves != null && windowHierarchyNode.curves.Length > 0)
        {
          if (windowHierarchyNode is AnimationWindowHierarchyPropertyGroupNode || windowHierarchyNode is AnimationWindowHierarchyPropertyNode)
            source.AddRange((IEnumerable<AnimationWindowCurve>) AnimationWindowUtility.FilterCurves(windowHierarchyNode.curves, windowHierarchyNode.path, windowHierarchyNode.animatableObjectType, windowHierarchyNode.propertyName));
          else
            source.AddRange((IEnumerable<AnimationWindowCurve>) AnimationWindowUtility.FilterCurves(windowHierarchyNode.curves, windowHierarchyNode.path, windowHierarchyNode.animatableObjectType));
        }
      }
      return source.Distinct<AnimationWindowCurve>().ToList<AnimationWindowCurve>();
    }

    private void MaintainTreeviewStateAfterRotationInterpolation(RotationCurveInterpolation.Mode newMode)
    {
      List<int> selectedIds = this.state.hierarchyState.selectedIDs;
      List<int> expandedIds = this.state.hierarchyState.expandedIDs;
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      for (int index = 0; index < selectedIds.Count; ++index)
      {
        AnimationWindowHierarchyNode windowHierarchyNode = this.state.hierarchyData.FindItem(selectedIds[index]) as AnimationWindowHierarchyNode;
        if (windowHierarchyNode != null && !windowHierarchyNode.propertyName.Equals(RotationCurveInterpolation.GetPrefixForInterpolation(newMode)))
        {
          string oldValue = windowHierarchyNode.propertyName.Split('.')[0];
          string str = windowHierarchyNode.propertyName.Replace(oldValue, RotationCurveInterpolation.GetPrefixForInterpolation(newMode));
          intList1.Add(selectedIds[index]);
          intList2.Add((windowHierarchyNode.path + windowHierarchyNode.animatableObjectType.Name + str).GetHashCode());
        }
      }
      for (int index1 = 0; index1 < intList1.Count; ++index1)
      {
        if (selectedIds.Contains(intList1[index1]))
        {
          int index2 = selectedIds.IndexOf(intList1[index1]);
          selectedIds[index2] = intList2[index1];
        }
        if (expandedIds.Contains(intList1[index1]))
        {
          int index2 = expandedIds.IndexOf(intList1[index1]);
          expandedIds[index2] = intList2[index1];
        }
        if (this.state.hierarchyState.lastClickedID == intList1[index1])
          this.state.hierarchyState.lastClickedID = intList2[index1];
      }
      this.state.hierarchyState.selectedIDs = new List<int>((IEnumerable<int>) selectedIds);
      this.state.hierarchyState.expandedIDs = new List<int>((IEnumerable<int>) expandedIds);
    }

    private RotationCurveInterpolation.Mode GetRotationInterpolationMode(EditorCurveBinding[] curves)
    {
      if (curves == null || curves.Length == 0)
        return RotationCurveInterpolation.Mode.Undefined;
      RotationCurveInterpolation.Mode modeFromCurveData1 = RotationCurveInterpolation.GetModeFromCurveData(curves[0]);
      for (int index = 1; index < curves.Length; ++index)
      {
        RotationCurveInterpolation.Mode modeFromCurveData2 = RotationCurveInterpolation.GetModeFromCurveData(curves[index]);
        if (modeFromCurveData1 != modeFromCurveData2)
          return RotationCurveInterpolation.Mode.Undefined;
      }
      return modeFromCurveData1;
    }

    private void SetStyleTextColor(GUIStyle style, Color color)
    {
      style.normal.textColor = color;
      style.focused.textColor = color;
      style.active.textColor = color;
      style.hover.textColor = color;
    }

    public override void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
    {
      firstRowVisible = 0;
      lastRowVisible = this.m_TreeView.data.rowCount - 1;
    }

    public float GetNodeHeight(AnimationWindowHierarchyNode node)
    {
      if (node is AnimationWindowHierarchyAddButtonNode)
        return 40f;
      return !(this.m_TreeView.state as AnimationWindowHierarchyState).GetTallMode(node) ? 16f : 32f;
    }

    public override Vector2 GetTotalSize()
    {
      IList<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      float y = 0.0f;
      for (int index = 0; index < rows.Count; ++index)
      {
        AnimationWindowHierarchyNode node = rows[index] as AnimationWindowHierarchyNode;
        y += this.GetNodeHeight(node);
      }
      return new Vector2(1f, y);
    }

    private float GetTopPixelOfRow(int row, IList<TreeViewItem> rows)
    {
      float num = 0.0f;
      for (int index = 0; index < row && index < rows.Count; ++index)
      {
        AnimationWindowHierarchyNode row1 = rows[index] as AnimationWindowHierarchyNode;
        num += this.GetNodeHeight(row1);
      }
      return num;
    }

    public override Rect GetRowRect(int row, float rowWidth)
    {
      IList<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      AnimationWindowHierarchyNode node = rows[row] as AnimationWindowHierarchyNode;
      if (!node.topPixel.HasValue)
        node.topPixel = new float?(this.GetTopPixelOfRow(row, rows));
      float nodeHeight = this.GetNodeHeight(node);
      return new Rect(0.0f, node.topPixel.Value, rowWidth, nodeHeight);
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
    {
      AnimationWindowHierarchyNode node1 = node as AnimationWindowHierarchyNode;
      this.DoNodeGUI(rowRect, node1, selected, focused, row);
    }

    public override bool BeginRename(TreeViewItem item, float delay)
    {
      this.m_RenamedNode = item as AnimationWindowHierarchyNode;
      GameObject gameObject = (GameObject) null;
      if (this.m_RenamedNode.curves.Length > 0)
      {
        AnimationWindowSelectionItem selectionBinding = this.m_RenamedNode.curves[0].selectionBinding;
        if ((UnityEngine.Object) selectionBinding != (UnityEngine.Object) null)
          gameObject = selectionBinding.rootGameObject;
      }
      return this.GetRenameOverlay().BeginRename(this.m_RenamedNode.path, item.id, delay);
    }

    protected override void SyncFakeItem()
    {
    }

    protected override void RenameEnded()
    {
      string name = this.GetRenameOverlay().name;
      string originalName = this.GetRenameOverlay().originalName;
      if (name != originalName)
      {
        Undo.RecordObject((UnityEngine.Object) this.state.activeAnimationClip, "Rename Curve");
        foreach (AnimationWindowCurve curve in this.m_RenamedNode.curves)
        {
          EditorCurveBinding renamedBinding = AnimationWindowUtility.GetRenamedBinding(curve.binding, name);
          if (AnimationWindowUtility.CurveExists(renamedBinding, this.state.allCurves.ToArray()))
            Debug.LogWarning((object) "Curve already exists, renaming cancelled.");
          else
            AnimationWindowUtility.RenameCurvePath(curve, renamedBinding, curve.clip);
        }
      }
      this.m_RenamedNode = (AnimationWindowHierarchyNode) null;
    }

    protected override Texture GetIconForItem(TreeViewItem item)
    {
      if (item != null)
        return (Texture) item.icon;
      return (Texture) null;
    }
  }
}
