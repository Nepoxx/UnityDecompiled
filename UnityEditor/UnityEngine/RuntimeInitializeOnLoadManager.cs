// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeOnLoadManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  internal sealed class RuntimeInitializeOnLoadManager
  {
    internal static extern string[] dontStripClassNames { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern RuntimeInitializeMethodInfo[] methodInfos { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateMethodExecutionOrders(int[] changedIndices, int[] changedOrder);
  }
}
