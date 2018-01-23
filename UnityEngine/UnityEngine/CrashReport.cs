// Decompiled with JetBrains decompiler
// Type: UnityEngine.CrashReport
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Holds data for a single application crash event and provides access to all gathered crash reports.</para>
  /// </summary>
  public sealed class CrashReport
  {
    private static object reportsLock = new object();
    private static List<CrashReport> internalReports;
    private readonly string id;
    /// <summary>
    ///   <para>Time, when the crash occured.</para>
    /// </summary>
    public readonly DateTime time;
    /// <summary>
    ///   <para>Crash report data as formatted text.</para>
    /// </summary>
    public readonly string text;

    private CrashReport(string id, DateTime time, string text)
    {
      this.id = id;
      this.time = time;
      this.text = text;
    }

    private static int Compare(CrashReport c1, CrashReport c2)
    {
      long ticks1 = c1.time.Ticks;
      long ticks2 = c2.time.Ticks;
      if (ticks1 > ticks2)
        return 1;
      return ticks1 < ticks2 ? -1 : 0;
    }

    private static void PopulateReports()
    {
      lock (CrashReport.reportsLock)
      {
        if (CrashReport.internalReports != null)
          return;
        string[] reports = CrashReport.GetReports();
        CrashReport.internalReports = new List<CrashReport>(reports.Length);
        foreach (string id in reports)
        {
          double secondsSinceUnixEpoch;
          string reportData = CrashReport.GetReportData(id, out secondsSinceUnixEpoch);
          DateTime time = new DateTime(1970, 1, 1).AddSeconds(secondsSinceUnixEpoch);
          CrashReport.internalReports.Add(new CrashReport(id, time, reportData));
        }
        List<CrashReport> internalReports = CrashReport.internalReports;
        // ISSUE: reference to a compiler-generated field
        if (CrashReport.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CrashReport.\u003C\u003Ef__mg\u0024cache0 = new Comparison<CrashReport>(CrashReport.Compare);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<CrashReport> fMgCache0 = CrashReport.\u003C\u003Ef__mg\u0024cache0;
        internalReports.Sort(fMgCache0);
      }
    }

    /// <summary>
    ///   <para>Returns all currently available reports in a new array.</para>
    /// </summary>
    public static CrashReport[] reports
    {
      get
      {
        CrashReport.PopulateReports();
        lock (CrashReport.reportsLock)
          return CrashReport.internalReports.ToArray();
      }
    }

    /// <summary>
    ///   <para>Returns last crash report, or null if no reports are available.</para>
    /// </summary>
    public static CrashReport lastReport
    {
      get
      {
        CrashReport.PopulateReports();
        lock (CrashReport.reportsLock)
        {
          if (CrashReport.internalReports.Count > 0)
            return CrashReport.internalReports[CrashReport.internalReports.Count - 1];
        }
        return (CrashReport) null;
      }
    }

    /// <summary>
    ///   <para>Remove all reports from available reports list.</para>
    /// </summary>
    public static void RemoveAll()
    {
      foreach (CrashReport report in CrashReport.reports)
        report.Remove();
    }

    /// <summary>
    ///   <para>Remove report from available reports list.</para>
    /// </summary>
    public void Remove()
    {
      if (!CrashReport.RemoveReport(this.id))
        return;
      lock (CrashReport.reportsLock)
        CrashReport.internalReports.Remove(this);
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string[] GetReports();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetReportData(string id, out double secondsSinceUnixEpoch);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool RemoveReport(string id);
  }
}
