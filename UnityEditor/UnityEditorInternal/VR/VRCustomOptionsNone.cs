// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRCustomOptionsNone
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal.VR
{
  internal class VRCustomOptionsNone : VRCustomOptions
  {
    public override Rect Draw(Rect rect)
    {
      return rect;
    }

    public override float GetHeight()
    {
      return 0.0f;
    }
  }
}
