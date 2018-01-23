// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  internal class GUIView : View
  {
    private int m_DepthBufferBits = 0;
    private bool m_AutoRepaintOnSceneChange = false;
    private bool m_BackgroundValid = false;
    private EventInterests m_EventInterests;

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetTitle(string title);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_Init(int depthBits);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_Recreate(int depthBits);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_Close();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_SendEvent(Event e);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void AddToAuxWindowList();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void RemoveFromAuxWindowList();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    protected extern void Internal_SetAsActiveWindow();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetWantsMouseMove(bool wantIt);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetWantsMouseEnterLeaveWindow(bool wantIt);

    public void SetInternalGameViewDimensions(Rect rect, Rect clippedRect, Vector2 targetSize)
    {
      GUIView.INTERNAL_CALL_SetInternalGameViewDimensions(this, ref rect, ref clippedRect, ref targetSize);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetInternalGameViewDimensions(GUIView self, ref Rect rect, ref Rect clippedRect, ref Vector2 targetSize);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetAsStartView();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearStartView();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetAutoRepaint(bool doit);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetWindow(ContainerWindow win);

    private void Internal_SetPosition(Rect windowPosition)
    {
      GUIView.INTERNAL_CALL_Internal_SetPosition(this, ref windowPosition);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetPosition(GUIView self, ref Rect windowPosition);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Focus();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Repaint();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void RepaintImmediately();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void CaptureRenderDoc();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void MakeVistaDWMHappyDance();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void StealMouseCapture();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void ClearKeyboardControl();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void SetKeyboardControl(int id);

    public static extern GUIView current { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern GUIView focusedView { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern GUIView mouseOverView { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern bool hasFocus { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal extern bool mouseRayInvisible { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal void GrabPixels(RenderTexture rd, Rect rect)
    {
      GUIView.INTERNAL_CALL_GrabPixels(this, rd, ref rect);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GrabPixels(GUIView self, RenderTexture rd, ref Rect rect);

    internal static event Action<GUIView> positionChanged = null;

    protected Panel panel
    {
      get
      {
        if (Panel.loadResourceFunc == null)
        {
          if (GUIView.\u003C\u003Ef__mg\u0024cache0 == null)
            GUIView.\u003C\u003Ef__mg\u0024cache0 = new LoadResourceFunction(StyleSheetResourceUtil.LoadResource);
          Panel.loadResourceFunc = GUIView.\u003C\u003Ef__mg\u0024cache0;
        }
        Panel orCreatePanel = UIElementsUtility.FindOrCreatePanel((ScriptableObject) this, ContextType.Editor, (IDataWatchService) DataWatchService.sharedInstance);
        GUIView.AddDefaultEditorStyleSheets(orCreatePanel.visualTree);
        return orCreatePanel;
      }
    }

    internal static void AddDefaultEditorStyleSheets(VisualElement p)
    {
      if (p.styleSheets != null)
        return;
      p.AddStyleSheetPath("StyleSheets/DefaultCommon.uss");
      if (EditorGUIUtility.isProSkin)
        p.AddStyleSheetPath("StyleSheets/DefaultCommonDark.uss");
      else
        p.AddStyleSheetPath("StyleSheets/DefaultCommonLight.uss");
    }

    public VisualElement visualTree
    {
      get
      {
        return this.panel.visualTree;
      }
    }

    protected IMGUIContainer imguiContainer { get; private set; }

    internal bool SendEvent(Event e)
    {
      bool flag;
      if (SavedGUIState.Internal_GetGUIDepth() > 0)
      {
        SavedGUIState savedGuiState = SavedGUIState.Create();
        flag = this.Internal_SendEvent(e);
        savedGuiState.ApplyAndForget();
      }
      else
        flag = this.Internal_SendEvent(e);
      return flag;
    }

    protected override void SetWindow(ContainerWindow win)
    {
      base.SetWindow(win);
      this.Internal_Init(this.m_DepthBufferBits);
      if ((bool) ((UnityEngine.Object) win))
        this.Internal_SetWindow(win);
      this.Internal_SetAutoRepaint(this.m_AutoRepaintOnSceneChange);
      this.Internal_SetPosition(this.windowPosition);
      this.Internal_SetWantsMouseMove(this.m_EventInterests.wantsMouseMove);
      this.Internal_SetWantsMouseEnterLeaveWindow(this.m_EventInterests.wantsMouseMove);
      this.panel.visualTree.SetSize(this.windowPosition.size);
      this.m_BackgroundValid = false;
    }

    internal void RecreateContext()
    {
      this.Internal_Recreate(this.m_DepthBufferBits);
      this.m_BackgroundValid = false;
    }

    public EventInterests eventInterests
    {
      get
      {
        return this.m_EventInterests;
      }
      set
      {
        this.m_EventInterests = value;
        this.panel.IMGUIEventInterests = this.m_EventInterests;
        this.Internal_SetWantsMouseMove(this.wantsMouseMove);
        this.Internal_SetWantsMouseEnterLeaveWindow(this.wantsMouseEnterLeaveWindow);
      }
    }

    public bool wantsMouseMove
    {
      get
      {
        return this.m_EventInterests.wantsMouseMove;
      }
      set
      {
        this.m_EventInterests.wantsMouseMove = value;
        this.panel.IMGUIEventInterests = this.m_EventInterests;
        this.Internal_SetWantsMouseMove(this.wantsMouseMove);
      }
    }

    public bool wantsMouseEnterLeaveWindow
    {
      get
      {
        return this.m_EventInterests.wantsMouseEnterLeaveWindow;
      }
      set
      {
        this.m_EventInterests.wantsMouseEnterLeaveWindow = value;
        this.panel.IMGUIEventInterests = this.m_EventInterests;
        this.Internal_SetWantsMouseEnterLeaveWindow(this.wantsMouseEnterLeaveWindow);
      }
    }

    internal bool backgroundValid
    {
      get
      {
        return this.m_BackgroundValid;
      }
      set
      {
        this.m_BackgroundValid = value;
      }
    }

    public bool autoRepaintOnSceneChange
    {
      get
      {
        return this.m_AutoRepaintOnSceneChange;
      }
      set
      {
        this.m_AutoRepaintOnSceneChange = value;
        this.Internal_SetAutoRepaint(this.m_AutoRepaintOnSceneChange);
      }
    }

    public int depthBufferBits
    {
      get
      {
        return this.m_DepthBufferBits;
      }
      set
      {
        this.m_DepthBufferBits = value;
      }
    }

    [Obsolete("AA is not supported on GUIViews", false)]
    public int antiAlias
    {
      get
      {
        return 1;
      }
      set
      {
      }
    }

    protected virtual void OnEnable()
    {
      this.imguiContainer = new IMGUIContainer(new Action(this.OldOnGUI))
      {
        useOwnerObjectGUIState = true
      };
      this.imguiContainer.StretchToParentSize();
      this.imguiContainer.persistenceKey = "Dockarea";
      this.visualTree.Insert(0, (VisualElement) this.imguiContainer);
    }

    protected virtual void OnDisable()
    {
      if (this.imguiContainer.HasCapture())
        this.imguiContainer.RemoveCapture();
      this.visualTree.Remove((VisualElement) this.imguiContainer);
    }

    protected virtual void OldOnGUI()
    {
    }

    protected virtual void OnGUI()
    {
    }

    protected override void SetPosition(Rect newPos)
    {
      Rect windowPosition = this.windowPosition;
      base.SetPosition(newPos);
      if (windowPosition == this.windowPosition)
      {
        this.Internal_SetPosition(this.windowPosition);
      }
      else
      {
        this.Internal_SetPosition(this.windowPosition);
        this.m_BackgroundValid = false;
        this.panel.visualTree.SetSize(this.windowPosition.size);
        // ISSUE: reference to a compiler-generated field
        if (GUIView.positionChanged != null)
        {
          // ISSUE: reference to a compiler-generated field
          GUIView.positionChanged(this);
        }
        this.Repaint();
      }
    }

    protected override void OnDestroy()
    {
      this.Internal_Close();
      base.OnDestroy();
    }

    internal void DoWindowDecorationStart()
    {
      if (!((UnityEngine.Object) this.window != (UnityEngine.Object) null))
        return;
      this.window.HandleWindowDecorationStart(this.windowPosition);
    }

    internal void DoWindowDecorationEnd()
    {
      if (!((UnityEngine.Object) this.window != (UnityEngine.Object) null))
        return;
      this.window.HandleWindowDecorationEnd(this.windowPosition);
    }
  }
}
