// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorSettingsInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Collaboration;
using UnityEditor.Hardware;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (EditorSettings))]
  internal class EditorSettingsInspector : ProjectSettingsBaseEditor
  {
    private EditorSettingsInspector.PopupElement[] vcDefaultPopupList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement(ExternalVersionControl.Disabled), new EditorSettingsInspector.PopupElement(ExternalVersionControl.Generic) };
    private EditorSettingsInspector.PopupElement[] vcPopupList = (EditorSettingsInspector.PopupElement[]) null;
    private EditorSettingsInspector.PopupElement[] serializationPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("Mixed"), new EditorSettingsInspector.PopupElement("Force Binary"), new EditorSettingsInspector.PopupElement("Force Text") };
    private EditorSettingsInspector.PopupElement[] behaviorPopupList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("3D"), new EditorSettingsInspector.PopupElement("2D") };
    private EditorSettingsInspector.PopupElement[] spritePackerPopupList = new EditorSettingsInspector.PopupElement[5]{ new EditorSettingsInspector.PopupElement("Disabled"), new EditorSettingsInspector.PopupElement("Enabled For Builds(Legacy Sprite Packer)"), new EditorSettingsInspector.PopupElement("Always Enabled(Legacy Sprite Packer)"), new EditorSettingsInspector.PopupElement("Enabled For Builds"), new EditorSettingsInspector.PopupElement("Always Enabled") };
    private EditorSettingsInspector.PopupElement[] lineEndingsPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("OS Native"), new EditorSettingsInspector.PopupElement("Unix"), new EditorSettingsInspector.PopupElement("Windows") };
    private EditorSettingsInspector.PopupElement[] spritePackerPaddingPowerPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("1"), new EditorSettingsInspector.PopupElement("2"), new EditorSettingsInspector.PopupElement("3") };
    private EditorSettingsInspector.PopupElement[] remoteCompressionList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("JPEG"), new EditorSettingsInspector.PopupElement("PNG") };
    private EditorSettingsInspector.PopupElement[] remoteResolutionList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("Downsize"), new EditorSettingsInspector.PopupElement("Normal") };
    private EditorSettingsInspector.PopupElement[] remoteJoystickSourceList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("Remote"), new EditorSettingsInspector.PopupElement("Local") };
    private string[] logLevelPopupList = new string[4]{ "Verbose", "Info", "Notice", "Fatal" };
    private string[] semanticMergePopupList = new string[3]{ "Off", "Premerge", "Ask" };
    private EditorSettingsInspector.PopupElement[] etcTextureCompressorPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("Legacy"), new EditorSettingsInspector.PopupElement("Default"), new EditorSettingsInspector.PopupElement("Custom") };
    private EditorSettingsInspector.PopupElement[] etcTextureFastCompressorPopupList = new EditorSettingsInspector.PopupElement[2]{ new EditorSettingsInspector.PopupElement("etcpak"), new EditorSettingsInspector.PopupElement("ETCPACK Fast") };
    private EditorSettingsInspector.PopupElement[] etcTextureNormalCompressorPopupList = new EditorSettingsInspector.PopupElement[4]{ new EditorSettingsInspector.PopupElement("etcpak"), new EditorSettingsInspector.PopupElement("ETCPACK Fast"), new EditorSettingsInspector.PopupElement("Etc2Comp Fast"), new EditorSettingsInspector.PopupElement("Etc2Comp Best") };
    private EditorSettingsInspector.PopupElement[] etcTextureBestCompressorPopupList = new EditorSettingsInspector.PopupElement[3]{ new EditorSettingsInspector.PopupElement("Etc2Comp Fast"), new EditorSettingsInspector.PopupElement("Etc2Comp Best"), new EditorSettingsInspector.PopupElement("ETCPACK Best") };
    private EditorSettingsInspector.PopupElement[] remoteDevicePopupList;
    private DevDevice[] remoteDeviceList;

    public void OnEnable()
    {
      Plugin[] availablePlugins = Plugin.availablePlugins;
      List<EditorSettingsInspector.PopupElement> popupElementList = new List<EditorSettingsInspector.PopupElement>((IEnumerable<EditorSettingsInspector.PopupElement>) this.vcDefaultPopupList);
      foreach (Plugin plugin in availablePlugins)
        popupElementList.Add(new EditorSettingsInspector.PopupElement(plugin.name, true));
      this.vcPopupList = popupElementList.ToArray();
      DevDeviceList.Changed += new DevDeviceList.OnChangedHandler(this.OnDeviceListChanged);
      this.BuildRemoteDeviceList();
    }

    public void OnDisable()
    {
      DevDeviceList.Changed -= new DevDeviceList.OnChangedHandler(this.OnDeviceListChanged);
    }

    private void OnDeviceListChanged()
    {
      this.BuildRemoteDeviceList();
    }

    private void BuildRemoteDeviceList()
    {
      List<DevDevice> devDeviceList = new List<DevDevice>();
      List<EditorSettingsInspector.PopupElement> popupElementList = new List<EditorSettingsInspector.PopupElement>();
      devDeviceList.Add(DevDevice.none);
      popupElementList.Add(new EditorSettingsInspector.PopupElement("None"));
      devDeviceList.Add(new DevDevice("Any Android Device", "Any Android Device", "virtual", "Android", DevDeviceState.Connected, DevDeviceFeatures.RemoteConnection));
      popupElementList.Add(new EditorSettingsInspector.PopupElement("Any Android Device"));
      foreach (DevDevice device in DevDeviceList.GetDevices())
      {
        bool flag = (device.features & DevDeviceFeatures.RemoteConnection) != DevDeviceFeatures.None;
        if (device.isConnected && flag)
        {
          devDeviceList.Add(device);
          popupElementList.Add(new EditorSettingsInspector.PopupElement(device.name));
        }
      }
      this.remoteDeviceList = devDeviceList.ToArray();
      this.remoteDevicePopupList = popupElementList.ToArray();
    }

    public override void OnInspectorGUI()
    {
      bool enabled = GUI.enabled;
      this.ShowUnityRemoteGUI(enabled);
      GUILayout.Space(10f);
      bool flag1 = Collab.instance.IsCollabEnabledForCurrentProject();
      using (new EditorGUI.DisabledScope(!flag1))
      {
        GUI.enabled = !flag1;
        GUILayout.Label("Version Control", EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.CreatePopupMenuVersionControl("Mode", this.vcPopupList, (string) (ExternalVersionControl) EditorSettings.externalVersionControl, new GenericMenu.MenuFunction2(this.SetVersionControlSystem));
        GUI.enabled = enabled && !flag1;
      }
      if (flag1)
        EditorGUILayout.HelpBox("Version Control not available when using Collaboration feature.", MessageType.Warning);
      if (this.VersionControlSystemHasGUI())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EditorSettingsInspector.\u003COnInspectorGUI\u003Ec__AnonStorey0 inspectorGuiCAnonStorey0 = new EditorSettingsInspector.\u003COnInspectorGUI\u003Ec__AnonStorey0();
        GUI.enabled = true;
        bool flag2 = false;
        if (!(EditorSettings.externalVersionControl == ExternalVersionControl.Generic) && !(EditorSettings.externalVersionControl == ExternalVersionControl.Disabled))
        {
          ConfigField[] activeConfigFields = Provider.GetActiveConfigFields();
          flag2 = true;
          foreach (ConfigField configField in activeConfigFields)
          {
            string configValue = EditorUserSettings.GetConfigValue(configField.name);
            string str;
            if (configField.isPassword)
            {
              str = EditorGUILayout.PasswordField(configField.label, configValue, new GUILayoutOption[0]);
              if (str != configValue)
                EditorUserSettings.SetPrivateConfigValue(configField.name, str);
            }
            else
            {
              str = EditorGUILayout.TextField(configField.label, configValue, new GUILayoutOption[0]);
              if (str != configValue)
                EditorUserSettings.SetConfigValue(configField.name, str);
            }
            if (configField.isRequired && string.IsNullOrEmpty(str))
              flag2 = false;
          }
        }
        // ISSUE: reference to a compiler-generated field
        inspectorGuiCAnonStorey0.logLevel = EditorUserSettings.GetConfigValue("vcSharedLogLevel");
        // ISSUE: reference to a compiler-generated method
        int selectedIndex = Array.FindIndex<string>(this.logLevelPopupList, new Predicate<string>(inspectorGuiCAnonStorey0.\u003C\u003Em__0));
        if (selectedIndex == -1)
        {
          // ISSUE: reference to a compiler-generated field
          inspectorGuiCAnonStorey0.logLevel = "notice";
          // ISSUE: reference to a compiler-generated method
          selectedIndex = Array.FindIndex<string>(this.logLevelPopupList, new Predicate<string>(inspectorGuiCAnonStorey0.\u003C\u003Em__1));
          if (selectedIndex == -1)
            selectedIndex = 0;
          // ISSUE: reference to a compiler-generated field
          inspectorGuiCAnonStorey0.logLevel = this.logLevelPopupList[selectedIndex];
          // ISSUE: reference to a compiler-generated field
          EditorUserSettings.SetConfigValue("vcSharedLogLevel", inspectorGuiCAnonStorey0.logLevel);
        }
        int index = EditorGUILayout.Popup("Log Level", selectedIndex, this.logLevelPopupList, new GUILayoutOption[0]);
        if (index != selectedIndex)
          EditorUserSettings.SetConfigValue("vcSharedLogLevel", this.logLevelPopupList[index].ToLower());
        GUI.enabled = enabled;
        string label2 = "Connected";
        switch (Provider.onlineState)
        {
          case OnlineState.Updating:
            label2 = "Connecting...";
            break;
          case OnlineState.Offline:
            label2 = "Disconnected";
            break;
        }
        EditorGUILayout.LabelField("Status", label2, new GUILayoutOption[0]);
        if (Provider.onlineState != OnlineState.Online && !string.IsNullOrEmpty(Provider.offlineReason))
        {
          GUI.enabled = false;
          GUILayout.TextArea(Provider.offlineReason);
          GUI.enabled = enabled;
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.enabled = flag2 && Provider.onlineState != OnlineState.Updating;
        if (GUILayout.Button("Connect", EditorStyles.miniButton, new GUILayoutOption[0]))
          Provider.UpdateSettings();
        GUILayout.EndHorizontal();
        EditorUserSettings.AutomaticAdd = EditorGUILayout.Toggle("Automatic add", EditorUserSettings.AutomaticAdd, new GUILayoutOption[0]);
        if (Provider.requiresNetwork)
        {
          bool flag3 = EditorGUILayout.Toggle("Work Offline", EditorUserSettings.WorkOffline, new GUILayoutOption[0]);
          if (flag3 != EditorUserSettings.WorkOffline)
          {
            if (flag3 && !EditorUtility.DisplayDialog("Confirm working offline", "Working offline and making changes to your assets means that you will have to manually integrate changes back into version control using your standard version control client before you stop working offline in Unity. Make sure you know what you are doing.", "Work offline", "Cancel"))
              flag3 = false;
            EditorUserSettings.WorkOffline = flag3;
            EditorApplication.RequestRepaintAllViews();
          }
          EditorUserSettings.allowAsyncStatusUpdate = EditorGUILayout.Toggle("Allow Async Update", EditorUserSettings.allowAsyncStatusUpdate, new GUILayoutOption[0]);
        }
        if (Provider.hasCheckoutSupport)
          EditorUserSettings.showFailedCheckout = EditorGUILayout.Toggle("Show failed checkouts", EditorUserSettings.showFailedCheckout, new GUILayoutOption[0]);
        GUI.enabled = enabled;
        EditorUserSettings.semanticMergeMode = (SemanticMergeMode) EditorGUILayout.Popup("Smart merge", (int) EditorUserSettings.semanticMergeMode, this.semanticMergePopupList, new GUILayoutOption[0]);
        this.DrawOverlayDescriptions();
      }
      GUILayout.Space(10f);
      int serializationMode = (int) EditorSettings.serializationMode;
      using (new EditorGUI.DisabledScope(!flag1))
      {
        GUI.enabled = !flag1;
        GUILayout.Label("Asset Serialization", EditorStyles.boldLabel, new GUILayoutOption[0]);
        GUI.enabled = enabled && !flag1;
        this.CreatePopupMenu("Mode", this.serializationPopupList, serializationMode, new GenericMenu.MenuFunction2(this.SetAssetSerializationMode));
      }
      if (flag1)
        EditorGUILayout.HelpBox("Asset Serialization is forced to Text when using Collaboration feature.", MessageType.Warning);
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUILayout.Label("Default Behavior Mode", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      this.CreatePopupMenu("Mode", this.behaviorPopupList, Mathf.Clamp((int) EditorSettings.defaultBehaviorMode, 0, this.behaviorPopupList.Length - 1), new GenericMenu.MenuFunction2(this.SetDefaultBehaviorMode));
      GUILayout.Space(10f);
      GUI.enabled = true;
      GUILayout.Label("Sprite Packer", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      this.CreatePopupMenu("Mode", this.spritePackerPopupList, Mathf.Clamp((int) EditorSettings.spritePackerMode, 0, this.spritePackerPopupList.Length - 1), new GenericMenu.MenuFunction2(this.SetSpritePackerMode));
      this.CreatePopupMenu("Padding Power (Legacy Sprite Packer)", this.spritePackerPaddingPowerPopupList, Mathf.Clamp(EditorSettings.spritePackerPaddingPower - 1, 0, 2), new GenericMenu.MenuFunction2(this.SetSpritePackerPaddingPower));
      this.DoProjectGenerationSettings();
      this.DoEtcTextureCompressionSettings();
      this.DoInternalSettings();
      this.DoLineEndingsSettings();
    }

    private void DoProjectGenerationSettings()
    {
      GUILayout.Space(10f);
      GUILayout.Label("C# Project Generation", EditorStyles.boldLabel, new GUILayoutOption[0]);
      string generationUserExtensions = EditorSettings.Internal_ProjectGenerationUserExtensions;
      string str1 = EditorGUILayout.TextField("Additional extensions to include", generationUserExtensions, new GUILayoutOption[0]);
      if (str1 != generationUserExtensions)
        EditorSettings.Internal_ProjectGenerationUserExtensions = str1;
      string generationRootNamespace = EditorSettings.projectGenerationRootNamespace;
      string str2 = EditorGUILayout.TextField("Root namespace", generationRootNamespace, new GUILayoutOption[0]);
      if (!(str2 != generationRootNamespace))
        return;
      EditorSettings.projectGenerationRootNamespace = str2;
    }

    private void DoInternalSettings()
    {
      if (!EditorPrefs.GetBool("InternalMode", false))
        return;
      GUILayout.Space(10f);
      GUILayout.Label("Internal Settings", EditorStyles.boldLabel, new GUILayoutOption[0]);
      string str = "-testable";
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle("Internals visible in user scripts", EditorSettings.Internal_UserGeneratedProjectSuffix == str, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        EditorSettings.Internal_UserGeneratedProjectSuffix = !flag ? "" : str;
      if (!flag)
        return;
      EditorGUILayout.HelpBox("If you want this to be set for other people, remember to manually add ProjectSettings/EditorSettings.asset to the repository", MessageType.Info);
    }

    private void DoEtcTextureCompressionSettings()
    {
      GUILayout.Space(10f);
      GUILayout.Label("ETC Texture Compressor", EditorStyles.boldLabel, new GUILayoutOption[0]);
      int selectedIndex = Mathf.Clamp(EditorSettings.etcTextureCompressorBehavior, 0, this.etcTextureCompressorPopupList.Length - 1);
      this.CreatePopupMenu("Behavior", this.etcTextureCompressorPopupList, selectedIndex, new GenericMenu.MenuFunction2(this.SetEtcTextureCompressorBehavior));
      ++EditorGUI.indentLevel;
      EditorGUI.BeginDisabledGroup(selectedIndex < 2);
      this.CreatePopupMenu("Fast", this.etcTextureFastCompressorPopupList, Mathf.Clamp(EditorSettings.etcTextureFastCompressor, 0, this.etcTextureFastCompressorPopupList.Length - 1), new GenericMenu.MenuFunction2(this.SetEtcTextureFastCompressor));
      this.CreatePopupMenu("Normal", this.etcTextureNormalCompressorPopupList, Mathf.Clamp(EditorSettings.etcTextureNormalCompressor, 0, this.etcTextureNormalCompressorPopupList.Length - 1), new GenericMenu.MenuFunction2(this.SetEtcTextureNormalCompressor));
      this.CreatePopupMenu("Best", this.etcTextureBestCompressorPopupList, Mathf.Clamp(EditorSettings.etcTextureBestCompressor, 0, this.etcTextureBestCompressorPopupList.Length - 1), new GenericMenu.MenuFunction2(this.SetEtcTextureBestCompressor));
      EditorGUI.EndDisabledGroup();
      --EditorGUI.indentLevel;
    }

    private void DoLineEndingsSettings()
    {
      GUILayout.Space(10f);
      GUILayout.Label("Line Endings For New Scripts", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.CreatePopupMenu("Mode", this.lineEndingsPopupList, (int) EditorSettings.lineEndingsForNewScripts, new GenericMenu.MenuFunction2(this.SetLineEndingsForNewScripts));
    }

    private static int GetIndexById(DevDevice[] elements, string id, int defaultIndex)
    {
      for (int index = 0; index < elements.Length; ++index)
      {
        if (elements[index].id == id)
          return index;
      }
      return defaultIndex;
    }

    private static int GetIndexById(EditorSettingsInspector.PopupElement[] elements, string id, int defaultIndex)
    {
      for (int index = 0; index < elements.Length; ++index)
      {
        if (elements[index].id == id)
          return index;
      }
      return defaultIndex;
    }

    private void ShowUnityRemoteGUI(bool editorEnabled)
    {
      GUI.enabled = true;
      GUILayout.Label("Unity Remote", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.enabled = editorEnabled;
      int indexById1 = EditorSettingsInspector.GetIndexById(this.remoteDeviceList, EditorSettings.unityRemoteDevice, 0);
      GUIContent content1 = new GUIContent(this.remoteDevicePopupList[indexById1].content);
      Rect rect1 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content1, EditorStyles.popup), 0, new GUIContent("Device"));
      if (EditorGUI.DropdownButton(rect1, content1, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect1, this.remoteDevicePopupList, indexById1, new GenericMenu.MenuFunction2(this.SetUnityRemoteDevice));
      int indexById2 = EditorSettingsInspector.GetIndexById(this.remoteCompressionList, EditorSettings.unityRemoteCompression, 0);
      GUIContent content2 = new GUIContent(this.remoteCompressionList[indexById2].content);
      Rect rect2 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content2, EditorStyles.popup), 0, new GUIContent("Compression"));
      if (EditorGUI.DropdownButton(rect2, content2, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect2, this.remoteCompressionList, indexById2, new GenericMenu.MenuFunction2(this.SetUnityRemoteCompression));
      int indexById3 = EditorSettingsInspector.GetIndexById(this.remoteResolutionList, EditorSettings.unityRemoteResolution, 0);
      GUIContent content3 = new GUIContent(this.remoteResolutionList[indexById3].content);
      Rect rect3 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content3, EditorStyles.popup), 0, new GUIContent("Resolution"));
      if (EditorGUI.DropdownButton(rect3, content3, FocusType.Passive, EditorStyles.popup))
        this.DoPopup(rect3, this.remoteResolutionList, indexById3, new GenericMenu.MenuFunction2(this.SetUnityRemoteResolution));
      int indexById4 = EditorSettingsInspector.GetIndexById(this.remoteJoystickSourceList, EditorSettings.unityRemoteJoystickSource, 0);
      GUIContent content4 = new GUIContent(this.remoteJoystickSourceList[indexById4].content);
      Rect rect4 = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content4, EditorStyles.popup), 0, new GUIContent("Joystick Source"));
      if (!EditorGUI.DropdownButton(rect4, content4, FocusType.Passive, EditorStyles.popup))
        return;
      this.DoPopup(rect4, this.remoteJoystickSourceList, indexById4, new GenericMenu.MenuFunction2(this.SetUnityRemoteJoystickSource));
    }

    private void DrawOverlayDescriptions()
    {
      if ((UnityEngine.Object) Provider.overlayAtlas == (UnityEngine.Object) null)
        return;
      GUILayout.Space(10f);
      GUILayout.Label("Overlay legends", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      this.DrawOverlayDescription(Asset.States.Local);
      this.DrawOverlayDescription(Asset.States.OutOfSync);
      this.DrawOverlayDescription(Asset.States.CheckedOutLocal);
      this.DrawOverlayDescription(Asset.States.CheckedOutRemote);
      this.DrawOverlayDescription(Asset.States.DeletedLocal);
      this.DrawOverlayDescription(Asset.States.DeletedRemote);
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      this.DrawOverlayDescription(Asset.States.AddedLocal);
      this.DrawOverlayDescription(Asset.States.AddedRemote);
      this.DrawOverlayDescription(Asset.States.Conflicted);
      this.DrawOverlayDescription(Asset.States.LockedLocal);
      this.DrawOverlayDescription(Asset.States.LockedRemote);
      this.DrawOverlayDescription(Asset.States.Updating);
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private void DrawOverlayDescription(Asset.States state)
    {
      Rect atlasRectForState = Provider.GetAtlasRectForState((int) state);
      if ((double) atlasRectForState.width == 0.0)
        return;
      Texture2D overlayAtlas = Provider.overlayAtlas;
      if ((UnityEngine.Object) overlayAtlas == (UnityEngine.Object) null)
        return;
      GUILayout.Label("    " + Asset.StateToString(state), EditorStyles.miniLabel, new GUILayoutOption[0]);
      Rect lastRect = GUILayoutUtility.GetLastRect();
      lastRect.width = 16f;
      GUI.DrawTextureWithTexCoords(lastRect, (Texture) overlayAtlas, atlasRectForState);
    }

    private void CreatePopupMenuVersionControl(string title, EditorSettingsInspector.PopupElement[] elements, string selectedValue, GenericMenu.MenuFunction2 func)
    {
      int index = Array.FindIndex<EditorSettingsInspector.PopupElement>(elements, (Predicate<EditorSettingsInspector.PopupElement>) (typeElem => typeElem.id == selectedValue));
      GUIContent content = new GUIContent(elements[index].content);
      this.CreatePopupMenu(title, content, elements, index, func);
    }

    private void CreatePopupMenu(string title, EditorSettingsInspector.PopupElement[] elements, int selectedIndex, GenericMenu.MenuFunction2 func)
    {
      this.CreatePopupMenu(title, elements[selectedIndex].content, elements, selectedIndex, func);
    }

    private void CreatePopupMenu(string title, GUIContent content, EditorSettingsInspector.PopupElement[] elements, int selectedIndex, GenericMenu.MenuFunction2 func)
    {
      Rect rect = EditorGUI.PrefixLabel(GUILayoutUtility.GetRect(content, EditorStyles.popup), 0, new GUIContent(title));
      if (!EditorGUI.DropdownButton(rect, content, FocusType.Passive, EditorStyles.popup))
        return;
      this.DoPopup(rect, elements, selectedIndex, func);
    }

    private void DoPopup(Rect popupRect, EditorSettingsInspector.PopupElement[] elements, int selectedIndex, GenericMenu.MenuFunction2 func)
    {
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < elements.Length; ++index)
      {
        EditorSettingsInspector.PopupElement element = elements[index];
        if (element.Enabled)
          genericMenu.AddItem(element.content, index == selectedIndex, func, (object) index);
        else
          genericMenu.AddDisabledItem(element.content);
      }
      genericMenu.DropDown(popupRect);
    }

    private bool VersionControlSystemHasGUI()
    {
      if (Collab.instance.IsCollabEnabledForCurrentProject())
        return false;
      ExternalVersionControl externalVersionControl = (ExternalVersionControl) EditorSettings.externalVersionControl;
      return (string) externalVersionControl != ExternalVersionControl.Disabled && (string) externalVersionControl != ExternalVersionControl.AutoDetect && (string) externalVersionControl != ExternalVersionControl.Generic;
    }

    private void SetVersionControlSystem(object data)
    {
      int index = (int) data;
      if (index < 0 && index >= this.vcPopupList.Length)
        return;
      EditorSettingsInspector.PopupElement vcPopup = this.vcPopupList[index];
      string externalVersionControl = EditorSettings.externalVersionControl;
      EditorSettings.externalVersionControl = vcPopup.id;
      Provider.UpdateSettings();
      AssetDatabase.Refresh();
      if (!(externalVersionControl != vcPopup.id) || !(vcPopup.content.text == ExternalVersionControl.Disabled) && !(vcPopup.content.text == ExternalVersionControl.Generic))
        return;
      WindowPending.CloseAllWindows();
    }

    private void SetAssetSerializationMode(object data)
    {
      EditorSettings.serializationMode = (SerializationMode) data;
    }

    private void SetUnityRemoteDevice(object data)
    {
      EditorSettings.unityRemoteDevice = this.remoteDeviceList[(int) data].id;
    }

    private void SetUnityRemoteCompression(object data)
    {
      EditorSettings.unityRemoteCompression = this.remoteCompressionList[(int) data].id;
    }

    private void SetUnityRemoteResolution(object data)
    {
      EditorSettings.unityRemoteResolution = this.remoteResolutionList[(int) data].id;
    }

    private void SetUnityRemoteJoystickSource(object data)
    {
      EditorSettings.unityRemoteJoystickSource = this.remoteJoystickSourceList[(int) data].id;
    }

    private void SetDefaultBehaviorMode(object data)
    {
      EditorSettings.defaultBehaviorMode = (EditorBehaviorMode) data;
    }

    private void SetSpritePackerMode(object data)
    {
      EditorSettings.spritePackerMode = (SpritePackerMode) data;
    }

    private void SetSpritePackerPaddingPower(object data)
    {
      EditorSettings.spritePackerPaddingPower = (int) data + 1;
    }

    private void SetEtcTextureCompressorBehavior(object data)
    {
      int num = (int) data;
      if (EditorSettings.etcTextureCompressorBehavior == num)
        return;
      EditorSettings.etcTextureCompressorBehavior = num;
      if (num == 0)
        EditorSettings.SetEtcTextureCompressorLegacyBehavior();
      else
        EditorSettings.SetEtcTextureCompressorDefaultBehavior();
    }

    private void SetEtcTextureFastCompressor(object data)
    {
      EditorSettings.etcTextureFastCompressor = (int) data;
    }

    private void SetEtcTextureNormalCompressor(object data)
    {
      EditorSettings.etcTextureNormalCompressor = (int) data;
    }

    private void SetEtcTextureBestCompressor(object data)
    {
      EditorSettings.etcTextureBestCompressor = (int) data;
    }

    private void SetLineEndingsForNewScripts(object data)
    {
      EditorSettings.lineEndingsForNewScripts = (LineEndingsMode) data;
    }

    private struct PopupElement
    {
      public readonly string id;
      public readonly bool requiresTeamLicense;
      public readonly GUIContent content;

      public PopupElement(string content)
      {
        this = new EditorSettingsInspector.PopupElement(content, false);
      }

      public PopupElement(string content, bool requiresTeamLicense)
      {
        this.id = content;
        this.content = new GUIContent(content);
        this.requiresTeamLicense = requiresTeamLicense;
      }

      public bool Enabled
      {
        get
        {
          return !this.requiresTeamLicense || InternalEditorUtility.HasTeamLicense();
        }
      }
    }
  }
}
