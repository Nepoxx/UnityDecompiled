// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.ContextualMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
  internal class ContextualMenu : Manipulator
  {
    private List<ContextualMenu.Action> menuActions = new List<ContextualMenu.Action>();

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.Capture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.Capture);
    }

    private void OnIMGUIEvent(IMGUIEvent evt)
    {
      if (evt.imguiEvent.type != EventType.ContextClick)
        return;
      GenericMenu genericMenu = new GenericMenu();
      foreach (ContextualMenu.Action menuAction in this.menuActions)
      {
        if (menuAction.enabled)
          genericMenu.AddItem(menuAction.name, false, menuAction.action);
        else
          genericMenu.AddDisabledItem(menuAction.name);
      }
      genericMenu.ShowAsContext();
    }

    public void AddAction(string actionName, GenericMenu.MenuFunction action, ContextualMenu.ActionStatusCallback actionStatusCallback)
    {
      ContextualMenu.ActionStatus actionStatus = actionStatusCallback == null ? ContextualMenu.ActionStatus.Off : actionStatusCallback();
      if (actionStatus <= ContextualMenu.ActionStatus.Off)
        return;
      this.menuActions.Add(new ContextualMenu.Action()
      {
        name = new GUIContent(actionName),
        action = action,
        enabled = actionStatus == ContextualMenu.ActionStatus.Enabled
      });
    }

    private struct Action
    {
      public GUIContent name;
      public GenericMenu.MenuFunction action;
      public bool enabled;
    }

    public enum ActionStatus
    {
      Off,
      Enabled,
      Disabled,
    }

    public delegate ContextualMenu.ActionStatus ActionStatusCallback();
  }
}
