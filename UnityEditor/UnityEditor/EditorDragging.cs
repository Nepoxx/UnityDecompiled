// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class EditorDragging
  {
    private int m_TargetIndex = -1;
    private int m_LastIndex = -1;
    private float m_LastMarkerY = 0.0f;
    private const string k_DraggingModeKey = "InspectorEditorDraggingMode";
    private InspectorWindow m_InspectorWindow;
    private bool m_TargetAbove;

    public EditorDragging(InspectorWindow inspectorWindow)
    {
      this.m_InspectorWindow = inspectorWindow;
    }

    public void HandleDraggingToEditor(int editorIndex, Rect dragRect, Rect contentRect, ActiveEditorTracker tracker)
    {
      if ((double) dragRect.height == 0.0)
        return;
      if ((double) contentRect.height == 0.0)
        contentRect = dragRect;
      float num = 8f;
      Rect targetRect = new Rect(contentRect.x, contentRect.yMax - (num - 2f), contentRect.width, (float) ((double) num * 2.0 + 1.0));
      float yMax = contentRect.yMax;
      this.m_LastIndex = editorIndex;
      this.m_LastMarkerY = yMax;
      this.HandleEditorDragging(editorIndex, targetRect, yMax, false, tracker);
    }

    public void HandleDraggingToBottomArea(Rect bottomRect, ActiveEditorTracker tracker)
    {
      int lastIndex = this.m_LastIndex;
      if (lastIndex < 0 || lastIndex >= tracker.activeEditors.Length)
        return;
      this.HandleEditorDragging(lastIndex, bottomRect, this.m_LastMarkerY, true, tracker);
    }

    private void HandleEditorDragging(int editorIndex, Rect targetRect, float markerY, bool bottomTarget, ActiveEditorTracker tracker)
    {
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.Repaint:
          if (this.m_TargetIndex == -1 || !targetRect.Contains(current.mousePosition))
            break;
          Rect position = new Rect(targetRect.x, markerY, targetRect.width, 3f);
          if (!this.m_TargetAbove)
            position.y += 2f;
          EditorDragging.Styles.insertionMarker.Draw(position, false, false, false, false);
          break;
        case EventType.DragUpdated:
          if (targetRect.Contains(current.mousePosition))
          {
            EditorDragging.DraggingMode? nullable = DragAndDrop.GetGenericData("InspectorEditorDraggingMode") as EditorDragging.DraggingMode?;
            if (!nullable.HasValue)
            {
              UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
              nullable = objectReferences.Length != 0 ? (!((IEnumerable<UnityEngine.Object>) objectReferences).All<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (o => o is Component && !(o is Transform))) ? (!((IEnumerable<UnityEngine.Object>) objectReferences).All<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (o => o is MonoScript)) ? new EditorDragging.DraggingMode?(EditorDragging.DraggingMode.NotApplicable) : new EditorDragging.DraggingMode?(EditorDragging.DraggingMode.Script)) : new EditorDragging.DraggingMode?(EditorDragging.DraggingMode.Component)) : new EditorDragging.DraggingMode?(EditorDragging.DraggingMode.NotApplicable);
              DragAndDrop.SetGenericData("InspectorEditorDraggingMode", (object) nullable);
            }
            if (nullable.Value == EditorDragging.DraggingMode.NotApplicable)
              break;
            Editor[] activeEditors = tracker.activeEditors;
            UnityEngine.Object[] objectReferences1 = DragAndDrop.objectReferences;
            if (bottomTarget)
            {
              this.m_TargetAbove = false;
              this.m_TargetIndex = this.m_LastIndex;
            }
            else
            {
              this.m_TargetAbove = (double) current.mousePosition.y < (double) targetRect.y + (double) targetRect.height / 2.0;
              this.m_TargetIndex = editorIndex;
              if (this.m_TargetAbove)
              {
                ++this.m_TargetIndex;
                while (this.m_TargetIndex < activeEditors.Length && this.m_InspectorWindow.ShouldCullEditor(activeEditors, this.m_TargetIndex))
                  ++this.m_TargetIndex;
                if (this.m_TargetIndex == activeEditors.Length)
                {
                  this.m_TargetIndex = -1;
                  break;
                }
              }
            }
            if (this.m_TargetAbove && this.m_InspectorWindow.EditorHasLargeHeader(this.m_TargetIndex, activeEditors))
            {
              --this.m_TargetIndex;
              while (this.m_TargetIndex >= 0 && this.m_InspectorWindow.ShouldCullEditor(activeEditors, this.m_TargetIndex))
                --this.m_TargetIndex;
              if (this.m_TargetIndex == -1)
                break;
              this.m_TargetAbove = false;
            }
            if (nullable.Value == EditorDragging.DraggingMode.Script)
            {
              DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            }
            else
            {
              bool flag = false;
              if (((IEnumerable<UnityEngine.Object>) activeEditors[this.m_TargetIndex].targets).All<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (t => t is Component)))
                flag = this.MoveOrCopyComponents(DragAndDrop.objectReferences.Cast<Component>().ToArray<Component>(), activeEditors[this.m_TargetIndex].targets.Cast<Component>().ToArray<Component>(), EditorUtility.EventHasDragCopyModifierPressed(current), true);
              if (flag)
              {
                DragAndDrop.visualMode = !EditorUtility.EventHasDragCopyModifierPressed(current) ? DragAndDropVisualMode.Move : DragAndDropVisualMode.Copy;
              }
              else
              {
                DragAndDrop.visualMode = DragAndDropVisualMode.None;
                this.m_TargetIndex = -1;
                break;
              }
            }
            current.Use();
            break;
          }
          this.m_TargetIndex = -1;
          break;
        case EventType.DragPerform:
          if (this.m_TargetIndex == -1)
            break;
          EditorDragging.DraggingMode? genericData = DragAndDrop.GetGenericData("InspectorEditorDraggingMode") as EditorDragging.DraggingMode?;
          if (!genericData.HasValue || genericData.Value == EditorDragging.DraggingMode.NotApplicable)
          {
            this.m_TargetIndex = -1;
            break;
          }
          Editor[] activeEditors1 = tracker.activeEditors;
          if (!((IEnumerable<UnityEngine.Object>) activeEditors1[this.m_TargetIndex].targets).All<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (t => t is Component)))
            break;
          Component[] array1 = activeEditors1[this.m_TargetIndex].targets.Cast<Component>().ToArray<Component>();
          if (genericData.Value == EditorDragging.DraggingMode.Script)
          {
            IEnumerable<MonoScript> source = DragAndDrop.objectReferences.Cast<MonoScript>();
            bool flag = true;
            foreach (Component component in array1)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              EditorDragging.\u003CHandleEditorDragging\u003Ec__AnonStorey0 draggingCAnonStorey0 = new EditorDragging.\u003CHandleEditorDragging\u003Ec__AnonStorey0();
              // ISSUE: reference to a compiler-generated field
              draggingCAnonStorey0.targetComponent = component;
              // ISSUE: reference to a compiler-generated field
              GameObject gameObject = draggingCAnonStorey0.targetComponent.gameObject;
              // ISSUE: reference to a compiler-generated method
              if (source.Any<MonoScript>(new Func<MonoScript, bool>(draggingCAnonStorey0.\u003C\u003Em__0)))
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              Undo.IncrementCurrentGroup();
              int currentGroup = Undo.GetCurrentGroup();
              int num = 0;
              Component[] components = new Component[array1.Length * source.Count<MonoScript>()];
              foreach (Component component in array1)
              {
                GameObject gameObject = component.gameObject;
                foreach (MonoScript monoScript in source)
                  components[num++] = Undo.AddComponent(gameObject, monoScript.GetClass());
              }
              if (!ComponentUtility.MoveComponentsRelativeToComponents(components, array1, this.m_TargetAbove))
                Undo.RevertAllDownToGroup(currentGroup);
            }
          }
          else
          {
            Component[] array2 = DragAndDrop.objectReferences.Cast<Component>().ToArray<Component>();
            if (array2.Length == 0 || array1.Length == 0)
              break;
            this.MoveOrCopyComponents(array2, array1, EditorUtility.EventHasDragCopyModifierPressed(current), false);
          }
          this.m_TargetIndex = -1;
          DragAndDrop.AcceptDrag();
          current.Use();
          GUIUtility.ExitGUI();
          break;
        default:
          if (type != EventType.DragExited)
            break;
          this.m_TargetIndex = -1;
          break;
      }
    }

    private bool MoveOrCopyComponents(Component[] sourceComponents, Component[] targetComponents, bool copy, bool validateOnly)
    {
      if (copy)
        return false;
      if (sourceComponents.Length != 1 || targetComponents.Length != 1)
        return ComponentUtility.MoveComponentsRelativeToComponents(sourceComponents, targetComponents, this.m_TargetAbove, validateOnly);
      if ((UnityEngine.Object) sourceComponents[0].gameObject != (UnityEngine.Object) targetComponents[0].gameObject)
        return false;
      return ComponentUtility.MoveComponentRelativeToComponent(sourceComponents[0], targetComponents[0], this.m_TargetAbove, validateOnly);
    }

    private enum DraggingMode
    {
      NotApplicable,
      Component,
      Script,
    }

    private static class Styles
    {
      public static readonly GUIStyle insertionMarker = (GUIStyle) "InsertionMarker";
    }
  }
}
