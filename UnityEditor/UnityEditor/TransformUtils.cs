// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor Transform Utility Class.</para>
  /// </summary>
  public static class TransformUtils
  {
    /// <summary>
    ///   <para>Returns the rotation of a transform as it is shown in the Transform Inspector window.</para>
    /// </summary>
    /// <param name="t">Transform to get the rotation from.</param>
    /// <returns>
    ///   <para>Rotation as it is shown in the Transform Inspector window.</para>
    /// </returns>
    public static Vector3 GetInspectorRotation(Transform t)
    {
      return t.GetLocalEulerAngles(t.rotationOrder);
    }

    /// <summary>
    ///   <para>Sets the rotation of a transform as it would be set by the Transform Inspector window.</para>
    /// </summary>
    /// <param name="t">Transform to set the rotation on.</param>
    /// <param name="r">Rotation as it would be set by the Transform Inspector window.</param>
    public static void SetInspectorRotation(Transform t, Vector3 r)
    {
      t.SetLocalEulerAngles(r, t.rotationOrder);
    }
  }
}
