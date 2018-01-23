// Decompiled with JetBrains decompiler
// Type: UnityEngine.DiagnosticSwitch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  internal struct DiagnosticSwitch
  {
    public string name;
    public string description;
    public DiagnosticSwitchFlags flags;
    public object value;
    public object minValue;
    public object maxValue;
    public object persistentValue;
    public EnumInfo enumInfo;

    [UsedByNativeCode]
    private static void AppendDiagnosticSwitchToList(List<DiagnosticSwitch> list, string name, string description, DiagnosticSwitchFlags flags, object value, object minValue, object maxValue, object persistentValue, EnumInfo enumInfo)
    {
      list.Add(new DiagnosticSwitch()
      {
        name = name,
        description = description,
        flags = flags,
        value = value,
        minValue = minValue,
        maxValue = maxValue,
        persistentValue = persistentValue,
        enumInfo = enumInfo
      });
    }
  }
}
