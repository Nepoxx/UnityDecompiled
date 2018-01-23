// Decompiled with JetBrains decompiler
// Type: UnityEngine.PlayerPrefsException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>An exception thrown by the PlayerPrefs class in a  web player build.</para>
  /// </summary>
  public sealed class PlayerPrefsException : Exception
  {
    public PlayerPrefsException(string error)
      : base(error)
    {
    }
  }
}
