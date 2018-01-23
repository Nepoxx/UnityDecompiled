// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProvisioningProfileGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProvisioningProfileGUI
  {
    internal static void ShowProvisioningProfileUIWithProperty(GUIContent titleWithToolTip, ProvisioningProfile profile, SerializedProperty prop)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(titleWithToolTip, EditorStyles.label, new GUILayoutOption[0]);
      EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(false, 0.0f, new GUILayoutOption[0]), EditorGUIUtility.TextContent("Profile ID:"), prop);
      if (GUILayout.Button("Browse", EditorStyles.miniButton, new GUILayoutOption[0]))
      {
        ProvisioningProfile provisioningProfile = ProvisioningProfileGUI.Browse("");
        if (provisioningProfile != null && !string.IsNullOrEmpty(provisioningProfile.UUID))
        {
          profile = provisioningProfile;
          prop.stringValue = profile.UUID;
          prop.serializedObject.ApplyModifiedProperties();
          GUI.FocusControl("");
        }
        GUIUtility.ExitGUI();
      }
      EditorGUI.EndProperty();
      GUILayout.EndHorizontal();
      EditorGUI.BeginChangeCheck();
      ++EditorGUI.indentLevel;
      Rect controlRect = EditorGUILayout.GetControlRect(true, 0.0f, new GUILayoutOption[0]);
      GUIContent label = EditorGUIUtility.TextContent("Profile ID:");
      EditorGUI.BeginProperty(controlRect, label, prop);
      profile.UUID = EditorGUILayout.TextField(label, profile.UUID, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        prop.stringValue = profile.UUID;
      EditorGUI.EndProperty();
      --EditorGUI.indentLevel;
    }

    internal static void ShowProvisioningProfileUIWithCallback(GUIContent titleWithToolTip, ProvisioningProfile profile, ProvisioningProfileGUI.ProvisioningProfileChangedDelegate callback)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(titleWithToolTip, EditorStyles.label, new GUILayoutOption[0]);
      if (GUILayout.Button("Browse", EditorStyles.miniButton, new GUILayoutOption[0]))
      {
        ProvisioningProfile provisioningProfile = ProvisioningProfileGUI.Browse("");
        if (provisioningProfile != null && !string.IsNullOrEmpty(provisioningProfile.UUID))
        {
          profile = provisioningProfile;
          callback(profile);
          GUI.FocusControl("");
        }
        GUIUtility.ExitGUI();
      }
      GUILayout.EndHorizontal();
      EditorGUI.BeginChangeCheck();
      ++EditorGUI.indentLevel;
      GUIContent label = EditorGUIUtility.TextContent("Profile ID:");
      profile.UUID = EditorGUILayout.TextField(label, profile.UUID, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
      if (!EditorGUI.EndChangeCheck())
        return;
      callback(profile);
    }

    internal static ProvisioningProfile Browse(string path)
    {
      string title = "Select the Provising Profile used for Manual Signing";
      string directory = path;
      if (InternalEditorUtility.inBatchMode)
        return (ProvisioningProfile) null;
      ProvisioningProfile provisioningProfile = (ProvisioningProfile) null;
      do
      {
        path = EditorUtility.OpenFilePanel(title, directory, "mobileprovision");
        if (path.Length == 0)
          return (ProvisioningProfile) null;
      }
      while (!ProvisioningProfileGUI.GetProvisioningProfileId(path, out provisioningProfile));
      return provisioningProfile;
    }

    internal static bool GetProvisioningProfileId(string filePath, out ProvisioningProfile provisioningProfile)
    {
      ProvisioningProfile provisioningProfileAtPath = ProvisioningProfile.ParseProvisioningProfileAtPath(filePath);
      provisioningProfile = provisioningProfileAtPath;
      return provisioningProfileAtPath.UUID != null;
    }

    internal static void ShowUIWithDefaults(string provisioningPrefKey, SerializedProperty enableAutomaticSigningProp, GUIContent automaticSigningGUI, SerializedProperty manualSigningIDProp, GUIContent manualSigningProfileGUI, SerializedProperty appleDevIDProp, GUIContent teamIDGUIContent)
    {
      bool automaticSigningValue = ProvisioningProfileGUI.GetBoolForAutomaticSigningValue(ProvisioningProfileGUI.GetDefaultAutomaticSigningValue(enableAutomaticSigningProp, iOSEditorPrefKeys.kDefaultiOSAutomaticallySignBuild));
      EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true, 0.0f, new GUILayoutOption[0]), automaticSigningGUI, enableAutomaticSigningProp);
      bool automaticallySign = EditorGUILayout.Toggle(automaticSigningGUI, automaticSigningValue, new GUILayoutOption[0]);
      if (automaticallySign != automaticSigningValue)
        enableAutomaticSigningProp.intValue = ProvisioningProfileGUI.GetIntValueForAutomaticSigningBool(automaticallySign);
      EditorGUI.EndProperty();
      if (!automaticallySign)
      {
        ProvisioningProfileGUI.ShowProvisioningProfileUIWithDefaults(provisioningPrefKey, manualSigningIDProp, manualSigningProfileGUI);
      }
      else
      {
        string defaultStringValue = ProvisioningProfileGUI.GetDefaultStringValue(appleDevIDProp, iOSEditorPrefKeys.kDefaultiOSAutomaticSignTeamId);
        EditorGUI.BeginProperty(EditorGUILayout.GetControlRect(true, 0.0f, new GUILayoutOption[0]), teamIDGUIContent, appleDevIDProp);
        EditorGUI.BeginChangeCheck();
        string str = EditorGUILayout.TextField(teamIDGUIContent, defaultStringValue, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          appleDevIDProp.stringValue = str;
        EditorGUI.EndProperty();
      }
    }

    private static void ShowProvisioningProfileUIWithDefaults(string defaultPreferenceKey, SerializedProperty uuidProp, GUIContent title)
    {
      string stringValue = uuidProp.stringValue;
      if (string.IsNullOrEmpty(stringValue))
        stringValue = EditorPrefs.GetString(defaultPreferenceKey);
      ProvisioningProfileGUI.ShowProvisioningProfileUIWithProperty(title, new ProvisioningProfile(stringValue), uuidProp);
    }

    private static bool GetBoolForAutomaticSigningValue(int signingValue)
    {
      return signingValue == 1;
    }

    private static int GetIntValueForAutomaticSigningBool(bool automaticallySign)
    {
      return !automaticallySign ? 2 : 1;
    }

    private static int GetDefaultAutomaticSigningValue(SerializedProperty prop, string editorPropKey)
    {
      int num = prop.intValue;
      if (num == 0)
        num = !EditorPrefs.GetBool(editorPropKey, true) ? 2 : 1;
      return num;
    }

    private static string GetDefaultStringValue(SerializedProperty prop, string editorPrefKey)
    {
      string stringValue = prop.stringValue;
      if (string.IsNullOrEmpty(stringValue))
        stringValue = EditorPrefs.GetString(editorPrefKey, "");
      return stringValue;
    }

    internal delegate void ProvisioningProfileChangedDelegate(ProvisioningProfile profile);
  }
}
