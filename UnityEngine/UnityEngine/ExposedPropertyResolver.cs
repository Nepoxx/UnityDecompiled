// Decompiled with JetBrains decompiler
// Type: UnityEngine.ExposedPropertyResolver
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Object that is used to resolve references to an ExposedReference field.</para>
  /// </summary>
  public struct ExposedPropertyResolver
  {
    internal IntPtr table;

    internal static Object ResolveReferenceInternal(IntPtr ptr, PropertyName name, out bool isValid)
    {
      return ExposedPropertyResolver.INTERNAL_CALL_ResolveReferenceInternal(ptr, ref name, out isValid);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Object INTERNAL_CALL_ResolveReferenceInternal(IntPtr ptr, ref PropertyName name, out bool isValid);
  }
}
