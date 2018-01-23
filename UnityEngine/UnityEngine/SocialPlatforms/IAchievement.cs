// Decompiled with JetBrains decompiler
// Type: UnityEngine.SocialPlatforms.IAchievement
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.SocialPlatforms
{
  public interface IAchievement
  {
    void ReportProgress(Action<bool> callback);

    string id { get; set; }

    double percentCompleted { get; set; }

    bool completed { get; }

    bool hidden { get; }

    DateTime lastReportedDate { get; }
  }
}
