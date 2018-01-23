// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefColor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Globalization;
using UnityEngine;

namespace UnityEditor
{
  internal class PrefColor : IPrefType
  {
    private string m_Name;
    private Color m_Color;
    private Color m_DefaultColor;
    private bool m_SeparateColors;
    private Color m_OptionalDarkColor;
    private Color m_OptionalDarkDefaultColor;
    private bool m_Loaded;

    public PrefColor()
    {
      this.m_Loaded = true;
    }

    public PrefColor(string name, float defaultRed, float defaultGreen, float defaultBlue, float defaultAlpha)
    {
      this.m_Name = name;
      this.m_Color = this.m_DefaultColor = new Color(defaultRed, defaultGreen, defaultBlue, defaultAlpha);
      this.m_SeparateColors = false;
      this.m_OptionalDarkColor = this.m_OptionalDarkDefaultColor = Color.clear;
      Settings.Add((IPrefType) this);
      this.m_Loaded = false;
    }

    public PrefColor(string name, float defaultRed, float defaultGreen, float defaultBlue, float defaultAlpha, float defaultRed2, float defaultGreen2, float defaultBlue2, float defaultAlpha2)
    {
      this.m_Name = name;
      this.m_Color = this.m_DefaultColor = new Color(defaultRed, defaultGreen, defaultBlue, defaultAlpha);
      this.m_SeparateColors = true;
      this.m_OptionalDarkColor = this.m_OptionalDarkDefaultColor = new Color(defaultRed2, defaultGreen2, defaultBlue2, defaultAlpha2);
      Settings.Add((IPrefType) this);
      this.m_Loaded = false;
    }

    public void Load()
    {
      if (this.m_Loaded)
        return;
      this.m_Loaded = true;
      PrefColor prefColor = Settings.Get<PrefColor>(this.m_Name, this);
      this.m_Name = prefColor.m_Name;
      this.m_Color = prefColor.m_Color;
      this.m_SeparateColors = prefColor.m_SeparateColors;
      this.m_OptionalDarkColor = prefColor.m_OptionalDarkColor;
    }

    public Color Color
    {
      get
      {
        this.Load();
        if (this.m_SeparateColors && EditorGUIUtility.isProSkin)
          return this.m_OptionalDarkColor;
        return this.m_Color;
      }
      set
      {
        this.Load();
        if (this.m_SeparateColors && EditorGUIUtility.isProSkin)
          this.m_OptionalDarkColor = value;
        else
          this.m_Color = value;
      }
    }

    public string Name
    {
      get
      {
        this.Load();
        return this.m_Name;
      }
    }

    public static implicit operator Color(PrefColor pcolor)
    {
      return pcolor.Color;
    }

    public string ToUniqueString()
    {
      this.Load();
      if (this.m_SeparateColors)
        return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0};{1};{2};{3};{4};{5};{6};{7};{8}", (object) this.m_Name, (object) this.m_Color.r, (object) this.m_Color.g, (object) this.m_Color.b, (object) this.m_Color.a, (object) this.m_OptionalDarkColor.r, (object) this.m_OptionalDarkColor.g, (object) this.m_OptionalDarkColor.b, (object) this.m_OptionalDarkColor.a);
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0};{1};{2};{3};{4}", (object) this.m_Name, (object) this.m_Color.r, (object) this.m_Color.g, (object) this.m_Color.b, (object) this.m_Color.a);
    }

    public void FromUniqueString(string s)
    {
      this.Load();
      string[] strArray = s.Split(';');
      if (strArray.Length != 5 && strArray.Length != 9)
      {
        Debug.LogError((object) "Parsing PrefColor failed");
      }
      else
      {
        this.m_Name = strArray[0];
        strArray[1] = strArray[1].Replace(',', '.');
        strArray[2] = strArray[2].Replace(',', '.');
        strArray[3] = strArray[3].Replace(',', '.');
        strArray[4] = strArray[4].Replace(',', '.');
        float result1;
        float result2;
        float result3;
        float result4;
        if (float.TryParse(strArray[1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1) & float.TryParse(strArray[2], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result2) & float.TryParse(strArray[3], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result3) & float.TryParse(strArray[4], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result4))
          this.m_Color = new Color(result1, result2, result3, result4);
        else
          Debug.LogError((object) "Parsing PrefColor failed");
        if (strArray.Length == 9)
        {
          this.m_SeparateColors = true;
          strArray[5] = strArray[5].Replace(',', '.');
          strArray[6] = strArray[6].Replace(',', '.');
          strArray[7] = strArray[7].Replace(',', '.');
          strArray[8] = strArray[8].Replace(',', '.');
          if (float.TryParse(strArray[5], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1) & float.TryParse(strArray[6], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result2) & float.TryParse(strArray[7], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result3) & float.TryParse(strArray[8], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result4))
            this.m_OptionalDarkColor = new Color(result1, result2, result3, result4);
          else
            Debug.LogError((object) "Parsing PrefColor failed");
        }
        else
        {
          this.m_SeparateColors = false;
          this.m_OptionalDarkColor = Color.clear;
        }
      }
    }

    internal void ResetToDefault()
    {
      this.Load();
      this.m_Color = this.m_DefaultColor;
      this.m_OptionalDarkColor = this.m_OptionalDarkDefaultColor;
    }
  }
}
