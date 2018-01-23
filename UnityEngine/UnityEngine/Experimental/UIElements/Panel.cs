// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Panel
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  internal class Panel : BaseVisualElementPanel
  {
    internal static LoadResourceFunction loadResourceFunc = (LoadResourceFunction) null;
    private StyleContext m_StyleContext;
    private VisualElement m_RootContainer;
    private IDataWatchService m_DataWatch;
    private TimerEventScheduler m_Scheduler;
    private bool m_KeepPixelCacheOnWorldBoundChange;
    private const int kMaxValidatePersistentDataCount = 5;
    private const int kMaxValidateLayoutCount = 5;

    public Panel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null, IEventDispatcher dispatcher = null)
    {
      this.ownerObject = ownerObject;
      this.contextType = contextType;
      this.m_DataWatch = dataWatch;
      this.dispatcher = dispatcher;
      this.stylePainter = (IStylePainter) new StylePainter();
      this.m_RootContainer = new VisualElement();
      this.m_RootContainer.name = VisualElementUtils.GetUniqueName("PanelContainer");
      this.m_RootContainer.persistenceKey = "PanelContainer";
      this.visualTree.ChangePanel((BaseVisualElementPanel) this);
      this.focusController = new FocusController((IFocusRing) new VisualElementFocusRing(this.visualTree, VisualElementFocusRing.DefaultFocusOrder.ChildOrder));
      this.m_StyleContext = new StyleContext(this.m_RootContainer);
      this.allowPixelCaching = true;
    }

    public override VisualElement visualTree
    {
      get
      {
        return this.m_RootContainer;
      }
    }

    public override IEventDispatcher dispatcher { get; protected set; }

    internal override IDataWatchService dataWatch
    {
      get
      {
        return this.m_DataWatch;
      }
    }

    public TimerEventScheduler timerEventScheduler
    {
      get
      {
        return this.m_Scheduler ?? (this.m_Scheduler = new TimerEventScheduler());
      }
    }

    internal override IScheduler scheduler
    {
      get
      {
        return (IScheduler) this.timerEventScheduler;
      }
    }

    internal StyleContext styleContext
    {
      get
      {
        return this.m_StyleContext;
      }
    }

    public override ScriptableObject ownerObject { get; protected set; }

    public bool allowPixelCaching { get; set; }

    public override ContextType contextType { get; protected set; }

    public override SavePersistentViewData savePersistentViewData { get; set; }

    public override GetViewDataDictionary getViewDataDictionary { get; set; }

    public override FocusController focusController { get; set; }

    public override EventInterests IMGUIEventInterests { get; set; }

    public override bool keepPixelCacheOnWorldBoundChange
    {
      get
      {
        return this.m_KeepPixelCacheOnWorldBoundChange;
      }
      set
      {
        if (this.m_KeepPixelCacheOnWorldBoundChange == value)
          return;
        this.m_KeepPixelCacheOnWorldBoundChange = value;
        if (value)
          return;
        this.m_RootContainer.Dirty(ChangeType.Transform | ChangeType.Repaint);
      }
    }

    public override int IMGUIContainersCount { get; set; }

    private VisualElement PickAll(VisualElement root, Vector2 point, List<VisualElement> picked = null)
    {
      if ((root.pseudoStates & PseudoStates.Invisible) == PseudoStates.Invisible)
        return (VisualElement) null;
      Vector3 vector3_1 = root.transform.matrix.inverse.MultiplyPoint3x4((Vector3) point);
      bool flag = root.ContainsPoint((Vector2) vector3_1);
      if (!flag && root.clippingOptions != VisualElement.ClippingOptions.NoClipping)
        return (VisualElement) null;
      if (picked != null && root.enabledInHierarchy && root.pickingMode == PickingMode.Position)
        picked.Add(root);
      Vector3 vector3_2 = vector3_1 - new Vector3(root.layout.position.x, root.layout.position.y, 0.0f);
      VisualElement visualElement1 = (VisualElement) null;
      for (int index = root.shadow.childCount - 1; index >= 0; --index)
      {
        VisualElement visualElement2 = this.PickAll(root.shadow[index], (Vector2) vector3_2, picked);
        if (visualElement1 == null && visualElement2 != null)
          visualElement1 = visualElement2;
      }
      if (visualElement1 != null)
        return visualElement1;
      switch (root.pickingMode)
      {
        case PickingMode.Position:
          if (flag && root.enabledInHierarchy)
            return root;
          break;
      }
      return (VisualElement) null;
    }

    public override VisualElement LoadTemplate(string path, Dictionary<string, VisualElement> slots = null)
    {
      VisualTreeAsset visualTreeAsset = Panel.loadResourceFunc(path, typeof (VisualTreeAsset)) as VisualTreeAsset;
      if ((UnityEngine.Object) visualTreeAsset == (UnityEngine.Object) null)
        return (VisualElement) null;
      return visualTreeAsset.CloneTree(slots);
    }

    public override VisualElement PickAll(Vector2 point, List<VisualElement> picked)
    {
      this.ValidateLayout();
      if (picked != null)
        picked.Clear();
      return this.PickAll(this.visualTree, point, picked);
    }

    public override VisualElement Pick(Vector2 point)
    {
      this.ValidateLayout();
      return this.PickAll(this.visualTree, point, (List<VisualElement>) null);
    }

    private void ValidatePersistentData()
    {
      int num = 0;
      while (this.visualTree.AnyDirty(ChangeType.PersistentData | ChangeType.PersistentDataPath))
      {
        this.ValidatePersistentDataOnSubTree(this.visualTree, true);
        ++num;
        if (num > 5)
        {
          Debug.LogError((object) ("UIElements: Too many children recursively added that rely on persistent data: " + (object) this.visualTree));
          break;
        }
      }
    }

    private void ValidatePersistentDataOnSubTree(VisualElement root, bool enablePersistence)
    {
      if (!root.IsPersitenceSupportedOnChildren())
        enablePersistence = false;
      if (root.IsDirty(ChangeType.PersistentData))
      {
        root.OnPersistentDataReady(enablePersistence);
        root.ClearDirty(ChangeType.PersistentData);
      }
      if (!root.IsDirty(ChangeType.PersistentDataPath))
        return;
      for (int index = 0; index < root.shadow.childCount; ++index)
        this.ValidatePersistentDataOnSubTree(root.shadow[index], enablePersistence);
      root.ClearDirty(ChangeType.PersistentDataPath);
    }

    private void ValidateStyling()
    {
      if (!Mathf.Approximately(this.m_StyleContext.currentPixelsPerPoint, GUIUtility.pixelsPerPoint))
      {
        this.m_RootContainer.Dirty(ChangeType.Styles);
        this.m_StyleContext.currentPixelsPerPoint = GUIUtility.pixelsPerPoint;
      }
      if (!this.m_RootContainer.AnyDirty(ChangeType.Styles | ChangeType.StylesPath))
        return;
      this.m_StyleContext.ApplyStyles();
    }

    public override void ValidateLayout()
    {
      this.ValidateStyling();
      int num = 0;
      while (this.visualTree.cssNode.IsDirty)
      {
        this.visualTree.cssNode.CalculateLayout();
        this.ValidateSubTree(this.visualTree);
        if (num++ >= 5)
        {
          Debug.LogError((object) ("ValidateLayout is struggling to process current layout (consider simplifying to avoid recursive layout): " + (object) this.visualTree));
          break;
        }
      }
    }

    private bool ValidateSubTree(VisualElement root)
    {
      if (root.renderData.lastLayout != new Rect(root.cssNode.LayoutX, root.cssNode.LayoutY, root.cssNode.LayoutWidth, root.cssNode.LayoutHeight))
      {
        root.Dirty(ChangeType.Transform);
        root.renderData.lastLayout = new Rect(root.cssNode.LayoutX, root.cssNode.LayoutY, root.cssNode.LayoutWidth, root.cssNode.LayoutHeight);
      }
      bool hasNewLayout = root.cssNode.HasNewLayout;
      if (hasNewLayout)
      {
        for (int index = 0; index < root.shadow.childCount; ++index)
          this.ValidateSubTree(root.shadow[index]);
      }
      PostLayoutEvent pooled = PostLayoutEvent.GetPooled(hasNewLayout);
      pooled.target = (IEventHandler) root;
      UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, (IPanel) this);
      EventBase<PostLayoutEvent>.ReleasePooled(pooled);
      root.ClearDirty(ChangeType.Layout);
      root.cssNode.MarkLayoutSeen();
      return hasNewLayout;
    }

    private Rect ComputeAAAlignedBound(Rect position, Matrix4x4 mat)
    {
      Rect rect = position;
      Vector3 vector3_1 = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y, 0.0f));
      Vector3 vector3_2 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y, 0.0f));
      Vector3 vector3_3 = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y + rect.height, 0.0f));
      Vector3 vector3_4 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y + rect.height, 0.0f));
      return Rect.MinMaxRect(Mathf.Min(vector3_1.x, Mathf.Min(vector3_2.x, Mathf.Min(vector3_3.x, vector3_4.x))), Mathf.Min(vector3_1.y, Mathf.Min(vector3_2.y, Mathf.Min(vector3_3.y, vector3_4.y))), Mathf.Max(vector3_1.x, Mathf.Max(vector3_2.x, Mathf.Max(vector3_3.x, vector3_4.x))), Mathf.Max(vector3_1.y, Mathf.Max(vector3_2.y, Mathf.Max(vector3_3.y, vector3_4.y))));
    }

    private bool ShouldUsePixelCache(VisualElement root)
    {
      return this.allowPixelCaching && root.clippingOptions == VisualElement.ClippingOptions.ClipAndCacheContents && (root.panel.panelDebug == null || !root.panel.panelDebug.RecordRepaint(root)) && (double) root.worldBound.size.magnitude > (double) Mathf.Epsilon;
    }

    public void PaintSubTree(Event e, VisualElement root, Matrix4x4 offset, Rect currentGlobalClip)
    {
      if ((root.pseudoStates & PseudoStates.Invisible) == PseudoStates.Invisible)
        return;
      if (root.clippingOptions != VisualElement.ClippingOptions.NoClipping)
      {
        Rect aaAlignedBound = this.ComputeAAAlignedBound(root.layout, offset * root.worldTransform);
        if (!aaAlignedBound.Overlaps(currentGlobalClip))
          return;
        float x = Mathf.Max(aaAlignedBound.x, currentGlobalClip.x);
        float num1 = Mathf.Min(aaAlignedBound.x + aaAlignedBound.width, currentGlobalClip.x + currentGlobalClip.width);
        float y = Mathf.Max(aaAlignedBound.y, currentGlobalClip.y);
        float num2 = Mathf.Min(aaAlignedBound.y + aaAlignedBound.height, currentGlobalClip.y + currentGlobalClip.height);
        currentGlobalClip = new Rect(x, y, num1 - x, num2 - y);
      }
      if (this.ShouldUsePixelCache(root))
      {
        IStylePainter stylePainter = this.stylePainter;
        Rect worldBound = root.worldBound;
        int width1 = (int) worldBound.width;
        int height1 = (int) worldBound.height;
        int width2 = (int) ((double) worldBound.width * (double) GUIUtility.pixelsPerPoint);
        int height2 = (int) ((double) worldBound.height * (double) GUIUtility.pixelsPerPoint);
        RenderTexture renderTexture = root.renderData.pixelCache;
        if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null && !this.keepPixelCacheOnWorldBoundChange && (renderTexture.width != width2 || renderTexture.height != height2))
        {
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) renderTexture);
          renderTexture = root.renderData.pixelCache = (RenderTexture) null;
        }
        float opacity = this.stylePainter.opacity;
        if (root.IsDirty(ChangeType.Repaint) || (UnityEngine.Object) root.renderData.pixelCache == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) renderTexture == (UnityEngine.Object) null)
            root.renderData.pixelCache = renderTexture = new RenderTexture(width2, height2, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
          RenderTexture active = RenderTexture.active;
          RenderTexture.active = renderTexture;
          GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
          offset = Matrix4x4.Translate(new Vector3(-worldBound.x, -worldBound.y, 0.0f));
          Rect rect = new Rect(0.0f, 0.0f, (float) width1, (float) height1);
          stylePainter.currentTransform = offset * root.worldTransform;
          GUIClip.SetTransform(stylePainter.currentTransform, rect);
          stylePainter.currentWorldClip = rect;
          root.DoRepaint(stylePainter);
          root.ClearDirty(ChangeType.Repaint);
          this.PaintSubTreeChildren(e, root, offset, rect);
          RenderTexture.active = active;
        }
        stylePainter.currentWorldClip = currentGlobalClip;
        stylePainter.currentTransform = root.worldTransform;
        GUIClip.SetTransform(stylePainter.currentTransform, currentGlobalClip);
        TextureStylePainterParameters painterParams = new TextureStylePainterParameters() { layout = root.layout, texture = (Texture) root.renderData.pixelCache, color = Color.white, scaleMode = ScaleMode.ScaleAndCrop };
        stylePainter.DrawTexture(painterParams);
      }
      else
      {
        this.stylePainter.currentTransform = offset * root.worldTransform;
        GUIClip.SetTransform(this.stylePainter.currentTransform, currentGlobalClip);
        this.stylePainter.currentWorldClip = currentGlobalClip;
        this.stylePainter.mousePosition = (Vector2) root.worldTransform.inverse.MultiplyPoint3x4((Vector3) e.mousePosition);
        this.stylePainter.opacity = root.style.opacity.GetSpecifiedValueOrDefault(1f);
        root.DoRepaint(this.stylePainter);
        this.stylePainter.opacity = 1f;
        root.ClearDirty(ChangeType.Repaint);
        this.PaintSubTreeChildren(e, root, offset, currentGlobalClip);
      }
    }

    private void PaintSubTreeChildren(Event e, VisualElement root, Matrix4x4 offset, Rect textureClip)
    {
      int childCount = root.shadow.childCount;
      for (int index = 0; index < childCount; ++index)
      {
        VisualElement root1 = root.shadow[index];
        this.PaintSubTree(e, root1, offset, textureClip);
        if (childCount != root.shadow.childCount)
          throw new NotImplementedException("Visual tree is read-only during repaint");
      }
    }

    public override void Repaint(Event e)
    {
      this.ValidatePersistentData();
      this.ValidateLayout();
      this.stylePainter.repaintEvent = e;
      Rect currentGlobalClip = this.visualTree.clippingOptions != VisualElement.ClippingOptions.NoClipping ? this.visualTree.layout : GUIClip.topmostRect;
      this.PaintSubTree(e, this.visualTree, Matrix4x4.identity, currentGlobalClip);
      if (this.panelDebug == null)
        return;
      GUIClip.SetTransform(Matrix4x4.identity, new Rect(0.0f, 0.0f, (float) this.visualTree.style.width, (float) this.visualTree.style.height));
      if (this.panelDebug.EndRepaint())
        this.visualTree.Dirty(ChangeType.Repaint);
    }
  }
}
