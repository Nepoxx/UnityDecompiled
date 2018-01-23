// Decompiled with JetBrains decompiler
// Type: UnityEngine.StaticBatchingUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine
{
  public sealed class StaticBatchingUtility
  {
    public static void Combine(GameObject staticBatchRoot)
    {
      InternalStaticBatchingUtility.CombineRoot(staticBatchRoot);
    }

    public static void Combine(GameObject[] gos, GameObject staticBatchRoot)
    {
      InternalStaticBatchingUtility.CombineGameObjects(gos, staticBatchRoot, false);
    }
  }
}
