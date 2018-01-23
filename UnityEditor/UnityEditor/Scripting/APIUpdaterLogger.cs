// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.APIUpdaterLogger
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Scripting
{
  internal class APIUpdaterLogger
  {
    public static void WriteToFile(string msg, params object[] args)
    {
      Console.WriteLine("[Script API Updater] {0}", (object) string.Format(msg, args));
    }

    public static void WriteErrorToConsole(string msg, params object[] args)
    {
      Debug.LogErrorFormat(msg, args);
    }

    public static void WriteInfoToConsole(string line)
    {
      Debug.Log((object) line);
    }
  }
}
