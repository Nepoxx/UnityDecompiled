// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.HolographicEmulationWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace UnityEditorInternal.VR
{
  public class HolographicEmulationWindow : EditorWindow
  {
    private static int s_MaxHistoryLength = 4;
    private static GUIContent s_ConnectionStatusText = new GUIContent("Connection Status");
    private static GUIContent s_EmulationModeText = new GUIContent("Emulation Mode");
    private static GUIContent s_RoomText = new GUIContent("Room");
    private static GUIContent s_HandText = new GUIContent("Gesture Hand");
    private static GUIContent s_RemoteMachineText = new GUIContent("Remote Machine");
    private static GUIContent s_EnableVideoText = new GUIContent("Enable Video");
    private static GUIContent s_EnableAudioText = new GUIContent("Enable Audio");
    private static GUIContent s_MaxBitrateText = new GUIContent("Max Bitrate (kbps)");
    private static GUIContent s_ConnectionButtonConnectText = new GUIContent("Connect");
    private static GUIContent s_ConnectionButtonDisconnectText = new GUIContent("Disconnect");
    private static GUIContent s_ConnectionStateDisconnectedText = new GUIContent("Disconnected");
    private static GUIContent s_ConnectionStateConnectingText = new GUIContent("Connecting");
    private static GUIContent s_ConnectionStateConnectedText = new GUIContent("Connected");
    private static Texture2D s_ConnectedTexture = (Texture2D) null;
    private static Texture2D s_ConnectingTexture = (Texture2D) null;
    private static Texture2D s_DisconnectedTexture = (Texture2D) null;
    private static GUIContent[] s_ModeStrings = new GUIContent[3]{ new GUIContent("None"), new GUIContent("Remote to Device"), new GUIContent("Simulate in Editor") };
    private static GUIContent[] s_RoomStrings = new GUIContent[6]{ new GUIContent("None"), new GUIContent("DefaultRoom"), new GUIContent("Bedroom1"), new GUIContent("Bedroom2"), new GUIContent("GreatRoom"), new GUIContent("LivingRoom") };
    private static GUIContent[] s_HandStrings = new GUIContent[2]{ new GUIContent("Left Hand"), new GUIContent("Right Hand") };
    private bool m_InPlayMode = false;
    private bool m_OperatingSystemChecked = false;
    private bool m_OperatingSystemValid = false;
    private HolographicStreamerConnectionState m_LastConnectionState = HolographicStreamerConnectionState.Disconnected;
    [SerializeField]
    private EmulationMode m_Mode = EmulationMode.None;
    [SerializeField]
    private int m_RoomIndex = 0;
    [SerializeField]
    private GestureHand m_Hand = GestureHand.Right;
    [SerializeField]
    private string m_RemoteMachineAddress = "";
    [SerializeField]
    private bool m_EnableVideo = true;
    [SerializeField]
    private bool m_EnableAudio = true;
    [SerializeField]
    private int m_MaxBitrateKbps = 99999;
    private string[] m_RemoteMachineHistory;

    public EmulationMode emulationMode
    {
      get
      {
        return this.m_Mode;
      }
      set
      {
        this.m_Mode = value;
        this.Repaint();
      }
    }

    internal static void Init()
    {
      EditorWindow.GetWindow<HolographicEmulationWindow>(false);
    }

    private bool RemoteMachineNameSpecified
    {
      get
      {
        return !string.IsNullOrEmpty(this.m_RemoteMachineAddress);
      }
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("Holographic");
      EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
      this.m_InPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
      this.m_RemoteMachineHistory = EditorPrefs.GetString("HolographicRemoting.RemoteMachineHistory").Split(',');
    }

    private void OnDisable()
    {
      EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
    }

    private void LoadCurrentRoom()
    {
      if (this.m_RoomIndex == 0)
        return;
      HolographicEmulation.LoadRoom(EditorApplication.applicationContentsPath + "/UnityExtensions/Unity/VR/HolographicSimulation/Rooms/" + HolographicEmulationWindow.s_RoomStrings[this.m_RoomIndex].text + ".xef");
    }

    private void InitializeSimulation()
    {
      this.Disconnect();
      HolographicEmulation.Initialize();
      this.LoadCurrentRoom();
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
      if (!this.IsWindowsMixedRealityCurrentTarget())
        return;
      bool inPlayMode = this.m_InPlayMode;
      this.m_InPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
      if (this.m_InPlayMode && !inPlayMode)
      {
        HolographicEmulation.SetEmulationMode(this.m_Mode);
        switch (this.m_Mode)
        {
          case EmulationMode.Simulated:
            this.InitializeSimulation();
            break;
        }
      }
      else
      {
        if (this.m_InPlayMode || !inPlayMode)
          return;
        switch (this.m_Mode)
        {
          case EmulationMode.Simulated:
            HolographicEmulation.Shutdown();
            break;
        }
      }
    }

    private void Connect()
    {
      PerceptionRemotingPlugin.SetVideoEncodingParameters(this.m_MaxBitrateKbps);
      PerceptionRemotingPlugin.SetEnableVideo(this.m_EnableVideo);
      PerceptionRemotingPlugin.SetEnableAudio(this.m_EnableAudio);
      PerceptionRemotingPlugin.Connect(this.m_RemoteMachineAddress);
    }

    private void Disconnect()
    {
      if (PerceptionRemotingPlugin.GetConnectionState() == HolographicStreamerConnectionState.Disconnected)
        return;
      PerceptionRemotingPlugin.Disconnect();
    }

    private bool IsConnectedToRemoteDevice()
    {
      return PerceptionRemotingPlugin.GetConnectionState() == HolographicStreamerConnectionState.Connected;
    }

    private void HandleButtonPress()
    {
      if (EditorApplication.isPlayingOrWillChangePlaymode)
      {
        Debug.LogError((object) "Unable to connect / disconnect remoting while playing.");
      }
      else
      {
        switch (PerceptionRemotingPlugin.GetConnectionState())
        {
          case HolographicStreamerConnectionState.Connecting:
          case HolographicStreamerConnectionState.Connected:
            this.Disconnect();
            break;
          default:
            if (this.RemoteMachineNameSpecified)
            {
              this.Connect();
              break;
            }
            Debug.LogError((object) "Cannot connect without a remote machine address specified");
            break;
        }
      }
    }

    private void UpdateRemoteMachineHistory()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.m_RemoteMachineHistory);
      for (int index = 0; index < this.m_RemoteMachineHistory.Length; ++index)
      {
        if (this.m_RemoteMachineHistory[index].Equals(this.m_RemoteMachineAddress, StringComparison.CurrentCultureIgnoreCase))
        {
          if (index == 0)
            return;
          stringList.RemoveAt(index);
          break;
        }
      }
      stringList.Insert(0, this.m_RemoteMachineAddress);
      if (stringList.Count > HolographicEmulationWindow.s_MaxHistoryLength)
        stringList.RemoveRange(HolographicEmulationWindow.s_MaxHistoryLength, stringList.Count - HolographicEmulationWindow.s_MaxHistoryLength);
      this.m_RemoteMachineHistory = stringList.ToArray();
      EditorPrefs.SetString("HolographicRemoting.RemoteMachineHistory", string.Join(",", this.m_RemoteMachineHistory));
    }

    private void RemotingPreferencesOnGUI()
    {
      EditorGUI.BeginChangeCheck();
      this.m_RemoteMachineAddress = EditorGUILayout.DelayedTextFieldDropDown(HolographicEmulationWindow.s_RemoteMachineText, this.m_RemoteMachineAddress, this.m_RemoteMachineHistory);
      if (EditorGUI.EndChangeCheck())
        this.UpdateRemoteMachineHistory();
      this.m_EnableVideo = EditorGUILayout.Toggle(HolographicEmulationWindow.s_EnableVideoText, this.m_EnableVideo, new GUILayoutOption[0]);
      this.m_EnableAudio = EditorGUILayout.Toggle(HolographicEmulationWindow.s_EnableAudioText, this.m_EnableAudio, new GUILayoutOption[0]);
      this.m_MaxBitrateKbps = EditorGUILayout.IntSlider(HolographicEmulationWindow.s_MaxBitrateText, this.m_MaxBitrateKbps, 1024, 99999, new GUILayoutOption[0]);
    }

    private void ConnectionStateGUI()
    {
      if ((UnityEngine.Object) HolographicEmulationWindow.s_ConnectedTexture == (UnityEngine.Object) null)
      {
        HolographicEmulationWindow.s_ConnectedTexture = EditorGUIUtility.LoadIconRequired("sv_icon_dot3_sml");
        HolographicEmulationWindow.s_ConnectingTexture = EditorGUIUtility.LoadIconRequired("sv_icon_dot4_sml");
        HolographicEmulationWindow.s_DisconnectedTexture = EditorGUIUtility.LoadIconRequired("sv_icon_dot6_sml");
      }
      Texture2D texture2D;
      GUIContent label;
      GUIContent content;
      switch (PerceptionRemotingPlugin.GetConnectionState())
      {
        case HolographicStreamerConnectionState.Connecting:
          texture2D = HolographicEmulationWindow.s_ConnectingTexture;
          label = HolographicEmulationWindow.s_ConnectionStateConnectingText;
          content = HolographicEmulationWindow.s_ConnectionButtonDisconnectText;
          break;
        case HolographicStreamerConnectionState.Connected:
          texture2D = HolographicEmulationWindow.s_ConnectedTexture;
          label = HolographicEmulationWindow.s_ConnectionStateConnectedText;
          content = HolographicEmulationWindow.s_ConnectionButtonDisconnectText;
          break;
        default:
          texture2D = HolographicEmulationWindow.s_DisconnectedTexture;
          label = HolographicEmulationWindow.s_ConnectionStateDisconnectedText;
          content = HolographicEmulationWindow.s_ConnectionButtonConnectText;
          break;
      }
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(HolographicEmulationWindow.s_ConnectionStatusText, (GUIStyle) "Button");
      float singleLineHeight = EditorGUIUtility.singleLineHeight;
      GUI.DrawTexture(GUILayoutUtility.GetRect(singleLineHeight, singleLineHeight, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }), (Texture) texture2D);
      EditorGUILayout.LabelField(label);
      EditorGUILayout.EndHorizontal();
      EditorGUI.BeginDisabledGroup(this.m_InPlayMode);
      bool flag = EditorGUILayout.DropdownButton(content, FocusType.Passive, EditorStyles.miniButton, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      if (!flag)
        return;
      if (EditorGUIUtility.editingTextField)
      {
        EditorGUIUtility.editingTextField = false;
        GUIUtility.keyboardControl = 0;
      }
      EditorApplication.CallDelayed((EditorApplication.CallbackFunction) (() => this.HandleButtonPress()), 0.0f);
    }

    private bool IsWindowsMixedRealityCurrentTarget()
    {
      return VREditor.GetVREnabledOnTargetGroup(BuildTargetGroup.WSA) && Array.IndexOf<string>(XRSettings.supportedDevices, "WindowsMR") >= 0;
    }

    private void DrawRemotingMode()
    {
      EditorGUI.BeginChangeCheck();
      this.m_Mode = (EmulationMode) EditorGUILayout.Popup(HolographicEmulationWindow.s_EmulationModeText, (int) this.m_Mode, HolographicEmulationWindow.s_ModeStrings, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck() || this.m_Mode == EmulationMode.RemoteDevice)
        return;
      this.Disconnect();
    }

    private bool CheckOperatingSystem()
    {
      if (!this.m_OperatingSystemChecked)
      {
        this.m_OperatingSystemValid = Environment.OSVersion.Version.Build >= 14318;
        this.m_OperatingSystemChecked = true;
      }
      return this.m_OperatingSystemValid;
    }

    private void OnGUI()
    {
      if (!this.CheckOperatingSystem())
        EditorGUILayout.HelpBox("You must be running Windows build 14318 or later to use Holographic Simulation or Remoting.", MessageType.Warning);
      else if (!this.IsWindowsMixedRealityCurrentTarget())
      {
        EditorGUILayout.HelpBox("You must enable Virtual Reality support in settings and add Windows Mixed Reality to the devices to use Holographic Emulation.", MessageType.Warning);
      }
      else
      {
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(this.m_InPlayMode);
        this.DrawRemotingMode();
        EditorGUI.EndDisabledGroup();
        switch (this.m_Mode)
        {
          case EmulationMode.RemoteDevice:
            EditorGUI.BeginDisabledGroup(this.IsConnectedToRemoteDevice());
            this.RemotingPreferencesOnGUI();
            EditorGUI.EndDisabledGroup();
            this.ConnectionStateGUI();
            break;
          case EmulationMode.Simulated:
            EditorGUI.BeginChangeCheck();
            this.m_RoomIndex = EditorGUILayout.Popup(HolographicEmulationWindow.s_RoomText, this.m_RoomIndex, HolographicEmulationWindow.s_RoomStrings, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck() && this.m_InPlayMode)
              this.LoadCurrentRoom();
            EditorGUI.BeginChangeCheck();
            this.m_Hand = (GestureHand) EditorGUILayout.Popup(HolographicEmulationWindow.s_HandText, (int) this.m_Hand, HolographicEmulationWindow.s_HandStrings, new GUILayoutOption[0]);
            if (!EditorGUI.EndChangeCheck())
              break;
            HolographicEmulation.SetGestureHand(this.m_Hand);
            break;
        }
      }
    }

    private void Update()
    {
      switch (this.m_Mode)
      {
        case EmulationMode.RemoteDevice:
          HolographicStreamerConnectionState connectionState = PerceptionRemotingPlugin.GetConnectionState();
          if (connectionState != this.m_LastConnectionState)
            this.Repaint();
          HolographicStreamerConnectionFailureReason connectionFailureReason = PerceptionRemotingPlugin.CheckForDisconnect();
          switch (connectionFailureReason)
          {
            case HolographicStreamerConnectionFailureReason.None:
              this.m_LastConnectionState = connectionState;
              return;
            case HolographicStreamerConnectionFailureReason.Unreachable:
            case HolographicStreamerConnectionFailureReason.ConnectionLost:
              Debug.LogWarning((object) ("Disconnected with failure reason " + (object) connectionFailureReason + ", attempting to reconnect."));
              this.Connect();
              goto case HolographicStreamerConnectionFailureReason.None;
            default:
              Debug.LogError((object) ("Disconnected with error " + (object) connectionFailureReason));
              goto case HolographicStreamerConnectionFailureReason.None;
          }
        case EmulationMode.Simulated:
          HolographicEmulation.SetGestureHand(this.m_Hand);
          break;
      }
    }
  }
}
