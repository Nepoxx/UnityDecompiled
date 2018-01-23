// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.GraphicRebuildTracker
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>EditorOnly class for tracking all Graphics.</para>
  /// </summary>
  public static class GraphicRebuildTracker
  {
    private static IndexedSet<Graphic> m_Tracked = new IndexedSet<Graphic>();
    private static bool s_Initialized;

    /// <summary>
    ///   <para>Track a Graphic.</para>
    /// </summary>
    /// <param name="g"></param>
    public static void TrackGraphic(Graphic g)
    {
      if (!GraphicRebuildTracker.s_Initialized)
      {
        // ISSUE: reference to a compiler-generated field
        if (GraphicRebuildTracker.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GraphicRebuildTracker.\u003C\u003Ef__mg\u0024cache0 = new CanvasRenderer.OnRequestRebuild(GraphicRebuildTracker.OnRebuildRequested);
        }
        // ISSUE: reference to a compiler-generated field
        CanvasRenderer.onRequestRebuild += GraphicRebuildTracker.\u003C\u003Ef__mg\u0024cache0;
        GraphicRebuildTracker.s_Initialized = true;
      }
      GraphicRebuildTracker.m_Tracked.AddUnique(g);
    }

    /// <summary>
    ///   <para>Untrack a Graphic.</para>
    /// </summary>
    /// <param name="g"></param>
    public static void UnTrackGraphic(Graphic g)
    {
      GraphicRebuildTracker.m_Tracked.Remove(g);
    }

    private static void OnRebuildRequested()
    {
      StencilMaterial.ClearAll();
      for (int index = 0; index < GraphicRebuildTracker.m_Tracked.Count; ++index)
        GraphicRebuildTracker.m_Tracked[index].OnRebuildRequested();
    }
  }
}
