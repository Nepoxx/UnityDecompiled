// Decompiled with JetBrains decompiler
// Type: UnityEngine.GlobalJavaObjectRef
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal class GlobalJavaObjectRef
  {
    private bool m_disposed = false;
    protected IntPtr m_jobject;

    public GlobalJavaObjectRef(IntPtr jobject)
    {
      this.m_jobject = !(jobject == IntPtr.Zero) ? AndroidJNI.NewGlobalRef(jobject) : IntPtr.Zero;
    }

    ~GlobalJavaObjectRef()
    {
      this.Dispose();
    }

    public static implicit operator IntPtr(GlobalJavaObjectRef obj)
    {
      return obj.m_jobject;
    }

    public void Dispose()
    {
      if (this.m_disposed)
        return;
      this.m_disposed = true;
      if (!(this.m_jobject != IntPtr.Zero))
        return;
      AndroidJNISafe.DeleteGlobalRef(this.m_jobject);
    }
  }
}
