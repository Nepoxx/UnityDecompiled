// Decompiled with JetBrains decompiler
// Type: UnityEngine.CustomGridBrushAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Class)]
  public class CustomGridBrushAttribute : Attribute
  {
    private bool m_HideAssetInstances;
    private bool m_HideDefaultInstance;
    private bool m_DefaultBrush;
    private string m_DefaultName;

    public CustomGridBrushAttribute()
    {
      this.m_HideAssetInstances = false;
      this.m_HideDefaultInstance = false;
      this.m_DefaultBrush = false;
      this.m_DefaultName = "";
    }

    public CustomGridBrushAttribute(bool hideAssetInstances, bool hideDefaultInstance, bool defaultBrush, string defaultName)
    {
      this.m_HideAssetInstances = hideAssetInstances;
      this.m_HideDefaultInstance = hideDefaultInstance;
      this.m_DefaultBrush = defaultBrush;
      this.m_DefaultName = defaultName;
    }

    public bool hideAssetInstances
    {
      get
      {
        return this.m_HideAssetInstances;
      }
    }

    public bool hideDefaultInstance
    {
      get
      {
        return this.m_HideDefaultInstance;
      }
    }

    public bool defaultBrush
    {
      get
      {
        return this.m_DefaultBrush;
      }
    }

    public string defaultName
    {
      get
      {
        return this.m_DefaultName;
      }
    }
  }
}
