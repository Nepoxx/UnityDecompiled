// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeClassInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class RuntimeInitializeClassInfo
  {
    private string m_AssemblyName;
    private string m_ClassName;
    private string[] m_MethodNames;
    private RuntimeInitializeLoadType[] m_LoadTypes;

    internal string assemblyName
    {
      get
      {
        return this.m_AssemblyName;
      }
      set
      {
        this.m_AssemblyName = value;
      }
    }

    internal string className
    {
      get
      {
        return this.m_ClassName;
      }
      set
      {
        this.m_ClassName = value;
      }
    }

    internal string[] methodNames
    {
      get
      {
        return this.m_MethodNames;
      }
      set
      {
        this.m_MethodNames = value;
      }
    }

    internal RuntimeInitializeLoadType[] loadTypes
    {
      get
      {
        return this.m_LoadTypes;
      }
      set
      {
        this.m_LoadTypes = value;
      }
    }
  }
}
