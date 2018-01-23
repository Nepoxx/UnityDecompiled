// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualElementAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  [Serializable]
  internal class VisualElementAsset : IUxmlAttributes
  {
    [SerializeField]
    private string m_Name;
    [SerializeField]
    private int m_Id;
    [SerializeField]
    private int m_ParentId;
    [SerializeField]
    private int m_RuleIndex;
    [SerializeField]
    private string m_Text;
    [SerializeField]
    private PickingMode m_PickingMode;
    [SerializeField]
    private string m_FullTypeName;
    [SerializeField]
    private string[] m_Classes;
    [SerializeField]
    private List<string> m_Stylesheets;
    [SerializeField]
    private List<string> m_Properties;

    public VisualElementAsset(string fullTypeName)
    {
      this.m_FullTypeName = fullTypeName;
    }

    public string name
    {
      get
      {
        return this.m_Name;
      }
      set
      {
        this.m_Name = value;
      }
    }

    public int id
    {
      get
      {
        return this.m_Id;
      }
      set
      {
        this.m_Id = value;
      }
    }

    public int parentId
    {
      get
      {
        return this.m_ParentId;
      }
      set
      {
        this.m_ParentId = value;
      }
    }

    public int ruleIndex
    {
      get
      {
        return this.m_RuleIndex;
      }
      set
      {
        this.m_RuleIndex = value;
      }
    }

    public string text
    {
      get
      {
        return this.m_Text;
      }
      set
      {
        this.m_Text = value;
      }
    }

    public PickingMode pickingMode
    {
      get
      {
        return this.m_PickingMode;
      }
      set
      {
        this.m_PickingMode = value;
      }
    }

    public string fullTypeName
    {
      get
      {
        return this.m_FullTypeName;
      }
      set
      {
        this.m_FullTypeName = value;
      }
    }

    public string[] classes
    {
      get
      {
        return this.m_Classes;
      }
      set
      {
        this.m_Classes = value;
      }
    }

    public List<string> stylesheets
    {
      get
      {
        return this.m_Stylesheets != null ? this.m_Stylesheets : (this.m_Stylesheets = new List<string>());
      }
      set
      {
        this.m_Stylesheets = value;
      }
    }

    public VisualElement Create(CreationContext ctx)
    {
      Func<IUxmlAttributes, CreationContext, VisualElement> factory;
      if (!Factories.TryGetValue(this.fullTypeName, out factory))
      {
        Debug.LogErrorFormat("Visual Element Type '{0}' has no factory method.", (object) this.fullTypeName);
        return (VisualElement) new Label(string.Format("Unknown type: '{0}'", (object) this.fullTypeName));
      }
      if (factory == null)
      {
        Debug.LogErrorFormat("Visual Element Type '{0}' has a null factory method.", (object) this.fullTypeName);
        return (VisualElement) new Label(string.Format("Type with no factory method: '{0}'", (object) this.fullTypeName));
      }
      VisualElement visualElement = factory((IUxmlAttributes) this, ctx);
      if (visualElement == null)
      {
        Debug.LogErrorFormat("The factory of Visual Element Type '{0}' has returned a null object", (object) this.fullTypeName);
        return (VisualElement) new Label(string.Format("The factory of Visual Element Type '{0}' has returned a null object", (object) this.fullTypeName));
      }
      visualElement.name = this.name;
      if (this.classes != null)
      {
        for (int index = 0; index < this.classes.Length; ++index)
          visualElement.AddToClassList(this.classes[index]);
      }
      if (this.stylesheets != null)
      {
        for (int index = 0; index < this.stylesheets.Count; ++index)
          visualElement.AddStyleSheetPath(this.stylesheets[index]);
      }
      if (!string.IsNullOrEmpty(this.text))
        visualElement.text = this.text;
      visualElement.pickingMode = this.pickingMode;
      return visualElement;
    }

    public void AddProperty(string propertyName, string propertyValue)
    {
      if (this.m_Properties == null)
        this.m_Properties = new List<string>();
      this.m_Properties.Add(propertyName);
      this.m_Properties.Add(propertyValue);
    }

    public string GetPropertyString(string propertyName)
    {
      if (this.m_Properties == null)
        return (string) null;
      int index = 0;
      while (index < this.m_Properties.Count - 1)
      {
        if (this.m_Properties[index] == propertyName)
          return this.m_Properties[index + 1];
        index += 2;
      }
      return (string) null;
    }

    public int GetPropertyInt(string propertyName, int defaultValue)
    {
      string propertyString = this.GetPropertyString(propertyName);
      int result;
      if (propertyString == null || !int.TryParse(propertyString, out result))
        return defaultValue;
      return result;
    }

    public bool GetPropertyBool(string propertyName, bool defaultValue)
    {
      string propertyString = this.GetPropertyString(propertyName);
      bool result;
      if (propertyString == null || !bool.TryParse(propertyString, out result))
        return defaultValue;
      return result;
    }

    public Color GetPropertyColor(string propertyName, Color defaultValue)
    {
      string propertyString = this.GetPropertyString(propertyName);
      Color32 color;
      if (propertyString == null || !ColorUtility.DoTryParseHtmlColor(propertyString, out color))
        return defaultValue;
      return (Color) color;
    }

    public long GetPropertyLong(string propertyName, long defaultValue)
    {
      string propertyString = this.GetPropertyString(propertyName);
      long result;
      if (propertyString == null || !long.TryParse(propertyString, out result))
        return defaultValue;
      return result;
    }

    public float GetPropertyFloat(string propertyName, float def)
    {
      string propertyString = this.GetPropertyString(propertyName);
      float result;
      if (propertyString == null || !float.TryParse(propertyString, out result))
        return def;
      return result;
    }

    public T GetPropertyEnum<T>(string propertyName, T def)
    {
      string propertyString = this.GetPropertyString(propertyName);
      if (propertyString == null || !Enum.IsDefined(typeof (T), (object) propertyString))
        return def;
      return (T) Enum.Parse(typeof (T), propertyString);
    }
  }
}
