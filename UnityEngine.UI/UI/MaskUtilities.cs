// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.MaskUtilities
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Mask related utility class.</para>
  /// </summary>
  public class MaskUtilities
  {
    /// <summary>
    ///   <para>Notify all IClippables under the given component that they need to recalculate clipping.</para>
    /// </summary>
    /// <param name="mask"></param>
    public static void Notify2DMaskStateChanged(Component mask)
    {
      List<Component> componentList = ListPool<Component>.Get();
      mask.GetComponentsInChildren<Component>(componentList);
      for (int index = 0; index < componentList.Count; ++index)
      {
        if (!((Object) componentList[index] == (Object) null) && !((Object) componentList[index].gameObject == (Object) mask.gameObject))
        {
          IClippable clippable = componentList[index] as IClippable;
          if (clippable != null)
            clippable.RecalculateClipping();
        }
      }
      ListPool<Component>.Release(componentList);
    }

    /// <summary>
    ///   <para>Notify all IMaskable under the given component that they need to recalculate masking.</para>
    /// </summary>
    /// <param name="mask"></param>
    public static void NotifyStencilStateChanged(Component mask)
    {
      List<Component> componentList = ListPool<Component>.Get();
      mask.GetComponentsInChildren<Component>(componentList);
      for (int index = 0; index < componentList.Count; ++index)
      {
        if (!((Object) componentList[index] == (Object) null) && !((Object) componentList[index].gameObject == (Object) mask.gameObject))
        {
          IMaskable maskable = componentList[index] as IMaskable;
          if (maskable != null)
            maskable.RecalculateMasking();
        }
      }
      ListPool<Component>.Release(componentList);
    }

    /// <summary>
    ///   <para>Find a root Canvas.</para>
    /// </summary>
    /// <param name="start">Search start.</param>
    /// <returns>
    ///   <para>Canvas transform.</para>
    /// </returns>
    public static Transform FindRootSortOverrideCanvas(Transform start)
    {
      List<Canvas> canvasList = ListPool<Canvas>.Get();
      start.GetComponentsInParent<Canvas>(false, canvasList);
      Canvas canvas = (Canvas) null;
      for (int index = 0; index < canvasList.Count; ++index)
      {
        canvas = canvasList[index];
        if (canvas.overrideSorting)
          break;
      }
      ListPool<Canvas>.Release(canvasList);
      return !((Object) canvas != (Object) null) ? (Transform) null : canvas.transform;
    }

    /// <summary>
    ///   <para>Find the stencil depth for a given element.</para>
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="stopAfter"></param>
    public static int GetStencilDepth(Transform transform, Transform stopAfter)
    {
      int num = 0;
      if ((Object) transform == (Object) stopAfter)
        return num;
      Transform parent = transform.parent;
      List<Mask> maskList = ListPool<Mask>.Get();
      for (; (Object) parent != (Object) null; parent = parent.parent)
      {
        parent.GetComponents<Mask>(maskList);
        for (int index = 0; index < maskList.Count; ++index)
        {
          if ((Object) maskList[index] != (Object) null && maskList[index].MaskEnabled() && maskList[index].graphic.IsActive())
          {
            ++num;
            break;
          }
        }
        if ((Object) parent == (Object) stopAfter)
          break;
      }
      ListPool<Mask>.Release(maskList);
      return num;
    }

    /// <summary>
    ///   <para>Helper function to determine if the child is a descendant of father or is father.</para>
    /// </summary>
    /// <param name="father">The transform to compare against.</param>
    /// <param name="child">The starting transform to search up the hierarchy.</param>
    /// <returns>
    ///   <para>Is child equal to father or is a descendant.</para>
    /// </returns>
    public static bool IsDescendantOrSelf(Transform father, Transform child)
    {
      if ((Object) father == (Object) null || (Object) child == (Object) null)
        return false;
      if ((Object) father == (Object) child)
        return true;
      for (; (Object) child.parent != (Object) null; child = child.parent)
      {
        if ((Object) child.parent == (Object) father)
          return true;
      }
      return false;
    }

    /// <summary>
    ///   <para>Find the correct RectMask2D for a given IClippable.</para>
    /// </summary>
    /// <param name="transform">Clippable to search from.</param>
    /// <param name="clippable"></param>
    public static RectMask2D GetRectMaskForClippable(IClippable clippable)
    {
      List<RectMask2D> rectMask2DList = ListPool<RectMask2D>.Get();
      List<Canvas> canvasList = ListPool<Canvas>.Get();
      RectMask2D rectMask2D1 = (RectMask2D) null;
      clippable.rectTransform.GetComponentsInParent<RectMask2D>(false, rectMask2DList);
      if (rectMask2DList.Count > 0)
      {
        for (int index1 = 0; index1 < rectMask2DList.Count; ++index1)
        {
          RectMask2D rectMask2D2 = rectMask2DList[index1];
          if ((Object) rectMask2D2.gameObject == (Object) clippable.gameObject)
            rectMask2D1 = (RectMask2D) null;
          else if (!rectMask2D2.isActiveAndEnabled)
          {
            rectMask2D1 = (RectMask2D) null;
          }
          else
          {
            clippable.rectTransform.GetComponentsInParent<Canvas>(false, canvasList);
            for (int index2 = canvasList.Count - 1; index2 >= 0; --index2)
            {
              if (!MaskUtilities.IsDescendantOrSelf(canvasList[index2].transform, rectMask2D2.transform) && canvasList[index2].overrideSorting)
              {
                rectMask2D2 = (RectMask2D) null;
                break;
              }
            }
            return rectMask2D2;
          }
        }
      }
      ListPool<RectMask2D>.Release(rectMask2DList);
      ListPool<Canvas>.Release(canvasList);
      return rectMask2D1;
    }

    public static void GetRectMasksForClip(RectMask2D clipper, List<RectMask2D> masks)
    {
      masks.Clear();
      List<Canvas> canvasList = ListPool<Canvas>.Get();
      List<RectMask2D> rectMask2DList = ListPool<RectMask2D>.Get();
      clipper.transform.GetComponentsInParent<RectMask2D>(false, rectMask2DList);
      if (rectMask2DList.Count > 0)
      {
        clipper.transform.GetComponentsInParent<Canvas>(false, canvasList);
        for (int index1 = rectMask2DList.Count - 1; index1 >= 0; --index1)
        {
          if (rectMask2DList[index1].IsActive())
          {
            bool flag = true;
            for (int index2 = canvasList.Count - 1; index2 >= 0; --index2)
            {
              if (!MaskUtilities.IsDescendantOrSelf(canvasList[index2].transform, rectMask2DList[index1].transform) && canvasList[index2].overrideSorting)
              {
                flag = false;
                break;
              }
            }
            if (flag)
              masks.Add(rectMask2DList[index1]);
          }
        }
      }
      ListPool<RectMask2D>.Release(rectMask2DList);
      ListPool<Canvas>.Release(canvasList);
    }
  }
}
