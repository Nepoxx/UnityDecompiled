// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UWPSDK
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.Scripting.Compilers
{
  internal class UWPSDK
  {
    public readonly Version Version;
    public readonly Version MinVSVersion;

    public UWPSDK(Version version, Version minVSVersion)
    {
      this.Version = version;
      this.MinVSVersion = minVSVersion;
    }
  }
}
