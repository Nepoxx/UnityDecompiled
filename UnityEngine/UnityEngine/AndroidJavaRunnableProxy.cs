// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaRunnableProxy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  internal class AndroidJavaRunnableProxy : AndroidJavaProxy
  {
    private AndroidJavaRunnable mRunnable;

    public AndroidJavaRunnableProxy(AndroidJavaRunnable runnable)
      : base("java/lang/Runnable")
    {
      this.mRunnable = runnable;
    }

    public void run()
    {
      this.mRunnable();
    }
  }
}
