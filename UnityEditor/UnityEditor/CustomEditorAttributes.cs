// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomEditorAttributes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class CustomEditorAttributes
  {
    private static readonly List<CustomEditorAttributes.MonoEditorType> kSCustomEditors = new List<CustomEditorAttributes.MonoEditorType>();
    private static readonly List<CustomEditorAttributes.MonoEditorType> kSCustomMultiEditors = new List<CustomEditorAttributes.MonoEditorType>();
    private static bool s_Initialized;

    internal static System.Type FindCustomEditorType(UnityEngine.Object o, bool multiEdit)
    {
      return CustomEditorAttributes.FindCustomEditorTypeByType(o.GetType(), multiEdit);
    }

    internal static System.Type FindCustomEditorTypeByType(System.Type type, bool multiEdit)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey1 typeCAnonStorey1 = new CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey1.type = type;
      if (!CustomEditorAttributes.s_Initialized)
      {
        Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
        for (int index = loadedAssemblies.Length - 1; index >= 0; --index)
          CustomEditorAttributes.Rebuild(loadedAssemblies[index]);
        CustomEditorAttributes.s_Initialized = true;
      }
      // ISSUE: reference to a compiler-generated field
      if (typeCAnonStorey1.type == null)
        return (System.Type) null;
      List<CustomEditorAttributes.MonoEditorType> source1 = !multiEdit ? CustomEditorAttributes.kSCustomEditors : CustomEditorAttributes.kSCustomMultiEditors;
      for (int index = 0; index < 2; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        for (System.Type type1 = typeCAnonStorey1.type; type1 != null; type1 = type1.BaseType)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          IEnumerable<CustomEditorAttributes.MonoEditorType> source2 = source1.Where<CustomEditorAttributes.MonoEditorType>(new Func<CustomEditorAttributes.MonoEditorType, bool>(new CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey0() { \u003C\u003Ef__ref\u00241 = typeCAnonStorey1, inspected1 = type1, pass1 = index }.\u003C\u003Em__0));
          if ((UnityEngine.Object) GraphicsSettings.renderPipelineAsset != (UnityEngine.Object) null)
          {
            System.Type type2 = GraphicsSettings.renderPipelineAsset.GetType();
            foreach (CustomEditorAttributes.MonoEditorType monoEditorType in source2)
            {
              if (monoEditorType.m_RenderPipelineType == type2)
                return monoEditorType.m_InspectorType;
            }
          }
          CustomEditorAttributes.MonoEditorType monoEditorType1 = source2.FirstOrDefault<CustomEditorAttributes.MonoEditorType>((Func<CustomEditorAttributes.MonoEditorType, bool>) (x => x.m_RenderPipelineType == null));
          if (monoEditorType1 != null)
            return monoEditorType1.m_InspectorType;
        }
      }
      return (System.Type) null;
    }

    private static bool IsAppropriateEditor(CustomEditorAttributes.MonoEditorType editor, System.Type parentClass, bool isChildClass, bool isFallback)
    {
      if (isChildClass && !editor.m_EditorForChildClasses || isFallback != editor.m_IsFallback)
        return false;
      return parentClass == editor.m_InspectedType || parentClass.IsGenericType && parentClass.GetGenericTypeDefinition() == editor.m_InspectedType;
    }

    internal static void Rebuild(Assembly assembly)
    {
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        foreach (CustomEditor customAttribute in type.GetCustomAttributes(typeof (CustomEditor), false))
        {
          CustomEditorAttributes.MonoEditorType monoEditorType = new CustomEditorAttributes.MonoEditorType();
          if (customAttribute.m_InspectedType == null)
            Debug.Log((object) ("Can't load custom inspector " + type.Name + " because the inspected type is null."));
          else if (!type.IsSubclassOf(typeof (Editor)))
          {
            if (!(type.FullName == "TweakMode") || !type.IsEnum || !(customAttribute.m_InspectedType.FullName == "BloomAndFlares"))
              Debug.LogWarning((object) (type.Name + " uses the CustomEditor attribute but does not inherit from Editor.\nYou must inherit from Editor. See the Editor class script documentation."));
          }
          else
          {
            monoEditorType.m_InspectedType = customAttribute.m_InspectedType;
            monoEditorType.m_InspectorType = type;
            monoEditorType.m_EditorForChildClasses = customAttribute.m_EditorForChildClasses;
            monoEditorType.m_IsFallback = customAttribute.isFallback;
            CustomEditorForRenderPipelineAttribute pipelineAttribute = customAttribute as CustomEditorForRenderPipelineAttribute;
            if (pipelineAttribute != null)
              monoEditorType.m_RenderPipelineType = pipelineAttribute.renderPipelineType;
            CustomEditorAttributes.kSCustomEditors.Add(monoEditorType);
            if (type.GetCustomAttributes(typeof (CanEditMultipleObjects), false).Length > 0)
              CustomEditorAttributes.kSCustomMultiEditors.Add(monoEditorType);
          }
        }
      }
    }

    private class MonoEditorType
    {
      public System.Type m_InspectedType;
      public System.Type m_InspectorType;
      public System.Type m_RenderPipelineType;
      public bool m_EditorForChildClasses;
      public bool m_IsFallback;
    }
  }
}
