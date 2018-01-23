// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.CalendarUnit
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.iOS
{
  public enum CalendarUnit
  {
    Era = 2,
    Year = 4,
    Month = 8,
    Day = 16, // 0x00000010
    Hour = 32, // 0x00000020
    Minute = 64, // 0x00000040
    Second = 128, // 0x00000080
    Week = 256, // 0x00000100
    Weekday = 512, // 0x00000200
    WeekdayOrdinal = 1024, // 0x00000400
    Quarter = 2048, // 0x00000800
  }
}
