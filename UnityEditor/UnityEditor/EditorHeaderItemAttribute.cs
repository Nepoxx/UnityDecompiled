// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorHeaderItemAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  internal sealed class EditorHeaderItemAttribute : CallbackOrderAttribute
  {
    public System.Type TargetType;

    public EditorHeaderItemAttribute(System.Type targetType, int priority = 1)
    {
      this.TargetType = targetType;
      this.m_CallbackOrder = priority;
    }

    [RequiredSignature]
    private static bool SignatureBool(Rect rectangle, UnityEngine.Object[] targetObjets)
    {
      return EditorHeaderItemAttribute.SignatureBool_Injected(ref rectangle, targetObjets);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SignatureBool_Injected(ref Rect rectangle, UnityEngine.Object[] targetObjets);
  }
}
