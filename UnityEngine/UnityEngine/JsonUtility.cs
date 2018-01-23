// Decompiled with JetBrains decompiler
// Type: UnityEngine.JsonUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  public static class JsonUtility
  {
    public static string ToJson(object obj)
    {
      return JsonUtility.ToJson(obj, false);
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ToJson(object obj, bool prettyPrint);

    public static T FromJson<T>(string json)
    {
      return (T) JsonUtility.FromJson(json, typeof (T));
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern object FromJson(string json, System.Type type);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FromJsonOverwrite(string json, object objectToOverwrite);
  }
}
