// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyTypeInfoGenerator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using Mono.Cecil;
using Mono.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.SerializationLogic;
using UnityEngine;

namespace UnityEditor
{
  internal class AssemblyTypeInfoGenerator
  {
    private List<AssemblyTypeInfoGenerator.ClassInfo> classes_ = new List<AssemblyTypeInfoGenerator.ClassInfo>();
    private Unity.SerializationLogic.TypeResolver typeResolver = new Unity.SerializationLogic.TypeResolver((GenericInstanceMethod) null);
    private AssemblyDefinition assembly_;

    public AssemblyTypeInfoGenerator(string assembly, string[] searchDirs)
    {
      this.assembly_ = AssemblyDefinition.ReadAssembly(assembly, new ReaderParameters()
      {
        AssemblyResolver = AssemblyTypeInfoGenerator.AssemblyResolver.WithSearchDirs(searchDirs)
      });
    }

    public AssemblyTypeInfoGenerator(string assembly, IAssemblyResolver resolver)
    {
      this.assembly_ = AssemblyDefinition.ReadAssembly(assembly, new ReaderParameters()
      {
        AssemblyResolver = resolver
      });
    }

    public AssemblyTypeInfoGenerator.ClassInfo[] ClassInfoArray
    {
      get
      {
        return this.classes_.ToArray();
      }
    }

    private string GetMonoEmbeddedFullTypeNameFor(TypeReference type)
    {
      TypeSpecification typeSpecification = type as TypeSpecification;
      if (typeSpecification != null && typeSpecification.IsRequiredModifier)
        type = typeSpecification.ElementType;
      else if (type.IsRequiredModifier)
        type = type.GetElementType();
      string str = type.FullName;
      if (type.HasGenericParameters || type.IsGenericInstance)
        str = str.Replace('<', '[').Replace('>', ']');
      return str.Replace('/', '+');
    }

    private TypeReference ResolveGenericInstanceType(TypeReference typeToResolve, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      ArrayType arrayType = typeToResolve as ArrayType;
      if (arrayType != null)
        typeToResolve = (TypeReference) new ArrayType(this.ResolveGenericInstanceType(arrayType.ElementType, genericInstanceTypeMap), arrayType.Rank);
      while (genericInstanceTypeMap.ContainsKey(typeToResolve))
        typeToResolve = genericInstanceTypeMap[typeToResolve];
      if (typeToResolve.IsGenericInstance)
      {
        GenericInstanceType genericInstanceType = (GenericInstanceType) typeToResolve;
        typeToResolve = this.MakeGenericInstance(genericInstanceType.ElementType, (IEnumerable<TypeReference>) genericInstanceType.GenericArguments, genericInstanceTypeMap);
      }
      return typeToResolve;
    }

    private void AddType(TypeReference typeRef, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      if (this.classes_.Any<AssemblyTypeInfoGenerator.ClassInfo>((Func<AssemblyTypeInfoGenerator.ClassInfo, bool>) (x => x.name == this.GetMonoEmbeddedFullTypeNameFor(typeRef))))
        return;
      TypeDefinition type;
      try
      {
        type = typeRef.Resolve();
      }
      catch (AssemblyResolutionException ex)
      {
        return;
      }
      catch (NotSupportedException ex)
      {
        return;
      }
      if (type == null)
        return;
      if (typeRef.IsGenericInstance)
      {
        Collection<TypeReference> genericArguments = ((GenericInstanceType) typeRef).GenericArguments;
        Collection<GenericParameter> genericParameters = type.GenericParameters;
        for (int index = 0; index < genericArguments.Count; ++index)
        {
          if (genericParameters[index] != genericArguments[index])
            genericInstanceTypeMap[(TypeReference) genericParameters[index]] = genericArguments[index];
        }
        this.typeResolver.Add((GenericInstanceType) typeRef);
      }
      bool flag = false;
      try
      {
        flag = UnitySerializationLogic.ShouldImplementIDeserializable((TypeReference) type);
      }
      catch
      {
      }
      if (!flag)
      {
        this.AddNestedTypes(type, genericInstanceTypeMap);
      }
      else
      {
        this.classes_.Add(new AssemblyTypeInfoGenerator.ClassInfo()
        {
          name = this.GetMonoEmbeddedFullTypeNameFor(typeRef),
          fields = this.GetFields(type, typeRef.IsGenericInstance, genericInstanceTypeMap)
        });
        this.AddNestedTypes(type, genericInstanceTypeMap);
        this.AddBaseType(typeRef, genericInstanceTypeMap);
      }
      if (!typeRef.IsGenericInstance)
        return;
      this.typeResolver.Remove((GenericInstanceType) typeRef);
    }

    private void AddNestedTypes(TypeDefinition type, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      foreach (TypeReference nestedType in type.NestedTypes)
        this.AddType(nestedType, genericInstanceTypeMap);
    }

    private void AddBaseType(TypeReference typeRef, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      TypeReference typeRef1 = typeRef.Resolve().BaseType;
      if (typeRef1 == null)
        return;
      if (typeRef.IsGenericInstance && typeRef1.IsGenericInstance)
      {
        GenericInstanceType genericInstanceType = (GenericInstanceType) typeRef1;
        typeRef1 = this.MakeGenericInstance(genericInstanceType.ElementType, (IEnumerable<TypeReference>) genericInstanceType.GenericArguments, genericInstanceTypeMap);
      }
      this.AddType(typeRef1, genericInstanceTypeMap);
    }

    private TypeReference MakeGenericInstance(TypeReference genericClass, IEnumerable<TypeReference> arguments, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      GenericInstanceType genericInstanceType = new GenericInstanceType(genericClass);
      foreach (TypeReference typeReference in arguments.Select<TypeReference, TypeReference>((Func<TypeReference, TypeReference>) (x => this.ResolveGenericInstanceType(x, genericInstanceTypeMap))))
        genericInstanceType.GenericArguments.Add(typeReference);
      return (TypeReference) genericInstanceType;
    }

