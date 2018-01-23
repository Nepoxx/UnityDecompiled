// Decompiled with JetBrains decompiler
// Type: UnityEngine.IGUIUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEngine
{
  internal interface IGUIUtility
  {
    int GetPermanentControlID();

    int hotControl { get; set; }

    int keyboardControl { get; set; }

    int GetControlID(int hint, FocusType focus);
  }
}
