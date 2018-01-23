// Decompiled with JetBrains decompiler
// Type: UnityEditor.View
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  internal class View : ScriptableObject
  {
    [SerializeField]
    private View[] m_Children = new View[0];
    [SerializeField]
    private Rect m_Position = new Rect(0.0f, 0.0f, 100f, 100f);
    [SerializeField]
    private MonoReloadableIntPtr m_ViewPtr;
    [NonSerialized]
    private View m_Parent;
    [NonSerialized]
    private ContainerWindow m_Window;
    [SerializeField]
    internal Vector2 m_MinSize;
    [SerializeField]
    internal Vector2 m_MaxSize;

    internal virtual void Reflow()
    {
      foreach (View child in this.children)
        child.Reflow();
    }

    internal string DebugHierarchy(int level)
    {
      string str1 = "";
      string str2 = "";
      for (int index = 0; index < level; ++index)
        str1 += "  ";
      string str3 = str2 + str1 + this.ToString() + " p:" + (object) this.position;
      string str4;
      if (this.children.Length > 0)
      {
        string str5 = str3 + " {\n";
        foreach (View child in this.children)
          str5 += child.DebugHierarchy(level + 2);
        str4 = str5 + str1 + " }\n";
      }
      else
        str4 = str3 + "\n";
      return str4;
    }

    internal virtual void Initialize(ContainerWindow win)
    {
      this.SetWindow(win);
      foreach (View child in this.m_Children)
      {
        child.m_Parent = this;
        child.Initialize(win);
      }
    }

    public Vector2 minSize
    {
      get
      {
        return this.m_MinSize;
      }
    }

    public Vector2 maxSize
    {
      get
      {
        return this.m_MaxSize;
      }
    }

    internal void SetMinMaxSizes(Vector2 min, Vector2 max)
    {
      if (this.minSize == min && this.maxSize == max)
        return;
      this.m_MinSize = min;
      this.m_MaxSize = max;
      if ((bool) ((UnityEngine.Object) this.m_Parent))
        this.m_Parent.ChildrenMinMaxChanged();
      if (!(bool) ((UnityEngine.Object) this.window) || !((UnityEngine.Object) this.window.rootView == (UnityEngine.Object) this))
        return;
      this.window.SetMinMaxSizes(min, max);
    }

    protected virtual void ChildrenMinMaxChanged()
    {
    }

    public View[] allChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        foreach (View child in this.m_Children)
          arrayList.AddRange((ICollection) child.allChildren);
        arrayList.Add((object) this);
        return (View[]) arrayList.ToArray(typeof (View));
      }
    }

    private void __internalAwake()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    public Rect position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.SetPosition(value);
      }
    }

    protected virtual void SetPosition(Rect newPos)
    {
      this.m_Position = newPos;
    }

    internal void SetPositionOnly(Rect newPos)
    {
      this.m_Position = newPos;
    }

    public Rect windowPosition
    {
      get
      {
        if ((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null)
          return this.position;
        Rect windowPosition = this.parent.windowPosition;
        return new Rect(windowPosition.x + this.position.x, windowPosition.y + this.position.y, this.position.width, this.position.height);
      }
    }

    public Rect screenPosition
    {
      get
      {
        Rect windowPosition = this.windowPosition;
        if ((UnityEngine.Object) this.window != (UnityEngine.Object) null)
        {
          Vector2 screenPoint = this.window.WindowToScreenPoint(Vector2.zero);
          windowPosition.x += screenPoint.x;
          windowPosition.y += screenPoint.y;
        }
        return windowPosition;
      }
    }

    public ContainerWindow window
    {
      get
      {
        return this.m_Window;
      }
    }

    public View parent
    {
      get
      {
        return this.m_Parent;
      }
    }

    public View[] children
    {
      get
      {
        return this.m_Children;
      }
    }

    public int IndexOfChild(View child)
    {
      int num = 0;
      foreach (UnityEngine.Object child1 in this.m_Children)
      {
        if (child1 == (UnityEngine.Object) child)
          return num;
        ++num;
      }
      return -1;
    }

    protected virtual void OnDestroy()
    {
      foreach (UnityEngine.Object child in this.m_Children)
        UnityEngine.Object.DestroyImmediate(child, true);
    }

    public void AddChild(View child)
    {
      this.AddChild(child, this.m_Children.Length);
    }

    public virtual void AddChild(View child, int idx)
    {
      Array.Resize<View>(ref this.m_Children, this.m_Children.Length + 1);
      if (idx != this.m_Children.Length - 1)
        Array.Copy((Array) this.m_Children, idx, (Array) this.m_Children, idx + 1, this.m_Children.Length - idx - 1);
      this.m_Children[idx] = child;
      if ((bool) ((UnityEngine.Object) child.m_Parent))
        child.m_Parent.RemoveChild(child);
      child.m_Parent = this;
      child.SetWindowRecurse(this.window);
      this.ChildrenMinMaxChanged();
    }

    public virtual void RemoveChild(View child)
    {
      int idx = Array.IndexOf<View>(this.m_Children, child);
      if (idx == -1)
        Debug.LogError((object) "Unable to remove child - it's not IN the view");
      else
        this.RemoveChild(idx);
    }

    public virtual void RemoveChild(int idx)
    {
      View child = this.m_Children[idx];
      child.m_Parent = (View) null;
      child.SetWindowRecurse((ContainerWindow) null);
      Array.Copy((Array) this.m_Children, idx + 1, (Array) this.m_Children, idx, this.m_Children.Length - idx - 1);
      Array.Resize<View>(ref this.m_Children, this.m_Children.Length - 1);
      this.ChildrenMinMaxChanged();
    }

    protected virtual void SetWindow(ContainerWindow win)
    {
      this.m_Window = win;
    }

    internal void SetWindowRecurse(ContainerWindow win)
    {
      this.SetWindow(win);
      foreach (View child in this.m_Children)
        child.SetWindowRecurse(win);
    }

    protected virtual bool OnFocus()
    {
      return true;
    }
  }
}
