// Decompiled with JetBrains decompiler
// Type: UnityEditor.AboutWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor.Modules;
using UnityEditor.VisualStudioIntegration;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AboutWindow : EditorWindow
  {
    private float m_TextYPos = 120f;
    private float m_TextInitialYPos = 120f;
    private float m_TotalCreditsHeight = float.PositiveInfinity;
    private double m_LastScrollUpdate = 0.0;
    private bool m_ShowDetailedVersion = false;
    private static GUIContent s_MonoLogo;
    private static GUIContent s_AgeiaLogo;
    private static GUIContent s_Header;
    private const string kSpecialThanksNames = "Thanks to Forest 'Yoggy' Johnson, Graham McAllister, David Janik-Jones, Raimund Schumacher, Alan J. Dickins and Emil 'Humus' Persson";
    private int m_InternalCodeProgress;

    private static void ShowAboutWindow()
    {
      AboutWindow windowWithRect = EditorWindow.GetWindowWithRect<AboutWindow>(new Rect(100f, 100f, 570f, 340f), true, "About Unity");
      windowWithRect.position = new Rect(100f, 100f, 570f, 340f);
      windowWithRect.m_Parent.window.m_DontSaveToLayout = true;
      AboutWindowNames.ParseCredits();
    }

    private static void LoadLogos()
    {
      if (AboutWindow.s_MonoLogo != null)
        return;
      AboutWindow.s_MonoLogo = EditorGUIUtility.IconContent("MonoLogo");
      AboutWindow.s_AgeiaLogo = EditorGUIUtility.IconContent("AgeiaLogo");
      AboutWindow.s_Header = EditorGUIUtility.IconContent("AboutWindow.MainHeader");
    }

    public void OnEnable()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.UpdateScroll);
      this.m_LastScrollUpdate = EditorApplication.timeSinceStartup;
    }

    public void OnDisable()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateScroll);
    }

    public void UpdateScroll()
    {
      double num = EditorApplication.timeSinceStartup - this.m_LastScrollUpdate;
      this.m_LastScrollUpdate = EditorApplication.timeSinceStartup;
      if (GUIUtility.hotControl != 0)
        return;
      this.m_TextYPos -= 40f * (float) num;
      if ((double) this.m_TextYPos < -(double) this.m_TotalCreditsHeight)
        this.m_TextYPos = this.m_TextInitialYPos;
      this.Repaint();
    }

    public void OnGUI()
    {
      AboutWindow.LoadLogos();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.Label(AboutWindow.s_Header, GUIStyle.none, new GUILayoutOption[0]);
      this.ListenForSecretCodes();
      string str1 = "";
      if (InternalEditorUtility.HasFreeLicense())
        str1 = " Personal";
      if (InternalEditorUtility.HasEduLicense())
        str1 = " Edu";
      GUILayout.BeginHorizontal();
      GUILayout.Space(52f);
      string str2 = this.FormatExtensionVersionString();
      this.m_ShowDetailedVersion |= Event.current.alt;
      if (this.m_ShowDetailedVersion)
      {
        int unityVersionDate = InternalEditorUtility.GetUnityVersionDate();
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        string unityBuildBranch = InternalEditorUtility.GetUnityBuildBranch();
        string str3 = "";
        if (unityBuildBranch.Length > 0)
          str3 = "Branch: " + unityBuildBranch;
        EditorGUILayout.SelectableLabel(string.Format("Version {0}{1}{2}\n{3:r}\n{4}", (object) InternalEditorUtility.GetFullUnityVersion(), (object) str1, (object) str2, (object) dateTime.AddSeconds((double) unityVersionDate), (object) str3), new GUILayoutOption[2]
        {
          GUILayout.Width(550f),
          GUILayout.Height(42f)
        });
        this.m_TextInitialYPos = 108f;
      }
      else
        GUILayout.Label(string.Format("Version {0}{1}{2}", (object) Application.unityVersion, (object) str1, (object) str2));
      if (Event.current.type == EventType.ValidateCommand)
        return;
      GUILayout.EndHorizontal();
      GUILayout.Space(4f);
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      float creditsWidth = this.position.width - 10f;
      float creditsChunkYOffset = this.m_TextYPos;
      Rect rect = GUILayoutUtility.GetRect(10f, this.m_TextInitialYPos);
      GUI.BeginGroup(rect);
      foreach (string name in AboutWindowNames.Names((string) null, true))
        creditsChunkYOffset = AboutWindow.DoCreditsNameChunk(name, creditsWidth, creditsChunkYOffset);
      this.m_TotalCreditsHeight = AboutWindow.DoCreditsNameChunk("Thanks to Forest 'Yoggy' Johnson, Graham McAllister, David Janik-Jones, Raimund Schumacher, Alan J. Dickins and Emil 'Humus' Persson", creditsWidth, creditsChunkYOffset) - this.m_TextYPos;
      GUI.EndGroup();
      this.HandleScrollEvents(rect);
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.Label(AboutWindow.s_MonoLogo);
      GUILayout.Label("Scripting powered by The Mono Project.\n\n(c) 2011 Novell, Inc.", (GUIStyle) "MiniLabel", new GUILayoutOption[1]
      {
        GUILayout.Width(200f)
      });
      GUILayout.Label(AboutWindow.s_AgeiaLogo);
      GUILayout.Label("Physics powered by PhysX.\n\n(c) 2011 NVIDIA Corporation.", (GUIStyle) "MiniLabel", new GUILayoutOption[1]
      {
        GUILayout.Width(200f)
      });
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.Space(5f);
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      string aboutWindowLabel = UnityVSSupport.GetAboutWindowLabel();
      if (aboutWindowLabel.Length > 0)
        GUILayout.Label(aboutWindowLabel, (GUIStyle) "MiniLabel", new GUILayoutOption[0]);
      GUILayout.Label(InternalEditorUtility.GetUnityCopyright(), (GUIStyle) "MiniLabel", new GUILayoutOption[0]);
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.Label(InternalEditorUtility.GetLicenseInfo(), (GUIStyle) "AboutWindowLicenseLabel", new GUILayoutOption[0]);
      GUILayout.EndVertical();
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
    }

    private void HandleScrollEvents(Rect scrollAreaRect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      switch (Event.current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (!scrollAreaRect.Contains(Event.current.mousePosition))
            break;
          GUIUtility.hotControl = controlId;
          Event.current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          Event.current.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId)
            break;
          this.m_TextYPos += Event.current.delta.y;
          this.m_TextYPos = Mathf.Min(this.m_TextYPos, this.m_TextInitialYPos);
          this.m_TextYPos = Mathf.Max(this.m_TextYPos, -this.m_TotalCreditsHeight);
          Event.current.Use();
          break;
      }
    }

    private static float DoCreditsNameChunk(string nameChunk, float creditsWidth, float creditsChunkYOffset)
    {
      float height = EditorStyles.wordWrappedLabel.CalcHeight(GUIContent.Temp(nameChunk), creditsWidth);
      Rect position = new Rect(5f, creditsChunkYOffset, creditsWidth, height);
      GUI.Label(position, nameChunk, EditorStyles.wordWrappedLabel);
      return position.yMax;
    }

    private void ListenForSecretCodes()
    {
      if (Event.current.type != EventType.KeyDown || (int) Event.current.character == 0 || !this.SecretCodeHasBeenTyped("internal", ref this.m_InternalCodeProgress))
        return;
      bool flag = !EditorPrefs.GetBool("InternalMode", false);
      EditorPrefs.SetBool("InternalMode", flag);
      this.ShowNotification(new GUIContent("Internal Mode " + (!flag ? "Off" : "On")));
      InternalEditorUtility.RequestScriptReload();
    }

    private bool SecretCodeHasBeenTyped(string code, ref int characterProgress)
    {
      if (characterProgress < 0 || characterProgress >= code.Length || (int) code[characterProgress] != (int) Event.current.character)
        characterProgress = 0;
      if ((int) code[characterProgress] == (int) Event.current.character)
      {
        ++characterProgress;
        if (characterProgress >= code.Length)
        {
          characterProgress = 0;
          return true;
        }
      }
      return false;
    }

    private string FormatExtensionVersionString()
    {
      string target = EditorUserBuildSettings.selectedBuildTargetGroup.ToString();
      string extensionVersion = ModuleManager.GetExtensionVersion(target);
      if (string.IsNullOrEmpty(extensionVersion))
        return "";
      return " [" + target + ": " + extensionVersion + "]";
    }
  }
}
