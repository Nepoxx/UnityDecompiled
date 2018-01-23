// Decompiled with JetBrains decompiler
// Type: UnityEngine.HumanBone
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The mapping between a bone in the model and the conceptual bone in the Mecanim human anatomy.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct HumanBone
  {
    private string m_BoneName;
    private string m_HumanName;
    /// <summary>
    ///   <para>The rotation limits that define the muscle for this bone.</para>
    /// </summary>
    public HumanLimit limit;

    /// <summary>
    ///   <para>The name of the bone to which the Mecanim human bone is mapped.</para>
    /// </summary>
    public string boneName
    {
      get
      {
        return this.m_BoneName;
      }
      set
      {
        this.m_BoneName = value;
      }
    }

    /// <summary>
    ///   <para>The name of the Mecanim human bone to which the bone from the model is mapped.</para>
    /// </summary>
    public string humanName
    {
      get
      {
        return this.m_HumanName;
      }
      set
      {
        this.m_HumanName = value;
      }
    }
  }
}
