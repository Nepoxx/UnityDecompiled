// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindowManipulator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AnimationWindowManipulator
  {
    public AnimationWindowManipulator.OnStartDragDelegate onStartDrag;
    public AnimationWindowManipulator.OnDragDelegate onDrag;
    public AnimationWindowManipulator.OnEndDragDelegate onEndDrag;
    public Rect rect;
    public int controlID;

    public AnimationWindowManipulator()
    {
      this.onStartDrag += (AnimationWindowManipulator.OnStartDragDelegate) ((manipulator, evt) => false);
      this.onDrag += (AnimationWindowManipulator.OnDragDelegate) ((manipulator, evt) => false);
      this.onEndDrag += (AnimationWindowManipulator.OnEndDragDelegate) ((manipulator, evt) => false);
    }

    public virtual void HandleEvents()
    {
      this.controlID = GUIUtility.GetControlID(FocusType.Passive);
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(this.controlID);
      bool flag = false;
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.button == 0)
          {
            flag = this.onStartDrag(this, current);
            if (flag)
              GUIUtility.hotControl = this.controlID;
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == this.controlID)
          {
            flag = this.onEndDrag(this, current);
            GUIUtility.hotControl = 0;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == this.controlID)
          {
            flag = this.onDrag(this, current);
            break;
          }
          break;
      }
      if (!flag)
        return;
      current.Use();
    }

    public virtual void IgnoreEvents()
    {
      GUIUtility.GetControlID(FocusType.Passive);
    }

    public delegate bool OnStartDragDelegate(AnimationWindowManipulator manipulator, Event evt);

    public delegate bool OnDragDelegate(AnimationWindowManipulator manipulator, Event evt);

    public delegate bool OnEndDragDelegate(AnimationWindowManipulator manipulator, Event evt);
  }
}
