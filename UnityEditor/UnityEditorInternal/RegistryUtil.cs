// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.RegistryUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditorInternal
{
  public sealed class RegistryUtil
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern uint GetRegistryUInt32Value(string subKey, string valueName, uint defaultValue, RegistryView view);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetRegistryStringValue(string subKey, string valueName, string defaultValue, RegistryView view);
  }
}
