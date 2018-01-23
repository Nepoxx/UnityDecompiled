// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RenderPassAttachment
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
  [NativeType("Runtime/Graphics/ScriptableRenderLoop/ScriptableRenderContext.h")]
  public class RenderPassAttachment : Object
  {
    public RenderPassAttachment(RenderTextureFormat fmt)
    {
      RenderPassAttachment.Internal_CreateAttachment(this);
      this.loadAction = RenderBufferLoadAction.DontCare;
      this.storeAction = RenderBufferStoreAction.DontCare;
      this.format = fmt;
      this.loadStoreTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.None);
      this.resolveTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.None);
      this.clearColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      this.clearDepth = 1f;
    }

    public extern RenderBufferLoadAction loadAction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

    public extern RenderBufferStoreAction storeAction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

    public extern RenderTextureFormat format { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

    private RenderTargetIdentifier loadStoreTarget
    {
      get
      {
        RenderTargetIdentifier ret;
        this.get_loadStoreTarget_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_loadStoreTarget_Injected(ref value);
      }
    }

    private RenderTargetIdentifier resolveTarget
    {
      get
      {
        RenderTargetIdentifier ret;
        this.get_resolveTarget_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_resolveTarget_Injected(ref value);
      }
    }

    public Color clearColor
    {
      get
      {
        Color ret;
        this.get_clearColor_Injected(out ret);
        return ret;
      }
      private set
      {
        this.set_clearColor_Injected(ref value);
      }
    }

    public extern float clearDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

    public extern uint clearStencil { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

    public void BindSurface(RenderTargetIdentifier tgt, bool loadExistingContents, bool storeResults)
    {
      this.loadStoreTarget = tgt;
      if (loadExistingContents && this.loadAction != RenderBufferLoadAction.Clear)
        this.loadAction = RenderBufferLoadAction.Load;
      if (!storeResults)
        return;
      this.storeAction = this.storeAction == RenderBufferStoreAction.StoreAndResolve || this.storeAction == RenderBufferStoreAction.Resolve ? RenderBufferStoreAction.StoreAndResolve : RenderBufferStoreAction.Store;
    }

    public void BindResolveSurface(RenderTargetIdentifier tgt)
    {
      this.resolveTarget = tgt;
      if (this.storeAction == RenderBufferStoreAction.StoreAndResolve || this.storeAction == RenderBufferStoreAction.Store)
        this.storeAction = RenderBufferStoreAction.StoreAndResolve;
      else
        this.storeAction = RenderBufferStoreAction.Resolve;
    }

    public void Clear(Color clearCol, float clearDep = 1f, uint clearStenc = 0)
    {
      this.clearColor = clearCol;
      this.clearDepth = clearDep;
      this.clearStencil = clearStenc;
      this.loadAction = RenderBufferLoadAction.Clear;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Internal_CreateAttachment([Writable] RenderPassAttachment self);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_loadStoreTarget_Injected(out RenderTargetIdentifier ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_loadStoreTarget_Injected(ref RenderTargetIdentifier value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_resolveTarget_Injected(out RenderTargetIdentifier ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_resolveTarget_Injected(ref RenderTargetIdentifier value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_clearColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_clearColor_Injected(ref Color value);
  }
}
