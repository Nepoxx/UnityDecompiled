// Decompiled with JetBrains decompiler
// Type: UnityEditor.TickHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TickHandler
  {
    [SerializeField]
    private float[] m_TickModulos = new float[0];
    [SerializeField]
    private float[] m_TickStrengths = new float[0];
    [SerializeField]
    private int m_SmallestTick = 0;
    [SerializeField]
    private int m_BiggestTick = -1;
    [SerializeField]
    private float m_MinValue = 0.0f;
    [SerializeField]
    private float m_MaxValue = 1f;
    [SerializeField]
    private float m_PixelRange = 1f;

    public int tickLevels
    {
      get
      {
        return this.m_BiggestTick - this.m_SmallestTick + 1;
      }
    }

    public void SetTickModulos(float[] tickModulos)
    {
      this.m_TickModulos = tickModulos;
    }

    public List<float> GetTickModulosForFrameRate(float frameRate)
    {
      if ((double) frameRate > 1073741824.0 || (double) frameRate != (double) Mathf.Round(frameRate))
        return new List<float>() { 1f / frameRate, 5f / frameRate, 10f / frameRate, 50f / frameRate, 100f / frameRate, 500f / frameRate, 1000f / frameRate, 5000f / frameRate, 10000f / frameRate, 50000f / frameRate, 100000f / frameRate, 500000f / frameRate };
      List<int> intList = new List<int>();
      int num1 = 1;
      while ((double) num1 < (double) frameRate && (double) Math.Abs((float) num1 - frameRate) >= 1E-05)
      {
        int num2 = Mathf.RoundToInt(frameRate / (float) num1);
        if (num2 % 60 == 0)
        {
          num1 *= 2;
          intList.Add(num1);
        }
        else if (num2 % 30 == 0)
        {
          num1 *= 3;
          intList.Add(num1);
        }
        else if (num2 % 20 == 0)
        {
          num1 *= 2;
          intList.Add(num1);
        }
        else if (num2 % 10 == 0)
        {
          num1 *= 2;
          intList.Add(num1);
        }
        else if (num2 % 5 == 0)
        {
          num1 *= 5;
          intList.Add(num1);
        }
        else if (num2 % 2 == 0)
        {
          num1 *= 2;
          intList.Add(num1);
        }
        else if (num2 % 3 == 0)
        {
          num1 *= 3;
          intList.Add(num1);
        }
        else
          num1 = Mathf.RoundToInt(frameRate);
      }
      List<float> floatList = new List<float>(13 + intList.Count);
      for (int index = 0; index < intList.Count; ++index)
        floatList.Add(1f / (float) intList[intList.Count - index - 1]);
      floatList.Add(1f);
      floatList.Add(5f);
      floatList.Add(10f);
      floatList.Add(30f);
      floatList.Add(60f);
      floatList.Add(300f);
      floatList.Add(600f);
      floatList.Add(1800f);
      floatList.Add(3600f);
      floatList.Add(21600f);
      floatList.Add(86400f);
      floatList.Add(604800f);
      floatList.Add(1209600f);
      return floatList;
    }

    public void SetTickModulosForFrameRate(float frameRate)
    {
      this.SetTickModulos(this.GetTickModulosForFrameRate(frameRate).ToArray());
    }

    public void SetRanges(float minValue, float maxValue, float minPixel, float maxPixel)
    {
      this.m_MinValue = minValue;
      this.m_MaxValue = maxValue;
      this.m_PixelRange = maxPixel - minPixel;
    }

    public float[] GetTicksAtLevel(int level, bool excludeTicksFromHigherlevels)
    {
      if (level < 0)
        return new float[0];
      int index1 = Mathf.Clamp(this.m_SmallestTick + level, 0, this.m_TickModulos.Length - 1);
      List<float> floatList = new List<float>();
      int num1 = Mathf.FloorToInt(this.m_MinValue / this.m_TickModulos[index1]);
      int num2 = Mathf.CeilToInt(this.m_MaxValue / this.m_TickModulos[index1]);
      for (int index2 = num1; index2 <= num2; ++index2)
      {
        if (!excludeTicksFromHigherlevels || index1 >= this.m_BiggestTick || index2 % Mathf.RoundToInt(this.m_TickModulos[index1 + 1] / this.m_TickModulos[index1]) != 0)
          floatList.Add((float) index2 * this.m_TickModulos[index1]);
      }
      return floatList.ToArray();
    }

    public float GetStrengthOfLevel(int level)
    {
      return this.m_TickStrengths[this.m_SmallestTick + level];
    }

    public float GetPeriodOfLevel(int level)
    {
      return this.m_TickModulos[Mathf.Clamp(this.m_SmallestTick + level, 0, this.m_TickModulos.Length - 1)];
    }

    public int GetLevelWithMinSeparation(float pixelSeparation)
    {
      for (int index = 0; index < this.m_TickModulos.Length; ++index)
      {
        if ((double) this.m_TickModulos[index] * (double) this.m_PixelRange / ((double) this.m_MaxValue - (double) this.m_MinValue) >= (double) pixelSeparation)
          return index - this.m_SmallestTick;
      }
      return -1;
    }

    public void SetTickStrengths(float tickMinSpacing, float tickMaxSpacing, bool sqrt)
    {
      this.m_TickStrengths = new float[this.m_TickModulos.Length];
      this.m_SmallestTick = 0;
      this.m_BiggestTick = this.m_TickModulos.Length - 1;
      for (int index = this.m_TickModulos.Length - 1; index >= 0; --index)
      {
        float num = (float) ((double) this.m_TickModulos[index] * (double) this.m_PixelRange / ((double) this.m_MaxValue - (double) this.m_MinValue));
        this.m_TickStrengths[index] = (float) (((double) num - (double) tickMinSpacing) / ((double) tickMaxSpacing - (double) tickMinSpacing));
        if ((double) this.m_TickStrengths[index] >= 1.0)
          this.m_BiggestTick = index;
        if ((double) num <= (double) tickMinSpacing)
        {
          this.m_SmallestTick = index;
          break;
        }
      }
      for (int smallestTick = this.m_SmallestTick; smallestTick <= this.m_BiggestTick; ++smallestTick)
      {
        this.m_TickStrengths[smallestTick] = Mathf.Clamp01(this.m_TickStrengths[smallestTick]);
        if (sqrt)
          this.m_TickStrengths[smallestTick] = Mathf.Sqrt(this.m_TickStrengths[smallestTick]);
      }
    }
  }
}
