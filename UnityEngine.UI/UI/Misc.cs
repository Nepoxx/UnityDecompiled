// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Misc
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  internal static class Misc
  {
    public static void Destroy(Object obj)
    {
      if (!(obj != (Object) null))
        return;
      if (Application.isPlaying)
      {
        if (obj is GameObject)
          (obj as GameObject).transform.parent = (Transform) null;
        Object.Destroy(obj);
      }
      else
        Object.DestroyImmediate(obj);
    }

    public static void DestroyImmediate(Object obj)
    {
      if (!(obj != (Object) null))
        return;
      if (Application.isEditor)
        Object.DestroyImmediate(obj);
      else
        Object.Destroy(obj);
    }
  }
}
