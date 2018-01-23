// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.SimulatedBody
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal.VR
{
  public class SimulatedBody
  {
    internal SimulatedBody()
    {
    }

    public Vector3 position
    {
      get
      {
        return HolographicEmulation.GetBodyPosition_Internal();
      }
      set
      {
        HolographicEmulation.SetBodyPosition_Internal(value);
      }
    }

    public float rotation
    {
      get
      {
        return HolographicEmulation.GetBodyRotation_Internal();
      }
      set
      {
        HolographicEmulation.SetBodyRotation_Internal(value);
      }
    }

    public float height
    {
      get
      {
        return HolographicEmulation.GetBodyHeight_Internal();
      }
      set
      {
        HolographicEmulation.SetBodyHeight_Internal(value);
      }
    }
  }
}
