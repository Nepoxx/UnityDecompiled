// Decompiled with JetBrains decompiler
// Type: UnityEngine.WaitForSecondsRealtime
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public class WaitForSecondsRealtime : CustomYieldInstruction
  {
    private float waitTime;

    public WaitForSecondsRealtime(float time)
    {
      this.waitTime = Time.realtimeSinceStartup + time;
    }

    public override bool keepWaiting
    {
      get
      {
        return (double) Time.realtimeSinceStartup < (double) this.waitTime;
      }
    }
  }
}
