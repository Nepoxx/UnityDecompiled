// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ComponentUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  public sealed class ComponentUtility
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool MoveComponentUp(Component component);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool MoveComponentDown(Component component);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool CopyComponent(Component component);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool PasteComponentValues(Component component);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool PasteComponentAsNew(GameObject go);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CollectConnectedComponents(GameObject targetGameObject, Component[] components, bool copy, out Component[] outCollectedComponents, out string outErrorMessage);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool MoveComponentToGameObject(Component component, GameObject targetGameObject, [DefaultValue("false")] bool validateOnly);

    [ExcludeFromDocs]
    internal static bool MoveComponentToGameObject(Component component, GameObject targetGameObject)
    {
      bool validateOnly = false;
      return ComponentUtility.MoveComponentToGameObject(component, targetGameObject, validateOnly);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool MoveComponentRelativeToComponent(Component component, Component targetComponent, bool aboveTarget, [DefaultValue("false")] bool validateOnly);

    [ExcludeFromDocs]
    internal static bool MoveComponentRelativeToComponent(Component component, Component targetComponent, bool aboveTarget)
    {
      bool validateOnly = false;
      return ComponentUtility.MoveComponentRelativeToComponent(component, targetComponent, aboveTarget, validateOnly);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool MoveComponentsRelativeToComponents(Component[] components, Component[] targetComponents, bool aboveTarget, [DefaultValue("false")] bool validateOnly);

    [ExcludeFromDocs]
    internal static bool MoveComponentsRelativeToComponents(Component[] components, Component[] targetComponents, bool aboveTarget)
    {
      bool validateOnly = false;
      return ComponentUtility.MoveComponentsRelativeToComponents(components, targetComponents, aboveTarget, validateOnly);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CopyComponentToGameObject(Component component, GameObject targetGameObject, bool validateOnly, out Component outNewComponent);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CopyComponentToGameObjects(Component component, GameObject[] targetGameObjects, bool validateOnly, out Component[] outNewComponents);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CopyComponentRelativeToComponent(Component component, Component targetComponent, bool aboveTarget, bool validateOnly, out Component outNewComponent);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CopyComponentRelativeToComponents(Component component, Component[] targetComponents, bool aboveTarget, bool validateOnly, out Component[] outNewComponents);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CopyComponentsRelativeToComponents(Component[] components, Component[] targetComponents, bool aboveTarget, bool validateOnly, out Component[] outNewComponents);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool WarnCanAddScriptComponent(GameObject gameObject, MonoScript script);

    private static bool CompareComponentOrderAndTypes(List<Component> srcComponents, List<Component> dstComponents)
    {
      if (srcComponents.Count != dstComponents.Count)
        return false;
      for (int index = 0; index != srcComponents.Count; ++index)
      {
        if (srcComponents[index].GetType() != dstComponents[index].GetType())
          return false;
      }
      return true;
    }

    private static void DestroyComponents(List<Component> components)
    {
      for (int index = components.Count - 1; index >= 0; --index)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) components[index]);
    }

    public static void DestroyComponentsMatching(GameObject dst, ComponentUtility.IsDesiredComponent componentFilter)
    {
      List<Component> componentList = new List<Component>();
      dst.GetComponents<Component>(componentList);
      componentList.RemoveAll((Predicate<Component>) (x => !componentFilter(x)));
      ComponentUtility.DestroyComponents(componentList);
    }

    public static void ReplaceComponentsIfDifferent(GameObject src, GameObject dst, ComponentUtility.IsDesiredComponent componentFilter)
    {
      List<Component> componentList1 = new List<Component>();
      src.GetComponents<Component>(componentList1);
      componentList1.RemoveAll((Predicate<Component>) (x => !componentFilter(x)));
      List<Component> componentList2 = new List<Component>();
      dst.GetComponents<Component>(componentList2);
      componentList2.RemoveAll((Predicate<Component>) (x => !componentFilter(x)));
      if (!ComponentUtility.CompareComponentOrderAndTypes(componentList1, componentList2))
      {
        ComponentUtility.DestroyComponents(componentList2);
        componentList2.Clear();
        for (int index = 0; index != componentList1.Count; ++index)
        {
          Component component = dst.AddComponent(componentList1[index].GetType());
          componentList2.Add(component);
        }
      }
      for (int index = 0; index != componentList1.Count; ++index)
        EditorUtility.CopySerializedIfDifferent((UnityEngine.Object) componentList1[index], (UnityEngine.Object) componentList2[index]);
    }

    public delegate bool IsDesiredComponent(Component c);
  }
}
