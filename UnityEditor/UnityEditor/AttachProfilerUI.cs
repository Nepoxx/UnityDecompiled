// Decompiled with JetBrains decompiler
// Type: UnityEditor.AttachProfilerUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Hardware;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AttachProfilerUI
  {
    private static string kEnterIPText = "<Enter IP>";
    private static GUIContent ms_NotificationMessage;
    private const int PLAYER_DIRECT_IP_CONNECT_GUID = 65261;
    private const int PLAYER_DIRECT_URL_CONNECT_GUID = 65262;

    protected void SelectProfilerClick(object userData, string[] options, int selected)
    {
      List<ProfilerChoise> source = (List<ProfilerChoise>) userData;
      if (selected >= source.Count<ProfilerChoise>())
        return;
      source[selected].ConnectTo();
    }

    public bool IsEditor()
    {
      return ProfilerDriver.IsConnectionEditor();
    }

    public string GetConnectedProfiler()
    {
      return ProfilerDriver.GetConnectionIdentifier(ProfilerDriver.connectedProfiler);
    }

    public static void DirectIPConnect(string ip)
    {
      ConsoleWindow.ShowConsoleWindow(true);
      AttachProfilerUI.ms_NotificationMessage = new GUIContent("Connecting to player...(this can take a while)");
      ProfilerDriver.DirectIPConnect(ip);
      AttachProfilerUI.ms_NotificationMessage = (GUIContent) null;
    }

    public static void DirectURLConnect(string url)
    {
      ConsoleWindow.ShowConsoleWindow(true);
      AttachProfilerUI.ms_NotificationMessage = new GUIContent("Connecting to player...(this can take a while)");
      ProfilerDriver.DirectURLConnect(url);
      AttachProfilerUI.ms_NotificationMessage = (GUIContent) null;
    }

    public void OnGUILayout(EditorWindow window)
    {
      this.OnGUI();
      if (AttachProfilerUI.ms_NotificationMessage != null)
        window.ShowNotification(AttachProfilerUI.ms_NotificationMessage);
      else
        window.RemoveNotification();
    }

    private static void AddLastIPProfiler(List<ProfilerChoise> profilers)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AttachProfilerUI.\u003CAddLastIPProfiler\u003Ec__AnonStorey0 profilerCAnonStorey0 = new AttachProfilerUI.\u003CAddLastIPProfiler\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      profilerCAnonStorey0.lastIP = ProfilerIPWindow.GetLastIPString();
      // ISSUE: reference to a compiler-generated field
      if (string.IsNullOrEmpty(profilerCAnonStorey0.lastIP))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      profilers.Add(new ProfilerChoise()
      {
        Name = profilerCAnonStorey0.lastIP,
        Enabled = true,
        IsSelected = (Func<bool>) (() => ProfilerDriver.connectedProfiler == 65261),
        ConnectTo = new Action(profilerCAnonStorey0.\u003C\u003Em__0)
      });
    }

    private static void AddPlayerProfilers(List<ProfilerChoise> profilers)
    {
      foreach (int availableProfiler in ProfilerDriver.GetAvailableProfilers())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AttachProfilerUI.\u003CAddPlayerProfilers\u003Ec__AnonStorey1 profilersCAnonStorey1 = new AttachProfilerUI.\u003CAddPlayerProfilers\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        profilersCAnonStorey1.guid = availableProfiler;
        // ISSUE: reference to a compiler-generated field
        string str = ProfilerDriver.GetConnectionIdentifier(profilersCAnonStorey1.guid);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = ProfilerDriver.IsIdentifierOnLocalhost(profilersCAnonStorey1.guid) && str.Contains("MetroPlayerX");
        // ISSUE: reference to a compiler-generated field
        bool flag2 = !flag1 && ProfilerDriver.IsIdentifierConnectable(profilersCAnonStorey1.guid);
        if (!flag2)
          str = !flag1 ? str + " (Version mismatch)" : str + " (Localhost prohibited)";
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        profilers.Add(new ProfilerChoise()
        {
          Name = str,
          Enabled = flag2,
          IsSelected = new Func<bool>(profilersCAnonStorey1.\u003C\u003Em__0),
          ConnectTo = new Action(profilersCAnonStorey1.\u003C\u003Em__1)
        });
      }
    }

    private static void AddDeviceProfilers(List<ProfilerChoise> profilers)
    {
      foreach (DevDevice device in DevDeviceList.GetDevices())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AttachProfilerUI.\u003CAddDeviceProfilers\u003Ec__AnonStorey2 profilersCAnonStorey2 = new AttachProfilerUI.\u003CAddDeviceProfilers\u003Ec__AnonStorey2();
        bool flag = (device.features & DevDeviceFeatures.PlayerConnection) != DevDeviceFeatures.None;
        if (device.isConnected && flag)
        {
          // ISSUE: reference to a compiler-generated field
          profilersCAnonStorey2.url = "device://" + device.id;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          profilers.Add(new ProfilerChoise()
          {
            Name = device.name,
            Enabled = true,
            IsSelected = new Func<bool>(profilersCAnonStorey2.\u003C\u003Em__0),
            ConnectTo = new Action(profilersCAnonStorey2.\u003C\u003Em__1)
          });
        }
      }
    }

    private void AddEnterIPProfiler(List<ProfilerChoise> profilers, Rect buttonScreenRect)
    {
      profilers.Add(new ProfilerChoise()
      {
        Name = AttachProfilerUI.kEnterIPText,
        Enabled = true,
        IsSelected = (Func<bool>) (() => false),
        ConnectTo = (Action) (() => ProfilerIPWindow.Show(buttonScreenRect))
      });
    }

    public void OnGUI()
    {
      GUIContent content = EditorGUIUtility.TextContent(this.GetConnectedProfiler() + "|Specifies the target player for receiving profiler and log data.");
      Vector2 vector2 = EditorStyles.toolbarDropDown.CalcSize(content);
      Rect rect = GUILayoutUtility.GetRect(vector2.x, vector2.y);
      if (!EditorGUI.DropdownButton(rect, content, FocusType.Passive, EditorStyles.toolbarDropDown))
        return;
      List<ProfilerChoise> profilerChoiseList = new List<ProfilerChoise>();
      profilerChoiseList.Clear();
      AttachProfilerUI.AddPlayerProfilers(profilerChoiseList);
      AttachProfilerUI.AddDeviceProfilers(profilerChoiseList);
      AttachProfilerUI.AddLastIPProfiler(profilerChoiseList);
      if (!ProfilerDriver.IsConnectionEditor() && !profilerChoiseList.Any<ProfilerChoise>((Func<ProfilerChoise, bool>) (p => p.IsSelected())))
        profilerChoiseList.Add(new ProfilerChoise()
        {
          Name = "(Autoconnected Player)",
          Enabled = false,
          IsSelected = (Func<bool>) (() => true),
          ConnectTo = (Action) (() => {})
        });
      this.AddEnterIPProfiler(profilerChoiseList, GUIUtility.GUIToScreenRect(rect));
      this.OnGUIMenu(rect, profilerChoiseList);
    }

    protected virtual void OnGUIMenu(Rect connectRect, List<ProfilerChoise> profilers)
    {
      string[] array1 = profilers.Select<ProfilerChoise, string>((Func<ProfilerChoise, string>) (p => p.Name)).ToArray<string>();
      bool[] array2 = profilers.Select<ProfilerChoise, bool>((Func<ProfilerChoise, bool>) (p => p.Enabled)).ToArray<bool>();
      int index = profilers.FindIndex((Predicate<ProfilerChoise>) (p => p.IsSelected()));
      int[] selected;
      if (index == -1)
        selected = new int[0];
      else
        selected = new int[1]{ index };
      EditorUtility.DisplayCustomMenu(connectRect, array1, array2, selected, new EditorUtility.SelectMenuItemFunction(this.SelectProfilerClick), (object) profilers);
    }
  }
}