    private AssemblyTypeInfoGenerator.FieldInfo[] GetFields(TypeDefinition type, bool isGenericInstance, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      List<AssemblyTypeInfoGenerator.FieldInfo> fieldInfoList = new List<AssemblyTypeInfoGenerator.FieldInfo>();
      foreach (FieldDefinition field in type.Fields)
      {
        AssemblyTypeInfoGenerator.FieldInfo? fieldInfo = this.GetFieldInfo(type, field, isGenericInstance, genericInstanceTypeMap);
        if (fieldInfo.HasValue)
          fieldInfoList.Add(fieldInfo.Value);
      }
      return fieldInfoList.ToArray();
    }

    private static CustomAttribute GetFixedBufferAttribute(FieldDefinition fieldDefinition)
    {
      if (!fieldDefinition.HasCustomAttributes)
        return (CustomAttribute) null;
      return fieldDefinition.CustomAttributes.SingleOrDefault<CustomAttribute>((Func<CustomAttribute, bool>) (a => a.AttributeType.FullName == "System.Runtime.CompilerServices.FixedBufferAttribute"));
    }

    private static int GetFixedBufferLength(CustomAttribute fixedBufferAttribute)
    {
      return (int) fixedBufferAttribute.ConstructorArguments[1].Value;
    }

    private static string GetFixedBufferTypename(CustomAttribute fixedBufferAttribute)
    {
      return ((MemberReference) fixedBufferAttribute.ConstructorArguments[0].Value).Name;
    }

    private AssemblyTypeInfoGenerator.FieldInfo? GetFieldInfo(TypeDefinition type, FieldDefinition field, bool isDeclaringTypeGenericInstance, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      if (!this.WillSerialize(field))
        return new AssemblyTypeInfoGenerator.FieldInfo?();
      AssemblyTypeInfoGenerator.FieldInfo fieldInfo = new AssemblyTypeInfoGenerator.FieldInfo();
      fieldInfo.name = field.Name;
      TypeReference type1 = !isDeclaringTypeGenericInstance ? field.FieldType : this.ResolveGenericInstanceType(field.FieldType, genericInstanceTypeMap);
      fieldInfo.type = this.GetMonoEmbeddedFullTypeNameFor(type1);
      fieldInfo.flags = AssemblyTypeInfoGenerator.FieldInfoFlags.None;
      CustomAttribute fixedBufferAttribute = AssemblyTypeInfoGenerator.GetFixedBufferAttribute(field);
      if (fixedBufferAttribute != null)
      {
        fieldInfo.flags |= AssemblyTypeInfoGenerator.FieldInfoFlags.FixedBuffer;
        fieldInfo.fixedBufferLength = AssemblyTypeInfoGenerator.GetFixedBufferLength(fixedBufferAttribute);
        fieldInfo.fixedBufferTypename = AssemblyTypeInfoGenerator.GetFixedBufferTypename(fixedBufferAttribute);
      }
      return new AssemblyTypeInfoGenerator.FieldInfo?(fieldInfo);
    }

    private bool WillSerialize(FieldDefinition field)
    {
      try
      {
        return UnitySerializationLogic.WillUnitySerialize(field, this.typeResolver);
      }
      catch (Exception ex)
      {
        Debug.LogFormat("Field '{0}' from '{1}', exception {2}", (object) field.FullName, (object) field.Module.FileName, (object) ex.Message);
        return false;
      }
    }

    public AssemblyTypeInfoGenerator.ClassInfo[] GatherClassInfo()
    {
      foreach (ModuleDefinition module in this.assembly_.Modules)
      {
        foreach (TypeDefinition type in module.Types)
        {
          if (!(type.Name == "<Module>"))
            this.AddType((TypeReference) type, new Dictionary<TypeReference, TypeReference>());
        }
      }
      return this.classes_.ToArray();
    }

    [System.Flags]
    public enum FieldInfoFlags
    {
      None = 0,
      FixedBuffer = 1,
    }

    public struct FieldInfo
    {
      public string name;
      public string type;
      public AssemblyTypeInfoGenerator.FieldInfoFlags flags;
      public int fixedBufferLength;
      public string fixedBufferTypename;
    }

    public struct ClassInfo
    {
      public string name;
      public AssemblyTypeInfoGenerator.FieldInfo[] fields;
    }

    private class AssemblyResolver : BaseAssemblyResolver
    {
      private readonly IDictionary m_Assemblies;

      private AssemblyResolver()
        : this((IDictionary) new Hashtable())
      {
      }

      private AssemblyResolver(IDictionary assemblyCache)
      {
        this.m_Assemblies = assemblyCache;
      }

      public static IAssemblyResolver WithSearchDirs(params string[] searchDirs)
      {
        AssemblyTypeInfoGenerator.AssemblyResolver assemblyResolver = new AssemblyTypeInfoGenerator.AssemblyResolver();
        foreach (string searchDir in searchDirs)
          assemblyResolver.AddSearchDirectory(searchDir);
        assemblyResolver.RemoveSearchDirectory(".");
        assemblyResolver.RemoveSearchDirectory("bin");
        return (IAssemblyResolver) assemblyResolver;
      }

      public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
      {
        AssemblyDefinition assembly = (AssemblyDefinition) this.m_Assemblies[(object) name.Name];
        if (assembly != null)
          return assembly;
        AssemblyDefinition assemblyDefinition = base.Resolve(name, parameters);
        this.m_Assemblies[(object) name.Name] = (object) assemblyDefinition;
        return assemblyDefinition;
      }
    }
  }
}
