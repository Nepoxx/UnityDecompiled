// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeMethodInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class RuntimeInitializeMethodInfo
  {
    private int m_OrderNumber = 0;
    private bool m_IsUnityClass = false;
    private string m_FullClassName;
    private string m_MethodName;

    internal string fullClassName
    {
      get
      {
        return this.m_FullClassName;
      }
      set
      {
        this.m_FullClassName = value;
      }
    }

    internal string methodName
    {
      get
      {
        return this.m_MethodName;
      }
      set
      {
        this.m_MethodName = value;
      }
    }

    internal int orderNumber
    {
      get
      {
        return this.m_OrderNumber;
      }
      set
      {
        this.m_OrderNumber = value;
      }
    }

    internal bool isUnityClass
    {
      get
      {
        return this.m_IsUnityClass;
      }
      set
      {
        this.m_IsUnityClass = value;
      }
    }
  }
}
