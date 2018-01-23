// Decompiled with JetBrains decompiler
// Type: UnityEditor.ConsoleWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Console", useTypeNameAsIconName = true)]
  internal class ConsoleWindow : EditorWindow, IHasCustomMenu
  {
    private static bool ms_LoadedIcons = false;
    private static ConsoleWindow ms_ConsoleWindow = (ConsoleWindow) null;
    private string m_ActiveText = "";
    private int m_ActiveInstanceID = 0;
    private Vector2 m_TextScroll = Vector2.zero;
    private SplitterState spl = new SplitterState(new float[2]{ 70f, 30f }, new int[2]{ 32, 32 }, (int[]) null);
    private int ms_LVHeight = 0;
    private ConsoleWindow.ConsoleAttachProfilerUI m_AttachProfilerUI = new ConsoleWindow.ConsoleAttachProfilerUI();
    private int m_LineHeight;
    private int m_BorderHeight;
    private bool m_HasUpdatedGuiStyles;
    private ListViewState m_ListView;
    private bool m_DevBuild;
    internal static Texture2D iconInfo;
    internal static Texture2D iconWarn;
    internal static Texture2D iconError;
    internal static Texture2D iconInfoSmall;
    internal static Texture2D iconWarnSmall;
    internal static Texture2D iconErrorSmall;
    internal static Texture2D iconInfoMono;
    internal static Texture2D iconWarnMono;
    internal static Texture2D iconErrorMono;

    public ConsoleWindow()
    {
      this.position = new Rect(200f, 200f, 800f, 400f);
      this.m_ListView = new ListViewState(0, 0);
    }

    private static void ShowConsoleWindowImmediate()
    {
      ConsoleWindow.ShowConsoleWindow(true);
    }

    public static void ShowConsoleWindow(bool immediate)
    {
      if ((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow == (UnityEngine.Object) null)
      {
        ConsoleWindow.ms_ConsoleWindow = ScriptableObject.CreateInstance<ConsoleWindow>();
        ConsoleWindow.ms_ConsoleWindow.Show(immediate);
        ConsoleWindow.ms_ConsoleWindow.Focus();
      }
      else
      {
        ConsoleWindow.ms_ConsoleWindow.Show(immediate);
        ConsoleWindow.ms_ConsoleWindow.Focus();
      }
    }

    internal static void LoadIcons()
    {
      if (ConsoleWindow.ms_LoadedIcons)
        return;
      ConsoleWindow.ms_LoadedIcons = true;
      ConsoleWindow.iconInfo = EditorGUIUtility.LoadIcon("console.infoicon");
      ConsoleWindow.iconWarn = EditorGUIUtility.LoadIcon("console.warnicon");
      ConsoleWindow.iconError = EditorGUIUtility.LoadIcon("console.erroricon");
      ConsoleWindow.iconInfoSmall = EditorGUIUtility.LoadIcon("console.infoicon.sml");
      ConsoleWindow.iconWarnSmall = EditorGUIUtility.LoadIcon("console.warnicon.sml");
      ConsoleWindow.iconErrorSmall = EditorGUIUtility.LoadIcon("console.erroricon.sml");
      ConsoleWindow.iconInfoMono = EditorGUIUtility.LoadIcon("console.infoicon.sml");
      ConsoleWindow.iconWarnMono = EditorGUIUtility.LoadIcon("console.warnicon.inactive.sml");
      ConsoleWindow.iconErrorMono = EditorGUIUtility.LoadIcon("console.erroricon.inactive.sml");
      ConsoleWindow.Constants.Init();
    }

    [RequiredByNativeCode]
    public static void LogChanged()
    {
      if ((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow == (UnityEngine.Object) null)
        return;
      ConsoleWindow.ms_ConsoleWindow.DoLogChanged();
    }

    public void DoLogChanged()
    {
      ConsoleWindow.ms_ConsoleWindow.Repaint();
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      ConsoleWindow.ms_ConsoleWindow = this;
      this.m_DevBuild = Unsupported.IsDeveloperBuild();
      ConsoleWindow.Constants.LogStyleLineCount = EditorPrefs.GetInt("ConsoleWindowLogLineCount", 2);
    }

    private void OnDisable()
    {
      if (!((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow == (UnityEngine.Object) this))
        return;
      ConsoleWindow.ms_ConsoleWindow = (ConsoleWindow) null;
    }

    private int RowHeight
    {
      get
      {
        return ConsoleWindow.Constants.LogStyleLineCount * this.m_LineHeight + this.m_BorderHeight;
      }
    }

    private static bool HasMode(int mode, ConsoleWindow.Mode modeToCheck)
    {
      return ((ConsoleWindow.Mode) mode & modeToCheck) != (ConsoleWindow.Mode) 0;
    }

    private static bool HasFlag(ConsoleWindow.ConsoleFlags flags)
    {
      return ((ConsoleWindow.ConsoleFlags) LogEntries.consoleFlags & flags) != (ConsoleWindow.ConsoleFlags) 0;
    }

    private static void SetFlag(ConsoleWindow.ConsoleFlags flags, bool val)
    {
      LogEntries.SetConsoleFlag((int) flags, val);
    }

    internal static Texture2D GetIconForErrorMode(int mode, bool large)
    {
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert | ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError | ConsoleWindow.Mode.ScriptingError | ConsoleWindow.Mode.ScriptCompileError | ConsoleWindow.Mode.GraphCompileError | ConsoleWindow.Mode.ScriptingAssertion))
        return !large ? ConsoleWindow.iconErrorSmall : ConsoleWindow.iconError;
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.AssetImportWarning | ConsoleWindow.Mode.ScriptingWarning | ConsoleWindow.Mode.ScriptCompileWarning))
        return !large ? ConsoleWindow.iconWarnSmall : ConsoleWindow.iconWarn;
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Log | ConsoleWindow.Mode.ScriptingLog))
        return !large ? ConsoleWindow.iconInfoSmall : ConsoleWindow.iconInfo;
      return (Texture2D) null;
    }

    internal static GUIStyle GetStyleForErrorMode(int mode, bool isIcon, bool isSmall)
    {
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert | ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError | ConsoleWindow.Mode.ScriptingError | ConsoleWindow.Mode.ScriptCompileError | ConsoleWindow.Mode.GraphCompileError | ConsoleWindow.Mode.ScriptingAssertion))
      {
        if (isIcon)
        {
          if (isSmall)
            return ConsoleWindow.Constants.IconErrorSmallStyle;
          return ConsoleWindow.Constants.IconErrorStyle;
        }
        if (isSmall)
          return ConsoleWindow.Constants.ErrorSmallStyle;
        return ConsoleWindow.Constants.ErrorStyle;
      }
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.AssetImportWarning | ConsoleWindow.Mode.ScriptingWarning | ConsoleWindow.Mode.ScriptCompileWarning))
      {
        if (isIcon)
        {
          if (isSmall)
            return ConsoleWindow.Constants.IconWarningSmallStyle;
          return ConsoleWindow.Constants.IconWarningStyle;
        }
        if (isSmall)
          return ConsoleWindow.Constants.WarningSmallStyle;
        return ConsoleWindow.Constants.WarningStyle;
      }
      if (isIcon)
      {
        if (isSmall)
          return ConsoleWindow.Constants.IconLogSmallStyle;
        return ConsoleWindow.Constants.IconLogStyle;
      }
      if (isSmall)
        return ConsoleWindow.Constants.LogSmallStyle;
      return ConsoleWindow.Constants.LogStyle;
    }

    internal static GUIStyle GetStatusStyleForErrorMode(int mode)
    {
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert | ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError | ConsoleWindow.Mode.ScriptingError | ConsoleWindow.Mode.ScriptCompileError | ConsoleWindow.Mode.GraphCompileError | ConsoleWindow.Mode.ScriptingAssertion))
        return ConsoleWindow.Constants.StatusError;
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.AssetImportWarning | ConsoleWindow.Mode.ScriptingWarning | ConsoleWindow.Mode.ScriptCompileWarning))
        return ConsoleWindow.Constants.StatusWarn;
      return ConsoleWindow.Constants.StatusLog;
    }

    private static string ContextString(LogEntry entry)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (ConsoleWindow.HasMode(entry.mode, ConsoleWindow.Mode.Error))
        stringBuilder.Append("Error ");
      else if (ConsoleWindow.HasMode(entry.mode, ConsoleWindow.Mode.Log))
        stringBuilder.Append("Log ");
      else
        stringBuilder.Append("Assert ");
      stringBuilder.Append("in file: ");
      stringBuilder.Append(entry.file);
      stringBuilder.Append(" at line: ");
      stringBuilder.Append(entry.line);
      if (entry.errorNum != 0)
      {
        stringBuilder.Append(" and errorNum: ");
        stringBuilder.Append(entry.errorNum);
      }
      return stringBuilder.ToString();
    }

    private static string GetFirstLine(string s)
    {
      int length = s.IndexOf("\n");
      return length == -1 ? s : s.Substring(0, length);
    }

    private static string GetFirstTwoLines(string s)
    {
      int num = s.IndexOf("\n");
      if (num != -1)
      {
        int length = s.IndexOf("\n", num + 1);
        if (length != -1)
          return s.Substring(0, length);
      }
      return s;
    }

    private void SetActiveEntry(LogEntry entry)
    {
      if (entry != null)
      {
        this.m_ActiveText = entry.condition;
        if (this.m_ActiveInstanceID == entry.instanceID)
          return;
        this.m_ActiveInstanceID = entry.instanceID;
        if (entry.instanceID != 0)
          EditorGUIUtility.PingObject(entry.instanceID);
      }
      else
      {
        this.m_ActiveText = string.Empty;
        this.m_ActiveInstanceID = 0;
        this.m_ListView.row = -1;
      }
    }

    private static void ShowConsoleRow(int row)
    {
      ConsoleWindow.ShowConsoleWindow(false);
      if (!(bool) ((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow))
        return;
      ConsoleWindow.ms_ConsoleWindow.m_ListView.row = row;
      ConsoleWindow.ms_ConsoleWindow.m_ListView.selectionChanged = true;
      ConsoleWindow.ms_ConsoleWindow.Repaint();
    }

    private void UpdateListView()
    {
      this.m_HasUpdatedGuiStyles = true;
      int rowHeight = this.RowHeight;
      this.m_ListView.rowHeight = rowHeight;
      this.m_ListView.row = -1;
      this.m_ListView.scrollPos.y = (float) (LogEntries.GetCount() * rowHeight);
    }

    private void OnGUI()
    {
      Event current1 = Event.current;
      ConsoleWindow.LoadIcons();
      if (!this.m_HasUpdatedGuiStyles)
      {
        this.m_LineHeight = Mathf.RoundToInt(ConsoleWindow.Constants.ErrorStyle.lineHeight);
        this.m_BorderHeight = ConsoleWindow.Constants.ErrorStyle.border.top + ConsoleWindow.Constants.ErrorStyle.border.bottom;
        this.UpdateListView();
      }
      GUILayout.BeginHorizontal(ConsoleWindow.Constants.Toolbar, new GUILayoutOption[0]);
      if (GUILayout.Button(ConsoleWindow.Constants.ClearLabel, ConsoleWindow.Constants.MiniButton, new GUILayoutOption[0]))
      {
        LogEntries.Clear();
        GUIUtility.keyboardControl = 0;
      }
      int count = LogEntries.GetCount();
      if (this.m_ListView.totalRows != count && this.m_ListView.totalRows > 0 && (double) this.m_ListView.scrollPos.y >= (double) (this.m_ListView.rowHeight * this.m_ListView.totalRows - this.ms_LVHeight))
        this.m_ListView.scrollPos.y = (float) (count * this.RowHeight - this.ms_LVHeight);
      EditorGUILayout.Space();
      bool flag1 = ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.Collapse);
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.Collapse, GUILayout.Toggle(flag1, ConsoleWindow.Constants.CollapseLabel, ConsoleWindow.Constants.MiniButtonLeft, new GUILayoutOption[0]));
      if (flag1 != ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.Collapse))
      {
        this.m_ListView.row = -1;
        this.m_ListView.scrollPos.y = (float) (LogEntries.GetCount() * this.RowHeight);
      }
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.ClearOnPlay, GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.ClearOnPlay), ConsoleWindow.Constants.ClearOnPlayLabel, ConsoleWindow.Constants.MiniButtonMiddle, new GUILayoutOption[0]));
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.ErrorPause, GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.ErrorPause), ConsoleWindow.Constants.ErrorPauseLabel, ConsoleWindow.Constants.MiniButtonRight, new GUILayoutOption[0]));
      this.m_AttachProfilerUI.OnGUILayout((EditorWindow) this);
      EditorGUILayout.Space();
      if (this.m_DevBuild)
      {
        GUILayout.FlexibleSpace();
        ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.StopForAssert, GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.StopForAssert), ConsoleWindow.Constants.StopForAssertLabel, ConsoleWindow.Constants.MiniButtonLeft, new GUILayoutOption[0]));
        ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.StopForError, GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.StopForError), ConsoleWindow.Constants.StopForErrorLabel, ConsoleWindow.Constants.MiniButtonRight, new GUILayoutOption[0]));
      }
      GUILayout.FlexibleSpace();
      int errorCount = 0;
      int warningCount = 0;
      int logCount = 0;
      LogEntries.GetCountsByType(ref errorCount, ref warningCount, ref logCount);
      EditorGUI.BeginChangeCheck();
      bool val1 = GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.LogLevelLog), new GUIContent(logCount > 999 ? "999+" : logCount.ToString(), logCount <= 0 ? (Texture) ConsoleWindow.iconInfoMono : (Texture) ConsoleWindow.iconInfoSmall), ConsoleWindow.Constants.MiniButtonRight, new GUILayoutOption[0]);
      bool val2 = GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.LogLevelWarning), new GUIContent(warningCount > 999 ? "999+" : warningCount.ToString(), warningCount <= 0 ? (Texture) ConsoleWindow.iconWarnMono : (Texture) ConsoleWindow.iconWarnSmall), ConsoleWindow.Constants.MiniButtonMiddle, new GUILayoutOption[0]);
      bool val3 = GUILayout.Toggle(ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.LogLevelError), new GUIContent(errorCount > 999 ? "999+" : errorCount.ToString(), errorCount <= 0 ? (Texture) ConsoleWindow.iconErrorMono : (Texture) ConsoleWindow.iconErrorSmall), ConsoleWindow.Constants.MiniButtonLeft, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.SetActiveEntry((LogEntry) null);
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.LogLevelLog, val1);
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.LogLevelWarning, val2);
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.LogLevelError, val3);
      GUILayout.EndHorizontal();
      this.m_ListView.totalRows = LogEntries.StartGettingEntries();
      SplitterGUILayout.BeginVerticalSplit(this.spl);
      int rowHeight = this.RowHeight;
      EditorGUIUtility.SetIconSize(new Vector2((float) rowHeight, (float) rowHeight));
      GUIContent content = new GUIContent();
      int controlId = GUIUtility.GetControlID(FocusType.Native);
      try
      {
        bool flag2 = false;
        bool flag3 = ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.Collapse);
        IEnumerator enumerator = ListViewGUI.ListView(this.m_ListView, ConsoleWindow.Constants.Box, new GUILayoutOption[0]).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            ListViewElement current2 = (ListViewElement) enumerator.Current;
            if (current1.type == EventType.MouseDown && current1.button == 0 && current2.position.Contains(current1.mousePosition))
            {
              if (current1.clickCount == 2)
                LogEntries.RowGotDoubleClicked(this.m_ListView.row);
              flag2 = true;
            }
            if (current1.type == EventType.Repaint)
            {
              int mask = 0;
              string outString = (string) null;
              LogEntries.GetLinesAndModeFromEntryInternal(current2.row, ConsoleWindow.Constants.LogStyleLineCount, ref mask, ref outString);
              (current2.row % 2 != 0 ? ConsoleWindow.Constants.EvenBackground : ConsoleWindow.Constants.OddBackground).Draw(current2.position, false, false, this.m_ListView.row == current2.row, false);
              ConsoleWindow.GetStyleForErrorMode(mask, true, ConsoleWindow.Constants.LogStyleLineCount == 1).Draw(current2.position, false, false, this.m_ListView.row == current2.row, false);
              content.text = outString;
              ConsoleWindow.GetStyleForErrorMode(mask, false, ConsoleWindow.Constants.LogStyleLineCount == 1).Draw(current2.position, content, controlId, this.m_ListView.row == current2.row);
              if (flag3)
              {
                Rect position = current2.position;
                content.text = LogEntries.GetEntryCount(current2.row).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                Vector2 vector2 = ConsoleWindow.Constants.CountBadge.CalcSize(content);
                position.xMin = position.xMax - vector2.x;
                position.yMin += (float) (((double) position.yMax - (double) position.yMin - (double) vector2.y) * 0.5);
                position.x -= 5f;
                GUI.Label(position, content, ConsoleWindow.Constants.CountBadge);
              }
            }
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
        if (flag2 && (double) this.m_ListView.scrollPos.y >= (double) (this.m_ListView.rowHeight * this.m_ListView.totalRows - this.ms_LVHeight))
          this.m_ListView.scrollPos.y = (float) (this.m_ListView.rowHeight * this.m_ListView.totalRows - this.ms_LVHeight - 1);
        if (this.m_ListView.totalRows == 0 || this.m_ListView.row >= this.m_ListView.totalRows || this.m_ListView.row < 0)
        {
          if (this.m_ActiveText.Length != 0)
            this.SetActiveEntry((LogEntry) null);
        }
        else
        {
          LogEntry logEntry = new LogEntry();
          LogEntries.GetEntryInternal(this.m_ListView.row, logEntry);
          this.SetActiveEntry(logEntry);
          LogEntries.GetEntryInternal(this.m_ListView.row, logEntry);
          if (this.m_ListView.selectionChanged || !this.m_ActiveText.Equals(logEntry.condition))
            this.SetActiveEntry(logEntry);
        }
        if (GUIUtility.keyboardControl == this.m_ListView.ID && current1.type == EventType.KeyDown && (current1.keyCode == KeyCode.Return && this.m_ListView.row != 0))
        {
          LogEntries.RowGotDoubleClicked(this.m_ListView.row);
          Event.current.Use();
        }
        if (current1.type != EventType.Layout)
        {
          if (ListViewGUI.ilvState.rectHeight != 1)
            this.ms_LVHeight = ListViewGUI.ilvState.rectHeight;
        }
      }
      finally
      {
        LogEntries.EndGettingEntries();
        EditorGUIUtility.SetIconSize(Vector2.zero);
      }
      this.m_TextScroll = GUILayout.BeginScrollView(this.m_TextScroll, ConsoleWindow.Constants.Box);
      float minHeight = ConsoleWindow.Constants.MessageStyle.CalcHeight(GUIContent.Temp(this.m_ActiveText), this.position.width);
      EditorGUILayout.SelectableLabel(this.m_ActiveText, ConsoleWindow.Constants.MessageStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MinHeight(minHeight));
      GUILayout.EndScrollView();
      SplitterGUILayout.EndVerticalSplit();
      if (current1.type != EventType.ValidateCommand && current1.type != EventType.ExecuteCommand || (!(current1.commandName == "Copy") || !(this.m_ActiveText != string.Empty)))
        return;
      if (current1.type == EventType.ExecuteCommand)
        EditorGUIUtility.systemCopyBuffer = this.m_ActiveText;
      current1.Use();
    }

    public static bool GetConsoleErrorPause()
    {
      return ConsoleWindow.HasFlag(ConsoleWindow.ConsoleFlags.ErrorPause);
    }

    public static void SetConsoleErrorPause(bool enabled)
    {
      ConsoleWindow.SetFlag(ConsoleWindow.ConsoleFlags.ErrorPause, enabled);
    }

    public void ToggleLogStackTraces(object userData)
    {
      ConsoleWindow.StackTraceLogTypeData traceLogTypeData = (ConsoleWindow.StackTraceLogTypeData) userData;
      PlayerSettings.SetStackTraceLogType(traceLogTypeData.logType, traceLogTypeData.stackTraceLogType);
    }

    public void ToggleLogStackTracesForAll(object userData)
    {
      IEnumerator enumerator = Enum.GetValues(typeof (LogType)).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          PlayerSettings.SetStackTraceLogType((LogType) enumerator.Current, (StackTraceLogType) userData);
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    public void AddItemsToMenu(GenericMenu menu)
    {
      if (Application.platform == RuntimePlatform.OSXEditor)
      {
        GenericMenu genericMenu = menu;
        GUIContent content = new GUIContent("Open Player Log");
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        if (ConsoleWindow.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConsoleWindow.\u003C\u003Ef__mg\u0024cache0 = new GenericMenu.MenuFunction(InternalEditorUtility.OpenPlayerConsole);
        }
        // ISSUE: reference to a compiler-generated field
        GenericMenu.MenuFunction fMgCache0 = ConsoleWindow.\u003C\u003Ef__mg\u0024cache0;
        genericMenu.AddItem(content, num != 0, fMgCache0);
      }
      GenericMenu genericMenu1 = menu;
      GUIContent content1 = new GUIContent("Open Editor Log");
      int num1 = 0;
      // ISSUE: reference to a compiler-generated field
      if (ConsoleWindow.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConsoleWindow.\u003C\u003Ef__mg\u0024cache1 = new GenericMenu.MenuFunction(InternalEditorUtility.OpenEditorConsole);
      }
      // ISSUE: reference to a compiler-generated field
      GenericMenu.MenuFunction fMgCache1 = ConsoleWindow.\u003C\u003Ef__mg\u0024cache1;
      genericMenu1.AddItem(content1, num1 != 0, fMgCache1);
      for (int index = 1; index <= 10; ++index)
        menu.AddItem(new GUIContent(string.Format("Log Entry/{0} Lines", (object) index)), index == ConsoleWindow.Constants.LogStyleLineCount, new GenericMenu.MenuFunction2(this.SetLogLineCount), (object) index);
      this.AddStackTraceLoggingMenu(menu);
    }

    private void SetLogLineCount(object obj)
    {
      int num = (int) obj;
      ConsoleWindow.Constants.LogStyleLineCount = num;
      EditorPrefs.SetInt("ConsoleWindowLogLineCount", num);
      ConsoleWindow.Constants.UpdateLogStyleFixedHeights();
      this.UpdateListView();
    }

    private void AddStackTraceLoggingMenu(GenericMenu menu)
    {
      IEnumerator enumerator1 = Enum.GetValues(typeof (LogType)).GetEnumerator();
      try
      {
        while (enumerator1.MoveNext())
        {
          LogType current1 = (LogType) enumerator1.Current;
          IEnumerator enumerator2 = Enum.GetValues(typeof (StackTraceLogType)).GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              StackTraceLogType current2 = (StackTraceLogType) enumerator2.Current;
              ConsoleWindow.StackTraceLogTypeData traceLogTypeData;
              traceLogTypeData.logType = current1;
              traceLogTypeData.stackTraceLogType = current2;
              menu.AddItem(new GUIContent("Stack Trace Logging/" + (object) current1 + "/" + (object) current2), (PlayerSettings.GetStackTraceLogType(current1) == current2 ? 1 : 0) != 0, new GenericMenu.MenuFunction2(this.ToggleLogStackTraces), (object) traceLogTypeData);
            }
          }
          finally
          {
            IDisposable disposable;
            if ((disposable = enumerator2 as IDisposable) != null)
              disposable.Dispose();
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator1 as IDisposable) != null)
          disposable.Dispose();
      }
      int num = (int) PlayerSettings.GetStackTraceLogType(LogType.Log);
      IEnumerator enumerator3 = Enum.GetValues(typeof (LogType)).GetEnumerator();
      try
      {
        while (enumerator3.MoveNext())
        {
          if (PlayerSettings.GetStackTraceLogType((LogType) enumerator3.Current) != (StackTraceLogType) num)
          {
            num = -1;
            break;
          }
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator3 as IDisposable) != null)
          disposable.Dispose();
      }
      IEnumerator enumerator4 = Enum.GetValues(typeof (StackTraceLogType)).GetEnumerator();
      try
      {
        while (enumerator4.MoveNext())
        {
          StackTraceLogType current = (StackTraceLogType) enumerator4.Current;
          menu.AddItem(new GUIContent("Stack Trace Logging/All/" + (object) current), (StackTraceLogType) num == current, new GenericMenu.MenuFunction2(this.ToggleLogStackTracesForAll), (object) current);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator4 as IDisposable) != null)
          disposable.Dispose();
      }
    }

    internal class Constants
    {
      public static readonly string ClearLabel = L10n.Tr("Clear");
      public static readonly string ClearOnPlayLabel = L10n.Tr("Clear on Play");
      public static readonly string ErrorPauseLabel = L10n.Tr("Error Pause");
      public static readonly string CollapseLabel = L10n.Tr("Collapse");
      public static readonly string StopForAssertLabel = L10n.Tr("Stop for Assert");
      public static readonly string StopForErrorLabel = L10n.Tr("Stop for Error");
      private static bool ms_Loaded;
      public static GUIStyle Box;
      public static GUIStyle Button;
      public static GUIStyle MiniButton;
      public static GUIStyle MiniButtonLeft;
      public static GUIStyle MiniButtonMiddle;
      public static GUIStyle MiniButtonRight;
      public static GUIStyle LogStyle;
      public static GUIStyle WarningStyle;
      public static GUIStyle ErrorStyle;
      public static GUIStyle IconLogStyle;
      public static GUIStyle IconWarningStyle;
      public static GUIStyle IconErrorStyle;
      public static GUIStyle EvenBackground;
      public static GUIStyle OddBackground;
      public static GUIStyle MessageStyle;
      public static GUIStyle StatusError;
      public static GUIStyle StatusWarn;
      public static GUIStyle StatusLog;
      public static GUIStyle Toolbar;
      public static GUIStyle CountBadge;
      public static GUIStyle LogSmallStyle;
      public static GUIStyle WarningSmallStyle;
      public static GUIStyle ErrorSmallStyle;
      public static GUIStyle IconLogSmallStyle;
      public static GUIStyle IconWarningSmallStyle;
      public static GUIStyle IconErrorSmallStyle;

      public static int LogStyleLineCount { get; set; }

      public static void Init()
      {
        if (ConsoleWindow.Constants.ms_Loaded)
          return;
        ConsoleWindow.Constants.ms_Loaded = true;
        ConsoleWindow.Constants.Box = new GUIStyle((GUIStyle) "CN Box");
        ConsoleWindow.Constants.Button = new GUIStyle((GUIStyle) "Button");
        ConsoleWindow.Constants.MiniButton = new GUIStyle((GUIStyle) "ToolbarButton");
        ConsoleWindow.Constants.MiniButtonLeft = new GUIStyle((GUIStyle) "ToolbarButton");
        ConsoleWindow.Constants.MiniButtonMiddle = new GUIStyle((GUIStyle) "ToolbarButton");
        ConsoleWindow.Constants.MiniButtonRight = new GUIStyle((GUIStyle) "ToolbarButton");
        ConsoleWindow.Constants.Toolbar = new GUIStyle((GUIStyle) "Toolbar");
        ConsoleWindow.Constants.LogStyle = new GUIStyle((GUIStyle) "CN EntryInfo");
        ConsoleWindow.Constants.LogSmallStyle = new GUIStyle((GUIStyle) "CN EntryInfoSmall");
        ConsoleWindow.Constants.WarningStyle = new GUIStyle((GUIStyle) "CN EntryWarn");
        ConsoleWindow.Constants.WarningSmallStyle = new GUIStyle((GUIStyle) "CN EntryWarnSmall");
        ConsoleWindow.Constants.ErrorStyle = new GUIStyle((GUIStyle) "CN EntryError");
        ConsoleWindow.Constants.ErrorSmallStyle = new GUIStyle((GUIStyle) "CN EntryErrorSmall");
        ConsoleWindow.Constants.IconLogStyle = new GUIStyle((GUIStyle) "CN EntryInfoIcon");
        ConsoleWindow.Constants.IconLogSmallStyle = new GUIStyle((GUIStyle) "CN EntryInfoIconSmall");
        ConsoleWindow.Constants.IconWarningStyle = new GUIStyle((GUIStyle) "CN EntryWarnIcon");
        ConsoleWindow.Constants.IconWarningSmallStyle = new GUIStyle((GUIStyle) "CN EntryWarnIconSmall");
        ConsoleWindow.Constants.IconErrorStyle = new GUIStyle((GUIStyle) "CN EntryErrorIcon");
        ConsoleWindow.Constants.IconErrorSmallStyle = new GUIStyle((GUIStyle) "CN EntryErrorIconSmall");
        ConsoleWindow.Constants.EvenBackground = new GUIStyle((GUIStyle) "CN EntryBackEven");
        ConsoleWindow.Constants.OddBackground = new GUIStyle((GUIStyle) "CN EntryBackodd");
        ConsoleWindow.Constants.MessageStyle = new GUIStyle((GUIStyle) "CN Message");
        ConsoleWindow.Constants.StatusError = new GUIStyle((GUIStyle) "CN StatusError");
        ConsoleWindow.Constants.StatusWarn = new GUIStyle((GUIStyle) "CN StatusWarn");
        ConsoleWindow.Constants.StatusLog = new GUIStyle((GUIStyle) "CN StatusInfo");
        ConsoleWindow.Constants.CountBadge = new GUIStyle((GUIStyle) "CN CountBadge");
        ConsoleWindow.Constants.LogStyleLineCount = EditorPrefs.GetInt("ConsoleWindowLogLineCount", 2);
        ConsoleWindow.Constants.UpdateLogStyleFixedHeights();
      }

      public static void UpdateLogStyleFixedHeights()
      {
        ConsoleWindow.Constants.ErrorStyle.fixedHeight = (float) ConsoleWindow.Constants.LogStyleLineCount * ConsoleWindow.Constants.ErrorStyle.lineHeight + (float) ConsoleWindow.Constants.ErrorStyle.border.top;
        ConsoleWindow.Constants.WarningStyle.fixedHeight = (float) ConsoleWindow.Constants.LogStyleLineCount * ConsoleWindow.Constants.WarningStyle.lineHeight + (float) ConsoleWindow.Constants.WarningStyle.border.top;
        ConsoleWindow.Constants.LogStyle.fixedHeight = (float) ConsoleWindow.Constants.LogStyleLineCount * ConsoleWindow.Constants.LogStyle.lineHeight + (float) ConsoleWindow.Constants.LogStyle.border.top;
      }
    }

    private class ConsoleAttachProfilerUI : AttachProfilerUI
    {
      private List<string> additionalMenuItems = (List<string>) null;

      protected void SelectClick(object userData, string[] options, int selected)
      {
        switch (selected)
        {
          case 0:
            ScriptableSingleton<PlayerConnectionLogReceiver>.instance.State = ScriptableSingleton<PlayerConnectionLogReceiver>.instance.State == PlayerConnectionLogReceiver.ConnectionState.Disconnected ? PlayerConnectionLogReceiver.ConnectionState.CleanLog : PlayerConnectionLogReceiver.ConnectionState.Disconnected;
            break;
          case 1:
            ScriptableSingleton<PlayerConnectionLogReceiver>.instance.State = ScriptableSingleton<PlayerConnectionLogReceiver>.instance.State != PlayerConnectionLogReceiver.ConnectionState.CleanLog ? PlayerConnectionLogReceiver.ConnectionState.CleanLog : PlayerConnectionLogReceiver.ConnectionState.FullLog;
            break;
          default:
            if (selected < this.additionalMenuItems.Count)
              break;
            this.SelectProfilerClick(userData, options, selected - this.additionalMenuItems.Count);
            break;
        }
      }

      protected override void OnGUIMenu(Rect connectRect, List<ProfilerChoise> profilers)
      {
        if (this.additionalMenuItems == null)
        {
          this.additionalMenuItems = new List<string>();
          this.additionalMenuItems.Add("Player Logging");
          if (Unsupported.IsDeveloperBuild())
            this.additionalMenuItems.Add("Full Log (Development Editor only)");
          this.additionalMenuItems.Add("");
        }
        IEnumerable<string> source = this.additionalMenuItems.Concat<string>(profilers.Select<ProfilerChoise, string>((Func<ProfilerChoise, string>) (p => p.Name)));
        List<bool> boolList = new List<bool>();
        boolList.Add(true);
        List<int> intList = new List<int>();
        if (ScriptableSingleton<PlayerConnectionLogReceiver>.instance.State != PlayerConnectionLogReceiver.ConnectionState.Disconnected)
        {
          intList.Add(0);
          if (Unsupported.IsDeveloperBuild())
          {
            if (ScriptableSingleton<PlayerConnectionLogReceiver>.instance.State == PlayerConnectionLogReceiver.ConnectionState.FullLog)
              intList.Add(1);
            boolList.Add(true);
          }
          boolList.Add(true);
          boolList.AddRange(profilers.Select<ProfilerChoise, bool>((Func<ProfilerChoise, bool>) (p => p.Enabled)));
        }
        else
          boolList.AddRange((IEnumerable<bool>) new bool[source.Count<string>() - 1]);
        int index = profilers.FindIndex((Predicate<ProfilerChoise>) (p => p.IsSelected()));
        if (index != -1)
          intList.Add(index + this.additionalMenuItems.Count);
        bool[] separator = new bool[boolList.Count];
        separator[this.additionalMenuItems.Count - 1] = true;
        EditorUtility.DisplayCustomMenuWithSeparators(connectRect, source.ToArray<string>(), boolList.ToArray(), separator, intList.ToArray(), new EditorUtility.SelectMenuItemFunction(this.SelectClick), (object) profilers);
      }

      private enum MenuItemIndex
      {
        PlayerLogging,
        FullLog,
      }
    }

    private enum Mode
    {
      Error = 1,
      Assert = 2,
      Log = 4,
      Fatal = 16, // 0x00000010
      DontPreprocessCondition = 32, // 0x00000020
      AssetImportError = 64, // 0x00000040
      AssetImportWarning = 128, // 0x00000080
      ScriptingError = 256, // 0x00000100
      ScriptingWarning = 512, // 0x00000200
      ScriptingLog = 1024, // 0x00000400
      ScriptCompileError = 2048, // 0x00000800
      ScriptCompileWarning = 4096, // 0x00001000
      StickyError = 8192, // 0x00002000
      MayIgnoreLineNumber = 16384, // 0x00004000
      ReportBug = 32768, // 0x00008000
      DisplayPreviousErrorInStatusBar = 65536, // 0x00010000
      ScriptingException = 131072, // 0x00020000
      DontExtractStacktrace = 262144, // 0x00040000
      ShouldClearOnPlay = 524288, // 0x00080000
      GraphCompileError = 1048576, // 0x00100000
      ScriptingAssertion = 2097152, // 0x00200000
    }

    private enum ConsoleFlags
    {
      Collapse = 1,
      ClearOnPlay = 2,
      ErrorPause = 4,
      Verbose = 8,
      StopForAssert = 16, // 0x00000010
      StopForError = 32, // 0x00000020
      Autoscroll = 64, // 0x00000040
      LogLevelLog = 128, // 0x00000080
      LogLevelWarning = 256, // 0x00000100
      LogLevelError = 512, // 0x00000200
    }

    public struct StackTraceLogTypeData
    {
      public LogType logType;
      public StackTraceLogType stackTraceLogType;
    }
  }
}
