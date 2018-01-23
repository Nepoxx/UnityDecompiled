// Decompiled with JetBrains decompiler
// Type: UnityEngine.DrivenRectTransformTracker
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A component can be designed to drive a RectTransform. The DrivenRectTransformTracker struct is used to specify which RectTransforms it is driving.</para>
  /// </summary>
  public struct DrivenRectTransformTracker
  {
    private List<RectTransform> m_Tracked;

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanRecordModifications();

    /// <summary>
    ///   <para>Add a RectTransform to be driven.</para>
    /// </summary>
    /// <param name="driver">The object to drive properties.</param>
    /// <param name="rectTransform">The RectTransform to be driven.</param>
    /// <param name="drivenProperties">The properties to be driven.</param>
    public void Add(Object driver, RectTransform rectTransform, DrivenTransformProperties drivenProperties)
    {
      if (this.m_Tracked == null)
        this.m_Tracked = new List<RectTransform>();
      rectTransform.drivenByObject = driver;
      rectTransform.drivenProperties |= drivenProperties;
      if (!Application.isPlaying && DrivenRectTransformTracker.CanRecordModifications())
        RuntimeUndo.RecordObject((Object) rectTransform, "Driving RectTransform");
      this.m_Tracked.Add(rectTransform);
    }

    [Obsolete("revertValues parameter is ignored. Please use Clear() instead.")]
    public void Clear(bool revertValues)
    {
      this.Clear();
    }

    /// <summary>
    ///   <para>Clear the list of RectTransforms being driven.</para>
    /// </summary>
    public void Clear()
    {
      if (this.m_Tracked == null)
        return;
      for (int index = 0; index < this.m_Tracked.Count; ++index)
      {
        if ((Object) this.m_Tracked[index] != (Object) null)
        {
          if (!Application.isPlaying && DrivenRectTransformTracker.CanRecordModifications())
            RuntimeUndo.RecordObject((Object) this.m_Tracked[index], "Driving RectTransform");
          this.m_Tracked[index].drivenByObject = (Object) null;
        }
      }
      this.m_Tracked.Clear();
    }
  }
}
