// Decompiled with JetBrains decompiler
// Type: UnityEngine.AddComponentMenu
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  public sealed class AddComponentMenu : Attribute
  {
    private string m_AddComponentMenu;
    private int m_Ordering;

    public AddComponentMenu(string menuName)
    {
      this.m_AddComponentMenu = menuName;
      this.m_Ordering = 0;
    }

    public AddComponentMenu(string menuName, int order)
    {
      this.m_AddComponentMenu = menuName;
      this.m_Ordering = order;
    }

    public string componentMenu
    {
      get
      {
        return this.m_AddComponentMenu;
      }
    }

    public int componentOrder
    {
      get
      {
        return this.m_Ordering;
      }
    }
  }
}
