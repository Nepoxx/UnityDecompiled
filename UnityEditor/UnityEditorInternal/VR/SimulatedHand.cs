// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.SimulatedHand
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal.VR
{
  public class SimulatedHand
  {
    private GestureHand m_Hand;

    internal SimulatedHand(GestureHand hand)
    {
      this.m_Hand = hand;
    }

    public Vector3 position
    {
      get
      {
        return HolographicEmulation.GetHandPosition_Internal(this.m_Hand);
      }
      set
      {
        HolographicEmulation.SetHandPosition_Internal(this.m_Hand, value);
      }
    }

    public bool activated
    {
      get
      {
        return HolographicEmulation.GetHandActivated_Internal(this.m_Hand);
      }
      set
      {
        HolographicEmulation.SetHandActivated_Internal(this.m_Hand, value);
      }
    }

    public bool visible
    {
      get
      {
        return HolographicEmulation.GetHandVisible_Internal(this.m_Hand);
      }
    }

    public void EnsureVisible()
    {
      HolographicEmulation.EnsureHandVisible_Internal(this.m_Hand);
    }

    public void PerformGesture(SimulatedGesture gesture)
    {
      HolographicEmulation.PerformGesture_Internal(this.m_Hand, gesture);
    }
  }
}
