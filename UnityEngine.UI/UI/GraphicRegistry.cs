// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.GraphicRegistry
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Registry which maps a Graphic to the canvas it belongs to.</para>
  /// </summary>
  public class GraphicRegistry
  {
    private static readonly List<Graphic> s_EmptyList = new List<Graphic>();
    private readonly Dictionary<Canvas, IndexedSet<Graphic>> m_Graphics = new Dictionary<Canvas, IndexedSet<Graphic>>();
    private static GraphicRegistry s_Instance;

    protected GraphicRegistry()
    {
    }

    /// <summary>
    ///   <para>Singleton instance.</para>
    /// </summary>
    public static GraphicRegistry instance
    {
      get
      {
        if (GraphicRegistry.s_Instance == null)
          GraphicRegistry.s_Instance = new GraphicRegistry();
        return GraphicRegistry.s_Instance;
      }
    }

    /// <summary>
    ///   <para>Store a link between the given canvas and graphic in the registry.</para>
    /// </summary>
    /// <param name="c">Canvas to register.</param>
    /// <param name="graphic">Graphic to register.</param>
    public static void RegisterGraphicForCanvas(Canvas c, Graphic graphic)
    {
      if ((Object) c == (Object) null)
        return;
      IndexedSet<Graphic> indexedSet;
      GraphicRegistry.instance.m_Graphics.TryGetValue(c, out indexedSet);
      if (indexedSet != null)
        indexedSet.AddUnique(graphic);
      else
        GraphicRegistry.instance.m_Graphics.Add(c, new IndexedSet<Graphic>()
        {
          graphic
        });
    }

    /// <summary>
    ///   <para>Deregister the given Graphic from a Canvas.</para>
    /// </summary>
    /// <param name="c">Canvas.</param>
    /// <param name="graphic">Graphic to deregister.</param>
    public static void UnregisterGraphicForCanvas(Canvas c, Graphic graphic)
    {
      IndexedSet<Graphic> indexedSet;
      if ((Object) c == (Object) null || !GraphicRegistry.instance.m_Graphics.TryGetValue(c, out indexedSet))
        return;
      indexedSet.Remove(graphic);
      if (indexedSet.Count == 0)
        GraphicRegistry.instance.m_Graphics.Remove(c);
    }

    /// <summary>
    ///   <para>Return a list of Graphics that are registered on the Canvas.</para>
    /// </summary>
    /// <param name="canvas">Input canvas.</param>
    /// <returns>
    ///   <para>Graphics on the input canvas.</para>
    /// </returns>
    public static IList<Graphic> GetGraphicsForCanvas(Canvas canvas)
    {
      IndexedSet<Graphic> indexedSet;
      if (GraphicRegistry.instance.m_Graphics.TryGetValue(canvas, out indexedSet))
        return (IList<Graphic>) indexedSet;
      return (IList<Graphic>) GraphicRegistry.s_EmptyList;
    }
  }
}
