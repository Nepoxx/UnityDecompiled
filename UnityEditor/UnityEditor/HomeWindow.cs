// Decompiled with JetBrains decompiler
// Type: UnityEditor.HomeWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;

namespace UnityEditor
{
  internal static class HomeWindow
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Show(HomeWindow.HomeMode mode);

    public enum HomeMode
    {
      Login,
      License,
      Launching,
      NewProjectOnly,
      OpenProjectOnly,
      ManageLicense,
      Welcome,
      Tutorial,
    }
  }
}
