// Decompiled with JetBrains decompiler
// Type: UnityEngine.SetupCoroutine
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequiredByNativeCode]
  internal class SetupCoroutine
  {
    [RequiredByNativeCode]
    [SecuritySafeCritical]
    public static unsafe void InvokeMoveNext(IEnumerator enumerator, IntPtr returnValueAddress)
    {
      if (returnValueAddress == IntPtr.Zero)
        throw new ArgumentException("Return value address cannot be 0.", nameof (returnValueAddress));
      *(sbyte*) (void*) returnValueAddress = (sbyte) enumerator.MoveNext();
    }

    [RequiredByNativeCode]
    public static object InvokeMember(object behaviour, string name, object variable)
    {
      object[] args = (object[]) null;
      if (variable != null)
        args = new object[1]{ variable };
      return behaviour.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, behaviour, args, (ParameterModifier[]) null, (CultureInfo) null, (string[]) null);
    }

    public static object InvokeStatic(System.Type klass, string name, object variable)
    {
      object[] args = (object[]) null;
      if (variable != null)
        args = new object[1]{ variable };
      return klass.InvokeMember(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, (object) null, args, (ParameterModifier[]) null, (CultureInfo) null, (string[]) null);
    }
  }
}
