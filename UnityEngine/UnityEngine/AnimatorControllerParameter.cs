// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorControllerParameter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Used to communicate between scripting and the controller. Some parameters can be set in scripting and used by the controller, while other parameters are based on Custom Curves in Animation Clips and can be sampled using the scripting API.</para>
  /// </summary>
  [UsedByNativeCode]
  public sealed class AnimatorControllerParameter
  {
    internal string m_Name = "";
    internal AnimatorControllerParameterType m_Type;
    internal float m_DefaultFloat;
    internal int m_DefaultInt;
    internal bool m_DefaultBool;

    /// <summary>
    ///   <para>The name of the parameter.</para>
    /// </summary>
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

    /// <summary>
    ///   <para>Returns the hash of the parameter based on its name.</para>
    /// </summary>
    public int nameHash
    {
      get
      {
        return Animator.StringToHash(this.m_Name);
      }
    }

    /// <summary>
    ///   <para>The type of the parameter.</para>
    /// </summary>
    public AnimatorControllerParameterType type
    {
      get
      {
        return this.m_Type;
      }
      set
      {
        this.m_Type = value;
      }
    }

    /// <summary>
    ///   <para>The default float value for the parameter.</para>
    /// </summary>
    public float defaultFloat
    {
      get
      {
        return this.m_DefaultFloat;
      }
      set
      {
        this.m_DefaultFloat = value;
      }
    }

    /// <summary>
    ///   <para>The default int value for the parameter.</para>
    /// </summary>
    public int defaultInt
    {
      get
      {
        return this.m_DefaultInt;
      }
      set
      {
        this.m_DefaultInt = value;
      }
    }

    /// <summary>
    ///   <para>The default bool value for the parameter.</para>
    /// </summary>
    public bool defaultBool
    {
      get
      {
        return this.m_DefaultBool;
      }
      set
      {
        this.m_DefaultBool = value;
      }
    }

    public override bool Equals(object o)
    {
      AnimatorControllerParameter controllerParameter = o as AnimatorControllerParameter;
      return controllerParameter != null && this.m_Name == controllerParameter.m_Name && (this.m_Type == controllerParameter.m_Type && (double) this.m_DefaultFloat == (double) controllerParameter.m_DefaultFloat) && this.m_DefaultInt == controllerParameter.m_DefaultInt && this.m_DefaultBool == controllerParameter.m_DefaultBool;
    }

    public override int GetHashCode()
    {
      return this.name.GetHashCode();
    }
  }
}
