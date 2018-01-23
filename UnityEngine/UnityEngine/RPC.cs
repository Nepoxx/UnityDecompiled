// Decompiled with JetBrains decompiler
// Type: UnityEngine.RPC
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Attribute for setting up RPC functions.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [RequiredByNativeCode(Optional = true)]
  [Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
  public sealed class RPC : Attribute
  {
  }
}
