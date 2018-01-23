// Decompiled with JetBrains decompiler
// Type: UnityEngine.BeforeRenderHelper
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine
{
  internal static class BeforeRenderHelper
  {
    private static List<BeforeRenderHelper.OrderBlock> s_OrderBlocks = new List<BeforeRenderHelper.OrderBlock>();

    private static int GetUpdateOrder(UnityAction callback)
    {
      object[] customAttributes = callback.Method.GetCustomAttributes(typeof (BeforeRenderOrderAttribute), true);
      BeforeRenderOrderAttribute renderOrderAttribute = customAttributes == null || customAttributes.Length <= 0 ? (BeforeRenderOrderAttribute) null : customAttributes[0] as BeforeRenderOrderAttribute;
      return renderOrderAttribute == null ? 0 : renderOrderAttribute.order;
    }

    public static void RegisterCallback(UnityAction callback)
    {
      int updateOrder = BeforeRenderHelper.GetUpdateOrder(callback);
      lock ((object) BeforeRenderHelper.s_OrderBlocks)
      {
        int index;
        for (index = 0; index < BeforeRenderHelper.s_OrderBlocks.Count && BeforeRenderHelper.s_OrderBlocks[index].order <= updateOrder; ++index)
        {
          if (BeforeRenderHelper.s_OrderBlocks[index].order == updateOrder)
          {
            BeforeRenderHelper.OrderBlock orderBlock = BeforeRenderHelper.s_OrderBlocks[index];
            orderBlock.callback += callback;
            BeforeRenderHelper.s_OrderBlocks[index] = orderBlock;
            return;
          }
        }
        BeforeRenderHelper.OrderBlock orderBlock1 = new BeforeRenderHelper.OrderBlock();
        orderBlock1.order = updateOrder;
        orderBlock1.callback += callback;
        BeforeRenderHelper.s_OrderBlocks.Insert(index, orderBlock1);
      }
    }

    public static void UnregisterCallback(UnityAction callback)
    {
      int updateOrder = BeforeRenderHelper.GetUpdateOrder(callback);
      lock ((object) BeforeRenderHelper.s_OrderBlocks)
      {
        for (int index = 0; index < BeforeRenderHelper.s_OrderBlocks.Count && BeforeRenderHelper.s_OrderBlocks[index].order <= updateOrder; ++index)
        {
          if (BeforeRenderHelper.s_OrderBlocks[index].order == updateOrder)
          {
            BeforeRenderHelper.OrderBlock orderBlock = BeforeRenderHelper.s_OrderBlocks[index];
            orderBlock.callback -= callback;
            BeforeRenderHelper.s_OrderBlocks[index] = orderBlock;
            if (orderBlock.callback != null)
              break;
            BeforeRenderHelper.s_OrderBlocks.RemoveAt(index);
            break;
          }
        }
      }
    }

    public static void Invoke()
    {
      lock ((object) BeforeRenderHelper.s_OrderBlocks)
      {
        for (int index = 0; index < BeforeRenderHelper.s_OrderBlocks.Count; ++index)
        {
          UnityAction callback = BeforeRenderHelper.s_OrderBlocks[index].callback;
          if (callback != null)
            callback();
        }
      }
    }

    private struct OrderBlock
    {
      internal int order;
      internal UnityAction callback;
    }
  }
}
