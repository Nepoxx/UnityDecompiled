// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.DopeSheetEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class DopeSheetEditor : TimeArea, CurveUpdater
  {
    public Bounds m_Bounds = new Bounds(Vector3.zero, Vector3.zero);
    public AnimationWindowState state;
    private const float k_KeyframeOffset = -6.5f;
    private const float k_PptrKeyframeOffset = -1f;
    private const int kLabelMarginHorizontal = 8;
    private const int kLabelMarginVertical = 2;
    [SerializeField]
    public EditorWindow m_Owner;
    private DopeSheetEditor.DopeSheetSelectionRect m_SelectionRect;
    private float m_DragStartTime;
    private bool m_MousedownOnKeyframe;
    private bool m_IsDragging;
    private bool m_IsDraggingPlayheadStarted;
    private bool m_IsDraggingPlayhead;
    private bool m_Initialized;
    private bool m_SpritePreviewLoading;
    private int m_SpritePreviewCacheSize;
    private DopeSheetEditor.DopeSheetControlPointRenderer m_PointRenderer;
    private DopeSheetEditorRectangleTool m_RectangleTool;

    public DopeSheetEditor(EditorWindow owner)
      : base(false)
    {
      this.m_Owner = owner;
    }

    public float contentHeight
    {
      get
      {
        float num = 0.0f;
        foreach (DopeLine dopeline in this.state.dopelines)
          num += !dopeline.tallMode ? 16f : 32f;
        return num + 40f;
      }
    }

    public override Bounds drawingBounds
    {
      get
      {
        return this.m_Bounds;
      }
    }

    public bool isDragging
    {
      get
      {
        return this.m_IsDragging;
      }
    }

    internal int assetPreviewManagerID
    {
      get
      {
        return !((UnityEngine.Object) this.m_Owner != (UnityEngine.Object) null) ? 0 : this.m_Owner.GetInstanceID();
      }
    }

    public bool spritePreviewLoading
    {
      get
      {
        return this.m_SpritePreviewLoading;
      }
    }

    public void OnDisable()
    {
      if (this.m_PointRenderer == null)
        return;
      this.m_PointRenderer.FlushCache();
    }

    internal void OnDestroy()
    {
      AssetPreview.DeletePreviewTextureManagerByID(this.assetPreviewManagerID);
    }

    public void OnGUI(Rect position, Vector2 scrollPosition)
    {
      this.Init();
      this.HandleDragAndDropToEmptyArea();
      GUIClip.Push(position, scrollPosition, Vector2.zero, false);
      this.HandleRectangleToolEvents();
      Rect rect = this.DopelinesGUI(new Rect(0.0f, 0.0f, position.width, position.height), scrollPosition);
      this.HandleKeyboard();
      this.HandleDragging();
      this.HandleSelectionRect(rect);
      this.HandleDelete();
      this.RectangleToolGUI();
      GUIClip.Pop();
    }

    public void Init()
    {
      if (!this.m_Initialized)
      {
        this.hSlider = true;
        this.vSlider = false;
        this.hRangeLocked = false;
        this.vRangeLocked = true;
        this.hRangeMin = 0.0f;
        this.margin = 40f;
        this.scaleWithWindow = true;
        this.ignoreScrollWheelUntilClicked = false;
      }
      this.m_Initialized = true;
      if (this.m_PointRenderer == null)
        this.m_PointRenderer = new DopeSheetEditor.DopeSheetControlPointRenderer();
      if (this.m_RectangleTool != null)
        return;
      this.m_RectangleTool = new DopeSheetEditorRectangleTool();
      this.m_RectangleTool.Initialize((TimeArea) this);
    }

    public void RecalculateBounds()
    {
      if (this.state.disabled)
        return;
      Vector2 timeRange = this.state.timeRange;
      this.m_Bounds.SetMinMax(new Vector3(timeRange.x, 0.0f, 0.0f), new Vector3(timeRange.y, 0.0f, 0.0f));
    }

    private Rect DopelinesGUI(Rect position, Vector2 scrollPosition)
    {
      Color color = GUI.color;
      Rect rect1 = position;
      this.m_PointRenderer.Clear();
      if (Event.current.type == EventType.Repaint)
        this.m_SpritePreviewLoading = false;
      if (Event.current.type == EventType.MouseDown)
        this.m_IsDragging = false;
      this.UpdateSpritePreviewCacheSize();
      List<DopeLine> dopelines = this.state.dopelines;
      for (int index = 0; index < dopelines.Count; ++index)
      {
        DopeLine dopeline = dopelines[index];
        dopeline.position = rect1;
        dopeline.position.height = !dopeline.tallMode ? 16f : 32f;
        if ((double) dopeline.position.yMin + (double) scrollPosition.y >= (double) position.yMin && (double) dopeline.position.yMin + (double) scrollPosition.y <= (double) position.yMax || (double) dopeline.position.yMax + (double) scrollPosition.y >= (double) position.yMin && (double) dopeline.position.yMax + (double) scrollPosition.y <= (double) position.yMax)
        {
          Event current = Event.current;
          EventType type = current.type;
          switch (type)
          {
            case EventType.Repaint:
              this.DopeLineRepaint(dopeline);
              break;
            case EventType.DragUpdated:
            case EventType.DragPerform:
              this.HandleDragAndDrop(dopeline);
              break;
            default:
              if (type != EventType.MouseDown)
              {
                if (type == EventType.ContextClick && !this.m_IsDraggingPlayhead)
                {
                  this.HandleContextMenu(dopeline);
                  break;
                }
                break;
              }
              if (current.button == 0)
              {
                this.HandleMouseDown(dopeline);
                break;
              }
              break;
          }
        }
        rect1.y += dopeline.position.height;
      }
      if (Event.current.type == EventType.MouseUp)
      {
        this.m_IsDraggingPlayheadStarted = false;
        this.m_IsDraggingPlayhead = false;
      }
      Rect rect2 = new Rect(position.xMin, position.yMin, position.width, rect1.yMax - position.yMin);
      this.m_PointRenderer.Render();
      GUI.color = color;
      return rect2;
    }

    private void RectangleToolGUI()
    {
      this.m_RectangleTool.OnGUI();
    }

    private void DrawGrid(Rect position)
    {
      this.TimeRuler(position, this.state.frameRate, false, true, 0.2f);
    }

    public void DrawMasterDopelineBackground(Rect position)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      AnimationWindowStyles.eventBackground.Draw(position, false, false, false, false);
    }

    private void UpdateSpritePreviewCacheSize()
    {
      int num = 1;
      foreach (DopeLine dopeline in this.state.dopelines)
      {
        if (dopeline.tallMode && dopeline.isPptrDopeline)
          num += dopeline.keys.Count;
      }
      int size = num + DragAndDrop.objectReferences.Length;
      if (size <= this.m_SpritePreviewCacheSize)
        return;
      AssetPreview.SetPreviewTextureCacheSize(size, this.assetPreviewManagerID);
      this.m_SpritePreviewCacheSize = size;
    }

    private void DopeLineRepaint(DopeLine dopeline)
    {
      Color color1 = GUI.color;
      AnimationWindowHierarchyNode windowHierarchyNode = (AnimationWindowHierarchyNode) this.state.hierarchyData.FindItem(dopeline.hierarchyNodeID);
      Color color2 = windowHierarchyNode == null || windowHierarchyNode.depth <= 0 ? Color.gray.AlphaMultiplied(0.16f) : Color.gray.AlphaMultiplied(0.05f);
      if (!dopeline.isMasterDopeline)
        DopeSheetEditor.DrawBox(dopeline.position, color2);
      int? nullable = new int?();
      int count = dopeline.keys.Count;
      for (int keyIndex = 0; keyIndex < count; ++keyIndex)
      {
        AnimationWindowKeyframe key = dopeline.keys[keyIndex];
        int num = key.m_TimeHash ^ key.curve.timeOffset.GetHashCode();
        if ((nullable.GetValueOrDefault() != num ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        {
          nullable = new int?(num);
          Rect rect = this.GetKeyframeRect(dopeline, key);
          Color color3 = !dopeline.isMasterDopeline ? Color.gray.RGBMultiplied(1.2f) : Color.gray.RGBMultiplied(0.85f);
          Texture2D texture = (Texture2D) null;
          if (key.isPPtrCurve && dopeline.tallMode)
            texture = key.value != null ? AssetPreview.GetAssetPreview(((UnityEngine.Object) key.value).GetInstanceID(), this.assetPreviewManagerID) : (Texture2D) null;
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            rect = this.GetPreviewRectFromKeyFrameRect(rect);
            color3 = Color.white.AlphaMultiplied(0.5f);
          }
          else if (key.value != null && key.isPPtrCurve && dopeline.tallMode)
            this.m_SpritePreviewLoading = true;
          if (Mathf.Approximately(key.time, 0.0f))
            rect.xMin -= 0.01f;
          if (this.AnyKeyIsSelectedAtTime(dopeline, keyIndex))
          {
            color3 = !dopeline.tallMode || !dopeline.isPptrDopeline ? new Color(0.34f, 0.52f, 0.85f, 1f) : Color.white;
            if (dopeline.isMasterDopeline)
              color3 = color3.RGBMultiplied(0.85f);
            this.m_PointRenderer.AddSelectedKey(new DopeSheetEditor.DrawElement(rect, color3, texture));
          }
          else
            this.m_PointRenderer.AddUnselectedKey(new DopeSheetEditor.DrawElement(rect, color3, texture));
        }
      }
      if (this.DoDragAndDrop(dopeline, dopeline.position, false))
      {
        float time = Mathf.Max(this.state.PixelToTime(Event.current.mousePosition.x, AnimationWindowState.SnapMode.SnapToClipFrame), 0.0f);
        Color color3 = Color.gray.RGBMultiplied(1.2f);
        Texture2D texture = (Texture2D) null;
        foreach (UnityEngine.Object dropObjectReference in this.GetSortedDragAndDropObjectReferences())
        {
          Rect rect = this.GetDragAndDropRect(dopeline, time);
          if (dopeline.isPptrDopeline && dopeline.tallMode)
            texture = AssetPreview.GetAssetPreview(dropObjectReference.GetInstanceID(), this.assetPreviewManagerID);
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            rect = this.GetPreviewRectFromKeyFrameRect(rect);
            color3 = Color.white.AlphaMultiplied(0.5f);
          }
          this.m_PointRenderer.AddDragDropKey(new DopeSheetEditor.DrawElement(rect, color3, texture));
          time += 1f / this.state.frameRate;
        }
      }
      GUI.color = color1;
    }

    private Rect GetPreviewRectFromKeyFrameRect(Rect keyframeRect)
    {
      keyframeRect.width -= 2f;
      keyframeRect.height -= 2f;
      keyframeRect.xMin += 2f;
      keyframeRect.yMin += 2f;
      return keyframeRect;
    }

    private Rect GetDragAndDropRect(DopeLine dopeline, float time)
    {
      Rect keyframeRect = this.GetKeyframeRect(dopeline, (AnimationWindowKeyframe) null);
      float keyframeOffset = this.GetKeyframeOffset(dopeline, (AnimationWindowKeyframe) null);
      keyframeRect.center = new Vector2(this.state.TimeToPixel(time) + keyframeRect.width * 0.5f + keyframeOffset, keyframeRect.center.y);
      return keyframeRect;
    }

    private static void DrawBox(Rect position, Color color)
    {
      Color color1 = GUI.color;
      GUI.color = color;
      DopeLine.dopekeyStyle.Draw(position, GUIContent.none, 0, false);
      GUI.color = color1;
    }

    private GenericMenu GenerateMenu(DopeLine dopeline)
    {
      GenericMenu menu = new GenericMenu();
      List<AnimationWindowKeyframe> animationWindowKeyframeList = new List<AnimationWindowKeyframe>();
      foreach (AnimationWindowKeyframe key in dopeline.keys)
      {
        if (this.GetKeyframeRect(dopeline, key).Contains(Event.current.mousePosition))
          animationWindowKeyframeList.Add(key);
      }
      AnimationKeyTime animationKeyTime = AnimationKeyTime.Time(this.state.PixelToTime(Event.current.mousePosition.x, AnimationWindowState.SnapMode.SnapToClipFrame), this.state.frameRate);
      string text1 = "Add Key";
      if (dopeline.isEditable && animationWindowKeyframeList.Count == 0)
        menu.AddItem(new GUIContent(text1), 0 != 0, new GenericMenu.MenuFunction2(this.AddKeyToDopeline), (object) new DopeSheetEditor.AddKeyToDopelineContext()
        {
          dopeline = dopeline,
          time = animationKeyTime
        });
      else
        menu.AddDisabledItem(new GUIContent(text1));
      string text2 = this.state.selectedKeys.Count <= 1 ? "Delete Key" : "Delete Keys";
      if (dopeline.isEditable && (this.state.selectedKeys.Count > 0 || animationWindowKeyframeList.Count > 0))
        menu.AddItem(new GUIContent(text2), false, new GenericMenu.MenuFunction2(this.DeleteKeys), this.state.selectedKeys.Count <= 0 ? (object) animationWindowKeyframeList : (object) this.state.selectedKeys);
      else
        menu.AddDisabledItem(new GUIContent(text2));
      if (dopeline.isEditable && AnimationWindowUtility.ContainsFloatKeyframes(this.state.selectedKeys))
      {
        menu.AddSeparator(string.Empty);
        List<KeyIdentifier> keyList = new List<KeyIdentifier>();
        Hashtable hashtable = new Hashtable();
        foreach (AnimationWindowKeyframe selectedKey in this.state.selectedKeys)
        {
          if (!selectedKey.isDiscreteCurve)
          {
            int keyframeIndex = selectedKey.curve.GetKeyframeIndex(AnimationKeyTime.Time(selectedKey.time, this.state.frameRate));
            if (keyframeIndex != -1)
            {
              int hashCode = selectedKey.curve.GetHashCode();
              AnimationCurve _curve = (AnimationCurve) hashtable[(object) hashCode];
              if (_curve == null)
              {
                _curve = AnimationUtility.GetEditorCurve(selectedKey.curve.clip, selectedKey.curve.binding) ?? new AnimationCurve();
                hashtable.Add((object) hashCode, (object) _curve);
              }
              keyList.Add(new KeyIdentifier(_curve, hashCode, keyframeIndex, selectedKey.curve.binding));
            }
          }
        }
        new CurveMenuManager((CurveUpdater) this).AddTangentMenuItems(menu, keyList);
      }
      return menu;
    }

    private void HandleDragging()
    {
      int controlId = GUIUtility.GetControlID("dopesheetdrag".GetHashCode(), FocusType.Passive, new Rect());
      EventType typeForControl = Event.current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseUp:
        case EventType.MouseDrag:
          if (this.m_MousedownOnKeyframe)
          {
            if (typeForControl == EventType.MouseDrag && !EditorGUI.actionKey && (!Event.current.shift && !this.m_IsDragging) && this.state.selectedKeys.Count > 0)
            {
              this.m_IsDragging = true;
              this.m_IsDraggingPlayheadStarted = true;
              GUIUtility.hotControl = controlId;
              this.m_DragStartTime = this.state.PixelToTime(Event.current.mousePosition.x);
              this.m_RectangleTool.OnStartMove(new Vector2(this.m_DragStartTime, 0.0f), this.m_RectangleTool.rippleTimeClutch);
              Event.current.Use();
            }
            float b = float.MaxValue;
            foreach (AnimationWindowKeyframe selectedKey in this.state.selectedKeys)
              b = Mathf.Min(selectedKey.time, b);
            float frame = this.state.SnapToFrame(this.state.PixelToTime(Event.current.mousePosition.x), AnimationWindowState.SnapMode.SnapToClipFrame);
            if (this.m_IsDragging && !Mathf.Approximately(frame, this.m_DragStartTime))
            {
              this.m_RectangleTool.OnMove(new Vector2(frame, 0.0f));
              Event.current.Use();
            }
            if (typeForControl == EventType.MouseUp)
            {
              if (this.m_IsDragging && GUIUtility.hotControl == controlId)
              {
                this.m_RectangleTool.OnEndMove();
                Event.current.Use();
                this.m_IsDragging = false;
              }
              this.m_MousedownOnKeyframe = false;
              GUIUtility.hotControl = 0;
            }
            break;
          }
          break;
      }
      if (this.m_IsDraggingPlayheadStarted && typeForControl == EventType.MouseDrag && Event.current.button == 1)
      {
        this.m_IsDraggingPlayhead = true;
        Event.current.Use();
      }
      if (!this.m_IsDragging)
        return;
      Vector2 mousePosition = Event.current.mousePosition;
      EditorGUIUtility.AddCursorRect(new Rect(mousePosition.x - 10f, mousePosition.y - 10f, 20f, 20f), MouseCursor.MoveArrow);
    }

    private void HandleKeyboard()
    {
      if (Event.current.type == EventType.ValidateCommand || Event.current.type == EventType.ExecuteCommand)
      {
        switch (Event.current.commandName)
        {
          case "SelectAll":
            if (Event.current.type == EventType.ExecuteCommand)
              this.HandleSelectAll();
            Event.current.Use();
            break;
          case "FrameSelected":
            if (Event.current.type == EventType.ExecuteCommand)
              this.FrameSelected();
            Event.current.Use();
            break;
        }
      }
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.A)
        return;
      this.FrameClip();
      Event.current.Use();
    }

    private void HandleSelectAll()
    {
      foreach (DopeLine dopeline in this.state.dopelines)
      {
        foreach (AnimationWindowKeyframe key in dopeline.keys)
          this.state.SelectKey(key);
        this.state.SelectHierarchyItem(dopeline, true, false);
      }
    }

    private void HandleDelete()
    {
      if (this.state.selectedKeys.Count == 0)
        return;
      switch (Event.current.type)
      {
        case EventType.KeyDown:
          if (Event.current.keyCode != KeyCode.Backspace && Event.current.keyCode != KeyCode.Delete)
            break;
          this.state.DeleteSelectedKeys();
          Event.current.Use();
          break;
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          if (!(Event.current.commandName == "SoftDelete") && !(Event.current.commandName == "Delete"))
            break;
          if (Event.current.type == EventType.ExecuteCommand)
            this.state.DeleteSelectedKeys();
          Event.current.Use();
          break;
      }
    }

    private void HandleSelectionRect(Rect rect)
    {
      if (this.m_SelectionRect == null)
        this.m_SelectionRect = new DopeSheetEditor.DopeSheetSelectionRect(this);
      if (this.m_MousedownOnKeyframe)
        return;
      this.m_SelectionRect.OnGUI(rect);
    }

    private void HandleDragAndDropToEmptyArea()
    {
      Event current = Event.current;
      if (current.type != EventType.DragPerform && current.type != EventType.DragUpdated || !DopeSheetEditor.ValidateDragAndDropObjects())
        return;
      if (DragAndDrop.objectReferences[0].GetType() == typeof (Sprite) || DragAndDrop.objectReferences[0].GetType() == typeof (Texture2D))
      {
        foreach (AnimationWindowSelectionItem selectedItem in this.state.selection.ToArray())
        {
          if (selectedItem.clipIsEditable && selectedItem.canAddCurves && !this.DopelineForValueTypeExists(typeof (Sprite)))
          {
            if (current.type == EventType.DragPerform)
            {
              EditorCurveBinding? newPptrDopeline = this.CreateNewPptrDopeline(selectedItem, typeof (Sprite));
              if (newPptrDopeline.HasValue)
                this.DoSpriteDropAfterGeneratingNewDopeline(selectedItem.animationClip, newPptrDopeline);
            }
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            current.Use();
            return;
          }
        }
      }
      DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    }

    private void DoSpriteDropAfterGeneratingNewDopeline(AnimationClip animationClip, EditorCurveBinding? spriteBinding)
    {
      if (DragAndDrop.objectReferences.Length == 1)
        UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop single sprite into empty dopesheet", "null", 1);
      else
        UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop multiple sprites into empty dopesheet", "null", 1);
      this.PerformDragAndDrop(new AnimationWindowCurve(animationClip, spriteBinding.Value, typeof (Sprite)), 0.0f);
    }

    private void HandleRectangleToolEvents()
    {
      this.m_RectangleTool.HandleEvents();
    }

    private bool DopelineForValueTypeExists(System.Type valueType)
    {
      return this.state.allCurves.Exists((Predicate<AnimationWindowCurve>) (curve => curve.valueType == valueType));
    }

    private EditorCurveBinding? CreateNewPptrDopeline(AnimationWindowSelectionItem selectedItem, System.Type valueType)
    {
      List<EditorCurveBinding> editorCurveBindingList = (List<EditorCurveBinding>) null;
      if ((UnityEngine.Object) selectedItem.rootGameObject != (UnityEngine.Object) null)
      {
        editorCurveBindingList = AnimationWindowUtility.GetAnimatableProperties(selectedItem.rootGameObject, selectedItem.rootGameObject, valueType);
        if (editorCurveBindingList.Count == 0 && valueType == typeof (Sprite))
          return this.CreateNewSpriteRendererDopeline(selectedItem.rootGameObject, selectedItem.rootGameObject);
      }
      else if ((UnityEngine.Object) selectedItem.scriptableObject != (UnityEngine.Object) null)
        editorCurveBindingList = AnimationWindowUtility.GetAnimatableProperties(selectedItem.scriptableObject, valueType);
      if (editorCurveBindingList == null || editorCurveBindingList.Count == 0)
        return new EditorCurveBinding?();
      if (editorCurveBindingList.Count == 1)
        return new EditorCurveBinding?(editorCurveBindingList[0]);
      List<string> stringList = new List<string>();
      foreach (EditorCurveBinding editorCurveBinding in editorCurveBindingList)
        stringList.Add(editorCurveBinding.type.Name);
      EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), EditorGUIUtility.TempContent(stringList.ToArray()), -1, new EditorUtility.SelectMenuItemFunction(this.SelectTypeForCreatingNewPptrDopeline), (object) new List<object>()
      {
        (object) selectedItem.animationClip,
        (object) editorCurveBindingList
      });
      return new EditorCurveBinding?();
    }

    private void SelectTypeForCreatingNewPptrDopeline(object userData, string[] options, int selected)
    {
      List<object> objectList = userData as List<object>;
      AnimationClip animationClip = objectList[0] as AnimationClip;
      List<EditorCurveBinding> editorCurveBindingList = objectList[1] as List<EditorCurveBinding>;
      if (editorCurveBindingList.Count <= selected)
        return;
      this.DoSpriteDropAfterGeneratingNewDopeline(animationClip, new EditorCurveBinding?(editorCurveBindingList[selected]));
    }

    private EditorCurveBinding? CreateNewSpriteRendererDopeline(GameObject targetGameObject, GameObject rootGameObject)
    {
      if (!(bool) ((UnityEngine.Object) targetGameObject.GetComponent<SpriteRenderer>()))
        targetGameObject.AddComponent<SpriteRenderer>();
      List<EditorCurveBinding> animatableProperties = AnimationWindowUtility.GetAnimatableProperties(targetGameObject, rootGameObject, typeof (SpriteRenderer), typeof (Sprite));
      if (animatableProperties.Count == 1)
        return new EditorCurveBinding?(animatableProperties[0]);
      Debug.LogError((object) "Unable to create animatable SpriteRenderer component");
      return new EditorCurveBinding?();
    }

    private void HandleDragAndDrop(DopeLine dopeline)
    {
      Event current = Event.current;
      if (current.type != EventType.DragPerform && current.type != EventType.DragUpdated)
        return;
      if (this.DoDragAndDrop(dopeline, dopeline.position, current.type == EventType.DragPerform))
      {
        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        current.Use();
      }
      else
        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    }

    private void HandleMouseDown(DopeLine dopeline)
    {
      Event current = Event.current;
      if (!dopeline.position.Contains(current.mousePosition))
        return;
      bool flag1 = false;
      foreach (AnimationWindowKeyframe key in dopeline.keys)
      {
        if (this.GetKeyframeRect(dopeline, key).Contains(current.mousePosition) && this.state.KeyIsSelected(key))
        {
          flag1 = true;
          break;
        }
      }
      bool flag2 = flag1 && EditorGUI.actionKey;
      bool flag3 = !flag1;
      if (!flag1 && !EditorGUI.actionKey && !current.shift)
        this.state.ClearSelections();
      float time = this.state.PixelToTime(Event.current.mousePosition.x);
      float num = time;
      if (Event.current.shift)
      {
        foreach (AnimationWindowKeyframe key in dopeline.keys)
        {
          if (this.state.KeyIsSelected(key))
          {
            if ((double) key.time < (double) time)
              time = key.time;
            if ((double) key.time > (double) num)
              num = key.time;
          }
        }
      }
      bool flag4 = false;
      foreach (AnimationWindowKeyframe key1 in dopeline.keys)
      {
        if (this.GetKeyframeRect(dopeline, key1).Contains(current.mousePosition))
        {
          flag4 = true;
          if (flag2)
          {
            if (this.state.KeyIsSelected(key1))
            {
              this.state.UnselectKey(key1);
              if (!this.state.AnyKeyIsSelected(dopeline))
                this.state.UnSelectHierarchyItem(dopeline);
            }
          }
          else if (flag3 && !this.state.KeyIsSelected(key1))
          {
            if (Event.current.shift)
            {
              foreach (AnimationWindowKeyframe key2 in dopeline.keys)
              {
                if (key2 == key1 || (double) key2.time > (double) time && (double) key2.time < (double) num)
                  this.state.SelectKey(key2);
              }
            }
            else
              this.state.SelectKey(key1);
            if (!dopeline.isMasterDopeline)
              this.state.SelectHierarchyItem(dopeline, EditorGUI.actionKey || current.shift);
          }
          this.state.activeKeyframe = key1;
          this.m_MousedownOnKeyframe = true;
          current.Use();
        }
      }
      if (dopeline.isMasterDopeline)
      {
        this.state.ClearHierarchySelection();
        foreach (int affectedHierarchyId in this.state.GetAffectedHierarchyIDs(this.state.selectedKeys))
          this.state.SelectHierarchyItem(affectedHierarchyId, true, true);
      }
      if (current.clickCount == 2 && current.button == 0 && (!Event.current.shift && !EditorGUI.actionKey))
        this.HandleDopelineDoubleclick(dopeline);
      if (current.button != 1 || this.state.controlInterface.playing || flag4)
        return;
      this.state.ClearSelections();
      this.m_IsDraggingPlayheadStarted = true;
      HandleUtility.Repaint();
      current.Use();
    }

    private void HandleDopelineDoubleclick(DopeLine dopeline)
    {
      AnimationKeyTime time = AnimationKeyTime.Time(this.state.PixelToTime(Event.current.mousePosition.x, AnimationWindowState.SnapMode.SnapToClipFrame), this.state.frameRate);
      AnimationWindowUtility.AddKeyframes(this.state, ((IEnumerable<AnimationWindowCurve>) dopeline.curves).ToArray<AnimationWindowCurve>(), time);
      Event.current.Use();
    }

    private void HandleContextMenu(DopeLine dopeline)
    {
      if (!dopeline.position.Contains(Event.current.mousePosition))
        return;
      this.GenerateMenu(dopeline).ShowAsContext();
    }

    private Rect GetKeyframeRect(DopeLine dopeline, AnimationWindowKeyframe keyframe)
    {
      float time = keyframe == null ? 0.0f : keyframe.time + keyframe.curve.timeOffset;
      float width = 10f;
      if (dopeline.isPptrDopeline && dopeline.tallMode && (keyframe == null || keyframe.value != null))
        width = dopeline.position.height;
      return new Rect(this.state.TimeToPixel(this.state.SnapToFrame(time, AnimationWindowState.SnapMode.SnapToClipFrame)) + this.GetKeyframeOffset(dopeline, keyframe), dopeline.position.yMin, width, dopeline.position.height);
    }

    private float GetKeyframeOffset(DopeLine dopeline, AnimationWindowKeyframe keyframe)
    {
      return dopeline.isPptrDopeline && dopeline.tallMode && (keyframe == null || keyframe.value != null) ? -1f : -6.5f;
    }

    public void FrameClip()
    {
      if (this.state.disabled)
        return;
      Vector2 timeRange = this.state.timeRange;
      timeRange.y = Mathf.Max(timeRange.x + 0.1f, timeRange.y);
      this.SetShownHRangeInsideMargins(timeRange.x, timeRange.y);
    }

    public void FrameSelected()
    {
      Bounds bounds = new Bounds();
      bool flag1 = true;
      bool flag2 = this.state.selectedKeys.Count > 0;
      if (flag2)
      {
        foreach (AnimationWindowKeyframe selectedKey in this.state.selectedKeys)
        {
          Vector2 vector2 = new Vector2(selectedKey.time + selectedKey.curve.timeOffset, 0.0f);
          if (flag1)
          {
            bounds.SetMinMax((Vector3) vector2, (Vector3) vector2);
            flag1 = false;
          }
          else
            bounds.Encapsulate((Vector3) vector2);
        }
      }
      bool flag3 = !flag2;
      if (!flag2 && this.state.hierarchyState.selectedIDs.Count > 0)
      {
        foreach (AnimationWindowCurve activeCurve in this.state.activeCurves)
        {
          int count = activeCurve.m_Keyframes.Count;
          if (count > 1)
          {
            Vector2 vector2_1 = new Vector2(activeCurve.m_Keyframes[0].time + activeCurve.timeOffset, 0.0f);
            Vector2 vector2_2 = new Vector2(activeCurve.m_Keyframes[count - 1].time + activeCurve.timeOffset, 0.0f);
            if (flag1)
            {
              bounds.SetMinMax((Vector3) vector2_1, (Vector3) vector2_2);
              flag1 = false;
            }
            else
            {
              bounds.Encapsulate((Vector3) vector2_1);
              bounds.Encapsulate((Vector3) vector2_2);
            }
            flag3 = false;
          }
        }
      }
      if (flag3)
      {
        this.FrameClip();
      }
      else
      {
        bounds.size = new Vector3(Mathf.Max(bounds.size.x, 0.1f), Mathf.Max(bounds.size.y, 0.1f), 0.0f);
        this.SetShownHRangeInsideMargins(bounds.min.x, bounds.max.x);
      }
    }

    private bool DoDragAndDrop(DopeLine dopeLine, Rect position, bool perform)
    {
      if (!position.Contains(Event.current.mousePosition) || !DopeSheetEditor.ValidateDragAndDropObjects())
        return false;
      System.Type type = DragAndDrop.objectReferences[0].GetType();
      AnimationWindowCurve animationWindowCurve = (AnimationWindowCurve) null;
      if (dopeLine.valueType == type)
      {
        animationWindowCurve = dopeLine.curves[0];
      }
      else
      {
        foreach (AnimationWindowCurve curve in dopeLine.curves)
        {
          if (curve.isPPtrCurve)
          {
            if (curve.valueType == type)
              animationWindowCurve = curve;
            List<Sprite> fromPathsOrObjects = SpriteUtility.GetSpriteFromPathsOrObjects(DragAndDrop.objectReferences, DragAndDrop.paths, Event.current.type);
            if (curve.valueType == typeof (Sprite) && fromPathsOrObjects.Count > 0)
            {
              animationWindowCurve = curve;
              type = typeof (Sprite);
            }
          }
        }
      }
      if (animationWindowCurve == null || !animationWindowCurve.clipIsEditable)
        return false;
      if (perform)
      {
        if (DragAndDrop.objectReferences.Length == 1)
          UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop single sprite into existing dopeline", "null", 1);
        else
          UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop multiple sprites into existing dopeline", "null", 1);
        float time = Mathf.Max(this.state.PixelToTime(Event.current.mousePosition.x, AnimationWindowState.SnapMode.SnapToClipFrame), 0.0f);
        this.PerformDragAndDrop(this.GetCurveOfType(dopeLine, type), time);
      }
      return true;
    }

    private void PerformDragAndDrop(AnimationWindowCurve targetCurve, float time)
    {
      if (DragAndDrop.objectReferences.Length == 0 || targetCurve == null)
        return;
      string undoLabel = "Drop Key";
      this.state.SaveKeySelection(undoLabel);
      this.state.ClearSelections();
      foreach (UnityEngine.Object dropObjectReference in this.GetSortedDragAndDropObjectReferences())
      {
        UnityEngine.Object @object = dropObjectReference;
        if (@object is Texture2D)
          @object = (UnityEngine.Object) SpriteUtility.TextureToSprite(dropObjectReference as Texture2D);
        this.CreateNewPPtrKeyframe(time, @object, targetCurve);
        time += 1f / targetCurve.clip.frameRate;
      }
      this.state.SaveCurve(targetCurve, undoLabel);
      DragAndDrop.AcceptDrag();
    }

    private UnityEngine.Object[] GetSortedDragAndDropObjectReferences()
    {
      UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
      Array.Sort<UnityEngine.Object>(objectReferences, (Comparison<UnityEngine.Object>) ((a, b) => EditorUtility.NaturalCompare(a.name, b.name)));
      return objectReferences;
    }

    private void CreateNewPPtrKeyframe(float time, UnityEngine.Object value, AnimationWindowCurve targetCurve)
    {
      AnimationWindowKeyframe animationWindowKeyframe = new AnimationWindowKeyframe(targetCurve, new ObjectReferenceKeyframe() { time = time, value = value });
      AnimationKeyTime keyTime = AnimationKeyTime.Time(animationWindowKeyframe.time, this.state.frameRate);
      targetCurve.AddKeyframe(animationWindowKeyframe, keyTime);
      this.state.SelectKey(animationWindowKeyframe);
    }

    private static bool ValidateDragAndDropObjects()
    {
      if (DragAndDrop.objectReferences.Length == 0)
        return false;
      for (int index = 0; index < DragAndDrop.objectReferences.Length; ++index)
      {
        UnityEngine.Object objectReference1 = DragAndDrop.objectReferences[index];
        if (objectReference1 == (UnityEngine.Object) null)
          return false;
        if (index < DragAndDrop.objectReferences.Length - 1)
        {
          UnityEngine.Object objectReference2 = DragAndDrop.objectReferences[index + 1];
          bool flag = (objectReference1 is Texture2D || objectReference1 is Sprite) && (objectReference2 is Texture2D || objectReference2 is Sprite);
          if (objectReference1.GetType() != objectReference2.GetType() && !flag)
            return false;
        }
      }
      return true;
    }

    private AnimationWindowCurve GetCurveOfType(DopeLine dopeLine, System.Type type)
    {
      foreach (AnimationWindowCurve curve in dopeLine.curves)
      {
        if (curve.valueType == type)
          return curve;
      }
      return (AnimationWindowCurve) null;
    }

    private bool AnyKeyIsSelectedAtTime(DopeLine dopeLine, int keyIndex)
    {
      AnimationWindowKeyframe key1 = dopeLine.keys[keyIndex];
      int num = key1.m_TimeHash ^ key1.curve.timeOffset.GetHashCode();
      int count = dopeLine.keys.Count;
      for (int index = keyIndex; index < count; ++index)
      {
        AnimationWindowKeyframe key2 = dopeLine.keys[index];
        if ((key2.m_TimeHash ^ key2.curve.timeOffset.GetHashCode()) != num)
          return false;
        if (this.state.KeyIsSelected(key2))
          return true;
      }
      return false;
    }

    private void AddKeyToDopeline(object obj)
    {
      this.AddKeyToDopeline((DopeSheetEditor.AddKeyToDopelineContext) obj);
    }

    private void AddKeyToDopeline(DopeSheetEditor.AddKeyToDopelineContext context)
    {
      AnimationWindowUtility.AddKeyframes(this.state, ((IEnumerable<AnimationWindowCurve>) context.dopeline.curves).ToArray<AnimationWindowCurve>(), context.time);
    }

    private void DeleteKeys(object obj)
    {
      this.DeleteKeys((List<AnimationWindowKeyframe>) obj);
    }

    private void DeleteKeys(List<AnimationWindowKeyframe> keys)
    {
      this.state.DeleteKeys(keys);
    }

    public void UpdateCurves(List<ChangedCurve> changedCurves, string undoText)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.state.activeAnimationClip, undoText);
      foreach (ChangedCurve changedCurve in changedCurves)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        DopeSheetEditor.\u003CUpdateCurves\u003Ec__AnonStorey1 curvesCAnonStorey1 = new DopeSheetEditor.\u003CUpdateCurves\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        curvesCAnonStorey1.changedCurve = changedCurve;
        // ISSUE: reference to a compiler-generated method
        AnimationWindowCurve animationWindowCurve = this.state.allCurves.Find(new Predicate<AnimationWindowCurve>(curvesCAnonStorey1.\u003C\u003Em__0));
        if (animationWindowCurve != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AnimationUtility.SetEditorCurve(animationWindowCurve.clip, curvesCAnonStorey1.changedCurve.binding, curvesCAnonStorey1.changedCurve.curve);
        }
        else
          Debug.LogError((object) "Could not match ChangedCurve data to destination curves.");
      }
    }

    private struct DrawElement
    {
      public Rect position;
      public Color color;
      public Texture2D texture;

      public DrawElement(Rect position, Color color, Texture2D texture)
      {
        this.position = position;
        this.color = color;
        this.texture = texture;
      }
    }

    private class DopeSheetControlPointRenderer
    {
      private List<DopeSheetEditor.DrawElement> m_UnselectedKeysDrawBuffer = new List<DopeSheetEditor.DrawElement>();
      private List<DopeSheetEditor.DrawElement> m_SelectedKeysDrawBuffer = new List<DopeSheetEditor.DrawElement>();
      private List<DopeSheetEditor.DrawElement> m_DragDropKeysDrawBuffer = new List<DopeSheetEditor.DrawElement>();
      private ControlPointRenderer m_UnselectedKeysRenderer;
      private ControlPointRenderer m_SelectedKeysRenderer;
      private ControlPointRenderer m_DragDropKeysRenderer;
      private Texture2D m_DefaultDopeKeyIcon;

      public DopeSheetControlPointRenderer()
      {
        this.m_DefaultDopeKeyIcon = EditorGUIUtility.LoadIcon("blendKey");
        this.m_UnselectedKeysRenderer = new ControlPointRenderer(this.m_DefaultDopeKeyIcon);
        this.m_SelectedKeysRenderer = new ControlPointRenderer(this.m_DefaultDopeKeyIcon);
        this.m_DragDropKeysRenderer = new ControlPointRenderer(this.m_DefaultDopeKeyIcon);
      }

      public void FlushCache()
      {
        this.m_UnselectedKeysRenderer.FlushCache();
        this.m_SelectedKeysRenderer.FlushCache();
        this.m_DragDropKeysRenderer.FlushCache();
      }

      private void DrawElements(List<DopeSheetEditor.DrawElement> elements)
      {
        if (elements.Count == 0)
          return;
        Color color1 = GUI.color;
        Color color2 = Color.white;
        GUI.color = color2;
        Texture defaultDopeKeyIcon = (Texture) this.m_DefaultDopeKeyIcon;
        for (int index = 0; index < elements.Count; ++index)
        {
          DopeSheetEditor.DrawElement element = elements[index];
          if (element.color != color2)
          {
            color2 = !GUI.enabled ? element.color * 0.8f : element.color;
            GUI.color = color2;
          }
          if ((UnityEngine.Object) element.texture != (UnityEngine.Object) null)
            GUI.Label(element.position, (Texture) element.texture, GUIStyle.none);
          else
            GUI.Label(new Rect(element.position.center.x - (float) (defaultDopeKeyIcon.width / 2), element.position.center.y - (float) (defaultDopeKeyIcon.height / 2), (float) defaultDopeKeyIcon.width, (float) defaultDopeKeyIcon.height), defaultDopeKeyIcon, GUIStyle.none);
        }
        GUI.color = color1;
      }

      public void Clear()
      {
        this.m_UnselectedKeysDrawBuffer.Clear();
        this.m_SelectedKeysDrawBuffer.Clear();
        this.m_DragDropKeysDrawBuffer.Clear();
        this.m_UnselectedKeysRenderer.Clear();
        this.m_SelectedKeysRenderer.Clear();
        this.m_DragDropKeysRenderer.Clear();
      }

      public void Render()
      {
        this.DrawElements(this.m_UnselectedKeysDrawBuffer);
        this.m_UnselectedKeysRenderer.Render();
        this.DrawElements(this.m_SelectedKeysDrawBuffer);
        this.m_SelectedKeysRenderer.Render();
        this.DrawElements(this.m_DragDropKeysDrawBuffer);
        this.m_DragDropKeysRenderer.Render();
      }

      public void AddUnselectedKey(DopeSheetEditor.DrawElement element)
      {
        if ((UnityEngine.Object) element.texture != (UnityEngine.Object) null)
        {
          this.m_UnselectedKeysDrawBuffer.Add(element);
        }
        else
        {
          Rect position = element.position;
          position.size = new Vector2((float) this.m_DefaultDopeKeyIcon.width, (float) this.m_DefaultDopeKeyIcon.height);
          this.m_UnselectedKeysRenderer.AddPoint(position, element.color);
        }
      }

      public void AddSelectedKey(DopeSheetEditor.DrawElement element)
      {
        if ((UnityEngine.Object) element.texture != (UnityEngine.Object) null)
        {
          this.m_SelectedKeysDrawBuffer.Add(element);
        }
        else
        {
          Rect position = element.position;
          position.size = new Vector2((float) this.m_DefaultDopeKeyIcon.width, (float) this.m_DefaultDopeKeyIcon.height);
          this.m_SelectedKeysRenderer.AddPoint(position, element.color);
        }
      }

      public void AddDragDropKey(DopeSheetEditor.DrawElement element)
      {
        if ((UnityEngine.Object) element.texture != (UnityEngine.Object) null)
        {
          this.m_DragDropKeysDrawBuffer.Add(element);
        }
        else
        {
          Rect position = element.position;
          position.size = new Vector2((float) this.m_DefaultDopeKeyIcon.width, (float) this.m_DefaultDopeKeyIcon.height);
          this.m_DragDropKeysRenderer.AddPoint(position, element.color);
        }
      }
    }

    private struct AddKeyToDopelineContext
    {
      public DopeLine dopeline;
      public AnimationKeyTime time;
    }

    internal class DopeSheetSelectionRect
    {
      private static int s_RectSelectionID = GUIUtility.GetPermanentControlID();
      public readonly GUIStyle createRect = (GUIStyle) "U2D.createRect";
      private Vector2 m_SelectStartPoint;
      private Vector2 m_SelectMousePoint;
      private bool m_ValidRect;
      private DopeSheetEditor owner;

      public DopeSheetSelectionRect(DopeSheetEditor owner)
      {
        this.owner = owner;
      }

      public void OnGUI(Rect position)
      {
        Event current = Event.current;
        Vector2 mousePosition = current.mousePosition;
        int rectSelectionId = DopeSheetEditor.DopeSheetSelectionRect.s_RectSelectionID;
        switch (current.GetTypeForControl(rectSelectionId))
        {
          case EventType.MouseDown:
            if (current.button != 0 || !position.Contains(mousePosition))
              break;
            GUIUtility.hotControl = rectSelectionId;
            this.m_SelectStartPoint = mousePosition;
            this.m_ValidRect = false;
            current.Use();
            break;
          case EventType.MouseUp:
            if (GUIUtility.hotControl != rectSelectionId || current.button != 0)
              break;
            if (this.m_ValidRect)
            {
              if (!current.shift && !EditorGUI.actionKey)
                this.owner.state.ClearSelections();
              float frameRate = this.owner.state.frameRate;
              Rect currentTimeRect = this.GetCurrentTimeRect();
              GUI.changed = true;
              this.owner.state.ClearHierarchySelection();
              List<AnimationWindowKeyframe> animationWindowKeyframeList1 = new List<AnimationWindowKeyframe>();
              List<AnimationWindowKeyframe> animationWindowKeyframeList2 = new List<AnimationWindowKeyframe>();
              foreach (DopeLine dopeline in this.owner.state.dopelines)
              {
                if ((double) dopeline.position.yMin >= (double) currentTimeRect.yMin && (double) dopeline.position.yMax <= (double) currentTimeRect.yMax)
                {
                  foreach (AnimationWindowKeyframe key in dopeline.keys)
                  {
                    AnimationKeyTime animationKeyTime1 = AnimationKeyTime.Time(currentTimeRect.xMin - key.curve.timeOffset, frameRate);
                    AnimationKeyTime animationKeyTime2 = AnimationKeyTime.Time(currentTimeRect.xMax - key.curve.timeOffset, frameRate);
                    AnimationKeyTime animationKeyTime3 = AnimationKeyTime.Time(key.time, frameRate);
                    if ((!dopeline.tallMode && animationKeyTime3.frame >= animationKeyTime1.frame && animationKeyTime3.frame <= animationKeyTime2.frame || dopeline.tallMode && animationKeyTime3.frame >= animationKeyTime1.frame && animationKeyTime3.frame < animationKeyTime2.frame) && (!animationWindowKeyframeList2.Contains(key) && !animationWindowKeyframeList1.Contains(key)))
                    {
                      if (!this.owner.state.KeyIsSelected(key))
                        animationWindowKeyframeList2.Add(key);
                      else if (this.owner.state.KeyIsSelected(key))
                        animationWindowKeyframeList1.Add(key);
                    }
                  }
                }
              }
              if (animationWindowKeyframeList2.Count == 0)
              {
                foreach (AnimationWindowKeyframe keyframe in animationWindowKeyframeList1)
                  this.owner.state.UnselectKey(keyframe);
              }
              foreach (AnimationWindowKeyframe keyframe in animationWindowKeyframeList2)
                this.owner.state.SelectKey(keyframe);
              foreach (DopeLine dopeline in this.owner.state.dopelines)
              {
                if (this.owner.state.AnyKeyIsSelected(dopeline))
                  this.owner.state.SelectHierarchyItem(dopeline, true, false);
              }
            }
            else
              this.owner.state.ClearSelections();
            current.Use();
            GUIUtility.hotControl = 0;
            break;
          case EventType.MouseDrag:
            if (GUIUtility.hotControl != rectSelectionId)
              break;
            this.m_ValidRect = (double) Mathf.Abs((mousePosition - this.m_SelectStartPoint).x) > 1.0;
            if (this.m_ValidRect)
              this.m_SelectMousePoint = new Vector2(mousePosition.x, mousePosition.y);
            current.Use();
            break;
          case EventType.Repaint:
            if (GUIUtility.hotControl != rectSelectionId || !this.m_ValidRect)
              break;
            EditorStyles.selectionRect.Draw(this.GetCurrentPixelRect(), GUIContent.none, false, false, false, false);
            break;
        }
      }

      public Rect GetCurrentPixelRect()
      {
        float num = 16f;
        Rect rect = AnimationWindowUtility.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint);
        rect.xMin = this.owner.state.TimeToPixel(this.owner.state.PixelToTime(rect.xMin, AnimationWindowState.SnapMode.SnapToClipFrame), AnimationWindowState.SnapMode.SnapToClipFrame);
        rect.xMax = this.owner.state.TimeToPixel(this.owner.state.PixelToTime(rect.xMax, AnimationWindowState.SnapMode.SnapToClipFrame), AnimationWindowState.SnapMode.SnapToClipFrame);
        rect.yMin = Mathf.Floor(rect.yMin / num) * num;
        rect.yMax = (Mathf.Floor(rect.yMax / num) + 1f) * num;
        return rect;
      }

      public Rect GetCurrentTimeRect()
      {
        float num = 16f;
        Rect rect = AnimationWindowUtility.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint);
        rect.xMin = this.owner.state.PixelToTime(rect.xMin, AnimationWindowState.SnapMode.SnapToClipFrame);
        rect.xMax = this.owner.state.PixelToTime(rect.xMax, AnimationWindowState.SnapMode.SnapToClipFrame);
        rect.yMin = Mathf.Floor(rect.yMin / num) * num;
        rect.yMax = (Mathf.Floor(rect.yMax / num) + 1f) * num;
        return rect;
      }

      private enum SelectionType
      {
        Normal,
        Additive,
        Subtractive,
      }
    }
  }
}
