// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIUtilitySystem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine
{
  internal class GUIUtilitySystem : IGUIUtility
  {
    public int GetPermanentControlID()
    {
      return GUIUtility.GetPermanentControlID();
    }

    public int hotControl
    {
      get
      {
        return GUIUtility.hotControl;
      }
      set
      {
        GUIUtility.hotControl = value;
      }
    }

    public int keyboardControl
    {
      get
      {
        return GUIUtility.keyboardControl;
      }
      set
      {
        GUIUtility.keyboardControl = value;
      }
    }

    public int GetControlID(int hint, FocusType focus)
    {
      return GUIUtility.GetControlID(hint, focus);
    }
  }
}
