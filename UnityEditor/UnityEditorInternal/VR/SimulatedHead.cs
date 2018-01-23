// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.SimulatedHead
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal.VR
{
  public class SimulatedHead
  {
    internal SimulatedHead()
    {
    }

    public float diameter
    {
      get
      {
        return HolographicEmulation.GetHeadDiameter_Internal();
      }
      set
      {
        HolographicEmulation.SetHeadDiameter_Internal(value);
      }
    }

    public Vector3 eulerAngles
    {
      get
      {
        return HolographicEmulation.GetHeadRotation_Internal();
      }
      set
      {
        HolographicEmulation.SetHeadRotation_Internal(value);
      }
    }
  }
}
