// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.FontUpdateTracker
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Utility class that is used to help with Text update.</para>
  /// </summary>
  public static class FontUpdateTracker
  {
    private static Dictionary<Font, HashSet<Text>> m_Tracked = new Dictionary<Font, HashSet<Text>>();

    /// <summary>
    ///   <para>Register a Text element for receiving texture atlas rebuild calls.</para>
    /// </summary>
    /// <param name="t"></param>
    public static void TrackText(Text t)
    {
      if ((UnityEngine.Object) t.font == (UnityEngine.Object) null)
        return;
      HashSet<Text> textSet;
      FontUpdateTracker.m_Tracked.TryGetValue(t.font, out textSet);
      if (textSet == null)
      {
        if (FontUpdateTracker.m_Tracked.Count == 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (FontUpdateTracker.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            FontUpdateTracker.\u003C\u003Ef__mg\u0024cache0 = new Action<Font>(FontUpdateTracker.RebuildForFont);
          }
          // ISSUE: reference to a compiler-generated field
          Font.textureRebuilt += FontUpdateTracker.\u003C\u003Ef__mg\u0024cache0;
        }
        textSet = new HashSet<Text>();
        FontUpdateTracker.m_Tracked.Add(t.font, textSet);
      }
      if (textSet.Contains(t))
        return;
      textSet.Add(t);
    }

    private static void RebuildForFont(Font f)
    {
      HashSet<Text> textSet;
      FontUpdateTracker.m_Tracked.TryGetValue(f, out textSet);
      if (textSet == null)
        return;
      foreach (Text text in textSet)
        text.FontTextureChanged();
    }

    /// <summary>
    ///   <para>Deregister a Text element from receiving texture atlas rebuild calls.</para>
    /// </summary>
    /// <param name="t"></param>
    public static void UntrackText(Text t)
    {
      if ((UnityEngine.Object) t.font == (UnityEngine.Object) null)
        return;
      HashSet<Text> textSet;
      FontUpdateTracker.m_Tracked.TryGetValue(t.font, out textSet);
      if (textSet == null)
        return;
      textSet.Remove(t);
      if (textSet.Count != 0)
        return;
      FontUpdateTracker.m_Tracked.Remove(t.font);
      if (FontUpdateTracker.m_Tracked.Count == 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (FontUpdateTracker.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FontUpdateTracker.\u003C\u003Ef__mg\u0024cache1 = new Action<Font>(FontUpdateTracker.RebuildForFont);
        }
        // ISSUE: reference to a compiler-generated field
        Font.textureRebuilt -= FontUpdateTracker.\u003C\u003Ef__mg\u0024cache1;
      }
    }
  }
}
