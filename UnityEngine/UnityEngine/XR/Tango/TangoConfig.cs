// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.Tango.TangoConfig
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.XR.Tango
{
  internal class TangoConfig
  {
    internal Dictionary<string, bool> m_boolParams = new Dictionary<string, bool>();
    internal Dictionary<string, double> m_doubleParams = new Dictionary<string, double>();
    internal Dictionary<string, int> m_intParams = new Dictionary<string, int>();
    internal Dictionary<string, long> m_longParams = new Dictionary<string, long>();
    internal Dictionary<string, string> m_stringParams = new Dictionary<string, string>();

    internal bool enableMotionTracking
    {
      set
      {
        this.AddConfigParameter("config_enable_motion_tracking", value);
        this.AddConfigParameter("config_enable_auto_recovery", value);
      }
    }

    internal bool enableDepth
    {
      set
      {
        this.AddConfigParameter("config_enable_depth", value);
        if (value)
          this.AddConfigParameter("config_depth_mode", 0);
        else
          this.RemoveConfigParameter("config_depth_mode");
      }
    }

    internal bool enableColorCamera
    {
      set
      {
        this.AddConfigParameter("config_enable_color_camera", value);
      }
    }

    internal AreaLearningMode areaLearningMode
    {
      set
      {
        switch (value)
        {
          case AreaLearningMode.LocalAreaDescriptionWithoutLearning:
            this.AddConfigParameter("config_enable_drift_correction", false);
            this.AddConfigParameter("config_load_area_description_UUID", TangoDevice.areaDescriptionUUID);
            this.AddConfigParameter("config_enable_learning_mode", false);
            this.AddConfigParameter("config_experimental", false);
            break;
          case AreaLearningMode.LocalAreaDescription:
            this.AddConfigParameter("config_enable_drift_correction", false);
            this.AddConfigParameter("config_load_area_description_UUID", TangoDevice.areaDescriptionUUID);
            this.AddConfigParameter("config_enable_learning_mode", true);
            this.AddConfigParameter("config_experimental", false);
            break;
          case AreaLearningMode.CloudAreaDescription:
            this.AddConfigParameter("config_enable_drift_correction", false);
            this.RemoveConfigParameter("config_load_area_description_UUID");
            this.AddConfigParameter("config_enable_learning_mode", false);
            this.AddConfigParameter("config_experimental", true);
            break;
        }
      }
    }

    internal void AddConfigParameter(string name, bool value)
    {
      this.m_boolParams[name] = value;
    }

    internal void AddConfigParameter(string name, double value)
    {
      this.m_doubleParams[name] = value;
    }

    internal void AddConfigParameter(string name, int value)
    {
      this.m_intParams[name] = value;
    }

    internal void AddConfigParameter(string name, long value)
    {
      this.m_longParams[name] = value;
    }

    internal void AddConfigParameter(string name, string value)
    {
      this.m_stringParams[name] = value;
    }

    internal void RemoveConfigParameter(string name)
    {
      this.m_stringParams.Remove(name);
      this.m_longParams.Remove(name);
      this.m_intParams.Remove(name);
      this.m_doubleParams.Remove(name);
      this.m_boolParams.Remove(name);
    }
  }
}
