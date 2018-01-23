// Decompiled with JetBrains decompiler
// Type: UnityEditor.Macros.MethodEvaluator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityEditor.Macros
{
  public static class MethodEvaluator
  {
    private static readonly BinaryFormatter s_Formatter = new BinaryFormatter() { AssemblyFormat = FormatterAssemblyStyle.Simple };

    public static object Eval(string assemblyFile, string typeName, string methodName, System.Type[] paramTypes, object[] args)
    {
      MethodEvaluator.AssemblyResolver assemblyResolver = new MethodEvaluator.AssemblyResolver(Path.GetDirectoryName(assemblyFile));
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(assemblyResolver.AssemblyResolve);
      try
      {
        Assembly assembly = Assembly.LoadFrom(assemblyFile);
        MethodInfo method = assembly.GetType(typeName, true).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, paramTypes, (ParameterModifier[]) null);
        if (method == null)
          throw new ArgumentException(string.Format("Method {0}.{1}({2}) not found in assembly {3}!", (object) typeName, (object) methodName, (object) MethodEvaluator.ToCommaSeparatedString<System.Type>((IEnumerable<System.Type>) paramTypes), (object) assembly.FullName));
        return method.Invoke((object) null, args);
      }
      finally
      {
        AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(assemblyResolver.AssemblyResolve);
      }
    }

    public static object ExecuteExternalCode(string parcel)
    {
      using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(parcel)))
      {
        if ((string) MethodEvaluator.s_Formatter.Deserialize((Stream) memoryStream) != "com.unity3d.automation")
          throw new Exception("Invalid parcel for external code execution.");
        string str = (string) MethodEvaluator.s_Formatter.Deserialize((Stream) memoryStream);
        MethodEvaluator.AssemblyResolver assemblyResolver = new MethodEvaluator.AssemblyResolver(Path.GetDirectoryName(str));
        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(assemblyResolver.AssemblyResolve);
        Assembly assembly = Assembly.LoadFrom(str);
        try
        {
          System.Type target = (System.Type) MethodEvaluator.s_Formatter.Deserialize((Stream) memoryStream);
          string name = (string) MethodEvaluator.s_Formatter.Deserialize((Stream) memoryStream);
          System.Type[] types = (System.Type[]) MethodEvaluator.s_Formatter.Deserialize((Stream) memoryStream);
          MethodInfo method = target.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, types, (ParameterModifier[]) null);
          if (method == null)
            throw new Exception(string.Format("Could not find method {0}.{1} in assembly {2} located in {3}.", (object) target.FullName, (object) name, (object) assembly.GetName().Name, (object) str));
          object[] args = (object[]) MethodEvaluator.s_Formatter.Deserialize((Stream) memoryStream);
          return MethodEvaluator.ExecuteCode(target, method, args);
        }
        finally
        {
          AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(assemblyResolver.AssemblyResolve);
        }
      }
    }

    private static object ExecuteCode(System.Type target, MethodInfo method, object[] args)
    {
      return method.Invoke(!method.IsStatic ? MethodEvaluator.GetActor(target) : (object) null, args);
    }

    private static object GetActor(System.Type type)
    {
      ConstructorInfo constructor = type.GetConstructor(new System.Type[0]);
      return constructor == null ? (object) null : constructor.Invoke(new object[0]);
    }

    private static string ToCommaSeparatedString<T>(IEnumerable<T> items)
    {
      return string.Join(", ", items.Select<T, string>((Func<T, string>) (o => o.ToString())).ToArray<string>());
    }

    private class AssemblyResolver
    {
      private readonly string m_AssemblyDirectory;

      public AssemblyResolver(string assemblyDirectory)
      {
        this.m_AssemblyDirectory = assemblyDirectory;
      }

      public Assembly AssemblyResolve(object sender, ResolveEventArgs args)
      {
        string str = Path.Combine(this.m_AssemblyDirectory, args.Name.Split(',')[0] + ".dll");
        if (File.Exists(str))
          return Assembly.LoadFrom(str);
        return (Assembly) null;
      }
    }
  }
}
