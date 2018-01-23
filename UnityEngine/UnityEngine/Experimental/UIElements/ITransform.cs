// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ITransform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public interface ITransform
  {
    /// <summary>
    ///   <para>The position of the VisualElement's transform.</para>
    /// </summary>
    Vector3 position { get; set; }

    /// <summary>
    ///   <para>The rotation of the VisualElement's transform stored as a Quaternion.</para>
    /// </summary>
    Quaternion rotation { get; set; }

    /// <summary>
    ///   <para>The scale of the VisualElement's transform.</para>
    /// </summary>
    Vector3 scale { get; set; }

    /// <summary>
    ///   <para>Transformation matrix calculated from the position, rotation and scale of the transform (Read Only).</para>
    /// </summary>
    Matrix4x4 matrix { get; }
  }
}
