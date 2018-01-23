// Decompiled with JetBrains decompiler
// Type: UnityEngine.ObjectGUIState
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  internal class ObjectGUIState : IDisposable
  {
    internal IntPtr m_Ptr;

    public ObjectGUIState()
    {
      this.m_Ptr = ObjectGUIState.Internal_Create();
    }

    public void Dispose()
    {
      this.Destroy();
      GC.SuppressFinalize((object) this);
    }

    ~ObjectGUIState()
    {
      this.Destroy();
    }

    private void Destroy()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      ObjectGUIState.Internal_Destroy(this.m_Ptr);
      this.m_Ptr = IntPtr.Zero;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Internal_Create();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Destroy(IntPtr ptr);
  }
}
