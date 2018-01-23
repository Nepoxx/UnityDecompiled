// Decompiled with JetBrains decompiler
// Type: UnityEngine.Pose
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Serializable]
  public struct Pose
  {
    private static readonly Pose k_Identity = new Pose(Vector3.zero, Quaternion.identity);
    public Vector3 position;
    public Quaternion rotation;

    public Pose(Vector3 position, Quaternion rotation)
    {
      this.position = position;
      this.rotation = rotation;
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", (object) this.position.ToString(), (object) this.rotation.ToString());
    }

    public string ToString(string format)
    {
      return string.Format("({0}, {1})", (object) this.position.ToString(format), (object) this.rotation.ToString(format));
    }

    public Pose GetTransformedBy(Pose lhs)
    {
      return new Pose() { position = lhs.position + lhs.rotation * this.position, rotation = lhs.rotation * this.rotation };
    }

    public Pose GetTransformedBy(Transform lhs)
    {
      return new Pose() { position = lhs.TransformPoint(this.position), rotation = lhs.rotation * this.rotation };
    }

    public static Pose identity
    {
      get
      {
        return Pose.k_Identity;
      }
    }
  }
}
