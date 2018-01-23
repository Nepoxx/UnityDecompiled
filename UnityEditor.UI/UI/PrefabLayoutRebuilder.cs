// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.PrefabLayoutRebuilder
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DDF3271B-B3CA-41C7-8FD7-FD00990E91BF
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\Editor\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  [InitializeOnLoad]
  internal class PrefabLayoutRebuilder
  {
    static PrefabLayoutRebuilder()
    {
      PrefabUtility.PrefabInstanceUpdated prefabInstanceUpdated = PrefabUtility.prefabInstanceUpdated;
      // ISSUE: reference to a compiler-generated field
      if (PrefabLayoutRebuilder.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabLayoutRebuilder.\u003C\u003Ef__mg\u0024cache0 = new PrefabUtility.PrefabInstanceUpdated(PrefabLayoutRebuilder.OnPrefabInstanceUpdates);
      }
      // ISSUE: reference to a compiler-generated field
      PrefabUtility.PrefabInstanceUpdated fMgCache0 = PrefabLayoutRebuilder.\u003C\u003Ef__mg\u0024cache0;
      PrefabUtility.prefabInstanceUpdated = prefabInstanceUpdated + fMgCache0;
    }

    private static void OnPrefabInstanceUpdates(GameObject instance)
    {
      if (!(bool) ((Object) instance))
        return;
      RectTransform transform = instance.transform as RectTransform;
      if ((bool) ((Object) transform))
        LayoutRebuilder.MarkLayoutForRebuild(transform);
    }
  }
}
