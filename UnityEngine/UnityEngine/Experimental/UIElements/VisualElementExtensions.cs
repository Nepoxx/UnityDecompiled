// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.VisualElementExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>VisualElementExtensions is a set of extension methods useful for VisualElement.</para>
  /// </summary>
  public static class VisualElementExtensions
  {
    public static Vector2 WorldToLocal(this VisualElement ele, Vector2 p)
    {
      Vector3 vector3 = ele.worldTransform.inverse.MultiplyPoint3x4(new Vector3(p.x, p.y, 0.0f));
      return new Vector2(vector3.x - ele.layout.position.x, vector3.y - ele.layout.position.y);
    }

    public static Vector2 LocalToWorld(this VisualElement ele, Vector2 p)
    {
      Vector3 vector3 = ele.worldTransform.MultiplyPoint3x4((Vector3) (p + ele.layout.position));
      return new Vector2(vector3.x, vector3.y);
    }

    public static Rect WorldToLocal(this VisualElement ele, Rect r)
    {
      Matrix4x4 inverse = ele.worldTransform.inverse;
      Vector2 vector2 = (Vector2) inverse.MultiplyPoint3x4((Vector3) r.position);
      r.position = vector2 - ele.layout.position;
      r.size = (Vector2) inverse.MultiplyVector((Vector3) r.size);
      return r;
    }

    public static Rect LocalToWorld(this VisualElement ele, Rect r)
    {
      Matrix4x4 worldTransform = ele.worldTransform;
      r.position = (Vector2) worldTransform.MultiplyPoint3x4((Vector3) (ele.layout.position + r.position));
      r.size = (Vector2) worldTransform.MultiplyVector((Vector3) r.size);
      return r;
    }

    public static Vector2 ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Vector2 point)
    {
      return dest.WorldToLocal(src.LocalToWorld(point));
    }

    public static Rect ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Rect rect)
    {
      return dest.WorldToLocal(src.LocalToWorld(rect));
    }

    public static void StretchToParentSize(this VisualElement elem)
    {
      IStyle style = elem.style;
      style.positionType = (StyleValue<PositionType>) PositionType.Absolute;
      style.positionLeft = (StyleValue<float>) 0.0f;
      style.positionTop = (StyleValue<float>) 0.0f;
      style.positionRight = (StyleValue<float>) 0.0f;
      style.positionBottom = (StyleValue<float>) 0.0f;
    }

    /// <summary>
    ///   <para>Add a manipulator associated to a VisualElement.</para>
    /// </summary>
    /// <param name="ele">VisualElement associated to the manipulator.</param>
    /// <param name="manipulator">Manipulator to be added to the VisualElement.</param>
    public static void AddManipulator(this VisualElement ele, IManipulator manipulator)
    {
      manipulator.target = ele;
    }

    /// <summary>
    ///   <para>Remove a manipulator associated to a VisualElement.</para>
    /// </summary>
    /// <param name="ele">VisualElement associated to the manipulator.</param>
    /// <param name="manipulator">Manipulator to be removed from the VisualElement.</param>
    public static void RemoveManipulator(this VisualElement ele, IManipulator manipulator)
    {
      manipulator.target = (VisualElement) null;
    }
  }
}
