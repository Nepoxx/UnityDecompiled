// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryElementDataManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class MemoryElementDataManager
  {
    private static int SortByMemoryClassName(ObjectInfo x, ObjectInfo y)
    {
      return y.className.CompareTo(x.className);
    }

    private static int SortByMemorySize(MemoryElement x, MemoryElement y)
    {
      return y.totalMemory.CompareTo(x.totalMemory);
    }

    private static MemoryElementDataManager.ObjectTypeFilter GetObjectTypeFilter(ObjectInfo info)
    {
      switch (info.reason)
      {
        case 1:
          return MemoryElementDataManager.ObjectTypeFilter.BuiltinResource;
        case 2:
          return MemoryElementDataManager.ObjectTypeFilter.DontSave;
        case 3:
        case 8:
        case 9:
          return MemoryElementDataManager.ObjectTypeFilter.Asset;
        case 10:
          return MemoryElementDataManager.ObjectTypeFilter.Other;
        default:
          return MemoryElementDataManager.ObjectTypeFilter.Scene;
      }
    }

    private static bool HasValidNames(List<MemoryElement> memory)
    {
      for (int index = 0; index < memory.Count; ++index)
      {
        if (!string.IsNullOrEmpty(memory[index].name))
          return true;
      }
      return false;
    }

    private static List<MemoryElement> GenerateObjectTypeGroups(ObjectInfo[] memory, MemoryElementDataManager.ObjectTypeFilter filter)
    {
      List<MemoryElement> memoryElementList1 = new List<MemoryElement>();
      MemoryElement memoryElement1 = (MemoryElement) null;
      foreach (ObjectInfo objectInfo in memory)
      {
        if (MemoryElementDataManager.GetObjectTypeFilter(objectInfo) == filter)
        {
          if (memoryElement1 == null || objectInfo.className != memoryElement1.name)
          {
            memoryElement1 = new MemoryElement(objectInfo.className);
            memoryElementList1.Add(memoryElement1);
          }
          memoryElement1.AddChild(new MemoryElement(objectInfo, true));
        }
      }
      List<MemoryElement> memoryElementList2 = memoryElementList1;
      // ISSUE: reference to a compiler-generated field
      if (MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache0 = new Comparison<MemoryElement>(MemoryElementDataManager.SortByMemorySize);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<MemoryElement> fMgCache0 = MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache0;
      memoryElementList2.Sort(fMgCache0);
      foreach (MemoryElement memoryElement2 in memoryElementList1)
      {
        List<MemoryElement> children = memoryElement2.children;
        // ISSUE: reference to a compiler-generated field
        if (MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache1 = new Comparison<MemoryElement>(MemoryElementDataManager.SortByMemorySize);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<MemoryElement> fMgCache1 = MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache1;
        children.Sort(fMgCache1);
        if (filter == MemoryElementDataManager.ObjectTypeFilter.Other && !MemoryElementDataManager.HasValidNames(memoryElement2.children))
          memoryElement2.children.Clear();
      }
      return memoryElementList1;
    }

    public static MemoryElement GetTreeRoot(ObjectMemoryInfo[] memoryObjectList, int[] referencesIndices)
    {
      ObjectInfo[] memory = new ObjectInfo[memoryObjectList.Length];
      for (int index = 0; index < memoryObjectList.Length; ++index)
        memory[index] = new ObjectInfo()
        {
          instanceId = memoryObjectList[index].instanceId,
          memorySize = memoryObjectList[index].memorySize,
          reason = memoryObjectList[index].reason,
          name = memoryObjectList[index].name,
          className = memoryObjectList[index].className
        };
      int num = 0;
      for (int index1 = 0; index1 < memoryObjectList.Length; ++index1)
      {
        for (int index2 = 0; index2 < memoryObjectList[index1].count; ++index2)
        {
          int referencesIndex = referencesIndices[index2 + num];
          if (memory[referencesIndex].referencedBy == null)
            memory[referencesIndex].referencedBy = new List<ObjectInfo>();
          memory[referencesIndex].referencedBy.Add(memory[index1]);
        }
        num += memoryObjectList[index1].count;
      }
      MemoryElement memoryElement = new MemoryElement();
      ObjectInfo[] array = memory;
      // ISSUE: reference to a compiler-generated field
      if (MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache2 = new Comparison<ObjectInfo>(MemoryElementDataManager.SortByMemoryClassName);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<ObjectInfo> fMgCache2 = MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache2;
      Array.Sort<ObjectInfo>(array, fMgCache2);
      memoryElement.AddChild(new MemoryElement("Scene Memory", MemoryElementDataManager.GenerateObjectTypeGroups(memory, MemoryElementDataManager.ObjectTypeFilter.Scene)));
      memoryElement.AddChild(new MemoryElement("Assets", MemoryElementDataManager.GenerateObjectTypeGroups(memory, MemoryElementDataManager.ObjectTypeFilter.Asset)));
      memoryElement.AddChild(new MemoryElement("Builtin Resources", MemoryElementDataManager.GenerateObjectTypeGroups(memory, MemoryElementDataManager.ObjectTypeFilter.BuiltinResource)));
      memoryElement.AddChild(new MemoryElement("Not Saved", MemoryElementDataManager.GenerateObjectTypeGroups(memory, MemoryElementDataManager.ObjectTypeFilter.DontSave)));
      memoryElement.AddChild(new MemoryElement("Other", MemoryElementDataManager.GenerateObjectTypeGroups(memory, MemoryElementDataManager.ObjectTypeFilter.Other)));
      List<MemoryElement> children = memoryElement.children;
      // ISSUE: reference to a compiler-generated field
      if (MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache3 = new Comparison<MemoryElement>(MemoryElementDataManager.SortByMemorySize);
      }
      // ISSUE: reference to a compiler-generated field
      Comparison<MemoryElement> fMgCache3 = MemoryElementDataManager.\u003C\u003Ef__mg\u0024cache3;
      children.Sort(fMgCache3);
      return memoryElement;
    }

    private enum ObjectTypeFilter
    {
      Scene,
      Asset,
      BuiltinResource,
      DontSave,
      Other,
    }
  }
}
